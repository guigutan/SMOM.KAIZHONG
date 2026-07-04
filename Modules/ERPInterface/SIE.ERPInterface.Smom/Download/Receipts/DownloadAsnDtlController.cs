using System;
using System.Collections.Generic;
using System.Linq;
using SIE.Common.Configs;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.Items;
using SIE.Packages;
using SIE.Warehouses;

namespace SIE.ERPInterface.Smom.Download
{
    /// <summary>
    /// ASN订单明细下载控制器
    /// </summary>
    public class DownloadAsnDtlController : DomainController
    {
        /// <summary>
        /// 从API下载ASN明细到业务表
        /// </summary>
        /// <param name="asnDtlDatas"></param>
        /// <param name="invOrg"></param>
        /// <returns></returns>
        public virtual ApiResult DownloadAsnDtlToBusiness(List<AsnDetailData> asnDtlDatas, int invOrg)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.ApiSaveBusinessData<AsnDetailData>(
                asnDtlDatas,
                p => this.SaveASNDetails(p.OrderByLastUpdateDate()),
                JobType.AsnDtl,
                invOrg);
        }

        /// <summary>
        /// 从中间表下载ASN明细到业务表
        /// </summary>
        public virtual ProcessResult DownloadAsnDtlInfToBusiness(bool isManual = false)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.SaveBusinessData<AsnDetailInf>(
                () => ctl.GetUnprocessedDatas<AsnDetailInf>(),             //ASN明细中间表数据
                 p =>
                 {
                     var paras = this.GenerateAsnDtlPara(p);
                     return this.SaveASNDetails(paras.OrderByLastUpdateDate());
                 },
                JobType.AsnDtl, isManual);
        }

        /// <summary>
        /// 生成ASN明细实体
        /// </summary>
        /// <param name="asnDtlInfs">中间表实体数据</param>
        /// <returns></returns>
        public virtual List<AsnDetailData> GenerateAsnDtlPara(IEnumerable<AsnDetailInf> asnDtlInfs)
        {
            var paras = new List<AsnDetailData>();

            asnDtlInfs.ForEach(p =>
            {
                var data = new AsnDetailData();
                data.LastUpdateDate = p.LastUpdateDate.HasValue ? p.LastUpdateDate.Value : DateTime.Now;
                data.IsDelete = p.IsDelete;
                data.Infkey = p.Id;
                data.AsnNo = p.AsnNo;
                data.ExpectQty = p.ExpectQty;
                data.OrderNo = p.OrderNo;
                data.Remark = p.Remark;
                data.ReceiveStorageLocationCode = p.ReceiveStorageLocation;
                data.ItemCode = p.ItemCode;             
                ////asnDtlData.AsnState = AsnState.Audit;
                data.Remark = p.Remark;
                data.PoNo = p.PoNo;
                data.PoDetailLineNo = p.PoLineNo;
                data.ErpKey = p.ErpKey;
                data.ErpId = double.Parse(p.ErpKey);

                paras.Add(data);
            });

            return paras;
        }


        /// <summary>
        /// 更新明细行
        /// </summary>
        /// <param name="datas">ASN数据列表</param>
        /// <returns>错误信息列表</returns>
        public virtual List<ErpErrorData> SaveASNDetails(List<AsnDetailData> datas)
        {
            var errors = new List<ErpErrorData>();
            if (datas.Count == 0)
                return errors;

            //var ctl = RT.Service.Resolve<AsnService>();

            #region 获取数据

            //获取Asn数据
            List<string> noList = datas.Select(p => p.AsnNo).Distinct().ToList();
            //var asnList = ctl.GetAsnDatas(noList);
            //var asnDict = asnList.ToDictionary(p => p.No, p => p);

            //获取明细数据
            //var asnDetails = ctl.GetAsnDetails(asnList.Select(p => p.Id).ToList(), new EagerLoadOptions().LoadWith(AsnDetail.AsnProperty));
            //var dicAsnDetails = asnDetails.GroupBy(p => p.Asn.No).ToDictionary(p => p.Key, p => p.ToList());    //<AsnNo,Asn明细列表>

            //获取物料数据
            var itemCodes = datas.Select(p => p.ItemCode).Distinct().ToList();
            var items = RT.Service.Resolve<ItemController>().GetItems(itemCodes);
            var dicItem = items.ToDictionary(p => p.Code, p => p);

            //获取库位
            var locationCodeList = datas.Select(p => p.ReceiveStorageLocationCode).Distinct().ToList();
            var locationList = RT.Service.Resolve<WarehouseController>().GetStorageLocations(locationCodeList);
            var dicLocation = locationList.ToDictionary(p => p.Code, p => p);

            //获取PO
            var poNoList = datas.Select(p => p.PoNo).Distinct().ToList();
            //var poList = RT.Service.Resolve<PO.PurchaseOrders.PurchaseOrderController>().GetPurchaseOrderDatas(poNoList);
            //var dicPo = poList.ToDictionary(p => p.No, p => p);

            //获取物料包装规则
            var itemIdList = dicItem.Select(p => p.Value.Id).Distinct().ToList();
            var dicPackDetail = RT.Service.Resolve<PackageController>().GetItemsMasterUnit(itemIdList);

            #endregion

            //按顺序处理数据
            datas.GroupBy(f => f.AsnNo).ForEach(f =>
            {
                //var asnDtls = RT.Service.Resolve<AsnService>().GetAsnDetailByAsnNo(f.Key);
                //var lineNo = asnDtls.Max(a => int.Parse(a.LineNo));
                //按顺序处理数据
                foreach (var data in f)
                {
                    try
                    {
                        var key = data.AsnNo;  //产品AsnNo编码为主键
                        //if (!asnDict.ContainsKey(key))
                        //    throw new ValidationException("ASN[{0}]不存在".L10nFormat(key));
                        //var asn = asnDict[key];
                        //if (!dicAsnDetails.ContainsKey(key))
                        //    dicAsnDetails.Add(key, new List<AsnDetail>());
                        //var dicDetail = dicAsnDetails[key].ToDictionary(p => p.Item.Code);
                        //lineNo++;
                        //SaveAsnDetail(data, dicDetail, dicItem, asn, dicLocation, dicPo, dicPackDetail, lineNo.ToString());
                    }
                    catch (Exception ex)
                    {
                        errors.Add(new ErpErrorData() { ErrMsg = ex.Message, Infkey = data.Infkey });
                    }
                }
            });

            return errors;
        }

        /// <summary>
        /// 执行数据保存
        /// </summary>
        /// <param name="data">数据实体</param>
        /// <param name="dic">数据字典</param>
        /// <param name="dicItem">数据字典</param>
        /// <param name="asn">Asn</param>
        /// <param name="dicLocation">数据字典</param>
        /// <param name="dicPo">数据字典</param>
        /// <param name="dicPackDetail">数据字典</param>
        /// <param name="lineNo">行号</param>
        //public virtual void SaveAsnDetail(AsnDetailData data,
        //    Dictionary<string, AsnDetail> dic,
        //    Dictionary<string, Item> dicItem,
        //    Asn asn,
        //    Dictionary<string, StorageLocation> dicLocation,
        //    Dictionary<string, PO.PurchaseOrders.PurchaseOrder> dicPo,
        //    Dictionary<double, ItemPackageRuleDetail> dicPackDetail, string lineNo)
        //{
        //    var ctl = RT.Service.Resolve<DownloadBusBaseController>();

        //    if (data.ItemCode.IsNullOrEmpty() || !dicItem.ContainsKey(data.ItemCode))
        //        throw new ValidationException("ASN[{1}]的明细行物料编码[{0}]不存在".L10nFormat(data.ItemCode, asn.No));
        //    var item = dicItem[data.ItemCode];

        //    AsnDetail asnDetail;
        //    var key = "{0}".FormatArgs(data.ItemCode); //物料+行号 为主键
        //    if (!dic.ContainsKey(key))
        //        dic.Add(key, new AsnDetail());
        //    asnDetail = dic[key];
        //    //处理待删除数据
        //    if (data.IsDelete)
        //    {
        //        ctl.DeleteEntity(dic, key, asnDetail);
        //        return;
        //    }

        //    if (data.ReceiveStorageLocationCode.IsNullOrEmpty() || !dicLocation.ContainsKey(data.ReceiveStorageLocationCode))
        //        throw new ValidationException("ASN[{1}]的明细行收货库位[{0}]不存在".L10nFormat(data.ReceiveStorageLocationCode, asn.No));

        //    if (!dicPackDetail.ContainsKey(item.Id))
        //        throw new ValidationException("ASN[{1}]的明细行物料编码[{0}]未维护包装规则明细".L10nFormat(data.ItemCode, asn.No));
        //    var itemPackDetail = dicPackDetail[item.Id];

        //    asnDetail.Asn = asn;
        //    asnDetail.LineNo = lineNo;
        //    asnDetail.Item = item;
        //    asnDetail.ItemId = item.Id;
        //    asnDetail.ExpectQty = data.ExpectQty;
        //    asnDetail.ReceiveStorageLocation = dicLocation[data.ReceiveStorageLocationCode];

        //    asnDetail.ItemPackageRuleDetail = itemPackDetail;
        //    asnDetail.ItemPackageRule = itemPackDetail.ItemPackageRule;

        //    asnDetail.Volume = asnDetail.ExpectQty * (item.Volume.HasValue ? item.Volume.Value : 0);
        //    asnDetail.Weight = asnDetail.ExpectQty * (item.Weight.HasValue ? item.Weight.Value : 0);
        //    asnDetail.OrderNo = data.OrderNo;

        //    if (asn.OrderType == OrderType.PurchaseIn)
        //    {
        //        if (data.PoNo.IsNullOrEmpty() || !dicPo.ContainsKey(data.PoNo))
        //            throw new ValidationException("ASN[{1}]的明细行PO[{0}]不存在".L10nFormat(data.PoNo, asn.No));
        //        asnDetail.PurchaseOrder = dicPo[data.PoNo];

        //        var poDetail = dicPo[data.PoNo].PurchaseOrderDetailList.FirstOrDefault(p => p.LineNo == data.PoDetailLineNo && p.Item.Code == data.ItemCode);
        //        if (poDetail == null)
        //            throw new ValidationException("ASN[{1}]的物料[{0}]没有匹配到相应的PO行信息".L10nFormat(data.ItemCode, asn.No));
        //        asnDetail.PurchaseOrderDetail = poDetail;

        //        asnDetail.ExpectQty = poDetail.UnDeliveredQty;
        //        if (poDetail.UnDeliveredQty == 0)
        //            throw new ValidationException("采购订单{0}行{1}未建单未收数=0，不能再用于创建ASN明细".L10nFormat(data.PoNo, data.PoDetailLineNo));

        //        asnDetail.OrderNo = poDetail.PurchaseOrder.No;
        //        asnDetail.Amount = asnDetail.ExpectQty * poDetail.UnitPrice;
        //        asnDetail.HasIsUpExpect = false;
        //        var config = ConfigService.GetConfig(new AsnParamConfig(), typeof(Asn));
        //        if (config != null && config.IsUpExpectQty)
        //        {
        //            asnDetail.HasIsUpExpect = config.IsUpExpectQty;
        //        }
        //        asnDetail.NotRecQty = poDetail.NotReceiveQty;
        //        asnDetail.UnbuiltQty = poDetail.UnDeliveredQty;
        //    }

        //    asnDetail.Lot = AsnDetail.LotProperty.Name;
        //    asnDetail.LotAtt01 = data.LotAtt01;
        //    asnDetail.LotAtt02 = data.LotAtt02;
        //    asnDetail.LotAtt04 = data.LotAtt04;
        //    asnDetail.LotAtt05 = data.LotAtt05;
        //    asnDetail.LotAtt06 = data.LotAtt06;
        //    asnDetail.LotAtt07 = data.LotAtt07;
        //    asnDetail.LotAtt08 = data.LotAtt08;
        //    asnDetail.LotAtt09 = data.LotAtt09;
        //    asnDetail.LotAtt10 = data.LotAtt10;
        //    asnDetail.LotAtt11 = data.LotAtt11;
        //    asnDetail.LotAtt12 = data.LotAtt12;
        //    asnDetail.Remark = data.Remark;
        //    asnDetail.SourceKey = data.ErpKey;

        //    RF.Save(asnDetail);
        //}
    }
}
