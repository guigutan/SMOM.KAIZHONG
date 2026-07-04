using System;
using System.Collections.Generic;
using System.Linq;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Datas.SmomOrder;
using SIE.ERPInterface.Common.Enums;
using SIE.Items;

namespace SIE.ERPInterface.Smom.Download.PurchaseOrders
{
    /// <summary>
    /// 采购订单明细下载控制
    /// </summary>
    public class EbsDownloadPoController : DomainController
    {
        /// <summary>
        /// 从API下载企业模型到业务表
        /// </summary>
        /// <param name="planDatas"></param>
        /// <param name="extentInvOrg">ERP库存组织Id</param>
        /// <returns></returns>
        public virtual ApiResult DownloadPurOrderToBusiness(List<EbsPurOrderData> planDatas, string extentInvOrg)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.EbsApiSaveBusinessData<EbsPurOrderData>(
                planDatas,
                p => this.SavePurOrderNo(p),
                JobType.PurchaseOrder,
                extentInvOrg);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public virtual List<ErpErrorData> SavePurOrderNo(List<EbsPurOrderData> datas)
        {
            var errors = new List<ErpErrorData>();
            if (datas.Count == 0)
                return errors;
            var supplierCodes = datas.Select(a => a.SupplierCode).Distinct().ToList();
            var shipDics = RT.Service.Resolve<SupplierController>().GetSuppliers(supplierCodes).ToDictionary(p => p.Code, p => p.Id);
            var itemCodes = datas.Select(a => a.ItemCode).Distinct().ToList();
            var itemDics = RT.Service.Resolve<ItemController>().GetItems(itemCodes, new EagerLoadOptions().LoadWithViewProperty()).ToDictionary(p => p.Code, p => p);
            var itemIds = itemDics.Select(a => a.Value.Id).ToList();
            var secondUnits = RT.Service.Resolve<ItemUnitController>().GetAllItemUnits(itemIds);
            var PurNos = datas.Select(a => a.PoNo).ToList();
            //var Purs = RT.Service.Resolve<PurchaseOrderController>().GetPurchaseOrderDatas(PurNos);
            //EntityList<PurchaseOrder> purchaseOrders = new EntityList<PurchaseOrder>();
            //EntityList<PurchaseOrderDetail> purchaseOrderDetails = new EntityList<PurchaseOrderDetail>();
            List<double> delPoDtlId = new List<double>();
            datas.GroupBy(a => new { a.OrderId, a.PoNo }).ForEach(p =>
              {
                  //PurchaseOrder purchaseOrder = new PurchaseOrder();
                  //var curPur = Purs.FirstOrDefault(x => x.ErpOrderId == p.Key.OrderId);
                  //if (curPur != null)
                  //{
                  //    purchaseOrder = curPur;
                  //}
                  bool isNew = false;
                  p.ForEach(f =>
                  {
                      try
                      {
                          if (f.SupplierCode.IsNullOrEmpty())
                          {
                              throw new ValidationException("供应商编码{0}不存在".L10nFormat(f.SupplierCode));
                          }
                          if (!f.SupplierCode.IsNullOrEmpty() && !shipDics.ContainsKey(f.SupplierCode))
                              throw new ValidationException("供应商编码{0}不存在".L10nFormat(f.SupplierCode));
                          if (!itemDics.ContainsKey(f.ItemCode))
                              throw new ValidationException("物料{0}不存在".L10nFormat(f.ItemCode));
                          if (f.PoNo.IsNullOrEmpty())
                              throw new ValidationException("单号不能为空".L10N());
                        //if (f.Quantity <= 0 && f.CancelQty > 0)
                        //{
                        //     throw new ValidationException("单据不为取消状态的采购数量必须大于0".L10N());
                        //}
                        var item = itemDics.GetValue<Item>(f.ItemCode);
                          //if (purchaseOrder.Id == 0)
                          //{
                          //    purchaseOrder = GetPurchaseOrderData(purchaseOrder, Purs, f, shipDics);
                          //    //purchaseOrders.Add(purchaseOrder);
                          //    isNew = true;
                          //}
                          f.SecondUnitCode = f.UnitCode;
                          f.SecondQuantity = f.Quantity;
                          if (item.UnitCode.ToUpper() != f.UnitCode.ToUpper())
                          {
                            //不是物料的主单位，找是否有辅助单位对应
                            var secondUnit = secondUnits.FirstOrDefault(a => a.UnitCode.ToUpper() == f.UnitCode.ToUpper() && a.MainUnitId == item.UnitId && (a.ItemId == item.Id || a.IsBaseUnit));
                              if (secondUnit == null)
                              {
                                  throw new ValidationException("ERP物料{0}单位{1}跟MOM的单位{2}不一致，而且在单位转换中找不到记录".L10nFormat(item.Code, f.UnitCode, item.UnitName));
                              }
                              
                              var den = ((decimal)secondUnit.Numerator / (decimal)secondUnit.Denominator);
                              var changeQty = RT.Service.Resolve<ItemUnitController>().TrancateTradeQty(f.Quantity / den, item.UnitPrecision, item.UnitTradeType);
                              //转换后的上线前数量
                              decimal firstQty = 0;
                              firstQty = f.FirstReceiveQty;
                              if (firstQty > 0)
                              {
                                  firstQty = RT.Service.Resolve<ItemUnitController>().TrancateTradeQty(firstQty / den, item.UnitPrecision, item.UnitTradeType);
                              }
                            var AllowMoreQty = RT.Service.Resolve<ItemUnitController>().TrancateTradeQty(f.AllowMoreQty / den, item.UnitPrecision, item.UnitTradeType);
                              f.Quantity = changeQty;
                              f.AllowMoreQty = AllowMoreQty;
                              f.UnitCode = secondUnit.UnitCode;
                              f.SecondUnitId = secondUnit.UnitId;
                              f.FirstReceiveQty = firstQty;
                              //f.SecondUnitCode = 
                          }
                          //var PurDtl = purchaseOrder.PurchaseOrderDetailList.FirstOrDefault(x => !x.ErpDetailId.IsNullOrEmpty() && x.ErpDetailId == f.ErpDetailId);
                          //var lineNo = (purchaseOrder.PurchaseOrderDetailList == null || purchaseOrder.PurchaseOrderDetailList.Count == 0) ? 1 : purchaseOrder.PurchaseOrderDetailList.Max(a => int.Parse(a.LineNo)) + 1;
                          //if (PurDtl == null)
                          //{
                          //    if (f.Quantity > 0)
                          //    {
                          //        var purDtlData = GetNewPurDtl(f, purchaseOrder.Id, item, lineNo);
                          //        if (isNew)
                          //        {
                          //            purchaseOrder.PurchaseOrderDetailList.Add(purDtlData);
                          //        }
                          //        else
                          //        {
                          //            purchaseOrderDetails.Add(purDtlData);
                          //        }
                          //        errors.Add(new ErpErrorData() { ErrMsg = "", Infkey = f.ErpDetailId, IsSuccess = true });
                          //    }
                          //    else
                          //    {
                          //        errors.Add(new ErpErrorData() { ErrMsg = "", Infkey = f.ErpDetailId, IsSuccess = true });
                          //    }
                          //}
                          //else
                          //{
                          //  //说明该明细已下载
                          //  SetPurDtl(f, PurDtl, purchaseOrder, item, purchaseOrderDetails, delPoDtlId);
                          //  errors.Add(new ErpErrorData() { ErrMsg = "", Infkey = f.ErpDetailId, IsSuccess = true });
                          //}
                      }
                      catch (Exception ex)
                      {
                          errors.Add(new ErpErrorData() { ErrMsg = ex.Message, Infkey = f.ErpDetailId });
                      }
                  });
                  //if (isNew && purchaseOrder.PurchaseOrderDetailList.Count > 0)
                  //{
                  //    purchaseOrders.Add(purchaseOrder);
                  //}
              });
            //if (purchaseOrders.Count > 0)
            //{
            //    //保存新的单据信息
            //    RF.Save(purchaseOrders);
            //}
            //if (purchaseOrderDetails.Count > 0)
            //{
            //    //保存更新的明细信息
            //    RF.Save(purchaseOrderDetails);
            //}
            //if (delPoDtlId.Count > 0)
            //{
            //    //删除采购订单明细
            //    RT.Service.Resolve<PurchaseOrderController>().DeletePurDetailByIds(delPoDtlId);
            //}
            return errors;
        }

