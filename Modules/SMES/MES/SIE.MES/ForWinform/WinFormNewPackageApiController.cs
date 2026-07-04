using SIE.Api;
using SIE.Barcodes;
using SIE.Common.Configs;
using SIE.Common.Sort;
using SIE.Core.Barcodes;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.ForWinform.ApiModels;
using SIE.MES.PackingPrints;
using SIE.MES.WIP;
using SIE.MES.WIP.NewPackages;
using SIE.MES.WIP.Products;
using SIE.MES.WorkOrders;
using SIE.Packages;
using SIE.Packages.ItemLabels;
using SIE.Packages.Packings;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using SIE.Tech.Stations.Configs;
using SIE.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIE.MES.ForWinform
{

    /// <summary>
    /// 新包装采集API
    /// </summary>
    public class WinFormNewPackageApiApiController : NewPackageController
    {
        /// <summary>
        /// 根据Workcell获取当前工单的相关信息
        /// </summary>
        /// <param name="employeeId">操作人ID</param>
        /// <param name="processId">工序ID</param>
        /// <param name="stationId">工位ID</param>
        /// <param name="resourceId">资源ID</param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        [ApiService("根据Workcell获取当前工单的相关信息")]
        [return: ApiReturn("根据Workcell获取当前工单的相关信息 GetCurrentInfo")]
        public virtual XPApiResultPackageInfo GetCurrentInfo([ApiParameter("操作人ID")] double employeeId,
            [ApiParameter("工序ID")] double processId, [ApiParameter("工位ID")] double stationId,
            [ApiParameter("资源ID")] double resourceId)
        {
            Process process = Query<Process>().Where(P => P.Id == processId).FirstOrDefault(new EagerLoadOptions());
            if (process == null)
                throw new ValidationException("获取工序失败(工序ID{0}不存在)".L10nFormat(processId));

            XPApiResultPackageInfo result = new XPApiResultPackageInfo();
            result.IsChangeOrder = false;

            WIP.Workcell workcell = new WIP.Workcell
            {
                EmployeeId = employeeId,
                ProcessId = processId,
                StationId = stationId,
                ResourceId = resourceId
            };

            //工单信息
            var wipLineWorkOrder = Query<WipResourceWorkOrder>()
                .Where(f => f.ResourceId == workcell.ResourceId && f.ProcessId == workcell.ProcessId && f.StationId == workcell.StationId)
                .FirstOrDefault();

            if (wipLineWorkOrder != null)
            {
                var workOrder = RF.GetById<WorkOrder>(wipLineWorkOrder.WorkOrderId, new EagerLoadOptions().LoadWithViewProperty());
                if (workOrder != null)
                {
                    result.WorkOrder = new WorkOrderInfo(workOrder);
                }
            }

            //条码明细
            EntityList<PackageSnRecord> PackageSnRecordList = RT.Service.Resolve<NewPackageController>().GetPackageSnRecords(resourceId, processId, stationId);
            if (PackageSnRecordList != null && PackageSnRecordList.Count > 0)
            {
                result.PackageSnRecords = new List<XPPackageSnRecord>();
                foreach (var packageRecord in PackageSnRecordList)
                {
                    result.PackageSnRecords.Add(new XPPackageSnRecord(packageRecord));
                }
            }

            //包装规则
            if (result.WorkOrder != null)
            {
                var workOrderRule = base.GetWorkOrderRule(result.WorkOrder.Id);
                if (workOrderRule != null)
                {
                    result.PackageRules = new List<XPWorkOrderPackageRuleDetail>();
                    foreach (var item in workOrderRule)
                    {
                        result.PackageRules.Add(new XPWorkOrderPackageRuleDetail(item));
                    }
                }
            }

            //包装关系
            var packageRelations = RT.Service.Resolve<PackingRelationController>().GetAllPackingRelations(PackageSnRecordList.Where(p => p.WoSn != "").Select(p => p.Sn).Distinct().ToList(), true);

            result.AllRelations = new List<XPPackingRelation>();
            var itemLabels = GetItemLabelsByPackageSnRecordList(PackageSnRecordList);
            foreach (var item in packageRelations)
                result.AllRelations.Add(new XPPackingRelation(item, itemLabels));


            result.Step = new XPReworkStep();
            result.Step.SetProcess(process);

            //打印模式
            Station station = RF.GetById<Station>(stationId);
            if (station == null)
                throw new ValidationException("获取工位失败(工位ID{0}不存在)".L10nFormat(stationId));
            var config = ConfigService.GetConfig(new NewPackingPrintModeConfig(), typeof(Station), station);
            result.PrintMode = config == null ? PrintMode.Online : config.PrintMode;

            return result;
        }

        /// <summary>
        /// 验证是否需要提前输入
        /// </summary>
        /// <param name="packageNo">包装号</param>
        /// <param name="ruleUnitId">包装单位ID</param>
        /// <param name="ruleUnitName">包装单位名称</param>
        /// <param name="workOrderId">工单ID</param>
        [ApiService("验证是否需要提前输入")]
        [return: ApiReturn("验证是否需要提前输入 AdvanceInputPackageNo")]
        public virtual bool AdvanceInputPackageNo([ApiParameter("包装号")] string packageNo, [ApiParameter("包装单位ID")] double ruleUnitId,
            [ApiParameter("包装单位名称")] string ruleUnitName, [ApiParameter("工单ID")] double workOrderId)
        {
            if (string.IsNullOrEmpty(packageNo))
            {
                throw new ValidationException("包装号不能为空".L10N());
            }
            var packingBarcode = RT.Service.Resolve<PackingBarcodeController>().GetPackingBarcode(packageNo) ?? throw new ValidationException("包装号[{0}]不存在".L10nFormat(packageNo));
            if (packingBarcode.IsUse)
            {
                throw new ValidationException("包装号[{0}]已使用".L10nFormat(packageNo));
            }
            if (packingBarcode.PackageUnitId != ruleUnitId)
            {
                throw new ValidationException("包装号【{0}】包装单位是【{1}】与要扫描的包装单位不相符，请扫描【{2}】的包装号"
                    .L10nFormat(packageNo, packingBarcode.PackageUnitName, ruleUnitName));
            }
            if (packingBarcode.IsUse)
            {
                throw new ValidationException("包装号[{0}]已使用".L10nFormat(packageNo));
            }
            if (packingBarcode.WorkOrderId != workOrderId)
            {
                throw new ValidationException("包装号[{0}]的工单与当前正在包装的工单不相同".L10nFormat(packageNo));
            }

            return true;
        }

        /// <summary>
        /// 验证输入条码，预算包装层级
        /// </summary>
        /// <param name="barcode">条码号</param>
        /// <param name="barcodeType">条码过站类型</param>
        /// <param name="employeeId">操作人ID</param>
        /// <param name="processId">工序ID</param>
        /// <param name="stationId">工位ID</param>
        /// <param name="resourceId">资源ID</param>
        /// <param name="oldWorkOrderId">原来的工单ID</param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        [ApiService("验证输入条码，预算包装层级")]
        [return: ApiReturn("验证输入条码，预算包装层级 AdvanceInputBarcode")]
        public virtual XPApiResultPackageInfo AdvanceInputBarcode([ApiParameter("条码号")] string barcode, [ApiParameter("条码过站类型")] int barcodeType,
            [ApiParameter("操作人ID")] double employeeId, [ApiParameter("工序ID")] double processId, [ApiParameter("工位ID")] double stationId,
            [ApiParameter("资源ID")] double resourceId, [ApiParameter("原来的工单ID")] double oldWorkOrderId)
        {
            XPApiResultPackageInfo result = new XPApiResultPackageInfo();

            var collectBarcode = new CollectBarcode { Code = barcode, Type = (BarcodeType)barcodeType };

            WIP.Workcell workcell = new WIP.Workcell
            {
                EmployeeId = employeeId,
                ProcessId = processId,
                StationId = stationId,
                ResourceId = resourceId
            };

            base.Validate(collectBarcode, workcell);
            if (barcode.IsNullOrEmpty())
            {
                throw new ValidationException("条码不能为空！".L10N());
            }
            var barcodeEntity = RT.Service.Resolve<BarcodeController>().GetBarcode(barcode) ?? throw new ValidationException("条码不存在！".L10N());
            var woId = barcodeEntity.WorkOrderId ?? throw new ValidationException("条码没有归属工单！".L10N());

            if (woId != oldWorkOrderId)
            {
                //新的工单
                result.IsChangeOrder = true;
                var workOrder = RF.GetById<WorkOrder>(woId, new EagerLoadOptions().LoadWithViewProperty());
                result.WorkOrder = new ApiModels.WorkOrderInfo(workOrder);
            }

            var workOrderRule = base.GetWorkOrderRule(woId) ?? throw new ValidationException("工单没有包装规则！".L10N());

            if (result.IsChangeOrder)
            {
                //包装规则
                result.PackageRules = new List<XPWorkOrderPackageRuleDetail>();
                foreach (var item in workOrderRule)
                {
                    result.PackageRules.Add(new XPWorkOrderPackageRuleDetail(item));
                }

                //条码明细
                EntityList<PackageSnRecord> PackageSnRecordList = RT.Service.Resolve<NewPackageController>().GetPackageSnRecords(resourceId, processId, stationId);
                if (PackageSnRecordList != null && PackageSnRecordList.Count > 0)
                {
                    result.PackageSnRecords = new List<XPPackageSnRecord>();
                    foreach (var packageRecord in PackageSnRecordList)
                        result.PackageSnRecords.Add(new XPPackageSnRecord(packageRecord));
                }

                //刷新包装关系
                var packageRelations = RT.Service.Resolve<PackingRelationController>().GetAllPackingRelations(PackageSnRecordList.Where(p => p.WoSn != "").Select(p => p.Sn).Distinct().ToList(), true);
                result.AllRelations = new List<XPPackingRelation>();
                var itemLabels = GetItemLabelsByPackageSnRecordList(PackageSnRecordList);
                foreach (var item in packageRelations)
                    result.AllRelations.Add(new XPPackingRelation(item, itemLabels));
            }

            if (workOrderRule.Length <= 1)
            {
                throw new ValidationException("工单包装规则至少要维护2层！".L10N());
            }
            var masterUnit = workOrderRule.FirstOrDefault();
            if (masterUnit == null || !masterUnit.PackageUnit.IsMasterUnit)
            {
                throw new ValidationException("请确保工单主单位已经维护并且是第一个！".L10N());
            }
            result.CurrentBarcode = barcode;
            var productId = base.GetWorkOrderProductId(woId);
            var records = base.GetPackageSnRecords(workcell.ResourceId, workcell.ProcessId, workcell.StationId, woId, productId);
            var record = base.GeneragePackageSnRecord(barcode, masterUnit.PackageUnitId, woId, productId, "", workcell.ResourceId, workcell.ProcessId, workcell.StationId, 1, 1);
            records.Add(record);

            // 预计算
            for (int i = 1; i < workOrderRule.Length; i++)
            {
                //上层包装规则
                var upperRule = workOrderRule[i - 1];
                //当前包装规则
                var currentRule = workOrderRule[i];
                //上层的所有数据
                var allRecords = records.Where(p => p.PackageUnitId == upperRule.PackageUnitId && p.PersistenceStatus != PersistenceStatus.Deleted).OrderBy(p => p.CreateDate).ToList();
                bool isLastRult = (i == workOrderRule.Length - 1);
                while (allRecords.Count >= currentRule.LevelQty)
                {
                    var curRecords = allRecords.Take(Convert.ToInt32(currentRule.LevelQty)).ToList();

                    //生成包装
                    if (!isLastRult)
                    {
                        var parentRecord = base.GeneragePackageSnRecord("", currentRule.PackageUnitId, woId, productId, "", workcell.ResourceId, workcell.ProcessId, workcell.StationId, curRecords.Count, curRecords.Sum(p => p.ItemQty));
                        records.Add(parentRecord);
                    }

                    // 生成新的包装
                    if (result.AdvancePackingUnits == null)
                        result.AdvancePackingUnits = new List<XPPackingUnit>();
                    result.AdvancePackingUnits.Add(new XPPackingUnit(currentRule.PackageUnit));

                    curRecords.ForEach(p => p.PersistenceStatus = PersistenceStatus.Deleted);

                    allRecords = records.Where(p => p.PackageUnitId == upperRule.PackageUnitId && p.PersistenceStatus != PersistenceStatus.Deleted).OrderBy(p => p.CreateDate).ToList();
                }
            }

            return result;
        }


        /// <summary>
        /// 包装采集
        /// </summary>
        /// <param name="barcode">条码号</param>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="employeeId">操作人ID</param>
        /// <param name="processId">工序ID</param>
        /// <param name="stationId">工位ID</param>
        /// <param name="resourceId">资源ID</param>
        /// <param name="printMode">打印模式</param>
        /// <param name="advanceBarcodeQueue">待扫描包装号</param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        [ApiService("包装采集")]
        [return: ApiReturn("包装采集 PackingCollect")]
        public virtual XPApiResultPackageInfo PackingCollect([ApiParameter("条码号")] string barcode,
            [ApiParameter("工单ID")] double workOrderId,
            [ApiParameter("操作人ID")] double employeeId, [ApiParameter("工序ID")] double processId, [ApiParameter("工位ID")] double stationId,
            [ApiParameter("资源ID")] double resourceId, [ApiParameter("打印方式")] int printMode, [ApiParameter] Queue<string> advanceBarcodeQueue)
        {
            XPApiResultPackageInfo result = new XPApiResultPackageInfo();

            var WorkOrder = RF.GetById<WorkOrder>(workOrderId);

            WIP.Workcell workcell = new WIP.Workcell
            {
                EmployeeId = employeeId,
                ProcessId = processId,
                StationId = stationId,
                ResourceId = resourceId
            };

            var barcodeWorkOrderId = RT.Service.Resolve<BarcodeController>().GetBarcodeWorkOrderId(barcode);

            if (barcodeWorkOrderId != 0 && barcodeWorkOrderId != workOrderId)
            {
                var wo = RF.GetById<WorkOrder>(barcodeWorkOrderId);
                if (WorkOrder != null)
                {
                    result.IsChangeOrder = true;
                }

                //切换当前在制工单
                WorkOrder = wo;

                Task.Run(new Action(() =>
                {
                    //切换产线、工序、工位的在制工单
                    base.ChangeWipResourceWorkOrder(wo.Id, workcell);

                    //这个事件在ESOP有订阅
                    RT.EventBus.Publish(new ChangeWipResourceWorkOrderEvent { WorkOrderId = barcodeWorkOrderId });

                    //更新工单任务报工方式
                    //UpdateWorkOrdeReportModel(wo.Id);
                }).WithCurrentThreadContext());
            }

            EntityList<PackageSnRecord> PackageSnRecordList = RT.Service.Resolve<NewPackageController>().GetPackageSnRecords(resourceId, processId, stationId);
            if (PackageSnRecordList.Count > 0 && !PackageSnRecordList.Any(p => p.ProductId == WorkOrder.ProductId))
            {
                throw new ValidationException("条码明细不为空，不允许切换产品包装".L10N());
            }

            if (result.IsChangeOrder)
            {
                //工单
                var workOrder = RF.GetById<WorkOrder>(barcodeWorkOrderId, new EagerLoadOptions().LoadWithViewProperty());
                result.WorkOrder = new ApiModels.WorkOrderInfo(workOrder);

                //包装规则
                var workOrderRule = base.GetWorkOrderRule(workOrder.Id) ?? throw new ValidationException("工单没有包装规则！".L10N());
                result.PackageRules = new List<XPWorkOrderPackageRuleDetail>();
                foreach (var item in workOrderRule)
                {
                    result.PackageRules.Add(new XPWorkOrderPackageRuleDetail(item));
                }
            }

            List<string> barcodes = new List<string>()
            {
                barcode
            };
            var collectData = new CollectData
            {
                State = WipProductProcessState.Finish
            };
            collectData.Context["ADVANCE_PACKAGE_NO_LIST"] = advanceBarcodeQueue;
            collectData.Context["IS_ADVANCE"] = printMode == (int)SIE.Tech.Stations.Configs.PrintMode.InAdvance;
            var productInfo = base.PkgCollect(barcodes.ToArray(), collectData, workcell);

            result.Sns = productInfo.Context["PACK_NO_LIST_STRING"] as string;

            //刷新条码明细
            PackageSnRecordList = RT.Service.Resolve<NewPackageController>().GetPackageSnRecords(resourceId, processId, stationId);
            PackageSnRecordList.MarkSaved();
            if (PackageSnRecordList != null && PackageSnRecordList.Count > 0)
            {
                result.PackageSnRecords = new List<XPPackageSnRecord>();
                foreach (var packageRecord in PackageSnRecordList)
                    result.PackageSnRecords.Add(new XPPackageSnRecord(packageRecord));
            }

            //需要打印的包装关系
            result.PrintRelations = new List<XPPackingRelation>();
            var printRelations = RT.Service.Resolve<PackingRelationController>().GetPackingRelations(result.Sns.Split(',').ToList());
            foreach (var item in printRelations)
                result.PrintRelations.Add(new XPPackingRelation(item, null));

            //刷新包装关系
            var packageRelations = RT.Service.Resolve<PackingRelationController>().GetAllPackingRelations(PackageSnRecordList.Where(p => p.WoSn != "").Select(p => p.Sn).Distinct().ToList(), true);
            result.AllRelations = new List<XPPackingRelation>();
            var itemLabels = GetItemLabelsByPackageSnRecordList(PackageSnRecordList);
            foreach (var item in packageRelations)
                result.AllRelations.Add(new XPPackingRelation(item, itemLabels));

            return result;
        }

        /// <summary>
        /// 手动打包
        /// </summary>
        /// <param name="employeeId">操作人ID</param>
        /// <param name="processId">工序ID</param>
        /// <param name="stationId">工位ID</param>
        /// <param name="resourceId">资源ID</param>
        /// <param name="targetSn">要打包的条码明细记录</param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        [ApiService("手动打包")]
        [return: ApiReturn("手动打包 PackageMuanual")]
        public virtual XPApiResultPackageInfo PackageMuanual([ApiParameter("操作人ID")] double employeeId, [ApiParameter("工序ID")] double processId, [ApiParameter("工位ID")] double stationId,
            [ApiParameter("资源ID")] double resourceId, [ApiParameter] List<string> targetSn)
        {
            EntityList<PackageSnRecord> PackageSnRecordList = RT.Service.Resolve<NewPackageController>().GetPackageSnRecords(resourceId, processId, stationId);
            EntityList<PackageSnRecord> records = new EntityList<PackageSnRecord>();
            records.AddRange(PackageSnRecordList.Where(p => targetSn.Contains(p.Sn)));

            if (records.Count <= 0)
                throw new ValidationException("条码已经被打包".L10N());

            var wo = RF.GetById<WorkOrder>(records.FirstOrDefault().WorkOrderId);
            if (wo == null)
                throw new ValidationException("数据异常，工单不存在".L10N());

            var rules = wo.PackageRuleDetailList.OrderBy(p => SortExtension.GetIndex(p)).ToArray();
            var curRule = rules.FirstOrDefault(p => p.PackageUnitId == records.FirstOrDefault().PackageUnitId);

            bool isNext = false;
            WorkOrderPackageRuleDetail nextRule = null;
            for (int i = 0; i < rules.Length; i++)
            {
                if (isNext)
                {
                    nextRule = rules[i];
                    break;
                }
                if (curRule.Id == rules[i].Id)
                    isNext = true;
            }

            if (nextRule == null)
                throw new ValidationException("找不到包装单位[{0}]对应的下一层级".L10nFormat(curRule.PackageUnit.Name));
            if (records.Count > nextRule.LevelQty)
                throw new ValidationException("最多选择[{0}]进行打包，当前选择的数量[{1}]".L10nFormat(nextRule.LevelQty, records.Count));

            WIP.Workcell workcell = new WIP.Workcell
            {
                EmployeeId = employeeId,
                ProcessId = processId,
                StationId = stationId,
                ResourceId = resourceId
            };

            //执行打包逻辑
            var resultInfo = RT.Service.Resolve<NewPackageController>().DoPackageMuanual(records, rules, nextRule, wo.Id, workcell);
            if (!resultInfo.Item1.IsNullOrEmpty())
            {
                throw new ValidationException(resultInfo.Item1);
            }
            XPApiResultPackageInfo result = new XPApiResultPackageInfo();
            result.MuanualPackageNo = resultInfo.Item2;
            if (result.MuanualPackageNo.IsNotEmpty())
            {
                var printRelations = RT.Service.Resolve<PackingRelationController>().GetPackingRelations(result.MuanualPackageNo.Split(',').ToList());
                RT.EventBus.Publish(new DoPackingEvent(DoPackingAction.Packed, "MesPacking1", printRelations.ToArray()));

                //需要打印的包装关系
                result.PrintRelations = new List<XPPackingRelation>();
                foreach (var item in printRelations)
                    result.PrintRelations.Add(new XPPackingRelation(item, null));
            }

            //刷新条码明细
            PackageSnRecordList = RT.Service.Resolve<NewPackageController>().GetPackageSnRecords(resourceId, processId, stationId);
            PackageSnRecordList.MarkSaved();
            if (PackageSnRecordList != null && PackageSnRecordList.Count > 0)
            {
                result.PackageSnRecords = new List<XPPackageSnRecord>();
                foreach (var packageRecord in PackageSnRecordList)
                    result.PackageSnRecords.Add(new XPPackageSnRecord(packageRecord));
            }

            //刷新包装关系
            var packageRelations = RT.Service.Resolve<PackingRelationController>().GetAllPackingRelations(PackageSnRecordList.Where(p => p.WoSn != "").Select(p => p.Sn).Distinct().ToList(), true);
            result.AllRelations = new List<XPPackingRelation>();
            var itemLabels = GetItemLabelsByPackageSnRecordList(PackageSnRecordList);
            foreach (var item in packageRelations)
                result.AllRelations.Add(new XPPackingRelation(item, itemLabels));

            return result;
        }


        /// <summary>
        /// 重新加载工位工序未完成的包装关系
        /// </summary>
        /// <param name="processId">工序ID</param>
        /// <param name="stationId">工位ID</param>
        /// <param name="resourceId">资源ID</param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        [ApiService("重新加载工位工序未完成的包装关系")]
        [return: ApiReturn("重新加载工位工序未完成的包装关系 ReloadPackingRelation")]
        public virtual XPApiResultPackageInfo ReloadPackingRelation([ApiParameter("工序ID")] double processId, [ApiParameter("工位ID")] double stationId,
            [ApiParameter("资源ID")] double resourceId)
        {
            EntityList<PackageSnRecord> PackageSnRecordList = RT.Service.Resolve<NewPackageController>().GetPackageSnRecords(resourceId, processId, stationId);
            PackageSnRecordList.MarkSaved();

            XPApiResultPackageInfo result = new XPApiResultPackageInfo();
            result.PackageSnRecords = new List<XPPackageSnRecord>();
            foreach (var packageRecord in PackageSnRecordList)
                result.PackageSnRecords.Add(new XPPackageSnRecord(packageRecord));

            //刷新包装关系
            var packageRelations = RT.Service.Resolve<PackingRelationController>().GetAllPackingRelations(PackageSnRecordList.Where(p => p.WoSn != "").Select(p => p.Sn).Distinct().ToList(), true);
            result.AllRelations = new List<XPPackingRelation>();
            var itemLabels = GetItemLabelsByPackageSnRecordList(PackageSnRecordList);
            foreach (var item in packageRelations)
                result.AllRelations.Add(new XPPackingRelation(item, itemLabels));

            return result;
        }

        private EntityList<ItemLabel> GetItemLabelsByPackageSnRecordList(EntityList<PackageSnRecord> packageSnRecordList)
        {
            String woSns = "";
            foreach (var packageSnRecord in packageSnRecordList)
                woSns += "," + packageSnRecord.WoSn;

            List<string> listSn = woSns.Split(',').ToList().Distinct().ToList();
            listSn.RemoveAll(p => p.IsNullOrEmpty());
            return RT.Service.Resolve<ItemLabelController>().GetItemLabelsWithWorkOrder(listSn);
        }


        /// <summary>
        /// 更新包装关系物流状态
        /// </summary>
        /// <param name="relationId">包装关系ID</param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        [ApiService("更新包装关系物流状态")]
        [return: ApiReturn("更新包装关系物流状态 ReloadPackingRelation")]
        public virtual void UpdateRelationStatePrinted([ApiParameter("包装关系ID")] double relationId)
        {
            RT.Service.Resolve<PackingRelationController>().UpdateRelationState(relationId, LogisticState.Printed);
        }
    }
}
