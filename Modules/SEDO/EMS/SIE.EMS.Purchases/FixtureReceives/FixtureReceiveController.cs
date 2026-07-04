using SIE.Common.Configs;
using SIE.Core.Common.Controllers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Purchases.Enums;
using SIE.EMS.Purchases.FixtureAcceptances;
using SIE.EMS.Purchases.FixtureReceives.Model;
using SIE.EMS.Purchases.PurchaseOrders;
using SIE.EMS.Purchases.PurchaseRequisitions;
using SIE.Equipments.Enums;
using SIE.Fixtures;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Fixtures.Models;
using SIE.Fixtures.Models.Config;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.Purchases.FixtureReceives
{
    /// <summary>
    /// 工治具接收控制器
    /// </summary>
    public class FixtureReceiveController : DomainController
    {

        /// <summary>
        /// 保存前检查
        /// </summary>
        /// <param name="receive"></param>
        /// <returns></returns>
        private void CheckSaveFixtureReceive(FixtureReceive receive)
        {
            if (receive == null)
            {
                throw new ValidationException("保存失败，数据异常".L10N());
            }
            if (receive.PersistenceStatus != PersistenceStatus.New)
            {
                var old = GetById<FixtureReceive>(receive.Id);
                if (old == null)
                {
                    throw new ValidationException("保存失败，数据异常".L10N());
                }
                if (old.ReceiveBillStatus != ReceiveBillStatus.ToBeSubmitted)
                {
                    throw new ValidationException("保存失败，状态为【待提交】的数据才能修改".L10N());
                }
            }

        }
        /// <summary>
        /// 获取扫描信息
        /// </summary>
        /// <param name="receiveId"></param>
        /// <returns></returns>
        public virtual ReceiveScanViewModel GetReceiveScanInfo(double receiveId)
        {
            var receive = GetFixtureReceiveById(receiveId);
            var model = new ReceiveScanViewModel();
            model.ReceiveNo = receive.ReceiveNo;
            model.FactoryName = receive.FactoryName;
            model.DepartmentName = receive.DepartmentName;
            model.ReceiveType = receive.ReceiveType;
            return model;
        }

        /// <summary>
        /// 获取工治具接收
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual FixtureReceive GetFixtureReceiveById(double id)
        {
            return Query<FixtureReceive>().Where(p => p.Id == id).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取接收明细根据接收ID
        /// </summary>
        /// <param name="fixtureReceiveId"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<FixtureReceiveDetail> GetDetailsByReceiveId(double fixtureReceiveId, PagingInfo pagingInfo)
        {
            return Query<FixtureReceiveDetail>().Where(p => fixtureReceiveId == p.FixtureReceiveId).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

        }

        /// <summary>
        /// 保存工治具接收
        /// </summary>
        /// <param name="receive"></param>
        public virtual void SaveFixtureReceive(FixtureReceive receive)
        {
            if (receive != null)
            {
                CheckSaveFixtureReceive(receive);
                using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
                {
                    var details = receive.FixtureReceiveDetailList;
                    if (details.Any(p => p.Qty <= 0))
                    {
                        throw new ValidationException("接收数量必须为正整数".L10N());
                    }
                    var orderItemIds = details.Where(p => p.PurchaseOrderItemId != null).Select(p => (double)p.PurchaseOrderItemId).Distinct().ToList();
                    var orderItems = RT.Service.Resolve<PurchaseOrderController>().GetPurDetailsByIds(orderItemIds);
                    if (receive.ReceiveType == ReceiveType.Purchase)
                    {
                        var noGiveaways = details.Where(p => !p.Giveaway).ToList();
                        if (noGiveaways.Any(p => p.PurchaseOrderId == null || p.PurchaseOrderItemId == null))
                        {
                            throw new ValidationException("接收类型为采购接收且不是赠品时,采购单号和采购行号必输".L10N());
                        }
                        foreach (var noGiveaway in noGiveaways)
                        {
                            var orderItem = orderItems.FirstOrDefault(p => p.Id == noGiveaway.PurchaseOrderItemId);
                            if (orderItem == null)
                            {
                                throw new ValidationException("找不到id为:{0}的采购订单行".L10nFormat(noGiveaway.PurchaseOrderItemId));
                            }
                            var qty = noGiveaways.Where(p => p.PurchaseOrderItemId == noGiveaway.PurchaseOrderItemId).Select(p => p.Qty).Sum();
                            qty += GetNoGiveawaysQty(noGiveaway.PurchaseOrderItemId.Value, receive.Id);
                            if (qty > orderItem.Qty - orderItem.ReciveQty - orderItem.AcceptanceQty - orderItem.InboundQty)
                            {
                                throw new ValidationException("采购订单号:{0}，行号{1}接收数量大于采购数量".L10nFormat(noGiveaway.PurchaseOrder.OrderNo, orderItem.LineNo));
                            }
                        }
                        if (details.Any(p => p.Giveaway && p.Price != 0))
                        {
                            throw new ValidationException("接收类型为【采购接收】且是赠品时，单价只能为0".L10N());
                        }
                    }
                    if ((receive.ReceiveType == ReceiveType.Customer || receive.ReceiveType == ReceiveType.Lease || receive.ReceiveType == ReceiveType.Other)
                       && details.Any(p => p.PurchaseOrderId != null || p.PurchaseOrderItemId != null))
                    {
                        throw new ValidationException("采购单号和采购行号只能为空".L10N());
                    }
                    if (receive.ReceiveType == ReceiveType.Customer && details.Any(p => p.CustomerId == null))
                    {
                        throw new ValidationException("接收类型为【客供接收】时，客户必输".L10N());
                    }
                    if (receive.ReceiveType == ReceiveType.Giveaway && details.Any(p => !p.Giveaway))
                    {
                        throw new ValidationException("接收类型为【赠品接收】时，明细必须是赠品".L10N());
                    }

                    //保存后再更新品种数和总数量
                    receive.VarietyQuantity = details.Select(p => p.FixtureEncodeId).Distinct().Count();
                    receive.TotalQty = details.Sum(p => p.Qty);
                    RF.Save(receive);
                    trans.Complete();
                }
            }
        }

        /// <summary>
        /// 获取工治具编码
        /// </summary>
        /// <returns></returns>
        public virtual FixtureEncodeInfo GetFixtureEncodeInfo(double orderItemId)
        {
            var orderItem = Query<PurchaseRequisitionItem>().Join<PurchaseOrderItem>((a, b) => a.Id == b.PurchaseRequisitionItemId && b.Id == orderItemId).FirstOrDefault();
            if (orderItem == null)
            {
                return null;
            }
            var result = Query<FixtureEncode>().Where(p => p.Code == orderItem.ObjectCode).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            if (result != null)
            {
                return new FixtureEncodeInfo()
                {
                    Code = result.Code,
                    ManageMode = (int)result.ManageMode,
                    ModelName = result.ModelName,
                    ModelCode = result.ModelCode,
                    ExemptionInspect = result.Exemption,
                    Id = result.Id,
                    UnitId = result.UnitId,
                    UnitName = result.UnitName
                };
            }
            return null;
        }

        /// <summary>
        /// 删除工治具接收
        /// </summary>
        /// <param name="ids"></param>
        public virtual void DeleteFixtureReceive(List<double> ids)
        {
            var entity = GetReceivesByIds(ids);
            if (entity.Any(p => p.ReceiveBillStatus != ReceiveBillStatus.ToBeSubmitted))
            {
                throw new ValidationException("只有状态为【待提交】的数据才能删除".L10N());
            }
            var allSnList = GetFixtureReceiveSnByReceiveIds(ids);
            var snList = allSnList.Select(p => p.Sn).Distinct().ToList();
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                DB.Delete<FixtureReceive>().Where(p => ids.Contains(p.Id)).Execute();
                DB.Delete<FixtureAccount>().Where(p => snList.Contains(p.Code)).Execute();
                trans.Complete();
            }
        }

        /// <summary>
        /// 根据ID集合获取接收单
        /// </summary>
        /// <param name="receiveIds"></param>
        /// <returns></returns>
        public virtual EntityList<FixtureReceive> GetReceivesByIds(List<double> receiveIds)
        {
            return receiveIds.SplitContains(ids => Query<FixtureReceive>().Where(p => ids.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty()));
        }

        /// <summary>
        ///根据接收单ID集合获取工治具接收SN集合
        /// </summary>
        /// <param name="receiveIds"></param>
        /// <returns></returns>
        public virtual EntityList<FixtureReceiveSn> GetFixtureReceiveSnByReceiveIds(List<double> receiveIds)
        {
            return Query<FixtureReceiveSn>().Join<FixtureReceiveDetail>((a, b) => a.FixtureReceiveDetailId == b.Id && receiveIds.Contains(b.FixtureReceiveId))
                  .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        ///根据ID集合获取工治具接收SN集合
        /// </summary>
        /// <param name="snIds"></param>
        /// <returns></returns>
        public virtual EntityList<FixtureReceiveSn> GetFixtureReceiveSnIds(List<double> snIds)
        {
            return Query<FixtureReceiveSn>().Where(m => snIds.Contains(m.Id))
                  .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }


        /// <summary>
        /// 获取接收明细
        /// </summary>
        /// <param name="receiveIds"></param>
        /// <returns></returns>
        public virtual EntityList<FixtureReceiveDetail> GetDetailsByReceiveIds(List<double> receiveIds)
        {
            return Query<FixtureReceiveDetail>().Where(p => receiveIds.Contains(p.FixtureReceiveId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }


        /// <summary>
        /// 获取接收明细的非赠品接收数量汇总
        /// </summary>
        /// <param name="purchaseOrderItemId"></param>
        /// <param name="fixtureReceiveId"></param>
        /// <returns>接收数量汇总</returns>
        private int GetNoGiveawaysQty(double purchaseOrderItemId, double fixtureReceiveId)
        {
            return Query<FixtureReceiveDetail>().Join<FixtureReceive>((a, b) => a.FixtureReceiveId == b.Id && b.Id != fixtureReceiveId
                && b.ReceiveType == ReceiveType.Purchase && b.ReceiveBillStatus == ReceiveBillStatus.ToBeSubmitted)
                .Where(p => p.PurchaseOrderItemId == purchaseOrderItemId && !p.Giveaway).Select(p => p.Qty).ToList<int>().Sum();
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList Fetch(FixtureReceiveCriteria criteria)
        {
            var query = Query<FixtureReceive>();
            if (criteria.FactoryId.HasValue)
            {
                query.Where(p => p.FactoryId == criteria.FactoryId.Value);
            }
            if (criteria.DepartmentId.HasValue)
            {
                query.Where(p => p.DepartmentId == criteria.DepartmentId.Value);
            }
            if (criteria.CustomerId.HasValue || criteria.SupplierId.HasValue || !criteria.PurchaseOrderNo.IsNullOrWhiteSpace())
            {
                query.Join<FixtureReceiveDetail>("f", (x, y) => y.FixtureReceiveId == x.Id);
            }
            if (!criteria.PurchaseOrderNo.IsNullOrWhiteSpace())
            {
                query.Join<FixtureReceiveDetail, PurchaseOrder>("f1", (m, n) => n.Id == m.PurchaseOrderId
                   && n.OrderNo.Contains(criteria.PurchaseOrderNo)).Where<FixtureReceiveDetail>((m, n) => m.Id == n.FixtureReceiveId);
            }
            if (criteria.ReceiveType.HasValue)
            {
                query.Where(p => p.ReceiveType == criteria.ReceiveType.Value);
            }
            if (criteria.SupplierId.HasValue)
            {
                query.Where<FixtureReceiveDetail>((x, y) => y.SupplierId == criteria.SupplierId);
            }
            if (criteria.CustomerId.HasValue)
            {
                query.Where<FixtureReceiveDetail>((x, y) => y.CustomerId == criteria.CustomerId);
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
            return query.Distinct().OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取接收SN信息
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="sortInfo"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual EntityList<FixtureReceiveSn> GetReceiveSnInfo(double parentId, IList<OrderInfo> sortInfo, PagingInfo pagingInfo)
        {
            return Query<FixtureReceiveSn>().Join<FixtureReceiveDetail>((x, y) => x.FixtureReceiveDetailId == y.Id && y.FixtureReceiveId == parentId)
            .OrderBy(sortInfo).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 创建一个新的工治具接收
        /// </summary>
        /// <returns>新的设备接收</returns>
        public virtual FixtureReceive GetNewFixtureReceive()
        {
            var entity = new FixtureReceive();
            entity.ReceiveNo = RT.Service.Resolve<CommonController>().GetNo<FixtureReceive>("工治具接收");
            entity.ReceiveBillStatus = ReceiveBillStatus.ToBeSubmitted;
            entity.ReceiveType = ReceiveType.Purchase;
            return entity;
        }

        /// <summary>
        /// 明细获取仓库
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="fixtureEncodeId"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<Warehouse> DetailGetWarehouses(FixtureReceiveDetail entity, PagingInfo pagingInfo, string keyword)
        {
            var fixtureEncode = RF.GetById<FixtureEncode>(entity.FixtureEncodeId);
            if (fixtureEncode == null)
            {
                return new EntityList<Warehouse>();
            }
            var ctr = RT.Service.Resolve<CoreFixtureController>();
            //1 查找选择的工治具编码是否存在存储位置
            //2 查找是否存在工治具类型仓库配置
            //3 以上均没有则返回所有仓库
            var storageLocations = ctr.GetFixtureEncodeStorageLocationsByEncodeId(fixtureEncode.Id);
            if (storageLocations.Any())
            {
                var whIds = storageLocations.Select(m => m.WarehouseId).ToList();
                return Query<Warehouse>().Where(m=>whIds.Contains(m.Id))
                    .WhereIf(!keyword.IsNullOrWhiteSpace(), p => p.Code.Contains(keyword)).ToList(pagingInfo);
            }

            var whs = ctr.GetFixtureEncodeConfigWarehouses(keyword, pagingInfo);
            if (whs.Any())
                return whs;
            return Query<Warehouse>().WhereIf(!keyword.IsNullOrWhiteSpace(), p => p.Code.Contains(keyword)).ToList(pagingInfo);
        }


        /// <summary>
        /// 根据ID获取工治具接收明细
        /// </summary>
        /// <param name="fixtureReceiveDetailId"></param>
        /// <returns></returns>
        public virtual FixtureReceiveDetail GetFixtureReceiveDetailById(double fixtureReceiveDetailId)
        {
            return RF.GetById<FixtureReceiveDetail>(fixtureReceiveDetailId, new EagerLoadOptions().LoadWithViewProperty());
        }


    }
}