        /// <summary>
        /// 获取采购订单单头数据
        /// </summary>
        /// <param name="purchaseOrder">SMOM采购订单实体</param>
        /// <param name="purchaseOrders">SMOM采购订单数据</param>
        /// <param name="EbsPurchaseOrder">EBS推送采购订单数据</param>
        /// <param name="shipDics">供应商字典</param>
        /// <returns></returns>
        //private PurchaseOrder GetPurchaseOrderData(PurchaseOrder purchaseOrder, EntityList<PurchaseOrder> purchaseOrders, EbsPurOrderData EbsPurchaseOrder, Dictionary<string, double> shipDics)
        //{
        //    var hasNo = purchaseOrders.FirstOrDefault(f => f.No == EbsPurchaseOrder.PoNo);
        //    purchaseOrder.No = EbsPurchaseOrder.PoNo;
        //    if (hasNo != null)
        //    {
        //        //说明单号有重复且重来没下载过这笔数据
        //        purchaseOrder.No = EbsPurchaseOrder.PoNo + "_ERP";
        //    }
        //    purchaseOrder.GenerateId();
        //    purchaseOrder.OrderType = PoOrderType.Purchase;
        //    purchaseOrder.State = PO.PurchaseOrders.State.Audited;
        //    purchaseOrder.AuditorId = RT.IdentityId;
        //    purchaseOrder.AuditDate = DateTime.Now;
        //    purchaseOrder.SupplierId = shipDics.GetValue<double>(EbsPurchaseOrder.SupplierCode);
        //    purchaseOrder.ErpOrderId = EbsPurchaseOrder.OrderId;
        //    //purchaseOrder.ErpOrgName = EbsPurchaseOrder.OrgName;
        //    purchaseOrder.ErpOrganizationId = EbsPurchaseOrder.OrganizationId;
        //    purchaseOrder.SourceType = PO.PurchaseOrders.Datas.PurSourceType.Erp;
        //    purchaseOrder.ErpOrganizationName = EbsPurchaseOrder.OrganizationName;
        //    purchaseOrder.SourceKey = EbsPurchaseOrder.PoNo;
        //    return purchaseOrder;
        //}


