using SIE.Domain;
using SIE.EMS.Purchases.SparePartReceives;
using SIE.EMS.Purchases.SparePartReceives.ViewModels;

namespace SIE.Web.EMS.Purchases.SparePartReceives.ViewModels
{
    /// <summary>
    /// 序列号筛选界面
    /// </summary>
    internal class LotScreenViewModelViewConfig : WebViewConfig<LotScreenViewModel>
    {
        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AssignAuthorize(typeof(SparePartReceive));
            View.Property(p => p.SparePartReceiveDetailId).UseDataSource((source, pagingInfo, keyword) =>
            {
                var entity = source as LotScreenViewModel;
                if (entity == null)
                {
                    return new EntityList<SparePartReceiveDetail>();
                }
                return RT.Service.Resolve<SparePartReceiveController>().GetDetailsByReceiveId(entity.SparePartReceiveId, pagingInfo);
            });
        }
    }
}
