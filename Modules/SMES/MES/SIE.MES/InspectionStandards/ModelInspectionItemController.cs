using SIE.Common.Import;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.InspectionStandards
{
    /// <summary>
    /// 机型检查项目控制器
    /// </summary>
    public class ModelInspectionItemController : DomainController
    {
        /// <summary>
        /// 根据ID查找机型检验项
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>返回机型检验项目</returns>
        public virtual ModelInspectionItem GetModelInspectionItemById(double id)
        {
            return RF.GetById<ModelInspectionItem>(id);
        }

        /// <summary>
        /// 根据ID集合查找机型检验项
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual EntityList<ModelInspectionItem> GetModelInspectionItemByIds(List<double> ids)
        {
            return ids.SplitContains(tempIds => Query<ModelInspectionItem>().Where(m => tempIds.Contains(m.Id)).ToList());
        }



        /// <summary>
        /// 导入设备机型
        /// </summary>
        /// <param name="batch"></param>
        /// <returns></returns>
        public virtual IEnumerable<ImportMessageResult> ImportInspection(IList<RowData> batch)
        {
            List<ImportMessageResult> messageList = new List<ImportMessageResult>();
            var modelInspectionItems = batch.Select(p => p.Entity as ModelInspectionItem).ToList();
            var saveLists = new EntityList<ModelInspectionItem>();
            foreach (var row in modelInspectionItems)
            {
                saveLists.Add(row);
            }

            using (var trans = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                try
                {
                    RF.Save(saveLists);
                    trans.Complete();
                }
                catch (Exception ex)
                {
                    messageList.Add(new ImportMessageResult() { Message = ex.Message, MsgType = ImportMessageType.SaveFail });
                    return messageList;

                }
                messageList.Add(new ImportMessageResult() { Message = "导入成功", MsgType = ImportMessageType.SaveSucess });

            }
            return messageList;
        }

        /// <summary>
        /// 根据机型Id，工序Id，检验方式获取相应的机型检验项目
        /// </summary>
        /// <param name="modelId">机型ID</param>
        /// <param name="processId">工序ID</param>
        /// <returns>返回相应的机型检验项目</returns>
        public virtual EntityList<ModelInspectionItem> GetModelInspectionItem(double modelId, double processId)
        {
            return Query<ModelInspectionItem>().Where(p => p.ModelId == modelId && p.ProcessId == processId && p.EffectiveStartTime <= DateTime.Now && (p.EffectiveEndTime > DateTime.Now || p.EffectiveEndTime == null)).ToList();
        }

        /// <summary>
        /// 获取成品、半成品物料
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<Item> GetTargetItems(PagingInfo pagingInfo, string keyword)
        {
            var query = Query<Item>()
                .WhereIf(!keyword.IsNullOrEmpty(), p => p.Name.Contains(keyword) || p.Code.Contains(keyword))
                .Where(p => p.Type == ItemType.Product || p.Type == ItemType.SemiFinished)
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            if (query != null)
            {
                return query;
            }
            else
            {
                return new EntityList<Item>();
            }
        }

        /// <summary>
        /// 获取机型(如果机型为空则返回物料的产品机型或0)
        /// </summary>
        /// <param name="inspectionItemId"></param>
        /// <returns></returns>
        public virtual double GetProductModel(double inspectionItemId)
        {
            var modelInspectionItem = RF.GetById<ModelInspectionItem>(inspectionItemId);
            var hasModel = modelInspectionItem.ModelId != 0 && modelInspectionItem.ModelId != null;
            var item = RF.GetById<Item>(modelInspectionItem.ProductItemId);
            var hasProductModel = item.ModelId != 0 && item.ModelId != null;
            if (hasModel)
            {
                return modelInspectionItem.ModelId.Value;
            }
            else if (hasProductModel)
            {
                return item.ModelId.Value;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 根据机型或产品获取当前最大排序值+1
        /// </summary>
        /// <param name="inspectionId"></param>
        /// <param name="modelId"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public virtual int? GetMaxOrderNum(double inspectionId, double? modelId, double? itemId)
        {
            var hasModel = modelId != 0 && modelId != null;
            var hasItem = itemId != 0 && itemId != null;
            //维护的是机型
            if (hasModel && !hasItem)
            {
                var query = Query<ModelInspectionItem>()
                    .Where(p => p.Id != inspectionId)
                    .Where(p => p.ModelId == modelId)
                    .OrderByDescending(p => p.OrderNum)
                    .FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
                if (query != null)
                {
                    return query.OrderNum ?? 0;
                }
                else
                {
                    return 0;
                }
            }
            else if (!hasModel && hasItem)
            {
                var query = Query<ModelInspectionItem>()
                    .Where(p => p.Id != inspectionId)
                    .Where(p => p.ProductItemId == itemId)
                    .OrderByDescending(p => p.OrderNum)
                    .FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
                if (query != null)
                {
                    return query.OrderNum ?? 0;

                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 检验项目保存命令
        /// </summary>
        /// <param name="modelInspectionItems"></param>
        public virtual void InspectionSave(EntityList<ModelInspectionItem> modelInspectionItems)
        {
            if (modelInspectionItems.Any(p => (p.ModelId == 0 || p.ModelId == null) && (p.ProductItemId == 0 || p.ProductItemId == null)))
            {
                throw new ValidationException("请维护【机型】或【产品编码】其中之一！".L10N());
            }
            if (modelInspectionItems.Any(p => (p.ModelId != 0 && p.ModelId != null) && (p.ProductItemId != 0 && p.ProductItemId != null)))
            {
                throw new ValidationException("【机型】和【产品编码】不可以同时维护！".L10N());
            }
        }

        /// <summary>
        /// 获取类型为检验的工序
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<Process> GetPqcProcess(PagingInfo pagingInfo, string keyword)
        {
            var query = Query<Process>().Where(p => p.Type == ProcessType.Pqc).WhereIf(!keyword.IsNullOrEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取机型检验项列表
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<ModelInspectionItem> GetModelInspectionItems(ModelInspectionItemCriteria criteria)
        {
            var q = Query<ModelInspectionItem>();
            if (criteria.Name.IsNotEmpty())
            {
                q.Where(p => p.Name.Contains(criteria.Name));
            }
            if (criteria.ModelId != null && criteria.ModelId != 0)
            {
                q.Where(p => p.ModelId == criteria.ModelId);
            }
            if (criteria.ProcessId != null && criteria.ProcessId != 0)
            {
                q.Where(p => p.ProcessId == criteria.ProcessId);
            }
            if (criteria.ProductItemId != null && criteria.ProductItemId != 0)
            {
                q.Where(p => p.ProductItemId == criteria.ProductItemId);
            }
            return q.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
    }
}
