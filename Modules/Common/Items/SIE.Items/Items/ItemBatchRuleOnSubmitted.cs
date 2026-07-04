using SIE.Core.Items;
using SIE.Domain;

namespace SIE.Items.Items
{
    /// <summary>
    /// 物料保存前需要保存物料批次规则
    /// </summary>
    [System.ComponentModel.DisplayName("物料保存后需要保存物料批次规则")]
    [System.ComponentModel.Description("物料保存后需要保存物料批次规则")]
    public class ItemBatchRuleOnSubmitted : OnSubmitted<Item>
    {
        /// <summary>
        /// 保存物料前执行
        /// </summary>
        /// <param name="entity">物料实体</param>
        /// <param name="e">实体提交参数</param>
        protected override void Invoke(Item entity, EntitySubmittedEventArgs e)
        {
            if (e.Action == SubmitAction.Insert || e.Action == SubmitAction.Update)
            {
                Item item = entity;
                ItemBatchRule itemBatchRule = entity.GetProperty(ItemExtBatchRule.BatchRuleProperty);
                if (itemBatchRule == null)
                {
                    itemBatchRule = RT.Service.Resolve<ItemController>().GetBatchRule(entity.Id);
                    if (itemBatchRule == null)
                        itemBatchRule = new ItemBatchRule() { RetrospectType = RT.Service.Resolve<ItemController>().GetRetrospectTypeConfig(), Item = item };
                }
                else
                {
                    itemBatchRule.Item = item;
                    itemBatchRule.ItemId = item.Id;
                }
                RF.Save(itemBatchRule);
                itemBatchRule.MarkSaved();
            }
            if (e.Action == SubmitAction.Delete)
            {
                var batchRule = RT.Service.Resolve<ItemController>().GetBatchRule(entity.Id);
                if (batchRule != null)
                {
                    batchRule.PersistenceStatus = PersistenceStatus.Deleted;
                    RF.Save(batchRule);
                }
            }
        }
    }
}