using System;
using System.Collections.Generic;
using System.Linq;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.Items;

namespace SIE.ERPInterface.Smom.Download
{
    /// <summary>
    /// 采购订单明细下载控制
    /// </summary>
    public class DownloadPoDtlController : DomainController
    {
        /// <summary>
        /// 从API下载采购订单明细到业务表
        /// </summary>
        /// <param name="poDtlDatas"></param>
        /// <param name="invOrg"></param>
        /// <returns></returns>
        public virtual ApiResult DownloadPoDtlToBusiness(List<PoDetailData> poDtlDatas, int invOrg)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.ApiSaveBusinessData<PoDetailData>(
                poDtlDatas,
                p => this.SavePurchaseOrderDetails(p.OrderByLastUpdateDate()),
                JobType.PurchaseOrderDetail,
                invOrg);
        }

        /// <summary>
        /// 从中间表下载采购订单明细到业务表
        /// </summary>
        public virtual ProcessResult DownloadPoDtlInfToBusiness(bool isManual = false)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.SaveBusinessData<PurchaseOrderDetailInf>(
                () => ctl.GetUnprocessedDatas<PurchaseOrderDetailInf>(),           //采购订单明细中间表数据
                 p =>
                 {
                     var paras = this.GeneratePoDtlPara(p);
                     return this.SavePurchaseOrderDetails(paras.OrderByLastUpdateDate());
                 },
                JobType.PurchaseOrderDetail, isManual);
        }

        /// <summary>
        /// 生成采购订单明细实体
        /// </summary>
        /// <param name="poDtlInfs">中间表实体数据</param>
        /// <returns></returns>
        public virtual List<PoDetailData> GeneratePoDtlPara(IEnumerable<PurchaseOrderDetailInf> poDtlInfs)
        {
            var paras = new List<PoDetailData>();

            poDtlInfs.ForEach(p =>
            {
                var data = new PoDetailData();
                data.LastUpdateDate = p.LastUpdateDate.HasValue ? p.LastUpdateDate.Value : DateTime.Now;
                data.IsDelete = p.IsDelete;
                data.Infkey = p.Id;
                data.PoNo = p.PoNo;
                data.LineNo = p.LineNo;
                data.ItemCode = p.ItemCode;
                data.Quantity = p.Quantity;
                data.UnitPrice = p.UnitPrice;
                data.DeliveryDate = p.DeliveryDate.ToString();
                data.UnitCode = p.PurchaseUnit;
                data.ErpKey = p.ErpKey;

                paras.Add(data);
            });

            return paras;
        }

        /// <summary>
        /// 执行数据保存
        /// </summary>
        /// <param name="data">数据实体</param>
        /// <param name="dic">数据字典</param>
        /// <param name="dicItem">数据字典</param>
        /// <param name="po">PO</param>
        //public virtual void SavePurchaseOrderDetail(PoDetailData data, Dictionary<string, PurchaseOrderDetail> dic,
        //    Dictionary<string, Item> dicItem, PurchaseOrder po, string lineNo, Dictionary<string, double> units, EntityList<SIE.Items.ItemUnit> itemUnits)
        //{
        //    var ctl = RT.Service.Resolve<DownloadBusBaseController>();

        //    PurchaseOrderDetail poDetail;
        //    var key = po.No + "_" + lineNo;
        //    if (key.IsNullOrEmpty())
        //        throw new ValidationException("PO[{0}]的明细行号为空".L10nFormat(po.No));
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
        //        dic.Add(key, new PurchaseOrderDetail());
        //    poDetail = dic[key];

        //    if (data.ItemCode.IsNullOrEmpty() || !dicItem.ContainsKey(data.ItemCode))
        //        throw new ValidationException("PO[{1}]的明细行[{2}]物料编码[{0}]不存在".L10nFormat(data.ItemCode, po.No, data.LineNo));

        //    poDetail.LineNo = data.LineNo;
        //    poDetail.Item = dicItem[data.ItemCode];
        //    poDetail.Quantity = data.Quantity;
        //    poDetail.UnitPrice = data.UnitPrice;
        //    DateTime dt;
        //    if (!DateTime.TryParse(data.DeliveryDate, out dt))
        //        throw new ValidationException("交货日期不正确[字段DeliveryDate]".L10N());
        //    poDetail.DeliveryDate = dt;
        //    poDetail.NotReceiveQty = data.Quantity;
        //    poDetail.ReceiveQty = 0m;
        //    poDetail.DeliveredQty = 0m;
        //    poDetail.UnDeliveredQty = data.Quantity;
        //    poDetail.SourceKey = data.ErpKey;
        //    poDetail.Numerator = 1;
        //    poDetail.Denominator = 1;
        //    poDetail.ConvertFigre = 1;
        //    if (units.TryGetValue(data.UnitCode, out double secUnitId))
        //    {
        //        poDetail.SecondUnitId = secUnitId;
        //        var itemUnit = itemUnits.FirstOrDefault(a => a.UnitId == secUnitId && a.ItemId == poDetail.ItemId);
        //        if (itemUnit != null)
        //        {
        //            poDetail.Numerator = itemUnit.Numerator;
        //            poDetail.Denominator = itemUnit.Denominator;
        //            poDetail.ConvertFigre = (decimal)itemUnit.Numerator / itemUnit.Denominator;
        //        }
        //    }
        //    else
        //    {
        //        poDetail.SecondUnitId = poDetail.Item.UnitId.Value;

        //    }
        //    poDetail.PurchaseOrder = po;
        //    RF.Save(poDetail);
        //}

        /// <summary>
        /// 更新明细行
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public virtual List<ErpErrorData> SavePurchaseOrderDetails(List<PoDetailData> datas)
        {
            var errors = new List<ErpErrorData>();
            if (datas.Count == 0)
                return errors;

            //var ctl = RT.Service.Resolve<PurchaseOrderController>();

            //获取PO数据
            //var pos = ctl.GetPurchaseOrderDatas(datas.Select(p => p.PoNo).Distinct().ToList());
            //var dicPo = pos.ToDictionary(p => p.No);    //<PoNo,Po>

            //获取PO明细数据
            //var poDetails = ctl.GetPurchaseOrderDetailData(pos.Select(p => p.Id).ToList());
            //var dicPoDetails = poDetails.GroupBy(p => p.PurchaseOrder.No).ToDictionary(p => p.Key, p => p.ToList());    //<PoNo,Po明细列表>

            //物料字典数据
            var itemCodes = datas.Select(p => p.ItemCode).ToList();
            var items = RT.Service.Resolve<ItemController>().GetItems(itemCodes.Distinct().ToList());
            var dicItem = items.DistinctBy(p => p.Id).ToDictionary(p => p.Code);
            var unitCodes = datas.Select(a => a.UnitCode).Distinct().ToList();
            var units = RT.Service.Resolve<ItemController>().GetUnitList(unitCodes).ToDictionary(p => p.Code, p => p.Id);
            var unitIds = units.Select(a => a.Value).ToList();
            var itemUnits = RT.Service.Resolve<ItemUnitController>().GetItemUnits(unitIds);
            datas.GroupBy(f => f.PoNo).ForEach(f =>
            {
                int lineNo = 0;
                //if (poDetails.Count > 0)
                //    lineNo = poDetails.Where(a => a.PurchaseOrderNo == f.Key).Max(a => int.Parse(a.LineNo));
                //按顺序处理数据
                foreach (var data in f)
                {
                    try
                    {
                        lineNo++;
                        var key = data.PoNo;  //产品PoNo编码为主键
                        //if (!dicPo.ContainsKey(key))
                        //    throw new ValidationException("PO[{0}]不存在".L10nFormat(key));
                        //var po = dicPo[key];
                        //if (!dicPoDetails.ContainsKey(key))
                        //    dicPoDetails.Add(key, new List<PurchaseOrderDetail>());
                        //var dicDetail = dicPoDetails[key].ToDictionary(p => p.PurchaseOrderNo + "_" + p.LineNo);

                        //SavePurchaseOrderDetail(data, dicDetail, dicItem, po, lineNo.ToString(), units, itemUnits);
                    }
                    catch (Exception ex)
                    {
                        errors.Add(new ErpErrorData() { ErrMsg = ex.Message, Infkey = data.Infkey });
                    }
                }
            });

            return errors;
        }
    }
}
