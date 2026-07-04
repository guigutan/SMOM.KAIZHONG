using SIE.Domain;
using SIE.ESop.Documents;
using SIE.MetaModel.View;
using SIE.Wpf.Command;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Wpf.ESOP.Documents.Commands
{
    /// <summary>
    /// 适用工单的选择命令
    /// </summary>
    [Command(ImageName = "PlaylistCheck", Label = "选择", ToolTip = "选择", GroupType = 10, DisplayMode = CommandDisplayMode.LabelAndIcon)]
    public class LookupWorkOrderCommand : LookupCommand
    {
        /// <summary>
        /// 命令是否能够执行
        /// </summary>
        /// <param name="view">逻辑视图</param>
        /// <returns>返回是否可执行</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return (view.Parent.Current != null) && view.Parent.Current.PersistenceStatus != PersistenceStatus.New;
        }

        /// <summary>
        /// 确定
        /// </summary>
        protected override void OnAccept()
        {
            FilterWorkOrder();
            base.OnAccept();
            var parentView = View.Parent;
            if (parentView != null && parentView.DataLoader.AnyLoaded)
                parentView.DataLoader.LoadDataAsync();
        }

        /// <summary>
        /// 过滤工单
        /// </summary>
        protected void FilterWorkOrder()
        {
            var collection = View.GetParentView(typeof(DocumentCollection)).Current as DocumentCollection;
            var workOrderList = View.Data.OfType<DocumentCollectionWorkOrder>().Select(p => p.WorkOrderId);
            var selectedEntities = SelectedView.Data.OfType<DocumentCollectionWorkOrder>();
            List<DocumentCollectionWorkOrder> collectionWorkOrders = new List<DocumentCollectionWorkOrder>();
            collectionWorkOrders.AddRange(selectedEntities);
            var workOrderIds = selectedEntities.Select(p => p.WorkOrderId).ToArray();
            var dicRefWorkOrders = RT.Service.Resolve<DocumentCollectionController>().IsWorkOrderRefCollection(collection.Id, workOrderIds);
            foreach (DocumentCollectionWorkOrder collectionWorkOrder in collectionWorkOrders)
            {
                if (workOrderList.Contains(collectionWorkOrder.WorkOrderId))   //已经选择过的过滤掉
                    continue;
                if (dicRefWorkOrders.ContainsKey(collectionWorkOrder.WorkOrderId) && !dicRefWorkOrders[collectionWorkOrder.WorkOrderId])
                    continue;
                if (!CRT.MessageService.AskQuestion("工单{0}已关联文档集,是否重新关联至此文档集".L10nFormat(collectionWorkOrder.WorkOrder.No)))
                {
                    var result = selectedEntities.FirstOrDefault(p => p.WorkOrderId == collectionWorkOrder.WorkOrderId);
                    SelectedView.Data.Remove(result);
                }
            }
        }
    }

    /// <summary>
    /// 适用工单的选择命令
    /// </summary>
    [Command(ImageName = "PlaylistCheck", Label = "选择", ToolTip = "选择", GroupType = 10, DisplayMode = CommandDisplayMode.LabelAndIcon)]
    public class DetailLookupWorkOrderCommand : LookupWorkOrderCommand
    {
        /// <summary>
        /// 确定
        /// </summary>
        protected override void OnAccept()
        {
            FilterWorkOrder();
            var selectedEntities = SelectedView.Data.OfType<DocumentCollectionWorkOrder>();
            var workOrderList = View.Data.OfType<DocumentCollectionWorkOrder>().Select(p => p.WorkOrderId);
            foreach (DocumentCollectionWorkOrder collectionWorkOrder in selectedEntities)
            {
                if (workOrderList.Contains(collectionWorkOrder.WorkOrderId))
                    continue;
                View.Data.Add(collectionWorkOrder);
            }
        }
    }
}
