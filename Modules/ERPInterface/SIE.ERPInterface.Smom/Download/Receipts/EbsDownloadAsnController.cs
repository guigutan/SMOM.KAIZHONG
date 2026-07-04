using System;
using System.Collections.Generic;
using System.Linq;
using SIE.Core.Enums;
using SIE.CSM.Customers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.Inventory.Commom;
using SIE.Inventory.Transactions;
using SIE.Items;
using SIE.Packages;

namespace SIE.ERPInterface.Smom.Download.Receipts
{
    /// <summary>
    /// ASN下载控制器
    /// </summary>
    public class EbsDownloadAsnController : DomainController
    {
        #region 销售退货
        /// <summary>
        /// 从API下载企业模型到业务表
        /// </summary>
        /// <param name="planDatas"></param>
        /// <param name="extentInvOrg">ERP库存组织Id</param>
        /// <returns></returns>
        public virtual ApiResult DownloadSaleReturnToBusiness(List<EbsToSmomAsnDetailData> planDatas, string extentInvOrg)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.EbsApiSaveBusinessData<EbsToSmomAsnDetailData>(
                planDatas,
                p => this.SaveSaleReturn(p),
                JobType.SaleReturn,
                extentInvOrg);
        }

        /// <summary>
        /// 设置ASN单数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="cusId">客户</param>
        /// <param name="transactionId">小类</param>
        /// <returns></returns>
        //private Asn SetAsn(EbsToSmomAsnDetailData data, double cusId, double transactionId)
        //{
        //    var asn = new Asn()
        //    {
        //        No = RT.Service.Resolve<AsnService>().GetAsnNo(OrderType.SaleReturn),
        //        OrderType = OrderType.SaleReturn,
        //        AsnState = AsnState.Save,
        //        AsnSource = AsnSource.ErpWay,
        //        Connecter = data.Connecter,
        //        CustomerId = cusId,
        //        CreateDate = data.CreateDate,
        //        DeliveryDate = data.DeliveryDate,
        //        ErpOrderId = data.OrderId,
        //        IsRelease = true,
        //        ReleaseDate = DateTime.Now,
        //        ReleaserId = RT.IdentityId,
        //        RequireNo = data.OrderNumber,
        //        ShipperCode = "*",
        //        ShipperName = "*",
        //        TransactionId = transactionId,
        //        SourceKey = data.OrderNumber,
        //    };
        //    asn.GenerateId();
        //    return asn;
        //}

        /// <summary>
        /// 设置明细数据
        /// </summary>       
        //private void SetAsnDetail(AsnDetail asnDtl, EbsToSmomAsnDetailData p, Item item, bool isIqc, bool isBatch, EntityList<ItemPackageRuleDetail> pkgDtls, EntityList<Items.ItemUnit> secondUnits)
        //{
        //    double itemId = item.Id;
        //    asnDtl.OrderNo = p.OrderNumber;
        //    asnDtl.ProjectNo = "*";
        //    asnDtl.TaskNo = "*";
        //    asnDtl.ItemId = itemId;
        //    asnDtl.AsnState = AsnState.Save;
        //    asnDtl.SourceKey = p.ErpDetailId;
        //    asnDtl.OrderLineNo = p.ErpSplitLineNo;
        //    asnDtl.ErpDetailId = p.ErpDetailId;
        //    asnDtl.ErpOrganizationName = p.OrganizationName;
        //    asnDtl.IsBatch = isBatch;
        //    asnDtl.IsNeedIqc = isIqc;
        //    var pkgDtl = pkgDtls.FirstOrDefault(a => a.ItemId == itemId);
        //    if (pkgDtl == null)
        //    {
        //        throw new ValidationException("物料{0}没有维护物料包装规则".L10nFormat(item.Code));
        //    }
        //    asnDtl.ItemPackageRuleId = pkgDtl.ItemPackageRuleId;
        //    asnDtl.ItemPackageRuleDetailId = pkgDtl.Id;
        //    asnDtl.ItemPackQty = pkgDtl.Qty;
        //    asnDtl.Lpn = "*";
        //    asnDtl.Remark = p.Remark;

        //    if (asnDtl.IsBatch)
        //    {
        //        asnDtl.Lot = "Lot";
        //        asnDtl.LotAtt04 = p.ProductBatch;
        //        asnDtl.LotAtt01 = p.LotAtt01;
        //        asnDtl.LotAtt02 = p.LotAtt02;
        //    }
        //    else
        //        asnDtl.Lot = Lot.LotDefault;

