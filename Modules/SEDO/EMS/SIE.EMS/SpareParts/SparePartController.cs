using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Core.Common;
using SIE.Domain;
using SIE.Domain.Query;
using SIE.Domain.Validation;
using SIE.EMS.ApiModel;
using SIE.EMS.Common.Configs;
using SIE.EMS.Equipments.Models;
using SIE.EMS.Items;
using SIE.EMS.MainenanceProjects;
using SIE.EMS.SpareParts.ApiModels;
using SIE.EMS.SpareParts.Applys.Enums;
using SIE.EMS.SpareParts.Configs;
using SIE.EMS.SpareParts.Criterias;
using SIE.EMS.SpareParts.Enums;
using SIE.EMS.SpareParts.OutDepots.Details;
using SIE.EMS.SpareParts.ViewModels;
using SIE.EMS.Warehouses;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipModels;
using SIE.EventMessages.EMS.SparePartReceives;
using SIE.Items;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.SpareParts
{
    /// <summary>
    /// 备件控制器
    /// </summary>
    public partial class SparePartController : DomainController
    {
        /// <summary>
        /// 同步物料信息
        /// </summary>
        public virtual void ImportSparePartItems()
        {
            EntityList<Item> sparePartItems;
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                //新增
                EntityList<SparePart> list = new EntityList<SparePart>();
                var itemCodes = Query<SparePart>().Where(p => p.IsImportItem == true).ToList()
                    .Select(p => p.SparePartCode).ToList();

                if (!itemCodes.Any())
                    sparePartItems = Query<Item>().Where(n => n.Type == ItemType.SparePart).ToList();
                else
                {
                    sparePartItems = Query<Item>()
                        .LeftJoin<SparePart>((a, b) => a.Code == b.SparePartCode && b.IsImportItem == true)
                        .Where<SparePart>((a, b) => b.SparePartCode == "" || b.SparePartCode == null)
                        .Where(p => p.Type == ItemType.SparePart).ToList();
                }

                var itemCateList = sparePartItems.Select(p => p.Id).SplitContains(tempIds =>
                {
                    return Query<ItemCategoryRelation>()
                    .Where(p => p.Type == SIE.Items.Items.CategoryType.Item
                    && p.ItemCategory.ItemType == ItemType.SparePart
                    && tempIds.Contains(p.ItemId)).ToList();
                });

                ItemCategoryRelation itemCategory = null;

                foreach (var item in sparePartItems)
                {
                    itemCategory = itemCateList.FirstOrDefault(p => p.ItemId == item.Id);
                    list.Add(new SparePart()
                    {
                        SparePartCode = item.Code,
                        SparePartName = item.Name,
                        SpartType = (SparePartType)ItemExtension.GetSpartType(item),
                        Specification = item.SpecificationModel,
                        UnitId = (double)item.UnitId,
                        State = item.State,
                        IsImportItem = true,
                        IsReplacement = false,
                        ControlMethod = ControlMethod.ItemCode,
                        ItemCategoryId = (double)(itemCategory == null ? 0 : itemCategory.ItemCategoryId),
                        SparePartItemId = item.Id
                    });
                }

                if (list.Any())
                    RF.Save(list);

                //更新
                sparePartItems.Clear();
                itemCateList.Clear();
                if (itemCodes.Any())
                {
                    sparePartItems = itemCodes.SplitContains(tempCodes => { return Query<Item>().Where(n => tempCodes.Contains(n.Code)).ToList(); });
                    itemCateList = sparePartItems.Select(p => p.Id).SplitContains(tempIds =>
                    {
                        return Query<ItemCategoryRelation>()
                        .Where(p => p.Type == SIE.Items.Items.CategoryType.Item && p.ItemCategory.ItemType == ItemType.SparePart && tempIds.Contains(p.ItemId)).ToList();
                    });
                }

                sparePartItems.ForEach(x =>
                {
                    SparePartType spartType = (SparePartType)ItemExtension.GetSpartType(x);
                    itemCategory = itemCateList.FirstOrDefault(p => p.ItemId == x.Id);

                    DB.Update<SparePart>()
                        .Set(p => p.SparePartName, x.Name)
                        .Set(p => p.SpartType, spartType)
                        .Set(p => p.Specification, x.SpecificationModel)
                        .Set(p => p.UnitId, x.UnitId)
                        .Set(p => p.State, x.State)
                        .Set(p => p.ItemCategoryId, (double)(itemCategory == null ? 0 : itemCategory.ItemCategoryId))
                        .Set(p => p.IsImportItem, true)
                    .Where(p => p.SparePartCode == x.Code).Execute();
                });

                //刪除
                //var deleteSpareParts = Query<SparePart>()
                //     .LeftJoin<Item>((a, b) => a.SparePartCode == b.Code && b.Type == ItemType.SparePart)
                //    .Where<Item>((a, b) => (b.Code == null || b.Code == ""))
                //    .Where(p => p.IsImportItem == true)
                //    .ToList();

                //deleteSpareParts.ForEach(x =>
                //{
                //    DB.Delete<SparePart>().Where(p => p.SparePartCode == x.SparePartCode).Execute();
                //});
                trans.Complete();
            }
        }

        /// <summary>
        /// 获取备件更换记录
        /// </summary>
        /// <param name="equipmentId">设备台账ID</param>
        /// <param name="orderInfoList">排序信息</param>
        /// <param name="pagingInfo">关键字</param>
        /// <param name="isSourceCompleted">来源单据已完成</param>
        /// <returns>备件更换记录列表</returns>
        public virtual EntityList<SparePartChangedRecord> GetSparePartChangedRecords(double equipmentId, List<OrderInfo> orderInfoList, PagingInfo pagingInfo, bool? isSourceCompleted = null)
        {
            var query = Query<SparePartChangedRecord>();
            query.WhereIf(equipmentId > 0, p => p.EquipAccountId == equipmentId);
            query.WhereIf(isSourceCompleted != null, p => p.IsSourceCompleted == isSourceCompleted && p.SparePart.LifeTime != null);

            return query.OrderBy(orderInfoList).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 更新备件更换记录单据完成标记
        /// </summary>
        /// <param name="fromType"></param>
        /// <param name="sourceId"></param>
        public virtual void UpdateSparePartChangedRecordFlag(FromType fromType, double sourceId)
        {
            DB.Update<SparePartChangedRecord>()
                .Where(p => p.Source == fromType && p.SourceId == sourceId)
                .Set(p => p.IsSourceCompleted, true)
                .Execute();
        }

        /// <summary>
        /// 启用备件
        /// </summary>
        /// <param name="idList">备件记录Id</param>
        public virtual void EnableSparePart(List<double> idList)
        {
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                foreach (var id in idList)
                {
                    DB.Update<SparePart>()
                         .Set(x => x.State, State.Enable)
                         .Where(p => p.Id == id)
                         .Execute();
                }
                trans.Complete();
            }
        }

        /// <summary>
        /// 禁用备件
        /// </summary>
        /// <param name="idList">备件记录Id</param>
        public virtual void DisableSparePart(List<double> idList)
        {
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                foreach (var id in idList)
                {
                    DB.Update<SparePart>()
                         .Set(x => x.State, State.Disable)
                         .Where(p => p.Id == id)
                         .Execute();
                }
                trans.Complete();
            }
        }

        /// <summary>
        /// 获取备件入库
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<SparePartStore> GetSparePartStoreList(SparePartStoreCriteria criteria)
        {
            var query = Query<SparePartStore>();

            if (criteria.StoreCode.IsNotEmpty())
            {
                query.Where(p => p.StoreCode.Contains(criteria.StoreCode));
            }

            if (criteria.LinkCode.IsNotEmpty())
            {
                query.Where(p => p.ReceiveNo.Contains(criteria.LinkCode) || p.AcceptanceNo.Contains(criteria.LinkCode) || p.DisposalNo.Contains(criteria.LinkCode));
            }

            if (criteria.Warehouse != null)
            {
                query.Where(p => p.WarehouseId == criteria.WarehouseId);
            }

            if (criteria.Supplier != null)
            {
                query.Where(p => p.SupplierId == criteria.SupplierId);
            }

            if (criteria.InboundType != null)
            {
                query.Where(p => p.InboundType == criteria.InboundType);
            }

            if (criteria.InboundStatus != null)
            {
                query.Where(p => p.InboundStatus == criteria.InboundStatus);
            }

            if (criteria.CreateDate.BeginValue.HasValue)
            {
                query.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue);
            }

            if (criteria.CreateDate.EndValue.HasValue)
            {
                query.Where(p => p.CreateDate <= criteria.CreateDate.EndValue);
            }

            if (criteria.SparePart != null || criteria.SparePartName.IsNotEmpty())
            {
                query.Exists<StoreDetail>((x, y) => y.Where(p => p.SparePartStoreId == x.Id)
                     .WhereIf(criteria.SparePartName.IsNotEmpty(), p => p.SparePart.SparePartName.Contains(criteria.SparePartName))
                     .WhereIf(criteria.SparePart != null, p => p.SparePartId == criteria.SparePartId));
            }

            var list = query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty().LoadWith(SparePartStore.StoreDetailListProperty));

            foreach (var item in list)
            {
                var goodItemNum = item.StoreDetailList.Count(p => p.QualityStatus == QualityStatus.Good);
                var rotItemNum = item.StoreDetailList.Count(p => p.QualityStatus == QualityStatus.Defective);

                if (goodItemNum == item.StoreDetailList.Count())
                {
                    item.QualityStatus = QualityStatus.Good;
                }

                if (rotItemNum == item.StoreDetailList.Count())
                {
                    item.QualityStatus = QualityStatus.Defective;
                }

                item.LinkCode = item.DisposalNo.IsNotEmpty() ? item.DisposalNo : (item.AcceptanceNo.IsNotEmpty() ? item.AcceptanceNo : item.ReceiveNo);
            }

            return list;
        }

        /// <summary>
        /// 获取备件库存列表
        /// </summary>
        /// <param name="criteria">备件库存查询条件</param>
        /// <returns>备件库存列表</returns>
        public virtual EntityList<StoreSummary> GetStoreSummary(StoreSummaryCriteria criteria)
        {
            var query = Query<StoreSummary>();

            if (criteria.SparePart != null)
            {
                query.Where(p => p.SparePartId == criteria.SparePartId);
            }
            if (criteria.SparePartName.IsNotEmpty())
            {
                query.Where(p => p.SparePart.SparePartName.Contains(criteria.SparePartName));
            }
            if (criteria.ItemCategory != null)
            {
                EntityList<ItemCategory> itemCateList = new EntityList<ItemCategory>();
                itemCateList.Add(criteria.ItemCategory);
                GetChildItemCategory(criteria.ItemCategory, itemCateList);
                List<double> itemCateIds = itemCateList.Select(p => p.Id).ToList();
                query.Where(p => itemCateIds.Contains((double)p.SparePart.ItemCategoryId));
            }

            if (criteria.OrderNumber.IsNotEmpty())
            {
                query.Exists<StoreSummaryDetail>((s, d) => d.Where(x => s.Id == x.StoreSummaryId && x.OrderNumberCode == criteria.OrderNumber));
            }

            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取新的备件入库单号
        /// </summary>
        /// <returns>备件入库单号</returns>
        public virtual string GetStoreCode()
        {
            var config = ConfigService.GetConfig(new SparePartStoreNoConfig(), typeof(SparePartStore));
            if (config == null || config.CodeRule == null)
                throw new ValidationException("未找到备件入库单号生成规则,请检查规则配置".L10N());

            string v = RT.Service.Resolve<NumberRuleController>()
                   .GenerateSegment(config.CodeRule.Id, 1)
                   .FirstOrDefault();
            return v;
        }

        /// <summary>
        /// 获取新的备件入库明细批次号
        /// </summary>
        /// <returns>批次号</returns>
        public virtual string GetDetailCode()
        {
            var config = ConfigService.GetConfig(new BatchNumberNoConfig(), typeof(StoreDetail));
            if (config == null || config.BatchNumberRule == null)
                throw new ValidationException("未找到批次号生成规则,请检查规则配置".L10N());

            string v = RT.Service.Resolve<NumberRuleController>()
                   .GenerateSegment(config.BatchNumberRule.Id, 1)
                   .FirstOrDefault();
            return v;
        }

        /// <summary>
        /// 获取新的备件入库明细批次号
        /// </summary>
        /// <returns>批次号</returns>
        public virtual string GetBatchCode()
        {
            var config = ConfigService.GetConfig(new BatchNumberNoConfig(), typeof(StoreSummary));
            if (config == null || config.BatchNumberRule == null)
                throw new ValidationException("未找到批次号生成规则,请检查规则配置".L10N());

            string v = RT.Service.Resolve<NumberRuleController>()
                   .GenerateSegment(config.BatchNumberRule.Id, 1)
                   .FirstOrDefault();
            return v;
        }

        /// <summary>
        /// 获取新的备件入库明细序列号
        /// </summary>
        /// <returns>序列号</returns>
        public virtual IEnumerable<string> GetSnCode(int qty)
        {
            var config = ConfigService.GetConfig(new BatchNumberNoConfig(), typeof(StoreSummary));
            if (config == null || config.SnNumberRule == null)
                throw new ValidationException("未找到序列号生成规则,请检查规则配置".L10N());

            var v = RT.Service.Resolve<NumberRuleController>()
                   .GenerateSegment(config.SnNumberRule.Id, qty);
            return v;
        }

        /// <summary>
        /// 获取新的备件入库明细批次号
        /// </summary>
        /// <returns>批次号</returns>
        public virtual List<string> GetDetailCodes(int count)
        {
            var config = ConfigService.GetConfig(new BatchNumberNoConfig(), typeof(StoreDetail));
            if (config == null || config.BatchNumberRule == null)
                throw new ValidationException("未找到批次号生成规则,请检查规则配置".L10N());

            return RT.Service.Resolve<NumberRuleController>()
                   .GenerateSegment(config.BatchNumberRule.Id, count).ToList();
        }

        /// <summary>
        /// 是否计算备件平均成本
        /// </summary>
        /// <returns>bool</returns>
        public virtual bool IsComputeAvgCost()
        {
            var config = ConfigService.GetConfig(new IsComputeAvgCostConfig(), typeof(SparePartStore));
            if (config == null)
                throw new ValidationException("未找到是否计算备件平均成本的配置,请检查配置项".L10N());
            return config.IsComputeAvgCost;
        }

        /// <summary>
        /// 获取入库明细批次号打印模板
        /// </summary>
        /// <returns>bool</returns>
        public virtual PrintTemplate GetStoreDetailLotPrintConfig()
        {
            var config = ConfigService.GetConfig(new StoreDetailLabelPrintConfig(), typeof(SparePartStore));
            if (config == null)
                throw new ValidationException("未找到入库明细批次号和序列号打印模板配置规则,请检查配置项".L10N());

            if (config.LotPrintTemplate == null)
                throw new ValidationException("入库明细批次号打印模板为空,请检查配置项".L10N());

            return config.LotPrintTemplate;
        }

        /// <summary>
        /// 获取入库明细序列号打印模板
        /// </summary>
        /// <returns>bool</returns>
        public virtual PrintTemplate GetStoreDetailSnPrintConfig()
        {
            var config = ConfigService.GetConfig(new StoreDetailLabelPrintConfig(), typeof(SparePartStore));
            if (config == null)
                throw new ValidationException("未找到入库明细批次号和序列号打印模板配置规则,请检查配置项".L10N());

            if (config.SnPrintTempldate == null)
                throw new ValidationException("入库明细序列号打印模板为空,请检查配置项".L10N());

            return config.SnPrintTempldate;
        }

        /// <summary>
        /// 获取备件基础数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual SparePart GetSparePart(double id)
        {
            var q = Query<SparePart>();
            q.Where(p => p.Id == id);

            var elo = new EagerLoadOptions();
            elo.LoadWith(SparePart.UnitProperty);

            var result = q.FirstOrDefault(elo);

            return result;
        }

        /// <summary>
        /// 获取备件基础数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public virtual SparePart GetSparePart(string code)
        {
            return Query<SparePart>().Where(p => p.SparePartCode == code).FirstOrDefault();
        }

        /// <summary>
        /// 获取备件基础数据
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual EntityList<SparePart> GetSpareParts(PagingInfo pagingInfo, string key)
        {
            var q = Query<SparePart>();
            if (key.IsNotEmpty())
                q.Where(p => p.SparePartCode.Contains(key) || p.SparePartName.Contains(key));

            var elo = new EagerLoadOptions();
            elo.LoadWith(SparePart.UnitProperty);
            elo.LoadWithViewProperty();

            return q.ToList(pagingInfo, elo);
        }

        /// <summary>
        /// 获取备件基础数据
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual EntityList<SparePart> GetEnableSpareParts(PagingInfo pagingInfo, string key)
        {
            var q = Query<SparePart>().Where(p => p.State == State.Enable);
            if (key.IsNotEmpty())
                q.Where(p => p.SparePartCode.Contains(key) || p.SparePartName.Contains(key));

            var elo = new EagerLoadOptions();
            elo.LoadWith(SparePart.UnitProperty);
            elo.LoadWithViewProperty();

            return q.ToList(pagingInfo, elo);
        }

        /// <summary>
        /// 获取备件基础数据
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        public virtual EntityList<SparePart> GetSpareParts(List<double> idList)
        {
            var elo = new EagerLoadOptions();
            elo.LoadWith(SparePart.UnitProperty);
            elo.LoadWithViewProperty();

            return idList.SplitContains((ids) =>
            {
                return Query<SparePart>().Where(p => ids.Contains(p.Id)).ToList(null, elo);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <param name="key"></param>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public virtual EntityList<SparePart> GetSparePartsByModelCode(PagingInfo pagingInfo, string key, double? modelId)
        {
            var q = Query<SparePart>().Where(p => p.State == State.Enable);
            if (key.IsNotEmpty())
                q.Where(p => p.SparePartCode.Contains(key) || p.SparePartName.Contains(key));

            if (modelId.HasValue && modelId != 0)
            {
                q.Where(m => m.SpartEquipModelId == modelId || m.SpartEquipModelId == null);
            }

            var list = q.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }

        /// <summary>
        /// 根据设备模型获取备件
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<SparePart> GetSparePartByAccount(SparePartByEquipModelCriteria criteria)
        {
            var query = DB.Query<SparePart>();

            var result = query.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return result;
        }

        /// <summary>
        /// 获取备件总库存数
        /// </summary>
        /// <param name="sparePartIds">备件ID列表</param>
        /// <returns></returns>
        public virtual IList<EmsAttachmentInfo> GetSparePartPictureAttachments(IList<double> sparePartIds)
        {
            List<EmsAttachmentInfo> emsAttachmentInfos = new List<EmsAttachmentInfo>();
            sparePartIds.SplitDataExecute(ids =>
            {
                var list = Query<SparePartPictureAttachment>().Where(p => ids.Contains((double)p.OwnerId))
                .Select(x => new
                {
                    OwnerId = x.OwnerId,
                    FileName = x.FileName,
                    FilePath = x.FilePath,
                    FileExtension = x.FileExtesion
                }).ToList<EmsAttachmentInfo>().ToList();
                emsAttachmentInfos.AddRange(list);
            });
            return emsAttachmentInfos;

        }

        /// <summary>
        /// 获取备件基础数据并且为可用状态
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual EntityList<SparePart> GetSparePartsToState(PagingInfo pagingInfo, string key)
        {
            var q = Query<SparePart>();
            if (key.IsNotEmpty())
                q.Where(p => p.SparePartCode.Contains(key) || p.SparePartName.Contains(key));
            q.Where(p => p.State == State.Enable);

            var elo = new EagerLoadOptions().LoadWithViewProperty();
            return q.ToList(pagingInfo, elo);
        }

        /// <summary>
        /// 获取序列号管控的备件基础数据
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">查询关键字</param>
        /// <returns>序列号管控的备件基础数据集合</returns>
        public virtual EntityList<SparePart> GetSnControlSpareParts(PagingInfo pagingInfo, string keyword)
        {
            var q = Query<SparePart>().Where(p => p.ControlMethod == ControlMethod.Sn);

            if (keyword.IsNotEmpty())
            {
                q.Where(p => p.SparePartCode.Contains(keyword) || p.SparePartName.Contains(keyword));
            }

            var elo = new EagerLoadOptions().LoadWithViewProperty();
            return q.ToList(pagingInfo, elo);
        }

        /// <summary>
        /// 获取备件基础数据
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<SparePart> GetStandardSparePartList(StandardSparePartSelCriteria criteria)
        {
            if (criteria == null)
            {
                return new EntityList<SparePart>();
            }

            List<double> projectDetailIds = new List<double>();
            var EquipModelRepairProjectList = Query<EquipModelRepairProject>().Where(p => p.EquipModelId == criteria.EquipModelId).ToList();

            if (criteria.ProjectDetailId.HasValue)
            {
                projectDetailIds = EquipModelRepairProjectList.Where(p => p.ProjectDetailId == criteria.ProjectDetailId).Select(p => p.ProjectDetailId).ToList();
            }
            else
            {
                projectDetailIds = EquipModelRepairProjectList.Select(p => p.ProjectDetailId).ToList();
            }

            var sparePartIds = Query<SparePartItem>().Where(p => projectDetailIds.Contains(p.ProjectDetailId)).ToList().Select(p => p.SparePartId);

            if (!sparePartIds.Any())
            {
                return new EntityList<SparePart>();
            }

            var query = Query<SparePart>();

            query.Where(p => p.State == State.Enable);

            if (criteria.SparePartCode.IsNotEmpty())
            {
                query.Where(p => p.SparePartCode == criteria.SparePartCode);
            }
            if (criteria.SparePartName.IsNotEmpty())
            {
                query.Where(p => p.SparePartName == criteria.SparePartName);
            }

            if (sparePartIds.Any())
            {
                query.Where(p => sparePartIds.Contains(p.Id));
            }

            var elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();
            return query.ToList(criteria.PagingInfo, elo);
        }

        /// <summary>
        /// 获取备件基础数据
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<SparePart> GetSparePartList(SparePartSelCriteria criteria)
        {
            var q = Query<SparePart>().Where(p => p.State == State.Enable);

            if (criteria.SparePartCode.IsNotEmpty())
            {
                q.Where(p => p.SparePartCode == criteria.SparePartCode);
            }

            if (criteria.SparePartName.IsNotEmpty())
            {
                q.Where(p => p.SparePartName == criteria.SparePartName);
            }

            if (criteria.ModelCode.IsNotEmpty())
            {
                q.Where(p => p.SpartEquipModel.Code == criteria.ModelCode || p.SpartEquipModelId == null);
            }

            var sparePartList = q.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            return sparePartList;
        }

        /// <summary>
        /// 获取备件基础数据
        /// </summary>
        /// <param name="criteria">备件查询条件</param>
        /// <returns></returns>
        public virtual EntityList<SparePart> GetSparePartList(SparePartCriteria criteria)
        {
            var q = Query<SparePart>().As("sp").LeftJoin<StoreSummary>("st", (p, s) => p.Id == s.SparePartId);

            if (criteria.SparePartCode.IsNotEmpty())
                q.Where(p => p.SparePartCode.Contains(criteria.SparePartCode));
            if (criteria.SparePartName.IsNotEmpty())
                q.Where(p => p.SparePartName.Contains(criteria.SparePartName));
            if (criteria.SpartType.HasValue)
                q.Where(p => p.SpartType == criteria.SpartType);
            if (criteria.SpartEquipModel != null)
                q.Where(p => p.SpartEquipModelId == criteria.SpartEquipModelId);
            if (criteria.CreateDate.BeginValue.HasValue)
                q.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue);
            if (criteria.CreateDate.EndValue.HasValue)
                q.Where(p => p.CreateDate <= criteria.CreateDate.EndValue);
            if (criteria.ItemCategory != null)
            {
                EntityList<ItemCategory> itemCateList = new EntityList<ItemCategory>();
                itemCateList.Add(criteria.ItemCategory);
                GetChildItemCategory(criteria.ItemCategory, itemCateList);
                List<double> itemCateIds = itemCateList.Select(p => p.Id).ToList();
                q.Where(p => itemCateIds.Contains((double)p.ItemCategoryId));
            }

            if (criteria.State !=null)
            {
                q.Where(p => p.State == criteria.State.Value);
            }
            if (criteria.ControlMethod.HasValue)
                q.Where(p => p.ControlMethod == criteria.ControlMethod);
            if (criteria.StorageState.HasValue)
            {
                if (criteria.StorageState == Enums.StorageState.HigherStorage)
                {
                    q.Where(p => p.SQL<bool>(new Data.FormattedSql("sp.SAFE_STOCK <= st.GOOD_NUMBER or (sp.SAFE_STOCK = 0 and st.GOOD_NUMBER is null)")));
                }
                else
                {
                    q.Where(p => p.SQL<bool>(new Data.FormattedSql("sp.SAFE_STOCK > st.GOOD_NUMBER or (sp.SAFE_STOCK>0 and st.GOOD_NUMBER is null)")));
                }
            }

            var sparePartList = q.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            var storeList = sparePartList.Select(p => p.Id).SplitContains(tempIds =>
            {
                return Query<StoreSummary>().Where(p => tempIds.Contains(p.SparePartId)).ToList();
            });

            foreach (var sparePart in sparePartList)
            {
                var store = storeList.FirstOrDefault(p => p.SparePartId == sparePart.Id);
                if (store != null)
                {
                    sparePart.GoodNumber = store.GoodNumber;
                    sparePart.RotNumber = store.RotNumber;
                }
            }
            return sparePartList;
        }

        /// <summary>
        /// 查询备件
        /// </summary>
        /// <param name="partType">备件类型</param>
        /// <param name="itemCategoryId">备件分类</param>
        /// <param name="keyWord">关键字</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns></returns>
        public virtual EntityList GetSparePartList(SparePartType? partType, double? itemCategoryId, string keyWord, PagingInfo pagingInfo)
        {
            var query = Query<SparePart>();
            if (partType.HasValue)
            {
                query.Where(x => x.SpartType == partType.Value);
            }

            if (itemCategoryId.HasValue)
            {
                query.Where(x => x.ItemCategoryId == itemCategoryId.Value);
            }

            if (!keyWord.IsNullOrEmpty())
            {
                query.Where(x => x.SparePartCode.Contains(keyWord) || x.SparePartName.Contains(keyWord));
            }

            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }


        /// <summary>
        /// 查询当前分类下的所有子类
        /// </summary>
        /// <param name="itemCate">当前分类</param>
        /// <param name="itemCateList">子类列表</param>
        private void GetChildItemCategory(ItemCategory itemCate, EntityList<ItemCategory> itemCateList)
        {
            var childItemCateList = Query<ItemCategory>().Where(p => p.TreePId == itemCate.Id).ToList();
            if (childItemCateList.Any())
            {
                itemCateList.AddRange(childItemCateList);
                foreach (var item in childItemCateList)
                {
                    GetChildItemCategory(item, itemCateList);
                }
            }
        }

        /// <summary>
        /// 是否启用WMS管理备件
        /// </summary>
        /// <returns>bool</returns>
        public virtual bool IsWmsControl()
        {
            var config = ConfigService.GetConfig(new IsWmsControlConfig());

            if (config == null)
            {
                throw new ValidationException("未找到备件是否启用WMS管理配置,请检查配置项".L10N());
            }
            else
            {
                return config.IsWmsControl == YesNo.Yes;
            }
        }

        /// <summary>
        /// 根据类型和仓库获取备件信息
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="criteria">备件查询信息</param>
        /// <returns>备件列表</returns>
        public virtual EntityList<SparePart> GetSparePartByTypeDepot(PagingInfo pagingInfo, TypeDepotQueryInfo criteria)
        {
            var entityQueryer = Query<SparePart>().As("sp");
            if (criteria.Keyword.IsNotEmpty())
                entityQueryer.Where(p => p.SparePartCode.Contains(criteria.Keyword) || p.SparePartName.Contains(criteria.Keyword));
            if (criteria.TypeId.HasValue)
                entityQueryer.Where(p => p.ItemCategoryId == criteria.TypeId.Value);

            var iq = entityQueryer.ToQuery();

            if (criteria.DepotId.HasValue)
            {
                var f = QueryFactory.Instance;

                var idProperty = entityQueryer.Repository.EntityMeta.ManagedProperties
                    .FindProperty(SparePart.IdProperty.Name);
                IColumnNode idColum = iq.MainTable.FindColumn(idProperty);

                var sparePartIdProperty = RF.Find<StoreSummary>().EntityMeta.ManagedProperties
                   .FindProperty(StoreSummary.SparePartIdProperty.Name);

                var subQueryLot = DB.Query<StoreSummary>("sm1")
                    .Join<StoreSummaryLot>((x, y) => x.Id == y.StoreSummaryId)
                    .Where<StoreSummaryLot>((x, y) => y.WarehouseId == criteria.DepotId && y.GoodNumber > 0)
                    .ToQuery();

                IColumnNode sparePartIdColumnLot = subQueryLot.MainTable.FindColumn(sparePartIdProperty);
                subQueryLot.Where = subQueryLot.Where.And(f.Constraint(idColum, sparePartIdColumnLot));

                var subQueryLoc = DB.Query<StoreSummary>("sm2")
                    .Join<StoreSummaryLocation>((x, y) => x.Id == y.StoreSummaryId)
                    .Where<StoreSummaryLocation>((x, y) => y.WarehouseId == criteria.DepotId && y.GoodNumber > 0)
                    .ToQuery();

                IColumnNode sparePartIdColumnOfLoc = subQueryLoc.MainTable.FindColumn(sparePartIdProperty);
                subQueryLoc.Where = subQueryLoc.Where.And(f.Constraint(idColum, sparePartIdColumnOfLoc));

                var subQuerySn = DB.Query<StoreSummary>("sm3")
                    .Join<StoreSummaryDetail>((x, y) => x.Id == y.StoreSummaryId)
                    .Where<StoreSummaryDetail>((x, y) => y.WarehouseId == criteria.DepotId && y.GoodNumber > 0)
                    .ToQuery();

                IColumnNode sparePartIdColumnOfSn = subQuerySn.MainTable.FindColumn(sparePartIdProperty);
                subQuerySn.Where = subQuerySn.Where.And(f.Constraint(idColum, sparePartIdColumnOfSn));

                iq.Where = f.And(iq.Where, f.Or(f.Exists(subQueryLot), f.Exists(subQueryLoc), f.Exists(subQuerySn)));
            }

            return entityQueryer.Repository.QueryList(iq, pagingInfo, new EagerLoadOptions().LoadWithViewProperty()) as EntityList<SparePart>;
        }

        /// <summary>
        /// 根据id获取备件基础数据
        /// </summary>
        /// <param name="id">备件id</param>
        /// <returns>备件基础数据</returns>
        public virtual SparePart GetSparePartById(double id)
        {
            return Query<SparePart>().Where(p => p.Id == id).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据备件编码获取备件基础数据
        /// </summary>
        /// <param name="code">备件编码</param>
        /// <returns>备件基础数据</returns>
        public virtual SparePart GetSparePartByCode(string code)
        {
            return Query<SparePart>().Where(p => p.SparePartCode == code).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取设备型号数据
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<EquipModel> GetEquipModel(PagingInfo pagingInfo, string keyword)
        {
            var list = Query<EquipModel>();

            if (!keyword.IsNullOrEmpty())
            {
                list.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            return list.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual object GetStoreDetailById(double id)
        {
            var query = Query<SparePart>().Where(p => p.Id == id);
            var sparePart = query.FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            var model = new { EquipModelId = sparePart.SpartEquipModelId, EquipModelCode = sparePart.EquipModelCode };
            return model;
        }

        /// <summary>
        /// 获取备件总库存数
        /// </summary>
        /// <param name="sparePartIds">备件ID列表</param>
        /// <returns></returns>
        public virtual EntityList<StoreSummary> GetStoreSummarys(IList<double> sparePartIds)
        {
            return sparePartIds.SplitContains(ids =>
            {
                return Query<StoreSummary>().Where(p => ids.Contains(p.SparePartId)).ToList();
            });


        }

        /// <summary>
        /// 计算备件库存
        /// </summary>
        /// <param name="sparePartIds"></param>
        /// <returns></returns>
        public virtual Dictionary<double, int> CountSpareStoreSummary(IList<double> sparePartIds)
        {
            Dictionary<double, int> storeDic = new Dictionary<double, int>();
            sparePartIds.SplitDataExecute(tempIds =>
            {
                var list = Query<StoreSummary>().Where(p => tempIds.Contains(p.SparePartId)).Select(p => new
                {
                    Id = p.SparePartId,
                    Value = p.GoodNumber,
                }).ToList<SpareStoryInfo>();
                foreach(var i in list)
                {
                    if (storeDic.ContainsKey(i.Id))
                    {
                        storeDic[i.Id] += i.Value;
                    }
                    else
                    {
                        storeDic.Add(i.Id, i.Value);
                    }
                }
            });
            return storeDic;
        }

        /// <summary>
        /// 获取备件库存
        /// </summary>
        /// <param name="sparePartId">备件ID</param>
        /// <param name="warehouseId">仓库ID</param>
        /// <returns></returns>
        public virtual int GetSparePartStoreQty(double sparePartId, double warehouseId)
        {
            //获取备件基础资料编码
            var sparePart = Query<SparePart>().Where(x => x.Id == sparePartId).FirstOrDefault();
            if (sparePart == null)
            {
                throw new ValidationException("备件基础资料为空".L10N());
            }

            int stockQty = 0;

            switch (sparePart.ControlMethod)
            {
                case ControlMethod.ItemCode:
                    var queryStoreSummaryLocation = Query<StoreSummaryLocation>()
                        .Where(p => p.StoreSummary.SparePartId == sparePartId && p.WarehouseId == warehouseId)
                        .Select(p => p.GoodNumber.SUM());

                    stockQty = queryStoreSummaryLocation.FirstOrDefault<int>();

                    break;
                case ControlMethod.Batch:
                    var queryStoreSummaryLot = Query<StoreSummaryLot>()
                        .Where(p => p.StoreSummary.SparePartId == sparePartId && p.WarehouseId == warehouseId)
                        .Select(p => p.GoodNumber.SUM());

                    stockQty = queryStoreSummaryLot.FirstOrDefault<int>();

                    break;
                case ControlMethod.Sn:
                    var queryStoreSummaryDetail = Query<StoreSummaryDetail>()
                        .Where(p => p.StoreSummary.SparePartId == sparePartId && p.WarehouseId == warehouseId)
                        .Select(p => p.GoodNumber.SUM());

                    stockQty = queryStoreSummaryDetail.FirstOrDefault<int>();
                    break;
                default:
                    stockQty = 0;
                    break;
            }

            return stockQty;
        }

        /// <summary>
        ///  筛选类型为备件入库的库存
        /// </summary>        
        /// <param name="sparePartIds">备件基础资料ID列表</param>
        /// <param name="whIds">仓库ID列表</param>
        /// <returns></returns>
        public virtual IList<SparePartWarehouseInfo> GetStoreSummaryDepots(List<double> sparePartIds, List<double> whIds = null)
        {
            var spareParts = sparePartIds.SplitContains(tempIds =>
            {
                return Query<SparePart>().Where(x => tempIds.Contains(x.Id)).ToList();
            });

            List<SparePartWarehouseInfo> sparePartWarehouseInfos = new List<SparePartWarehouseInfo>();

            AddControlByBatchStocks(whIds, spareParts, sparePartWarehouseInfos);

            AddControlByCodeStocks(whIds, spareParts, sparePartWarehouseInfos);

            AddControlBySnStocks(whIds, spareParts, sparePartWarehouseInfos);

            return sparePartWarehouseInfos;
        }

        /// <summary>
        /// 获取按批次管控的库存信息
        /// </summary>
        /// <param name="whIds"></param>
        /// <param name="spareParts"></param>
        /// <param name="sparePartWarehouseInfos"></param>
        private void AddControlByBatchStocks(List<double> whIds, EntityList<SparePart> spareParts,
            List<SparePartWarehouseInfo> sparePartWarehouseInfos)
        {
            var batchs = spareParts.Where(x => x.ControlMethod == ControlMethod.Batch);
            if (batchs.Any())
            {
                batchs.Select(x => x.Id).Distinct().SplitDataExecute(tempIds =>
                {

                    var q = Query<StoreSummaryLot>()
                        .Join<StoreSummary>((x, y) => x.StoreSummaryId == y.Id)
                        .Where(p => p.GoodNumber > 0)
                        .Where<StoreSummary>((x, y) => tempIds.Contains(y.SparePartId))
                        .GroupBy<StoreSummary>((x, y) => new
                        {
                            y.SparePartId,
                            x.WarehouseId,
                            x.Warehouse.Code,
                            x.Warehouse.Name,
                        })
                        .Select<StoreSummary>((x, y) => new
                        {
                            SparePartId = y.SparePartId,
                            WarehouseId = x.WarehouseId,
                            WarehouseCode = x.Warehouse.Code,
                            WarehouseName = x.Warehouse.Name,
                            StoreQty = x.GoodNumber.SUM()
                        });

                    //过滤仓库
                    if (whIds != null && whIds.Any())
                    {
                        var expWh = whIds.CreateContainsExpression<StoreSummaryLot>("y",
                            StoreSummaryLot.WarehouseIdProperty.Name);
                        if (expWh != null)
                        {
                            q.Where(expWh);
                        }
                    }

                    sparePartWarehouseInfos.AddRange(q.OrderBy(p => p.WarehouseId).ToList<SparePartWarehouseInfo>());
                });
            }
        }

        /// <summary>
        /// 获取按编码管控的库存信息
        /// </summary>
        /// <param name="whIds"></param>
        /// <param name="spareParts"></param>
        /// <param name="sparePartWarehouseInfos"></param>
        private void AddControlByCodeStocks(List<double> whIds, EntityList<SparePart> spareParts,
            List<SparePartWarehouseInfo> sparePartWarehouseInfos)
        {
            var batchs = spareParts.Where(x => x.ControlMethod == ControlMethod.ItemCode);
            if (batchs.Any())
            {
                batchs.Select(x => x.Id).Distinct().SplitDataExecute(tempIds =>
                {
                    var storeSummaries = Query<StoreSummary>().Where(x => tempIds.Contains(x.SparePartId))
                        .Select(x => x.Id)
                        .ToList();

                    if (storeSummaries.Any())
                    {
                        var exp = storeSummaries.Select(x => x.Id).Distinct().ToList()
                            .CreateContainsExpression<StoreSummaryLocation>("x", StoreSummaryLocation.StoreSummaryIdProperty.Name);

                        if (exp != null)
                        {
                            var q = Query<StoreSummaryLocation>()
                                .Join<StoreSummary>((x, y) => x.StoreSummaryId == y.Id)
                                .Where(p => p.GoodNumber > 0).Where(exp)
                                .GroupBy<StoreSummary>((x, y) => new
                                {
                                    y.SparePartId,
                                    x.WarehouseId,
                                    x.Warehouse.Code,
                                    x.Warehouse.Name,
                                })
                                .Select<StoreSummary>((x, y) => new
                                {
                                    SparePartId = y.SparePartId,
                                    WarehouseId = x.WarehouseId,
                                    WarehouseCode = x.Warehouse.Code,
                                    WarehouseName = x.Warehouse.Name,
                                    StoreQty = x.GoodNumber.SUM()
                                });

                            //过滤仓库
                            if (whIds != null && whIds.Any())
                            {
                                var expWh = whIds.CreateContainsExpression<StoreSummaryLocation>("y",
                                    StoreSummaryLot.WarehouseIdProperty.Name);

                                if (expWh != null)
                                {
                                    q.Where(expWh);
                                }
                            }

                            sparePartWarehouseInfos.AddRange(q.OrderBy(p => p.WarehouseId).ToList<SparePartWarehouseInfo>());
                        }
                    }
                });
            }
        }

        /// <summary>
        /// 获取按序列号管控的库存信息
        /// </summary>
        /// <param name="whIds"></param>
        /// <param name="spareParts"></param>
        /// <param name="sparePartWarehouseInfos"></param>
        private void AddControlBySnStocks(List<double> whIds, EntityList<SparePart> spareParts,
            List<SparePartWarehouseInfo> sparePartWarehouseInfos)
        {
            var batchs = spareParts.Where(x => x.ControlMethod == ControlMethod.Sn);
            if (batchs.Any())
            {
                batchs.Select(x => x.Id).Distinct().SplitDataExecute(tempIds =>
                {
                    var storeSummaries = Query<StoreSummary>().Where(x => tempIds.Contains(x.SparePartId))
                        .Select(x => x.Id)
                        .ToList();

                    if (storeSummaries.Any())
                    {
                        var exp = storeSummaries.Select(x => x.Id).Distinct().ToList()
                            .CreateContainsExpression<StoreSummaryDetail>("x", StoreSummaryDetail.StoreSummaryIdProperty.Name);

                        if (exp != null)
                        {
                            var q = Query<StoreSummaryDetail>()
                                .Join<StoreSummary>((x, y) => x.StoreSummaryId == y.Id)
                                .Where(p => p.GoodNumber > 0).Where(exp)
                                .GroupBy<StoreSummary>((x, y) => new
                                {
                                    y.SparePartId,
                                    x.WarehouseId,
                                    x.Warehouse.Code,
                                    x.Warehouse.Name,
                                })
                                .Select<StoreSummary>((x, y) => new
                                {
                                    SparePartId = y.SparePartId,
                                    WarehouseId = x.WarehouseId,
                                    WarehouseCode = x.Warehouse.Code,
                                    WarehouseName = x.Warehouse.Name,
                                    StoreQty = x.GoodNumber.SUM()
                                });

                            //过滤仓库
                            if (whIds != null && whIds.Any())
                            {
                                var expWh = whIds.CreateContainsExpression<StoreSummaryDetail>("y",
                                    StoreSummaryLot.WarehouseIdProperty.Name);
                                if (expWh != null)
                                {
                                    q.Where(expWh);
                                }
                            }

                            sparePartWarehouseInfos.AddRange(q.OrderBy(p => p.WarehouseId).ToList<SparePartWarehouseInfo>());
                        }
                    }
                });
            }
        }

        /// <summary>
        ///  获取仓库明细
        /// </summary>
        /// <param name="sortInfo">排序信息</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="storeSummary">库存汇总记录</param>
        /// <param name="sparePart">备件基础资料</param>
        /// <returns>仓库明细列表</returns>
        public virtual IList<StoreSummaryWarehouse> GetStoreSummaryWarehouseList(
            IList<OrderInfo> sortInfo, PagingInfo pagingInfo, StoreSummary storeSummary,
            SparePart sparePart = null)
        {
            if (sparePart == null)
            {
                sparePart = storeSummary.SparePart;
            }

            IList<StoreSummaryWarehouse> list = null;
            if (sparePart.ControlMethod == ControlMethod.ItemCode)
            {
                //按备件编码管控

                var q = Query<StoreSummaryLocation>().Where(p => p.StoreSummaryId == storeSummary.Id);

                q.GroupBy(p => new
                {
                    p.WarehouseId,
                    p.Warehouse.Code,
                    p.Warehouse.Name,
                    p.Warehouse.LibraryType,
                });

                q.Select(p => new
                {
                    WarehouseId = p.WarehouseId,
                    WarehouseCode = p.Warehouse.Code,
                    WarehouseName = p.Warehouse.Name,
                    LibraryType = p.Warehouse.LibraryType,
                    RotNumber = p.RotNumber.SUM(),
                    GoodNumber = p.GoodNumber.SUM(),
                    SumNumber = p.SumNumber.SUM()
                });

                list = q.ToList<StoreSummaryWarehouse>();
            }
            else if (sparePart.ControlMethod == ControlMethod.Batch)
            {
                //按批次管控

                var q = Query<StoreSummaryLot>().Where(p => p.StoreSummaryId == storeSummary.Id);

                q.GroupBy(p => new
                {
                    p.WarehouseId,
                    p.Warehouse.Code,
                    p.Warehouse.Name,
                    p.Warehouse.LibraryType,
                });

                q.Select(p => new
                {
                    WarehouseId = p.WarehouseId,
                    WarehouseCode = p.Warehouse.Code,
                    WarehouseName = p.Warehouse.Name,
                    LibraryType = p.Warehouse.LibraryType,
                    RotNumber = p.RotNumber.SUM(),
                    GoodNumber = p.GoodNumber.SUM(),
                    SumNumber = p.SumNumber.SUM()
                });

                list = q.ToList<StoreSummaryWarehouse>();
            }
            else
            {
                //按序列号管控
                var q = Query<StoreSummaryDetail>().Where(p => p.StoreSummaryId == storeSummary.Id);

                q.GroupBy(p => new
                {
                    p.WarehouseId,
                    p.Warehouse.Code,
                    p.Warehouse.Name,
                    p.Warehouse.LibraryType,
                });

                q.Select(p => new
                {
                    WarehouseId = p.WarehouseId,
                    WarehouseCode = p.Warehouse.Code,
                    WarehouseName = p.Warehouse.Name,
                    LibraryType = p.Warehouse.LibraryType,
                    RotNumber = p.RotNumber.SUM(),
                    GoodNumber = p.GoodNumber.SUM(),
                    SumNumber = p.SumNumber.SUM()
                });

                list = q.ToList<StoreSummaryWarehouse>();
            }

            if (list.Any())
            {
                var whList = list.Select(p => p.WarehouseCode).SplitContains(tempCodes =>
            {
                return Query<Warehouse>().Where(p => tempCodes.Contains(p.Code)).ToList();
            });

                list.ForEach(data =>
                {
                    data.IsZeroCost = whList.First(p => p.Code == data.WarehouseCode).GetIsZeroCost();
                });
            }

            return list;
        }

        /// <summary>
        ///  获取库位明细
        /// </summary>
        /// <param name="sortInfo">排序信息</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="storeSummary">库存汇总记录</param>
        /// <param name="sparePart">备件基础资料</param>
        /// <returns>库位明细列表</returns>
        public virtual IList<StoreSummaryStock> GetStoreSummaryStockList(
            IList<OrderInfo> sortInfo, PagingInfo pagingInfo, StoreSummary storeSummary,
            SparePart sparePart = null)
        {
            if (sparePart == null)
            {
                sparePart = storeSummary.SparePart;
            }

            IList<StoreSummaryStock> list = null;
            if (sparePart.ControlMethod == ControlMethod.ItemCode)
            {
                //按备件编码管控

                var q = Query<StoreSummaryLocation>().Where(p => p.StoreSummaryId == storeSummary.Id);

                q.GroupBy(p => new
                {
                    WarehouseCode = p.Warehouse.Code,
                    WarehouseName = p.Warehouse.Name,
                    p.Warehouse.LibraryType,
                    StorageLocationCode = p.StorageLocation.Code,
                    StorageLocationName = p.StorageLocation.Name,
                });

                q.Select(p => new
                {
                    WarehouseCode = p.Warehouse.Code,
                    WarehouseName = p.Warehouse.Name,
                    LibraryType = p.Warehouse.LibraryType,
                    StorageLocationCode = p.StorageLocation.Code,
                    StorageLocationName = p.StorageLocation.Name,
                    RotNumber = p.RotNumber.SUM(),
                    GoodNumber = p.GoodNumber.SUM(),
                    SumNumber = p.SumNumber.SUM()
                });

                list = q.ToList<StoreSummaryStock>();
            }
            else if (sparePart.ControlMethod == ControlMethod.Batch)
            {
                //按批次管控

                var q = Query<StoreSummaryLot>().Where(p => p.StoreSummaryId == storeSummary.Id);

                q.GroupBy(p => new
                {
                    WarehouseCode = p.Warehouse.Code,
                    WarehouseName = p.Warehouse.Name,
                    p.Warehouse.LibraryType,
                    StorageLocationCode = p.StorageLocation.Code,
                    StorageLocationName = p.StorageLocation.Name,
                });

                q.Select(p => new
                {
                    WarehouseCode = p.Warehouse.Code,
                    WarehouseName = p.Warehouse.Name,
                    LibraryType = p.Warehouse.LibraryType,
                    StorageLocationCode = p.StorageLocation.Code,
                    StorageLocationName = p.StorageLocation.Name,
                    RotNumber = p.RotNumber.SUM(),
                    GoodNumber = p.GoodNumber.SUM(),
                    SumNumber = p.SumNumber.SUM()
                });

                list = q.ToList<StoreSummaryStock>();
            }
            else
            {
                //按序列号管控
                var q = Query<StoreSummaryDetail>().Where(p => p.StoreSummaryId == storeSummary.Id);

                q.GroupBy(p => new
                {
                    WarehouseCode = p.Warehouse.Code,
                    WarehouseName = p.Warehouse.Name,
                    p.Warehouse.LibraryType,
                    StorageLocationCode = p.StorageLocation.Code,
                    StorageLocationName = p.StorageLocation.Name,
                });

                q.Select(p => new
                {
                    WarehouseCode = p.Warehouse.Code,
                    WarehouseName = p.Warehouse.Name,
                    LibraryType = p.Warehouse.LibraryType,
                    StorageLocationCode = p.StorageLocation.Code,
                    StorageLocationName = p.StorageLocation.Name,
                    RotNumber = p.RotNumber.SUM(),
                    GoodNumber = p.GoodNumber.SUM(),
                    SumNumber = p.SumNumber.SUM()
                });

                list = q.ToList<StoreSummaryStock>();
            }

            if (list.Any())
            {
                var whList = list.Select(p => p.WarehouseCode).SplitContains(tempCodes =>
                {
                    return Query<Warehouse>().Where(p => tempCodes.Contains(p.Code)).ToList();
                });

                list.ForEach(data =>
                {
                    data.IsZeroCost = whList.First(p => p.Code == data.WarehouseCode).GetIsZeroCost();
                });
            }

            return list;
        }

        /// <summary>
        /// 获取库位明细(带批次号）
        /// </summary>
        /// <param name="sortInfo">排序信息</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="storeSummary">库存汇总记录</param>
        /// <param name="sparePart">备件基础资料</param>
        /// <returns>库位明细列表</returns>
        public virtual IList<StoreSummaryStockLot> GetStoreSummaryStockLotList(
            IList<OrderInfo> sortInfo, PagingInfo pagingInfo, StoreSummary storeSummary,
            SparePart sparePart = null)
        {
            if (sparePart == null)
            {
                sparePart = storeSummary.SparePart;
            }

            IList<StoreSummaryStockLot> list = null;
            if (sparePart.ControlMethod == ControlMethod.ItemCode)
            {
                //按备件编码管控

                var q = Query<StoreSummaryLocation>().Where(p => p.StoreSummaryId == storeSummary.Id);

                q.GroupBy(p => new
                {
                    WarehouseCode = p.Warehouse.Code,
                    WarehouseName = p.Warehouse.Name,
                    p.Warehouse.LibraryType,
                    StorageLocationCode = p.StorageLocation.Code,
                    StorageLocationName = p.StorageLocation.Name,
                });

                q.Select(p => new
                {
                    WarehouseCode = p.Warehouse.Code,
                    WarehouseName = p.Warehouse.Name,
                    LibraryType = p.Warehouse.LibraryType,
                    StorageLocationCode = p.StorageLocation.Code,
                    StorageLocationName = p.StorageLocation.Name,
                    RotNumber = p.RotNumber.SUM(),
                    GoodNumber = p.GoodNumber.SUM(),
                    SumNumber = p.SumNumber.SUM(),
                });

                list = q.ToList<StoreSummaryStockLot>();
            }
            else if (sparePart.ControlMethod == ControlMethod.Batch)
            {
                //按批次管控

                var q = Query<StoreSummaryLot>().Where(p => p.StoreSummaryId == storeSummary.Id);

                q.GroupBy(p => new
                {
                    WarehouseCode = p.Warehouse.Code,
                    WarehouseName = p.Warehouse.Name,
                    p.Warehouse.LibraryType,
                    StorageLocationCode = p.StorageLocation.Code,
                    StorageLocationName = p.StorageLocation.Name,
                    LotName = p.BatchNumber
                });

                q.Select(p => new
                {
                    WarehouseCode = p.Warehouse.Code,
                    WarehouseName = p.Warehouse.Name,
                    LibraryType = p.Warehouse.LibraryType,
                    StorageLocationCode = p.StorageLocation.Code,
                    StorageLocationName = p.StorageLocation.Name,
                    LotName = p.BatchNumber,
                    RotNumber = p.RotNumber.SUM(),
                    GoodNumber = p.GoodNumber.SUM(),
                    SumNumber = p.SumNumber.SUM()
                });

                list = q.ToList<StoreSummaryStockLot>();
            }
            else
            {
                //按序列号管控
                var q = Query<StoreSummaryDetail>().Where(p => p.StoreSummaryId == storeSummary.Id);

                q.GroupBy(p => new
                {
                    WarehouseCode = p.Warehouse.Code,
                    WarehouseName = p.Warehouse.Name,
                    p.Warehouse.LibraryType,
                    StorageLocationCode = p.StorageLocation.Code,
                    StorageLocationName = p.StorageLocation.Name,
                });

                q.Select(p => new
                {
                    WarehouseCode = p.Warehouse.Code,
                    WarehouseName = p.Warehouse.Name,
                    LibraryType = p.Warehouse.LibraryType,
                    StorageLocationCode = p.StorageLocation.Code,
                    StorageLocationName = p.StorageLocation.Name,
                    RotNumber = p.RotNumber.SUM(),
                    GoodNumber = p.GoodNumber.SUM(),
                    SumNumber = p.SumNumber.SUM()
                });

                list = q.ToList<StoreSummaryStockLot>();
            }

            if (list.Any())
            {
                var whList = list.Select(p => p.WarehouseCode).SplitContains(tempCodes =>
                {
                    return Query<Warehouse>().Where(p => tempCodes.Contains(p.Code)).ToList();
                });

                list.ForEach(data =>
                {
                    data.IsZeroCost = whList.First(p => p.Code == data.WarehouseCode).GetIsZeroCost();
                });
            }

            return list;
        }

        /// <summary>
        ///  获取物料编码明细
        /// </summary>
        ///  <param name="sortInfo">排序信息</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="storeSummary">库存汇总记录</param>
        /// <returns>物料编码明细列表</returns>
        public virtual IList<StoreSummaryLocation> GetStoreSummaryLocationList(IList<OrderInfo> sortInfo, PagingInfo pagingInfo, StoreSummary storeSummary)
        {
            IList<StoreSummaryLocation> list = Query<StoreSummaryLocation>().Where(p => p.StoreSummaryId == storeSummary.Id).OrderBy(sortInfo).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            var whList = list.Select(p => p.Warehouse.Code).SplitContains(tempCodes =>
            {
                return Query<Warehouse>().Where(p => tempCodes.Contains(p.Code)).ToList();
            });
            list.ForEach(data =>
            {
                data.IsZeroCost = whList.First(p => p.Code == data.Warehouse.Code).GetIsZeroCost();
            });
            return list;
        }

        /// <summary>
        ///  获取批次明细
        /// </summary>
        ///  <param name="sortInfo">排序信息</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="storeSummary">库存汇总记录</param>
        /// <returns>批次明细列表</returns>
        public virtual EntityList<StoreSummaryLot> GetStoreSummaryLotList(IList<OrderInfo> sortInfo, PagingInfo pagingInfo, StoreSummary storeSummary)
        {
            var list = Query<StoreSummaryLot>().Where(p => p.StoreSummaryId == storeSummary.Id).OrderBy(sortInfo).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            var whList = list.Select(p => p.Warehouse.Code).SplitContains(tempCodes =>
            {
                return Query<Warehouse>().Where(p => tempCodes.Contains(p.Code)).ToList();
            });
            list.ForEach(data =>
            {
                data.IsZeroCost = whList.First(p => p.Code == data.Warehouse.Code).GetIsZeroCost();
            });
            return list;
        }

        /// <summary>
        /// 获取备件基础信息（筛选出设备型号为空或满足设备型号条件的备件）
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <param name="key"></param>
        /// <param name="equipModelId"></param>
        /// <returns></returns>
        public virtual EntityList<SparePart> GetSparePartByEquipModelId(PagingInfo pagingInfo, string key, double? equipModelId)
        {
            var query = Query<SparePart>().Where(p => p.State == State.Enable);

            if (key.IsNotEmpty())
            {
                query.Where(p => p.SparePartCode.Contains(key) || p.SparePartName.Contains(key));
            }

            if (equipModelId.HasValue)
            {
                query.Where(p => p.SpartEquipModelId==equipModelId|| p.SpartEquipModelId == null);
            }

            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取现场退库的备件信息
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="entity">入库单头</param>
        /// <param name="keyword">查询关键字</param>
        /// <returns>备件列表</returns>
        public virtual EntityList<SparePart> GetSparePartBySparePartStore(PagingInfo pagingInfo, SparePartStore entity, string keyword)
        {
            EntityList<SparePart> list = null;

            if (entity.PartOutDepotDetailId == null || entity.PartOutDepotDetailId == 0)
            {
                list = Query<SparePart>().Where(p => p.State == State.Enable)
                    .WhereIf(keyword.IsNotEmpty(), p => p.SparePartCode.Contains(keyword) || p.SparePartName.Contains(keyword))
                    .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            }
            else
            {
                list = Query<SparePart>().Join<PartOutDepotDetail>((a, b) => a.Id == b.SparePartId)
                    .Where<PartOutDepotDetail>((a, b) => b.Id == entity.PartOutDepotDetailId)
                    .WhereIf(keyword.IsNotEmpty(), p => p.SparePartCode.Contains(keyword) || p.SparePartName.Contains(keyword)).Distinct()
                    .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            }

            foreach (var item in list)
            {
                item.IsImportItem = true;//仅用来标记是手动进行选择的备件
                if (item.ControlMethod == ControlMethod.Sn)
                {
                    item.LifeTime = 1;
                }
                else
                {
                    item.LifeTime = null;
                }

            }
            return list;
        }

        /// <summary>
        /// 获取入库的备件信息
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="entity">入库单头</param>
        /// <param name="keyword">查询关键字</param>
        /// <returns>备件列表</returns>
        public virtual EntityList<SparePart> GetSparePartByStoreDetails(PagingInfo pagingInfo, SparePartStore entity, string keyword)
        {
            var list = Query<SparePart>()
                .WhereIf(keyword.IsNotEmpty(), p => p.SparePartCode.Contains(keyword) || p.SparePartName.Contains(keyword))
                .Exists<StoreDetail>((x, y) => y.Where(p => p.SparePartId == x.Id && p.SparePartStoreId == entity.Id)).ToList(pagingInfo);

            var storeDetailList = Query<StoreDetail>().Where(p => p.SparePartStoreId == entity.Id).ToList();

            list.ForEach(p =>
            {
                if (p.ControlMethod == ControlMethod.ItemCode)
                {
                    var storeDetail = storeDetailList.First(detail => detail.SparePartId == p.Id);
                    p.LifeTime = storeDetail.Number;
                    p.UnitPrice = storeDetail.UnitPrice ?? p.UnitPrice;
                }
                p.IsImportItem = true;//用来标记前端是手动选择的备件
            });

            return list;
        }

        /// <summary>
        /// 获取现场退库所需的仓库信息
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="entity">入库单信息</param>
        /// <param name="keyword">查询关键字</param>
        /// <returns>仓库信息列表</returns>
        public virtual EntityList<Warehouse> GetWarehouseBySparePartStore(PagingInfo pagingInfo, SparePartStore entity, string keyword)
        {
            EntityList<Warehouse> list = new EntityList<Warehouse>();
            var whList = Query<Warehouse>().WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword)).ToList(pagingInfo);
            var isComputeAvgCost = IsComputeAvgCost();

            if (isComputeAvgCost)
            {
                foreach (var item in whList)
                {
                    if (entity.StorePartType == StorePartType.OldPart)
                    {
                        if (item.GetIsZeroCost())
                        {
                            list.Add(item);
                        }
                    }
                    else
                    {
                        if (!item.GetIsZeroCost())
                        {
                            list.Add(item);
                        }
                    }
                }
            }
            else
            {
                list.AddRange(whList);
            }
            return list;
        }

        /// <summary>
        /// 获取零成本仓
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">查询关键字</param>
        /// <returns>仓库信息列表</returns>
        public virtual EntityList<Warehouse> GetZereCostWarehouses(PagingInfo pagingInfo, string keyword)
        {
            var searchWhList = Query<Warehouse>().WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword)).ToList();

            var list = searchWhList.Where(p => p.GetIsZeroCost()).Select(p => p.Id).SplitContains(tempIds =>
            {
                return Query<Warehouse>().Where(p => tempIds.Contains(p.Id)).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            });

            return list;
        }

        /// <summary>
        ///  获取备件入库明细
        /// </summary>
        ///  <param name="sortInfo">排序信息</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="entity">入库单实体</param>
        /// <returns>备件入库明细列表</returns>
        public virtual EntityList<StoreDetail> GetSparePartStoreDetailList(IList<OrderInfo> sortInfo, PagingInfo pagingInfo, SparePartStore entity)
        {
            OrderInfo orderInfo = new OrderInfo();
            orderInfo.Property = "LineNo";
            orderInfo.SortOrder = System.ComponentModel.ListSortDirection.Ascending;
            orderInfo.SortIndex = 1;
            sortInfo.Add(orderInfo);

            var list = Query<StoreDetail>()
                .Where(p => p.SparePartStoreId == entity.Id)
                .WhereIf(entity.StoreDetailKeyWord.IsNotEmpty(), p => p.SparePart.SparePartCode.Contains(entity.StoreDetailKeyWord)
                                    || p.SparePart.SparePartName.Contains(entity.StoreDetailKeyWord)
                                    || p.SparePart.Specification.Contains(entity.StoreDetailKeyWord)
                                    || p.BatchNumber.Contains(entity.StoreDetailKeyWord)
                                    || p.Sn.Contains(entity.StoreDetailKeyWord))
                .OrderBy(sortInfo).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            return list;
        }

        /// <summary>
        ///  获取待入库的备件入库明细
        /// </summary>
        ///  <param name="sortInfo">排序信息</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="spareStoreId">入库单Id</param>
        /// <returns>备件入库明细列表</returns>
        public virtual EntityList<StoreDetail> GetSparePartStoreDetailList(IList<OrderInfo> sortInfo, PagingInfo pagingInfo, double spareStoreId)
        {
            OrderInfo orderInfo = new OrderInfo();
            orderInfo.Property = "LineNo";
            orderInfo.SortOrder = System.ComponentModel.ListSortDirection.Ascending;
            orderInfo.SortIndex = 1;
            sortInfo.Add(orderInfo);

            var list = Query<StoreDetail>()
                .Where(p => p.SparePartStoreId == spareStoreId && p.InboundStatus != InboundStatus.Done)
                .OrderBy(sortInfo).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            return list;
        }

        /// <summary>
        /// 获取备件入库明细
        /// </summary>
        /// <param name="idList">备件入库明细Id集合</param>
        /// <returns>备件入库明细记录集合</returns>
        public virtual EntityList<StoreDetail> GetSparePartStoreDetailListByIds(IList<double> idList)
        {
            return idList.SplitContains((ids) =>
            {
                return Query<StoreDetail>().Where(p => ids.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });

        }

        /// <summary>
        /// 备件条码入库查询
        /// </summary>
        /// <param name="barcode">备件条码</param>
        /// <param name="form">入库单头信息</param>
        /// <returns>入库条码扫描返回信息</returns>
        public virtual SparePartStoreQueryInfo SparePartStoreBarcodeQuery(string barcode, SparePartStore form)
        {
            SparePartStoreQueryInfo info = new SparePartStoreQueryInfo();
            info.SparePartStoreInfo = new SparePartStore();

            var storeSummary = StoreSummaryBarcodeQuery(barcode, form);

            if (storeSummary == null)
            {
                info.Success = false;
                info.Message = "所扫描内容有误，在所选仓库中既不是序列号、批次号，也不是备件编码，或不存在出库记录，请确认后重新扫描！".L10N();
            }
            else
            {
                if (form.SparePartId != null && storeSummary.ControlMethod != form.ControlMethod)
                {
                    if (form.ControlMethod == SpareParts.Enums.ControlMethod.Batch)
                    {
                        info.Success = false;
                        info.Message = "未在此仓库找到该批次信息，请确认后重新扫描批次号！".L10N();
                        return info;
                    }

                    if (form.ControlMethod == SpareParts.Enums.ControlMethod.Sn)
                    {
                        info.Success = false;
                        info.Message = "未在此仓库找到该序列号信息，请确认后重新扫描序列号！".L10N();
                        return info;
                    }
                }

                info.Success = true;
                info.SparePartStoreInfo.SparePartId = storeSummary.SparePartId;
                info.SparePartStoreInfo.SparePartCode = storeSummary.SparePartCode;
                info.SparePartStoreInfo.SparePartName = storeSummary.SparePartName;
                info.SparePartStoreInfo.ControlMethod = storeSummary.ControlMethod;
                info.SparePartStoreInfo.IsReplacement = storeSummary.IsReplacement;
                info.SparePartStoreInfo.IsCreateNewLabel = false;

                if (form.StorePartType == StorePartType.NewPart && (storeSummary.ControlMethod == ControlMethod.Batch && (form.PartOutDepotDetailId == null || form.PartOutDepotDetailId == 0)))
                {
                    info.SparePartStoreInfo.IsCreateNewLabel = true;
                }

                if (form.StorePartType == StorePartType.OldPart && storeSummary.ControlMethod == ControlMethod.Batch)
                {
                    info.SparePartStoreInfo.IsCreateNewLabel = true;
                }

                if (form.QualityStatus == 0)
                {
                    info.Message = "请维护本次入库备件的质量状态！".L10N();
                }
                else if (form.Number == 0)
                {
                    info.Message = "请维护本次入库备件的入库数量！".L10N();
                }
                else if (form.StorageLocationId == null || form.StorageLocationId == 0)
                {
                    info.Message = "请维护本次入库备件的库位！".L10N();
                }
                else
                {
                    info.Message = "请点击确定按钮生成入库明细！".L10N();
                }
            }
            return info;
        }

        /// <summary>
        /// 备件条码库存查询
        /// </summary>
        /// <param name="barcode">备件条码</param>
        /// <param name="form">入库单头信息</param>
        /// <returns>备件库存信息</returns>
        public virtual StoreSummary StoreSummaryBarcodeQuery(string barcode, SparePartStore form)
        {
            DateTime? nullValue = null;

            var query = Query<StoreSummary>()
                .LeftJoin<StoreSummaryLocation>((a, b) => a.Id == b.StoreSummaryId && b.WarehouseId == form.WarehouseId && b.StoreSummary.SparePart.SparePartCode == barcode)
                .LeftJoin<StoreSummaryLot>((a, c) => a.Id == c.StoreSummaryId && c.WarehouseId == form.WarehouseId && c.BatchNumber == barcode)
                .LeftJoin<StoreSummaryDetail>((a, d) => a.Id == d.StoreSummaryId && d.WarehouseId == form.WarehouseId && d.OrderNumberCode == barcode && (d.StoreStatus == OrdNumStoreStatus.Out || d.StoreStatus == OrdNumStoreStatus.Outsourced))
                .Where<StoreSummaryLocation, StoreSummaryLot, StoreSummaryDetail>((a, b, c, d) => b.CreateDate != nullValue || c.CreateDate != nullValue || d.CreateDate != nullValue);

            return query.FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty()
                .LoadWith(StoreSummary.StoreSummaryLocationListProperty)
                .LoadWith(StoreSummary.StoreSummaryDepotListProperty)
                .LoadWith(StoreSummary.StoreSummaryDetailListProperty));
        }

        /// <summary>
        /// 备件条码入库查询
        /// </summary>
        /// <param name="barcode">备件条码</param>
        /// <param name="form">入库单头信息</param>
        /// <returns>入库条码扫描返回信息</returns>
        public virtual SparePartStoreQueryInfo StoreDetailsBarcodeQuery(string barcode, SparePartStore form)
        {
            SparePartStoreQueryInfo info = new SparePartStoreQueryInfo();
            info.SparePartStoreInfo = new SparePartStore();
            info.Success = true;

            var detail = Query<StoreDetail>()
                .Where(p => p.SparePartStoreId == form.Id)
                .Where(p => p.QualityStatus == form.QualityStatus)
                .Where(p => (p.SparePart.SparePartCode == barcode && p.SparePart.ControlMethod == Enums.ControlMethod.ItemCode) || p.BatchNumber == barcode || p.Sn == barcode)
                .FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());

            if (detail == null)
            {
                info.Success = false;
                info.Message = "所扫描内容有误，在入库明细中找不到对应质量状态的【序列号】/【批次号】/【备件编码】，请确认后重新扫描！".L10N();
            }
            else
            {
                info.SparePartStoreInfo.SparePartId = detail.SparePartId;
                info.SparePartStoreInfo.SparePartCode = detail.SparePartCode;
                info.SparePartStoreInfo.SparePartName = detail.SparePartName;
                info.SparePartStoreInfo.ControlMethod = detail.ControlMethod;
                info.SparePartStoreInfo.IsReplacement = detail.IsReplacement;
                info.SparePartStoreInfo.Number = detail.Number;
                info.SparePartStoreInfo.UnitPrice = detail.UnitPrice ?? detail.SparePartUnitPrice;
                info.Message = "入库成功，请继续扫描【序列号】/【批次号】/【备件编码】！".L10N();
            }

            return info;
        }

        /// <summary>
        /// 生成库存记录
        /// </summary>
        /// <param name="storeBill">入库单信息</param>
        /// <param name="item">入库明细行</param>
        /// <returns></returns>
        public virtual void CreateStoreSummary(SparePartStore storeBill, StoreDetail item)
        {
            var nowDate = RF.Find<SparePartStore>().GetDbTime();
            var isComputeAvgCost = IsComputeAvgCost();
            var isZeroCostWh = storeBill.Warehouse.GetIsZeroCost();

            var storeSummaryList = storeBill.StoreDetailList.Select(p => p.SparePartId).Distinct().SplitContains(tempIds =>
            {
                return Query<StoreSummary>().Where(p => tempIds.Contains(p.SparePartId))
                .ToList(null, new EagerLoadOptions().LoadWith(StoreSummary.StoreSummaryLocationListProperty)
                .LoadWith(StoreSummary.StoreSummaryDepotListProperty).LoadWith(StoreSummary.StoreSummaryDetailListProperty));
            });

            var storeSummary = storeSummaryList.FirstOrDefault(p => p.SparePartId == item.SparePartId) ?? new StoreSummary();

            if (isComputeAvgCost && !isZeroCostWh)
            {
                if (item.UnitPrice == null || item.UnitPrice == 0)
                {
                    throw new ValidationException("入库仓库为非零成本仓且启用了计算备件平均成本的配置，请输入备件单价！".L10N());
                }

                storeSummary.AverageCost = (storeSummary.SumNumber + item.Number) == 0 ? (decimal)item.UnitPrice : (storeSummary.AverageCost * storeSummary.SumNumber + (decimal)item.UnitPrice * item.Number) / (storeSummary.SumNumber + item.Number);
            }

            storeSummary.SparePartId = item.SparePartId;
            storeSummary.GoodNumber += item.QualityStatus == QualityStatus.Good ? item.Number : 0;
            storeSummary.RotNumber += item.QualityStatus == QualityStatus.Defective ? item.Number : 0;
            storeSummary.SumNumber += item.Number;

            if (item.SparePart.ControlMethod == ControlMethod.ItemCode)
            {
                var location = storeSummary.StoreSummaryLocationList.FirstOrDefault(p => p.StorageLocationId == item.StorageLocationId && p.WarehouseId == storeBill.WarehouseId);

                if (location == null)
                {
                    location = new StoreSummaryLocation();
                    storeSummary.StoreSummaryLocationList.Add(location);
                }

                location.WarehouseId = storeBill.WarehouseId;
                location.StorageLocationId = (double)item.StorageLocationId;
                location.GoodNumber += item.QualityStatus == QualityStatus.Good ? item.Number : 0;
                location.RotNumber += item.QualityStatus == QualityStatus.Defective ? item.Number : 0;
                location.SumNumber += item.Number;
            }
            else if (item.SparePart.ControlMethod == ControlMethod.Batch)
            {
                var lot = storeSummary.StoreSummaryDepotList.FirstOrDefault(p => p.StorageLocationId == item.StorageLocationId && p.WarehouseId == storeBill.WarehouseId && p.BatchNumber == item.BatchNumber);

                if (lot == null)
                {
                    lot = new StoreSummaryLot();
                    lot.InboundDate = nowDate;
                    storeSummary.StoreSummaryDepotList.Add(lot);
                }

                lot.WarehouseId = storeBill.WarehouseId;
                lot.StorageLocationId = (double)item.StorageLocationId;
                lot.BatchNumber = item.BatchNumber;
                lot.GoodNumber += item.QualityStatus == QualityStatus.Good ? item.Number : 0;
                lot.RotNumber += item.QualityStatus == QualityStatus.Defective ? item.Number : 0;
                lot.SumNumber += item.Number;
            }
            else
            {
                var detail = storeSummary.StoreSummaryDetailList.FirstOrDefault(p => p.OrderNumberCode == item.Sn);

                if (detail == null)
                {
                    detail = new StoreSummaryDetail();
                    detail.InboundDate = nowDate;
                    storeSummary.StoreSummaryDetailList.Add(detail);
                }

                detail.WarehouseId = storeBill.WarehouseId;
                detail.StorageLocationId = (double)item.StorageLocationId;
                detail.OrderNumberCode = item.Sn;
                detail.OdNbStatus = item.QualityStatus == QualityStatus.Good ? OdNbStatus.GoodProduct : OdNbStatus.NoGoodProduct;
                detail.StoreStatus = OrdNumStoreStatus.In;
                detail.StoreCode = storeBill.StoreCode;
                detail.SupplierId = storeBill.SupplierId;
                detail.GoodNumber += item.QualityStatus == QualityStatus.Good ? item.Number : 0;
                detail.RotNumber += item.QualityStatus == QualityStatus.Defective ? item.Number : 0;
                detail.SumNumber += item.Number;
            }
            RF.Save(storeSummary);
        }

        /// <summary>
        /// 整单入库操作
        /// </summary>
        /// <param name="storeBill">入库单信息</param>
        /// <returns></returns>
        public virtual void WholeBillInStorage(SparePartStore storeBill)
        {
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                var nowDate = RF.Find<SparePartStore>().GetDbTime();
                storeBill.InboundStatus = InboundStatus.Done;
                storeBill.StoreDateTime = nowDate;
                storeBill.WarehouseOperatorId = RT.IdentityId;

                //备件入库接收单更新信息
                var receiveInfo = new SparePartReceiveInfo();
                receiveInfo.ReceiveNo = storeBill.ReceiveNo;
                receiveInfo.AcceptanceNo = storeBill.AcceptanceNo;
                receiveInfo.SparePartReceiveDtlInfoList = new List<SparePartReceiveDtlInfo>();

                //若库位为空，则获取仓库的默认虚拟库位
                var locationList = Query<StorageLocation>().Where(p => p.WarehouseId == storeBill.WarehouseId && p.LibraryType == LibraryType.Fictitious).ToList();

                double? locationId = null;

                if (locationList.Any())
                {
                    var location = locationList.FirstOrDefault(p => p.Code == "STAGE");

                    if (location != null)
                    {
                        locationId = location.Id;
                    }
                    else
                    {
                        locationId = locationList.First().Id;
                    }
                }

                //筛选出需要入库的入库明细
                var storeDetailList = storeBill.StoreDetailList.Where(p => p.InboundStatus != InboundStatus.Done);

                if (!storeDetailList.Any())
                {
                    throw new ValidationException("不存在待入库的入库明细！".L10N());
                }

                foreach (var item in storeDetailList)
                {
                    if (item.StorageLocationId == null)
                    {
                        if (locationId != null)
                        {
                            item.StorageLocationId = (double)locationId;
                        }
                        else
                        {
                            throw new ValidationException("该入库单的入库仓库不存在虚拟库位，入库库位为空的备件无法入库！".L10N());
                        }
                    }
                    item.InboundStatus = InboundStatus.Done;

                    //生成入库记录
                    CreateStoreSummary(storeBill, item);

                    //回写出库明细退回数量
                    if (item.PartOutDepotDetailId != null && item.PartOutDepotDetailId != 0)
                    {

                        if (item.IsOldPart)
                        {
                            DB.Update<PartOutDepotDetail>()
                                .Set(p => p.OldReturnQty, p => p.OldReturnQty + item.Number)
                                .Where(p => p.Id == item.PartOutDepotDetailId)
                                .Execute();
                        }
                        else
                        {
                            DB.Update<PartOutDepotDetail>()
                                .Set(p => p.ReturnQty, p => p.ReturnQty + item.Number)
                                .Where(p => p.Id == item.PartOutDepotDetailId)
                                .Execute();
                        }

                    }

                    if (storeBill.InboundType == SparePartInboundType.Po)
                    {
                        var receiveDtlInfo = new SparePartReceiveDtlInfo();
                        receiveDtlInfo.SparePartId = item.SparePartId;
                        receiveDtlInfo.InboundQty = item.Number;
                        receiveDtlInfo.PurchaseOrderNo = item.PurchaseOrderNo;
                        receiveDtlInfo.PurchaseOrderLineNo = item.PurchaseOrderLineNo;
                        receiveInfo.SparePartReceiveDtlInfoList.Add(receiveDtlInfo);
                    }
                }

                //回写采购明细入库数量
                if (storeBill.InboundType == SparePartInboundType.Po)
                {
                    RT.Service.Resolve<ISparePartReceives>().UpdatePurchaseDtlInboundQty(receiveInfo);
                }

                RF.Save(storeBill);

                trans.Complete();
            }
        }

        /// <summary>
        /// 部分入库操作
        /// </summary>
        /// <param name="storeBill">入库单信息</param>
        /// <returns></returns>
        public virtual void PartBillInStorage(SparePartStore storeBill)
        {
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                //备件入库接收单更新信息
                var receiveInfo = new SparePartReceiveInfo();
                receiveInfo.ReceiveNo = storeBill.ReceiveNo;
                receiveInfo.AcceptanceNo = storeBill.AcceptanceNo;
                receiveInfo.SparePartReceiveDtlInfoList = new List<SparePartReceiveDtlInfo>();
                var nowDate = RF.Find<SparePartStore>().GetDbTime();

                //筛选出需要入库的入库明细
                var storeDetailList = storeBill.StoreDetailList.Where(p => p.StorageLocationId != null && p.StorageLocationId != 0 && p.InboundStatus != InboundStatus.Done).ToList();

                if (!storeDetailList.Any())
                {
                    throw new ValidationException("入库明细的入库库位不能为空！".L10N());
                }

                foreach (var item in storeDetailList)
                {
                    item.InboundStatus = InboundStatus.Done;

                    //生成入库记录
                    CreateStoreSummary(storeBill, item);

                    //回写出库明细退回数量
                    if (item.PartOutDepotDetailId != null && item.PartOutDepotDetailId != 0)
                    {
                        if (item.IsOldPart)
                        {
                            DB.Update<PartOutDepotDetail>()
                                .Set(p => p.OldReturnQty, p => p.OldReturnQty + item.Number)
                                .Where(p => p.Id == item.PartOutDepotDetailId)
                                .Execute();
                        }
                        else
                        {
                            DB.Update<PartOutDepotDetail>()
                                .Set(p => p.ReturnQty, p => p.ReturnQty + item.Number)
                                .Where(p => p.Id == item.PartOutDepotDetailId)
                                .Execute();
                        }
                    }

                    if (storeBill.InboundType == SparePartInboundType.Po)
                    {
                        var receiveDtlInfo = new SparePartReceiveDtlInfo();
                        receiveDtlInfo.SparePartId = item.SparePartId;
                        receiveDtlInfo.InboundQty = item.Number;
                        receiveDtlInfo.PurchaseOrderNo = item.PurchaseOrderNo;
                        receiveDtlInfo.PurchaseOrderLineNo = item.PurchaseOrderLineNo;
                        receiveInfo.SparePartReceiveDtlInfoList.Add(receiveDtlInfo);
                    }
                }

                //回写采购明细入库数量
                if (storeBill.InboundType == SparePartInboundType.Po)
                {
                    RT.Service.Resolve<ISparePartReceives>().UpdatePurchaseDtlInboundQty(receiveInfo);
                }

                storeBill.InboundStatus = storeBill.StoreDetailList.Count() == storeDetailList.Count ? InboundStatus.Done : InboundStatus.Doing;
                storeBill.StoreDateTime = nowDate;
                storeBill.WarehouseOperatorId = RT.IdentityId;

                RF.Save(storeBill);

                trans.Complete();
            }
        }

        /// <summary>
        /// 盘点获取备件序列号明细
        /// </summary>        
        /// <param name="storageLocationIds">库位ID列表</param>
        /// <param name="sparePartId">备件ID</param>        
        /// <param name="spartType">备件类型</param>        
        /// <param name="isFixAsset">是否固定资产</param>
        /// <param name="assetsCategory">固定资产类别</param>
        /// <param name="assetOwnerId">固定资产所有人</param>
        /// <returns></returns>
        public virtual int GetStoreSummaryDetailCountForInventory(List<double> storageLocationIds, double? sparePartId,
            SparePartType? spartType, YesNo? isFixAsset, string assetsCategory, double? assetOwnerId)
        {
            int count = 0;
            storageLocationIds.SplitDataExecute(tempIds =>
          {
              var q = Query<StoreSummaryDetail>()
                  .Where(x => x.StoreStatus == OrdNumStoreStatus.In)
                  .Where(x => tempIds.Contains(x.StorageLocationId));

              if (sparePartId.HasValue)
              {
                  q.Where(x => x.StoreSummary.SparePartId == sparePartId.Value);
              }
              else
              {
                  if (spartType.HasValue)
                  {
                      q.Where(x => x.StoreSummary.SparePart.SpartType == spartType.Value);
                  }
              }

              if (isFixAsset.HasValue)
              {
                  if (isFixAsset == YesNo.Yes)
                  {
                      q.Where(x => x.FixedAssetsAccountId != null);

                      if (!assetsCategory.IsNullOrEmpty())
                      {
                          q.Where(x => x.FixedAssetsAccount.AssetsCategory == assetsCategory);
                      }

                      if (assetOwnerId.HasValue)
                      {
                          q.Where(x => x.FixedAssetsAccount.AssetOwnerId == assetOwnerId.Value);
                      }
                  }
                  else
                  {
                      q.Where(x => x.FixedAssetsAccountId == null);
                  }
              }

              count += q.Count();
          });

            return count;
        }

        /// <summary>
        /// 盘点获取备件序列号明细
        /// </summary>        
        /// <param name="storageLocationIds">库位ID列表</param>        
        /// <param name="spartType">备件类型</param>
        /// <param name="sparePartIds">备件ID列表</param>
        /// <param name="isFixAsset">是否固定资产</param>
        /// <param name="assetsCategory">固定资产类别</param>
        /// <param name="assetOwnerId">固定资产所有人</param>
        /// <returns></returns>
        public virtual int GetStoreSummaryDetailCountForInventory(List<double> storageLocationIds,
            SparePartType? spartType, List<double> sparePartIds, YesNo? isFixAsset,
            string assetsCategory, double? assetOwnerId)
        {
            int count = 0;
            sparePartIds.SplitDataExecute(tempIds =>
            {
                var q = Query<StoreSummaryDetail>().Where(x => x.StoreStatus == OrdNumStoreStatus.In);

                if (storageLocationIds.Any())
                {
                    var exp = storageLocationIds.CreateContainsExpression<StoreSummaryDetail>("x",
                        StoreSummaryDetail.StorageLocationIdProperty.Name);
                    q.Where(exp);
                }

                q.Where(x => tempIds.Contains(x.StoreSummary.SparePartId));

                if (spartType.HasValue)
                {
                    q.Where(x => x.StoreSummary.SparePart.SpartType == spartType.Value);
                }

                if (isFixAsset.HasValue)
                {
                    if (isFixAsset == YesNo.Yes)
                    {
                        q.Where(x => x.FixedAssetsAccountId != null);

                        if (!assetsCategory.IsNullOrEmpty())
                        {
                            q.Where(x => x.FixedAssetsAccount.AssetsCategory == assetsCategory);
                        }

                        if (assetOwnerId.HasValue)
                        {
                            q.Where(x => x.FixedAssetsAccount.AssetOwnerId == assetOwnerId.Value);
                        }
                    }
                    else
                    {
                        q.Where(x => x.FixedAssetsAccountId == null);
                    }
                }

                count += q.Count();
            });

            return count;
        }


        /// <summary>
        /// 盘点获取备件序列号明细
        /// </summary>        
        /// <param name="storageLocationIds">库位ID列表</param>
        /// <param name="sparePartId">备件ID</param>        
        /// <param name="spartType">备件类型</param>        
        /// <param name="isFixAsset">是否固定资产</param>
        /// <param name="assetsCategory">固定资产类别</param>
        /// <param name="assetOwnerId">固定资产所有人</param>
        /// <returns></returns>
        public virtual EntityList<StoreSummaryDetail> GetStoreSummaryDetailsForInventory(List<double> storageLocationIds, double? sparePartId,
            SparePartType? spartType, YesNo? isFixAsset, string assetsCategory, double? assetOwnerId)
        {
            return storageLocationIds.SplitContains(tempIds =>
              {
                  var q = Query<StoreSummaryDetail>()
                      .Where(x => x.StoreStatus == OrdNumStoreStatus.In)
                      .Where(x => tempIds.Contains(x.StorageLocationId));

                  if (sparePartId.HasValue)
                  {
                      q.Where(x => x.StoreSummary.SparePartId == sparePartId.Value);
                  }
                  else
                  {
                      if (spartType.HasValue)
                      {
                          q.Where(x => x.StoreSummary.SparePart.SpartType == spartType.Value);
                      }
                  }

                  if (isFixAsset.HasValue)
                  {
                      if (isFixAsset == YesNo.Yes)
                      {
                          q.Where(x => x.FixedAssetsAccountId != null);

                          if (!assetsCategory.IsNullOrEmpty())
                          {
                              q.Where(x => x.FixedAssetsAccount.AssetsCategory == assetsCategory);
                          }

                          if (assetOwnerId.HasValue)
                          {
                              q.Where(x => x.FixedAssetsAccount.AssetOwnerId == assetOwnerId.Value);
                          }
                      }
                      else
                      {
                          q.Where(x => x.FixedAssetsAccountId == null);
                      }
                  }

                  return q.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
              });
        }

        /// <summary>
        /// 盘点获取备件序列号明细
        /// </summary>        
        /// <param name="storageLocationIds">库位ID列表</param>        
        /// <param name="spartType">备件类型</param>
        /// <param name="sparePartIds">备件ID列表</param>
        /// <param name="isFixAsset">是否固定资产</param>
        /// <param name="assetsCategory">固定资产类别</param>
        /// <param name="assetOwnerId">固定资产所有人</param>
        /// <returns></returns>
        public virtual EntityList<StoreSummaryDetail> GetStoreSummaryDetailsForInventory(List<double> storageLocationIds,
            SparePartType? spartType, List<double> sparePartIds, YesNo? isFixAsset,
            string assetsCategory, double? assetOwnerId)
        {
            return sparePartIds.SplitContains(tempIds =>
            {
                var q = Query<StoreSummaryDetail>().Where(x => x.StoreStatus == OrdNumStoreStatus.In);

                if (storageLocationIds.Any())
                {
                    var exp = storageLocationIds.CreateContainsExpression<StoreSummaryDetail>("x",
                        StoreSummaryDetail.StorageLocationIdProperty.Name);
                    q.Where(exp);
                }

                q.Where(x => tempIds.Contains(x.StoreSummary.SparePartId));

                if (spartType.HasValue)
                {
                    q.Where(x => x.StoreSummary.SparePart.SpartType == spartType.Value);
                }

                if (isFixAsset.HasValue)
                {
                    if (isFixAsset == YesNo.Yes)
                    {
                        q.Where(x => x.FixedAssetsAccountId != null);

                        if (!assetsCategory.IsNullOrEmpty())
                        {
                            q.Where(x => x.FixedAssetsAccount.AssetsCategory == assetsCategory);
                        }

                        if (assetOwnerId.HasValue)
                        {
                            q.Where(x => x.FixedAssetsAccount.AssetOwnerId == assetOwnerId.Value);
                        }
                    }
                    else
                    {
                        q.Where(x => x.FixedAssetsAccountId == null);
                    }
                }

                return q.ToList(null,new EagerLoadOptions().LoadWith(StoreSummaryDetail.StoreSummaryProperty));
            });

        }


        /// <summary>
        /// 盘点获取备件序列号明细
        /// </summary>        
        /// <param name="storageLocationIds">库位ID列表</param>
        /// <param name="sparePartId">备件ID</param>        
        /// <param name="spartType">备件类型</param>
        /// <returns></returns>
        public virtual int GetStoreSummaryLotCountForInventory(List<double> storageLocationIds, double? sparePartId,
            SparePartType? spartType)
        {
            int count = 0;
            storageLocationIds.SplitDataExecute(tempIds =>
          {
              var q = Query<StoreSummaryLot>()
                  .Where(x => x.SumNumber > 0)
                  .Where(x => tempIds.Contains(x.StorageLocationId));

              if (sparePartId.HasValue)
              {
                  q.Where(x => x.StoreSummary.SparePartId == sparePartId.Value);
              }
              else
              {
                  if (spartType.HasValue)
                  {
                      q.Where(x => x.StoreSummary.SparePart.SpartType == spartType.Value);
                  }
              }

              count += q.Count();
          });

            return count;
        }

        /// <summary>
        /// 盘点获取备件批次明细
        /// </summary>        
        /// <param name="storageLocationIds">库位ID列表</param>        
        /// <param name="spartType">备件类型</param>
        /// <param name="sparePartIds">备件ID列表</param>        
        /// <returns></returns>
        public virtual int GetStoreSummaryLotCountForInventory(List<double> storageLocationIds,
            SparePartType? spartType, List<double> sparePartIds)
        {
            int count = 0;

            sparePartIds.SplitDataExecute(tempIds =>
            {
                var q = Query<StoreSummaryLot>().Where(x => x.SumNumber > 0);

                if (storageLocationIds.Any())
                {
                    var exp = storageLocationIds.CreateContainsExpression<StoreSummaryLot>("x",
                        StoreSummaryLot.StorageLocationIdProperty.Name);
                    q.Where(exp);
                }

                q.Where(x => tempIds.Contains(x.StoreSummary.SparePartId));

                if (spartType.HasValue)
                {
                    q.Where(x => x.StoreSummary.SparePart.SpartType == spartType.Value);
                }

                count += q.Count();
            });

            return count;
        }

        /// <summary>
        /// 盘点获取备件批次明细
        /// </summary>        
        /// <param name="storageLocationIds">库位ID列表</param>        
        /// <param name="spartType">备件类型</param>
        /// <param name="sparePartIds">备件ID列表</param>        
        /// <returns></returns>
        public virtual EntityList<StoreSummaryLot> GetStoreSummaryLotsForInventory(List<double> storageLocationIds,
            SparePartType? spartType, List<double> sparePartIds)
        {
            return sparePartIds.SplitContains(tempIds =>
            {
                var q = Query<StoreSummaryLot>().Where(x => x.SumNumber > 0);

                if (storageLocationIds.Any())
                {
                    var exp = storageLocationIds.CreateContainsExpression<StoreSummaryLot>("x",
                        StoreSummaryLot.StorageLocationIdProperty.Name);
                    q.Where(exp);
                }

                q.Where(x => tempIds.Contains(x.StoreSummary.SparePartId));

                if (spartType.HasValue)
                {
                    q.Where(x => x.StoreSummary.SparePart.SpartType == spartType.Value);
                }

                return q.ToList(null, new EagerLoadOptions().LoadWith(StoreSummaryLot.StoreSummaryProperty));
            });
        }

        /// <summary>
        /// 盘点获取备件序列号明细
        /// </summary>        
        /// <param name="storageLocationIds">库位ID列表</param>
        /// <param name="sparePartId">备件ID</param>        
        /// <param name="spartType">备件类型</param>
        /// <returns></returns>
        public virtual EntityList<StoreSummaryLot> GetStoreSummaryLotsForInventory(List<double> storageLocationIds, double? sparePartId,
            SparePartType? spartType)
        {
            return storageLocationIds.SplitContains(tempIds =>
            {
                var q = Query<StoreSummaryLot>()
                    .Where(x => x.SumNumber > 0)
                    .Where(x => tempIds.Contains(x.StorageLocationId));

                if (sparePartId.HasValue)
                {
                    q.Where(x => x.StoreSummary.SparePartId == sparePartId.Value);
                }
                else
                {
                    if (spartType.HasValue)
                    {
                        q.Where(x => x.StoreSummary.SparePart.SpartType == spartType.Value);
                    }
                }

                return q.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 盘点获取备件编码明细
        /// </summary>        
        /// <param name="storageLocationIds">库位ID列表</param>        
        /// <param name="spartType">备件类型</param>
        /// <param name="sparePartIds">备件ID列表</param>        
        /// <returns></returns>
        public virtual int GetStoreSummaryLocationCountForInventory(List<double> storageLocationIds,
            SparePartType? spartType, List<double> sparePartIds)
        {
            int count = 0;
            sparePartIds.SplitDataExecute(tempIds =>
            {
                var q = Query<StoreSummaryLocation>().Where(x => x.SumNumber > 0);

                if (storageLocationIds.Any())
                {
                    var exp = storageLocationIds.CreateContainsExpression<StoreSummaryLocation>("x",
                        StoreSummaryLocation.StorageLocationIdProperty.Name);
                    q.Where(exp);
                }

                q.Where(x => tempIds.Contains(x.StoreSummary.SparePartId));

                if (spartType.HasValue)
                {
                    q.Where(x => x.StoreSummary.SparePart.SpartType == spartType.Value);
                }

                count += q.Count();
            });

            return count;
        }

        /// <summary>
        /// 盘点获取备件编码明细
        /// </summary>        
        /// <param name="storageLocationIds">库位ID列表</param>
        /// <param name="sparePartId">备件ID</param>        
        /// <param name="spartType">备件类型</param>
        /// <returns></returns>
        public virtual int GetStoreSummaryLocationCountForInventory(List<double> storageLocationIds, double? sparePartId,
            SparePartType? spartType)
        {
            int count = 0;
            storageLocationIds.SplitDataExecute(tempIds =>
              {
                  var q = Query<StoreSummaryLocation>()
                      .Where(x => x.SumNumber > 0)
                      .Where(x => tempIds.Contains(x.StorageLocationId));

                  if (sparePartId.HasValue)
                  {
                      q.Where(x => x.StoreSummary.SparePartId == sparePartId.Value);
                  }
                  else
                  {
                      if (spartType.HasValue)
                      {
                          q.Where(x => x.StoreSummary.SparePart.SpartType == spartType.Value);
                      }
                  }

                  count += q.Count();
              });

            return count;
        }

        /// <summary>
        /// 盘点获取备件编码明细
        /// </summary>        
        /// <param name="storageLocationIds">库位ID列表</param>        
        /// <param name="spartType">备件类型</param>
        /// <param name="sparePartIds">备件ID列表</param>        
        /// <returns></returns>
        public virtual EntityList<StoreSummaryLocation> GetStoreSummaryLocationsForInventory(List<double> storageLocationIds,
            SparePartType? spartType, List<double> sparePartIds)
        {
            return sparePartIds.SplitContains(tempIds =>
            {
                var q = Query<StoreSummaryLocation>().Where(x => x.SumNumber > 0);

                if (storageLocationIds.Any())
                {
                    var exp = storageLocationIds.CreateContainsExpression<StoreSummaryLocation>("x",
                        StoreSummaryLocation.StorageLocationIdProperty.Name);
                    q.Where(exp);
                }

                q.Where(x => tempIds.Contains(x.StoreSummary.SparePartId));

                if (spartType.HasValue)
                {
                    q.Where(x => x.StoreSummary.SparePart.SpartType == spartType.Value);
                }

                return q.ToList(null,new EagerLoadOptions().LoadWith(StoreSummaryLocation.StoreSummaryProperty));
            });
        }

        /// <summary>
        /// 盘点获取备件编码明细
        /// </summary>        
        /// <param name="storageLocationIds">库位ID列表</param>
        /// <param name="sparePartId">备件ID</param>        
        /// <param name="spartType">备件类型</param>
        /// <returns></returns>
        public virtual EntityList<StoreSummaryLocation> GetStoreSummaryLocationsForInventory(List<double> storageLocationIds, double? sparePartId,
            SparePartType? spartType)
        {
            return storageLocationIds.SplitContains(tempIds =>
            {
                var q = Query<StoreSummaryLocation>()
                    .Where(x => x.SumNumber > 0)
                    .Where(x => tempIds.Contains(x.StorageLocationId));

                if (sparePartId.HasValue)
                {
                    q.Where(x => x.StoreSummary.SparePartId == sparePartId.Value);
                }
                else
                {
                    if (spartType.HasValue)
                    {
                        q.Where(x => x.StoreSummary.SparePart.SpartType == spartType.Value);
                    }
                }

                return q.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 获取批次
        /// </summary>
        /// <param name="lotNos">批次号列表</param>
        /// <returns></returns>
        public virtual EntityList<StoreSummaryLot> GetStoreSummaryLotByLotNos(List<string> lotNos)
        {
            return lotNos.SplitContains(tempLotNos =>
            {
                return Query<StoreSummaryLot>().Where(x => tempLotNos.Contains(x.BatchNumber)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 获取序列
        /// </summary>
        /// <param name="snList">序列号列表</param>
        /// <returns></returns>
        public virtual EntityList<StoreSummaryDetail> GetStoreSummaryDetailBySns(List<string> snList)
        {
            return snList.SplitContains(tempSnList =>
            {
                return Query<StoreSummaryDetail>().Where(x => tempSnList.Contains(x.OrderNumberCode))
                    .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 获取批次编码
        /// </summary>
        /// <returns>批次编码</returns>
        public virtual string GetLotNumber()
        {
            var config = ConfigService.GetConfig(new BatchNumberNoConfig(), typeof(StoreSummary));
            if (config == null || config.BatchNumberRuleId == null)
            {
                throw new ValidationException("请维护批次编码规则".L10N());
            }
            return RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.BatchNumberRuleId.Value, 1).FirstOrDefault();
        }

        /// <summary>
        /// 获取序列号编码
        /// </summary>
        /// <returns>序列号编码</returns>
        public virtual string GetSnNumber()
        {
            var config = ConfigService.GetConfig(new BatchNumberNoConfig(), typeof(StoreSummary));
            if (config == null || config.SnNumberRuleId == null)
            {
                throw new ValidationException("请维护序列号编码规则".L10N());
            }
            return RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.SnNumberRuleId.Value, 1).FirstOrDefault();
        }
    }
}
