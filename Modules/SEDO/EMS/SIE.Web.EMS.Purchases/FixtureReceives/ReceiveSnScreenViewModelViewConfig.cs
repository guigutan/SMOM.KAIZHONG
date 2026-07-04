using SIE.Domain;
using SIE.EMS.Purchases.FixtureReceives;

namespace SIE.Web.EMS.Purchases.FixtureReceives
{
    /// <summary>
    /// 序列号筛选界面
    /// </summary>
    internal class ReceiveSnScreenViewModelViewConfig : WebViewConfig<ReceiveSnScreenViewModel>
    {
        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AssignAuthorize(typeof(FixtureReceive));
            View.Property(p => p.FixtureReceiveDetailId).UseDataSource((source, pagingInfo, keyword) =>
            {
                var entity = source as ReceiveSnScreenViewModel;
                if (entity == null)
                {
                    return new EntityList<FixtureReceiveDetail>();
                }
                return RT.Service.Resolve<FixtureReceiveController>().GetDetailsByReceiveId(entity.FixtureReceiveId, pagingInfo);
            });
        }
    }
}
