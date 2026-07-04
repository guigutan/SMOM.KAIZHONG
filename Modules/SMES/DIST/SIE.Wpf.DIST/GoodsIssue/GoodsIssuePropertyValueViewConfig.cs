using SIE.DIST;
using SIE.Domain;
using SIE.Items.ViewModels;
using SIE.ObjectModel;
using SIE.Wpf.Items;
using System.Text;

namespace SIE.Wpf.DIST
{
    /// <summary>
    /// 发料单属性值视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class GoodsIssuePropertyValueExtViewConfig : WPFViewConfig<PropertyValueViewModel>
    {
        /// <summary>
        /// 自定义ViewGroup  GoodsIssuePropertyValueExtView
        /// </summary>
        public const string GoodsIssuePropertyValueExtView = "GoodsIssuePropertyValueExtViewConfig";

        /// <summary>
        /// 载具关联物料属性视图
        /// </summary>
        public const string VehicleAssociatedViewGroup = "StoreIssueViewGroup";

        #region 获取属性值 GoodIssueValueProperty
        /// <summary>
        /// 属性值
        /// </summary>
        [Label("值")]
        public static readonly Property<string> GoodIssueValueProperty = P<PropertyValueViewModel>.RegisterExtensionReadOnly("GoodIssueValue", typeof(GoodsIssuePropertyValueExtViewConfig),
            GetValue, PropertyValueViewModel.DefinitionIdProperty);

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="me">属性值视图模型</param>
        /// <returns>属性值</returns>
        public static string GetValue(PropertyValueViewModel me)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var value in me.Values)
            {
                sb.Append( value + ";");
            }

            return sb.ToString();
        }
        #endregion

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(GoodsIssuePropertyValueExtView, VehicleAssociatedViewGroup);
            View.InlineEdit();
            if (ViewGroup == GoodsIssuePropertyValueExtView || ViewGroup == VehicleAssociatedViewGroup)
            {
                PropertyValueView();
            }
        }

        /// <summary>
        /// 属性值视图
        /// </summary>
        void PropertyValueView()
        {
            View.AssignAuthorize(typeof(GoodsIssue));
            View.ClearCommands();
            if (ViewGroup == GoodsIssuePropertyValueExtView)
                View.UseCommands(typeof(GoodsIssuePropertyValueCommand), WPFCommandNames.ListEdit, WPFCommandNames.ListDelete);
            using (View.OrderProperties())
            {
                View.Property(p => p.Definition).Show(ShowInWhere.All);
                View.Property(GoodIssueValueProperty).UsePropertyValueEditor().Readonly(false).Show(ShowInWhere.List);
            }
        }
    }
}