        //    if (item.UnitCode.ToUpper() != p.UnitCode.ToUpper())
        //    {//不是物料的主单位，找是否有辅助单位对应
        //        var secondUnit = secondUnits.FirstOrDefault(a => a.UnitCode.ToUpper() == p.UnitCode.ToUpper() && a.MainUnitId == item.UnitId && (a.ItemId == item.Id || a.IsBaseUnit));
        //        if (secondUnit == null)
        //            throw new ValidationException("ERP物料{0}单位{1}跟MOM的单位{2}不一致，而且在单位转换中找不到记录".L10nFormat(item.Code, p.UnitCode, item.UnitName));
        //        var den = ((decimal)secondUnit.Numerator / (decimal)secondUnit.Denominator);
        //        var changeQty = RT.Service.Resolve<ItemUnitController>().TrancateTradeQty(p.ExpectQty / den, item.UnitPrecision, item.UnitTradeType);
        //        p.ExpectQty = changeQty;
        //        asnDtl.SecondUnitId = secondUnit.UnitId;
        //        asnDtl.Numerator = secondUnit.Numerator;
        //        asnDtl.Denominator = secondUnit.Denominator;
        //        asnDtl.ConvertFigre = secondUnit.GetConvertFigre();
        //    }
        //    else
        //    {
        //        asnDtl.SecondUnitId = item.UnitId;
        //        asnDtl.Numerator = 1;
        //        asnDtl.Denominator = 1;
        //        asnDtl.ConvertFigre = 1;
        //    }
        //    asnDtl.ExpectQty = p.ExpectQty;
        //}

        /// <summary>
        /// 保存库存调拨数据
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public virtual List<ErpErrorData> SaveSaleReturn(List<EbsToSmomAsnDetailData> datas)
        {
            var errors = new List<ErpErrorData>();
            if (datas.Count == 0)
                return errors;
            var tranSca = RT.Service.Resolve<TransactionController>().GetDefaultTransactions(OrderType.SaleReturn);
            if (tranSca == null)
                throw new ValidationException("销售退货没有设置单据小类".L10N());
            var func = RT.Service.Resolve<TransactionController>().GetFunctionByType(OrderType.SaleReturn);
            bool isIqc = func?.IsQc == true;
            var customerCodes = datas.Select(a => a.CustomerCode).Distinct().ToList();
            var cusDics = RT.Service.Resolve<CustomerController>().GetCustomers(customerCodes).ToDictionary(p => p.Code, p => p.Id);

            var itemCodes = datas.Select(a => a.ItemCode).Distinct().ToList();
            var itemDics = RT.Service.Resolve<ItemController>().GetItems(itemCodes, new EagerLoadOptions().LoadWithViewProperty()).ToDictionary(p => p.Code, p => p);
            var itemIds = itemDics.Select(a => a.Value.Id).ToList();
            //var itemStocks = new ItemStockDataHandle(itemIds);
            //var batchItemIds = itemStocks.GetIsBatchItems();
            var pkgDtls = RT.Service.Resolve<PackageController>().GetDefaultItemPackageRuleDtlByItemIds(itemIds);
            if (!pkgDtls.Any())
                throw new ValidationException("没有维护物料包装规则".L10N());
            var secondUnits = RT.Service.Resolve<ItemUnitController>().GetAllItemUnits(itemIds);
            var erpNos = datas.Select(a => a.OrderNumber).Distinct().ToList();
            //var asns = erpNos.SplitContains(sons =>
            // {
            //     return DB.Query<Asn>().Where(f => sons.Contains(f.RequireNo) && f.AsnSource == AsnSource.ErpWay && (f.AsnState == AsnState.Audit || f.AsnState == AsnState.PartCollect)).ToList();
            // });

            using (var trans = DB.TransactionScope(InterfaceEntityDataProvider.ConnectionStringName))
            { trans.Complete(); }

            datas.Where(f => f.LineState == 0 && f.OrderNumber.IsNotEmpty() &&
                (f.ErpSplitFromDetailId.IsNullOrEmpty() || f.ErpSplitFromDetailId.IsNotEmpty() && f.ErpSplitFromState == 0)//这个条件是这行明细是需要新增的
                ).GroupBy(f => f.OrderNumber).ForEach(q =>
          {
              //Asn asn;
              var data = q.First();
              if (data.CustomerCode.IsNullOrEmpty())
                  throw new ValidationException("客户编码不能为空".L10nFormat(data.CustomerCode));
              if (!data.CustomerCode.IsNullOrEmpty() && !cusDics.ContainsKey(data.CustomerCode))
                  throw new ValidationException("客户编码{0}不存在".L10nFormat(data.CustomerCode));
              double cusId = cusDics.GetValue<double>(data.CustomerCode);
              //asn = asns.FirstOrDefault(a => a.SourceKey == q.Key);
              //if (asn == null)//不存在则新增
              //{
              //    asn = SetAsn(data, cusId, tranSca.Id);
              //}
              //else
              //{
              //    asn.CustomerId = cusId;
              //}

              //var lineNo = asn.AsnDetailList.Any() ? asn.AsnDetailList.Max(a => int.Parse(a.LineNo)) + 1 : 1;
              q.ForEach(p =>
              {//只要可发货的明细
                  try
                  {
                      bool isCreateOrUpdateData = true;//是否创建或更新数据
                      if (p.ExpectQty <= 0)
                          throw new ValidationException("数量必须大于0".L10N());

                      if (!itemDics.ContainsKey(p.ItemCode))
                          throw new ValidationException("物料{0}不存在".L10nFormat(p.ItemCode));

                      //var asnDtl = asn.AsnDetailList.FirstOrDefault(a => a.ErpDetailId == p.ErpDetailId);

                      //if (asnDtl == null)//当前下载的数据不存在
                      //{
                      //    asnDtl = new AsnDetail();
                      //}
                      //else
                      //{//当前下载的数据已经存在，判断是否要做更新
                      //    if (asnDtl.AsnState != AsnState.Audit)
                      //    {
                      //        errors.Add(new ErpErrorData() { ErrMsg = "单据{0}行号{1}ERP主键{2},状态已经变更，不再更新数据".L10nFormat(p.OrderNumber, p.ErpLineNo, p.ErpDetailId), Infkey = p.ErpDetailId });
                      //        isCreateOrUpdateData = false;
                      //    }
                      //}
                      var item = itemDics.GetValue<Item>(p.ItemCode);

                      if (isCreateOrUpdateData)
                      {

                          //SetAsnDetail(asnDtl, p, item, isIqc, batchItemIds.ContainsKey(item.Id), pkgDtls, secondUnits);
                          //if (asnDtl.PersistenceStatus == PersistenceStatus.New)
                          //{
                          //    asnDtl.LineNo = lineNo.ToString();
                          //    lineNo++;
                          //    asnDtl.CreateDate = p.CreateDate;
                          //    asnDtl.GenerateId();
                          //}

                          //asn.AsnDetailList.Add(asnDtl);
                          errors.Add(new ErpErrorData() { ErrMsg = "", Infkey = p.ErpDetailId, IsSuccess = true });
                      }

                  }
                  catch (Exception ex)
                  {
                      errors.Add(new ErpErrorData() { ErrMsg = ex.Message, Infkey = p.ErpDetailId });
                  }
              });
              //asns.Add(asn);
          });
            //RF.Save(asns);
            //SetErpSplitData(asns, datas, errors, itemDics, isIqc, batchItemIds, pkgDtls, secondUnits, cusDics, tranSca.Id);
            SetCancelData(datas, errors);

            return errors;
        }

