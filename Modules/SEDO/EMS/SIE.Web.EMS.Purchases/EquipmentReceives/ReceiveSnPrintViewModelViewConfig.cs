using SIE.EMS.Purchases.EquipmentReceives;
using SIE.EMS.Purchases.SparePartReceives;
using System;

namespace SIE.Web.EMS.Purchases.EquipmentReceives
{
    /// <summary>
    /// 序列号打印界面
    /// </summary>
    public class ReceiveSnPrintViewModelViewConfig : WebViewConfig<ReceiveSnPrintViewModel>
    {
        /// <summary>
        /// 备件视图
        /// </summary>
        public const string SparePartView = "SparePartView";

        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(SparePartView);
            if (ViewGroup == SparePartView)
            {
                ConfigSparePartView();
            }
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AssignAuthorize(typeof(EquipmentReceive));
            View.Property(p => p.TemplateId).UseDataSource((e, p, r) =>
            {
                return RT.Service.Resolve<EquipmentReceiveSnController>().GetPrintTemplatesByType(typeof(EquipReceiveSnPrintable).GetQualifiedName(), p, r);
            });
        }

        /// <summary>
        /// 备件视图
        /// </summary>
        protected void ConfigSparePartView()
        {
            View.AssignAuthorize(typeof(SparePartReceive));
            using (View.OrderProperties())
            {
                View.Property(p => p.TemplateId).UseDataSource((e, p, r) =>
                {
                    return RT.Service.Resolve<EquipmentReceiveSnController>().GetPrintTemplatesByType(typeof(SparePartLotSnPrintable).GetQualifiedName(), p, r);
                }).ShowInDetail();
            }
        }
    }
}
