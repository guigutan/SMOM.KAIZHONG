using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.PackingPrints;
using SIE.MES.PackingPrints.ViewModels;
using SIE.Wpf.Command;
using System;
using System.ComponentModel;
using System.Linq;

namespace SIE.Wpf.MES.PackingPrints.Commonds
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
                return true;
            }
            return false;
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="view">视图</param>
        public override void Execute(ListLogicalView view)
        {
            var barcodes = view.SelectedEntities.OfType<PackingBarcode> ();
            if (!barcodes.Any())
                return;
            var entity = new ReprintInfoViewModel(barcodes);
            DetailsUITemplate tmpl = new DetailsUITemplate(entity.GetType());
            ControlResult ui = tmpl.CreateUI();
            ui.MainView.Data = entity;
            CRT.Workbench.ShowDialog(ui, w =>
            {
                w.Title = "补打包装号".L10N();
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
        private void ReprintBarcode(ReprintInfoViewModel entity)
        {
            PackingPrintHelper helper = new PackingPrintHelper();
            helper.PrintBarcodes(entity.BarcodeList, entity.Template, entity.Printer, (short)entity.Times, () =>
            {
                RT.Service.Resolve<PackingBarcodeController>().ReprintPackingBarcode(entity.BarcodeList, entity.Reason, entity.Times);
            });
            CRT.MessageService.ShowMessage("打印成功".L10N(), "操作提示".L10N());
        }
    }
}
