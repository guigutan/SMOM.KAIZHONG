using SIE.Barcodes;
using SIE.Domain.Validation;
using SIE.Wpf.Barcodes.ViewModels;
using SIE.Wpf.Command;
using System;
using System.Linq;

namespace SIE.Wpf.Barcodes.Commonds
{
    /// <summary>
    /// 条码报废命令
    /// </summary>
    [Command(Label = "条码报废", ToolTip = "条码报废", GroupType = CommandGroupType.Edit)]
    public class ScrapBarcodeCommand : ListSaveCommand
    {
        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>选择的条码都是非报废状态返回true，否则返回false</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return view.SelectedEntities.Count > 0 && view.SelectedEntities.Cast<Barcode>().All(p => p.IsScraped);
        }

        /// <summary>
        /// 命令执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var vm = new ScrapBarcodeViewModel(view.SelectedEntities.Cast<Barcode>());
            var template = new DetailsUITemplate<ScrapBarcodeViewModel>();
            var ui = template.CreateUI();
            ui.MainView.Data = vm;
            var result = CRT.Workbench.ShowDialog(ui, v =>
            {
                v.Title = "条码报废".L10N();
                v.Width = 300;
                v.Height = 120;
                v.Closing += (o, e) =>
                {
                    if (v.Result == 0)
                    {
                        try
                        {
                            var broken = vm.Validate();
                            if (broken.Count > 0)
                                throw new ValidationException(broken.ToString());
                            RT.Service.Resolve<BarcodeController>().BarcodeScrap(vm.BarcodeList, vm.Reason);
                        }
                        catch (Exception exc)
                        {
                            exc.Alert();
                            e.Cancel = true;
                        }
                    }
                };
            });
            if (result == 0)
            {
                view.QueryView?.TryExecuteQuery();
            }
        }
    }
}