        /// <summary>
        /// Erp拆分的数据
        /// </summary>       
        //private void SetErpSplitData(EntityList<Asn> asns, List<EbsToSmomAsnDetailData> datas, List<ErpErrorData> errors, Dictionary<string, Item> itemDics, bool isIqc,
        //    Dictionary<double, double> batchItemIds, EntityList<ItemPackageRuleDetail> pkgDtls, EntityList<Items.ItemUnit> secondUnits, Dictionary<string, double> cusDics, double tranId)
        //{
        //    var coverDatas = datas.Where(f => f.LineState == 0 && f.ErpSplitFromDetailId.IsNotEmpty() && f.ErpSplitFromState == 1);
        //    if (coverDatas.Any())
        //    { //ERP拆分了明细，需要覆盖原来的明细的ErpDetailId
        //      //①已存在拆分后的ERP明细Id数据，直接更新，同时存在来源Id的数据，来源Id的数据更新为关闭
        //      //②不存在拆分后的ERP明细Id,
        //      //1.找不到原来的明细，需要直接创建新的明细，找到ASN，ASN单状态是审核，部分发货-创建明细。否则创建新的单
        //      //2.找到原来的明细，覆盖Id
        //        var fromDtlIds = coverDatas.Select(f => f.ErpSplitFromDetailId).Distinct().ToList();
        //        var erpDtlIds = coverDatas.Select(f => f.ErpDetailId).Distinct().ToList();
        //        erpDtlIds.AddRange(fromDtlIds);
        //        var asnDtls = erpDtlIds.SplitContains(sons =>
        //        {
        //            return DB.Query<AsnDetail>().Where(f => f.Asn.AsnSource == AsnSource.ErpWay && sons.Contains(f.ErpDetailId)).ToList();
        //        });
        //        var existErpIds = asnDtls.Select(a => a.ErpDetailId).ToList();
        //        //执行①
        //        coverDatas.Where(f => existErpIds.Contains(f.ErpDetailId)).ForEach(p =>
        //        {
        //            try
        //            {
        //                var asnDtl = asnDtls.FirstOrDefault(a => a.ErpDetailId == p.ErpDetailId);
        //                if (asnDtl.AsnState != AsnState.Audit)
        //                {
        //                    errors.Add(new ErpErrorData() { ErrMsg = "取消失败:SMOM明细行状态已发生变更{0}行号{1}ERP主键{2}".L10nFormat(p.OrderNumber, p.ErpLineNo, p.ErpDetailId), Infkey = p.ErpDetailId });
        //                }
        //                else
        //                {
        //                    var item = itemDics.GetValue<Item>(p.ItemCode);
        //                    asnDtl.Remark = p.Remark;
        //                    if (item.UnitCode.ToUpper() != p.UnitCode.ToUpper())
        //                    {//不是物料的主单位，找是否有辅助单位对应
        //                        var secondUnit = secondUnits.FirstOrDefault(a => a.UnitCode.ToUpper() == p.UnitCode.ToUpper() && a.MainUnitId == item.UnitId && (a.ItemId == item.Id || a.IsBaseUnit));
        //                        if (secondUnit == null)
        //                            throw new ValidationException("ERP物料{0}单位{1}跟MOM的单位{2}不一致，而且在单位转换中找不到记录".L10nFormat(item.Code, p.UnitCode, item.UnitName));
        //                        var den = ((decimal)secondUnit.Numerator / (decimal)secondUnit.Denominator);
        //                        var changeQty = RT.Service.Resolve<ItemUnitController>().TrancateTradeQty(p.ExpectQty / den, item.UnitPrecision, item.UnitTradeType);
        //                        p.ExpectQty = changeQty;
        //                        asnDtl.SecondUnitId = secondUnit.UnitId;
        //                        asnDtl.Numerator = secondUnit.Numerator;
        //                        asnDtl.Denominator = secondUnit.Denominator;
        //                        asnDtl.ConvertFigre = secondUnit.GetConvertFigre();
        //                    }
        //                    else
        //                    {
        //                        asnDtl.SecondUnitId = item.UnitId;
        //                        asnDtl.Numerator = 1;
        //                        asnDtl.Denominator = 1;
        //                        asnDtl.ConvertFigre = 1;
        //                    }
        //                    asnDtl.ExpectQty = p.ExpectQty;
        //                    RF.Save(asnDtl);
        //                    errors.Add(new ErpErrorData() { ErrMsg = "", Infkey = p.ErpDetailId, IsSuccess = true });
        //                }
        //                var closeDtl = asnDtls.FirstOrDefault(a => a.ErpDetailId == p.ErpSplitFromDetailId);
        //                if (closeDtl != null && closeDtl.AsnState != AsnState.Cancel && closeDtl.AsnState != AsnState.Grounding)
        //                {
        //                    closeDtl.AsnState = AsnState.Cancel;
        //                    closeDtl.Remark = "ERP拆分取消||" + closeDtl.Remark;
        //                    RF.Save(closeDtl);                           
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                errors.Add(new ErpErrorData() { ErrMsg = ex.Message, Infkey = p.ErpDetailId });
        //            }
        //        });

