using SIE.Common.Configs;
using SIE.Core.Common.Controllers;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.AssetScraps;
using SIE.EMS.Enums;
using SIE.EMS.InventoryBalances.Configs;
using SIE.EMS.InventoryTasks;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Enums;
using SIE.EMS.SpareParts.OutDepots;
using SIE.EMS.SpareParts.OutDepots.Controllers;
using SIE.EMS.SpareParts.OutDepots.Details;
using SIE.EMS.SpareParts.OutDepots.Enums;
using SIE.Equipments.Configs;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipmentCards;
using SIE.Equipments.WorkFlows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.EMS.InventoryBalances
{
    /// <summary>
    /// 盘点平账控制器
    /// </summary>
    public partial class InventoryBalanceController : DomainController
    {
        /// <summary>
        /// 根据id获取盘点平账
        /// </summary>
        /// <param name="balanceId">盘点平账id</param>
        /// <returns>盘点平账</returns>
        public virtual InventoryBalance GetInventoryBalanceById(double balanceId)
        {
            return Query<InventoryBalance>().Where(p => p.Id == balanceId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 查询盘点平账
        /// </summary>
        /// <param name="criteria">盘点平账查询</param>
        /// <returns>盘点平账</returns>
        public virtual EntityList<InventoryBalance> CriteriaInventoryBalances(InventoryBalanceCriteria criteria)
        {
            var query = Query<InventoryBalance>();
            if (criteria.InventoryAssetObject.HasValue)
            {
                query.Where(p => p.InventoryPlan.InventoryAssetObject == criteria.InventoryAssetObject);
            }

            if (!criteria.TaskNo.IsNullOrWhiteSpace())
            {
                query.Where(p => p.TaskNo.Contains(criteria.TaskNo));
            }
            if (criteria.FactoryId.HasValue)
            {
                query.Where(p => p.FactoryId == criteria.FactoryId.Value);
            }
            if (!criteria.PlanNo.IsNullOrWhiteSpace())
            {
                query.Where(p => p.InventoryPlan.PlanNo.Contains(criteria.PlanNo));
            }

            if (!criteria.InventoryType.IsNullOrWhiteSpace())
            {
                query.Where(p => p.InventoryType == criteria.InventoryType);
            }
            if (criteria.ResponsibleId.HasValue)
            {
                query.Where(p => p.ResponsibleId == criteria.ResponsibleId.Value);
            }
            if (criteria.ApprovalStatus.HasValue)
            {
                query.Where(p => p.ApprovalStatus == criteria.ApprovalStatus.Value);
            }
            if (criteria.CreateDate.BeginValue.HasValue)
            {
                query.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue.Value);
            }
            if (criteria.CreateDate.EndValue.HasValue)
            {
                query.Where(p => p.CreateDate <= criteria.CreateDate.EndValue.Value);
            }
            //查询盘点任务数据，只查询盘点状态为【盘点完成】的数据
            query.Where(p => p.InventoryTaskStatus == InventoryTaskStatus.Completed);
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 查询设备清单
        /// </summary>
        /// <param name="balanceId">平账id</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="sortInfo">排序</param>
        /// <returns>设备清单</returns>
        public virtual EntityList<InventoryTaskEquipment> GetTaskEquipments(double balanceId, PagingInfo pagingInfo, IList<OrderInfo> sortInfo)
        {
            return Query<InventoryTaskEquipment>().Where(p => p.InventoryTaskId == balanceId)
                .OrderBy(sortInfo).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 查询盘点任务原因分析
        /// </summary>
        /// <param name="balanceId">平账id</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="sortInfo">排序</param>
        /// <returns>盘点任务原因分析</returns>
        public virtual EntityList<InventoryCause> GetInventoryCauses(double balanceId, PagingInfo pagingInfo, IList<OrderInfo> sortInfo)
        {
            return Query<InventoryCause>().Where(p => p.InventoryTaskId == balanceId)
                .OrderBy(sortInfo).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        ///<summary>
        /// 获取审批流程配置
        /// </summary>
        /// <returns>审批流程配置</returns>
        public virtual ApprovalConfigValue GetApprovalConfigValue()
        {
            var config = ConfigService.GetConfig(new ApprovalConfig(), typeof(InventoryBalance));
            if (config == null)
            {
                throw new ValidationException("未找到审批流程配置,请检查规则配置".L10N());
            }
            return config;
        }

        ///<summary>
        /// 获取报废类型配置
        /// </summary>
        /// <returns>报废类型</returns>
        public virtual string GetBalanceScrapType()
        {
            var config = ConfigService.GetConfig(new BalanceScrapTypeConfig(), typeof(InventoryBalance));
            if (config == null)
            {
                throw new ValidationException("未找到报废类型配置,请检查规则配置".L10N());
            }
            return config.ScrapType;
        }

        /// <summary>
        /// 提交平账
        /// </summary>
        /// <param name="balanceId">盘点平账id</param>
        public virtual void SubmitBalance(double balanceId)
        {
            var balance = GetInventoryBalanceById(balanceId);

            if (balance == null)
            {
                throw new ValidationException("提交数据异常".L10N());
            }

            //只有审核状态为【待提交】、【驳回】的数据才能点击，点击时还要后台获取最新状态进行校验
            if (balance.ApprovalStatus != ApprovalStatus.Draft && balance.ApprovalStatus != ApprovalStatus.Reject)
            {
                throw new ValidationException("只有审核状态为【待提交】、【驳回】的数据才能提交".L10N());
            }

            switch (balance.InventoryAssetObject)
            {
                case InventoryAssetObject.Equipment:
                    CheckEquipmentBalnace(balance);
                    break;
                case InventoryAssetObject.Spare:
                    CheckSparePartBalnace(balance);
                    break;
                case InventoryAssetObject.Fixture:
                    CheckFixtureBalnace(balance);
                    break;
                default:
                    break;
            }

            var config = GetApprovalConfigValue();

            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                //根据配置项【是否启用审批】，【是】则更新审核状态为【待审核】
                balance.ApprovalStatus = ApprovalStatus.PendingReview;

                if (config.EnableAudit)
                {                   
                    RF.Save(balance);
                }

                //生成审核结果为提交的审核记录数据
                var now = RF.Find<InventoryBalance>().GetDbTime();
                RT.Service.Resolve<WorkFlowRecordController>()
                    .CreateWorkFlowRecords(new List<double> { balanceId }, typeof(InventoryBalance).FullName, ApprovalResult.Submit, now, "");

                //是否启用审批为false时提交后自动审批
                if (!config.EnableAudit)
                {
                    ExamineBalanceInner(balanceId, ApprovalResult.Pass, "通过".L10N(), balance);
                }

                trans.Complete();
            }
        }

        /// <summary>
        /// 检查工治具平账数据
        /// </summary>
        /// <param name="balance"></param>        
        /// <exception cref="NotImplementedException"></exception>
        private void CheckFixtureBalnace(InventoryBalance balance)
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (balance.FixtureCauseAnalysis.IsNullOrEmpty())
            {
                stringBuilder.AppendLine("原因分析必填".L10N());
            }

            if (balance.FixtureImprovement.IsNullOrEmpty())
            {
                stringBuilder.AppendLine("改善措施必填".L10N());
            }

            if (stringBuilder.Length > 0)
            {
                throw new ValidationException(stringBuilder.ToString());
            }
        }

        /// <summary>
        /// 校验设备平账数据
        /// </summary>
        /// <param name="balance">盘点单</param>        
        /// <exception cref="ValidationException"></exception>
        private void CheckEquipmentBalnace(InventoryBalance balance)
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (balance.InventoryTaskEquipmentList.Any(x => !x.InventoryProcessMethod.HasValue))
            {
                stringBuilder.AppendLine("设备清单中有设备编码的平账方式没有维护".L10N());
            }

            //盘点结果为【盘亏】（复盘结果为空取初盘结果，复盘结果不为空的取复盘结果，但是剔除初盘盘盈，复盘盘亏的数据）的数据才能选择【报废】
            foreach (var eqp in balance.InventoryTaskEquipmentList
                .Where(x => x.InventoryProcessMethod == InventoryProcessMethod.Scrap))
            {
                var inventoryResult = eqp.SecondInventoryResult ?? eqp.FirstInventoryResult;
                if (inventoryResult != InventoryResult.Loss ||
                    (eqp.SecondInventoryResult == InventoryResult.Loss && eqp.FirstInventoryResult == InventoryResult.Profit))
                {
                    stringBuilder.AppendLine("盘点结果为【盘亏】的数据才能选择【报废】".L10N());
                    break;
                }
            }

            //盘点结果为【盘盈】且设备台账ID为空的数据才能选择【新增卡片】
            if (balance.InventoryTaskEquipmentList.Any(x => x.InventoryProcessMethod == InventoryProcessMethod.NewCard
                   && ((x.SecondInventoryResult ?? x.FirstInventoryResult) != InventoryResult.Profit || x.EquipAccountId.HasValue)))
            {
                stringBuilder.AppendLine("盘点结果为【盘盈】且设备台账ID为空的数据才能选择【新增卡片】".L10N());
            }

            if (balance.InventoryTaskEquipmentList
                 .Any(x => x.InventoryProcessMethod == InventoryProcessMethod.NewCard && (x.EquipModelId == null || x.EquipModelId == 0)))
            {
                stringBuilder.AppendLine("设备清单中有设备编码的平账方式为【{0}】,需维护【设备型号】"
                    .L10nFormat(InventoryProcessMethod.NewCard.ToLabel()));
            }

            if (stringBuilder.Length > 0)
            {
                throw new ValidationException(stringBuilder.ToString());
            }
        }

        /// <summary>
        /// 平账审核通过
        /// </summary>
        /// <param name="balance">平账</param>
        private void BalanceAuditedPass(InventoryBalance balance)
        {
            switch (balance.InventoryAssetObject)
            {
                case InventoryAssetObject.Equipment:
                    {
                        //资产对象为【设备】时，执行逻辑：
                        ProcessEquipments(balance);
                    }
                    break;
                case InventoryAssetObject.Spare:
                    {
                        //资产对象为【备件】时，执行逻辑：
                        ProcessSpareParts(balance);
                    }
                    break;
                case InventoryAssetObject.Fixture:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 备件平账处理
        /// </summary>
        /// <param name="balance">盘点任务</param>
        /// <exception cref="ValidationException"></exception>
        private void ProcessSpareParts(InventoryBalance balance)
        {
            var now = RF.Find<InventoryBalance>().GetDbTime();

            //校验所有数据的平账方式都填写(提交验证时，有用到设备清单，这里直接使用
            var inventoryTaskSparePartDetails = RT.Service.Resolve<InventoryTaskSpartPartController>()
                .GetInventoryTaskSparePartDetails(balance.Id);

            if (inventoryTaskSparePartDetails.Any(p => p.SparePartProcessMethod == null))
            {
                throw new ValidationException("所有备件盘点明细的平账方式都要填写".L10N());
            }

            if (!balance.WarehouseId.HasValue)
            {
                throw new ValidationException("备件盘点任务的仓库为空".L10N());
            }

            //	库存调整：
            //	编码管控：根据【备件编码 + 库位】到备件库存查询，更新主表和库位明细的【不良品数、可用库存、总库存】
            //	批次管控：根据【备件编码 + 库位 + 批次】到备件库存查询，更新主表和批次明细的【不良品数、可用库存、总库存】
            //	序列号管控：根据【备件编码 + 序列号】到备件库存查询，更新主表【不良品数、可用库存、总库存】，更新序列号明细的【状态、库位、库存状态（入库）】
            AdjustSparePartStock(inventoryTaskSparePartDetails);

            //	报废：除了执行库存调整的逻辑，所有报废的数据，盘点良品数小于良品数的数据统一生成一个备件出库单，盘点不良品数小于不良品数的统一生成一个备件出库单
            CreateSparePartScrap(balance, now, inventoryTaskSparePartDetails);

            //	盘盈入库：所有盘盈入库的数据统一生成一个备件入库单
            CreateSparePartProfitStockIn(balance, inventoryTaskSparePartDetails);
        }

        /// <summary>
        /// 盘盈入库：所有盘盈入库的数据统一生成一个备件入库单
        /// </summary>
        /// <param name="balance"></param>
        /// <param name="inventoryTaskSparePartDetails"></param>
        private void CreateSparePartProfitStockIn(InventoryBalance balance, EntityList<InventoryTaskSparePartDetail> inventoryTaskSparePartDetails)
        {
            var profitStockIns = inventoryTaskSparePartDetails.Where(x => x.SparePartProcessMethod == SparePartProcessMethod.ProfitStockIn)
              .ToList();

            if (profitStockIns.Any())
            {
                var store = new SparePartStore();
                store.StoreCode = RT.Service.Resolve<SparePartController>().GetStoreCode();
                store.InboundType = SparePartInboundType.Profit;
                store.DisposalNo = balance.TaskNo;
                store.InboundStatus = InboundStatus.ToBe;
                store.WarehouseId = balance.WarehouseId.Value;

                var lineNo = 1;
                foreach (var sparePartDetail in profitStockIns)
                {
                    //良品和不良品分开入库明细

                    var ngQtyActual = (sparePartDetail.SecondNgQty ?? sparePartDetail.FirstNg) ?? 0;

                    if (ngQtyActual > 0)
                    {
                        //不良品
                        var storeDetail = new StoreDetail();
                        storeDetail.LineNo = lineNo.ToString();
                        storeDetail.UnitPrice = 0;
                        storeDetail.SparePartId = sparePartDetail.SparePartId;
                        storeDetail.SparePartStore = store;
                        storeDetail.InboundStatus = InboundStatus.ToBe;
                        storeDetail.BatchNumber = sparePartDetail.LotNo;
                        storeDetail.Sn = sparePartDetail.Sn;
                        storeDetail.Number = ngQtyActual;
                        storeDetail.QualityStatus = QualityStatus.Defective;
                        store.StoreDetailList.Add(storeDetail);
                        lineNo++;
                    }

                    var goodQtyActual = (sparePartDetail.SecondGoodQty ?? sparePartDetail.FirstGood) ?? 0;
                    if (goodQtyActual > 0)
                    {
                        //良品
                        var storeDetail = new StoreDetail();
                        storeDetail.LineNo = lineNo.ToString();
                        storeDetail.UnitPrice = 0;
                        storeDetail.SparePartId = sparePartDetail.SparePartId;
                        storeDetail.SparePartStore = store;
                        storeDetail.QualityStatus = QualityStatus.Good;
                        storeDetail.InboundStatus = InboundStatus.ToBe;
                        storeDetail.BatchNumber = sparePartDetail.LotNo;
                        storeDetail.Sn = sparePartDetail.Sn;
                        storeDetail.Number = goodQtyActual;
                        store.StoreDetailList.Add(storeDetail);
                        lineNo++;
                    }
                }

                RF.Save(store);
            }
        }

        /// <summary>
        /// 创建备件报废
        /// </summary>
        /// <param name="balance">盘点任务</param>
        /// <param name="now">当前时间（取数据库）</param>
        /// <param name="inventoryTaskSparePartDetails">备件盘点明细</param>
        /// <exception cref="ValidationException"></exception>
        private void CreateSparePartScrap(InventoryBalance balance, DateTime now, EntityList<InventoryTaskSparePartDetail> inventoryTaskSparePartDetails)
        {
            //报废：除了执行库存调整的逻辑，所有报废的数据，盘点良品数小于良品数的数据统一生成一个备件出库单，盘点不良品数小于不良品数的统一生成一个备件出库单，字段如下：
            var scraps = inventoryTaskSparePartDetails.Where(x => x.SparePartProcessMethod == SparePartProcessMethod.Scrap)
               .ToList();

            if (scraps.Any())
            {
                EntityList<PartOutDepotDetail> detailListNg = new EntityList<PartOutDepotDetail>();
                EntityList<PartOutDepotDetail> detailListGood = new EntityList<PartOutDepotDetail>();

                foreach (var sparePartDetail in scraps)
                {
                    var diff = (sparePartDetail.SecondDiff ?? sparePartDetail.FirstDiff) ?? 0;

                    if (diff == 0 || diff > 0)
                    {
                        //差异数为0 或大于0 不生成报废出库
                        continue;
                    }

                    if (!sparePartDetail.StorageLocationId.HasValue)
                    {
                        throw new ValidationException("生成报废出库失败，备件明细中的备件【{0}】库位为空"
                            .L10nFormat(sparePartDetail.SparePartCode));
                    }

                    var ngQtyActual = (sparePartDetail.SecondNgQty ?? sparePartDetail.FirstNg) ?? 0;
                    var goodQtyActual = (sparePartDetail.SecondGoodQty ?? sparePartDetail.FirstGood) ?? 0;

                    //差异的绝对值
                    var absDiff = Math.Abs(diff);

                    if (ngQtyActual < sparePartDetail.NgQty)
                    {
                        //盘点不良品数小于不良品数的统一生成一个备件出库单
                        //出库数量: MIN{【盘点差异数】,【盘点不良品数 - 不良品数】}
                        CreateNgScrapOutDetail(now, detailListNg, sparePartDetail, absDiff, ngQtyActual);
                    }

                    if (goodQtyActual < sparePartDetail.GoodQty)
                    {
                        //盘点不良品数小于不良品数的统一生成一个备件出库单
                        //出库数量:MIN{【盘点差异数】,【盘点良品数-良品数】}
                        CreateGoodScrapOutDetail(now, detailListGood, sparePartDetail, absDiff, goodQtyActual);
                    }
                }

                if (detailListNg.Any())
                {
                    OutDepot outDepot = new OutDepot();
                    outDepot.OutDepotType = OutDepotType.Scrap;
                    outDepot.No = RT.Service.Resolve<OutDepotController>().GetNo();
                    outDepot.OutDepotState = OutDepotState.Ed;
                    outDepot.SourceNo = balance.TaskNo;
                    outDepot.QualityStatus = QualityStatus.Defective;
                    //2022-06-15 张俊杰的需求 领用部门改成非必输，添加出库单那里。要加个检验部门不能为空
                    // 领用部门 /*outDepot.GetDepartmentId = bill.GetDepartmentId;*/
                    outDepot.OutDepotDate = now;
                    outDepot.IsAppComeHere = YesNo.No;
                    outDepot.WarehouseId = balance.WarehouseId;

                    RF.Save(outDepot);

                    int i = 1;
                    foreach (var item in detailListNg)
                    {
                        item.LineNo = i;
                        item.OutDepotId = outDepot.Id;
                        i++;
                    }

                    RF.Save(detailListNg);
                }

                if (detailListGood.Any())
                {
                    OutDepot outDepot = new OutDepot();
                    outDepot.OutDepotType = OutDepotType.Scrap;
                    outDepot.No = RT.Service.Resolve<OutDepotController>().GetNo();
                    outDepot.OutDepotState = OutDepotState.Ed;
                    outDepot.SourceNo = balance.TaskNo;
                    outDepot.QualityStatus = QualityStatus.Good;

                    //2022-06-15 张俊杰的需求 领用部门改成非必输，添加出库单那里。要加个检验部门不能为空
                    // 领用部门 /*outDepot.GetDepartmentId = bill.GetDepartmentId;*/
                    outDepot.OutDepotDate = now;
                    outDepot.IsAppComeHere = YesNo.No;
                    outDepot.WarehouseId = balance.WarehouseId;

                    RF.Save(outDepot);

                    int i = 1;
                    foreach (var item in detailListGood)
                    {
                        item.LineNo = i;
                        item.OutDepotId = outDepot.Id;
                        i++;
                    }

                    RF.Save(detailListGood);
                }
            }
        }

        /// <summary>
        /// 创建良品的报废出库明细
        /// </summary>
        /// <param name="now">当前时间（数据库）</param>
        /// <param name="detailListGood">良品报废出库明细</param>
        /// <param name="sparePartDetail">备件盘点明细</param>
        /// <param name="qty">差异数（绝对值）</param>
        /// <param name="goodQtyActual">实盘良品数</param>
        private void CreateGoodScrapOutDetail(DateTime now, EntityList<PartOutDepotDetail> detailListGood, InventoryTaskSparePartDetail sparePartDetail, int qty, int goodQtyActual)
        {
            var goodDiff = sparePartDetail.GoodQty - goodQtyActual;

            if (goodDiff < qty)
            {
                qty = goodDiff;
            }

            PartOutDepotDetail dtl = new PartOutDepotDetail();
            dtl.SparePartId = sparePartDetail.SparePartId;
            dtl.StorageLocationId = sparePartDetail.StorageLocationId.Value;
            dtl.BatchNo = sparePartDetail.LotNo;
            dtl.BatchNoRefId = sparePartDetail.StoreSummaryLotId;
            dtl.SeriaNo = sparePartDetail.Sn;
            dtl.SeriaNoRefId = sparePartDetail.StoreSummaryDetailId;
            dtl.OutboundStatus = OutboundStatus.Shipped;
            dtl.OutDepotCount = qty;
            dtl.OutDepotDate = now;
            dtl.QualityStatus = QualityStatus.Good;
            detailListGood.Add(dtl);
        }

        /// <summary>
        /// 创建良品的报废出库明细
        /// </summary>
        /// <param name="now">当前时间（数据库）</param>
        /// <param name="detailListNg">不良品报废出库明细</param>
        /// <param name="sparePartDetail">备件盘点明细</param>
        /// <param name="qty">差异数（绝对值）</param>
        /// <param name="ngQtyActual">实盘不良品数</param>
        private void CreateNgScrapOutDetail(DateTime now, EntityList<PartOutDepotDetail> detailListNg, InventoryTaskSparePartDetail sparePartDetail, int qty, int ngQtyActual)
        {
            var ngDiff = sparePartDetail.NgQty - ngQtyActual;

            if (ngDiff < qty)
            {
                qty = ngDiff;
            }

            PartOutDepotDetail dtl = new PartOutDepotDetail();
            dtl.SparePartId = sparePartDetail.SparePartId;
            dtl.StorageLocationId = sparePartDetail.StorageLocationId.Value;
            dtl.BatchNo = sparePartDetail.LotNo;
            dtl.BatchNoRefId = sparePartDetail.StoreSummaryLotId;
            dtl.SeriaNo = sparePartDetail.Sn;
            dtl.SeriaNoRefId = sparePartDetail.StoreSummaryDetailId;
            dtl.OutboundStatus = OutboundStatus.Shipped;
            dtl.OutDepotCount = qty;
            dtl.OutDepotDate = now;
            dtl.QualityStatus = QualityStatus.Defective;
            detailListNg.Add(dtl);
        }

        /// <summary>
        /// 调整备件库存
        /// </summary>
        /// <param name="inventoryTaskSparePartDetails"></param>
        private void AdjustSparePartStock(EntityList<InventoryTaskSparePartDetail> inventoryTaskSparePartDetails)
        {
            //	编码管控：根据【备件编码 +库位】到备件库存查询，更新主表和库位明细的【不良品数、可用库存、总库存】
            var adjustsItemCode = inventoryTaskSparePartDetails.Where(x =>
                    (x.SparePartProcessMethod == SparePartProcessMethod.Adjust || x.SparePartProcessMethod == SparePartProcessMethod.Scrap)
                   && x.ControlMethod == ControlMethod.ItemCode)
                .ToList();

            var sparePartIds = adjustsItemCode.Select(x => x.SparePartId).Distinct().ToList();
            var storeSummaries = sparePartIds.SplitContains(tempIds =>
            {
                return Query<StoreSummary>().Where(x => tempIds.Contains(x.SparePartId)).ToList();
            });

            if (adjustsItemCode.Any())
            {
              
                foreach (var part in adjustsItemCode)
                {
                    var storeSummary = storeSummaries.FirstOrDefault(x => x.SparePartId == part.SparePartId);

                    var ngQty = (part.SecondNgQty ?? part.FirstNg) ?? 0;
                    var goodQty = (part.SecondGoodQty ?? part.FirstGood) ?? 0;
                    var totalQty = ((part.SecondTotal ?? part.FirstTotal) ?? 0);

                    DB.Update<StoreSummaryLocation>()
                        .Set(x => x.RotNumber, ngQty)
                        .Set(x => x.GoodNumber, goodQty)
                        .Set(x => x.SumNumber, totalQty)
                        .Where(x => x.StoreSummaryId == storeSummary.Id && x.StorageLocationId == part.StorageLocationId)
                        .Execute();
                }

              

            }

            //	批次管控：根据【备件编码 +库位+批次】到备件库存查询，更新主表和批次明细的【不良品数、可用库存、总库存】
            var adjustsLot = inventoryTaskSparePartDetails.Where(x =>
                    (x.SparePartProcessMethod == SparePartProcessMethod.Adjust || x.SparePartProcessMethod == SparePartProcessMethod.Scrap)
                   && x.ControlMethod == ControlMethod.Batch)
                .ToList();

            foreach (var part in adjustsLot)
            {
                var ngQty = (part.SecondNgQty ?? part.FirstNg) ?? 0;
                var goodQty = (part.SecondGoodQty ?? part.FirstGood) ?? 0;
                var totalQty = ((part.SecondTotal ?? part.FirstTotal) ?? 0);

                DB.Update<StoreSummaryLot>()
                    .Set(x => x.RotNumber, ngQty)
                    .Set(x => x.GoodNumber, goodQty)
                    .Set(x => x.SumNumber, totalQty)
                    .Where(x => x.Id == part.StoreSummaryLotId)
                    .Execute();
            }

            //	序列号管控：根据【备件编码 +序列号】到备件库存查询，更新主表【不良品数、可用库存、总库存】，更新序列号明细的【状态、库位、库存状态（入库）】
            var adjustsSn = inventoryTaskSparePartDetails.Where(x =>
                (x.SparePartProcessMethod == SparePartProcessMethod.Adjust || x.SparePartProcessMethod == SparePartProcessMethod.Scrap)
                && x.ControlMethod == ControlMethod.Sn)
                .ToList();

            foreach (var part in adjustsSn)
            {
                var ngQty = (part.SecondNgQty ?? part.FirstNg) ?? 0;
                var goodQty = (part.SecondGoodQty ?? part.FirstGood) ?? 0;
                var totalQty = ((part.SecondTotal ?? part.FirstTotal) ?? 0);
                var result = (part.SecondResult ?? part.SecondResult);

                var odNbStatus = OdNbStatus.NoGoodProduct;
                if (goodQty == 1)
                {
                    odNbStatus = OdNbStatus.GoodProduct;
                }

                //质量状态更新为【报废】
                if (part.SparePartProcessMethod == SparePartProcessMethod.Scrap)
                {
                    odNbStatus = OdNbStatus.Scrap;
                }

                var entityUpdate = DB.Update<StoreSummaryDetail>()
                    .Set(x => x.RotNumber, ngQty)
                    .Set(x => x.GoodNumber, goodQty)
                    .Set(x => x.SumNumber, totalQty)
                    .Set(x => x.OdNbStatus, odNbStatus)
                    .Set(x => x.StorageLocationId, part.StorageLocationId);

                //库存状态更新为出库
                if (part.SparePartProcessMethod == SparePartProcessMethod.Scrap || result == InventoryResult.Loss)
                {
                    entityUpdate.Set(x => x.StoreStatus, OrdNumStoreStatus.Out);
                }
                else
                {
                    entityUpdate.Set(x => x.StoreStatus, OrdNumStoreStatus.In);
                }

                entityUpdate.Where(x => x.Id == part.StoreSummaryDetailId)
                    .Execute();
            }

            //更新主表的信息
            foreach (var summary in storeSummaries)
            {
                summary.RotNumber = +summary.StoreSummaryLocationList.Sum(x => x.RotNumber);
                summary.GoodNumber = +summary.StoreSummaryLocationList.Sum(x => x.GoodNumber);
                summary.SumNumber = +summary.StoreSummaryLocationList.Sum(x => x.SumNumber);
                RF.Save(summary);
            }


        }

        private void ProcessEquipments(InventoryBalance balance)
        {
            //校验所有数据的平账方式都填写(提交验证时，有用到设备清单，这里直接使用
            var taskEquips = RT.Service.Resolve<InventoryBalanceController>().GetTaskEquipments(balance.Id, null, null);
            if (taskEquips.Any(p => p.InventoryProcessMethod == null))
            {
                throw new ValidationException("所有设备的平账方式都要填写".L10N());
            }

            //生成报废单
            GenerateAssetScrap(taskEquips, balance);

            //平账方式为【新增卡片】的数据
            var newCardEquips = taskEquips.Where(p => p.InventoryProcessMethod == InventoryProcessMethod.NewCard).ToList();
            if (newCardEquips.Any())
            {
                var enableEquipCard = RT.Service.Resolve<EquipAccountController>().GetUseCard();
                if (enableEquipCard)
                {
                    //【启用立卡】勾选生成立卡数据
                    GenerateEquipmentCard(newCardEquips, balance);
                }
                else
                {
                    //未勾选时生成设备台账数据（字段来源设备清单的字段或者为空）
                    GenerateEquipAccount(newCardEquips, balance);
                }
            }
        }

        /// <summary>
        /// 生成报废单
        /// </summary>
        /// <param name="taskEquips">设备清单</param>
        /// <param name="balance">盘点平账</param>
        private void GenerateAssetScrap(EntityList<InventoryTaskEquipment> taskEquips, InventoryBalance balance)
        {
            //平账方式为【报废】的所有设备按【管理部门+使用部门+仓库+是否固定资产（都是取当前设备台账的值）】分组生成报废单
            var scrapEquips = taskEquips.Where(p => p.InventoryProcessMethod == InventoryProcessMethod.Scrap).ToList();

            if (!scrapEquips.Any())
            {
                return;
            }

            var dicEquips = scrapEquips.GroupBy(p => new { p.OldManageDeptId, p.OldUseDeptId, p.OldWarehouseId, p.IssAsset }).ToDictionary(p => p.Key, p => p.ToList());
            var scrapType = GetBalanceScrapType();
            var now = RF.Find<InventoryBalance>().GetDbTime();
            var nos = RT.Service.Resolve<CommonController>().GetNos<AssetScrap>(dicEquips.Count, "报废单号");
            var i = 0;
            foreach (var dicEquip in dicEquips)
            {
                //生成报废单
                var assetScrap = new AssetScrap();
                assetScrap.No = nos[i];
                i++;
                assetScrap.FactoryId = balance.FactoryId;
                assetScrap.ApprovalStatus = ApprovalStatus.Draft;
                assetScrap.AssetObject = AssetObject.Equipment;
                assetScrap.Remark = "盘点报废，盘点任务单号{0}".L10nFormat(balance.TaskNo);
                assetScrap.ManageDeptId = dicEquip.Key.OldManageDeptId;
                assetScrap.UseDeptId = dicEquip.Key.OldUseDeptId;
                assetScrap.WarehouseId = dicEquip.Key.OldWarehouseId;
                assetScrap.IsFixAsset = dicEquip.Key.IssAsset;
                assetScrap.ApplicantId = RT.IdentityId;
                assetScrap.ApplyDate = now;
                RF.Save(assetScrap);
                foreach (var item in dicEquip.Value)
                {
                    var scrapEquip = new AssetScrapEquipment();
                    scrapEquip.AssetScrapId = assetScrap.Id;
                    scrapEquip.EquipAccountId = item.EquipAccountId ?? 0;
                    scrapEquip.ScrapType = scrapType;
                    scrapEquip.Reason = "盘点报废".L10N();
                    scrapEquip.ScrapNetValue = item.NetAssetValue;
                    RF.Save(scrapEquip);
                }
            }
        }

        /// <summary>
        /// 生成立卡数据
        /// </summary>
        /// <param name="newCardEquips">盘点任务设备清单</param>
        /// <param name="balance">盘点平账</param>
        private void GenerateEquipmentCard(List<InventoryTaskEquipment> newCardEquips, InventoryBalance balance)
        {
            //审核状态取值待提交，卡片来源取值盘盈新增，其他字段来源设备清单的字段或者为空
            foreach (var newCardEquip in newCardEquips)
            {
                var card = new EquipmentCard();
                card.ApprovalStatus = ApprovalStatus.Draft;
                card.EquipmentCardSource = EquipmentCardSource.Surplus;
                card.Code = newCardEquip.EquipmentCode;
                card.Name = newCardEquip.EquipmentName;
                card.FactoryId = balance.FactoryId;
                card.InstallationLocation = newCardEquip.RealLocation;
                card.Alias = newCardEquip.Alias;
                card.ManagementId = newCardEquip.RealManageDeptId;
                card.UseDepartmentId = newCardEquip.RealUseDeptId;
                card.WorkShopId = newCardEquip.RealWorkShopId;
                card.ResourceId = newCardEquip.RealResourceId;
                card.UserId = newCardEquip.UserId;
                card.WarehouseId = newCardEquip.RealWarehouseId;
                card.StorageLocationId = newCardEquip.StorageLocationId;
                card.EquipModelId = newCardEquip.EquipModelId ?? 0;
                card.AccountState = newCardEquip.AccountState ?? AccountState.Running;
                card.AccountUseState = newCardEquip.AccountUseState ?? AccountUseState.InIdle;
                RF.Save(card);
            }
        }

        /// <summary>
        /// 写入设备台账表
        /// </summary>
        /// <param name="newCardEquips">盘点任务设备清单</param>
        /// <param name="balance">盘点平账</param>
        private void GenerateEquipAccount(List<InventoryTaskEquipment> newCardEquips, InventoryBalance balance)
        {
            foreach (var newCardEquip in newCardEquips)
            {
                var equipAccount = new EquipAccount();
                equipAccount.FactoryId = balance.FactoryId;
                equipAccount.Code = newCardEquip.EquipmentCode;
                equipAccount.Name = newCardEquip.EquipmentName;
                equipAccount.EquipModelId = newCardEquip.EquipModelId ?? 0;
                equipAccount.UseState = newCardEquip.AccountUseState ?? AccountUseState.InIdle;
                equipAccount.State = newCardEquip.AccountState ?? AccountState.Running;
                equipAccount.InstallationLocation = newCardEquip.RealLocation;
                equipAccount.Alias = newCardEquip.Alias;
                equipAccount.ManageDepartmentId = newCardEquip.RealManageDeptId;
                equipAccount.UseDepartmentId = newCardEquip.RealUseDeptId;
                equipAccount.WorkShopId = newCardEquip.RealWorkShopId;
                equipAccount.ResourceId = newCardEquip.RealResourceId;
                equipAccount.UserId = newCardEquip.UserId;
                equipAccount.WarehouseId = newCardEquip.RealWarehouseId;
                equipAccount.StorageLocationId = newCardEquip.StorageLocationId;
                RF.Save(equipAccount);
            }
        }

        /// <summary>
        /// 撤回盘点平账
        /// </summary>
        /// <param name="balanceId">选择行id</param>
        public virtual void CancelBalance(double balanceId)
        {
            var balance = GetInventoryBalanceById(balanceId);
            if (balance == null)
            {
                throw new ValidationException("撤回数据异常".L10N());
            }
            //只有状态为【待审核】的数据才能点击，点击时还要后台获取最新状态进行校验
            if (balance.ApprovalStatus != ApprovalStatus.PendingReview)
            {
                throw new ValidationException("只有状态为【待审核】的数据才能操作".L10N());
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                //更新状态为【待提交】
                balance.ApprovalStatus = ApprovalStatus.Draft;
                RF.Save(balance);

                //生成审核结果为撤回的审核记录数据
                var now = RF.Find<InventoryBalance>().GetDbTime();
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(new List<double> { balanceId }, typeof(InventoryBalance).FullName, ApprovalResult.Retract, now, "");
                trans.Complete();
            }
        }

        /// <summary>
        /// 审核盘点平账
        /// </summary>
        /// <param name="balanceId">选择行id</param>
        /// <param name="value">结果</param>
        /// <param name="remark">备注</param>
        public virtual void ExamineBalance(double balanceId, ApprovalResult value, string remark)
        {
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                ExamineBalanceInner(balanceId, value, remark);

                trans.Complete();
            }
        }

        /// <summary>
        /// 审核盘点平账
        /// </summary>
        /// <param name="balanceId">选择行id</param>
        /// <param name="value">结果</param>
        /// <param name="remark">备注</param>
        /// <param name="balance">盘点平账</param>
        public virtual void ExamineBalanceInner(double balanceId, ApprovalResult value, string remark, InventoryBalance balance = null)
        {
            if (balance == null)
            {
                balance = GetInventoryBalanceById(balanceId);
                if (balance == null)
                {
                    throw new ValidationException("审核数据异常".L10N());
                }
            }

            //只有状态为【待审核】的数据才能点击，点击时还要后台获取最新状态进行校验
            if (balance.ApprovalStatus != ApprovalStatus.PendingReview)
            {
                throw new ValidationException("只有状态为【待审核】的数据才能审核".L10N());
            }

            //更新审核状态为【通过】或【驳回】
            balance.ApprovalStatus = value == ApprovalResult.Pass ? ApprovalStatus.Audited : ApprovalStatus.Reject;
            RF.Save(balance);

            //审核【通过】时，执行【提交】按钮写的重复的逻辑
            if (value == ApprovalResult.Pass)
            {
                BalanceAuditedPass(balance);
            }

            //往审批记录子表插入一条数据
            var now = RF.Find<InventoryBalance>().GetDbTime();
            RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(new List<double> { balanceId }, typeof(InventoryBalance).FullName, value, now, remark);
        }

        /// <summary>
        /// 修改原因分析
        /// </summary>
        /// <param name="cause">原因分析</param>
        public virtual void EditInventoryCause(InventoryCause cause)
        {
            RF.Save(cause);
        }

        /// <summary>
        /// 保存盘点平账
        /// </summary>
        /// <param name="balanceList">盘点平账</param>
        public virtual void SaveInventoryBalanceList(EntityList<InventoryBalance> balanceList)
        {
            //保存时，平账方式为【新增卡片】的不能为空，且校验不能重复，不能和设备卡片重复，不能和设备台账重复
            var allCodes = new List<string>();

            foreach (var taskEquip in balanceList.SelectMany(x => x.InventoryTaskEquipmentList))
            {
                if (taskEquip.InventoryProcessMethod == InventoryProcessMethod.NewCard)
                {
                    allCodes.Add(taskEquip.EquipmentCode);
                }

                var result = taskEquip.SecondInventoryResult.HasValue ? taskEquip.SecondInventoryResult : taskEquip.FirstInventoryResult;

                //盘点结果为【盘亏】的数据才能选择【报废】
                if (taskEquip.InventoryProcessMethod == InventoryProcessMethod.Scrap)
                {
                    if (result != InventoryResult.Loss)
                    {
                        throw new ValidationException("盘点结果为【盘亏】的数据才能选择【报废】".L10N());
                    }
                    if (taskEquip.FirstInventoryResult == InventoryResult.Profit && taskEquip.SecondInventoryResult == InventoryResult.Loss)
                    {
                        throw new ValidationException("初盘盘盈，复盘盘亏的数据不能选择【报废】".L10N());
                    }
                }

                //盘点结果为【盘盈】且设备台账ID为空的数据才能选择【新增卡片】
                if (taskEquip.InventoryProcessMethod == InventoryProcessMethod.NewCard)
                {
                    if (result != InventoryResult.Profit)
                    {
                        throw new ValidationException("盘点结果为【盘盈】的数据才能选择【新增卡片】".L10N());
                    }
                    if (taskEquip.EquipAccountId.HasValue)
                    {
                        throw new ValidationException("设备台账ID为空的数据才能选择【新增卡片】".L10N());
                    }
                }
            }

            if (allCodes.Any(p => p.IsNullOrWhiteSpace()))
            {
                throw new ValidationException("平账方式为【新增卡片】的设备编码不能为空".L10N());
            }

            var equips = RT.Service.Resolve<EquipAccountController>().GetEquipAccounts(allCodes);

            if (equips.Any())
            {
                throw new ValidationException("平账方式为【新增卡片】的设备编码和设备台账重复".L10N());
            }

            var equipmentCards = RT.Service.Resolve<EquipmentCardController>().GetEquipmentCardByCode(allCodes);

            if (equipmentCards.Any())
            {
                throw new ValidationException("平账方式为【新增卡片】的设备编码和设备卡片重复".L10N());
            }

            //保存界面数据
            RF.Save(balanceList);
        }

        /// <summary>
        /// 校验设备平账数据
        /// </summary>
        /// <param name="balance">盘点单</param>        
        /// <exception cref="ValidationException"></exception>
        private void CheckSparePartBalnace(InventoryBalance balance)
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (balance.InventoryTaskSparePartDetailList.Any(x => !x.SparePartProcessMethod.HasValue))
            {
                stringBuilder.AppendLine("备件明细中有数据的平账方式没有维护".L10N());
            }

            //根据盘点结果（复盘结果有值以复盘结果为准）和【来源】平账方式可选不同的值：
            //正常：不调整
            //信息变动：不调整、库存调整
            //盘盈且账内资产：不调整、库存调整
            //盘盈且盘盈新增：不调整、盘盈入库
            //盘亏：不调整、报废
            if (balance.InventoryTaskSparePartDetailList.Any(x => x.SparePartProcessMethod != SparePartProcessMethod.Unchanged
                && (x.SecondResult ?? x.FirstResult) == InventoryResult.Normal))
            {
                stringBuilder.AppendLine("盘点结果（复盘结果有值以复盘结果为准）为【正常】，平账方式只能选【不调整】".L10N());
            }

            if (balance.InventoryTaskSparePartDetailList.Any(x => (x.SecondResult ?? x.FirstResult) == InventoryResult.InfoChange
                && x.SparePartProcessMethod != SparePartProcessMethod.Unchanged
                && x.SparePartProcessMethod != SparePartProcessMethod.Adjust))
            {
                stringBuilder.AppendLine("盘点结果（复盘结果有值以复盘结果为准）为【信息变动】，平账方式只能选【不调整】或【库存调整】".L10N());
            }

            if (balance.InventoryTaskSparePartDetailList.Any(x => (x.SecondResult ?? x.FirstResult) == InventoryResult.Profit
                && x.InventoryAssetSource == InventoryAssetSource.Account
                && x.SparePartProcessMethod != SparePartProcessMethod.Unchanged
                && x.SparePartProcessMethod != SparePartProcessMethod.Adjust))
            {
                stringBuilder.AppendLine("盘点结果（复盘结果有值以复盘结果为准）为【盘盈】且来源为【账内资产】，平账方式只能选【不调整】或【库存调整】".L10N());
            }

            if (balance.InventoryTaskSparePartDetailList.Any(x => (x.SecondResult ?? x.FirstResult) == InventoryResult.Profit
               && x.InventoryAssetSource == InventoryAssetSource.Profit
               && x.SparePartProcessMethod != SparePartProcessMethod.Unchanged
               && x.SparePartProcessMethod != SparePartProcessMethod.ProfitStockIn))
            {
                stringBuilder.AppendLine("盘点结果（复盘结果有值以复盘结果为准）为【盘盈】且来源为【盘盈新增】，平账方式只能选【不调整】或【盘盈入库】".L10N());
            }

            if (balance.InventoryTaskSparePartDetailList.Any(x => (x.SecondResult ?? x.FirstResult) == InventoryResult.Loss
               && x.InventoryAssetSource == InventoryAssetSource.Profit
               && x.SparePartProcessMethod != SparePartProcessMethod.Unchanged
               && x.SparePartProcessMethod != SparePartProcessMethod.Scrap))
            {
                stringBuilder.AppendLine("盘点结果（复盘结果有值以复盘结果为准）为【盘亏】，平账方式只能选【不调整】或【报废】".L10N());
            }

            if (stringBuilder.Length > 0)
            {
                throw new ValidationException(stringBuilder.ToString());
            }
        }

    }
}
