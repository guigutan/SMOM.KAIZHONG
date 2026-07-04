using SIE.Common.Configs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Kit.EventMessages.UrgentOrder;
using SIE.Kit.UrgentOrder.ItemUrgentOrders;
using SIE.Kit.UrgentOrder.ItemUrgentOrders.Configs;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Kit.UrgentOrder.ItemUrgentOrders.Commands
{
    /// <summary>
    /// 物料加急单保存命令
    /// </summary>
    public class ItemUrgentOrderSaveCommand : SaveCommand
    {
        /// <summary>
        /// 保存前动作
        /// </summary>
        /// <param name="data">实体列表</param>
        protected override void OnSaving(EntityList data)
        {
            double Time = 24;

            var ItemUrgentOrderDate = ConfigService.GetConfig(new ItemUrgentOrderDateConfig(), typeof(ItemUrgentOrder));

            if (ItemUrgentOrderDate != null) {
                Time = ItemUrgentOrderDate.Time;
            }
              
            foreach (ItemUrgentOrder i in data)
            {
                if (i.PersistenceStatus == PersistenceStatus.New) {
                    DateTime max = DateTime.Now.AddHours(Time);

                    if (i.DemandTime > max)
                    {
                        throw new ValidationException("需求时间不能大于创建时间【" + Time + "】小时");
                    }
                }
                //收料
                if (i.IsReceive && (i.ReceiveQty == null || i.ReceiveQty <= 0))
                {
                    throw new ValidationException("收料看板数量必须大于0!");
                }
                //IQC检验
                if (i.IsInspectIqc && (i.InspectIqcQty == null || i.InspectIqcQty <= 0))
                {
                    throw new ValidationException("IQC检验看板数量必须大于0!");
                }
                //入库
                if (i.IsInstorage && (i.InstorageQty == null || i.InstorageQty <= 0))
                {
                    throw new ValidationException("入库看板数量必须大于0!");
                }
                //备料
                if (i.IsStockUp && (i.StockUpQty == null || i.StockUpQty <= 0))
                {
                    throw new ValidationException("备料看板数量必须大于0!");
                }
            }

            base.OnSaving(data);
        }

        /// <summary>
        /// 进行保存
        /// </summary>
        /// <param name="data">实体列表</param>
        protected override void DoSave(EntityList data)
        {
            var itemUrgentOrderList = data as EntityList<ItemUrgentOrder>;
            List<ItemUrgentOrderData> urgentOrderList = new List<ItemUrgentOrderData>();
            ItemUrgentOrderSendEvent itemUrgentOrderSendEvent = new ItemUrgentOrderSendEvent();
            var inReceiveList = itemUrgentOrderList.Where(p => p.IsNew && p.IsReceive).ToList();
            var iqcList = itemUrgentOrderList.Where(p => p.IsNew && p.IsInspectIqc).ToList();
            var instorageList = itemUrgentOrderList.Where(p => p.IsNew && p.IsInstorage).ToList();
            var stockUpList = itemUrgentOrderList.Where(p => p.IsNew && p.IsStockUp).ToList();

            if (inReceiveList.Count > 0)
            {
                inReceiveList.ForEach(p => urgentOrderList.Add(new ItemUrgentOrderData()
                {
                    ItemId = p.ItemId,
                    Qty = p.ReceiveQty.Value,
                    Type = UrgentOrderType.IsReceive,
                }));

                itemUrgentOrderSendEvent.ItemUrgentOrderDataList.AddRange(urgentOrderList);
            }

            if (iqcList.Count > 0)
            {
                iqcList.ForEach(p => urgentOrderList.Add(new ItemUrgentOrderData()
                {
                    ItemId = p.ItemId,
                    Qty = p.InspectIqcQty.Value,
                    Type = UrgentOrderType.IsInspectIqc,
                }));

                itemUrgentOrderSendEvent.ItemUrgentOrderDataList.AddRange(urgentOrderList);
            }

            if (instorageList.Count > 0)
            {
                instorageList.ForEach(p => urgentOrderList.Add(new ItemUrgentOrderData()
                {
                    ItemId = p.ItemId,
                    Qty = p.InstorageQty.Value,
                    Type = UrgentOrderType.IsInstorage,
                }));

                itemUrgentOrderSendEvent.ItemUrgentOrderDataList.AddRange(urgentOrderList);
            }

            if (stockUpList.Count > 0)
            {
                stockUpList.ForEach(p => urgentOrderList.Add(new ItemUrgentOrderData()
                {
                    ItemId = p.ItemId,
                    Qty = p.StockUpQty.Value,
                    Type = UrgentOrderType.IsStockUp,
                }));

                itemUrgentOrderSendEvent.ItemUrgentOrderDataList.AddRange(urgentOrderList);
            }

            RT.EventBus.Publish(itemUrgentOrderSendEvent);
            base.DoSave(data);
        }
    }

}