        //        //执行②
        //        coverDatas.Where(f => !existErpIds.Contains(f.ErpDetailId)).GroupBy(f => f.OrderNumber).ForEach(q =>
        //        {
        //            int lineNo = 1;
        //            var asn = asns.FirstOrDefault(a => a.RequireNo == q.Key);
        //            if (asn != null)
        //            {
        //                lineNo = asn.AsnDetailList.Any() ? asn.AsnDetailList.Max(a => int.Parse(a.LineNo)) + 1 : 1;
        //            }
        //            q.ForEach(p =>
        //            {
        //                try
        //                {
        //                    var fromDtl = asnDtls.FirstOrDefault(a => a.ErpDetailId == p.ErpSplitFromDetailId);
        //                    if (fromDtl != null && fromDtl.AsnState != AsnState.Cancel)
        //                    {//覆盖Id逻辑
        //                        if (fromDtl.AsnState == AsnState.Grounding)
        //                        {
        //                            errors.Add(new ErpErrorData() { ErrMsg = "拆分来源行在SMOM明细行状态已完成入库【单号{0}行号{1}ERP主键{2}】".L10nFormat(p.OrderNumber, p.ErpLineNo, p.ErpDetailId), Infkey = p.ErpDetailId });
        //                        }
        //                        else
        //                        {
        //                            fromDtl.ErpDetailId = p.ErpDetailId;
        //                            fromDtl.OrderLineNo = p.ErpLineNo;
        //                            RF.Save(fromDtl);
        //                            errors.Add(new ErpErrorData() { ErrMsg = "", Infkey = p.ErpDetailId, IsSuccess = true });
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (p.ExpectQty <= 0)
        //                            errors.Add(new ErpErrorData() { ErrMsg = "数量必须大于0【单号{0}行号{1}ERP主键{2}】".L10nFormat(p.OrderNumber, p.ErpLineNo, p.ErpDetailId), Infkey = p.ErpDetailId });
        //                        else if (!itemDics.ContainsKey(p.ItemCode))
        //                            errors.Add(new ErpErrorData() { ErrMsg = "物料不存在【单号{0}行号{1}ERP主键{2}】".L10nFormat(p.OrderNumber, p.ErpLineNo, p.ErpDetailId), Infkey = p.ErpDetailId });
        //                        else
        //                        {
        //                            AsnDetail asnDtl = new AsnDetail();
        //                            var item = itemDics.GetValue<Item>(p.ItemCode);

