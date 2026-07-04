using SIE.Domain;
using SIE.EMS.Purchases.EquipmentReceives;

namespace SIE.Web.EMS.Purchases.EquipmentReceives
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
            View.AssignAuthorize(typeof(EquipmentReceive));
            View.Property(p => p.EquipmentReceiveDetailId).UseDataSource((source, pagingInfo, keyword) =>
            {
                var entity = source as ReceiveSnScreenViewModel;
                if (entity == null)
                {
                    return new EntityList<EquipmentReceiveDetail>();
                }
                return RT.Service.Resolve<EquipmentReceiveController>().GetDetailsByReceiveId(entity.EquipmentReceiveId, pagingInfo);
            });
        }
    }
}
