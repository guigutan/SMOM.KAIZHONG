using Org.BouncyCastle.Utilities;
using SIE.Core.Common.Service;
using SIE.Core.Labels;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages;
using SIE.EventMessages.Shipment;
using SIE.Items;
using SIE.LES.MaterialReceptions.APIModels;
using SIE.LES.StockOrders;
using SIE.LES.StockOrders.Service;
using SIE.Packages.ItemLabels;
using SIE.Resources.Employees;
using SIE.Warehouses;
using SIE.Warehouses.ItemStockData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SIE.LES.MaterialReceptions.Services
{
    public partial class MaterialReceptionServices : DomainService
    {
        /// <summary>
        /// 提交物料接收
        /// </summary>
        /// <param name="labelInfos"></param>
        public virtual void Submit(List<MaterialReceptionInfo> labelInfos)
        {
            var receiveTime = RF.Find<StockOrder>().GetDbTime();
            EntityList<StockOrderSn> stockDetialSNs = IsValidBackData(labelInfos);
            var stockOrderDtls = RT.Service.Resolve<StockOrderDetailService>().GetStockOrderDetails(labelInfos.Select(p => p.BillDtlId).ToList());
            var stockOrderIds = stockOrderDtls.Select(m => m.StockOrderId).ToList();

            var stockOrderList = DB.Query<StockOrder>().Where(m => stockOrderIds.Contains(m.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var labelDatas = new List<MesLabelData>();
            var receiveDatas = new List<UpdateSoReciveData>();
            EntityList<StockOrderDetail> saveDtlList = new EntityList<StockOrderDetail>();
            EntityList<StockOrderSn> saveSnList = new EntityList<StockOrderSn >();
            using (var tran = DB.AutonomousTransactionScope(LESEntityDataProvider.ConnectionStringName))
            {
                foreach (var labelInfo in labelInfos)
                {
                    var stockOrder = stockOrderList.FirstOrDefault(m => m.Id == labelInfo.BillId);
                    UpdateStockOrderInfo(labelInfo, receiveTime, stockOrderDtls, stockOrder, stockDetialSNs, labelDatas, receiveDatas, saveDtlList, saveSnList);
                }
                //// 保存备料单接收记录
                //RF.Save(stockDetialSNs);
                ////保存备料单物料需求明细
                //RF.Save(stockOrderDtls);
                if (saveDtlList.Any())
                {
                    RF.Save(saveDtlList);
                }
                if (saveSnList.Any())
                {
                    RF.Save(saveSnList);
                }
                foreach (var stockOrder in stockOrderList)
                {
                    //更新备料单备料状态
                    ////stockOrder.StockState = stockOrder.StockOrderDetailList.Sum(p => p.ReceiveQty) < stockOrder.StockOrderDetailList.Sum(p => p.Qty) ? StockState.TobeReceive : StockState.Received;
                    RT.Service.Resolve<StockOrderService>().UpdateStockOrderState(stockOrder, stockOrder.StockOrderDetailList);
                }
                RF.Save(stockOrderList);

                //创建物料标签
                GenerateItemLabelList(labelInfos, stockOrderDtls);

                //调用WMS接口更新库存信息
                if (labelDatas.Any())
                {
                    var onhandData = new MesUpdateOnhandData();
                    onhandData.LabelDatas = labelDatas;
                    onhandData.OpType = 3;
                    onhandData.WoId = labelInfos[0].WorkOrderId == 0 ? labelInfos[0].BillId : labelInfos[0].WorkOrderId;
                    onhandData.WoNo = labelInfos[0].WorkOrderNo.IsNullOrEmpty() ? labelInfos[0].BillNo : labelInfos[0].WorkOrderNo;
                    var hasReceive = labelInfos[0].ReceiveBy == 0;
                    var receiveById = hasReceive ? RT.IdentityId : labelInfos[0].ReceiveBy;
                    onhandData.EmpCode = RF.GetById<Employee>(receiveById).Code;
                    onhandData.EnterpriseId = stockOrderDtls.FirstOrDefault()?.StockOrder?.WorkShopId;

                    RT.Service.Resolve<ILotLpnOnhand>().MesUpdateOnhand(onhandData);
                }

                // 更新发运单已接收字段
                RT.Service.Resolve<IShippingOrder>().UpdateSoRecive(receiveDatas);
                tran.Complete();
            }

        }

        /// <summary>
        /// 后台验证数据
        /// </summary>
        /// <param name="labelInfos"></param>
        /// <returns></returns>
        private EntityList<StockOrderSn> IsValidBackData(List<MaterialReceptionInfo> labelInfos)
        {
            var stockDetialSNs = _stockOrderSnDao.GetStockOrderSns(labelInfos.Select(p => p.Id).ToList());

            if (!stockDetialSNs.Any())
            {
                throw new ValidationException("无法找到接收明细记录或提交的数据接收数量为0，提交失败".L10N());
            }

            foreach (var item in stockDetialSNs)
            {
                var isExsited = labelInfos.FirstOrDefault(m => m.Id == item.Id);
                if (isExsited != null && isExsited.Qty > item.ShipQty - item.Qty)
                {
                    throw new ValidationException("接收数量超过待接收数量，请检查".L10N());
                }
                if (item.State != ReceiveState.TobeReceived)
                {
                    throw new ValidationException("存在明细状态不为待接收状态，请检查".L10N());
                }
            }

            return stockDetialSNs;
        }

        /// <summary>
        /// 更新备料单发料记录表数据
        /// </summary>
        /// <param name="labelInfo"></param>
        /// <param name="receiveTime"></param>
        /// <param name="stockOrderDtls"></param>
        /// <param name="stockOrder"></param>
        /// <param name="stockDetialSNs"></param>
        /// <param name="labelDatas"></param>
        /// <param name="receiveDatas"></param>
        /// <param name="saveDtlList"></param>
        /// <param name="saveSnList"></param>
        private static void UpdateStockOrderInfo(MaterialReceptionInfo labelInfo, DateTime receiveTime,
            EntityList<StockOrderDetail> stockOrderDtls,
            StockOrder stockOrder, EntityList<StockOrderSn> stockDetialSNs, List<MesLabelData> labelDatas,
            List<UpdateSoReciveData> receiveDatas, EntityList<StockOrderDetail> saveDtlList, EntityList<StockOrderSn> saveSnList)
        {
            var stockOrderDtl = stockOrderDtls.FirstOrDefault(p => p.Id == labelInfo.BillDtlId);
            var stockDetialSN = stockDetialSNs.FirstOrDefault(p => p.Id == labelInfo.Id);
            if (stockOrderDtl != null && stockDetialSN != null)
            {
                //更新备料单发料记录表
                stockDetialSN.ReceiveById = RT.IdentityId;
                stockDetialSN.ReceiveTime = receiveTime;
                stockDetialSN.Qty += labelInfo.Qty;
                stockDetialSN.State = (stockDetialSN.ShipQty - stockDetialSN.Qty) == 0 ? ReceiveState.Received : ReceiveState.TobeReceived;
                //DB.Update<StockOrderSn>()
                //    .Set(p => p.ReceiveById, RT.IdentityId)
                //    .Set(p => p.ReceiveTime, receiveTime)
                //    .Set(p => p.Qty, stockDetialSN.Qty + labelInfo.Qty)
                //    .Set(p => p.State, (stockDetialSN.ShipQty - stockDetialSN.Qty - labelInfo.Qty) == 0 ? ReceiveState.Received : ReceiveState.TobeReceived)
                //    .Where(p => p.StockOrderId == stockOrder.Id && p.StockOrderDetailId == stockOrderDtl.Id && p.LineNo == labelInfo.LineNo)
                //    .Execute();

                //更新备料单物料需求明细
                stockOrderDtl.ReceiveQty += labelInfo.Qty;
                //stockOrderDtl.StockState = stockOrderDtl.ReceiveQty >= stockOrderDtl.Qty ? StockState.Received : StockState.TobeReceive;
                RT.Service.Resolve<StockOrderDetailService>().UpdateStockDetailState(stockOrderDtl);
                if (stockOrderDtl.StorageLocationId != null)
                {
                    var mesLabelData = new MesLabelData();
                    mesLabelData.LabelNo = labelInfo.Label;
                    mesLabelData.IsFail = false;
                    mesLabelData.StorageLocationId = stockOrderDtl.StorageLocationId;
                    mesLabelData.Qty = labelInfo.Qty;
                    mesLabelData.ItemExtPropName = stockOrderDtl.ItemExtPropName;
                    mesLabelData.ItemExtProp = stockOrderDtl.ItemExtProp;
                    mesLabelData.ItemId = stockOrderDtl.ItemId;
                    mesLabelData.LotCode = labelInfo.BatchNo;
                    labelDatas.Add(mesLabelData);
                }
                saveDtlList.Add(stockOrderDtl);
                saveSnList.Add(stockDetialSN);
            }

            var receiveData = new UpdateSoReciveData();
            receiveData.LabelNo = labelInfo.Label;
            receiveData.SoNo = labelInfo.SoNo;
            receiveData.LineNo = labelInfo.SoLineNo;
            receiveData.BillNO = stockOrder.No;
            receiveDatas.Add(receiveData);
        }

        /// <summary>
        /// 创建物料标签列表
        /// </summary>
        /// <param name="labelInfos"></param>
        /// <param name="stockOrderDtls"></param>
        /// <returns></returns>
        private void GenerateItemLabelList(List<MaterialReceptionInfo> labelInfos,
            EntityList<StockOrderDetail> stockOrderDtls)
        {
            var itemIds = labelInfos.Select(p => p.ItemId).Distinct().ToList();
            var itemList = RT.Service.Resolve<ItemController>().GetItemList(itemIds);
            var stocks = RT.Service.Resolve<ItemStockBaseController>().GetItemStockDataBases(itemIds);
            var labelNos = labelInfos.Select(p => p.Label).Distinct().ToList();
            EntityList<Packages.ItemLabels.ItemLabel> labelInfoBigs = new EntityList<Packages.ItemLabels.ItemLabel>();
            EntityList<Packages.ItemLabels.ItemLabel> labelInfoZores = new EntityList<Packages.ItemLabels.ItemLabel>();
            EntityList<Packages.ItemLabels.ItemLabel> newSaveItemLabel = new EntityList<Packages.ItemLabels.ItemLabel>();
            var batchNoList = labelInfos.Select(p => p.BatchNo).ToList();
            var itemCodeList = labelInfos.Select(p => p.ItemCode).ToList();
            var labelList = labelInfos.Select(p => p.Label).ToList();
            var isExesitedItemLabelList = GetIsExesitedItemLabels(batchNoList, itemCodeList, labelList);
            var workOrderIds = labelInfos.Select(p => p.WorkOrderId).ToList();
            EntityList<ItemLabelWorkOrder> itemLabelWorkOrders = workOrderIds.SplitContains(tempIds =>
            {
                return DB.Query<ItemLabelWorkOrder>().Where(m => tempIds.Contains(m.WorkOrderId)).ToList();
            });
            EntityList<ItemLabelWorkOrder> saveItemLabelWorkOrders = new EntityList<ItemLabelWorkOrder>();
            using (SIE.DataAuth.DataAuths.LoadAll())
            {
                labelInfoBigs = labelNos.SplitContains(nos =>
                {
                    return DB.Query<Packages.ItemLabels.ItemLabel>().Where(m => nos.Contains(m.Label) && m.Qty > 0).ToList();
                });
                labelInfoZores = labelNos.SplitContains(nos =>
                {
                    return DB.Query<Packages.ItemLabels.ItemLabel>().Where(m => nos.Contains(m.Label) && m.Qty == 0).ToList();
                });
            }
            //labelInfos序列号要分组，合并处理
            foreach (var labelInfo in labelInfos)
            {
                var itemStock = stocks.FirstOrDefault(a => a.ItemId == labelInfo.ItemId);
                var isSerialNumber = itemStock?.IsSerialNumber == true;
                var isBitchManage = itemStock?.IsBatch == true;
                var item = itemList.FirstOrDefault(a => a.Id == labelInfo.ItemId);
                if (item != null)
                {
                    labelInfo.ItemCode = item.Code;
                }
                var isLabelManage = item?.IsLabel == true;
                if (isSerialNumber)//序列号管理
                {
                    SerialNumberSNhandle(stockOrderDtls, labelInfo, itemList, labelInfoBigs, labelInfoZores);
                    newSaveItemLabel.Add(CreatSnItemLabel(stockOrderDtls, labelInfo, itemList, labelInfo.Label, isSerialNumber));
                    continue;
                }
                
                //非序列号
                var keyword = "";
                if (isBitchManage && !isLabelManage)//批次管理
                {
                    keyword = labelInfo.BatchNo;
                }
                if (!isBitchManage && !isLabelManage)//物料编码管理
                {
                    keyword = labelInfo.ItemCode;
                }
                if (isLabelManage)
                {
                    keyword = labelInfo.Label;
                }
                // 按（标签号 + 批次号 + 物料号 + 仓库 + 库位 + 工厂）判断物料标签表中是否存在相同记录，不存在则插入新的数据，写入标签与工单对应关系。存在相同记录则在原有基础上累加“数量”和累加“工单对应关系”对应的数量。
                
                var isExesitedItemLabel = isExesitedItemLabelList
.FirstOrDefault(m => m.Label == keyword && m.Lot == labelInfo.BatchNo
                        && m.Item.Code == labelInfo.ItemCode && m.WarehouseId == labelInfo.ReceiveWarehouseId
                        && m.StorageLocationId == labelInfo.ReceiveStorageLocationId
                        && m.FactoryId == labelInfo.FactoryId
                        && m.ItemExtProp == labelInfo.ItemExtProp);
                
                if (isExesitedItemLabel != null)//更新
                {
                    isExesitedItemLabel.IsSerialNumber = isSerialNumber;
                    var firstwo = itemLabelWorkOrders.FirstOrDefault(p => p.ItemLabelId == isExesitedItemLabel.Id);
                    newSaveItemLabel.Add(UpdateItemLabel(labelInfo, isExesitedItemLabel, firstwo));
                    if (firstwo != null)
                    {
                        saveItemLabelWorkOrders.Add(firstwo);
                    }
                }
                else//新增
                {
                    newSaveItemLabel.Add(CreatSnItemLabel(stockOrderDtls, labelInfo, itemList, keyword, isSerialNumber));
                }
                
            }
            RF.Save(labelInfoBigs);
            RF.Save(labelInfoZores);
            RF.Save(newSaveItemLabel);
            RF.Save(saveItemLabelWorkOrders);
        }

        private EntityList<Packages.ItemLabels.ItemLabel> GetIsExesitedItemLabels(List<string> batchNoList, List<string> itemCodeList, List<string> labelList)
        {
            EntityList<Packages.ItemLabels.ItemLabel> isExesitedItemLabelList = new EntityList<Packages.ItemLabels.ItemLabel>();
            using (SIE.DataAuth.DataAuths.LoadAll())
            {
                // 批次管理
                var batchItemLabelList = batchNoList.SplitContains(nos =>
                {
                    return DB.Query<Packages.ItemLabels.ItemLabel>()
                            .Where(m => nos.Contains(m.Label)).ToList();
                });

                // 物料编码管理
                var itemCodeLabelList = itemCodeList.SplitContains(nos =>
                {
                    return DB.Query<Packages.ItemLabels.ItemLabel>()
                            .Where(m => nos.Contains(m.Label)).ToList();
                });

                // 物料标签
                var normalLabelList = labelList.SplitContains(nos =>
                {
                    return DB.Query<Packages.ItemLabels.ItemLabel>()
                            .Where(m => nos.Contains(m.Label)).ToList();
                });

                isExesitedItemLabelList.AddRange(normalLabelList);
                isExesitedItemLabelList.AddRange(itemCodeLabelList);
                isExesitedItemLabelList.AddRange(batchItemLabelList);
            }
            return isExesitedItemLabelList;
            
        }

        /// <summary>
        /// 更新存在的物料标签
        /// </summary>
        /// <param name="labelInfo"></param>
        /// <param name="isExesitedItemLabel"></param>
        /// <param name="firstWo"></param>
        private Packages.ItemLabels.ItemLabel UpdateItemLabel(MaterialReceptionInfo labelInfo, Packages.ItemLabels.ItemLabel isExesitedItemLabel, ItemLabelWorkOrder firstWo)
        {
            isExesitedItemLabel.Qty = isExesitedItemLabel.Qty + labelInfo.Qty;
            isExesitedItemLabel.PersistenceStatus = PersistenceStatus.Modified;
            //存在相同记录则在原有基础上累加“数量”和累加“工单对应关系”对应的数量。
            if (labelInfo.WorkOrderId.HasValue)
            {
                if (firstWo != null)//存在工单
                {
                    firstWo.Qty += labelInfo.Qty;
                    //RF.Save(firstWo);
                }
                else
                {
                    if (labelInfo.WorkOrderId.HasValue && labelInfo.WorkOrderId > 0)
                    {
                        ItemLabelWorkOrder itemLabelWorkOrder = new ItemLabelWorkOrder();
                        itemLabelWorkOrder.WorkOrderId = labelInfo.WorkOrderId.Value;
                        itemLabelWorkOrder.Qty = labelInfo.Qty;
                        itemLabelWorkOrder.GenerateId();
                        itemLabelWorkOrder.ItemLabelId = isExesitedItemLabel.Id;
                        isExesitedItemLabel.WorkOrderList.Add(itemLabelWorkOrder);
                    }
                }
            }
            //RF.Save(isExesitedItemLabel);
            return isExesitedItemLabel;
        }

        /// <summary>
        ///序列号管控的标签处理
        /// </summary>
        /// <param name="stockOrderDtls"></param>
        /// <param name="labelInfo"></param>
        /// <param name="itemList"></param>
        private void SerialNumberSNhandle(EntityList<StockOrderDetail> stockOrderDtls, MaterialReceptionInfo labelInfo, EntityList<Item> itemList, EntityList<Packages.ItemLabels.ItemLabel> labelInfoBigs, EntityList<Packages.ItemLabels.ItemLabel> labelInfoZores)
        {
            //按标签号判断是否存在数量不等于0的记录。存在等于0则删除原数据再新增，存在大于数据0的记录则报错“物料标签表存在相同的序列号标签”，找不到标签号相同的记录则直接新增。
            

            var labelInfoBig = labelInfoBigs.FirstOrDefault(m => m.Label == labelInfo.Label);

            if (labelInfoBig != null)////存在大于0的数据则加明细和数量 不再报错 2023/8/23
            {
                if (labelInfoBig.SourceType == LabelSource.Wip|| labelInfoBig.SourceType != LabelSource.BatchWip)
                {
                    labelInfoBig.Qty = 0;
                    //RF.Save(labelInfoBig);
                }
                else
                {
                    if (labelInfo.WorkOrderId.HasValue && labelInfo.WorkOrderId > 0)
                    {
                        ItemLabelWorkOrder itemLabelWorkOrder = new ItemLabelWorkOrder();
                        itemLabelWorkOrder.WorkOrderId = labelInfo.WorkOrderId.Value;
                        itemLabelWorkOrder.Qty = labelInfo.Qty;
                        itemLabelWorkOrder.GenerateId();
                        itemLabelWorkOrder.ItemLabelId = labelInfoBig.Id;
                        labelInfoBig.WorkOrderList.Add(itemLabelWorkOrder);
                    }
                    labelInfoBig.Qty += labelInfo.Qty;
                    labelInfoBig.PersistenceStatus = PersistenceStatus.Modified;
                    //RF.Save(labelInfoBig);
                    return;
                }
            }
            var labelInfoZore = labelInfoZores.FirstOrDefault(m => m.Label == labelInfo.Label);
            if (labelInfoZore != null&& labelInfoZore.SourceType!= LabelSource.Wip&& labelInfoZore.SourceType!= LabelSource.BatchWip)//存在等于0则删除原数据再新增
            {
                labelInfoZore.PersistenceStatus = PersistenceStatus.Deleted;
                //RF.Save(labelInfoZore);
            }
        }

        /// <summary>
        /// 创建序列号管理物料标签
        /// </summary>
        /// <param name="stockOrderDtls"></param>
        /// <param name="labelInfo"></param>
        /// <param name="itemList"></param>
        /// <param name="keyword"></param>
        /// <param name="isSerialNumber">是否序列号管理</param>
        private Packages.ItemLabels.ItemLabel CreatSnItemLabel(EntityList<StockOrderDetail> stockOrderDtls, MaterialReceptionInfo labelInfo,
            EntityList<Item> itemList, string keyword, bool isSerialNumber)
        {
            var item = itemList.FirstOrDefault(p => p.Id == labelInfo.ItemId);
            var stockOrderDtl = stockOrderDtls.FirstOrDefault(p => p.Id == labelInfo.BillDtlId);

            var itemLabel = new Packages.ItemLabels.ItemLabel();
            itemLabel.GenerateId();
            itemLabel.Label = keyword;
            itemLabel.Lot = labelInfo.BatchNo;
            itemLabel.ItemId = labelInfo.ItemId;
            itemLabel.Qty = labelInfo.Qty;
            itemLabel.SourceType = LabelSource.Receive;
            itemLabel.UnitId = item?.UnitId;
            itemLabel.FactoryId = labelInfo.FactoryId;
            if (stockOrderDtl != null)
            {
                itemLabel.WarehouseId = stockOrderDtl?.WarehouseId;
                itemLabel.ItemExtProp = stockOrderDtl.ItemExtProp;
                itemLabel.ItemExtPropName = stockOrderDtl.ItemExtPropName;
                itemLabel.StorageLocationId = stockOrderDtl?.StorageLocationId;
            }

            itemLabel.NgQty = 0;
            itemLabel.PersistenceStatus = PersistenceStatus.New;

            itemLabel.IsSerialNumber = isSerialNumber;
            if (labelInfo.WorkOrderId.HasValue && labelInfo.WorkOrderId > 0)
            {
                ItemLabelWorkOrder itemLabelWorkOrder = new ItemLabelWorkOrder();
                itemLabelWorkOrder.WorkOrderId = labelInfo.WorkOrderId.Value;
                itemLabelWorkOrder.Qty = labelInfo.Qty;
                itemLabelWorkOrder.GenerateId();
                itemLabelWorkOrder.ItemLabelId = itemLabel.Id;
                itemLabel.WorkOrderList.Add(itemLabelWorkOrder);
            }
            //RF.Save(itemLabel);
            return itemLabel;
        }

        #region
        /// <summary>
        /// 自动接受
        /// </summary>
        /// <param name="labelInfos">物料接收标签</param>
        /// <param name="stockOrderDtls">备料单需求明细</param>
        public virtual void SaveUpdateReceiveItem(List<MaterialReceptionInfo> labelInfos, EntityList<StockOrderDetail> stockOrderDtls)
        {
            var labelDatas = new List<MesLabelData>();
            var itemIds = labelInfos.Select(p => p.ItemId).ToList();

            var itemList = RT.Service.Resolve<ItemController>().GetItemList(itemIds);
            var itemStockDataBases = RT.Service.Resolve<ItemStockBaseController>().GetItemStockDataBases(itemIds);
            var serItems = itemStockDataBases.Where(a => a.IsSerialNumber == true).Select(a => a.ItemId).ToList();

            GenerateItemLabelList(labelInfos, stockOrderDtls);

            labelInfos.Where(a => serItems.Contains(a.ItemId)).GroupBy(a => a.Label).ForEach(a =>
            {
                if (a.Count() > 1)
                {//序列号管理，1个序列号分给至少两个备料单情况, 合并这些序列号的数量
                    var qty = a.Sum(b => b.Qty);
                    a.First().Qty = qty;
                    a.ForEach(f =>
                    {
                        if (f.Qty != qty)
                            labelInfos.Remove(f);
                    });
                }
            });
            labelInfos.ForEach(p =>
            {
                var stockOrderDtl = stockOrderDtls.FirstOrDefault(x => x.Id == p.BillDtlId);
                var item = itemList.FirstOrDefault(x => x.Id == p.ItemId);
                var itemStock = itemStockDataBases.FirstOrDefault(x => x.ItemId == p.ItemId);
                if (item == null || itemStock == null)
                {
                    throw new ValidationException("物料不存在或已作废".L10nFormat());
                }
                p.ItemCode = item.Code;
                if (p.BatchNo.IsNullOrEmpty())
                    p.BatchNo = "LotDefault";
                if (stockOrderDtl != null)
                {
                    var mesLabelData = new MesLabelData();
                    mesLabelData.LabelNo = p.Label;
                    mesLabelData.IsFail = false;
                    mesLabelData.StorageLocationId = stockOrderDtl.StorageLocationId;
                    mesLabelData.Qty = p.Qty;
                    mesLabelData.ItemId = stockOrderDtl.ItemId;
                    mesLabelData.ItemExtPropName = stockOrderDtl.ItemExtPropName;
                    mesLabelData.ItemExtProp = stockOrderDtl.ItemExtProp;
                    mesLabelData.LotCode = p.BatchNo;
                    mesLabelData.WorkOrderId = p.WorkOrderId;
                    mesLabelData.WorkOrderNo = p.WorkOrderNo;
                    mesLabelData.StockDtlId = stockOrderDtl.Id;
                    labelDatas.Add(mesLabelData);
                }
            });

            //RF.Save(itemLabelList);
            //调用WMS接口更新库存信息
            if (labelDatas.Any())
            {
                var mesLabelData = labelDatas.Where(p => p.StorageLocationId.HasValue).ToList();
                mesLabelData.GroupBy(p => new { p.WorkOrderId, p.WorkOrderNo, p.StockDtlId }).ForEach((p) =>
                {
                    var onhandData = new MesUpdateOnhandData();
                    onhandData.LabelDatas = p.ToList();
                    onhandData.OpType = 3;
                    onhandData.WoId = p.Key.WorkOrderId;
                    onhandData.WoNo = p.Key.WorkOrderNo;
                    onhandData.EmpCode = RF.GetById<Employee>(labelInfos[0].ReceiveBy).Code;
                    onhandData.EnterpriseId = stockOrderDtls.FirstOrDefault(x => x.Id == p.Key.StockDtlId)?.StockOrder?.WorkShopId;
                    if (onhandData.LabelDatas.Count > 0)
                    {
                        RT.Service.Resolve<ILotLpnOnhand>().MesUpdateOnhand(onhandData);
                    }
                });
            }
        }
        #endregion
    }
}
