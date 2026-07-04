using SIE.Common;
using SIE.ProductIntfc.ProductStorages;
using SIE.Wpf.Command;
using System;
using System.Linq;

namespace SIE.Wpf.ProductIntfc.ProductStorages.Commands
{
    /// <summary>
    /// 入库命令
    /// </summary>
    [Command(Label = "入库", ToolTip = "入库", Gestures = "Ctrl+Shift+N", GroupType = 10)]
    public class ToStorageCommand : ListViewCommand
    {
        /// <summary>
        /// 执行入库
        /// </summary>
        /// <param name="view">数据视图</param>
        public override void Execute(ListLogicalView view)
        {
            var entityList = view.SelectedEntities.OfType<ToStorageBarcode>().AsEntityList();
            RT.Service.Resolve<ProductStorageController>().ToStorageIn(entityList);
            UpdateBarcodeList(view);
            CRT.MessageService.ShowMessage("入库成功".L10N(), "提示".L10N());
        }

        /// <summary>
        /// 更新条件列表
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        private void UpdateBarcodeList(ListLogicalView view)
        {
            var storageWorkOrder = view.Parent.Current as StorageWorkOrder;
            //更新未入库条码
            var barcodes = RT.Service.Resolve<ProductStorageController>().GetToStoreBarcode(storageWorkOrder.Id, false);
            barcodes ??= new Domain.EntityList<ToStorageBarcode>();
            view.Data = barcodes;
            //更新已入库条码
            var pview = view.Parent as ListLogicalView;
            if (pview.GetChildView(typeof(InStorageBill)) is ListLogicalView InStorageBillView)
            {
                var inbarcodes = RT.Service.Resolve<ProductStorageController>().GetInStoreBarcode(storageWorkOrder.Id);
                if (inbarcodes == null)
                    inbarcodes = new Domain.EntityList<InStorageBill>();
                InStorageBillView.Data = inbarcodes;
            }

            //完全入库时，刷新主表
            if (barcodes.Count == 0)
                view.Parent.QueryView?.TryExecuteQuery();
        }

        /// <summary>
        /// 判断入库命令能否执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>能执行返回true，否则返回false</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return view.SelectedEntities.Count > 0;
        }
    }
}