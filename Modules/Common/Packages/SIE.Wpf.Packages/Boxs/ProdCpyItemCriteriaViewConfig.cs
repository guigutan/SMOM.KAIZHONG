using SIE.Domain;
using SIE.Items;
using SIE.Packages.Boxs;

namespace SIE.Wpf.Packages.Boxs
{
    /// <summary>
    /// 产品容量添加命令物料查询实体视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class ProdCpyItemCriteriaViewConfig : WPFViewConfig<ProdCpyItemCriteria>
    {
        #region 产品机型清空 ClearModel
        /// <summary>
        /// 产品机型清空
        /// </summary>
        public static readonly Property<bool> ClearModelProperty = P<ProdCpyItemCriteria>.RegisterExtensionReadOnly("ClearModel", typeof(ProdCpyItemCriteriaViewConfig),
            GetClearModel, ProdCpyItemCriteria.ProductFamilyIdProperty);

        /// <summary>
        /// 产品机型清空
        /// </summary>
        public static bool GetClearModel(ProdCpyItemCriteria me)
        {
            var criteria = me;
            criteria.ProductModel = null;
            return false;
        }
        #endregion

        /// <summary>
        /// 默认配置
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();

            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show();
                View.Property(p => p.Name).Show();
                View.Property(p => p.Type).Show();
                View.Property(p => p.ItemSourceType).Show();
                View.Property(p => p.ProductFamily).Show(ShowInWhere.All).UseProductFamilyEditor(p => p.DisplayMember = nameof(ProductFamily.Name));
                View.Property(p => p.ProductModel).Show(ShowInWhere.All).UseProductModelEditor(p => { p.DisplayMember = nameof(ProductModel.Name); p.ReloadDataOnPopping = true; }).Readonly(ClearModelProperty);
            }
        }
    }
}
