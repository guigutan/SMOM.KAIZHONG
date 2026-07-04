using SIE.EMS.Purchases.FixtureReceives;
using System;

namespace SIE.Web.EMS.Purchases.FixtureReceives
{
    /// <summary>
    /// 序列号打印界面
    /// </summary>
    public class ReceiveSnPrintViewModelViewConfig : WebViewConfig<ReceiveSnPrintViewModel>
    {
        /// <summary>
        /// 备件视图
        /// </summary>
        public const string FixtureView = "FixtureView";

        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(FixtureView);
            if (ViewGroup == FixtureView)
            {
                ConfigSparePartView();
            }
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AssignAuthorize(typeof(FixtureReceive));
            View.Property(p => p.TemplateId).UseDataSource((e, p, r) =>
            {
                return RT.Service.Resolve<FixtureReceivePrintController>().GetPrintTemplatesByType(typeof(FixtureReceiveSnPrintable).GetQualifiedName(), p, r);
            });
        }

        /// <summary>
        /// 备件视图
        /// </summary>
        protected void ConfigSparePartView()
        {
            View.AssignAuthorize(typeof(FixtureReceive));
            using (View.OrderProperties())
            {
                View.Property(p => p.TemplateId).UseDataSource((e, p, r) =>
                {
                    return RT.Service.Resolve<FixtureReceivePrintController>().GetPrintTemplatesByType(typeof(FixtureReceiveSnPrintable).GetQualifiedName(), p, r);
                }).ShowInDetail();
            }
        }
    }
}
