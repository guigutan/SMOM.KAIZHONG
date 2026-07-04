using SIE.Domain;
using SIE.Packages.Packages;
using SIE.Packages.Packings.Strategys;
using SIE.Packages.Packings;
using SIE.Packages;
using System;
using System.Collections.Generic;
using System.Text;
using SIE.MES.WorkOrders;
using SIE.Domain.Validation;
using SIE.Packages.ItemLabels;
using System.Linq;
using SIE.Common.Sort;
using SIE.MES.WIP.Packings.Configs;
using SIE.Packages.Packings.Enums;
using SIE.Tech.Processs;
using SIE.Common.NumberRules;
using SIE.MES.PackingPrints;
using SIE.Core.Barcodes;
using SIE.Barcodes;
using SIE.Common;
using SIE.MES.WIP.NewPackages;
using System.Security.Cryptography;
using SIE.MES.WIP.Products;
using SIE.EventMessages.MES.WIP;
using System.Threading.Tasks;
using SIE.Tech.Stations.Configs;

namespace SIE.MES.WIP.Packings
{
    /// <summary>
    /// 直接包装采集
    /// </summary>
    public class DirectPackingController : WipController
    {
        /// <summary>
        /// 包装包装数量是否装满
        /// </summary>
        /// <param name="packingRelation">包装关系</param>
        /// <param name="workOrder">工单</param>
        /// <returns>bool</returns>
        public virtual bool IsPackingRelationPackageQtyFull(PackingRelation packingRelation, WorkOrder workOrder)
        {
            if (packingRelation == null)
                throw new ArgumentNullException(nameof(packingRelation));
            if (workOrder == null)
                throw new ArgumentNullException(nameof(workOrder));
            var outerPackageRuleDetail = RT.Service.Resolve<WipPackingController>().GetPackageRuleDetail(packingRelation.PackageUnit, workOrder);
            var innerPackageDetail = RT.Service.Resolve<WipPackingController>().GetInnerPackageRuleDetail(packingRelation.PackageUnit, workOrder);
            return packingRelation.PackedQty == (outerPackageRuleDetail.Qty / innerPackageDetail.Qty);
        }

        /// <summary>
        /// 是否满足整个包装规则
        /// </summary>
        /// <param name="relationId">包装关系ID</param>
        /// <param name="workOrder">工单</param>
        /// <returns>bool</returns>
        private bool IsPackageRuleFull(double relationId, WorkOrder workOrder)
        {
            if (workOrder == null)
            {
                throw new ArgumentNullException(nameof(workOrder));
            }

            var topRule = RT.Service.Resolve<WipPackingController>().GetTopOuterPackageRuleDetail(workOrder);
            var topPkg = RF.GetById<PackingRelation>(relationId);
            return topRule.PackageUnitId == topPkg.PackageUnitId && topRule.Qty == topPkg.ItemQty;
        }

        /// <summary>
        /// 包装物料数量是否装满
        /// </summary>
        /// <param name="packingRelation">包装关系</param>
        /// <param name="workOrder">工单</param>
        /// <returns>bool</returns>
        [IgnoreProxy]
        public virtual bool IsPackingRelationItemQtyFull(PackingRelation packingRelation, WorkOrder workOrder)
        {
            if (packingRelation == null)
            {
                throw new ArgumentNullException(nameof(packingRelation));
            }

            if (workOrder == null)
            {
                throw new ArgumentNullException(nameof(workOrder));
            }

            var outerPackageRuleDetail = RT.Service.Resolve<WipPackingController>().GetPackageRuleDetail(packingRelation.PackageUnit, workOrder);
            return packingRelation.ItemQty == outerPackageRuleDetail.Qty;
        }

        /// <summary>
        /// 创建包装Event
        /// </summary>
        /// <param name="group"></param>
        /// <param name="scanModel">扫描模式</param>
        /// <param name="collectBarcode"></param>
        /// <param name="relation"></param>
        /// <param name="workOrder"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public virtual NewPackingStrategyEvent CreatePackingEvent(string group, ScanMode scanModel, string collectBarcode, PackingRelation relation, WorkOrder workOrder, PackingUnit unit)
        {
            var packingEvent = new NewPackingStrategyEvent();
            packingEvent.Group = group;
            packingEvent.StrategyType = scanModel == ScanMode.Normal ? ScanStrategyMode.ScanSingle : ScanStrategyMode.ScanOneJoinToMany;
            packingEvent.CollectBarcode = collectBarcode;
            packingEvent.IsPackageItemFull = IsPackingRelationItemQtyFull(relation, workOrder);
            packingEvent.IsPackageQtyFull = IsPackingRelationPackageQtyFull(relation, workOrder);
            packingEvent.IsPackageRuleFull = IsPackageRuleFull(relation.Id, workOrder);

            //重新取一次，是为了给视图属性设置上值（如包装单位名称，前端提示时需要用到）
            packingEvent.OuterPackingRelation = RF.GetById<PackingRelation>(relation.Id,
                new EagerLoadOptions().LoadWithViewProperty());
            return packingEvent;
        }

        /// <summary>
        /// 包装采集过站
        /// </summary>
        /// <param name="barcode">采集的条码</param>
        /// <param name="collectData">采集数据</param>
        /// <param name="workcell">工作单元</param>
        /// <param name="needMove">是否需要过站</param>
        /// <returns>最外层的包装（关系）</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ValidationException"></exception>

        public virtual PackingRelation PackingCollect(string barcode, CollectData collectData, Workcell workcell, bool needMove)
        {
            if (string.IsNullOrEmpty(barcode))
                throw new ArgumentNullException(nameof(barcode));

            if (collectData == null)
                throw new ArgumentNullException(nameof(collectData));

            if (workcell == null)
                throw new ArgumentNullException(nameof(workcell));

            if (collectData.PackingData == null)
            {
                throw new ValidationException("采集失败，包装数据为空".L10N());
            }

            var isExsitedPackingRelation = Query<ItemLabel>().Where(m => m.Label== barcode && m.RelationId > 0).FirstOrDefault();
            if (isExsitedPackingRelation != null)
            {
                throw new ValidationException("条码已包装，请检查".L10N());
            }

            PackingRelation resultPackingRelation = null;

            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                //处理包装关系，不进行级联打包
                resultPackingRelation = ProcessPackageRelation(barcode, collectData, workcell);
                tran.Complete();
            }
            //当前处理的包装关系
            return resultPackingRelation;
        }



