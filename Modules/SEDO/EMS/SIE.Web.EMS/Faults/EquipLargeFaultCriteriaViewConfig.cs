using SIE.EMS.Faults;

namespace SIE.Web.EMS.Faults
{
    /// <summary>
    /// 故障大类查询实体视图配置
    /// </summary>
    internal class EquipLargeFaultCriteriaViewConfig : WebViewConfig<EquipLargeFaultCriteria>
    {
        ///<summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.LargeCode);
            View.Property(p => p.LargeName);
            View.Property(p => p.MiddleCode);
            View.Property(p => p.MiddleName);
            View.Property(p => p.SmallCode);
            View.Property(p => p.SmallName);
        }
    }
}
