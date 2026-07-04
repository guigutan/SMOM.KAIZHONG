using SIE.MES.OnOffDuty;

namespace SIE.Wpf.MES.OnOffDuty
{
    class OnOffDutyCollectDetailViewModelViewConfig : WPFViewConfig<OnOffDutyCollectDetailViewModel>
    {


        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DomainName("采集操作记录");
            CollectionView();
        }

        /// <summary>
        /// 配置CollectionView视图
        /// </summary>
        protected void CollectionView()
        {
            View.AssignAuthorize(typeof(OnOffDutyViewModel));
            using (View.OrderProperties())
            {
                View.ClearCommands();
                View.Property(p => p.StaffNO).Show(ShowInWhere.All);
                View.Property(p => p.StaffName).Show(ShowInWhere.All);
                View.Property(p => p.ResourceName).Show(ShowInWhere.All);
                View.Property(p => p.ProcessName).Show(ShowInWhere.All);
                View.Property(p => p.StationName).Show(ShowInWhere.All);
                View.Property(p => p.OnOffDutyType).Show(ShowInWhere.All);
                View.Property(p => p.InputDate).Show(ShowInWhere.All);
                View.Property(p => p.CollectUseName).Show(ShowInWhere.All);
                View.Property(p => p.CollectDate).UseListSetting(e => e.ListGridWidth = 150).Show(ShowInWhere.All).Readonly();
            }
        }
    }
}
