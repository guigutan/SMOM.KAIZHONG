using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using SIE.Core.ApiModels;
using SIE.Domain;
using SIE.EventMessages.LES;
using SIE.EventMessages.LES.Datas;
using SIE.Items;
using SIE.LES.MaterialPreparations.ApiModels;
using SIE.LES.MaterialPreparations.Enums;
using SIE.LES.MaterialReceives;
using SIE.LES.MaterialReturnApplys.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.LES.MaterialPreparations
{
    /// <summary>
    /// 备料需求单控制器接口
    /// </summary>
    public partial class MaterialPreparationController : DomainController, ILesMaterialPrepare
    {
        /// <summary>
        /// 更新备料明细发运
        /// </summary>
        /// <param name="shippingUpdateDatas"></param>
        public virtual void ShippingUpdateMaterialPre(List<ShippingUpdateData> shippingUpdateDatas)
        {
            using (SIE.Common.InvOrg.InvOrgs.With(new List<double>() { shippingUpdateDatas.First().InvOrgId }))
            {
                // 物料编码
                var itemCodes = shippingUpdateDatas.Select(p => p.ItemCode).ToList();
                List<BaseDataInfo> itemInfos = new List<BaseDataInfo>();
                itemCodes.SplitDataExecute(temps =>
                {
                    var list = Query<Item>().Where(p => temps.Contains(p.Code))
                    .Select(p => new
                    {
                        Id = p.Id,
                        Code = p.Code,
                    }).ToList<BaseDataInfo>();
                    itemInfos.AddRange(list);
                });
                Dictionary<string, double> itemCodeIdDic = itemInfos.ToDictionary(p => p.Code, p => p.Id);

                // 备料明细
                var materialPreNos = shippingUpdateDatas.Select(p => p.MaterialPreNo).ToList();
                List<MaterialPreDetailInfo> detailInfos = new List<MaterialPreDetailInfo>();
                materialPreNos.SplitDataExecute(temps =>
                {
                    var list = Query<MaterialPreparationDetail>().LeftJoin<MaterialPreparation>((mpd, mp) => mpd.MaterialPreparationId == mp.Id)
                    .Where<MaterialPreparation>((mpd, mp) => temps.Contains(mp.No))
                    .Select<MaterialPreparation>((mpd, mp) => new
                    {
                        Id = mpd.Id,
                        LineNo = mpd.LineNo,
                        MaterialPreId = mp.Id,
                        MaterialPreNo = mp.No,
                        ItemId = mpd.ItemId,
                        ItemExtProp = mpd.ItemExtProp,
                        Qty = mpd.Qty,
                        RefuseQty = mpd.RefuseQty,
                        ReceiveQty = mpd.ReceiveQty,
                    }).ToList<MaterialPreDetailInfo>();
                    detailInfos.AddRange(list);
                });
                // key: 备料单号.行号,物料编码,扩展属性 value: 明细
                Dictionary<Tuple<string, string, double, string>, MaterialPreDetailInfo> detailInfoDic = detailInfos.ToDictionary(p => new Tuple<string, string, double, string>(p.MaterialPreNo, p.LineNo, p.ItemId, p.ItemExtProp), p => p);


                foreach (var data in shippingUpdateDatas)
                {
                    var hasItem = itemCodeIdDic.TryGetValue(data.ItemCode, out double itemId);
                    var hasDetail = detailInfoDic.TryGetValue(new Tuple<string, string, double, string>(data.MaterialPreNo, data.LineNo, itemId, data.ItemExtProp), out MaterialPreDetailInfo detail);
                    if (hasDetail && hasItem)
                    {
                        var updateSql = DB.Update<MaterialPreparationDetail>()
                            .Set(p => p.ShippingQty, p => p.ShippingQty + data.ShippingQty)
                            .Where(p => p.Id == detail.Id);
                        if (detail.RefuseQty + detail.ReceiveQty == 0) // 接收+拒收为0需要更新状态为待接收
                        {
                            updateSql.Set(p => p.PreDetailStatus, Enums.PrepareDetailStatus.ToReceive);
                        }
                        updateSql.Execute();
                    }
                }
            }
        }

        /// <summary>
        /// 更新备料单数量
        /// </summary>
        /// <param name="materialPreNo"></param>
        /// <param name="LineNos"></param>
        public virtual void UpdateMaterialPreQty(string materialPreNo, List<string> LineNos)
        {
            var materialPre = Query<MaterialPreparation>().Where(p => p.No == materialPreNo).FirstOrDefault();
            if (materialPre == null)
                return;
            var receiveDtls = Query<MaterialReceiveDetail>().Where(p => p.MaterialReceive.MaterialPreparation.No == materialPreNo && LineNos.Contains(p.SoLineNo)).ToList();

            foreach (var dtl in materialPre.DetailList.Where(p => LineNos.Contains(p.LineNo)))
            {
                var shippingQty = receiveDtls.Where(p => p.SoLineNo == dtl.LineNo).Sum(p => p.IssuedQty);  //累计发料数
                var recQty = receiveDtls.Where(p => p.SoLineNo == dtl.LineNo && p.State != ReceiveState.TobeReceived).Sum(p => p.ReceivedQty);  //累计接收数
                var refuseQty = receiveDtls.Where(p => p.SoLineNo == dtl.LineNo && p.State != ReceiveState.TobeReceived).Sum(p => p.IssuedQty - p.ReceivedQty);    //累计拒收数

                dtl.ShippingQty = shippingQty;
                dtl.ReceiveQty = recQty;
                dtl.RefuseQty = refuseQty;
                SetPreparationDetailStatus(dtl);
                DB.Update<MaterialPreparationDetail>()
                    .Set(p => p.ShippingQty, dtl.ShippingQty)
                    .Set(p => p.ReceiveQty, dtl.ReceiveQty)
                    .Set(p => p.RefuseQty, dtl.RefuseQty)
                    .Set(p => p.PreDetailStatus, dtl.PreDetailStatus)
                    .Where(p => p.Id == dtl.Id).Execute();

            }
        }

        /// <summary>
        /// WSM创建发运订单更新备料需求单Asn单号
        /// </summary>
        /// <param name="shippingUpdateSoNoDatas">更新信息</param>
        public virtual void ShippingUpdateSourceSo(List<ShippingUpdateSoNoData> shippingUpdateSoNoDatas)
        {
            using (SIE.Common.InvOrg.InvOrgs.With(new List<double>() { shippingUpdateSoNoDatas.First().InvOrgId }))
            {
                // 来源类型为备料类型
                var preNoList = shippingUpdateSoNoDatas.Where(p => p.SourceType == 1).Select(p => p.SourceNo).Distinct().ToList();
                List<BaseDataInfo> preNoIdList = new List<BaseDataInfo>();
                preNoList.SplitDataExecute(tempNos =>
                {
                    var list = Query<MaterialPreparation>().Where(p => tempNos.Contains(p.No))
                    .Select(p => new
                    {
                        Id = p.Id,
                        Code = p.No,
                    }).ToList<BaseDataInfo>();
                    preNoIdList.AddRange(list);
                });
                var preNoIdDic = preNoIdList.ToDictionary(p => p.Code, p => p.Id); // 备料需求单 单号 id 映射
                var preDic = shippingUpdateSoNoDatas.Where(p => p.SourceType == 1).GroupBy(p => p.SourceNo).ToDictionary(p => p.Key, p => p.ToList());
                foreach (var key in preDic.Keys)
                {
                    // id
                    if (!preNoIdDic.TryGetValue(key, out var id)) continue;
                    var datas = preDic[key];
                    var shippingOrderNo = string.Join(";", datas.Select(p => p.ShippingOrderNo).Distinct().ToList());
                    DB.Update<MaterialPreparation>().Set(p => p.ShippingOrderNo, shippingOrderNo).Where(p => p.Id == id).Execute();
                    foreach (var data in datas)
                    {
                        DB.Update<MaterialPreparationDetail>().Set(p => p.ShippingDetailNo, data.ShippingOrderNo).Where(p => p.MaterialPreparationId == id && p.LineNo == data.LineNo).Execute();
                    }
                }
            }
        }

        /// <summary>
        /// WMS关闭发运订单回写备料需求单取消数并更新状态
        /// </summary>
        /// <param name="shippingCloseDatas"></param>
        public virtual void ShippingCloseMaterialPre(List<ShippingUpdateData> shippingCloseDatas)
        {
            using (SIE.Common.InvOrg.InvOrgs.With(new List<double>() { shippingCloseDatas.First().InvOrgId }))
            {
                // 物料编码
                var itemCodes = shippingCloseDatas.Select(p => p.ItemCode).ToList();
                List<BaseDataInfo> itemInfos = new List<BaseDataInfo>();
                itemCodes.SplitDataExecute(temps =>
                {
                    var list = Query<Item>().Where(p => temps.Contains(p.Code))
                    .Select(p => new
                    {
                        Id = p.Id,
                        Code = p.Code,
                    }).ToList<BaseDataInfo>();
                    itemInfos.AddRange(list);
                });
                Dictionary<string, double> itemCodeIdDic = itemInfos.ToDictionary(p => p.Code, p => p.Id);

                // 备料明细
                var materialPreNos = shippingCloseDatas.Select(p => p.MaterialPreNo).ToList();
                List<MaterialPreDetailInfo> detailInfos = new List<MaterialPreDetailInfo>();
                materialPreNos.SplitDataExecute(temps =>
                {
                    var list = Query<MaterialPreparationDetail>().LeftJoin<MaterialPreparation>((mpd, mp) => mpd.MaterialPreparationId == mp.Id)
                    .Where<MaterialPreparation>((mpd, mp) => temps.Contains(mp.No))
                    .Select<MaterialPreparation>((mpd, mp) => new
                    {
                        Id = mpd.Id,
                        LineNo = mpd.LineNo,
                        MaterialPreId = mp.Id,
                        MaterialPreNo = mp.No,
                        ItemId = mpd.ItemId,
                        ItemExtProp = mpd.ItemExtProp,
                        PreDetailStatus = mpd.PreDetailStatus,
                        Qty = mpd.Qty,
                        ShippingQty = mpd.ShippingQty,
                        CancelQty = mpd.CancelQty,
                        RefuseQty = mpd.RefuseQty,
                        ReceiveQty = mpd.ReceiveQty,
                    }).ToList<MaterialPreDetailInfo>();
                    detailInfos.AddRange(list);
                });
                // key: 备料单号.行号,物料编码,扩展属性 value: 明细
                Dictionary<Tuple<string, string, double, string>, MaterialPreDetailInfo> detailInfoDic = detailInfos.ToDictionary(p => new Tuple<string, string, double, string>(p.MaterialPreNo, p.LineNo, p.ItemId, p.ItemExtProp), p => p);

                foreach (var data in shippingCloseDatas)
                {
                    var hasItem = itemCodeIdDic.TryGetValue(data.ItemCode, out double itemId);
                    var hasDetail = detailInfoDic.TryGetValue(new Tuple<string, string, double, string>(data.MaterialPreNo, data.LineNo, itemId, data.ItemExtProp), out MaterialPreDetailInfo detail);
                    if (hasDetail && hasItem)
                    {
                        // 更新取消数
                        detail.CancelQty += data.CancelQty;
                        // 更新明细状态
                        var mpDetail = new MaterialPreparationDetail { Qty = detail.Qty, CancelQty = detail.CancelQty, ShippingQty = detail.ShippingQty, RefuseQty = detail.ShippingQty, ReceiveQty = detail.ReceiveQty };
                        SetPreparationDetailStatus(mpDetail);
                        DB.Update<MaterialPreparationDetail>()
                            .Set(p => p.CancelQty, detail.CancelQty)
                            .Set(p => p.PreDetailStatus, mpDetail.PreDetailStatus)
                            .Where(p => p.Id == detail.Id).Execute();

                    }
                }
            }
        }
    }
}
