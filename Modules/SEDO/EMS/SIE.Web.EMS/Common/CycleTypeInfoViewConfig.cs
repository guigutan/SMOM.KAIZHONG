using SIE.EMS.Common.Entity;

namespace SIE.Web.EMS.Common
{
    /// <summary>
    /// 周期类型视图
    /// </summary>
    public class CycleTypeInfoViewConfig : WebViewConfig<CycleTypeInfo>
    {
        /// <summary>
        /// 列表逻辑视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {            
            View.Property(p => p.Value);
            View.WithoutPaging();
        }
    }
}
