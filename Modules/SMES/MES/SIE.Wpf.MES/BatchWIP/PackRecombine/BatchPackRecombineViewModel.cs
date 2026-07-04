using SIE.Common.Configs;
using SIE.ManagedProperty;
using SIE.MES.WIP.PackRecombine;
using SIE.MES.WIP.PackRecombine.Configs;
using SIE.ObjectModel;
using SIE.Wpf.Common;
using SIE.Wpf.Common.Editors;
using SIE.Wpf.MES.Controls.Messager;
using SIE.Wpf.MES.WIP.PackRecombine;
using SIE.Wpf.MES.WIP.PackRecombine.Commands;
using System;

namespace SIE.Wpf.MES.BatchWIP.PackRecombine
{
    /// <summary>
    /// 批次包装拆合视图模型基类
    /// </summary>
    [RootEntity, Serializable]
    [EntityWithConfig(typeof(PackRecombinePortConfig))]
    [EntityWithConfig(typeof(PackRecombineDevicePortConfig))]
    [Label("批次包装拆合")]
    public class BatchPackRecombineViewModel : PackRecombineBaseViewModel
    {
        /// <summary>
        /// 条码扫描
        /// </summary>
        /// <param name="e"></param>
        protected override void OnBarcodeChanged(ManagedPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (!Barcode.IsNotEmpty()) return;
            try
            {
                if (PackScanMode == ScanMode.Move)
                    SplitPacking(Barcode, true);
                else if (PackScanMode == ScanMode.Join)
                    JoinPacking(Barcode, true);
                else
                    SearchPacking(Barcode, true);
            }
            catch (Exception exc)
            {
                ShowError(exc);
            }
            finally
            {
                Barcode = null;
            }
        }
    }

    internal class BatchPackRecombineViewModelViewConfig : WPFViewConfig<BatchPackRecombineViewModel>
    {
        protected override void ConfigDetailsView()
        {
            View.AssignAuthorize(typeof(BatchPackRecombineViewModel));
            View.UseCommands(typeof(PackRecombineReset));
            View.UseDetail(columnCount: 3);
            using (View.DeclareGroup(string.Empty))
            {
                View.Property(p => p.Tips).UseEditor(TipsEditor.EditorName).ShowInDetail(columnSpan: 3, height: 0, hideLabel: true);
                View.Property(p => p.Error).UseEditor(ErrorEditor.EditorName).ShowInDetail(columnSpan: 3, height: 0, hideLabel: true);
              
            }
            using (View.DeclareGroup("扫描条码".L10N()))
            {
                View.Property(p => p.Barcode).UseEditor(BarcodeEditor.EditorName).ShowInDetail(columnSpan: 2, height: 0, hideLabel: true);
                View.Property(p => p.PackScanMode).UseEditor(EnumButtonEditor.EditorName).ShowInDetail(columnSpan: 1, height: 0, hideLabel: true);
            }
            using (View.DeclareGroup("打印机配置".L10N()))
            {
                View.Property(p => p.Printer).UsePrinterEditor();
            }
            using (View.DeclareGroup("工单信息".L10N(), detailColumnCount: 4, collapsable: true))
            {
                View.Property(p => p.WorkOrder.No).UseEditor(PackRecombineEditor.EditorName).HasLabel("工单号");
                View.Property(p => p.WorkOrder.Product.Code).HasLabel("产品编码");
                View.Property(p => p.WorkOrder.Product.Name).HasLabel("产品名称");
                View.Property(p => p.WorkOrder.Product.Model.Name).HasLabel("产品型号");
            }
        }
    }
}