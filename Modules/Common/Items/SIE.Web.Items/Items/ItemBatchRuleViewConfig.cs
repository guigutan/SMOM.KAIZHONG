using SIE.Core.Items;
using SIE.Domain;
using SIE.ObjectModel;
using SIE.Web.Items._Extentions_;

namespace SIE.Web.Items.Items
{
    /// <summary>
    /// 物料批次规则视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class ItemBatchRuleViewConfig : WebViewConfig<ItemBatchRule>
    {
        #region 按产品分批 BatchRulePorduct
        /// <summary>
        /// 按产品分批
        /// 只有按产品分批可以编辑数量
        /// </summary>
        [Label("自定义")]
        public static readonly Property<bool> BatchRuleProperty = P<ItemBatchRule>.RegisterExtensionReadOnly("BatchRulePorduct", typeof(ItemBatchRuleViewConfig),
            GetBatchRulePorduct, ItemBatchRule.BatchRuleProperty);

        /// <summary>
        /// 按产品分批
        /// </summary>
        /// <param name="me">实体ItemBatchRule</param>
        /// <returns>bool</returns>
        public static bool GetBatchRulePorduct(ItemBatchRule me)
        {
            bool flag = me.RetrospectType == RetrospectType.Single || me.BatchRule != BatchRule.Product;
            if (flag)
                me.Qty = 1;
            return flag;
        }
        #endregion

        #region 单体追溯 RetrospectSingle
        /// <summary>
        /// 追溯方式
        /// 追溯方式是单体不可编辑其他两项
        /// </summary>
        [Label("自定义")]
        public static readonly Property<bool> RetrospectSingleProperty = P<ItemBatchRule>.RegisterExtensionReadOnly("BatchRetrospectSingle", typeof(ItemBatchRuleViewConfig),
            GeRetrospectSingle, ItemBatchRule.RetrospectTypeProperty);

        /// <summary>
        /// 追溯方式
        /// </summary>
        /// <param name="me">实体ItemBatchRule</param>
        /// <returns>bool</returns>
        public static bool GeRetrospectSingle(ItemBatchRule me)
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
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.RetrospectType).Show().Readonly(RetrospectSingleProperty)
                .UseListSetting(e => { e.HelpInfo = "追溯方式等于单体追溯不可编辑"; });
            View.Property(p => p.BatchRule).Show().Readonly(p=>p.RetrospectType == RetrospectType.Single);
            View.Property(p => p.Qty).Show().Readonly(BatchRuleProperty).UseSpinEditor(e => { e.MinValue = 1; e.MaxValue = 1000000; })
                .UseListSetting(e => { e.HelpInfo = "追溯方式等于单体追溯或批次规则不按产品分批不可编辑"; });
        }


        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.ItemId).Show(ShowInWhere.Hide);
            View.Property(p => p.RetrospectType).DefaultValue(0).UseEnumEditor(p => p.XType = "RetrospectTypeComboList").Show();
            View.Property(p => p.BatchRule).UseEnumEditor(p => p.XType = "BatchRuleComboList").Show()
                .Readonly(p => p.RetrospectType == RetrospectType.Single)
                .UseListSetting(e => { e.HelpInfo = "追溯方式等于单体追溯不可编辑"; });
            View.Property(p => p.Qty).Show().DefaultValue(1).UseItemUnitEditor(e => { e.MinValue = 1; e.MaxValue = 1000000; })
                .Readonly(p => p.RetrospectType == RetrospectType.Single || p.BatchRule != BatchRule.Product)
                .UseListSetting(e => { e.HelpInfo = "追溯方式等于单体追溯或批次规则不按产品分批不可编辑"; });
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