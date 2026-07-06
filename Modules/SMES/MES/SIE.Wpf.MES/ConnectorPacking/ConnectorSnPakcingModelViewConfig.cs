using SIE.Wpf.Common.Editors;
using SIE.Wpf.MES.ConnectorPacking.Commands;
using SIE.Wpf.MES.WIP;
using System;

namespace SIE.Wpf.MES.ConnectorPacking
{
    /// <summary>
    /// 连接器单体包装采集界面
    /// </summary>
    public class ConnectorSnPakcingModelViewConfig : WPFViewConfig<ConnectorSnPackingViewModel>
    {
        /// <summary>
        /// 验证码视图
        /// </summary>
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
        /// 默认视图配置
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.UseCommands(typeof(ConnectorSnRestartCommand), typeof(ConnectorSnNormalCommand), typeof(ConnectorSnUnboxingCommand), typeof(ConnectorSnChangeCommand), typeof(ConnectorSnSubmitCommand), typeof(ConnectorDeleteCommand), typeof(ConnectorReplaceCommand));
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
                    View.Property(p => p.BatchLable).ShowInDetail(columnSpan: 2).Readonly();
                    View.Property(p => p.BatchZInt).ShowInDetail(columnSpan: 2).Readonly();
                    View.Property(p => p.BatchInt).ShowInDetail(columnSpan: 2).Readonly();
                    View.Property(p => p.DeleteState).ShowInDetail(columnSpan: 2).Readonly();
                    //UseMemoEditor
                }
            }
        }

        /// <summary>
        /// 验证码视图
        /// </summary>
        protected void ConfigVerifyCodeView()
        {
            View.ClearCommands();
            View.Property(p => p.VerifyCode).ShowInDetail().UsePasswordEditor();
        }
    }
}
