using SIE.Common.Import;
using SIE.Core.Common.Controllers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Purchases.Enums;
using SIE.EMS.Purchases.PurchaseOrders;
using SIE.EMS.Purchases.PurchaseRequisitions;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Enums;
using SIE.EMS.SpareParts.OutDepots;
using SIE.EMS.SpareParts.OutDepots.Details;
using SIE.EMS.SpareParts.OutDepots.Enums;
using SIE.Equipments.Enums;
using SIE.EventMessages.EMS.SparePartReceives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.Purchases.SparePartReceives
{
    /// <summary>
    /// 备件控制器
    /// </summary>
    public partial class SparePartReceiveController : DomainController, ISparePartReceives
    {
        /// <summary>
        /// 获取备件接收
        /// </summary>
        /// <param name="receiveId">接收id</param>
        /// <returns>备件接收</returns>
        public virtual SparePartReceive GetSparePartReceiveById(double receiveId)
        {
            return Query<SparePartReceive>().Where(p => p.Id == receiveId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 查询备件接收信息
        /// </summary>
        /// <param name="criteria">备件接收查询实体</param>
        /// <returns>备件接收信息</returns>
        public virtual EntityList<SparePartReceive> CriteriaSparePartReceives(SparePartReceiveCriteria criteria)
        {
            var query = Query<SparePartReceive>();
            if (criteria.FactoryId.HasValue)
            {
                query.Where(p => p.FactoryId == criteria.FactoryId.Value);
            }

            if (criteria.DepartmentId.HasValue)
            {
                query.Where(p => p.DepartmentId == criteria.DepartmentId.Value);
            }

            if (!criteria.PurchaseOrderNo.IsNullOrWhiteSpace() && criteria.SupplierId.HasValue)
            {
                query.Exists<SparePartReceiveDetail>((a, b) => b.Where(p => p.SparePartReceiveId == a.Id
                    && p.PurchaseOrder.OrderNo.Contains(criteria.PurchaseOrderNo)
                    && p.SupplierId == criteria.SupplierId.Value));
            }
            else
            {
                if (!criteria.PurchaseOrderNo.IsNullOrWhiteSpace())
                {
                    query.Exists<SparePartReceiveDetail>((a, b) => b.Where(p => p.SparePartReceiveId == a.Id
                        && p.PurchaseOrder.OrderNo.Contains(criteria.PurchaseOrderNo)));
                }

                if (criteria.SupplierId.HasValue)
                {
                    query.Exists<SparePartReceiveDetail>((a, b) => b.Where(p => a.Id == p.SparePartReceiveId
                        && p.SupplierId == criteria.SupplierId.Value));
                }
            }

            if (criteria.ReceiveType.HasValue)
            {
                query.Where(p => p.ReceiveType == criteria.ReceiveType.Value);
            }

            if (criteria.ReceiveBillStatus.HasValue)
            {
                query.Where(p => p.ReceiveBillStatus == criteria.ReceiveBillStatus.Value);
            }

            if (criteria.CreateDate.BeginValue.HasValue)
            {
                query.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue.Value);
            }

            if (criteria.CreateDate.EndValue.HasValue)
            {
                query.Where(p => p.CreateDate <= criteria.CreateDate.EndValue.Value);
            }

            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据id列表获取备件接收信息
        /// </summary>
        /// <param name="receiveIds">id列表</param>
        /// <returns>备件接收信息</returns>
        public virtual EntityList<SparePartReceive> GetSparePartReceivesByIds(List<double> receiveIds)
        {
            return receiveIds.SplitContains(ids => Query<SparePartReceive>().Where(p => ids.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty()));
        }

        /// <summary>
        /// 根据备件接收id列表获取明细列表
        /// </summary>
        /// <param name="receiveIds">备件接收id列表</param>
        /// <returns>明细列表</returns>
        public virtual EntityList<SparePartReceiveDetail> GetDetailsByReceiveIds(List<double> receiveIds)
        {
            return Query<SparePartReceiveDetail>().Where(p => receiveIds.Contains(p.SparePartReceiveId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据备件接收id获取接收明细列表
        /// </summary>
        /// <param name="receiveId">备件接收id</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>接收明细列表</returns>
        public virtual EntityList<SparePartReceiveDetail> GetDetailsByReceiveId(double receiveId, PagingInfo pagingInfo)
        {
            return Query<SparePartReceiveDetail>().Where(p => receiveId == p.SparePartReceiveId).OrderBy(p => p.LineNo)
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取批次明细
        /// </summary>
        /// <param name="receiveId">接收id</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>批次明细</returns>
        public virtual EntityList<SparePartReceiveLot> GetReceiveLotInfo(double receiveId, PagingInfo pagingInfo)
        {
            return Query<SparePartReceiveLot>().Join<SparePartReceiveDetail>((a, b) => a.SparePartReceiveDetailId == b.Id && b.SparePartReceiveId == receiveId)
                .OrderByDescending(p => p.CreateDate).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取批次明细
        /// </summary>
        /// <param name="snIds">批次明细id</param>
        /// <returns>批次明细</returns>
        public virtual EntityList<SparePartReceiveLot> GetSparePartReceiveLotsByIds(List<double> snIds)
        {
            return snIds.SplitContains(ids => Query<SparePartReceiveLot>().Where(p => ids.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty()));
        }

        /// <summary>
        /// 获取批次明细
        /// </summary>
        /// <param name="receiveIds">接收id</param>
        /// <returns>批次明细</returns>
        public virtual EntityList<SparePartReceiveLot> GetReceiveLotList(List<double> receiveIds)
        {
            return Query<SparePartReceiveLot>().Join<SparePartReceiveDetail>((a, b) => a.SparePartReceiveDetailId == b.Id && receiveIds.Contains(b.SparePartReceiveId))
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取序列号明细
        /// </summary>
        /// <param name="receiveId">接收id</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>序列号明细</returns>
        public virtual EntityList<SparePartReceiveSn> GetReceiveSnInfo(double receiveId, PagingInfo pagingInfo)
        {
            return Query<SparePartReceiveSn>().Join<SparePartReceiveDetail>((a, b) => a.SparePartReceiveDetailId == b.Id && b.SparePartReceiveId == receiveId)
                .OrderByDescending(p => p.CreateDate).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取序列号明细
        /// </summary>
        /// <param name="snIds">序列号明细id</param>
        /// <returns>序列号明细</returns>
        public virtual EntityList<SparePartReceiveSn> GetSparePartReceiveSnsByIds(List<double> snIds)
        {
            return snIds.SplitContains(ids => Query<SparePartReceiveSn>().Where(p => ids.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty()));
        }

        /// <summary>
        /// 获取序列号明细
        /// </summary>
        /// <param name="receiveIds">接收id</param>
        /// <returns>序列号明细</returns>
        public virtual EntityList<SparePartReceiveSn> GetReceiveSnList(List<double> receiveIds)
        {
            return Query<SparePartReceiveSn>().Join<SparePartReceiveDetail>((a, b) => a.SparePartReceiveDetailId == b.Id && receiveIds.Contains(b.SparePartReceiveId))
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取序列号明细里原厂序列号存在数量
        /// </summary>
        /// <param name="sn">原厂序列号</param>
        /// <param name="receiveSnId">序列号明细id</param>
        /// <returns>存在数量</returns>
        public virtual int GetReceiveSnOriginalSnQty(string sn, double receiveSnId)
        {
            return Query<SparePartReceiveSn>().Where(p => p.OriginalSn == sn && p.Id != receiveSnId).Count();
        }

        /// <summary>
        /// 获取出库单明细
        /// </summary>
        /// <param name="sparePartId">备件id</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        /// <returns>出库单明细</returns>
        public virtual EntityList<PartOutDepotDetail> GetPartOutDepotDetails(double sparePartId, PagingInfo pagingInfo, string keyword)
        {
            return Query<PartOutDepotDetail>().Join<OutDepot>((a, b) => a.OutDepotId == b.Id && b.OutDepotType == OutDepotType.DgMaintain)
                .Where(p => p.SparePartId == sparePartId)
                .WhereIf(!keyword.IsNullOrWhiteSpace(), p => p.SeriaNo.Contains(keyword)).ToList(pagingInfo);
        }

        /// <summary>
        /// 根据采购订单行获取备件数据
        /// </summary>
        /// <param name="orderItemId">采购订单行</param>
        /// <returns>备件数据</returns>
        public virtual SparePart GetSparePartInfo(double orderItemId)
        {
            var orderItem = Query<PurchaseRequisitionItem>().Exists<PurchaseOrderItem>((a, y) =>
            y.Where(b => a.Id == b.PurchaseRequisitionItemId && b.Id == orderItemId)).FirstOrDefault();
            if (orderItem == null)
            {
                return null;
            }
            return Query<SparePart>().Where(p => p.SparePartCode == orderItem.ObjectCode).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 创建一个新的备件接收
        /// </summary>
        /// <returns>新的备件接收</returns>
        public virtual SparePartReceive GetNewSparePartReceive()
        {
            var entity = new SparePartReceive();
            entity.ReceiveNo = RT.Service.Resolve<CommonController>().GetNo<SparePartReceive>("备件接收");
            entity.ReceiveBillStatus = ReceiveBillStatus.ToBeSubmitted;
            entity.ReceiveType = ReceiveType.Purchase;
            return entity;
        }

        /// <summary>
        /// 保存备件接收
        /// </summary>
        /// <param name="receive">备件接收</param>
        public virtual void SaveSparePartReceive(SparePartReceive receive)
        {
            if (receive == null)
            {
                throw new ValidationException("保存失败，数据异常".L10N());
            }
            if (receive.PersistenceStatus != PersistenceStatus.New)
            {
                var old = GetById<SparePartReceive>(receive.Id);
                if (old == null)
                {
                    throw new ValidationException("保存失败，数据异常".L10N());
                }
                if (old.ReceiveBillStatus != ReceiveBillStatus.ToBeSubmitted)
                {
                    throw new ValidationException("保存失败，状态为【待提交】的数据才能修改".L10N());
                }
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                RF.Save(receive);

                var details = GetDetailsByReceiveIds(new List<double> { receive.Id });
                if (details.Any(p => p.RecivedQty > 0))
                {
                    throw new ValidationException("接收明细存在【已接收数量】不为0时，不允许修改".L10N());
                }
                if (details.Any(p => p.Qty < p.RecivedQty))
                {
                    throw new ValidationException("接收数量不能小于已经接收的数量".L10N());
                }
                if (receive.ReceiveType == ReceiveType.Purchase && details.Any(p => p.PurchaseOrderId == null || p.PurchaseOrderItemId == null))
                {
                    throw new ValidationException("接收类型为采购接收时,采购单号和采购行号必输".L10N());
                }
                if (receive.ReceiveType == ReceiveType.Giveaway && details.Any(p => p.Price != 0 || p.TaxRate != 0))
                {
                    throw new ValidationException("接收类型为【赠品接收】时，单价和税率只能为0".L10N());
                }
                if ((receive.ReceiveType == ReceiveType.Customer || receive.ReceiveType == ReceiveType.Lease || receive.ReceiveType == ReceiveType.Other)
                   && details.Any(p => p.PurchaseOrderId != null || p.PurchaseOrderItemId != null))
                {
                    throw new ValidationException("采购单号和采购行号只能为空".L10N());
                }
                //保存后再更新品种数和总数量
                receive.VarietyQuantity = details.Select(p => p.SparePartId).Distinct().Count();
                receive.TotalQty = details.Sum(p => p.Qty);
                RF.Save(receive);
                trans.Complete();
            }
        }

        /// <summary>
        /// 导入保存备件接收
        /// </summary>
        /// <param name="batch">导入数据</param>
        /// <returns>导入返回信息</returns>
        public virtual List<ImportMessageResult> ImportOnSave(IList<RowData> batch)
        {
            if (batch == null)
            {
                throw new ValidationException("数据异常".L10N());
            }
            List<ImportMessageResult> messageList = new List<ImportMessageResult>();
            var details = batch.Select(p => p.Entity as SparePartReceiveDetail).ToList();
            var receiveIds = details.Select(p => p.SparePartReceiveId).ToList();

            //获取备件库存数据
            var allLotNos = details.Select(p => p.LotNo).ToList();
            var oldLots = allLotNos.SplitContains(tempLotNos =>
            {
                return Query<StoreSummaryLot>().Where(p => tempLotNos.Contains(p.BatchNumber)).ToList();
            });

            var allSns = details.Select(p => p.Sn).ToList();
            var oldSns = allSns.SplitContains(tempSns =>
            {
                return Query<StoreSummaryDetail>().Where(p => tempSns.Contains(p.OrderNumberCode)).ToList();
            });

            var receives = GetSparePartReceivesByIds(receiveIds);
            var allDetails = GetDetailsByReceiveIds(receiveIds);
            foreach (var row in batch)
            {
                try
                {
                    var detail = row.Entity as SparePartReceiveDetail;
                    var receive = receives.FirstOrDefault(p => p.Id == detail.SparePartReceiveId);

                    //导入校验数据
                    var spDetail = ImportOnCheck(receive, allDetails, detail);
                    using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
                    {
                        if (spDetail.ControlMethod == ControlMethod.ItemCode)
                        {
                            spDetail.RecivedQty = detail.ImportQty.Value;
                            RF.Save(spDetail);
                        }

                        if (spDetail.ControlMethod == ControlMethod.Batch)
                        {
                            if (allLotNos.Count(p => p == detail.LotNo) > 1)
                            {
                                throw new ValidationException("批次号重复".L10N());
                            }
                            var oldLot = oldLots.FirstOrDefault(p => p.BatchNumber == detail.LotNo);
                            if (oldLot != null)
                            {
                                throw new ValidationException("批次号{0}已存在，请确认".L10nFormat(detail.LotNo));
                            }
                            spDetail.RecivedQty += detail.ImportQty.Value;
                            RF.Save(spDetail);
                            var sparePartReceiveLot = new SparePartReceiveLot();
                            sparePartReceiveLot.LotNo = detail.LotNo;
                            sparePartReceiveLot.Qty = detail.ImportQty.Value;
                            sparePartReceiveLot.SparePartReceiveDetailId = spDetail.Id;
                            RF.Save(sparePartReceiveLot);
                        }
                        if (spDetail.ControlMethod == ControlMethod.Sn)
                        {
                            if (allSns.Count(p => p == detail.Sn) > 1)
                            {
                                throw new ValidationException("序列号重复".L10N());
                            }
                            var oldSn = oldSns.FirstOrDefault(p => p.OrderNumberCode == detail.Sn);
                            if (oldSn != null)
                            {
                                throw new ValidationException("序列号{0}已存在，请确认".L10nFormat(detail.Sn));
                            }
                            spDetail.RecivedQty += 1;
                            RF.Save(spDetail);
                            var sparePartReceiveSn = new SparePartReceiveSn();
                            sparePartReceiveSn.Sn = detail.Sn;
                            sparePartReceiveSn.OriginalSn = detail.OriginalSn;
                            sparePartReceiveSn.SparePartReceiveDetailId = spDetail.Id;
                            RF.Save(sparePartReceiveSn);
                        }
                        trans.Complete();
                    }
                    messageList.Add(new ImportMessageResult { RowNum = row.RowIndex + 1, MsgType = ImportMessageType.SaveSucess, Message = "保存成功！".L10N() });
                }
                catch (Exception exc)
                {
                    messageList.Add(new ImportMessageResult { RowNum = row.RowIndex + 1, MsgType = ImportMessageType.SaveFail, Message = exc.GetBaseException().Message });
                }
            }
            return messageList;
        }

        /// <summary>
        /// 导入校验数据
        /// </summary>
        /// <param name="receive">备件接收</param>
        /// <param name="allDetails">备件接收明细</param>
        /// <param name="detail">导入的备件接收明细</param>
        /// <returns>备件接收明细</returns>
        private SparePartReceiveDetail ImportOnCheck(SparePartReceive receive, EntityList<SparePartReceiveDetail> allDetails, SparePartReceiveDetail detail)
        {
            if (receive == null)
            {
                throw new ValidationException("数据异常".L10N());
            }
            if (receive.ReceiveBillStatus != ReceiveBillStatus.ToBeSubmitted)
            {
                throw new ValidationException("接收单{0}已完成".L10nFormat(receive.ReceiveNo));
            }
            var spDetail = allDetails.FirstOrDefault(p => p.SparePartReceiveId == detail.SparePartReceiveId && p.LineNo == detail.LineNo);
            if (spDetail == null)
            {
                throw new ValidationException("接收单{0}不存在行号{1}".L10nFormat(receive.ReceiveNo, detail.LineNo));
            }
            if (detail.ImportQty == null || detail.ImportQty < 0)
            {
                throw new ValidationException("数量不能为空且不能为负数".L10N());
            }
            if (spDetail.ControlMethod != ControlMethod.ItemCode && spDetail.RecivedQty + detail.ImportQty > spDetail.Qty)
            {
                throw new ValidationException("【已接收数量】加上本次数量不能大于【接收数量】".L10N());
            }
            if (spDetail.ControlMethod == ControlMethod.Batch && detail.LotNo.IsNullOrWhiteSpace())
            {
                throw new ValidationException("接收单{0}接收行{1}备件{2}批次号不能为空".L10nFormat(receive.ReceiveNo, detail.LineNo, spDetail.SparePartName));
            }
            if (spDetail.ControlMethod == ControlMethod.Sn && detail.Sn.IsNullOrWhiteSpace())
            {
                throw new ValidationException("接收单{0}接收行{1}备件{2}序列号不能为空".L10nFormat(receive.ReceiveNo, detail.LineNo, spDetail.SparePartName));
            }
            return spDetail;
        }

        /// <summary>
        /// 一键接收
        /// </summary>
        /// <param name="receiveId">接收id</param>
        public virtual void OnekeyReceive(double receiveId)
        {
            var details = GetDetailsByReceiveIds(new List<double> { receiveId });
            var idList = details.Where(p => p.ControlMethod == ControlMethod.ItemCode).Select(p => p.Id).ToList();
            DB.Update<SparePartReceiveDetail>().Set(p => p.RecivedQty, p => p.Qty).Where(p => idList.Contains(p.Id)).Execute();
        }

        /// <summary>
        /// 更新备件采购明细入库数量和状态
        /// </summary>
        /// <param name="sparePartReceiveInfo">备件入库接收单更新信息</param>
        /// <returns></returns>
        public virtual void UpdatePurchaseDtlInboundQty(SparePartReceiveInfo sparePartReceiveInfo)
        {
            var receiveDtlList = sparePartReceiveInfo.SparePartReceiveDtlInfoList.Select(p => p.SparePartId).SplitContains(tempIds =>
            {
                return Query<SparePartReceiveDetail>().Where(p => tempIds.Contains(p.SparePartId) && p.SparePartReceive.ReceiveNo == sparePartReceiveInfo.ReceiveNo).ToList(null,new EagerLoadOptions().LoadWithViewProperty());
            });

            var purOrderItemList = receiveDtlList.Select(p => p.PurchaseOrderItemId).Distinct().SplitContains(tempIds =>
            {
                return Query<PurchaseOrderItem>().Where(p => tempIds.Contains(p.Id)).ToList();
            });

            var purOrderList = purOrderItemList.Select(p => p.PurchaseOrderId).Distinct().SplitContains(tempIds =>
            {
                return Query<PurchaseOrder>().Where(p => tempIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWith(PurchaseOrder.DetailListProperty));
            });

            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                foreach (var item in sparePartReceiveInfo.SparePartReceiveDtlInfoList)
                {
                    var detail = receiveDtlList.First(p => p.PurchaseOrderNo == item.PurchaseOrderNo && p.PurchaseOrderLine == item.PurchaseOrderLineNo);
                    var purOrderItem = purOrderItemList.First(p => p.Id == detail.PurchaseOrderItemId);

                    //当验收单号为空时，更新采购订单行的接收数量为原来的值减本次入库的数量
                    if (sparePartReceiveInfo.AcceptanceNo.IsNullOrEmpty())
                    {
                        purOrderItem.ReciveQty -= item.InboundQty;
                    }
                    else
                    {
                        //当验收单号不为空时，更新采购订单行的验收数量为原来的值减本次入库的数量
                        purOrderItem.AcceptanceQty -= item.InboundQty;
                    }

                    //更新采购明细行的入库数量和状态
                    purOrderItem.InboundQty += item.InboundQty;

                    purOrderItem.Status = purOrderItem.InboundQty >= purOrderItem.Qty ? PurchaseOrderStatus.Complete : purOrderItem.Status;
                    RF.Save(purOrderItem);

                    //更新采购主表的状态
                    var purOrder = purOrderList.First(p => p.Id == purOrderItem.PurchaseOrderId);
                    purOrder.DetailList.First(p => p.Id == purOrderItem.Id).Status = purOrderItem.Status;

                    if (purOrder.DetailList.All(p => p.Status == PurchaseOrderStatus.Complete || p.Status == PurchaseOrderStatus.Close))
                    {
                        DB.Update<PurchaseOrder>().Set(p => p.PurchaseOrderStatus, PurchaseOrderStatus.Complete).Where(p => p.Id == purOrder.Id).Execute();
                    }
                }
                trans.Complete();
            }
        }
    }
}
