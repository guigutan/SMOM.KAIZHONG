using SIE.Core.Items;
using SIE.Domain;
using SIE.ObjectModel;

namespace SIE.WPF.Items.Items
{
    /// <summary>
    /// 物料批次规则视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class ItemBatchRuleViewConfig : WPFViewConfig<ItemBatchRule>
    {
        #region 按产品分批 BatchRulePorduct
        /// <summary>
        /// 按产品分批
        /// 只有按产品分批可以编辑数量
        /// </summary>
        [Label("自定义")]
        internal static readonly Property<bool> BatchRuleProperty = P<ItemBatchRule>.RegisterExtensionReadOnly("BatchRulePorduct", typeof(ItemBatchRuleViewConfig),
            GetBatchRulePorduct, ItemBatchRule.BatchRuleProperty);

        /// <summary>
        /// 按产品分批
        /// </summary>
        /// <param name="me">实体ItemBatchRule</param>
        /// <returns>bool</returns>
        internal static bool GetBatchRulePorduct(ItemBatchRule me)
        {
            bool flag = me.RetrospectType == RetrospectType.Single || me.BatchRule != BatchRule.Product;
            if (flag)
                me.Qty = 1;
            return flag;
        }
        #endregion

        #region 追溯方式 RetrospectType
        /// <summary>
        /// 追溯方式
        /// 追溯方式是单体不可编辑其他两项
        /// </summary>
        [Label("自定义")]
        internal static readonly Property<bool> RetrospectTypeProperty = P<ItemBatchRule>.RegisterExtensionReadOnly("BatchRetrospectType", typeof(ItemBatchRuleViewConfig),
            GetRetrospectType, ItemBatchRule.RetrospectTypeProperty);

        /// <summary>
        /// 追溯方式
        /// </summary>
        /// <param name="me">实体ItemBatchRule</param>
        /// <returns>bool</returns>
        internal static bool GetRetrospectType(ItemBatchRule me)
        {
            return me.RetrospectType == RetrospectType.Single;
        }
        #endregion 

        #region 单体追溯 RetrospectSingle
        /// <summary>
        /// 追溯方式
        /// 追溯方式是单体不可编辑其他两项
        /// </summary>
        [Label("自定义")]
        internal static readonly Property<bool> RetrospectSingleProperty = P<ItemBatchRule>.RegisterExtensionReadOnly("BatchRetrospectSingle", typeof(ItemBatchRuleViewConfig),
            GeRetrospectSingle, ItemBatchRule.RetrospectTypeProperty);

        /// <summary>
        /// 追溯方式
        /// </summary>
        /// <param name="me">实体ItemBatchRule</param>
        /// <returns>bool</returns>
        internal static bool GeRetrospectSingle(ItemBatchRule me)
        {
            if (me.RetrospectType == RetrospectType.Single)
            {
                me.BatchRule = null;
                me.Qty = 1;
            }
            return false;
        }
        #endregion 

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.RetrospectType).Show().Readonly(RetrospectSingleProperty);
            View.Property(p => p.BatchRule).Show().Readonly(RetrospectTypeProperty);
            View.Property(p => p.Qty).Show().Readonly(BatchRuleProperty).UseSpinEditor(e => { e.MinValue = 1; e.MaxValue = 1000000; });
        }

        /// <summary>
        /// 下拉选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.HasDelegate(ItemBatchRule.BatchRuleProperty);
            View.Property(p => p.BatchRule);
            View.Property(p => p.Qty);
        }
    }
}