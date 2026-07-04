using SIE.EMS.Maintains.Plans;

namespace SIE.Web.EMS.EquipMaint.Maintains.Plans
{
    /// <summary>
    /// 保养类型视图
    /// </summary>
    public class MaintainTypeInfoViewConfig : WebViewConfig<MaintainTypeInfo>
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
