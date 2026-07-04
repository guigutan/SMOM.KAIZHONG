using SIE.Common;
using SIE.MES.WIP.Reworks;
using SIE.MES.WorkOrders.Reworks;
using SIE.Wpf.Command;
using System.Linq;

namespace SIE.Wpf.MES.WorkOrders.Reworks
{
    /// <summary>
    /// 关联条码保存命令
    /// </summary>
    [Command(ImageName = "SaveEntity", Label = "保存")]
    public class UnionBarcodeSaveCommand : FormSaveCommand
    {
        /// <summary>
        /// 判断保存命令能否执行
        /// </summary>
        /// <param name="view">明细视图</param>
        /// <returns>存在未关联条码返回true，否则返回false</returns>
        public override bool CanExecute(DetailLogicalView view)
        {
            var vm = view.Current as WorkOrderUnionBarcode;
            return vm != null;
        }

        /// <summary>
        /// 命令执行
        /// </summary>
        /// <param name="view">表单逻辑视图</param>
        public override void Execute(DetailLogicalView view)
        {
            var viewModel = view.Data as WorkOrderUnionBarcode;
            var barcodeList = viewModel.BarcodeList.Where(b => b.CodeState == CodeState.NotAssociated).AsEntityList();
            RT.Service.Resolve<ReworkController>().UnionBarcode(viewModel.WorkOrder.Id, viewModel.KeyItemList, barcodeList);
            viewModel.RefreshUnionBarcodes();
            viewModel.RefreshKeyItems();
            viewModel.RefreshRelevancyQty();
            viewModel.NotifyPropertyChanged(WorkOrderUnionBarcode.ScanQtyProperty);
            viewModel.MarkSaved();
            CRT.MessageService.ShowMessage("保存成功!");
        }
    }
}