        //                            SetAsnDetail(asnDtl, p, item, isIqc, batchItemIds.ContainsKey(item.Id), pkgDtls, secondUnits);
        //                            if (asnDtl.PersistenceStatus == PersistenceStatus.New)
        //                            {
        //                                asnDtl.LineNo = lineNo.ToString();
        //                                lineNo++;
        //                                asnDtl.CreateDate = p.CreateDate;
        //                                asnDtl.GenerateId();
        //                            }

        //                            if (asn != null)
        //                            {
        //                                asn.AsnDetailList.Add(asnDtl);
        //                            }
        //                            else
        //                            {
        //                                if (p.CustomerCode.IsNullOrEmpty())
        //                                    throw new ValidationException("客户编码不能为空".L10nFormat(p.CustomerCode));
        //                                if (!p.CustomerCode.IsNullOrEmpty() && !cusDics.ContainsKey(p.CustomerCode))
        //                                    throw new ValidationException("客户编码{0}不存在".L10nFormat(p.CustomerCode));
        //                                asn = SetAsn(p, cusDics.GetValue<double>(p.CustomerCode), tranId);
        //                            }
        //                            RF.Save(asn);
        //                            errors.Add(new ErpErrorData() { ErrMsg = "", Infkey = p.ErpDetailId, IsSuccess = true });
        //                        }
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    errors.Add(new ErpErrorData() { ErrMsg = ex.Message, Infkey = p.ErpDetailId });
        //                }
        //            });
        //        });
        //        RF.Save(asns);
        //    }
        //}

        /// <summary>
        /// 设置取消的数据
        /// </summary>      
        private void SetCancelData(List<EbsToSmomAsnDetailData> datas, List<ErpErrorData> errors)
        {
            if (datas.Any(f => f.LineState == 2))
            {
                var lineIds = datas.Where(f => f.LineState == 2).Select(a => a.ErpDetailId).Distinct().ToList();
                //var dtls = DB.Query<AsnDetail>().Where(f => lineIds.Contains(f.ErpDetailId)).ToList();
                datas.Where(f => f.LineState == 2).ForEach(p =>
                {//只取消审核状态的单据
                    //var existsDtl = dtls.Where(a => a.ErpDetailId == p.ErpDetailId && a.AsnState == AsnState.Audit);
                    //if (existsDtl.Any())
                    //{
                    //    existsDtl.ForEach(f =>
                    //    {
                    //        f.AsnState = AsnState.Cancel;
                    //        f.Remark = "ERP取消||" + f.Remark;
                    //        RF.Save(f);
                    //    });
                    //    errors.Add(new ErpErrorData() { ErrMsg = "", Infkey = p.ErpDetailId, IsSuccess = true });
                    //}
                    //else
                    //{
                    //    if (dtls.Any(a => a.ErpDetailId == p.ErpDetailId && a.AsnState != AsnState.Audit && a.AsnState != AsnState.Cancel))
                    //        errors.Add(new ErpErrorData() { ErrMsg = "取消失败:SMOM明细行状态已发生变更{0}行号{1}ERP主键{2}".L10nFormat(p.OrderNumber, p.ErpLineNo, p.ErpDetailId), Infkey = p.ErpDetailId });
                    //}

                });
            }
        }
        #endregion
    }
}