        /// <summary>
        /// 处理包装关系
        /// </summary>
        /// <param name="barcode">包装号或生产条码</param>        
        /// <param name="collectData">采集数据</param>
        /// <param name="workcell">工作单元</param>
        /// <exception cref="ValidationException"></exception>
        private PackingRelation ProcessPackageRelation(string barcode, CollectData collectData, Workcell workcell)
        {
            var packingMode = collectData.PackingData.PackingMode;
            var currentPackingUnit = collectData.PackingData.CurrentPackingUnit;

            var printMode = collectData.PackingData.PrintMode;
            var scannedPackageNos = collectData.PackingData.ScannedPackageNos;

            //当前处理的包装关系，即当前包装的条码的上一层包装关系
            var currentPackingRelationId = collectData.PackingData.OuterPackingRelationId;

            //最后包装的包装（关系）
            PackingRelation packingRelation = null;

            if (currentPackingUnit == null || currentPackingUnit.IsMasterUnit)
            {
                //单体条码对应的物料标签，工序勾选了Create SKU，过站时会生成
                var itemLabel = RT.Service.Resolve<ItemLabelController>().GetItemLabel(barcode);

                if (itemLabel == null)
                    throw new ValidationException("未找到物料标签[{0}]，请检查是否生成SKU".L10nFormat(barcode));

                if (itemLabel.Relation != null)
                    throw new ValidationException("包装失败，条码[{0}]已被打包".L10nFormat(barcode));

                //获取工单
                var workOrder = RF.GetById<WorkOrderMove>(itemLabel.WorkOrderId);

                //工单的包装规则列表
                List<WorkOrderPackageRuleDetail> woRuleDtls
                    = GetWorkOrderPackageRuleDetails(workOrder.Id, new EagerLoadOptions().LoadWithViewProperty());

                if (!woRuleDtls.Any())
                {
                    throw new ValidationException("工单[{0}]未维护包装规则明细，请检查".L10nFormat(workOrder.No));
                }

                //这里是取【主单位】的上一层包装规则
                var upLevelPackageRuleDetail = GetUpLevelPackingRule(woRuleDtls, workOrder.No, currentPackingUnit);
                collectData.Context["TopUnitId"] = upLevelPackageRuleDetail.PackageUnitId;
                //打包
                packingRelation = ItemLabelPacking(workcell, packingMode, upLevelPackageRuleDetail.PackageUnit, printMode, scannedPackageNos,
                    currentPackingRelationId, workOrder, woRuleDtls, itemLabel, upLevelPackageRuleDetail);

            }
            else
            {
                //获取包装的包装关系
                var innerRelation = GetPackingRelation(barcode, new EagerLoadOptions().LoadWithViewProperty());

                if (innerRelation == null)
                    throw new ValidationException("{0}不存在".L10nFormat(currentPackingUnit.Name));

                if (innerRelation.TreePId.HasValue)
                    throw new ValidationException("包装失败，{0}[{1}]已被打包".L10nFormat(innerRelation.PackageUnitName, barcode));

                //获取包装下的物料标签（生产条码在CreateSKU工序过站时会产生物料标签）
                var itemLabels = RT.Service.Resolve<ItemLabelController>()
                   .GetRootPackingRelationItemLabels(innerRelation.Id);

                if (!itemLabels.Any())
                {
                    throw new ValidationException("找不到包装号【{0}】的生产条码".L10nFormat(barcode));
                }

                var itemLabel = itemLabels.FirstOrDefault();

                //获取工单
                var workOrder = RF.GetById<WorkOrderMove>(itemLabel.WorkOrderId);

                //工单的包装规则列表
                List<WorkOrderPackageRuleDetail> woRuleDtls
                    = GetWorkOrderPackageRuleDetails(workOrder.Id, new EagerLoadOptions().LoadWithViewProperty());

                if (!woRuleDtls.Any())
                {
                    throw new ValidationException("工单[{0}]未维护包装规则明细，请检查".L10nFormat(workOrder.No));
                }

                //上一层的包装规则
                var upLevelPackageRuleDetail = GetUpLevelPackingRule(woRuleDtls, workOrder.No, innerRelation.PackageUnit);
                collectData.Context["TopUnitId"] = upLevelPackageRuleDetail.PackageUnitId;
                //将下层包装进行打包
                packingRelation = RelationPacking(workcell, packingMode, printMode, scannedPackageNos, currentPackingRelationId,
                    workOrder, woRuleDtls, innerRelation, upLevelPackageRuleDetail);
            }

            return packingRelation;
        }



        /// <summary>
        /// 将包装进行打包
        /// </summary>
        /// <param name="workcell">工作单元信息</param>
        /// <param name="autoDoPackingMode">包装模式(手工扫码/自动打包/自动级联打包)</param>        
        /// <param name="printMode">包装号打印模式(在线/提前)</param>
        /// <param name="scannedPackageNos">已采集的包装号</param>
        /// <param name="outerPackingRelationId">当前处理的包装关系（当前的上一层包装）</param>
        /// <param name="workOrder">工单</param>
        /// <param name="woRuleDtls">工单的包装规则明细</param>
        /// <param name="innerRelation">要包装的内包装</param>
        /// <param name="upLevelPackageRuleDetail">上一层包装规则</param>
        /// <returns></returns>
        private PackingRelation RelationPacking(Workcell workcell, AutoDoPackingMode autoDoPackingMode, PrintMode printMode, Queue<string> scannedPackageNos, double? outerPackingRelationId, WorkOrderMove workOrder, List<WorkOrderPackageRuleDetail> woRuleDtls, BatchPackingRelation innerRelation, WorkOrderPackageRuleDetail upLevelPackageRuleDetail)
        {
            //外层包装
            PackingRelation outerPackageRelation;

            //获取【工单】的当前包装工序允许打包的最顶包装
            WorkOrderPackageRuleDetail topRuleDtl = GetProcessTopPackageRuleDetail(workOrder.Id, workcell.ProcessId);

            if (outerPackingRelationId.HasValue)
            {
                //指定外包装，则加入加入到指定包装，满包装打包
                outerPackageRelation = RF.GetById<PackingRelation>(outerPackingRelationId.Value);

                if (outerPackageRelation == null)
                    throw new ValidationException("未找到{0}包装关系".L10nFormat(upLevelPackageRuleDetail.PackageUnitName));

                if (outerPackageRelation.IsPacked)
                    throw new ValidationException("包装失败，{0}[{1}]已打包".L10nFormat(upLevelPackageRuleDetail.PackageUnitName, outerPackageRelation.PackageNo));

                if (outerPackageRelation.PackageUnitId != upLevelPackageRuleDetail.PackageUnitId)
                    throw new ValidationException("包装失败，外层包装必须是[{0}]".L10nFormat(upLevelPackageRuleDetail.PackageUnitName));

                //包装数加1
                outerPackageRelation.PackedQty += 1;

                outerPackageRelation.ItemQty += innerRelation.ItemQty;

                PackagingWhenFullPackage(outerPackageRelation, printMode, scannedPackageNos, upLevelPackageRuleDetail.NumberRule.Id,
                    topRuleDtl);
            }
            else
            {
                //未指定外包装，则生成新的上层包装，满包装打包
                outerPackageRelation = CreateEmptyPackingRelation(workcell, upLevelPackageRuleDetail.PackageUnit, workOrder, 1, innerRelation.ItemQty, upLevelPackageRuleDetail.LevelQty);

                PackagingWhenFullPackage(outerPackageRelation, printMode, scannedPackageNos, upLevelPackageRuleDetail.NumberRule.Id,
                    topRuleDtl);
            }

            //内层包装的TreePId 为 外层包装的Id
            innerRelation.TreePId = outerPackageRelation.Id;

            //内层包装的根ID设置为新外包装的ID
            innerRelation.RootId = outerPackageRelation.Id;

            //更新下一层的Root ID 
            DB.Update<PackingRelation>()
                .Set(x => x.RootId, outerPackageRelation.Id)
                .Where(x => x.RootId == innerRelation.Id && x.Id != innerRelation.Id)
                .Execute();

            RF.Save(outerPackageRelation);
            RF.Save(innerRelation);
            CreatePackDiretSnRecord(workcell, innerRelation, outerPackageRelation, upLevelPackageRuleDetail.PackageUnit, workOrder);
            return outerPackageRelation;

        }