        /// <summary>
        /// 创建新的采购明细
        /// </summary>
        /// <param name="EbsPurchaseOrder">EBS采购订单推送数据</param>
        /// <param name="purId">采购订单ID</param>
        /// <param name="item">物料</param>
        /// <param name="LineNo">行号</param>
        /// <returns></returns>
        //private PurchaseOrderDetail GetNewPurDtl(EbsPurOrderData EbsPurchaseOrder, double purId, Item item, int LineNo)
        //{

        //    PurchaseOrderDetail purchaseOrderDetail = new PurchaseOrderDetail();
        //    purchaseOrderDetail.GenerateId();
        //    purchaseOrderDetail.Quantity = EbsPurchaseOrder.Quantity;
        //    //purchaseOrderDetail.NotReceiveQty = EbsPurchaseOrder.Quantity;
        //    purchaseOrderDetail.UnitPrice = EbsPurchaseOrder.UnitPrice;
        //    purchaseOrderDetail.UnDeliveredQty = EbsPurchaseOrder.Quantity;
        //    purchaseOrderDetail.ErpDetailId = EbsPurchaseOrder.ErpDetailId;
        //    purchaseOrderDetail.SourceKey = EbsPurchaseOrder.ERPLineNo;
        //    //purchaseOrderDetail.SecondUnitName = EbsPurchaseOrder.SecondUnitCode;
        //    purchaseOrderDetail.PurchaseQty = EbsPurchaseOrder.SecondQuantity;
        //    purchaseOrderDetail.SecondUnitId = EbsPurchaseOrder.SecondUnitId;
        //    //purchaseOrderDetail.
        //    //purchaseOrderDetail.AllowMoreQty = EbsPurchaseOrder.AllowMoreQty;
        //    if (EbsPurchaseOrder.CancelQty > 0)
        //    {
        //        purchaseOrderDetail.ErpCancelQty = EbsPurchaseOrder.CancelQty;
        //        purchaseOrderDetail.ErpIsCancel = true;

        //    }
        //    //首次创建时>0的，上线前已收数；修改时>0不做处理 上线前已收数量更新到已收数量
        //    purchaseOrderDetail.ErpOrgName = EbsPurchaseOrder.OrgName;
        //    purchaseOrderDetail.ReceiveQty = EbsPurchaseOrder.FirstReceiveQty;
        //    purchaseOrderDetail.FirstReceiveQty = EbsPurchaseOrder.FirstReceiveQty;
        //    purchaseOrderDetail.PsType = PsType.Standard;
        //    purchaseOrderDetail.PurchaseOrderId = purId;
        //    purchaseOrderDetail.ErpLineNo = EbsPurchaseOrder.ERPLineNo;
        //    purchaseOrderDetail.OrderNo = EbsPurchaseOrder.OrderNumber;
        //    purchaseOrderDetail.ErpVersion = EbsPurchaseOrder.VersionNo;
        //    purchaseOrderDetail.ItemId = item.Id;
        //    purchaseOrderDetail.LineNo = LineNo.ToString();
        //    purchaseOrderDetail.IsReturnErp = EbsPurchaseOrder.IsReturnErp == 1;
        //    purchaseOrderDetail.NotReceiveQty = purchaseOrderDetail.Quantity - purchaseOrderDetail.ReceiveQty + purchaseOrderDetail.RejectedQty;
        //    if (DateTime.TryParse(EbsPurchaseOrder.DeliveryDate, out DateTime dt))
        //        purchaseOrderDetail.DeliveryDate = dt;
        //    else
        //        purchaseOrderDetail.DeliveryDate = DateTime.Now;
        //    return purchaseOrderDetail;
        //}

