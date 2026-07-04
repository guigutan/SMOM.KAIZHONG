using SIE.Barcodes;
using SIE.Domain;
using SIE.Wpf.Barcodes.Utils;
using SIE.Wpf.Barcodes.ViewModels;
using SIE.Wpf.Command;
using System;
using System.Linq;

namespace SIE.Wpf.Barcodes.Commonds
{
    /// <summary>
    /// 补打
    /// </summary>
    [Command(Label = "补打", ToolTip = "补打", GroupType = CommandGroupType.Edit)]
    public class ReprintCommand : ListViewCommand
    {
        /// <summary>
        /// 是否能执行
        /// </summary>
        /// <param name="view">视图</param>
        /// <returns>选择条码条码数量大于0且工单相同返回true，否则返回false</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            if (view.SelectedEntities.Count > 0)
            {
                var barcodes = view.SelectedEntities.OfType<Barcode>();
                return barcodes.All(p => p.WorkOrderId == barcodes.FirstOrDefault().WorkOrderId);
            }

            return false;
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="view">视图</param>
        public override void Execute(ListLogicalView view)
        {
            var barcodes = view.SelectedEntities.OfType<Barcode>();
            if (!barcodes.Any())
                return;
            var entity = new ReprintViewModel(barcodes);
            DetailsUITemplate tmpl = new DetailsUITemplate(entity.GetType());
            ControlResult ui = tmpl.CreateUI();
            ui.MainView.Data = entity;
            CRT.Workbench.ShowDialog(ui, w =>
            {
                w.Title = "{0} 条码".L10nFormat(Meta.Label);
                w.Width = 480;
                w.Height = 280;
                w.Closing += (s, e) =>
                {
                    if (w.Result == 0)
                    {
                        try
                        {
                            ReprintBarcode(entity);
                        }
                        catch (Exception ex)
                        {
                            CRT.MessageService.ShowException(ex);
                            e.Cancel = true;
                        }
                    }
                };
            });
        }

        /// <summary>
        /// 打印保存
        /// </summary>
        /// <param name="entity">视图</param>
        private void ReprintBarcode(ReprintViewModel entity)
        {
            PrintHelper helper = new PrintHelper();
            var viewModelBarcodeList = entity.BarcodeList.GroupBy(p => p.WorkOrderId);
            viewModelBarcodeList.ForEach(e =>
            {
                var barcodes = e.ToList();
                EntityList<Barcode> barcodeList = new EntityList<Barcode>();
                barcodeList.AddRange(barcodes);
                helper.PrintBarcodes(barcodeList, entity.Template, entity.Printer, (short)entity.Times, () =>
                {
                    RT.Service.Resolve<BarcodeController>().Reprint(barcodeList, BarcodeLogType.Remedy, entity.Reason, entity.Times);
                });
            });
            CRT.MessageService.ShowMessage("打印成功".L10N(), "操作提示".L10N());
        }
    }
}