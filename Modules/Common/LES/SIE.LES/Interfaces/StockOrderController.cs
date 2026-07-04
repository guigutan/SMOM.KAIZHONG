using SIE.Core.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.LES;
using SIE.EventMessages.MES.WorkOrders;
using SIE.EventMessages.MES.WorkOrders.Models;
using SIE.EventMessages.WMS.Inventory;
using SIE.EventMessages.WMS.Shipment;
using SIE.Inventory.Commom;
using SIE.Items;
using SIE.LES.Interfaces.Datas;
using SIE.LES.StockOrders;
using SIE.LES.StockOrders.Service;
using SIE.LES.StockPlans;
using SIE.Resources.Employees;
using SIE.Warehouses;
using SIE.Warehouses.ItemStockData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.LES.Interfaces
{
    /// <summary>
    /// 备料单接口控制器
    /// </summary>
    public class StockOrderController : DomainController, ISoUpdateStock
    {
        /// <summary>
        /// 退料更新备料单信息
        /// </summary>
        /// <param name="returnSnDatas">退料数据</param>      
        /// <param name="sourceBillNo">来源单号</param>
        public virtual void ReturnSnUpdate(List<ReturnSnData> returnSnDatas, string sourceBillNo = "", string soRemark = "工单退料")
        {
            using (var tran = DB.TransactionScope(LESEntityDataProvider.ConnectionStringName))
            {
                returnSnDatas.Where(f => f.LotCode.IsNullOrEmpty()).ForEach(f => f.LotCode = Lot.LotDefault);
                var itemIds = returnSnDatas.Select(f => f.ItemId).Distinct().ToList();
                var itemStocks = RT.Service.Resolve<ItemStockBaseController>().GetItemStockDataBases(itemIds);
                var serItemIds = itemStocks.Where(a => a.IsSerialNumber == true).Select(f => f.ItemId).ToList();
                if (serItemIds.Any())
                {//序列号物料处理
                    returnSnDatas.Where(a => serItemIds.Contains(a.ItemId)).ForEach(a => a.IsSerialNumber = true);
                    ReturnCreateSoAndAsn(sourceBillNo, returnSnDatas, soRemark);
                }
                var notSerItemIds = itemStocks.Where(a => a.IsSerialNumber != true).Select(f => f.ItemId).ToList();
                if (notSerItemIds.Any())
                {//非序列号物料处理
                    var items = RT.Service.Resolve<ItemController>().GetItemList(notSerItemIds);
                    var labelItemIds = items.Where(f => f.IsLabel == true).Select(a => a.Id).ToList();
                    var sns = returnSnDatas.Where(a => labelItemIds.Contains(a.ItemId)).Select(f => f.Sn).ToList();
                    if (sns.Any())
                    {//标签管理物料                       
                        ReturnCreateSoAndAsn(sourceBillNo, returnSnDatas, soRemark);
                    }
                    var noLabelItemIds = items.Where(f => f.IsLabel != true).Select(a => a.Id).ToList();
                    var noLabelButBatchItemIds = itemStocks.Where(a => a.IsBatch == true && a.IsSerialNumber != true && noLabelItemIds.Contains(a.ItemId)).Select(a => a.ItemId).ToList();
                    var lots = returnSnDatas.Where(a => noLabelButBatchItemIds.Contains(a.ItemId)).Select(f => f.LotCode).ToList();
                    if (lots.Any())
                    { //无标签管理物料，但是批次管理                                              
                        ReturnCreateSoAndAsn(sourceBillNo, returnSnDatas, soRemark);
                    }
                    var noLabelnoBatchItemIds = itemStocks.Where(a => a.IsBatch != true && a.IsSerialNumber != true && noLabelItemIds.Contains(a.ItemId)).Select(a => a.ItemId).ToList();
                    var curItemIds = returnSnDatas.Where(a => noLabelnoBatchItemIds.Contains(a.ItemId)).Select(f => f.ItemId).ToList();
                    if (curItemIds.Any())
                    {
                        ReturnCreateSoAndAsn(sourceBillNo, returnSnDatas, soRemark);
                    }
                }

                tran.Complete();
            }
        }

        /// <summary>
        /// 退料创建发货收货单
        /// </summary>        
        private void ReturnCreateSoAndAsn(string sourceBillNo, List<ReturnSnData> returnSnDatas, string remark)
        {
            var whs = RT.Service.Resolve<WarehouseController>().GetWarehouse();
            List<MesMoveCreateSoData> mesMoves = new List<MesMoveCreateSoData>();
            List<ReturnLabelData> returnLabels = new List<ReturnLabelData>();
            returnSnDatas.GroupBy(p => new { p.WoNo, p.WorkShopId }).ForEach(p =>
            {
                p.GroupBy(f => new { f.SourceWarehouseId, f.SourceWarehouseCode }).ForEach(f =>
                        {
                            var wh = whs.FirstOrDefault(a => a.Id == f.Key.SourceWarehouseId);
                            if (wh != null && wh.Code == f.Key.SourceWarehouseCode)
                            {
                                MesMoveCreateSoData mesMoveCreateSoData = new MesMoveCreateSoData()
                                {
                                    WorkShopId = p.Key.WorkShopId,
                                    BillNo = p.Key.WoNo,
                                    MesLabelDatas = new List<MesLabelData>(),
                                    Remark = remark,
                                };
                                f.ForEach(a =>
                                {
                                    MesLabelData label = new MesLabelData()
                                    {
                                        LabelNo = a.Sn,
                                        LineNo = "",
                                        StorageLocationId = a.SourceStorageLocationId.Value,
                                        WarehouseId = a.SourceWarehouseId.Value,
                                        WarehouseCode = a.SourceWarehouseCode,
                                        ItemExtProp = a.ItemExtProp,
                                        LotCode = a.LotCode,
                                        ItemExtPropName = a.ItemExtPropName,
                                        ItemId = a.ItemId,
                                        IsFail = a.IsFail,
                                        Qty = a.Qty,
                                    };
                                    mesMoveCreateSoData.MesLabelDatas.Add(label);
                                });
                                mesMoves.Add(mesMoveCreateSoData);
                                RT.Service.Resolve<ICreateSo>().CreateShippingOrderByLes(mesMoveCreateSoData);
                            }

                            f.ForEach(a =>
                            {
                                ReturnLabelData labelData = new ReturnLabelData()
                                {
                                    EnterprisesId = p.Key.WorkShopId,
                                    IsFail = a.IsFail,
                                    ItemId = a.ItemId,
                                    Sn = a.Sn,
                                    StorageLocationId = a.SourceStorageLocationId,
                                    WarehouseId = f.Key.SourceWarehouseId,
                                    Qty = a.Qty,
                                    IsSerialNumber = a.IsSerialNumber,
                                    LotCode = a.LotCode,
                                    ItemExtProp = a.ItemExtProp,
                                    ItemExtPropName = a.ItemExtPropName,
                                };
                                returnLabels.Add(labelData);
                            });
                        });
            });
            var emp = RT.Service.Resolve<EmployeeController>().GetEmployeeById(RT.IdentityId);
            MesReturnItemData mesReturn = new MesReturnItemData()
            {
                EmpCode = emp.Code,
                SourceBillNo = sourceBillNo,
                returnLabelList = returnLabels
            };
            //接口已有扣减线边仓库存逻辑
            RT.Service.Resolve<IMesReturnItem>().UpdateInventoryByReturnItem(mesReturn);
        }

        /// <summary>
        /// 工单挪料
        /// </summary>
        /// <remarks>业务：线边仓1退到仓库再退到线边仓2</remarks>
        /// <param name="moveSnData"></param>
        public virtual void WoMoveSn(MoveSnData moveSnData)
        {
            using (var tran = DB.TransactionScope(LESEntityDataProvider.ConnectionStringName))
            {
                if (moveSnData.TargetStorageLocationId == null || moveSnData.TargetWarehouseId == null)
                    throw new ValidationException("没有目标仓库、库位，不能进行挪料".L10N());
                if (moveSnData.ReturnSnDatas.Any(f => f.SourceStorageLocationId == null || f.SourceWarehouseId == null))
                    throw new ValidationException("没有来源仓库、库位，不能进行挪料".L10N());
                var sns = moveSnData.ReturnSnDatas.Select(f => f.Sn).ToList();
                moveSnData.ReturnSnDatas.Where(f => f.LotCode.IsNullOrEmpty()).ForEach(f => f.LotCode = "LotDefault");
                var stockSns = RT.Service.Resolve<StockOrderSnService>().GetOrderReturnSnBySn(sns);
                var dtlIds = stockSns.Select(a => a.StockOrderDetailId).Distinct().ToList();
                var dtls = RT.Service.Resolve<StockOrderDetailService>().GetStockOrderDetails(dtlIds);
                //创建备料单
                CreateStockOrder(moveSnData, dtls, stockSns);
                tran.Complete();
            }
        }

        /// <summary>
        /// 设置备料单头数据
        /// </summary>       
        private StockOrder SetStockOrder(MoveSnData moveSnData, EntityList<StockOrderDetail> dtls, double factoryId)
        {
            var targetWo = RT.Service.Resolve<IWorkOrderQuery>().GetWorkOrderResource(moveSnData.TargetWoId);
            var oldStock = dtls.FirstOrDefault().StockOrder;
            StockOrder stockOrder = new StockOrder()
            {
                No = RT.Service.Resolve<StockOrderService>().GetStockOrderNo(),
                WorkOrderId = targetWo.WorkOrderId,
                ResourceId = targetWo.ResourceId,
                WorkShopId = targetWo.WorkShopId,
                FactoryId = factoryId,
                BillSource = BillSource.Automatic,
                DemandMode = oldStock.DemandMode,
                StockState = StockState.Received,
                StockType = oldStock.StockType,
                TriggerMode = oldStock.TriggerMode,
            };
            stockOrder.GenerateId();
            return stockOrder;
        }

        /// <summary>
        /// 创建备料单
        /// </summary>
        /// <param name="moveSnData">目标工单</param>
        /// <param name="dtls">需求明细</param>
        /// <param name="stockSns">接收记录</param>
        private void CreateStockOrder(MoveSnData moveSnData, EntityList<StockOrderDetail> dtls, EntityList<StockOrderSn> stockSns)
        {
            var bomList = new List<WoBomInfoForLes>();

            if (moveSnData.TargetWoId > 0)
            {
                var woLesDatas = RT.Service.Resolve<IWorkOrderQuery>().GetWoInfoForLes(null, null, new List<double>() { moveSnData.TargetWoId });
                if (!woLesDatas.Any())
                {
                    throw new ValidationException("创建备料单的工单无法找到数据".L10N());
                }
                bomList = woLesDatas.FirstOrDefault()?.WoBomInfos;
                if (woLesDatas.First().FactoryId == null)
                    throw new ValidationException("创建备料单的工厂不能为空".L10N());
            }
            EntityList<StockOrderSn> stockOrderSns = new EntityList<StockOrderSn>();
            var stockOrder = SetStockOrder(moveSnData, dtls, moveSnData.FactoryId);
            int lineNo = 1;
            var returnSns = moveSnData.ReturnSnDatas;
            var whIds = returnSns.Select(f => f.SourceWarehouseId.Value).Distinct().ToList();
            whIds.Add(moveSnData.TargetWarehouseId.Value);
            var whs = RT.Service.Resolve<WarehouseController>().GetWarehouses(whIds);
            MesMoveCreateSoData mesMoveCreateSoData = new MesMoveCreateSoData()
            {
                WorkShopId = stockOrder.WorkShopId,
                BillNo = stockOrder.No,
                MesLabelDatas = new List<MesLabelData>(),
                Remark = "工单挪料",
                IsUpdateOnhand = true,
            };
            var sourceWoNos = moveSnData.ReturnSnDatas.Select(f => f.WoNo).Distinct().ToList();
            if (!stockSns.Any(f => sourceWoNos.Contains(f.WoNo)))
            {
                throw new ValidationException("找不到原来的备料单接收记录[工单{0}]".L10nFormat(sourceWoNos.First()));
            }
            //只要来源工单的接收记录
            stockSns.Where(f => sourceWoNos.Contains(f.WoNo)).ForEach(p =>
            {//匹配来源工单的接收记录，工单+接收记录条码号
                if (!moveSnData.ReturnSnDatas.Any(a => a.WoNo == p.WoNo && p.Sn == a.Sn))
                    stockSns.Remove(p);
            });

            int snLineNo = 1;

            stockSns.Where(f => sourceWoNos.Contains(f.WoNo)).GroupBy(p => p.StockOrderDetailId).ForEach(p =>
              {

                  var dtl = dtls.FirstOrDefault(a => a.Id == p.Key);
                  var bom = bomList.FirstOrDefault(f => f.ItemId == dtl.ItemId);
                  var curLabels = p.Select(a => a.Sn).ToList();
                  var moveQty = returnSns.Where(a => curLabels.Contains(a.Sn) && !a.IsUse).Sum(a => a.Qty);

                  StockOrderDetail stockOrderDetail = new StockOrderDetail()
                  {
                      ItemId = dtl.ItemId,
                      DemandTime = DateTime.Now,
                      Qty = moveQty,
                      StockState = StockState.Received,
                      LineNo = lineNo.ToString(),
                      WoTotalQty = bom?.RequestQty ?? 0,
                      ReceiveQty = moveQty,
                      ShipQty = moveQty,
                      ItemExtProp = dtl.ItemExtProp,
                      ItemExtPropName = dtl.ItemExtPropName,
                      StorageLocationId = moveSnData.TargetStorageLocationId,
                      WarehouseId = moveSnData.TargetWarehouseId,
                  };
                  stockOrderDetail.GenerateId();

                  stockOrder.StockOrderDetailList.Add(stockOrderDetail);
                  p.ForEach(f =>
                  {   //找到同一个工单的的标签，而且没有被使用过的
                      var returnSn = returnSns.FirstOrDefault(a => a.Sn == f.Sn && a.WoNo == f.WoNo && !a.IsUse);
                      if (returnSn == null)
                          return;
                      returnSn.IsUse = true;
                      stockOrderSns.Add(GetStockOrderSn(f, returnSn.Qty, snLineNo, stockOrder.Id, stockOrderDetail.Id));
                      snLineNo++;
                      var wh = whs.FirstOrDefault(a => a.Id == returnSn.SourceWarehouseId.Value);
                      mesMoveCreateSoData.MesLabelDatas.Add(new MesLabelData()
                      {
                          LabelNo = f.Sn,
                          LineNo = lineNo.ToString(),
                          StorageLocationId = returnSn.SourceStorageLocationId.Value,
                          WarehouseId = wh != null && wh.Code == returnSn.SourceWarehouseCode ? returnSn.SourceWarehouseId.Value : 0,
                          WarehouseCode = wh?.Code,
                          ItemExtProp = dtl.ItemExtProp,
                          ItemExtPropName = dtl.ItemExtPropName,
                          LotCode = f.LotNo.IsNullOrEmpty() ? "LotDefault" : f.LotNo,
                          ItemId = f.ItemId,
                          Qty = returnSn.Qty,
                      });
                  });
                  lineNo++;
              });
            RF.Save(stockOrder);
            RF.Save(stockOrderSns);
            //创建线边仓发货单，回库的收货单，回库的发运单，收货线边仓的收货单，MES通过调用MesMoveUpdateOnhand更新发货库位的库存
            RT.Service.Resolve<ICreateSo>().CreateShippingOrderByLes(mesMoveCreateSoData);

            RT.Service.Resolve<IMesReturnItem>().CreateAsnByLes(mesMoveCreateSoData, true);
            var whTar = RF.GetById<Warehouse>(moveSnData.TargetWarehouseId);
            if (whTar != null && whTar.Code == moveSnData.TargetWarehouseCode)
            {//仓库确认是WMS的仓库才收货
                mesMoveCreateSoData.TargetWarhouseId = moveSnData.TargetWarehouseId;
                mesMoveCreateSoData.TargetStorageLocationId = moveSnData.TargetStorageLocationId;
                RT.Service.Resolve<IMesReturnItem>().CreateAsnByLes(mesMoveCreateSoData, false);
            }
        }

        /// <summary>
        /// 设置接收记录
        /// </summary>        
        private StockOrderSn GetStockOrderSn(StockOrderSn oldStockSn, decimal qty, int snLineNo, double stockId, double stockDtlId)
        {
            StockOrderSn stockOrderSn = new StockOrderSn()
            {
                ItemId = oldStockSn.ItemId,
                Sn = oldStockSn.Sn,
                LotNo = oldStockSn.LotNo,
                PackageNo = oldStockSn.PackageNo,
                Qty = qty,
                StockOrderDetailId = stockDtlId,
                StockOrderId = stockId,
                ReceiveById = RT.IdentityId,
                ReceiveTime = DateTime.Now,
                ShipQty = qty,
                State = StockOrders.ReceiveState.Received,
                LineNo = snLineNo.ToString(),
                IsSerialNumber = oldStockSn.IsSerialNumber,
                IsBatch = oldStockSn.IsBatch,
                SoLineNo = oldStockSn.SoLineNo,
                SoNo = oldStockSn.SoNo,
            };
            return stockOrderSn;
        }

        /// <summary>
        /// 备料计划ID集合
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual EntityList<StockPlan> GetStockPlansByIds(List<double> ids)
        {
            if (ids.Count == 0)
            {
                return new EntityList<StockPlan>();
            }
            return Query<StockPlan>().Where(p => ids.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 发货更新备料单
        /// </summary>
        /// <param name="labelDatas"></param>
        public virtual void UpdateStockOrderBySo(List<SoLabelData> labelDatas)
        {

        }


        /// <summary>
        /// 删除备料单明细
        /// </summary>
        /// <param name="orderNo">发运单号</param>
        /// <param name="lineNos">发运单行号集合</param>
        public virtual void DeleteStockOrderSnBySo(string orderNo, List<string> lineNos)
        {
            RT.Service.Resolve<StockOrderSnService>().DeleteStockSnRecord(orderNo, lineNos);
        }

        /// <summary>
        /// 更新备料单接收记录
        /// </summary>
        /// <param name="billNo">备料单</param>
        /// <param name="soNo">发运单</param>
        /// <param name="soLineNo">行号</param>
        /// <param name="labelNos">条码</param>        
        public virtual void UpdateStockSn(string billNo, string soNo, string soLineNo, List<string> labelNos)
        {
            var stock = Query<StockOrder>().Where(a => a.No == billNo).FirstOrDefault();
            if (stock != null)
            {
                DataProcessEx.SplitDataExecute(labelNos, sons =>
                {
                    DB.Update<StockOrderSn>().Set(p => p.SoNo, soNo).Set(p => p.SoLineNo, soLineNo)
                    .Where(p => sons.Contains(p.Sn) && p.StockOrderId == stock.Id).Execute();
                });
            }
        }

        /// <summary>
        /// 接收工单对应的物料需求明细
        /// </summary>
        /// <param name="itemIds"></param>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        public virtual EntityList<StockOrderDetail> GetStockOrderDetailsWithWoId(List<double> itemIds, double workOrderId)
        {
            List<StockState> states = new List<StockState> { StockState.Submitted, StockState.Issued, StockState.PickStocking, StockState.TobeReceive };
            return itemIds.SplitContains(tempIds =>
            {
                return Query<StockOrderDetail>()
                .Join<StockOrder>((x, y) => x.StockOrderId == y.Id && y.WorkOrderId == workOrderId)
                .Where(p => tempIds.Contains(p.ItemId) && states.Contains(p.StockState)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 接收仓库对应的物料需求明细
        /// </summary>
        /// <param name="itemIds"></param>
        /// <param name="wareId"></param>
        /// <returns></returns>
        public virtual EntityList<StockOrderDetail> GetStockOrderDetailsWithWareId(List<double> itemIds, double wareId)
        {
            List<StockState> states = new List<StockState> { StockState.Submitted, StockState.Issued, StockState.PickStocking, StockState.TobeReceive };
            return itemIds.SplitContains(tempIds =>
            {
                return Query<StockOrderDetail>()
                .Where(p => p.WarehouseId == wareId && tempIds.Contains(p.ItemId) && states.Contains(p.StockState)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }
    }
}