        /// <summary>
        /// 生成非主单位记录
        /// </summary>
        /// <param name="workcell"></param>
        /// <param name="innerRelation"></param>
        /// <param name="packingRelation"></param>
        /// <param name="upLevelPackingUnit"></param>
        /// <param name="workOrder"></param>
        public virtual void CreatePackDiretSnRecord(Workcell workcell, BatchPackingRelation innerRelation, PackingRelation packingRelation, PackingUnit upLevelPackingUnit, WorkOrderMove workOrder)
        {
            // 本次包装的包装号
            var packing = Query<DirectPackageSnRecord>().Where(p => p.Sn == innerRelation.PackageNo).FirstOrDefault();
            if (packing != null)
            {
                packing.PersistenceStatus = PersistenceStatus.Deleted;
                var directPackageSnRecord = Query<DirectPackageSnRecord>().Where(p => p.PackRelationId == packingRelation.Id).FirstOrDefault();
                if (directPackageSnRecord != null)
                {
                    directPackageSnRecord.Sn = packingRelation.PackageNo;
                    directPackageSnRecord.WoSn += "," + packing.WoSn;
                    directPackageSnRecord.PackedQty = packingRelation.PackedQty;
                    RF.Save(directPackageSnRecord);
                }
                else
                {
                    DirectPackageSnRecord newdirectPackageSnRecord = new DirectPackageSnRecord
                    {
                        Sn = packingRelation.PackageNo,
                        WorkOrderId = packingRelation.WorkOrderId,
                        PackRelationId = packingRelation.Id,
                        Product = workOrder.Product,
                        PackageUnit = upLevelPackingUnit,
                        WoSn = packing.WoSn,
                        ResourceId = workcell.ResourceId,
                        ProcessId = workcell.ProcessId,
                        StationId = workcell.StationId,
                        PackedQty = packingRelation.PackedQty,
                    };
                    RF.Save(newdirectPackageSnRecord);
                }
                RF.Save(packing);
            }
            else
            {
                //
            }
        }

        /// <summary>
        /// 创建空白包装关系
        /// </summary>
        /// <param name="workcell">工作单元信息</param>
        /// <param name="upLevelPackingUnit">上一层的包装单位</param>
        /// <param name="workOrder">工单</param>
        /// <param name="packedQty">已加入的包装数量</param>
        /// <param name="itemQty">已加入的主单位数量（物料标签）</param>
        /// <param name="levelQty">满包装包装数量</param>
        /// <returns></returns>
        private PackingRelation CreateEmptyPackingRelation(Workcell workcell, PackingUnit upLevelPackingUnit, WorkOrderMove workOrder,
            decimal packedQty, decimal itemQty, decimal levelQty)
        {
            var date = RF.Find<PackingRelation>().GetDbTime();

            var relation = new PackingRelation()
            {
                //包装号增加了提前打印模式，所以这里不生成
                //PackageNo = packageNo,
                PackageUnit = upLevelPackingUnit,
                PackedDate = date,
                PackingBy = RT.IdentityId,
                ProcessId = workcell.ProcessId,
                StationId = workcell.StationId,
                IsProcessFinish = false,
                WorkOrderId = workOrder.Id,
                State = LogisticState.UnPrinted,
                PackedQty = packedQty,
                ItemQty = itemQty,
                FullPackedQty = levelQty,
            };

            relation.GenerateId();
            relation.RootId = relation.Id;

            return relation;
        }

        /// <summary>
        /// 获取包装关系
        /// </summary>
        /// <param name="packageNo">包装号</param>
        /// <param name="eagerLoad">贪婪加载</param>
        /// <returns></returns>
        public virtual BatchPackingRelation GetPackingRelation(string packageNo, EagerLoadOptions eagerLoad = null)
        {
            if (packageNo.IsNullOrEmpty())
                throw new ValidationException("包装号不能为空".L10N());
            return Query<BatchPackingRelation>().Where(p => p.PackageNo == packageNo).FirstOrDefault(eagerLoad);
        }

        /// <summary>
        /// 获取指定工序的工单包装规则明细
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="processId">工序ID</param>
        /// <returns></returns>
        private EntityList<WorkOrderPackageRuleDetail> GetWoPackageRuleDetailOfProcess(double workOrderId, double processId)
        {
            return Query<WorkOrderPackageRuleDetail>()
                                .Exists<WorkOrderProcessPackingUnit>((x, y) => y.Where(z => z.PackageRuleId == x.Id && z.ProcessId == processId))
                                .Where(p => p.WorkOrderId == workOrderId)
                                .ToList();
        }

        /// <summary>
        /// 是否多包装采集
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <returns>多包装采集返回true，否则返回false</returns>
        private bool IsMultPacking(double workOrderId)
        {
            return Query<WorkOrderRoutingProcess>().Where(p => (p.ProcessType == Tech.Processs.ProcessType.Packing || p.ProcessType == Tech.Processs.ProcessType.BatchPacking) && p.WorkOrderId == workOrderId).Count() > 1;
        }

        /// <summary>
        /// 获取工序允许包装的最顶层包装关系明细
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <param name="processId"></param>
        /// <returns></returns>
        public virtual WorkOrderPackageRuleDetail GetProcessTopPackageRuleDetail(double workOrderId, double processId)
        {
            var isMultPacking = IsMultPacking(workOrderId);
            if (isMultPacking)
            {
                var process = RF.GetById<Process>(processId);

                if (process == null)
                {
                    throw new ValidationException("工序不能为空".L10N());
                }

                if (process.Type != ProcessType.Packing && process.Type != ProcessType.BatchPacking)
                {
                    throw new ValidationException("工序[{0}]非包装工序".L10nFormat(process.Name));
                }

                EntityList<WorkOrderPackageRuleDetail> workOrderPackageRuleDetails = GetWoPackageRuleDetailOfProcess(workOrderId, processId);

                if (workOrderPackageRuleDetails.Any())
                {
                    return workOrderPackageRuleDetails.OrderByDescending(p => p.GetIndex()).FirstOrDefault();
                }
            }

            return GetWorkOrderTopPackageRuleDetail(workOrderId);
        }

        /// <summary>
        /// Get work order top package rule detail
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        public virtual WorkOrderPackageRuleDetail GetWorkOrderTopPackageRuleDetail(double workOrderId)
        {
            return Query<WorkOrderPackageRuleDetail>()
                .Where(p => p.WorkOrderId == workOrderId)
                .ToList()
                .OrderByDescending(f => SortExtension.GetIndex(f))
                .FirstOrDefault();
        }

