using SIE.Domain;
using SIE.Items;
using SIE.MES.DashBoard.Reports.FpySettings;

namespace SIE.Web.MES.DashBoard.Reports.FpySettings
{
    /// <summary>
    /// 产品机型直通率查询实体视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class ProductModelFpySettingCriteriaViewConfig : WebViewConfig<ProductModelFpySettingCriteria>
    {
        #region 清除产品 ClearProduct
        /// <summary>
        /// 清除产品
        /// </summary> 
        public static readonly Property<bool> ClearProductProperty = P<ProductModelFpySettingCriteria>.RegisterExtensionReadOnly("ClearProduct", typeof(ProductModelFpySettingCriteriaViewConfig),
            GetClearProduct, ProductModelFpySettingCriteria.ModelIdProperty);

        /// <summary>
        /// 清除产品
        /// </summary>
        /// <param name="me">查询实体</param>
        /// <returns>是否空</returns>
        public static bool GetClearProduct(ProductModelFpySettingCriteria me)
        {
            me.Product = null;
            return false;
        }
        #endregion

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.Property(p => p.Model).HasLabel("产品机型").ShowInDetail().UsePagingLookUpEditor();
            View.Property(p => p.Product).UseDataSource((e, c, r) =>
            {
                var setting = e as ProductModelFpySettingCriteria;
                if (setting?.Model != null)
                    return RT.Service.Resolve<ItemController>().GetItems(ItemType.Material, c, r, setting.ModelId.Value);
                return RT.Service.Resolve<ItemController>().GetProducts(ItemType.Product, c, r);
            }).UsePagingLookUpEditor().HasLabel("产品编码").ShowInDetail().Readonly(ClearProductProperty);
        }
    }
}