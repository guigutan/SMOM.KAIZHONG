using SIE.Wpf.Common.Editors;
using SIE.Wpf.MES.PackingQC.Commands;
using SIE.Wpf.MES.WIP;
using SIE.Wpf.MES.WIP.SuspectReport.Commands;
using System;

namespace SIE.Wpf.MES.PackingQC
{
    /// <summary>
    /// 装箱配置
    /// </summary>
    public class PackingQcViewModelViewConfig :WPFViewConfig<PackingQcViewModel>
    {
        public const string VerifyCodeView = "VerifyCodeView";

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(VerifyCodeView);
            if (ViewGroup == VerifyCodeView)
                ConfigVerifyCodeView();
            else if (ViewGroup == CollectionUITemplate.CollectionUIViewGroup)
                ConfigDetailsView();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.UseCommands(typeof(SIE.Wpf.MES.PackingQC.Commands.CollectRestartCommand), typeof(NormalCommand), typeof(UnboxingCommand), typeof(BoxExChangeCommand), typeof(SubmitCommand), typeof(ShowSuspectReportCommand),typeof(DeleteCommand));
            View.UseDetail(columnCount: 6);
            using (View.OrderProperties())
            {
                using (View.DeclareGroup(string.Empty))
                {
                    View.Property(p => p.Tips).UseEditor("TipsEditor").ShowInDetail(columnSpan: 6, height: 0, hideLabel: true);
                    View.Property(p => p.Error).UseEditor("ErrorEditor").ShowInDetail(columnSpan: 6, height: 0, hideLabel: true);
                }

                using (View.DeclareGroup("扫描信息".L10N()))
                {
                    View.Property(p => p.Barcode).UseEditor(BarcodeEditor.EditorName).ShowInDetail(columnSpan: 6, height: 0, hideLabel: true);
                }

                using (View.DeclareGroup("工单信息".L10N(), collapsable: true))
                {
                    View.Property(p => p.WorkOrder.No).HasLabel("工单号").UseEditor("WorkOrderEditor").ShowInDetail(columnSpan: 2).Readonly();
                    View.Property(p => p.WorkOrder.Product.Name).HasLabel("产品名称").ShowInDetail(columnSpan: 2).Readonly();
                    View.Property(p => p.WorkOrder.Product.Code).HasLabel("产品编码").ShowInDetail(columnSpan: 2).Readonly();
                    View.Property(p => p.blueInt).ShowInDetail(columnSpan: 2).Readonly();
                    View.Property(p => p.blueZInt).ShowInDetail(columnSpan: 2).Readonly();
                    View.Property(p => p.XtBlue).ShowInDetail(columnSpan: 2).Readonly();
                    View.Property(p => p.YXtBlue).ShowInDetail(columnSpan: 2).Readonly();
                    View.Property(p => p.DeleteState).ShowInDetail(columnSpan: 2).Readonly();
                    //UseMemoEditor
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        protected void ConfigVerifyCodeView()
        {

            View.ClearCommands();
            View.Property(p => p.VerifyCode).ShowInDetail().UsePasswordEditor();
        }
    }
}