        /// <summary>
        /// 设置旧的采购明细
        /// </summary>
        /// <param name="EbsPurchaseOrder">ERP推送采购订单数据</param>
        /// <param name="purchaseOrderDetail">采购明细数据</param>
        /// <param name="PurOrder">采购订单</param>
        /// <param name="item">物料</param>
        /// <param name="purchaseOrderDetails">采购订单明细</param>
        /// <param name="delPoDtlId">需要删除的单据</param>
        /// <returns></returns>
        //private void SetPurDtl(EbsPurOrderData EbsPurchaseOrder, PurchaseOrderDetail purchaseOrderDetail, PurchaseOrder PurOrder, Item item, EntityList<PurchaseOrderDetail> purchaseOrderDetails, List<double> delPoDtlId)
        //{
        //    //ERP推送的版本号大于当前采购明细的订单号才允许更新
        //    if (int.Parse(EbsPurchaseOrder.VersionNo) > int.Parse(purchaseOrderDetail.ErpVersion))
        //    {
        //        /*
        //           1、《采购订单》= 审核，全单覆盖更新；采购数量=0 的时候删除单据明细
        //           2、《采购订单》> 审核，< 已收货，只更新采购数量，限制：接口的采购数量>=上线前已收数+已收数 + 已建单未收数；
        //           3、《采购订单》=已收货，不更新；
        //        */
        //        if (PurOrder.State == PO.PurchaseOrders.State.Audited)
        //        {
        //            if (EbsPurchaseOrder.Quantity == 0)
        //            {
        //                delPoDtlId.Add(purchaseOrderDetail.Id);
        //                return;
        //            }
        //            //purchaseOrderDetail.Quantity = EbsPurchaseOrder.Quantity;
        //            purchaseOrderDetail.UnitPrice = EbsPurchaseOrder.UnitPrice;
        //            purchaseOrderDetail.ItemId = item.Id;
        //            //当有取消的时候不更新原来的采购数量 更新取消数量
        //            purchaseOrderDetail.UnitPrice = EbsPurchaseOrder.UnitPrice;
        //            purchaseOrderDetail.Quantity = EbsPurchaseOrder.Quantity;
        //            if (EbsPurchaseOrder.CancelQty > 0)
        //            {
        //                purchaseOrderDetail.ErpCancelQty = EbsPurchaseOrder.CancelQty;
        //                purchaseOrderDetail.ErpIsCancel = true;
        //            }
        //            purchaseOrderDetail.ErpVersion = EbsPurchaseOrder.VersionNo;
        //            purchaseOrderDetail.NotReceiveQty = purchaseOrderDetail.Quantity - purchaseOrderDetail.ReceiveQty + purchaseOrderDetail.RejectedQty;
        //            purchaseOrderDetails.Add(purchaseOrderDetail);
        //        }
        //        if (PurOrder.State == PO.PurchaseOrders.State.PartialReceived)
        //        {
        //            if (EbsPurchaseOrder.Quantity < purchaseOrderDetail.FirstReceiveQty + purchaseOrderDetail.ReceiveQty + purchaseOrderDetail.DeliveredQty)
        //            {
        //                throw new ValidationException("采购订单:{0}下物料{1}采购数量小于SMOM采购订单:{2}下物料:{1}的上线前已收数:{3}+已收数:{4}+已建单未收数:{5}".L10nFormat(EbsPurchaseOrder.PoNo, EbsPurchaseOrder.ItemCode, PurOrder.No, purchaseOrderDetail.FirstReceiveQty, purchaseOrderDetail.ReceiveQty, purchaseOrderDetail.DeliveredQty));
        //            }
        //            purchaseOrderDetail.Quantity = EbsPurchaseOrder.Quantity;
        //            if (EbsPurchaseOrder.CancelQty > 0)
        //            {
        //                purchaseOrderDetail.ErpCancelQty = EbsPurchaseOrder.CancelQty;
        //                purchaseOrderDetail.ErpIsCancel = true;
        //            }
        //            purchaseOrderDetail.ErpVersion = EbsPurchaseOrder.VersionNo;
        //            purchaseOrderDetails.Add(purchaseOrderDetail);
        //        }
        //    }
        //}
    }
}
