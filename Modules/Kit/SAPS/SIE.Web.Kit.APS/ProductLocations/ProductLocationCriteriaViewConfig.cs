using SIE.Kit.APS.Common;
using SIE.Kit.APS.ProductLocations;
using SIE.Web.Resources;

namespace SIE.Web.Kit.APS.ProductLocations
{
    /// <summary>
    /// 产品定位查询视图
    /// </summary>
    public class ProductLocationCriteriaViewConfig : WebViewConfig<ProductLocationCriteria>
    {
        /// <summary>
        /// 列表逻辑视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.EnterpriseId).UseFactoryEditor().Show(ShowInWhere.All);
                View.Property(p => p.Classification).Show(ShowInWhere.All);
                View.Property(p => p.TypeValue).UseProductLocationTypeEditor(p =>
                {
                    p.Editable = false;
                    p.ReloadDataOnPopping = true;
                    p.JsClassName = "SIE.Web.Kit.APS.ProductLocations.Scripts.ProductLocationTypeComboList";
                    p.ValueField = ClassificationInfo.KeyProperty.Name;
                }).UseListSetting(e => { e.HelpInfo = "显示选择分类的分类值"; }).Show(ShowInWhere.All);
            }
        }
    }
}
