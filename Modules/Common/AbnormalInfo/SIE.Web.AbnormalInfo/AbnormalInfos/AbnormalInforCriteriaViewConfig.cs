using SIE.AbnormalInfo.AbnormalInfos;

namespace SIE.Web.AbnormalInfo.AbnormalInfos
{
    /// <summary>
    /// 异常信息查询实体视图配置
    /// </summary>
    public class AbnormalInforCriteriaViewConfig : WebViewConfig<AbnormalInforCriteria>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.No).Show(ShowInWhere.All);
                View.Property(p => p.AbnormalInfoDefinitionId).HasLabel("异常信息").Show(ShowInWhere.All);
                View.Property(p => p.ProcessId).Show(ShowInWhere.All);
                View.Property(p => p.WorkShopId).Show(ShowInWhere.All);
                View.Property(p => p.LineId).Show(ShowInWhere.All);
                View.Property(p => p.ItemId).Show(ShowInWhere.All);
                View.Property(p => p.AbnormalStatus).Show(ShowInWhere.All);
                View.Property(p => p.CreateDate).UseDateRangeEditor(p => { p.DateFormat = "Y/m/d"; p.DateRangeType = ObjectModel.DateRangeType.Month; }).Show(ShowInWhere.All);
            }
        }
    }
}
