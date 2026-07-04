using System;
using System.Collections.Generic;
using System.Linq;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.Inventory.Commom;
using SIE.Inventory.Strategy;
using SIE.Items;
using SIE.Warehouses;

namespace SIE.ERPInterface.Smom.Download
{
    /// <summary>
    /// 发运单明细下载控制器
    /// </summary>
    public class DownloadShipmentDtlController : DomainController
    {
        /// <summary>
        /// 从API下载发运单明细到业务表
        /// </summary>
        /// <param name="orderDtlDatas"></param>
        /// <param name="invOrg"></param>
        /// <returns></returns>
        public virtual ApiResult DownloadOrderDtlToBusiness(List<ShippingOrderDetailData> orderDtlDatas, int invOrg)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.ApiSaveBusinessData<ShippingOrderDetailData>(
                orderDtlDatas,
                p => this.SaveShippingOrderDetails(p.OrderByLastUpdateDate()),
                JobType.ShippingOrderDtl,
                invOrg);
        }

        /// <summary>
        /// 从中间表下载发运单明细到业务表
        /// </summary>
        public virtual ProcessResult DownloadOrderDtlInfToBusiness(bool isManual = false)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.SaveBusinessData<ShippingOrderDetailInf>(
                () => ctl.GetUnprocessedDatas<ShippingOrderDetailInf>(),           //发运单明细中间表数据
                p =>
                {
                    var paras = this.GenerateShippingOrderDtlPara(p);
                    return this.SaveShippingOrderDetails(paras.OrderByLastUpdateDate());
                },
                JobType.ShippingOrderDtl, isManual);
        }

        /// <summary>
        /// 生成发运单明细实体
        /// </summary>
        /// <param name="orderDtlInfs">中间表实体数据</param>
        /// <returns></returns>
        public virtual List<ShippingOrderDetailData> GenerateShippingOrderDtlPara(IEnumerable<ShippingOrderDetailInf> orderDtlInfs)
        {
            var paras = new List<ShippingOrderDetailData>();

            orderDtlInfs.ForEach(p =>
            {
                var data = new ShippingOrderDetailData();
                data.LastUpdateDate = p.LastUpdateDate.HasValue ? p.LastUpdateDate.Value : DateTime.Now;
                data.IsDelete = p.IsDelete;
                data.Infkey = p.Id;
                data.ShippingOrderNo = p.ShippingOrderNo;
                data.ExpectQty = p.ExpectQty;
                data.OrderNo = p.OrderNo;
                data.AppointStorageLocationCode = p.AppointStorageLocation;
                data.ItemCode = p.ItemCode;
                data.LineNo = p.LineNo;
                data.PoNo = p.PoNo;
                data.PoDetailLineNo = p.PoDetailLineNo;

                ////if (orderDtlData.PersistenceStatus == PersistenceStatus.New)
                ////    orderDtlData.OrderState = ShippingOrderState.Audited;
                ////else if (orderDtlInf.OrderState == ShippingOrderState.Cancel || orderDtlInf.OrderState == ShippingOrderState.Delivery)
                ////    orderDtlData.OrderState = orderDtlInf.OrderState;

                data.ErpKey = p.ErpKey;
                data.ErpId = double.Parse(p.ErpKey);

                paras.Add(data);
            });

            return paras;
        }



        /// <summary>
        /// 保存发运单明细
        /// </summary>
        /// <param name="data">发运单明细数据</param>
        /// <param name="dic">发运单明细字典</param>
        /// <param name="dicItem">物料字典</param>
        /// <param name="so">发运单</param>
        /// <param name="dicLocation">库位字典</param>
        /// <param name="dicLot">批次字典</param>
        /// <param name="dicPo">PO字典</param>
        /// <param name="dicItemIOLimit">仓储字典</param>
        /// <param name="defaultAssignRule">默认分配规则</param>
        /// <param name="defaultTurnOverRule">默认周转规则</param>
        //public virtual void SaveShippingOrderDetail(ShippingOrderDetailData data,
        //    Dictionary<string, ShippingOrderDetail> dic,
        //    Dictionary<string, Item> dicItem,
        //    ShippingOrder so,
        //    Dictionary<string, StorageLocation> dicLocation,
        //    Dictionary<string, Lot> dicLot,
        //    Dictionary<string, PurchaseOrder> dicPo,
        //    Dictionary<string, ItemIOLimit> dicItemIOLimit,
        //    AssignRule defaultAssignRule,
        //    TurnOverRule defaultTurnOverRule)
        //{
        //    var ctl = RT.Service.Resolve<DownloadBusBaseController>();

        //    if (data.ItemCode.IsNullOrEmpty() || !dicItem.ContainsKey(data.ItemCode))
        //        throw new ValidationException("发运单[{1}]的明细行物料编码[{0}]不存在".L10nFormat(data.ItemCode, so.No));
        //    var item = dicItem[data.ItemCode];

        //    ShippingOrderDetail soDetail = new ShippingOrderDetail();
        //    var key = "{0}_{1}".FormatArgs(data.ItemCode, data.LineNo); //物料+行号 为主键

        //    //处理待删除数据
        //    if (dic.ContainsKey(key))
        //    {
        //        if (data.IsDelete)
        //        {
        //            ctl.DeleteEntity(dic, key, dic[key]);

        //        }

        //        return;
        //    }
        //    if (!dic.ContainsKey(key))
        //        dic.Add(key, new ShippingOrderDetail());

        //    soDetail.ShippingOrder = so;
        //    soDetail.LineNo = data.LineNo;
        //    soDetail.Item = item;
        //    soDetail.ExpectQty = data.ExpectQty;
        //    soDetail.OrderNo = data.OrderNo;
        //    soDetail.SourceKey = data.ErpKey;
           
        //    soDetail.AppointLpn = data.AppointLpn;
        //    if (data.AppointStorageLocationCode.IsNotEmpty())
        //    {
        //        if (!dicLocation.ContainsKey(data.AppointStorageLocationCode))
        //            throw new ValidationException("发运单[{1}]的明细行指定库位编码[{0}]不存在".L10nFormat(data.AppointStorageLocationCode, so.No));
        //        soDetail.AppointStorageLocation = dicLocation[data.AppointStorageLocationCode];
        //    }
        //    if (data.AppointLotCode.IsNotEmpty())
        //    {
        //        if (!dicLot.ContainsKey(data.AppointLotCode))
        //            throw new ValidationException("发运单[{1}]的明细行指定批次编码[{0}]不存在".L10nFormat(data.AppointLotCode, so.No));
        //        soDetail.AppointLot = dicLot[data.AppointLotCode];
        //    }

        //    if (dicItemIOLimit.ContainsKey("{0}_{1}".FormatArgs(item.Id, so.ShippingWareHouseId)))
        //    {
        //        var itemIOLimit = dicItemIOLimit["{0}_{1}".FormatArgs(item.Id, so.ShippingWareHouseId)];
        //        soDetail.AssignRule = itemIOLimit?.AssignRule ?? defaultAssignRule;
        //        soDetail.TurnOverRule = itemIOLimit?.TurnOverRule ?? defaultTurnOverRule;
        //    }
        //    if (soDetail.AssignRuleId == 0)
        //    {
        //        soDetail.AssignRuleId = defaultAssignRule.Id;
        //    }
        //    if (soDetail.TurnOverRuleId == 0)
        //    {
        //        soDetail.TurnOverRuleId = defaultTurnOverRule.Id;
        //    }

        //    if (so.OrderType == OrderType.SupplierReturn)
        //    {
        //        if (data.PoNo.IsNullOrEmpty() || !dicPo.ContainsKey(data.PoNo))
        //            throw new ValidationException("发运单[{1}]的明细行PO[{0}]不存在".L10nFormat(data.PoNo, so.No));
        //        soDetail.PurchaseOrder = dicPo[data.PoNo];

        //        var poDetail = dicPo[data.PoNo].PurchaseOrderDetailList.FirstOrDefault(p => p.LineNo == data.LineNo && p.Item.Code == data.ItemCode);
        //        if (poDetail == null)
        //            throw new ValidationException("发运单[{1}]的明细行[{0}]没有匹配到相应的PO行信息".L10nFormat(data.LineNo, so.No));
        //        soDetail.PurchaseOrderDetail = poDetail;

        //        soDetail.OrderNo = poDetail.PurchaseOrder.No;                
        //    }

        //    RF.Save(soDetail);
        //}

        /// <summary>
        /// 更新明细行
        /// </summary>
        /// <param name="datas">ASN数据列表</param>
        /// <returns>错误信息列表</returns>
        public virtual List<ErpErrorData> SaveShippingOrderDetails(List<ShippingOrderDetailData> datas)
        {
            var errors = new List<ErpErrorData>();
            if (datas.Count == 0)
                return errors;

            //var ctl = RT.Service.Resolve<ShippingOrderService>();

            #region 获取数据

            List<string> noList = datas.Select(p => p.ShippingOrderNo).Distinct().ToList();
            //var shippingOrderList = ctl.GetShippingOrders(noList);
            //var soDict = shippingOrderList.ToDictionary(p => p.No, p => p);

            //获取明细数据
            //var soDetails = ctl.GetShippingOrderDetailList(shippingOrderList.Select(p => p.Id).ToList());
            //var dicDetails = soDetails.GroupBy(p => p.ShippingOrder.No).ToDictionary(p => p.Key, p => p.ToList());    //<soNo,明细列表>

            //获取库位
            var locationCodeList = datas.Select(p => p.AppointStorageLocationCode).Distinct().ToList();
            var locationList = RT.Service.Resolve<WarehouseController>().GetStorageLocations(locationCodeList);
            var dicLocation = locationList.ToDictionary(p => p.Code, p => p);

            // 批次
            var lotCodeList = datas.Select(p => p.AppointLotCode).Distinct().ToList();
            var lotList = RT.Service.Resolve<LotController>().GetLot(lotCodeList);
            var dicLot = lotList.ToDictionary(p => p.Code, p => p);

            //获取PO
            var poNoList = datas.Select(p => p.PoNo).Distinct().ToList();
            //var poList = RT.Service.Resolve<PurchaseOrderController>().GetPurchaseOrderDatas(poNoList);
            //var dicPo = poList.ToDictionary(p => p.No, p => p);

            //获取物料数据
            var itemCodes = datas.Select(p => p.ItemCode).Distinct().ToList();
            var items = RT.Service.Resolve<ItemController>().GetItems(itemCodes);
            var dicItem = items.ToDictionary(p => p.Code, p => p);

            //获取物料包装规则
            var itemIdList = dicItem.Select(p => p.Value.Id).Distinct().ToList();
            //var dicPackDetail = RT.Service.Resolve<PackageController>().GetItemsMasterUnit(itemIdList);

            //获取物料收发信息
            //var itemIOLimits = RT.Service.Resolve<ItemExtController>().GetItemIOLimits(itemIdList);
            //var dicItemIOLimit = itemIOLimits.ToDictionary(p => "{0}_{1}".FormatArgs(p.ItemId, p.WarehouseId));

            var ruleCtl = RT.Service.Resolve<RuleController>();
            AssignRule defaultAssignRule = ruleCtl.GetAssignRule(AssignRule.Default);          //默认分配规则
            TurnOverRule defaultTurnOverRule = ruleCtl.GetTurnOverRule(TurnOverRule.Default);  //默认周转规则
            #endregion

            //按顺序处理数据
            foreach (var data in datas)
            {
                try
                {
                    var key = data.ShippingOrderNo;  //产品ShippingOrderNo编码为主键
                    //if (!soDict.ContainsKey(key))
                    //    throw new ValidationException("发运单号[{0}]不存在".L10nFormat(key));
                    //var so = soDict[key];
                    //if (!dicDetails.ContainsKey(key))
                    //    dicDetails.Add(key, new List<ShippingOrderDetail>());
                    //var dicDetail = dicDetails[key].ToDictionary(p => p.Item.Code);

                    //SaveShippingOrderDetail(data, dicDetail, dicItem, so, dicLocation, dicLot, dicPo, dicItemIOLimit, defaultAssignRule, defaultTurnOverRule);

                }
                catch (Exception ex)
                {
                    errors.Add(new ErpErrorData() { ErrMsg = ex.Message, Infkey = data.Infkey });
                }
            }

            return errors;
        }
    }
}
