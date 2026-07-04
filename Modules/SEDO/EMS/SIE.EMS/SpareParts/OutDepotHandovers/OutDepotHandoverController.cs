using SIE.Domain;
using SIE.EMS.SpareParts.OutDepots.Details;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.SpareParts.OutDepotHandovers
{
    /// <summary>
    /// 备件交接单控制器
    /// </summary>
    public class OutDepotHandoverController : DomainController
    {
        /// <summary>
        /// 查询备件交接单
        /// </summary>
        /// <param name="criteria">备件交接单查询实体</param>
        /// <returns>备件交接单集合</returns>
        public virtual EntityList<OutDepotHandover> GetOutDepotHandoverList(OutDepotHandoverCriteria criteria)
        {
            var query = DB.Query<OutDepotHandover>();

            if (criteria.HandoverNo.IsNotEmpty())
            {
                query.Where(p => p.HandoverNo.Contains(criteria.HandoverNo));
            }
            if (criteria.OutDepotNo.IsNotEmpty())
            {
                query.Where(p => p.OutDepot.No.Contains(criteria.OutDepotNo));
            }
            if (criteria.HandOverStatus != null)
            {
                query.Where(p => p.HandOverStatus == criteria.HandOverStatus);
            }

            if (criteria.SparePart != null || criteria.SparePartName.IsNotEmpty() || criteria.SeriaNo.IsNotEmpty() || criteria.BatchNo.IsNotEmpty())
            {
                query.Exists<OutDepotHandoverDetail>((x, y) => y.Where(p => p.OutDepotHandoverId == x.Id)
                     .WhereIf(criteria.SparePartName.IsNotEmpty(), p => p.SparePart.SparePartName.Contains(criteria.SparePartName))
                     .WhereIf(criteria.SeriaNo.IsNotEmpty(), p => p.SeriaNo.Contains(criteria.SeriaNo))
                     .WhereIf(criteria.BatchNo.IsNotEmpty(), p => p.BatchNo.Contains(criteria.BatchNo))
                     .WhereIf(criteria.SparePart != null, p => p.SparePartId == criteria.SparePartId));
            }

            var list = query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            return list;
        }

        /// <summary>
        /// 整单交接操作
        /// </summary>
        /// <param name="handoverBill">交接单信息</param>
        /// <returns></returns>
        public virtual void WholeBillHandover(OutDepotHandover handoverBill)
        {
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                handoverBill.HandOverStatus = HandOverStatus.Received;

                foreach (var item in handoverBill.OutDepotHandoverDetailList)
                {
                    item.HandOverStatus = HandOverStatus.Received;
                    item.ReceiveQty = item.Qty;

                    DB.Update<OutDepotDetail>().Where(p => p.OutDepotId == handoverBill.OutDepotId && p.SparePartId == item.SparePartId)
                                               .Set(p => p.ReceiveQty, p => p.ReceiveQty + item.ReceiveQty).Execute();
                }

                RF.Save(handoverBill);

                trans.Complete();
            }  
        }

        /// <summary>
        /// 扫码交接操作
        /// </summary>
        /// <param name="outHandoverDetailList">交接单明细信息</param>
        /// <returns></returns>
        public virtual void ScanOutHandoverDetail(List<OutDepotHandoverDetail> outHandoverDetailList)
        {
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                var outDepotHandover = Query<OutDepotHandover>().Where(p => p.Id == outHandoverDetailList[0].OutDepotHandoverId).FirstOrDefault(new EagerLoadOptions().LoadWith(OutDepotHandover.OutDepotHandoverDetailListProperty));

                foreach (var item in outHandoverDetailList)
                {
                    var detail = outDepotHandover.OutDepotHandoverDetailList.First(p => p.Id == item.Id);
                    detail.ReceiveQty += item.ReceiveQty;
                    detail.HandOverStatus = item.HandOverStatus;

                    DB.Update<OutDepotDetail>().Where(p => p.OutDepotId == outDepotHandover.OutDepotId && p.SparePartId == item.SparePartId)
                                               .Set(p => p.ReceiveQty, p => p.ReceiveQty + item.ReceiveQty).Execute();
                }

                int sumQty = outDepotHandover.OutDepotHandoverDetailList.Sum(p => p.Qty);
                int sumReceiveQty = outDepotHandover.OutDepotHandoverDetailList.Sum(p => p.ReceiveQty);

                outDepotHandover.HandOverStatus = sumQty - sumReceiveQty > 0 ? HandOverStatus.Receiving : HandOverStatus.Received;

                RF.Save(outDepotHandover);

                trans.Complete();
            }
        }

        /// <summary>
        /// 获取接收明细
        /// </summary>
        /// <param name="handoverId">交接单Id</param>
        /// <param name="sparePartId">备件Id</param>
        /// <param name="orderInfoList">排序信息</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>接收明细列表</returns>
        public virtual EntityList<OutDepotHandoverDetail> GetOutDepotHandoverDetails(double? handoverId, double? sparePartId, IList<OrderInfo> orderInfoList, PagingInfo pagingInfo)
        {
            var list = Query<OutDepotHandoverDetail>().Where(p => p.OutDepotHandoverId == handoverId && p.HandOverStatus == HandOverStatus.Pending)
                .OrderBy(orderInfoList).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            if (sparePartId != null && sparePartId != 0) {

                var detail = list.FirstOrDefault(p => p.SparePartId == sparePartId);
                if (detail!=null) 
                {
                    detail.ReceiveQty = detail.Qty;
                    detail.HandOverStatus = HandOverStatus.Received;
                }
            }

            return list;
        }

        /// <summary>
        /// 获取符合已选备件的交接单
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="sparePartId">备件Id</param>
        /// <param name="keyword">查询关键字</param>
        /// <returns>交接单列表</returns>
        public virtual EntityList<OutDepotHandover> GetOutDepotHandoverBySparePart(PagingInfo pagingInfo, double? sparePartId, string keyword)
        {
            var list = Query<OutDepotHandover>().Exists<OutDepotHandoverDetail>((x, y) => y.Where(b=>b.OutDepotHandoverId == x.Id)
                .Where(b => b.HandOverStatus == HandOverStatus.Pending || b.HandOverStatus == HandOverStatus.Receiving)
                .WhereIf(sparePartId != null && sparePartId !=0, b => b.SparePartId == sparePartId))
                .WhereIf(keyword.IsNotEmpty(), x => x.HandoverNo.Contains(keyword))
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty().LoadWith(OutDepotHandover.OutDepotHandoverDetailListProperty));

            if (sparePartId != null && sparePartId != 0) 
            {
                var outDepotHandoverDetailList = list.Select(p => p.Id).SplitContains(tempIds => {
                    return Query<OutDepotHandoverDetail>().Where(p => tempIds.Contains(p.OutDepotHandoverId) && p.HandOverStatus == HandOverStatus.Pending)
                    .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                });

                foreach (var item in list)
                {
                    var detail = outDepotHandoverDetailList.First(p => p.OutDepotHandoverId == item.Id && p.SparePartId == sparePartId);

                    item.SparePartId = detail.SparePartId;
                    item.SparePartCode = detail.SparePartCode;
                    item.SparePartName = detail.SparePartName;
                    item.ControlMethod = detail.ControlMethod;
                    item.Barcode = detail.ControlMethod == Enums.ControlMethod.ItemCode ? "" : (detail.ControlMethod == Enums.ControlMethod.Batch ? detail.BatchNo : detail.SeriaNo);
                    item.Qty = detail.Qty;
                    item.ReceiveQty = detail.Qty;
                }
            }
            
            return list;
        }

        /// <summary>
        /// 获取交接明细里的备件信息
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="handover">交接单头</param>
        /// <param name="keyword">查询关键字</param>
        /// <returns>备件列表</returns>
        public virtual EntityList<SparePart> GetSparePartByOutDepotHandover(PagingInfo pagingInfo, OutDepotHandover handover, string keyword)
        {
            EntityList<SparePart> list = Query<SparePart>().Join<OutDepotHandoverDetail>((a, b) => a.Id == b.SparePartId)
                                                            .Where<OutDepotHandoverDetail>((a, b) => b.OutDepotHandoverId == handover.OutDepotHandoverBillId && b.HandOverStatus == HandOverStatus.Pending)
                                                            .WhereIf(keyword.IsNotEmpty(), p => p.SparePartCode.Contains(keyword) || p.SparePartName.Contains(keyword))
                                                            .Distinct().ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            foreach (var item in list)
            {
                item.IsReplacement = true;//仅用来标记是手动进行选择的备件
            }
            return list;
        }

        /// <summary>
        /// 交接单明细扫描查询
        /// </summary>
        /// <param name="barcode">扫描条码</param>
        /// <returns>查询信息</returns>
        public virtual OutDepotHandoverQueryInfo OutDepotHandoverBarcodeQuery(string barcode)
        {
            OutDepotHandoverQueryInfo info = new OutDepotHandoverQueryInfo();
            info.Success = true;

            info.OutDepotHandoverInfoList = Query<OutDepotHandover>().Exists<OutDepotHandoverDetail>((x, y) => y.Where(b => b.OutDepotHandoverId == x.Id)
                .Where(b => b.HandOverStatus == HandOverStatus.Pending)
                .Where(b => (b.SparePart.SparePartCode == barcode && b.SparePart.ControlMethod ==  Enums.ControlMethod.ItemCode) || b.BatchNo == barcode || b.SeriaNo == barcode))
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty().LoadWith(OutDepotHandover.OutDepotHandoverDetailListProperty));

            var outDepotHandoverDetailList = info.OutDepotHandoverInfoList.Select(p => p.Id).SplitContains(tempIds => {
                return Query<OutDepotHandoverDetail>().Where(p => tempIds.Contains(p.OutDepotHandoverId) && p.HandOverStatus == HandOverStatus.Pending)
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });

            if (!info.OutDepotHandoverInfoList.Any()) 
            {
                info.Success = false;
                info.Message = "所扫描的内容在接收明细中找不到符合其管控方式和状态为待接收的数据，请确认后重新扫描!".L10N();
            }

            foreach (var item in info.OutDepotHandoverInfoList)
            {
                var detailList = outDepotHandoverDetailList.Where(p => p.OutDepotHandoverId == item.Id).ToList();
                var detail = detailList.First(b=>(b.SparePart.SparePartCode == barcode && b.ControlMethod == Enums.ControlMethod.ItemCode) || b.BatchNo == barcode || b.SeriaNo == barcode);

                item.SparePartId = detail.SparePartId;
                item.SparePartCode = detail.SparePartCode;
                item.SparePartName = detail.SparePartName;
                item.ControlMethod = detail.ControlMethod;
                item.Barcode = barcode;
                item.Qty = detail.Qty;
                item.ReceiveQty = detail.Qty;
            }

            return info;
        }
    }
}
