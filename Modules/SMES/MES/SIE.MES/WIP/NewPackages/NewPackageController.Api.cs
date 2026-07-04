using Castle.Core.Internal;
using Microsoft.Scripting.Utils;
using NPOI.HSSF.Record;
using SIE.Api;
using SIE.Barcodes;
using SIE.Common.Configs;
using SIE.Common.Sort;
using SIE.Core.Barcodes;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.MES.ProcessStatistics;
using SIE.MES.PackingPrints;
using SIE.MES.WIP.Configs;
using SIE.MES.WIP.NewPackages.API;
using SIE.MES.WIP.Products;
using SIE.MES.WorkOrders;
using SIE.Packages;
using SIE.Packages.ItemLabels;
using SIE.Packages.Packages;
using SIE.Packages.Packings;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace SIE.MES.WIP.NewPackages
{
    /// <summary>
    /// 包装采集API
    /// </summary>
    public partial class NewPackageController : WipController
    {
        /// <summary>
        /// 初始化app页面（带出为完成的打包记录）
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="processId"></param>
        /// <param name="stationId"></param>
        /// <returns></returns>
        [ApiService("初始化app页面（带出为完成的打包记录）")]
        [return: ApiReturn("获取工单信息、包装记录列表、包装规则列表 PackageInfo")]
        public virtual PackageInfo InitPackageInfo([ApiParameter] double resourceId, [ApiParameter] double processId, [ApiParameter] double stationId)
        {
            if (resourceId == 0)
            {
                throw new ValidationException("资源信息有误！".L10N());
            }
            if (processId == 0)
            {
                throw new ValidationException("工序信息有误！".L10N());
            }
            if (stationId == 0)
            {
                throw new ValidationException("工位信息有误！".L10N());
            }
            PackageInfo packageInfo = new PackageInfo();
            var wipResourceWorkOrder = Query<WipResourceWorkOrder>().Where(f => f.ResourceId == resourceId && f.ProcessId == processId && f.StationId == stationId).FirstOrDefault();
            List<PackageRuleInfo> workOrderPackageRuleDetailList = new List<PackageRuleInfo>();
            // 在制工单信息和包装规则
            if (wipResourceWorkOrder != null)
            {
                var workOrder = RF.GetById<WorkOrder>(wipResourceWorkOrder.WorkOrderId, new EagerLoadOptions().LoadWithViewProperty());
                if (workOrder == null)
                {
                    throw new ValidationException("工单不存在，请检查是否有对应工厂权限！".L10N());
                }
                var workOrderPackageRule = Query<WorkOrderPackageRuleDetail>().Where(p => p.WorkOrderId == workOrder.Id).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                var collectQty = RT.Service.Resolve<IProcessStatistics>().GetCollectQty(resourceId, processId, stationId, workOrder.Id);
                // 产品信息
                WorkOrderInfo workOrderInfo = new WorkOrderInfo
                {
                    WoId = workOrder.Id,
                    WoNo = workOrder.No,
                    ProductCode = workOrder.ProductCode,
                    Barcode = string.Empty,
                    Qty = collectQty,
                };
                packageInfo.WorkOrderInfo = workOrderInfo;
                workOrderPackageRule.ForEach(rule =>
                {
                    PackageRuleInfo packageRule = new PackageRuleInfo
                    {
                        PackageUnit = rule.PackageUnitName,
                        LevelQty = rule.LevelQty,
                        Qty = rule.Qty,
                        NumRule = rule.NumberRuleName,
                    };
                    workOrderPackageRuleDetailList.Add(packageRule);
                });
            }
            else
            {
                WorkOrderInfo workOrderInfo = new WorkOrderInfo();
                packageInfo.WorkOrderInfo = workOrderInfo;
            }

            List<PackageSn> packageSns = GetPackageSnRecord(resourceId, processId, stationId);
            packageInfo.WorkOrderPackageRuleDetailList = workOrderPackageRuleDetailList;
            packageInfo.DirectPackageSnRecordList = packageSns;
            return packageInfo;
        }

        /// <summary>
        /// 验证是否提前打印
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="resourceId"></param>
        /// <param name="processId"></param>
        /// <param name="stationId"></param>
        /// <returns></returns>
        [ApiService("验证是否提前打印")]
        [return: ApiReturn("是否需要包装号、包装号单位队列")]
        public virtual AdvanceInfo IsNeedPackageNo([ApiParameter] string barcode, [ApiParameter] double resourceId, [ApiParameter] double processId, [ApiParameter] double stationId)
        {
            AdvanceInfo advanceInfo = new AdvanceInfo();
            bool isNeedPackageNo = false;
            Queue<PackingUnit> packageNoList = new Queue<PackingUnit>();
            Workcell workcell = new Workcell
            {
                ResourceId = resourceId,
                ProcessId = processId,
                StationId = stationId,
                EmployeeId = RT.IdentityId,
            };
            var collectBarcode = new CollectBarcode { Code = barcode, Type = BarcodeType.SN };
            if (barcode.IsNullOrEmpty())
            {
                throw new ValidationException("条码不能为空！".L10N());
            }
            var barcodeEntity = RT.Service.Resolve<BarcodeController>().GetBarcode(barcode);
            if (barcodeEntity == null)
            {
                throw new ValidationException("条码不存在！".L10N());
            }
            var woId = barcodeEntity.WorkOrderId;
            if (woId == null)
            {
                throw new ValidationException("条码没有归属工单！".L10N());
            }
            var workOrderRule = GetWorkOrderRule(woId.Value);
            if (workOrderRule == null)
            {
                throw new ValidationException("工单没有包装规则！".L10N());
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
            Validate(collectBarcode, workcell);


            var productId = GetWorkOrderProductId(woId.Value);
            var records = GetPackageSnRecords(workcell.ResourceId, workcell.ProcessId, workcell.StationId, woId.Value, productId);
            var record = GeneragePackageSnRecord(barcode, masterUnit.PackageUnitId, woId.Value, productId, "", workcell.ResourceId, workcell.ProcessId, workcell.StationId, 1, 1);
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
                    // 当前扫描记录中拿出当前层级数量
                    var curRecords = allRecords.Take(Convert.ToInt32(currentRule.LevelQty)).ToList();

                    //生成包装
                    if (!isLastRult)
                    {
                        var parentRecord = GeneragePackageSnRecord("", currentRule.PackageUnitId, woId.Value, productId, "", workcell.ResourceId, workcell.ProcessId, workcell.StationId, curRecords.Count, curRecords.Sum(p => p.ItemQty));
                        // 当前所有扫描记录中添加新生成的包装层级
                        records.Add(parentRecord);
                    }
                    // 生成新的包装层级单位
                    packageNoList.Enqueue(currentRule.PackageUnit);

                    // 将这些获取的记录标记删除(已合成上级包装)
                    curRecords.ForEach(p => p.PersistenceStatus = PersistenceStatus.Deleted);

                    // 更新上一层级的数据
                    allRecords = records.Where(p => p.PackageUnitId == upperRule.PackageUnitId && p.PersistenceStatus != PersistenceStatus.Deleted).OrderBy(p => p.CreateDate).ToList();
                }
            }


            advanceInfo.PackageUnitList = packageNoList;
            if (packageNoList.Any())
            {
                isNeedPackageNo = true;
            }
            advanceInfo.IsNeedPackageNo = isNeedPackageNo;
            return advanceInfo;
        }

        /// <summary>
        /// 验证预输入的包装号是否合理且是否继续输入包装号
        /// </summary>
        /// <param name="packageNo"></param>
        /// <param name="packageUnitList"></param>
        /// <param name="woId"></param>
        /// <returns></returns>
        [ApiService("验证预输入的包装号是否合理且是否继续输入包装号")]
        [return: ApiReturn("是否需要包装号、包装号单位队列")]
        public virtual AdvanceInfo PackageNoIsLegal([ApiParameter] string packageNo, [ApiParameter] Queue<PackingUnit> packageUnitList, [ApiParameter] double woId)
        {
            AdvanceInfo advanceInfo = new AdvanceInfo();
            var isNeedPackageNo = false;
            if (!packageUnitList.Any() || packageUnitList == null)
            {
                throw new ValidationException("预输入包装单位队列异常".L10N());
            }
            var curRuleUnit = packageUnitList.Peek();
            if (curRuleUnit == null)
            {
                throw new ValidationException("单位异常！".L10N());

            }
            if (packageNo.IsNullOrEmpty())
            {
                throw new ValidationException("包装号不能为空".L10N());
            }
            var packingBarcode = RT.Service.Resolve<PackingBarcodeController>().GetPackingBarcode(packageNo);
            if (packingBarcode == null)
            {
                throw new ValidationException("包装号[{0}]不存在".L10nFormat(packageNo));
            }

            if (packingBarcode.IsUse)
            {
                throw new ValidationException("包装号[{0}]已使用".L10nFormat(packageNo));
            }
            if (packingBarcode.PackageUnitId != curRuleUnit.Id)
            {
                throw new ValidationException("包装号【{0}】包装单位是【{1}】与要扫描的包装单位不相符，请扫描【{2}】的包装号"
                    .L10nFormat(packageNo, packingBarcode.PackageUnitName, curRuleUnit.Name));
            }
            if (packingBarcode.IsUse)
            {
                throw new ValidationException("包装号[{0}]已使用".L10nFormat(packageNo));
            }
            if (packingBarcode.WorkOrderId != woId)
            {
                throw new ValidationException("包装号[{0}]的工单与当前正在包装的工单不相同".L10nFormat(packageNo));
            }
            packageUnitList.Dequeue();
            if (packageUnitList.Any())
            {
                isNeedPackageNo = true;
            }
            advanceInfo.IsNeedPackageNo = isNeedPackageNo;
            advanceInfo.PackageUnitList = packageUnitList;
            return advanceInfo;
        }

        /// <summary>
        /// 包装采集APP端扫码
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="nowWoNo"></param>
        /// <param name="resourceId"></param>
        /// <param name="processId"></param>
        /// <param name="stationId"></param>
        /// <param name="packageNoList"></param>
        /// <param name="advance"></param>
        /// <returns></returns>
        [ApiService("包装采集APP端扫码")]
        [return: ApiReturn("获取包装信息 PackageScanInfo")]
        public virtual PackageScanInfo ScanBarCode([ApiParameter] string barcode, [ApiParameter] string nowWoNo,
            [ApiParameter] double resourceId, [ApiParameter] double processId, [ApiParameter] double stationId,
            [ApiParameter] Queue<string> packageNoList, [ApiParameter] bool advance = false)
        {
            if (resourceId == 0)
            {
                throw new ValidationException("资源信息有误！".L10N());
            }
            if (processId == 0)
            {
                throw new ValidationException("工序信息有误！".L10N());
            }
            if (stationId == 0)
            {
                throw new ValidationException("工位信息有误！".L10N());
            }
            if (barcode.IsNullOrEmpty())
            {
                throw new ValidationException("条码不能为空！".L10N());
            }
            // 组装过站信息：条码、预输入包装号，是否提前
            List<string> barcodes = new List<string>();
            barcodes.Add(barcode);
            var collectData = new CollectData();
            collectData.State = WipProductProcessState.Finish;
            collectData.Context["ADVANCE_PACKAGE_NO_LIST"] = packageNoList;
            collectData.Context["IS_ADVANCE"] = advance;

            // 操作提示
            string message = string.Empty;
            //条码对应工单id
            var barcodeWorkOrderId = RT.Service.Resolve<BarcodeController>().GetBarcodeWorkOrderId(barcode);
            var workOrderPackageRule = Query<WorkOrderPackageRuleDetail>().Where(p => p.WorkOrderId == barcodeWorkOrderId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            List<PackageRuleInfo> workOrderPackageRuleDetailList = new List<PackageRuleInfo>();
            workOrderPackageRule.ForEach(rule =>
            {
                PackageRuleInfo packageRule = new PackageRuleInfo
                {
                    PackageUnit = rule.PackageUnitName,
                    LevelQty = rule.LevelQty,
                    Qty = rule.Qty,
                    NumRule = rule.NumberRuleName,
                };
                workOrderPackageRuleDetailList.Add(packageRule);
            });
            var barcodeWorkOrder = RF.GetById<WorkOrder>(barcodeWorkOrderId, new EagerLoadOptions().LoadWithViewProperty());
            Workcell workcell = new Workcell
            {
                ResourceId = resourceId,
                ProcessId = processId,
                StationId = stationId,
                EmployeeId = RT.IdentityId,
            };
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                // 切换在制工单
                if (barcodeWorkOrder != null && barcodeWorkOrder.No != nowWoNo)
                {
                    message += "工单已切换,由[{0}]切换到[{1}]".L10nFormat(nowWoNo, barcodeWorkOrder.No);
                    ChangeWipResourceWorkOrder(barcodeWorkOrder.Id, workcell);
                }
                // 过站
                var productInfo = PkgCollect(barcodes.ToArray(), collectData, workcell);
                message += "【{0}】采集成功！".L10nFormat(barcode);
                string pkgNo = productInfo.Context["PACK_NO_LIST_STRING"] as string;
                if (pkgNo.IsNotEmpty())
                {
                    message += "生成包装号【{0}】!".L10nFormat(pkgNo);
                }

                // 产品信息
                WorkOrderInfo workOrderInfo = new WorkOrderInfo
                {
                    WoId = barcodeWorkOrderId,
                    WoNo = barcodeWorkOrder.No,
                    ProductCode = barcodeWorkOrder.ProductCode,
                    Barcode = barcode,
                };
                // 刷新包装采集记录
                List<PackageSn> packageSns = GetPackageSnRecord(resourceId, processId, stationId);

                PackageScanInfo packageScanInfo = new PackageScanInfo
                {
                    WorkOrderInfo = workOrderInfo,
                    WorkOrderPackageRuleDetailList = workOrderPackageRuleDetailList,
                    DirectPackageSnRecordList = packageSns,
                    ResultMessage = message,
                };
                tran.Complete();
                return packageScanInfo;
            }
        }

        /// <summary>
        /// 获取包装采集记录
        /// </summary>
        /// <returns></returns>
        [ApiService("获取包装采集记录")]
        [return: ApiReturn("包装采集记录 List<PackageSn>")]
        public virtual List<PackageSn> GetPackageSnRecord([ApiParameter] double resourceId, [ApiParameter] double processId, [ApiParameter] double stationId)
        {
            var packageSnRecordList = Query<PackageSnRecord>().Where(p => p.ResourceId == resourceId && p.ProcessId == processId && p.StationId == stationId)
                .OrderBy(p => p.Sn).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            List<PackageSn> packageSns = new List<PackageSn>();
            packageSnRecordList.ForEach(record =>
            {
                PackageSn packageSn = new PackageSn
                {
                    Id = record.Id,
                    Sn = record.Sn,
                    UnitId = record.PackageUnitId,
                    UnitName = record.PackageUnitName,
                    ItemCode = record.ProductCode,
                    WoId = record.WorkOrderId,
                    WoNo = record.WoNo,
                    ItemLabelList = record.WoSn,
                };
                packageSns.Add(packageSn);
            });
            return packageSns;
        }

        /// <summary>
        /// 手动打包
        /// </summary>
        /// <param name="packageSns"></param>
        /// <param name="resourceId"></param>
        /// <param name="processId"></param>
        /// <param name="stationId"></param>
        /// <returns></returns>
        [ApiService("手动打包验证")]
        [return: ApiReturn("是否能手动打包生成信息 IsPackInfo")]
        public virtual IsPackInfo PackageByHand([ApiParameter] List<PackageSn> packageSns,
            [ApiParameter] double resourceId, [ApiParameter] double processId, [ApiParameter] double stationId)
        {
            IsPackInfo isPackInfo = new IsPackInfo();
            if (packageSns == null || !packageSns.Any())
            {
                isPackInfo.CanPackage = false;
                isPackInfo.Message = "包装数据异常！".L10N();
                return isPackInfo;
            }
            if (packageSns.Count <= 0)
            {
                isPackInfo.CanPackage = false;
                isPackInfo.Message = "请选择打包内容！".L10N();
                return isPackInfo;
            }
            else
            {
                var relation = packageSns.FirstOrDefault();
                if (packageSns.Exists(p => p.UnitId != relation.UnitId) && packageSns.Exists(p => p.WoId != relation.WoId))
                {
                    isPackInfo.CanPackage = false;
                    isPackInfo.Message = "同工单、同单位的包装才能进行打包！".L10N();
                    return isPackInfo;
                }

            }
            var wo = RF.GetById<WorkOrder>(packageSns.FirstOrDefault().WoId) ?? throw new ValidationException("数据异常，工单不存在".L10N());
            var rules = wo.PackageRuleDetailList.OrderBy(p => SortExtension.GetIndex(p)).ToArray();
            if (!rules.Any())
            {
                isPackInfo.CanPackage = false;
                isPackInfo.Message = "当前工单没有包装规则！".L10N();
                return isPackInfo;
            }
            var curRule = rules.Find(p => p.PackageUnitId == packageSns.FirstOrDefault().UnitId);

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
                {
                    isNext = true;
                }
            }
            if (IsPackaged(packageSns, rules))
            {
                isPackInfo.CanPackage = false;
                isPackInfo.Message = "存在已打包的数据，请刷新界面！".L10N();
                return isPackInfo;
            }
            if (nextRule == null)
            {
                isPackInfo.CanPackage = false;
                isPackInfo.Message = "找不到包装单位[{0}]对应的下一层级".L10nFormat(curRule.PackageUnit.Name);
                return isPackInfo;
            }
            if (packageSns.Count > nextRule.LevelQty)
            {
                isPackInfo.CanPackage = false;
                isPackInfo.Message = "最多选择[{0}]进行打包，当前选择的数量[{1}]".L10nFormat(nextRule.LevelQty, packageSns.Count);
                return isPackInfo;
            }
            else if (nextRule.LevelQty > packageSns.Count)
            {
                isPackInfo.CanPackage = true;
                isPackInfo.Message = "外包装最大包装数为[{0}]，当前选择包装数为[{1}]，是否生成未满层级包装？".L10nFormat(nextRule.LevelQty, packageSns.Count);
                return isPackInfo;
            }
            else
            {
                isPackInfo.CanPackage = true;
                isPackInfo.IsFull = true;
                return isPackInfo;
            }
        }

        /// <summary>
        /// 打包
        /// </summary>
        /// <param name="packageSns"></param>
        /// <param name="resourceId"></param>
        /// <param name="processId"></param>
        /// <param name="stationId"></param>
        /// <returns></returns>
        [ApiService("打包")]
        [return: ApiReturn("打包 IsPackInfo")]
        public virtual PackageHand Package([ApiParameter] List<PackageSn> packageSns,
            [ApiParameter] double resourceId, [ApiParameter] double processId, [ApiParameter] double stationId)
        {
            if (packageSns == null || !packageSns.Any())
            {
                throw new ValidationException("包装数据异常!".L10N());
            }
            var wo = RF.GetById<WorkOrder>(packageSns.FirstOrDefault().WoId);
            var rules = wo.PackageRuleDetailList.OrderBy(p => SortExtension.GetIndex(p)).ToArray();
            var curRule = rules.Find(p => p.PackageUnitId == packageSns.FirstOrDefault().UnitId);
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
                {
                    isNext = true;
                }
            }
            Workcell workcell = new Workcell
            {
                ResourceId = resourceId,
                ProcessId = processId,
                StationId = stationId,
                EmployeeId = RT.IdentityId,
            };
            //执行打包逻辑
            var resultInfo = RT.Service.Resolve<NewPackageController>().DoPackageMuanual(GetPackageSnRecordByIds(packageSns.Select(p => p.Id).ToList()), rules, nextRule, wo.Id, workcell);
            if (!resultInfo.Item1.IsNullOrEmpty())
            {
                throw new ValidationException(resultInfo.Item1);
            }
            var pkgNo = resultInfo.Item2;
            // 重新获取一次包装记录
            var pkgSnRecord = GetPackageSnRecord(resourceId, processId, stationId);
            PackageHand packageHand = new PackageHand
            {
                PkgNo = pkgNo,
                DirectPackageSnRecordList = pkgSnRecord,
            };
            return packageHand;
        }

        private EntityList<PackageSnRecord> GetPackageSnRecordByIds(List<double> recordIds)
        {
            var packageSnRecordList = recordIds.SplitContains(tempIds =>
            {
                return Query<PackageSnRecord>().Where(p => tempIds.Contains(p.Id)).ToList();
            });
            return packageSnRecordList;
        }

        /// <summary>
        /// 验证是否已打包(防止双端操作)
        /// </summary>
        /// <param name="packageSns"></param>
        /// <param name="rules"></param>
        /// <returns></returns>
        private bool IsPackaged(List<PackageSn> packageSns, WorkOrderPackageRuleDetail[] rules)
        {
            // 主单位包装验证(还未生成包装关系)
            if (packageSns.Exists(p => p.UnitId == rules[0].Id))
            {
                return MainUnitPacked(packageSns);
            }
            // 非主单位包装验证
            else
            {
                return UnMainUnitPacked(packageSns);
            }
        }

        /// <summary>
        /// 主单位验证是否打包
        /// </summary>
        /// <param name="packageSns"></param>
        /// <returns></returns>
        private bool MainUnitPacked(List<PackageSn> packageSns)
        {
            var packageNoList = packageSns.Select(p => p.Sn).ToList();
            var itemLabelList = packageNoList.SplitContains(tempLabels =>
            {
                return Query<ItemLabel>().Where(p => tempLabels.Contains(p.Label)).ToList();
            });
            var relationIds = itemLabelList.Select(p => p.RelationId).ToList();
            var relationList = relationIds.SplitContains(tempIds =>
            {
                return Query<PackingRelation>().Where(p => tempIds.Contains(p.Id)).ToList();
            });
            return relationList.Any(p => p.IsPacked);
        }

        private bool UnMainUnitPacked(List<PackageSn> packageSns)
        {
            var packageNoList = packageSns.Select(p => p.Sn).ToList();
            var relationList = packageNoList.SplitContains(tempNos =>
            {
                return Query<PackingRelation>().Where(p => tempNos.Contains(p.PackageNo)).ToList();
            });
            return relationList.Any(p => p.RootId != p.Id);
        }
    }
}
