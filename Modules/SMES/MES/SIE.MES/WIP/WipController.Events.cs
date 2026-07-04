using SIE.Common;
using SIE.Domain;
using SIE.EventMessages.MES.PanelBindings;
using SIE.MES.PanelBindings;
using SIE.MES.WIP.Products;
using SIE.MES.WIP.Runtime;
using SIE.MES.WorkOrders;
using System.Linq;

namespace SIE.MES.WIP
{
    /// <summary>
    /// WIP控制器 Work In Process 在制品
    /// </summary>
    partial class WipController
    {
        /// <summary>
        /// 采集完成后，会发EventBus通知工单首件、上线、下线，采集完成
        /// </summary>
        /// <param name="data">CollectEventData</param>
        /// <param name="version">生产采集版本</param>
        protected virtual void OnCollected(CollectEventData data, WipProductVersion version)
        {
            if (data.Product.WorkOrderMove.State == Core.WorkOrders.WorkOrderState.Release)
            {
                RT.Service.Resolve<WorkOrderController>().StartProducing(data.Product.WorkOrderId);

                //更新子工单为上线时，将所属的组合板工单也更新为“生产中”
                if (data.Product.WorkOrderMove.PanelWorkOrderId.HasValue)
                {
                    RT.Service.Resolve<WorkOrderController>()
                        .StartProducing(data.Product.WorkOrderMove.PanelWorkOrderId.Value);
                }

                //// 首件投产通知
                RT.EventBus.Publish(new WipFirstArticleEvent(data));
                RT.EventBus.Publish(new WorkOrderProductingEvent() { WorkOrderId = data.Product.WorkOrderId });
            }
            //需要工序完成时才触发
            if (data.CollectData.State != WipProductProcessState.Finish)
            {
                return;
            }
            var current = data.Product.Routing.Current;
            if (current.IsStart && RT.Service.Resolve<WipProductVersionController>().GetWipProductProcessCountBySn(data.Barcodes[0].Code, data.Barcodes[0].Type) == 1)
            {
                RT.Service.Resolve<WorkOrderController>()
                    .AddOnlineQty(data.Product.WorkOrderId, data.Product.Qty);

                var workOrder = data.Product.WorkOrderMove;
                if (workOrder.IsPanelWorkOrder)
                {
                    var barcode = data.Barcodes[0].Code;
                    var panelAndBarcodes = RT.Service.Resolve<PanelBindingController>().GetPanelAndBarcodesByPanleCode(barcode);

                    if (panelAndBarcodes.Any())
                    {
                        //存在条码绑定记录，则按实际单体条码数量更新
                        var panelAndBarcodesDictionary = panelAndBarcodes
                            .Where(x => x.ChildWorkOrderId.HasValue)
                            .GroupBy(x => x.ChildWorkOrderId.Value)
                            .ToDictionary(x => x.Key, y => y.ToList());

                        foreach (var childWorkOrderId in panelAndBarcodesDictionary.Keys)
                        {
                            var qty = panelAndBarcodesDictionary[childWorkOrderId].Sum(x => x.Qty);
                            RT.Service.Resolve<WorkOrderController>()
                                    .AddOnlineQty(childWorkOrderId, qty);
                        }
                    }
                    else
                    {
                        //取PCB物料属性明细
                        var pcbItemDetails = RT.Service.Resolve<IPanelBinding>()
                            .GetPcbItemDetailInfos(workOrder.ProductId);
                        //取组合板的子工单列表
                        var childWorkOrderList = RT.Service.Resolve<WorkOrderController>()
                            .GetWorkOrdersByPanelWorkId(workOrder.Id);

                        //更新组合板工单完工数量时，同步更新子工单的完工数量。
                        foreach (var pcbItemDetail in pcbItemDetails)
                        {
                            var childWorkOrdersOfItem = childWorkOrderList
                                .Where(x => x.ProductId == pcbItemDetail.ItemId)
                                .ToList();

                            decimal qty = pcbItemDetail.Qty;

                            foreach (var childWorkOrder in childWorkOrdersOfItem)
                            {
                                if (qty <= 0)
                                {
                                    break;
                                }

                                if (childWorkOrder.OnlineQty >= childWorkOrder.PlanQty)
                                {
                                    continue;
                                }

                                //该工单剩余可上料数量 
                                var maxOnlineQty = childWorkOrder.PlanQty - childWorkOrder.OnlineQty;

                                var currentOnlineQty = qty;
                                if (maxOnlineQty < currentOnlineQty)
                                {
                                    currentOnlineQty = maxOnlineQty;
                                }

                                qty -= currentOnlineQty;

                                RT.Service.Resolve<WorkOrderController>()
                                    .AddOnlineQty(childWorkOrder.Id, currentOnlineQty);
                            }
                        }
                    }
                }

                //组合板的子工单的条码上线时，更新子工单的上线数量
                ////var panelWorkOrderId = workOrder.PanelWorkOrderId;
                ////if (panelWorkOrderId.HasValue)
                ////{
                ////    var panelWorkOrder = RF.GetById<WorkOrder>(panelWorkOrderId.Value);
                ////    var pcbItemDetails = RT.Service.Resolve<IPanelBinding>().GetPcbItemDetailInfos(panelWorkOrder.ProductId);
                ////    var dicItemDetails = pcbItemDetails.ToDictionary(p => p.ItemId);

                ////    var childWorkOrderList = RT.Service.Resolve<WorkOrderController>()
                ////        .GetWorkOrdersByPanelWorkId(panelWorkOrderId.Value);

                ////    decimal maxChildOnlineQty = 0;
                ////    foreach (var pcbItemDetail in pcbItemDetails)
                ////    {
                ////        var onlineQty = childWorkOrderList
                ////            .Where(x => x.ProductId == pcbItemDetail.ItemId)
                ////            .Sum(x => x.OnlineQty);
                ////        var childOnlineQty = onlineQty / pcbItemDetail.Qty;
                ////        if (childOnlineQty > maxChildOnlineQty)
                ////        {
                ////            maxChildOnlineQty = childOnlineQty;
                ////        }
                ////    }

                ////    DB.Update<WorkOrder>()
                ////        .Set(p => p.OnlineQty, maxChildOnlineQty)
                ////        .Where(p => p.Id == panelWorkOrderId.Value)
                ////        .Execute();
                ////}

                //// 上线数量变更通知
                RT.EventBus.Publish(new WipOnlineEvent(data));
            }

            if (version != null && version.IsFinish)
            {
                CompleteProduct(data.Product, data.Barcodes[0].Code);
                RT.EventBus.Publish(new WipFinishedEvent(data.Product, data.Barcodes[0].Code, data.CollectDate,
                    data.CollectData.OutputBatch));
            }

            // 采集完成通知
            RT.EventBus.Publish(new WipCollectedEvent(data));
        }

