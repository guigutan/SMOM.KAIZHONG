using SIE.Wpf.Common;
using SIE.Wpf.MES.BatchWIP.Repairs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Wpf.MES.WIP.Pressure
{
    /// <summary>
    /// 耐压采集视图配置
    /// </summary>
    public class PressureViewModelViewConfig : WPFViewConfig<PressureViewModel>
    {
        /// <summary>
        /// 验证码 视图
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
        /// 
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.UseDetail(columnCount: 3);
            View.UseCommands(typeof(PressureRestartCommand),typeof(PrintSettingCommand));
            using (View.OrderProperties())
            {
                using (View.DeclareGroup(string.Empty))
                {
                    View.Property(p => p.Tips).UseTipsEditor().ShowInDetail(columnSpan: 3, height: 0, hideLabel: true);
                    View.Property(p => p.Error).UseErrorEditor().ShowInDetail(columnSpan: 3, height: 0, hideLabel: true);
                }

                using (View.DeclareGroup("扫描信息".L10N()))
                {
                    View.Property(p => p.Barcode).UseBarcodeEditor().ShowInDetail(columnSpan: 3, height: 0, hideLabel: true);
                }

                using (View.DeclareGroup("工单信息".L10N(), detailColumnCount: 4, collapsable: true))
                {
                    View.Property(p => p.Resource.Code).HasLabel("资源编码").ShowInDetail().Readonly();
                    View.Property(p => p.Resource.Name).HasLabel("资源名称").ShowInDetail().Readonly();
                    View.Property(p => p.WipBatch.BatchNo).HasLabel("工序标签").ShowInDetail().Readonly();
                    View.Property(p => p.WipBatch.Qty).HasLabel("数量").ShowInDetail().Readonly();

                    View.Property(p => p.WorkOrder.No).UseWorkOrderDetailEditor().HasLabel("工单号").ShowInDetail().Readonly();
                    View.Property(p => p.WorkOrder.Product.Code).HasLabel("产品编码").ShowInDetail().Readonly();
                    View.Property(p => p.WorkOrder.Product.Name).HasLabel("产品名称").ShowInDetail().Readonly();
                    View.Property(p => p.WipPressure.OriginalQty).HasLabel("原始数量").ShowInDetail().Readonly();
                }
                using (View.DeclareGroup("打印信息".L10N(), detailColumnCount: 4, collapsable: true))
                {
                    View.Property(p => p.NumberRule).HasLabel("SN编码规则").ShowInDetail().Readonly();
                    View.Property(p => p.Template).HasLabel("打印模板").ShowInDetail().Readonly();
                    View.Property(p => p.PrinterSettingTpl).HasLabel("打印设置").ShowInDetail().Readonly();
                    View.Property(p => p.Printer).HasLabel("打印机").ShowInDetail().UsePrinterExEditor();

                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void ConfigVerifyCodeView() {

            View.ClearCommands();
            View.Property(p => p.VerifyCode).ShowInDetail().UsePasswordEditor();
        }
    }
}
