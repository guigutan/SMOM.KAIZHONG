using DevExpress.XtraSpreadsheet.Model;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.Wpf.Common;
using SIE.Wpf.MES.TaskManagement.Pressure.Commands;
using SIE.Wpf.MES.WIP.Pressure;
using System;
using System.Collections.Generic;

namespace SIE.Wpf.MES.TaskManagement.Pressure
{
    /// <summary>
    /// 耐压采集视图配置
    /// </summary>
    public class PressureSnPrintViewModelViewConfig : WPFViewConfig<PressureSnPrintViewModel>
    {
        /// <summary>
        /// SN条码打印 视图
        /// </summary>
        public const string PressureSnPrintView = "PressureSnPrintView";
        /// <summary>
        /// 打印SN 视图
        /// </summary>
        public const string PrintSnView = "PrintSnView";
        /// <summary>
        /// 验证码 视图
        /// </summary>
        public const string VerifyCodeView = "VerifyCodeView";
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(VerifyCodeView, PressureSnPrintView, PrintSnView);
            if (ViewGroup == VerifyCodeView)
                ConfigVerifyCodeView();

            else if (ViewGroup == PrintSnView)
                ConfigPrintSnView();
            else if (ViewGroup == PressureSnPrintView)
                ConfigPressureSnPrintView();
        }

        /// <summary>
        /// SN条码打印视图
        /// </summary>
        protected void ConfigPressureSnPrintView()
        {
            View.UseDetail(columnCount: 3);
            View.UseCommands(typeof(PressureRestartCommand), typeof(PrintSettingCommand), typeof(ShowPrintSnWinCommand));
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
                    View.Property(p => p.DispatchTask).HasLabel("任务单号").ShowInDetail()
                        .UsePagingLookUpEditor(p => p.ReloadDataOnPopping = true)
                        .UseDataSource((e, p, s) =>
                        {
                            var entity = e as PressureSnPrintViewModel;
                            var statuses = new List<DispatchTaskStatus>() { DispatchTaskStatus.Dispatched, DispatchTaskStatus.Executing };
                            return RT.Service.Resolve<DispatchController>().GetDispatchTaskByResourceId(entity.Workstation.ResourceId ?? 0, p, s, statuses, true);
                        });
                    View.Property(p => p.DispatchTask.DispatchQty).HasLabel("计划数量").ShowInDetail().Readonly();

                    View.Property(p => p.WorkOrder.No).UseWorkOrderDetailEditor().HasLabel("工单号").ShowInDetail().Readonly();
                    View.Property(p => p.WorkOrder.Product.Code).HasLabel("产品编码").ShowInDetail().Readonly();
                    View.Property(p => p.WorkOrder.Product.Name).HasLabel("产品名称").ShowInDetail().Readonly();
                    View.Property(p => p.DispatchTask.PrintedQty).HasLabel("已打印数量").ShowInDetail().Readonly();
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
        protected void ConfigPrintSnView()
        {

            View.ClearCommands();
            View.Property(p => p.PrintQty).ShowInDetail().UseSpinEditor(p => { p.MinValue = 1; p.Decimals = 0; });
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
