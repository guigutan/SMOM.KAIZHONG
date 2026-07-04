using SIE.CSM.Common;

namespace SIE.Web.CSM.Suppliers
{
    /// <summary>
    /// 区域视图配置
    /// </summary>
    public class RegionalInfoViewConfig : WebViewConfig<RegionalInfo>
    {
        /// <summary>
        /// 查询视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Region).ShowInList(width: 200);
        }
    }
}
