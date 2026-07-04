using DevExpress.XtraCharts;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.MES.WorkOrders;
using SIE.LES;
using SIE.LES.Commons;
using SIE.LES.LinesideWarehouses;
using SIE.LES.StockOrders;
using SIE.LES.StockOrders.APIModels;
using SIE.LES.StockOrders.Models;
using SIE.LES.StockOrders.Service;
using SIE.LES.StockOrders.WorkOrders;
using SIE.Resources.WipResources;
using SIE.Web.LES.StockOrders.WorkOrders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.LES.StockOrders.DataQueryer
{
    /// <summary>
    /// 备料单查询器
    /// </summary>
    public class StockOrderQueryer : Data.DataQueryer
    {
        /// <summary>
        /// 获取物料需求明细数据
        /// </summary>
        /// <param name="bill">备料单</param>
        /// <returns>物料需求明细数据</returns>
        public virtual List<ItemRequireData> GetRequireItemData(StockOrder bill)
        {
            //当 拉式 且 需求计算方式=手工填写时，生产资源可不填，其余情况生产资源必填。
            if (bill.DemandMode != DemandMode.ManualFillIn
                && !bill.ResourceId.HasValue
                && bill.StockType != PrepareItemType.Pull)
            {
                throw new ValidationException("备料单的需求计算方式不是[手工填写],生产资源必须填写".L10N());
            }

            List<ItemRequireData> datas = new List<ItemRequireData>();
            if (bill.ResourceId.HasValue)
            {
                var lineWh = RT.Service.Resolve<LinesideWarehouseController>().GetLinesideWarehouse(bill.ResourceId.Value);

                //获取拉式备料维护
                if (bill.StockType == PrepareItemType.Pull)
                {
                    if (lineWh == null)
                    {
                        var wipResource = RF.GetById<WipResource>(bill.ResourceId);
                        string wipResourceName = bill.ResourceId.ToString();
                        if (wipResource != null)
                        {
                            wipResourceName = wipResource.Name;
                        }

                        throw new ValidationException("备料模式为【拉式】时，需要维护生产资源【{0}】的【产线线边仓】"
                            .L10nFormat(wipResourceName));
                    }

                    datas.AddRange(SetPullRequireData(bill, lineWh, lineWh.WarehouseId));
                }

                //获取推式备料维护
                if (bill.StockType == PrepareItemType.Push && bill.WorkOrderId.HasValue
                    && bill.WorkOrderId.Value > 0)
                {
                    datas.AddRange(SetPushRequireData(bill, lineWh));
                }
            }

            return datas;
        }

        /// <summary>
        /// 获取拉式备料需求数据
        /// </summary>
        /// <param name="bill">备料单</param>
        /// <param name="lineWh">线边仓</param>
        /// <param name="warehouseId">仓库ID</param>
        /// <returns>拉式备料需求数据</returns>
        private List<ItemRequireData> SetPullRequireData(StockOrder bill, LinesideWarehouse lineWh, double warehouseId)
        {
            return RT.Service.Resolve<PrepareItemController>().SetPullRequireData(bill, lineWh, warehouseId);
        }

        /// <summary>
        /// 获取推式备料需求数据
        /// </summary>
        /// <param name="bill">备料单</param>
        /// <param name="lineWh">线边仓</param>
        /// <returns>推式备料需求数据</returns>
        private List<ItemRequireData> SetPushRequireData(StockOrder bill, LinesideWarehouse lineWh)
        {
            return RT.Service.Resolve<PrepareItemPushController>().SetPushRequireData(bill, lineWh);
        }

        /// <summary>
        /// 获取备料单数据
        /// </summary>
        /// <param name="billId">备料单ID</param>
        /// <returns>备料单</returns>
        public virtual object GetStockOrder(double billId)
        {
            if (billId > 0)
            {
                var stockOrder = RF.GetById<StockOrder>(billId);
                return stockOrder.StockState;
            }

            return null;
        }

        /// <summary>
        /// 根据物料Id 工单ID 物料扩展属性获取工单需求总量
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="woId"></param>
        /// <param name="itemExtProp"></param>
        /// <returns></returns>
        public virtual Tuple<decimal, string, string> ChangeItemGetWorkTotalQty(double itemId, double woId, string itemExtProp)
        {
            var woLesDatas = RT.Service.Resolve<IWorkOrderQuery>().GetWoInfoForLes(null, null, new List<double>() { woId });
            if (woLesDatas.Any())
            {
                var firstwoLesDatas = woLesDatas.FirstOrDefault();
                if (firstwoLesDatas != null && firstwoLesDatas.WoBomInfos.Any())
                {
                    var woBomInfo = firstwoLesDatas.WoBomInfos.Find(m => m.ItemId == itemId);
                    if (woBomInfo != null)
                    {
                        if (itemExtProp != null)
                        {
                            woBomInfo = firstwoLesDatas.WoBomInfos.Find(m => m.ItemId == itemId && m.ItemExtProp == itemExtProp);
                        }
                        return new Tuple<decimal, string, string>(woBomInfo.RequestQty, woBomInfo.ItemExtProp, woBomInfo.ItemExtPropName);
                    }
                }
            }
            return new Tuple<decimal, string, string>(0, "", "");
        }

        /// <summary>
        /// 获取线边仓
        /// </summary>
        /// <param name="resouceId"></param>
        /// <returns></returns>
        public virtual LinesideWarehouse GetLineWaresHouse(double resouceId)
        {
            return RT.Service.Resolve<LinesideWarehouseController>().GetLinesideWarehouse(resouceId);
        }

        /// <summary>
        /// 备料单查询工单
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<StockOrderWoViewModel> GetStockOrderWoViewModels()
        {
            return RT.Service.Resolve<StockOrderService>().GetStockOrderWoViewModels(new StockOrderWoViewModelCriteria());
        }

        /// <summary>
        /// 备料单查询物料
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<StockOrderItemViewModel> GetStockOrderItemViewModels(StockOrderItemViewModelCriteria criteria)
        {
            return RT.Service.Resolve<StockOrderService>().GetStockOrderItemViewModels(criteria);
        }
    }
}