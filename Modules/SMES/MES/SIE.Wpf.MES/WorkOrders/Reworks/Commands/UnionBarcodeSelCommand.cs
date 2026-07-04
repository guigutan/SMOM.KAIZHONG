using SIE.Domain.Validation;
using SIE.MES.WorkOrders.Reworks;
using SIE.MetaModel.View;
using SIE.Wpf.Command;
using System;
using System.Linq;

namespace SIE.Wpf.MES.WorkOrders.Reworks
{
    /// <summary>
    /// 工单关联条码选择
    /// </summary>
    [Command(ImageName = "PlaylistCheck", Label = "条码选择", ToolTip = "条码选择", GroupType = CommandGroupType.Business, DisplayMode = CommandDisplayMode.LabelAndIcon)]
    public class UnionBarcodeSelCommand : ListViewCommand
    {
        /// <summary>
        /// 判断条码关联是否能执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>工单不为空，返回true，否则返回false</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var vm = view.Relations.Find("mainView")?.Current as WorkOrderUnionBarcode;
            return vm != null && vm.WorkOrder != null;
        }

        /// <summary>
        /// 条码关联执行逻辑
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var template = new ListUITemplate(typeof(UnionBarcodeCore));
            var ui = template.CreateUI();
            ListLogicalView mainView = ui.MainView as ListLogicalView;
            CRT.Workbench.ShowDialog(ui, w =>
            {
                w.Title = "条码关联 选择".L10N();
                w.Closing += (s, e) =>
                {
                    if (w.Result == 0)
                    {
                        var barcodes = mainView.SelectedEntities.OfType<UnionBarcodeCore>().Select(p => p.Barcode).ToArray();
                        var vm = view.Relations.Find("mainView")?.Current as WorkOrderUnionBarcode;
                        if (vm != null)
                        {
                            var stringBuilder = vm.UnionBarcodes(barcodes);
                            if (stringBuilder.Length > 0)
                            {
                                e.Cancel = true;
                                throw new ValidationException(stringBuilder.ToString());
                            }
                        }
                    }
                };
            });
        }
    }
}