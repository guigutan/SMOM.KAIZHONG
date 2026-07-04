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
using SIE.MES.WIP.Packings;
using SIE.MES.WIP.Products;
using SIE.MES.WorkOrders;
using SIE.Packages;
using SIE.Packages.ItemLabels;
using SIE.Packages.Packages;
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
    /// 直接包装的API控制器
    /// </summary>
    public class WinFormDirectPackingApiController : DirectPackingController
    {
        /// <summary>
        /// 验证包装号
        /// </summary>
        /// <param name="packageNo">包装号</param>
        /// <param name="packingUnit">包装单位</param>
        /// <param name="packingRelation">包装关系</param>
        [ApiService("包装采集-验证包装号")]
        public virtual void ValidatePackingBarcode([ApiParameter("包装号")] string packageNo,
            [ApiParameter("包装单位")] PackingUnit packingUnit, [ApiParameter("包装关系")] PackingRelation packingRelation)
        {
            RT.Service.Resolve<PackingBarcodeController>().ValidatePackingBarcode(packageNo, packingUnit, packingRelation);
        }

        /// <summary>
        /// 包装采集-校验条码信息
        /// </summary>
        /// <param name="collectBarcode">扫描条码</param>
        /// <param name="workcell">工作单元</param>
        /// <param name="woId">当前工单Id</param>
        /// <returns>条码信息</returns>
        [ApiService("包装采集-校验条码信息")]
        [return: ApiReturn("条码信息")]
        public virtual ValidateResult PackingValidate([ApiParameter("扫描条码")] CollectBarcode collectBarcode, [ApiParameter("工作单元")] WIP.Workcell workcell, [ApiParameter("当前工单Id")] double woId)
        {
            var ct = RT.Service.Resolve<WinFormMoveApiController>();
            ApiModels.WorkOrderInfo resultWo = null;
            int? reportModel = -1;
            var product = this.Validate(collectBarcode, workcell);
            if (product.WorkOrderId != 0)
            {
                var workOrder = RF.GetById<WorkOrder>(product.WorkOrderId, new EagerLoadOptions().LoadWithViewProperty());
                if (workOrder != null && product.WorkOrderId != woId)
                {
                    resultWo = new ApiModels.WorkOrderInfo(workOrder);
                    this.ChangeWipResourceWorkOrder(workOrder.Id, workcell);
                    RT.EventBus.Publish(new ChangeWipResourceWorkOrderEvent { WorkOrderId = product.WorkOrderId });
                    reportModel = ct.UpdateWorkOrdeReportModel(product.WorkOrderId);
                }
            }
            ct.ValidateTaskReport(product.WorkOrderId, workcell, reportModel);
            var result = new ValidateResult()
            {
                ProductInfo = product,
                WorkOrderInfo = resultWo,
                Context = product.Context
            };
            return result;
        }



        /// <summary>
        /// 获取最高层级得包装规则
        /// </summary>
        /// <param name="woRuleDtls"></param>
        /// <param name="workOrderNo"></param>
        /// <param name="packingUnit"></param>
        /// <returns></returns>
        [ApiService("获取最高层级得包装规则")]
        [return: ApiReturn("获取最高层级得包装规则")]
        public virtual XPWorkOrderPackageRuleDetail GetUpLevelPackingRule(List<WorkOrderPackageRuleDetail> woRuleDtls, string workOrderNo,
            PackingUnit packingUnit)
        {
            var result = this.GetUpLevelPackingRule(woRuleDtls, workOrderNo, packingUnit, true);
            if (result != null)
            {
                return new XPWorkOrderPackageRuleDetail(result);
            }
            return null;
        }

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
            EntityList<DirectPackageSnRecord> PackageSnRecordList = this.GetPackageSnRecords(resourceId, processId, stationId);
            if (PackageSnRecordList != null && PackageSnRecordList.Count > 0)
            {
                result.PackageSnRecords = new List<XPPackageSnRecord>();
                foreach (var packageRecord in PackageSnRecordList)
                {
                    result.PackageSnRecords.Add(XPPackageSnRecord.GetDirectPackageSnRecord(packageRecord));
                }
            }

            //包装规则
            if (result.WorkOrder != null)
            {
                var workOrderRule = this.GetWorkOrderRule(result.WorkOrder.Id);
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
            var config = ConfigService.GetConfig(new DirectPackingPrintModeConfig(), typeof(Station), station);
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

            var workOrderRule = this.GetWorkOrderRule(woId) ?? throw new ValidationException("工单没有包装规则！".L10N());

            if (result.IsChangeOrder)
            {
                //包装规则
                result.PackageRules = new List<XPWorkOrderPackageRuleDetail>();
                foreach (var item in workOrderRule)
                {
                    result.PackageRules.Add(new XPWorkOrderPackageRuleDetail(item));
                }

                //条码明细
                var PackageSnRecordList = base.GetPackageSnRecords(resourceId, processId, stationId);
                if (PackageSnRecordList != null && PackageSnRecordList.Count > 0)
                {
                    result.PackageSnRecords = new List<XPPackageSnRecord>();
                    foreach (var packageRecord in PackageSnRecordList)
                        result.PackageSnRecords.Add(XPPackageSnRecord.GetDirectPackageSnRecord(packageRecord));
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
            var productId = this.GetWorkOrderProductId(woId);
            var records = base.GetPackageSnRecords(workcell.ResourceId, workcell.ProcessId, workcell.StationId);

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
        /// 获取工单包装规则
        /// </summary>
        /// <param name="woId"></param>
        /// <returns></returns>
        public virtual WorkOrderPackageRuleDetail[] GetWorkOrderRule(double woId)
        {
            return Query<WorkOrderPackageRuleDetail>().Where(p => p.WorkOrderId == woId).ToList(null, new EagerLoadOptions().LoadWithViewProperty()).OrderBy(p => SortExtension.GetIndex(p)).ToArray();
        }

        /// <summary>
        /// 包装采集
        /// </summary>
        /// <param name="collectData"></param>
        /// <param name="workOrderId"></param>
        /// <param name="workcell"></param>
        /// <param name="needMove"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        [ApiService("包装采集")]
        [return: ApiReturn("包装采集 PackingCollect")]
        public virtual XPApiResultPackageInfo PackingCollect([ApiParameter("扫描数据")] CollectData collectData,
            [ApiParameter("工单ID")] double workOrderId,
            [ApiParameter("工作单元")] WIP.Workcell workcell,
             [ApiParameter("是否需要过站")] bool needMove
            )
        {
            XPApiResultPackageInfo result = new XPApiResultPackageInfo();
            var WorkOrder = RF.GetById<WorkOrder>(workOrderId);

            var barcodeWorkOrderId = RT.Service.Resolve<BarcodeController>().GetBarcodeWorkOrderId(collectData.CollectBarcode.Code);

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

                }).WithCurrentThreadContext());
            }
            if (result.IsChangeOrder)
            {
                //工单
                var workOrder = RF.GetById<WorkOrder>(barcodeWorkOrderId, new EagerLoadOptions().LoadWithViewProperty());
                result.WorkOrder = new ApiModels.WorkOrderInfo(workOrder);

                //包装规则
                var workOrderRule = this.GetWorkOrderRule(workOrder.Id) ?? throw new ValidationException("工单没有包装规则！".L10N());
                result.PackageRules = new List<XPWorkOrderPackageRuleDetail>();
                foreach (var item in workOrderRule)
                {
                    result.PackageRules.Add(new XPWorkOrderPackageRuleDetail(item));
                }
            }

            //处理包装关系            
            var CurrentPackingRelation = this.PackingCollect(collectData.CollectBarcode.Code, collectData, workcell, needMove);
            // 打印
            if (CurrentPackingRelation.PackageNo.IsNotEmpty())
            {
                var printRelations = RT.Service.Resolve<PackingRelationController>().GetPackingRelations(CurrentPackingRelation.PackageNo.Split(',').ToList());
                var packingEvent = this.CreatePackingEvent("MesPacking", ScanMode.Normal, collectData.CollectBarcode.Code, CurrentPackingRelation, WorkOrder, collectData.PackingData.CurrentPackingUnit);

                foreach (var item in printRelations)
                {
                    result.PrintRelations.Add(new XPPackingRelation(item, null));
                }

            }
            var PackageSnRecordList = base.GetPackageSnRecords(workcell.ResourceId, workcell.ProcessId, workcell.StationId);
            if (PackageSnRecordList != null && PackageSnRecordList.Count() > 0)
            {
                result.PackageSnRecords = new List<XPPackageSnRecord>();
                foreach (var packageRecord in PackageSnRecordList)
                {
                    result.PackageSnRecords.Add(XPPackageSnRecord.GetDirectPackageSnRecord(packageRecord));
                }
            }



            //刷新包装关系
            var packageRelations = RT.Service.Resolve<PackingRelationController>().GetAllPackingRelations(PackageSnRecordList.Where(p => p.WoSn != "").Select(p => p.Sn).Distinct().ToList(), true);
            result.AllRelations = new List<XPPackingRelation>();
            var itemLabels = GetItemLabelsByPackageSnRecordList(PackageSnRecordList);
            foreach (var item in packageRelations)
                result.AllRelations.Add(new XPPackingRelation(item, itemLabels));
            result.CurrentPackingRelation = CurrentPackingRelation;
            return result;
        }

        /// <summary>
        /// 手动打包
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <param name="record"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        [ApiService("手动打包")]
        [return: ApiReturn("手动打包 PackageMuanual")]
        public virtual XPApiResultPackageInfo PackageMuanual([ApiParameter("工单Id")] WIP.Workcell workcell, [ApiParameter("工单Id")] double workOrderId, [ApiParameter("所选记录的包装单位")] DirectPackageSnRecord record)
        {
            var wo = RF.GetById<WorkOrder>(workOrderId, new EagerLoadOptions().LoadWith(WorkOrder.PackageRuleDetailListProperty));
            if (wo == null)
                throw new ValidationException("数据异常，工单不存在".L10N());
            XPApiResultPackageInfo xPApiResultPackageInfo = new XPApiResultPackageInfo();
            var rules = wo.PackageRuleDetailList.OrderBy(p => SortExtension.GetIndex(p)).ToList();
            record = RF.GetById<DirectPackageSnRecord>(record.Id);//重新取值

            var curRule = rules.FirstOrDefault(p => p.PackageUnitId == record.PackageUnitId);

            //执行打包逻辑
            var pkgNo = RT.Service.Resolve<DirectPackingController>().GivePackageNo(rules, curRule, record);
            if (pkgNo.IsNotEmpty())
            {
                var printRelations = RT.Service.Resolve<PackingRelationController>().GetPackingRelations(pkgNo.Split(',').ToList());

                foreach (var item in printRelations)
                {
                    xPApiResultPackageInfo.PrintRelations.Add(new XPPackingRelation(item, null));
                }
            }
            xPApiResultPackageInfo.MuanualPackageNo = pkgNo;
            //刷新条码明细
            var PackageSnRecordList = this.GetPackageSnRecords(workcell.ResourceId, workcell.ProcessId, workcell.StationId);
            PackageSnRecordList.MarkSaved();
            if (PackageSnRecordList != null && PackageSnRecordList.Count > 0)
            {
                xPApiResultPackageInfo.PackageSnRecords = new List<XPPackageSnRecord>();
                foreach (var packageRecord in PackageSnRecordList)
                    xPApiResultPackageInfo.PackageSnRecords.Add(XPPackageSnRecord.GetDirectPackageSnRecord(packageRecord));
            }

            //刷新包装关系
            var packageRelations = RT.Service.Resolve<PackingRelationController>().GetAllPackingRelations(PackageSnRecordList.Where(p => p.WoSn != "").Select(p => p.Sn).Distinct().ToList(), true);
            xPApiResultPackageInfo.AllRelations = new List<XPPackingRelation>();
            var itemLabels = GetItemLabelsByPackageSnRecordList(PackageSnRecordList);
            foreach (var item in packageRelations)
                xPApiResultPackageInfo.AllRelations.Add(new XPPackingRelation(item, itemLabels));
            return xPApiResultPackageInfo;
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
            EntityList<DirectPackageSnRecord> PackageSnRecordList = base.GetPackageSnRecords(resourceId, processId, stationId);
            PackageSnRecordList.MarkSaved();

            XPApiResultPackageInfo result = new XPApiResultPackageInfo();
            result.PackageSnRecords = new List<XPPackageSnRecord>();
            foreach (var packageRecord in PackageSnRecordList)
            {
                result.PackageSnRecords.Add(XPPackageSnRecord.GetDirectPackageSnRecord(packageRecord));
            }
            //刷新包装关系
            var packageRelations = RT.Service.Resolve<PackingRelationController>().GetAllPackingRelations(PackageSnRecordList.Where(p => p.WoSn != "").Select(p => p.Sn).Distinct().ToList(), true);
            result.AllRelations = new List<XPPackingRelation>();
            var itemLabels = GetItemLabelsByPackageSnRecordList(PackageSnRecordList);
            foreach (var item in packageRelations)
                result.AllRelations.Add(new XPPackingRelation(item, itemLabels));

            return result;
        }

        private EntityList<ItemLabel> GetItemLabelsByPackageSnRecordList(EntityList<DirectPackageSnRecord> packageSnRecordList)
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

        /// <summary>
        /// 获取工单产品id
        /// </summary>
        /// <param name="woId"></param>
        /// <returns></returns>
        private double GetWorkOrderProductId(double woId)
        {
            var wo = Query<WorkOrder>().Where(p => p.Id == woId).FirstOrDefault();
            return wo != null ? wo.ProductId : 0;
        }
        /// <summary>
        /// 获取所有包装单位
        /// </summary>
        /// <returns></returns>
        [ApiService("获取所有包装单位")]
        [return: ApiReturn("获取所有包装单位 ReloadPackingRelation")]
        public virtual List<PackingUnit> GetAllPackingUnit()
        {
            return RF.GetAll<PackingUnit>().ToList();
        }
    }
}
