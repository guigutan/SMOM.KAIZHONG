using SIE.Domain;
using SIE.MES.DashBoard.Reports.FpySettings;
using SIE.MES.DashBoard.Reports.ProductFPY;
using System.Linq;

namespace SIE.Wpf.MES.DashBoard.Reports.FpySettings
{
    /// <summary>
    /// 产品机型直通率设置视图配置
    /// </summary> 
    [ManagedProperty.CompiledPropertyDeclarer]
    public class ProductModelFpySettingViewConfig : WPFViewConfig<ProductModelFpySetting>
    {
        #region 机型只读 ModelReadOnly
        /// <summary>
        /// 机型只读
        /// </summary> 
        public static readonly Property<bool> ModelReadOnlyProperty = P<ProductModelFpySetting>.RegisterExtensionReadOnly("ModelReadOnly", typeof(ProductModelFpySettingViewConfig),
            GetModelReadOnly, ProductModelFpySetting.ProductFpyListProperty);

        /// <summary>
        /// 机型只读
        /// </summary>
        public static bool GetModelReadOnly(ProductModelFpySetting me)
        {
            return me.ProductFpyList.Any(p => p.Product != null);
        }
        #endregion

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.HasDelegate(ProductModelFpySetting.ModelIdProperty);
            View.UseDefaultBehaviors();
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(ProductReportViewModel));
            View.UseCommands(typeof(AddSettingCommand), WPFCommandNames.ListEdit, WPFCommandNames.ListDelete, WPFCommandNames.ListSave);
            View.Property(p => p.Model).HasLabel("产品机型编码").Readonly(ModelReadOnlyProperty);
            View.Property(p => p.Model.Name).HasLabel("产品机型名称");
            View.Property(p => p.Desired).UseSpinEditor(e => { e.Decimals = 2; e.MinValue = 0; e.MaxValue = 100; });
            View.Property(p => p.Alarm).UseSpinEditor(e => { e.Decimals = 2; e.MinValue = 0; e.MaxValue = 100; });
            View.ChildrenProperty(p => p.ProductFpyList).HasLabel("产品");
        }

        ///<summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            // 配置明细视图
        }

        ///<summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            // 配置下拉视图
        }
    }
}