        /// <summary>
        /// 产品完工处理
        /// </summary>
        /// <param name="product"></param>
        /// <param name="sn"></param>        
        public virtual void CompleteProduct(product product, string sn)
        {
            //// 下线完成产品生产，CompleteProduct效率比较低，移到move方法处理
            ////CompleteProduct(data.Product, data.CollectData.CollectBarcode);
            RT.Service.Resolve<WorkOrderController>().AddFinishQty(product.WorkOrderMove,
                product.Qty - product.NgQty);

            var workOrder = product.WorkOrderMove;
            if (workOrder.IsPanelWorkOrder)
            {
                var barcode = sn;
                var panelAndBarcodes = RT.Service.Resolve<PanelBindingController>().GetPanelAndBarcodesByPanleCode(barcode);

                if (panelAndBarcodes.Any())
                {
                    //存在条码绑定记录，则按实际单体条码数量更新
                    var panelAndBarcodesDictionary = panelAndBarcodes
                        .Where(x => x.ChildWorkOrderId.HasValue)
                        .GroupBy(x => x.ChildWorkOrderId.Value)
                        .ToDictionary(x => x.Key, y => y.ToList());

                    foreach (var childWorkOrderId in panelAndBarcodesDictionary.Keys)
                    {
                        var qty = panelAndBarcodesDictionary[childWorkOrderId].Sum(x => x.Qty);
                        var childWorkOrder = RF.GetById<WorkOrderMove>(childWorkOrderId);
                        RT.Service.Resolve<WorkOrderController>()
                                .AddFinishQty(childWorkOrder, qty);
                    }
                }
                else
                {
                    //取PCB物料属性明细
                    var pcbItemDetails = RT.Service.Resolve<IPanelBinding>()
                        .GetPcbItemDetailInfos(workOrder.ProductId);

                    //取组合板的子工单列表
                    var childWorkOrderList = RT.Service.Resolve<WorkOrderController>()
                        .GetWorkOrderMovesByPanelWorkId(workOrder.Id);

                    //更新组合板工单完工数量时，同步更新子工单的完工数量。
                    foreach (var pcbItemDetail in pcbItemDetails)
                    {
                        var childWorkOrdersOfItem = childWorkOrderList
                            .Where(x => x.ProductId == pcbItemDetail.ItemId)
                            .ToList();

                        decimal qty = pcbItemDetail.Qty;

                        foreach (var childWorkOrder in childWorkOrdersOfItem)
                        {
                            if (qty <= 0)
                            {
                                break;
                            }

                            if (childWorkOrder.ScrapQty + childWorkOrder.FinishQty >= childWorkOrder.PlanQty)
                            {
                                continue;
                            }

                            var maxCanFinishQty = childWorkOrder.PlanQty - (childWorkOrder.ScrapQty + childWorkOrder.FinishQty);
                            var currentFinishQty = qty;
                            if (maxCanFinishQty < currentFinishQty)
                            {
                                currentFinishQty = maxCanFinishQty;
                            }

                            qty -= currentFinishQty;

                            RT.Service.Resolve<WorkOrderController>().AddFinishQty(childWorkOrder,
                                currentFinishQty);
                        }
                    }
                }
            }

            //更新子工单完工数量时，将所属的组合板工单也更新
            var panelWorkOrderId = workOrder.PanelWorkOrderId;
            if (panelWorkOrderId.HasValue)
            {
                var panelWorkOrder = RF.GetById<WorkOrder>(panelWorkOrderId.Value);

                //取PCB物料属性明细
                var pcbItemDetails = RT.Service.Resolve<IPanelBinding>().GetPcbItemDetailInfos(panelWorkOrder.ProductId);

                //取组合板的子工单列表
                var childWorkOrderList = RT.Service.Resolve<WorkOrderController>()
                    .GetWorkOrdersByPanelWorkId(panelWorkOrderId.Value);

                decimal minChildFinishQty = decimal.MaxValue;
                foreach (var pcbItemDetail in pcbItemDetails)
                {
                    var finishQty = childWorkOrderList
                        .Where(x => x.ProductId == pcbItemDetail.ItemId)
                        .Sum(x => x.FinishQty);

                    var childFinishQty = finishQty / pcbItemDetail.Qty;

                    if (childFinishQty < minChildFinishQty)
                    {
                        minChildFinishQty = childFinishQty;
                    }
                }

                DB.Update<WorkOrder>()
                    .Set(p => p.FinishQty, minChildFinishQty)
                    .Where(p => p.Id == panelWorkOrderId.Value)
                    .Execute();
            }

            //// 下线工序通知
        }

        /// <summary>
        /// 采集开始
        /// </summary>
        /// <param name="data">CollectEventData</param>
        protected virtual void OnCollecting(CollectEventData data)
        {
            RT.EventBus.Publish(new WipCollectingEvent(data));
        }

        /// <summary>
        /// 报废事件
        /// </summary>
        /// <param name="data">CollectEventData</param>
        protected virtual void OnScraped(CollectEventData data)
        {
            RT.Service.Resolve<WorkOrderController>().AddScrapQty(data.Product.WorkOrderId, data.Product.NgQty);
            RT.EventBus.Publish(new WipScrapedEvent(data));
        }

        /// <summary>
        /// 产线在产工单切换时间
        /// </summary>
        /// <param name="workOrder">工单对象</param>
        protected virtual void OnChangeWipResourceWorkOrder(WorkOrder workOrder)
        {
            RT.RemotingEventBus.Publish(new ChangeWipResourceWorkOrderEvent { WorkOrderId = workOrder.Id });
        }
    }
}
