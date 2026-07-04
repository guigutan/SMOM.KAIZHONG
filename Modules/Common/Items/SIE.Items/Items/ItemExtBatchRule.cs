using SIE.Core.Items;
using SIE.Domain;
using SIE.MetaModel;

namespace SIE.Items.Items
{
    /// <summary>
    /// 附件关联属性生产批次规则
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class ItemExtBatchRule
    {
        /// <summary>
        /// 
        /// </summary>
        protected ItemExtBatchRule() { }
        #region 物料批次规则 BatchRule
        /// <summary>
        /// 物料批次规则 扩展属性。
        /// </summary>
        public static readonly Property<ItemBatchRule> BatchRuleProperty =
            P<Item>.RegisterExtension<ItemBatchRule>("BatchRule", typeof(ItemExtBatchRule));

        /// <summary>
        /// 获取物料批次规则
        /// </summary>
        /// <param name="me">物料</param>
        public static ItemBatchRule GetBatchRule(Item me)
        {
            return me.GetProperty(BatchRuleProperty);
        }

        /// <summary>
        /// 设置物料批次规则
        /// </summary>
        /// <param name="me">物料</param>
        /// <param name="value">物料批次规则</param>
        public static void SetPropertyName(Item me, ItemBatchRule value)
        {
            me.SetProperty(BatchRuleProperty, value);
        }
        #endregion
    }

    /// <summary>
    /// 作用是映射的时候能找到对应的实体
    /// </summary>
    internal class ItemExtBatchRuleConfig : EntityConfig<Item>
    {
        protected override void ConfigMeta()
        {
            Meta.Property(ItemExtBatchRule.BatchRuleProperty).DontMapColumn();
        }
    }
}