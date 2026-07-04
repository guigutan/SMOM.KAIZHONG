using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Kit.EventMessages.UrgentOrder;
using SIE.Kit.UrgentOrder.ItemUrgentOrders.Configs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Kit.UrgentOrder.ItemUrgentOrders
{
    /// <summary>
    /// 物料加急单控制器
    /// </summary>
    public partial class ItemUrgentOrderController : DomainController
    {
        /// <summary>
        /// 更新物料加急单
        /// </summary>
        /// <param name="itemUrgentOrderDatas">物料加急单数据</param>
        public virtual void UpdateItemUrgentOrder(ItemUrgentOrderReceiveEvent itemUrgentOrderReceiveEvent)
        {
            try
            {
                var itemUrgentOrderDatas = itemUrgentOrderReceiveEvent.ItemUrgentOrderDataList;
                if (itemUrgentOrderDatas != null && itemUrgentOrderDatas.Count > 0)
                {
                    using (var tran = DB.TransactionScope(UrgentOrderEntityDataProvider.ConnectionStringName))
                    {
                        itemUrgentOrderDatas.ForEach(data =>
                        {
                            var oriUrgentOrderList = Query<ItemUrgentOrder>().Where(p => p.ItemId == data.ItemId && p.IsInspectIqc == true).OrderBy(p => p.DemandTime).ToList();
                            foreach (var oriUrgentOrder in oriUrgentOrderList)
                            {
                                if (oriUrgentOrder.InspectIqcQty < data.Qty)
                                {
                                    data.Qty -= oriUrgentOrder.InspectIqcQty.Value;
                                    oriUrgentOrder.IsInspectIqc = false;
                                    oriUrgentOrder.InspectIqcState = UrgentOrderState.InValid;
                                    oriUrgentOrder.InspectIqcQty = null;
                                    RF.Save(oriUrgentOrder);
                                    continue;
                                }
                                if (oriUrgentOrder.InspectIqcQty > data.Qty)
                                {
                                    oriUrgentOrder.InspectIqcQty -= data.Qty;
                                    RF.Save(oriUrgentOrder);
                                    break;
                                }
                                if (oriUrgentOrder.InspectIqcQty == data.Qty)
                                {
                                    oriUrgentOrder.IsInspectIqc = false;
                                    oriUrgentOrder.InspectIqcState = UrgentOrderState.InValid;
                                    oriUrgentOrder.InspectIqcQty = null;
                                    RF.Save(oriUrgentOrder);
                                    break;
                                }
                            }
                        });

                        tran.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ValidationException(ex.Message.L10N());
            }
        }

        /// <summary>
        /// 通过查询条件获取物料加急列表
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>物料加急列表</returns>
        public virtual EntityList<ItemUrgentOrder> GetItemUrgentOrderList(ItemUrgentOrderCriteria criteria)
        {
            var query = Query<ItemUrgentOrder>();
            if (criteria.No.IsNotEmpty())
            {
                query.Where(p => p.No.Contains(criteria.No));
            }

            if (criteria.OrderState.HasValue)
            {
                query.Where(p => p.OrderState == criteria.OrderState);
            }

            if (criteria.CreateDate.BeginValue.HasValue)
            {
                query.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue);
            }

            if (criteria.CreateDate.EndValue.HasValue)
            {
                query.Where(p => p.CreateDate <= criteria.CreateDate.EndValue);
            }

            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取物料加急单的No
        /// </summary>
        /// <returns>物料加急单No字符串</returns>
        public virtual string GetItemUrgentOrderNo()
        {
            var config = ConfigService.GetConfig(new ItemUrgentOrderNoConfig(), typeof(ItemUrgentOrder));
            if (config == null || config.NumberRule == null)
            {
                throw new ValidationException("未找到物料加急单号配置规则，请检查规则配置".L10N());
            }

            return RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.NumberRule.Id, 1).FirstOrDefault();
        }

        /// <summary>
        /// 获取排程管理配置项
        /// </summary>
        public virtual double GetItemUrgentOrderDateConfig()
        {
            var config = ConfigService.GetConfig(new ItemUrgentOrderDateConfig(), typeof(ItemUrgentOrder));
            if (config == null)
                throw new ValidationException("物料加急单时间配置项信息未配置".L10N());
            return config.Time;
        }

        #region WMS收货更新数据

        /// <summary>
        /// 获取加急物料数据
        /// </summary>
        /// <param name="itemIds">物料Id</param>         
        /// <returns></returns>
        public virtual EntityList<ItemUrgentOrder> GetItemUrgentOrdersForWMS(List<double> itemIds)
        {
            var query = Query<ItemUrgentOrder>().Where(p => itemIds.Contains(p.ItemId) && p.IsReceive == true && p.ReceiveState == UrgentOrderState.Valid);
            return query.ToList();
        }

        /// <summary>
        /// 获取加急物料
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<ItemUrgentOrder> GetItemUrgentOrdersForWMS()
        {
            var query = Query<ItemUrgentOrder>().Where(p => p.IsReceive == true && p.ReceiveState == UrgentOrderState.Valid);
            return query.ToList();
        }

        /// <summary>
        /// 更新加急物料数据
        /// </summary>
        /// <param name="datas">WMS收货取消收货传入</param>
        /// <param name="isCancel">是否取消收货</param>
        public virtual void UpdateItemUrgentOrdersFromWMS(List<ItemUrgentOrderData> datas, bool isCancel = false)
        {
            var itemIds = datas.Select(p => p.ItemId).ToList();
            if (itemIds.Count == 0) return;
            var urgentDatas = GetItemUrgentOrdersForWMS(itemIds);
            if (urgentDatas.Count == 0) return;
            urgentDatas.ForEach(p =>
            {
                var qty = datas.FirstOrDefault(f => f.ItemId == p.ItemId).Qty;
                if (!p.ReceiveQty.HasValue)
                    p.ReceiveQty = 0;
                if (isCancel)
                {
                    p.ReceiveQty += qty;
                    p.ReceiveState = UrgentOrderState.Valid;
                }
                else
                {
                    p.ReceiveQty -= qty;

                    if (p.ReceiveQty <= 0)
                    {
                        p.ReceiveQty = 0;
                        p.ReceiveState = UrgentOrderState.InValid;
                    }
                }
            });
            RF.Save(urgentDatas);
        }
        #endregion
    }
}
