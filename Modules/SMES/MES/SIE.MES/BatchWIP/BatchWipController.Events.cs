using SIE.Common;
using SIE.Domain.Validation;
using SIE.MES.BatchWIP.Products;
using SIE.MES.WIP.Products;
using SIE.MES.WorkOrders;
using System;
using System.Linq;

namespace SIE.MES.WIP
{
    /// <summary>
    /// 批次采集事件
    /// </summary>
    public partial class WipController
    {
        /// <summary>
        /// 采集完成后，会发EventBus通知工单首件、上线、下线，采集完成
        /// </summary>
        /// <param name="data">CollectEventData</param>
        /// <param name="relation">批次关系</param>
        protected virtual void OnBatchCollected(CollectEventData data, BatchRelation relation)
        {
            if (data.Product.WorkOrder.State == Core.WorkOrders.WorkOrderState.Release)
            {
                RT.Service.Resolve<WorkOrderController>().StartProducing(data.Product.WorkOrderId);
                //// 首件投产通知
                RT.EventBus.Publish(new WipFirstArticleEvent(data));
                RT.EventBus.Publish(new WorkOrderProductingEvent() { WorkOrderId = data.Product.WorkOrderId });
            }

            if (data.CollectData.State == WipProductProcessState.Finish) //需要工序完成时才触发
            {
                ////添加工单报废数
                if (data.CollectData.ScrapQty > 0)
                {
                    OnBatchScraped(data);
                }

                bool isStart = false;
                bool isEnd = false;

                //工序组中的工序，是否开始和结束要以工序组为准
                if (!string.IsNullOrEmpty(data.Product.Routing.Current.GroupId))
                {
                    var groupId = data.Product.Routing.Current.GroupId;

                    var groupProcess = data.Product.Routing.Processes
                      .FirstOrDefault(x => x.GroupId == data.Product.Routing.Current.GroupId
                        && x.IsGroup == true);

                    if (groupProcess == null)
                    {
                        throw new ValidationException("找不到工序组【{0}】的信息"
                            .L10nFormat(data.Product.Routing.Current.GroupId));
                    }

                    //工序组是起始工序，且同工序组没有其他工序已过站
                    if (groupProcess.IsStart && !data.Product.Routing.Processes
                        .Any(x => x.GroupId == groupId && x.IsGroup != true
                            && x.PassNum > 0 && x.Id != data.Product.Routing.Current.Id))
                    {
                        isStart = true;
                    }

                    //工序组是结束工序，且同工序组其他工序都已经过站
                    if (groupProcess.IsEnd && !data.Product.Routing.Processes
                        .Any(x => x.GroupId == groupId && x.IsGroup != true
                            && x.PassNum <= 0 && x.Id != data.Product.Routing.Current.Id))
                    {
                        isEnd = true;
                    }
                }
                else
                {
                    isStart = data.Product.Routing.Current.IsStart;
                    isEnd = data.Product.Routing.Current.IsEnd;
                }

                //// 添加工单上线数量
                if (isStart && GetBatchWipProductProcess(relation.Pid).Count == 1)
                {
                    RT.Service.Resolve<WorkOrderController>().AddOnlineQty(data.Product.WorkOrderId, data.Product.Qty);
                    //// 上线数量变更通知
                    RT.EventBus.Publish(new WipOnlineEvent(data));
                }

                if (isEnd && data.CollectData.Result != ResultType.Fail)
                {
                    CompleteBatchProduct(data.Product, relation, data.Workcell);

                    // 下线更新产品完工数量 
                    RT.Service.Resolve<WorkOrderController>().AddFinishQty(data.Product.WorkOrderMove, relation.Qty);

                    // 下线工序通知                    
                    RT.EventBus.Publish(new WipFinishedEvent(data.Product, data.Barcodes[0].Code, data.CollectDate,
                        data.CollectData.OutputBatch));
                }
            }

            // 采集完成通知
            RT.EventBus.Publish(new WipCollectedEvent(data));
        }

        /// <summary>
        /// 产品报废
        /// </summary>
        /// <param name="data">采集数据</param>
        private void OnBatchScraped(CollectEventData data)
        {
            RT.Service.Resolve<WorkOrderController>().AddScrapQty(data.Product.WorkOrderId, data.CollectData.ScrapQty);
            RT.EventBus.Publish(new WipScrapedEvent(data));
        }
    }
}