        /// <summary>
        /// 将物料标签（生产条码）打包
        /// </summary>
        /// <param name="workcell">工作单元信息</param>
        /// <param name="packingMode">自动打包方式</param>
        /// <param name="upLevelPackingUnit">上一层包装单位</param>
        /// <param name="printMode">包装号打印模式</param>
        /// <param name="packageNos"></param>
        /// <param name="outerPackingRelationId">当前处理的包装关系，即当前包装的条码的上一层包装关系</param>
        /// <param name="workOrder">工单</param>
        /// <param name="woRuleDtls">工单包装规则明细</param>
        /// <param name="itemLabel">物料标签</param>
        /// <param name="upLevelPackageRuleDetail">上一层包装规则明细</param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        private PackingRelation ItemLabelPacking(Workcell workcell, AutoDoPackingMode packingMode, PackingUnit upLevelPackingUnit,
            PrintMode printMode, Queue<string> packageNos, double? outerPackingRelationId, WorkOrderMove workOrder,
            List<WorkOrderPackageRuleDetail> woRuleDtls, ItemLabel itemLabel, WorkOrderPackageRuleDetail upLevelPackageRuleDetail)
        {
            PackingRelation relation;

            //获取【工单】的当前包装工序允许打包的最顶包装
            WorkOrderPackageRuleDetail topRuleDtl = GetProcessTopPackageRuleDetail(workOrder.Id, workcell.ProcessId);

            if (outerPackingRelationId.HasValue)
            {
                //有正在处理的包装关系
                relation = RF.GetById<PackingRelation>(outerPackingRelationId.Value);

                if (relation == null)
                    throw new ValidationException("未找到{0}包装关系".L10nFormat(upLevelPackageRuleDetail.PackageUnitName));

                if (relation.IsPacked)
                    throw new ValidationException("包装失败，{0}[{1}]已打包".L10nFormat(upLevelPackageRuleDetail.PackageUnitName, relation.PackageNo));

                if (relation.PackageUnitId != upLevelPackageRuleDetail.PackageUnitId)
                    throw new ValidationException("包装失败，外层包装必须是[{0}]".L10nFormat(upLevelPackageRuleDetail.PackageUnitName));

                relation.PackedQty += itemLabel.Qty;
                relation.ItemQty += itemLabel.Qty;
                itemLabel.Relation = relation;

                PackagingWhenFullPackage(relation, printMode, packageNos, upLevelPackageRuleDetail.NumberRule.Id, topRuleDtl);
            }
            else
            {
                //无当前正在处理的包装关系，则建立一个包装关系统
                relation = CreateEmptyPackingRelation(workcell, upLevelPackingUnit, workOrder, itemLabel.Qty, itemLabel.Qty, upLevelPackageRuleDetail.LevelQty);

                PackagingWhenFullPackage(relation, printMode, packageNos, upLevelPackageRuleDetail.NumberRule.Id, topRuleDtl);

                itemLabel.Relation = relation;
            }

            RF.Save(relation);
            RF.Save(itemLabel);
            // 创建记录
            CreateItemLabelDiretSnRecord(workcell, relation, upLevelPackingUnit, workOrder);

            return relation;

        }

        /// <summary>
        /// 创建记录
        /// </summary>
        /// <param name="workcell">工作单元</param>
        /// <param name="packingRelation">当前包装</param>
        /// <param name="upLevelPackingUnit"></param>
        /// <param name="workOrder"></param>
        private void CreateItemLabelDiretSnRecord(Workcell workcell, PackingRelation packingRelation, PackingUnit upLevelPackingUnit, WorkOrderMove workOrder)
        {
            var itemLabelRelation = Query<ItemLabel>().Where(p => p.RelationId == packingRelation.Id).ToList();
            var itemLabelNoList = itemLabelRelation.Select(p => p.Label).ToList();
            var directPackageSnRecord = Query<DirectPackageSnRecord>().Where(p => p.PackRelationId == packingRelation.Id).FirstOrDefault();
            if (directPackageSnRecord != null)
            {
                directPackageSnRecord.Sn = packingRelation.PackageNo;
                directPackageSnRecord.WoSn = string.Join(",", itemLabelNoList);
                directPackageSnRecord.PackedQty = packingRelation.PackedQty;
                RF.Save(directPackageSnRecord);
            }
            else
            {
                DirectPackageSnRecord newdirectPackageSnRecord = new DirectPackageSnRecord
                {
                    Sn = packingRelation.PackageNo,
                    WorkOrderId = packingRelation.WorkOrderId,
                    PackRelationId = packingRelation.Id,
                    Product = workOrder.Product,
                    PackageUnit = upLevelPackingUnit,
                    WoSn = string.Join(",", itemLabelNoList),
                    ResourceId = workcell.ResourceId,
                    ProcessId = workcell.ProcessId,
                    StationId = workcell.StationId,
                    PackedQty = packingRelation.PackedQty,
                };
                RF.Save(newdirectPackageSnRecord);
            }
        }

        /// <summary>
        /// Get workorder package rule details
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <param name="eagerLoad"></param>
        /// <returns></returns>
        public virtual List<WorkOrderPackageRuleDetail> GetWorkOrderPackageRuleDetails(double workOrderId, EagerLoadOptions eagerLoad = null)
        {
            return Query<WorkOrderPackageRuleDetail>().Where(p => p.WorkOrderId == workOrderId).ToList(eagerLoad: eagerLoad).OrderBy(p => SortExtension.GetIndex(p)).ToList();
        }

        /// <summary>
        /// 获取外层包装规则明细
        /// </summary>
        /// <param name="woRuleDtls">工单包装规则明细列表</param>
        /// <param name="workOrderNo">工单号</param>
        /// <param name="packingUnit">内层包装单位</param>
        /// <param name="throwExc">未找到外层包装规则是否抛异常</param>
        /// <returns>外层包装规则明细</returns>
        public virtual WorkOrderPackageRuleDetail GetUpLevelPackingRule(List<WorkOrderPackageRuleDetail> woRuleDtls, string workOrderNo,
            PackingUnit packingUnit, bool throwExc = true)
        {
            WorkOrderPackageRuleDetail currentRuleDtl;
            if (packingUnit == null)
            {
                //工单下是【主单位】的包装规则明细
                currentRuleDtl = woRuleDtls.FirstOrDefault(p => p.PackageUnit.IsMasterUnit);

                if (currentRuleDtl == null)
                {
                    throw new ValidationException("未找到工单[{0}]的为【{1}】包装规则明细".L10nFormat(workOrderNo, "主单位"));
                }
            }
            else
            {
                currentRuleDtl = woRuleDtls.FirstOrDefault(p => p.PackageUnitId == packingUnit.Id);

                if (currentRuleDtl == null)
                {
                    throw new ValidationException("未找到工单[{0}]的{1}包装明细".L10nFormat(workOrderNo, packingUnit.Name));
                }
            }

            //上一层的包装规则
            var upLeverPackageRuleDtl = woRuleDtls
                .Where(p => SortExtension.GetIndex(p) > SortExtension.GetIndex(currentRuleDtl))
                .OrderBy(p => SortExtension.GetIndex(p))
                .FirstOrDefault();

            if (upLeverPackageRuleDtl == null && throwExc)
            {
                throw new ValidationException("未找到工单【{0}】的【{1}】的上层包装明细"
                    .L10nFormat(workOrderNo, currentRuleDtl.PackageUnitName));
            }

            return upLeverPackageRuleDtl;
        }

        /// <summary>
        /// 满包时将包装关系打包，生成包装号或设置为采集的包装号
        /// </summary>
        /// <param name="relation">要处理的包装关系</param>
        /// <param name="printMode">包装号打印模式（在线/提前）</param>
        /// <param name="scannedPackageNos">采集的包装号</param>
        /// <param name="numberRuleId">生成包装号的编码规则</param>
        /// <param name="topRuleDtl">工单在当前工序允许的最顶层包装规则明细</param>        
        /// <returns></returns>
        private void PackagingWhenFullPackage(PackingRelation relation, PrintMode printMode, Queue<string> scannedPackageNos, double numberRuleId, WorkOrderPackageRuleDetail topRuleDtl)
        {
            if (relation.IsFullPackage)
            {
                if (printMode == PrintMode.InAdvance)
                {
                    //预包装，计算需要手动输入外包箱数量
                    if (!scannedPackageNos.Any())
                    {
                        throw new ValidationException("包装号不能为空".L10N());
                    }

                    string packageNo = scannedPackageNos.Dequeue();
                    relation.PackageNo = packageNo;

                    //提前打印更新包装号状态 TO_DO:
                    DB.Update<PackingBarcode>()
                        .Set(x => x.IsUse, true)
                        .Where(x => x.Code == packageNo)
                        .Execute();
                }
                else
                {
                    string packageNo = RT.Service.Resolve<NumberRuleController>().GenerateSegment(numberRuleId, 1).FirstOrDefault();
                    relation.PackageNo = packageNo;
                }

                //达到工序允许的顶层包装关系
                if (relation.PackageUnitId == topRuleDtl.PackageUnitId)
                {
                    relation.IsProcessFinish = true;
                }
            }
        }

        /// <summary>
        /// 验证：
        /// 1.产品工艺路线。
        /// 2.工单状态。
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="workcell">工作单元</param>
        /// <returns>返回产品信息</returns> 
        /// <exception cref="ArgumentNullException">采集条码为空、工作单元为空</exception>
        public override ProductInfo Validate(CollectBarcode barcode, Workcell workcell)
        {
            Check.NotNull(barcode, nameof(barcode));
            Check.NotNull(workcell, nameof(workcell));

            //Tuple<单体条码列表，工单，当前处理的包装单位，需要采集的包装单位列表，是否过站>
            Tuple<List<string>, Core.WorkOrders.WorkOrder, PackingUnit, PackingUnit, Queue<PackingUnit>, bool> result
                = ValidatePackingBarcode(barcode, workcell);

            //单体条码列表
            List<string> snList = result.Item1;

            //工单
            Core.WorkOrders.WorkOrder workOrder = result.Item2;

            //是否需要过站
            bool needMove = result.Item6;

            //需要过站，则进行过站前验证
            if (needMove)
            {
                using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
                {
                    for (int i = 0; i < snList.Count; i++)
                    {
                        var sn = snList[i];

                        var collectBarcode = new CollectBarcode(sn, BarcodeType.SN);

                        // 循环验证单体条码能否在指定工序中过站
                        var product = ValidateProduct(collectBarcode, workcell);

                        if (i > 0)
                        {
                            continue;
                        }

                        //验证工单信息
                        ValidateWorkOrder(product);
                    }

                    tran.Complete();
                }

            }
            var info = new ProductInfo()
            {
                WorkOrderId = workOrder.Id,
                ItemId = workOrder.ProductId
            };

            //当前包装的包装单位
            PackingUnit currentPackingUnit = result.Item3;
            info.Context["PackingUnit"] = currentPackingUnit;
            // 上一层包装单位
            PackingUnit outerPackingUnit = result.Item4;
            info.Context["OuterPackingUnit"] = outerPackingUnit;
            // 要采集 包装单位 清单
            Queue<PackingUnit> toScanPackageUnits = result.Item5;
            info.Context["ToScanPackageUnits"] = toScanPackageUnits;

            //是否需要过站
            info.Context["NeedMove"] = needMove;

            return info;
        }

        /// <summary>
        /// 验证包装条码
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="workcell">工作单元</param>
        /// <returns>验证结果</returns>
        private Tuple<List<string>, Core.WorkOrders.WorkOrder, PackingUnit, PackingUnit, Queue<PackingUnit>, bool> ValidatePackingBarcode(
            CollectBarcode barcode, Workcell workcell)
        {
            //包装号打印模式 => 提前打印/在线打印
            //包装号打印模式 => 提前打印/在线打印
            var printMode = PrintMode.Online;
            if (barcode.Context["PrintMode"] != null)
            {
                switch (barcode.Context["PrintMode"].ToString().ToLower())
                {
                    case "online":
                        printMode = PrintMode.Online;
                        break;
                    case "inadvance":
                        printMode = PrintMode.InAdvance;
                        break;
                    default:
                        printMode = (PrintMode)Convert.ToInt32(barcode.Context["PrintMode"].ToString());//此方式兼容信创
                        break;
                }
            }
            //自动打包方式 => 手工扫码/自动打包/自动级联打包

            AutoDoPackingMode autoDoPackingMode = AutoDoPackingMode.AutoPacking;
            if (barcode.Context["AutoDoPackingMode"] != null)
            {
                switch (barcode.Context["AutoDoPackingMode"].ToString().ToLower())
                {
                    case "autopacking":
                        autoDoPackingMode = AutoDoPackingMode.AutoPacking;
                        break;
                    case "autocasepacking":
                        autoDoPackingMode = AutoDoPackingMode.AutoCasePacking;
                        break;
                    default:
                        autoDoPackingMode = (AutoDoPackingMode)Convert.ToInt32(barcode.Context["AutoDoPackingMode"].ToString());//此方式兼容信创
                        break;
                }
            }

            //生产条码 列表
            List<string> snList = new List<string>();
            Core.WorkOrders.WorkOrder workOrder = null;

            // Tuple<当前包装的包装单位，最上层包装单位，是否需要过站>
            Tuple<PackingUnit, PackingUnit, PackingUnit, bool> packingUnits;

            //用当前采集的条码去查询单体条码
            var sn = RT.Service.Resolve<BarcodeController>().GetBarcode(barcode.Code);
            if (sn != null)
            {// 验证条码是否已完工
                var wipVersion = GetWipProductVersionByCode(sn.Sn);
                // 验证条码是否入库
                var isBarcodeDetail = RT.Service.Resolve<IDirectPackage>().BarcodeIsDetail(sn.Sn, sn.WorkOrderId);
                if (wipVersion != null)
                {
                    if (!wipVersion.IsFinish)
                    {
                        throw new ValidationException("当前条码[{0}]未完工下线".L10nFormat(barcode));
                    }
                }
                else
                {
                    throw new ValidationException("当前条码[{0}]没有过站记录".L10nFormat(barcode));
                }
                if (isBarcodeDetail)
                {
                    throw new ValidationException("当前条码[{0}]已成品入库，不允许包装！".L10nFormat(barcode));
                }

                //当前采集的是单体条码
                if (sn.IsScraped)
                {
                    throw new ValidationException("[{0}]已经报废".L10nFormat(barcode));
                }

                if (sn.IsPending)
                {
                    throw new ValidationException("[{0}]已经挂起".L10nFormat(barcode));
                }
                snList.Add(sn.Sn);

                workOrder = sn.WorkOrder;

                //当前处理的包装单位ID
                double? currentPackageUnitId = null;

                packingUnits = ValidateMultPackingProcess(workOrder.Id, workcell.ProcessId, true, currentPackageUnitId);
            }
            else
            {
                //获取包装号对应的包装关系
                var packingRelation = RT.Service.Resolve<PackingRelationController>().GetBatchPackingRelation(barcode.Code, false);

                if (packingRelation == null)
                {
                    throw new ValidationException("采集失败,无此包装号:[{0}]".L10nFormat(barcode.Code));
                }

                if (packingRelation.TreePId != null)
                {
                    throw new ValidationException("包装失败，{0}[{1}]已经被包装。"
                        .L10nFormat(packingRelation.PackageUnitName, packingRelation.PackageNo));
                }

                var itemLabels = RT.Service.Resolve<ItemLabelController>()
                    .GetRootPackingRelationItemLabels(packingRelation.Id);

                if (!itemLabels.Any())
                {
                    throw new ValidationException("找不到包装号【{0}】的生产条码".L10nFormat(barcode.Code));
                }

                var itemLabel = itemLabels.FirstOrDefault();
                snList.AddRange(itemLabels.Select(p => p.Label));
                workOrder = itemLabel.WorkOrder;

                packingUnits = ValidateMultPackingProcess(workOrder.Id, workcell.ProcessId, false,
                    packingRelation.PackageUnitId);
            }

            //当前包装单位，单体条码的当前包装单位为主单位
            PackingUnit currentPackingUnint = packingUnits.Item1;
            // 当前包装单位的上一层包装单位
            PackingUnit outerPackingUnit = packingUnits.Item2;
            //当前工序能采集的最上层的包装单位
            PackingUnit topLevelPackingUnit = packingUnits.Item3;

            //需要提前扫描的上级包装单位队列
            Queue<PackingUnit> upLevelPackageUnits = new Queue<PackingUnit>();

            // 包装号打印模式为【提前打印】
            // 计算要采集的上级包装单位
            if (printMode == PrintMode.InAdvance)
            {
                //当前在处理的包装关系
                var currentPackingRelation = (PackingRelation)barcode.Context["OuterPackingRelation"];

                //计算要采集的包装号的类型（函数中有递归逻辑）
                upLevelPackageUnits = CalculateUpLevelPackageUnit(workOrder, workcell, currentPackingRelation, autoDoPackingMode,
                    currentPackingUnint, topLevelPackingUnit);
            }

            //是否过站
            var needMove = packingUnits.Item4;

            return Tuple.Create(snList, workOrder, currentPackingUnint, outerPackingUnit, upLevelPackageUnits, needMove);
        }

        /// <summary>
        /// 获取已打包且未关联上层包装的同一包装单位的包装关系
        /// </summary>
        /// <param name="workOrderId">工单</param>
        /// <param name="processId">工序</param>
        /// <param name="stationId">工位</param>
        /// <param name="packageUnitId">包装单位ID</param>
        /// <returns>包装关系列表</returns>
        private EntityList<PackingRelation> GetCanPackingRelations(double workOrderId, double processId, double stationId, double packageUnitId)
        {
            return Query<PackingRelation>()
                .Where(p => p.WorkOrderId == workOrderId && p.ProcessId == processId && p.StationId == stationId)
                .Where(p => p.PackageUnitId == packageUnitId && p.PackageNo != null && p.TreePId == null)
                .ToList();
        }

        /// <summary>
        /// 递归计算【需扫描的上层包装单位】
        /// </summary>
        /// <param name="workOrder">工单</param>
        /// <param name="workcell">工单单元</param>
        /// <param name="packageUnit">包装单位</param>
        /// <param name="upLevelPackageUnits">【需扫描的上层包装单位】</param>
        /// <param name="woRuleDtls">工单包装规则明细</param>
        /// <param name="maxPackageUnitId">本工序最上层可以包装的包装单位</param>        
        private void ComputeUpLevelPackageUnit(Core.WorkOrders.WorkOrder workOrder, Workcell workcell, PackingUnit packageUnit,
            Queue<PackingUnit> upLevelPackageUnits, List<WorkOrderPackageRuleDetail> woRuleDtls, double maxPackageUnitId)
        {
            //按CalculateUpLevelPackageUnit方法中的假设：第一次进这个方式时，当前包装单位（packageUnit）是 【箱】
            var relations = GetCanPackingRelations(workOrder.Id, workcell.ProcessId, workcell.StationId, packageUnit.Id);

            //按CalculateUpLevelPackageUnit方法中的假设：第一次进这个方式时，上一层包装规则（upLevelPackageRuleDtl）包装单位【托】
            var upLevelPackageRuleDtl = GetUpLevelPackingRule(woRuleDtls, workOrder.No, packageUnit, false);

            //当前工位存在多个没有打包的【包装单位】的包装，报错提示先手动打包完成
            if (relations.Count > upLevelPackageRuleDtl.LevelQty)
            {
                throw new ValidationException("当前工位存在超过{0}【{1}】未打包，请先手动打包。"
                    .L10nFormat(upLevelPackageRuleDtl.LevelQty, packageUnit.Name));
            }

            //按CalculateUpLevelPackageUnit方法中的假设：待打包的包装【箱】加上待包装的【箱】，达到【托】中的包装单位数时            
            if (relations.Count + 1 == upLevelPackageRuleDtl.LevelQty)
            {
                //则待采集的包装单位队列中，加上【托】
                upLevelPackageUnits.Enqueue(upLevelPackageRuleDtl.PackageUnit);

                //如果上层包装单位与当前工序最大可以包装的包装单位相同，则返回
                if (upLevelPackageRuleDtl.PackageUnitId == maxPackageUnitId)
                {
                    return;
                }
                else
                {
                    //否则递归计算
                    ComputeUpLevelPackageUnit(workOrder, workcell, upLevelPackageRuleDtl.PackageUnit, upLevelPackageUnits,
                        woRuleDtls, maxPackageUnitId);
                }
            }
        }

        /// <summary>
        /// 计算需要采集的上层包装单位
        /// </summary>
        /// <param name="workOrder">工单</param>
        /// <param name="workcell">工单单元</param>
        /// <param name="currentPackingRelation">当前处理的包装关系</param>
        /// <param name="autoDoPackingMode">包装模式（手工扫码/自动打包/自动级联打包）</param>
        /// <param name="currentPackageUnit">当前包装单位</param>
        /// <param name="maxPackageUnit">工序允许打包的最大包装单位</param>
        /// <returns></returns>
        public virtual Queue<PackingUnit> CalculateUpLevelPackageUnit(Core.WorkOrders.WorkOrder workOrder, Workcell workcell, PackingRelation currentPackingRelation, AutoDoPackingMode autoDoPackingMode, PackingUnit currentPackageUnit, PackingUnit maxPackageUnit)
        {
            //假设有包装规则如下
            //包装单位 包装数 产品数
            //主单位   1     1
            //箱      2     2
            //托      2     4
            //-----------------------------------
            // 假设：
            // 【当前包装单位】（currentPackageUnit）为 【主单位】
            // 【当前处理的包装关系】（currentPackingRelation）的有值的话，包装单位应该为【箱】
            // 【最大包装单位】没有设备多工序包装的话，应该是【托]

            Queue<PackingUnit> upLevelPackageUnits = new Queue<PackingUnit>();

            //工单钮装规则明细
            var woRuleDtls = GetWorkOrderPackageRuleDetails(workOrder.Id, new EagerLoadOptions().LoadWithViewProperty());

            // 是否抛出异常
            const bool isThrowException = false;

            //获取 当前包装单位 的上一层包装规则
            //按前面的假设 【的上一层包装规则（upLevelPackageRuleDtl）的包装单位应该为【箱】
            var upLevelPackageRuleDtl = GetUpLevelPackingRule(woRuleDtls, workOrder.No, currentPackageUnit, isThrowException);

            if (upLevelPackageRuleDtl == null)
            {
                return upLevelPackageUnits;
            }

            //【上一层工单包装规则】的【包装数】为1
            // 或【当前处理包装关系】不为空，且 与【上一层工单包装规则】的【包装单位】相同 且 【当前处理包装关系】加上当前条码后数量满包装
            // 则添加【上一层工单包装规则】的【包装单位】到待采集包装单位队列中
            if (upLevelPackageRuleDtl.LevelQty == 1
                || (currentPackingRelation != null
                    && currentPackingRelation.PackageUnitId == upLevelPackageRuleDtl.PackageUnitId
                    && currentPackingRelation.PackedQty + 1 == upLevelPackageRuleDtl.LevelQty))
            {
                //当前处理的包装关系，加上当前包装（或单体条码）后满包装，则需要采集上一层的包装号
                //按前面的假设，这里添加的【包装单位】应该为【箱】
                upLevelPackageUnits.Enqueue(upLevelPackageRuleDtl.PackageUnit);
            }
            else
            {
                //【上一层工单包装规则】还不满包装，则返回
                return upLevelPackageUnits;
            }

            //【上一层工单包装规则】的【包装单位】是当前工序可以包装的最上层包装单位，则返回
            if (upLevelPackageRuleDtl.PackageUnitId == maxPackageUnit.Id)
            {
                return upLevelPackageUnits;
            }

            //自动级联打包，则递归计算【需扫描的上层包装单位】
            if (autoDoPackingMode == AutoDoPackingMode.AutoCasePacking)
            {
                ComputeUpLevelPackageUnit(workOrder, workcell, upLevelPackageRuleDtl.PackageUnit,
                    upLevelPackageUnits, woRuleDtls, maxPackageUnit.Id);
            }

            return upLevelPackageUnits;
        }

        /// <summary>
        /// 验证多个包装工序
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="processId">工序ID</param>
        /// <param name="isSn">是否主单位</param>
        /// <param name="packageUnitId">当前条码对应的包装单位，为空表示主单位</param>        
        /// <returns>当前包装单位，上层包装单位，工序允许最大包装单位，是否需要过站</returns>
        private Tuple<PackingUnit, PackingUnit, PackingUnit, bool> ValidateMultPackingProcess(double workOrderId, double processId, bool isSn,
            double? packageUnitId)
        {
            //当前工单是否多个包装工序
            var isMultPackingProcess = IsMultPacking(workOrderId);

            //工单所有包装规则
            var workOrderPackageRuleDetails = Query<WorkOrderPackageRuleDetail>()
                .Where(p => p.WorkOrderId == workOrderId)
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            //当前包装的包装单位对就的工单包装规则
            var currentPackageRuleDetail = workOrderPackageRuleDetails
                .FirstOrDefault(x => (packageUnitId == null && x.IsMasterUnit) || x.PackageUnitId == packageUnitId.Value);
            if (currentPackageRuleDetail == null)
            {
                throw new ValidationException("工单没有包装规则！".L10N());
            }
            //是否过站
            bool needMove = false;

            //当前包装单位是主单位 需要过站
            //if (currentPackageRuleDetail.IsMasterUnit)
            //{
            //    needMove = true;
            //}

            if (isMultPackingProcess)
            {
                //获取工单指定工序的工单包装规则明细列表
                var woProcessPackingRuleDtls = GetWoPackageRuleDetailOfProcess(workOrderId, processId);

                //工序最上层可以包装的工单包装规则明细
                var maxRuleDtl = woProcessPackingRuleDtls.OrderByDescending(p => SortExtension.GetIndex(p)).FirstOrDefault();

                //上层应该包装的工单包装规则明细
                var packingRuleDtl = woProcessPackingRuleDtls
                    .Where(x => SortExtension.GetIndex(x) > SortExtension.GetIndex(currentPackageRuleDetail))
                    .OrderBy(p => SortExtension.GetIndex(p))
                    .FirstOrDefault() ?? throw new ValidationException("工单为多工序包装，当前工序未找到【{0}】的上一层【工单包装规则】，请检查！"
                        .L10nFormat(currentPackageRuleDetail.PackageUnitName));

                //多工序包装，要判断包装的层级是否当前工序最底层包装
                //if (!needMove)
                //{
                //    //当前工序最底层的包装规则
                //    var firstPackingRuleDetail = woProcessPackingRuleDtls.OrderBy(x => SortExtension.GetIndex(x)).FirstOrDefault();

                //    if (packingRuleDtl.PackageUnitId == firstPackingRuleDetail.PackageUnitId)
                //    {
                //        //如果要包装的包装规则是最底层的，则需要过站
                //        needMove = true;
                //    }
                //    //否则不需要过站
                //}

                return Tuple.Create(currentPackageRuleDetail.PackageUnit, packingRuleDtl.PackageUnit, maxRuleDtl.PackageUnit, needMove);
            }
            else
            {
                //上层应该包装的工单包装规则明细
                var packingRuleDtl = workOrderPackageRuleDetails
                    .Where(x => SortExtension.GetIndex(x) > SortExtension.GetIndex(currentPackageRuleDetail))
                    .OrderBy(p => SortExtension.GetIndex(p))
                    .FirstOrDefault() ?? throw new ValidationException("未找到【{0}】的上一层【工单包装规则】，请检查！"
                        .L10nFormat(currentPackageRuleDetail.PackageUnitName));

                //非多工序包装，则只有当前包装单位是主单位要过站                
                var maxRuleDetail = workOrderPackageRuleDetails
                    .OrderByDescending(p => SortExtension.GetIndex(p)).FirstOrDefault();

                return Tuple.Create(currentPackageRuleDetail.PackageUnit, packingRuleDtl.PackageUnit, maxRuleDetail.PackageUnit, needMove);
            }
        }



        /// <summary>
        /// 获取包装记录
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="processId"></param>
        /// <param name="stationId"></param>
        /// <returns></returns>
        public virtual EntityList<DirectPackageSnRecord> GetPackageSnRecords(double resourceId, double processId, double stationId)
        {
            return Query<DirectPackageSnRecord>()
                .Where(p => p.ResourceId == resourceId && p.ProcessId == processId && p.StationId == stationId)
                .OrderBy(p => p.Sn).ToList(null, new EagerLoadOptions().LoadWith(DirectPackageSnRecord.PackageUnitProperty)
                .LoadWith(DirectPackageSnRecord.WorkOrderProperty)
                .LoadWith(DirectPackageSnRecord.ProductProperty)
                .LoadWithViewProperty()
                );
        }

        /// <summary>
        /// 打包
        /// </summary>
        /// <param name="records"></param>
        /// <param name="rules"></param>
        /// <param name="curRule"></param>
        /// <param name="nextRule"></param>
        /// <param name="woId"></param>
        /// <param name="workcell"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual string DoPackageMuanual(EntityList<DirectPackageSnRecord> records, WorkOrderPackageRuleDetail[] rules, WorkOrderPackageRuleDetail curRule, WorkOrderPackageRuleDetail nextRule, double woId, Workcell workcell)
        {
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                var wo = Query<WorkOrder>().Where(p => p.Id == woId).Select(p => new { p.Id, p.No, p.ProductId }).FirstOrDefault<WoData>();
                string pkg = string.Empty;
                var lastRule = rules[rules.Length - 1];
                bool isLast = (lastRule.Id == nextRule.Id);
                PackingRelation relation = null;
                relation = GeneratePackingRelation(nextRule.NumberRuleId.Value, woId, nextRule.PackageUnitId, workcell.ProcessId, workcell.StationId, isLast, records.Count, records.Sum(p => p.ItemQty));

                EntityList<PackingRelation> packageRelations = new EntityList<PackingRelation>();
                packageRelations.Add(relation);
                EntityList<ItemLabel> itemLabels = new EntityList<ItemLabel>();
                if (records.Any(p => p.WoSn != ""))
                {
                    var sns = records.Select(p => p.Sn).Distinct().ToList();
                    packageRelations.AddRange(RT.Service.Resolve<PackingRelationController>().GetAllPackingRelations(sns));
                    var curRelations = packageRelations.Where(p => sns.Contains(p.PackageNo)).ToList();
                    curRelations.ForEach(p =>
                    {
                        p.RootId = relation.Id;
                        p.TreePId = relation.Id;
                        p.ParentNo = relation.PackageNo;
                    });
                    UpdateChildRelations(curRelations, relation.Id, ref packageRelations);
                }
                else
                {
                    itemLabels = RT.Service.Resolve<ItemLabelController>().GetItemLabels(records.Select(p => p.Sn).Distinct().ToList());
                    itemLabels.ForEach(p => p.RelationId = relation.Id);
                }

                records.ForEach(p => p.PersistenceStatus = PersistenceStatus.Deleted);
                string woSn = string.Empty;
                if (records.Any(p => p.WoSn != ""))
                {
                    woSn = string.Join(",", records.OrderBy(p => p.WoSn).Select(p => p.WoSn).ToList());
                }
                else
                {
                    woSn = string.Join(",", records.OrderBy(p => p.Sn).Select(p => p.Sn).ToList());
                }

                if (!isLast)
                {
                    var parentRecord = GeneragePackageSnRecord(relation.PackageNo, nextRule.PackageUnitId, woId, wo.ProductId, woSn, workcell.ResourceId, workcell.ProcessId, workcell.StationId, records.Count, records.Sum(p => p.ItemQty));
                    records.Add(parentRecord);
                }

                RF.Save(records);
                var insertRelations = packageRelations.Where(p => p.PersistenceStatus == PersistenceStatus.New).OrderByDescending(p => p.Id).AsEntityList();
                RF.Save(insertRelations);
                var otherRelations = packageRelations.Where(p => p.PersistenceStatus != PersistenceStatus.New).AsEntityList();
                RF.Save(otherRelations);
                if (itemLabels.Any())
                {
                    RF.Save(itemLabels);
                }
                tran.Complete();
                pkg = relation.PackageNo;
                return pkg;
            }
        }

        /// <summary>
        /// 打包(给当前包装)
        /// </summary>
        /// <param name="ruleList"></param>
        /// <param name="curRule"></param>
        /// <param name="record"></param>
        /// <returns></returns>
        public virtual string GivePackageNo(List<WorkOrderPackageRuleDetail> ruleList, WorkOrderPackageRuleDetail curRule, DirectPackageSnRecord record)
        {
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                var packageRelation = Query<PackingRelation>().Where(p => p.Id == record.PackRelationId).FirstOrDefault();
                var packageNo = RT.Service.Resolve<NumberRuleController>().GenerateSegment(curRule.NumberRuleId.Value, 1).FirstOrDefault();
                var lastRule = ruleList[ruleList.Count - 1];
                if (lastRule.Id == curRule.Id)
                {
                    record.PersistenceStatus = PersistenceStatus.Deleted;
                }
                record.Sn = packageNo;
                packageRelation.PackageNo = packageNo;
                RF.Save(record);
                RF.Save(packageRelation);
                tran.Complete();
                return packageNo;
            }
        }

        private PackingRelation GeneratePackingRelation(double numberRule, double woId, double packageUnitId, double processId, double stationId, bool isProcessFinish, decimal packedQty, decimal itemQty)
        {
            var no = RT.Service.Resolve<NumberRuleController>().GenerateSegment(numberRule, 1).FirstOrDefault();
            var packingRelation = new PackingRelation()
            {
                PackageNo = no,
                ParentNo = "",
                FullPackedQty = 0,
                PackedQty = packedQty, //下一层的子数量
                ItemQty = itemQty, //包装产品SN的数量
                PackingBy = RT.IdentityId,
                PackedDate = DateTime.Now,
                PackageUnitId = packageUnitId,
                //RootId = rootId, //根ID
                TreePId = null, //父ID
                State = LogisticState.Printed,
                ProcessId = processId,
                StationId = stationId,
                IsProcessFinish = true,
                WorkOrderId = woId
            };
            packingRelation.GenerateId();
            packingRelation.RootId = packingRelation.Id;
            return packingRelation;
        }
        private void UpdateChildRelations(List<PackingRelation> curRelations, double rootId, ref EntityList<PackingRelation> packageRelations)
        {
            if (curRelations.Count > 0)
            {
                var ids = curRelations.Select(p => (double?)p.Id).ToList();
                var curPackageRelations = packageRelations.Where(p => ids.Contains(p.TreePId)).ToList();
                if (curPackageRelations.Count > 0)
                {
                    curPackageRelations.ForEach(p => { p.RootId = rootId; });
                    UpdateChildRelations(curPackageRelations, rootId, ref packageRelations);
                }
            }
        }

        public virtual DirectPackageSnRecord GeneragePackageSnRecord(string sn, double packageUnitId, double woId, double productId, string woSn, double resourceId, double processId, double stationId, decimal packedQty, decimal itemQty)
        {
            var record = new DirectPackageSnRecord()
            {
                Sn = sn,
                PackageUnitId = packageUnitId,
                WorkOrderId = woId,
                WoSn = woSn,
                ResourceId = resourceId,
                ProcessId = processId,
                StationId = stationId,
                PackedQty = packedQty,
                ItemQty = itemQty,
                ProductId = productId
            };
            return record;
        }

        private PackingRelation UpdatePackageSnRecord(DirectPackageSnRecord record, WorkOrderPackageRuleDetail curRule)
        {
            var packingRelation = Query<PackingRelation>().Where(p => p.Id == record.PackRelationId).FirstOrDefault();
            var curRuleNumber = Query<WorkOrderPackageRuleDetail>().Where(p => p.Id == curRule.Id).FirstOrDefault();
            if (packingRelation != null && curRuleNumber != null)
            {

                var no = RT.Service.Resolve<NumberRuleController>().GenerateSegment(curRuleNumber.NumberRuleId.Value, 1).FirstOrDefault();
                packingRelation.PackageNo = no;
                record.Sn = no;
                return packingRelation;
            }
            else
            {
                throw new ValidationException("记录包装关系异常或当前包装单位{0}没有维护编码规则".L10nFormat(curRule.PackageUnitName));
            }
        }

        private WipProductVersion GetWipProductVersionByCode(string barcode)
        {
            return Query<WipProductVersion>().Where(p => p.Sn == barcode).FirstOrDefault();
        }
    }
}
