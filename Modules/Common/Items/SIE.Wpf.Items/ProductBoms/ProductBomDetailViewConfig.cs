using SIE.Domain;
using SIE.Items;
using System.Linq;

namespace SIE.Wpf.Items
{
    /// <summary>
    /// 产品BOM明细视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class ProductBomDetailViewConfig : WPFViewConfig<ProductBomDetail>
    {
        /// <summary>
        /// 工艺单BOM明细视图ViewGroup
        /// </summary>
        public const string TechOrderItemView = "TechOrderItemView";

        #region 替代料值 AlternativeProperty
        /// <summary>
        /// 替代料值
        /// </summary>
        public static readonly Property<string> AlternativeProperty = P<ProductBomDetail>.RegisterExtensionReadOnly("Alternative", typeof(ProductBomDetailViewConfig),
            GetAlternative, ProductBomDetail.RemarkProperty);

        /// <summary>
        /// 替代料值
        /// </summary>
        /// <param name="detail">产品BOM明细</param>
        /// <returns>string</returns>
        public static string GetAlternative(ProductBomDetail detail)
        {
            string[] result = new string[detail.AlternativeList.Count];
            for (int i = 0; i < detail.AlternativeList.Count; i++)
            {
                result[i] = detail.AlternativeList[i].Item.Code;
            }

            return string.Join(";", result);
        }
        #endregion

        #region 物料属性值 Item
        /// <summary>
        /// 物料属性值
        /// </summary>
        public static readonly Property<string> ItemPropertyProperty = P<ProductBomDetail>.RegisterExtensionReadOnly("ItemProperty", typeof(ProductBomDetailViewConfig),
            GetItemProperty, ProductBomDetail.PropertyValueListProperty);

        /// <summary>
        /// 获取物料属性
        /// </summary>
        /// <param name="detail">产品BOM明细</param>
        /// <returns>string</returns>
        public static string GetItemProperty(ProductBomDetail detail)
        {
            var groups = detail.PropertyValueList.GroupBy(p => p.Definition.Name).ToList();
            string[] result = new string[groups.Count];

            for (int i = 0; i < groups.Count; i++)
            {
                var values = groups[i].Select(p => p.Value);
                result[i] = groups[i].Key + "：" + string.Join("、", values);
            }

            return string.Join("；", result);
        }
        #endregion

        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDetail(3);
            View.UseChildrenAsHorizontal(true);
            View.UseDefaultBehaviors();
            View.DeclareExtendViewGroup(TechOrderItemView);
            if (ViewGroup == TechOrderItemView)
                ConfigTechOrderItemView();
        }

        /// <summary>
        /// 默认表格视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseChildrenAsHorizontal(true);
            View.UseCommands(WPFCommandNames.ListAdd, WPFCommandNames.ListEdit, WPFCommandNames.ListDelete);

            using (View.OrderProperties())
            {
                View.Property(p => p.Item).UseMaterialCombinationEditor().Readonly(false);
                View.Property(p => p.Item.Name).HasLabel("物料名称");
                View.Property(p => p.Item.SpecificationModel).HasLabel("规格型号");
                View.Property(ProductBomDetailViewConfig.ItemPropertyProperty).UseItemPropertyEditor().HasLabel("物料属性").Show(ShowInWhere.All).Readonly(false);
                View.Property(p => p.UnitQty).UseSpinEditor(e => e.MinValue = 0);
                View.Property(p => p.Item.Unit.Name).HasLabel("单位");
                View.Property(p => p.LossRate).UseSpinEditor(e => e.MinValue = 0);
                View.Property(p => p.Item.ItemSourceType).UseEnumEditor().HasLabel("来源类型");
                View.Property(ProductBomDetailViewConfig.AlternativeProperty).UseAlternativeEditor().HasLabel("物料替代").Readonly(false);
                View.Property(p => p.Remark);
                View.Property(p => p.ProcessSegment);
                View.ChildrenProperty(p => p.AlternativeList).IsVisible = false;
                View.ChildrenProperty(p => p.PropertyValueList).IsVisible = false;
            }
        }

        /// <summary>
        /// 工艺单BOM明细视图
        /// </summary>
        private void ConfigTechOrderItemView()
        {
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.Item).ShowInList().Readonly();
                View.Property(p => p.Item.Name).ShowInList().HasLabel("物料名称");
                View.Property(p => p.Item.SpecificationModel).ShowInList().HasLabel("规格型号");
                View.Property(ProductBomDetailViewConfig.ItemPropertyProperty).UseItemPropertyEditor().ShowInList().HasLabel("物料属性").Readonly();
                View.Property(p => p.UnitQty).ShowInList().UseSpinEditor(e => e.MinValue = 0).Readonly();
                View.Property(p => p.Item.Unit.Name).ShowInList().HasLabel("单位");
                View.Property(p => p.LossRate).UseSpinEditor(e => e.MinValue = 0).ShowInList().Readonly();
                View.Property(p => p.Item.ItemSourceType).UseEnumEditor().ShowInList().HasLabel("来源类型");
                View.Property(ProductBomDetailViewConfig.AlternativeProperty).UseAlternativeEditor().HasLabel("物料替代").ShowInList().Readonly();
                View.Property(p => p.Remark).ShowInList().Readonly();
                View.Property(p => p.ProcessSegment).ShowInList().Readonly();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.ChildrenProperty(p => p.AlternativeList).IsVisible = false;
                View.ChildrenProperty(p => p.PropertyValueList).IsVisible = false;
            }
        }
    }
}