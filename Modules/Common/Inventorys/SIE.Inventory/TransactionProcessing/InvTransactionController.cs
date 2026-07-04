using SIE.Common.InvOrg;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages;
using SIE.Inventory.Commom;
using SIE.Inventory.Onhands;
using SIE.Inventory.TransactionProcessing;
using SIE.Items;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Inventory.Transactions
{
    /// <summary>
    /// 库存交易控制器
    /// </summary>
    public partial class InvTransactionController : DomainController
    {
        /// <summary>
        /// 查询库存交易
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>库存交易数据</returns>
        public virtual EntityList<InvTransaction> GetInvTransactions(InvTransactionCriteria criteria)
        {
            var query = Query<InvTransaction>();
            if (criteria.Id.HasValue)
                query.Where(p => p.Id == criteria.Id);
            if (criteria.BillNo.IsNotEmpty())
                query.Where(p => p.BillNo.Contains(criteria.BillNo));
            if (criteria.ItemId > 0)
                query.Where(p => p.ItemId == criteria.ItemId);
            if (criteria.WarehouseId > 0)
                query.Join<Warehouse>("sh", (t, sh) => (t.ToWarehouseId == sh.Id || t.FromWarehouseId == sh.Id) && sh.Id == criteria.WarehouseId);
            if (criteria.StorageLocationId > 0)
                query.Join<StorageLocation>("sl", (t, sl) => (t.FromLocationId == sl.Id || t.ToLocationId == sl.Id) && sl.Id == criteria.StorageLocationId);
            if (criteria.LotId > 0)
                query.Where(p => p.LotId == criteria.LotId);
            if (criteria.Lpn.IsNotEmpty())
                query.Where(p => criteria.Lpn.Contains(p.ToLpn) || criteria.Lpn.Contains(p.FromLpn));
            if (criteria.CustomerId > 0)
                query.Where(p => p.CustomerId == criteria.CustomerId);
            if (criteria.SupplierId > 0)
                query.Where(p => p.SupplierId == criteria.SupplierId);
            if (criteria.ProjectNo.IsNotEmpty())
                query.Where(p => p.ProjectNo == criteria.ProjectNo);
            if (criteria.TransactionType.HasValue)
                query.Where(p => p.TransactionType == criteria.TransactionType);
            if (criteria.OrderType.HasValue)
                query.Where(p => p.OrderType == criteria.OrderType);
            if (criteria.TransactionId > 0)
                query.Where(p => p.TransactionId == criteria.TransactionId);
            if (criteria.CreateBy != null)
                query.Where(p => p.CreateBy == criteria.CreateById);
            if (criteria.TransactionDate.BeginValue.HasValue)
            {
                query.Where(p => p.TransactionDate >= criteria.TransactionDate.BeginValue);
            }

            if (criteria.TransactionDate.EndValue.HasValue)
            {
                query.Where(p => p.TransactionDate <= criteria.TransactionDate.EndValue);
            }

            if (criteria.CreateDate.BeginValue.HasValue)
            {
                query.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue);
            }

            if (criteria.CreateDate.EndValue.HasValue)
            {
                query.Where(p => p.CreateDate <= criteria.CreateDate.EndValue);
            }

            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(BaseTransaction.ItemProperty);
            elo.LoadWith(BaseTransaction.UnitProperty);
            elo.LoadWith(BaseTransaction.TransactionProperty);
            elo.LoadWith(BaseTransaction.FromWarehouseProperty);
            elo.LoadWith(BaseTransaction.ToWarehouseProperty);
            elo.LoadWith(BaseTransaction.FromLocationProperty);
            elo.LoadWith(BaseTransaction.ToLocationProperty);
            elo.LoadWith(TransactionProcessing.InvTransaction.CustomerProperty);
            elo.LoadWith(TransactionProcessing.InvTransaction.SupplierProperty);
            elo.LoadWithViewProperty();
            query.OrderBy(criteria.OrderInfoList);
            return query.ToList(criteria.PagingInfo, elo);
        }

        /// <summary>
        /// 验证有效性
        /// </summary>
        /// <param name="data">库存事务交易数据有效性</param>
        private void Validate(InvCollectData data)
        {
            if (data.item == null)
                throw new ValidationException("物料为空，交易失败".L10N());

            if (data.lot == null)
                throw new ValidationException("批次为空，交易失败".L10N());

            if (data.stockTrans == null)
                throw new ValidationException("交易双方参数为空，交易失败".L10N());

            if (data.stockTrans.Qty == 0)
                throw new ValidationException("交易数量为0，交易失败".L10N());
        }

        /// <summary>
        /// 验证交易双方参数
        /// </summary>
        /// <param name="sourceLocation">自库位</param>
        /// <param name="targetLocation">至库位</param>
        /// <param name="stockTrans">交易双方参数</param>
        /// <exception cref="ValidationException">
        /// 来源库位已冻结
        /// or
        /// 来源库位所属库区已冻结
        /// or
        /// 来源库位所属仓库已冻结
        /// or
        /// 目标库位已冻结
        /// or
        /// 目标库位[所属库区已冻结
        /// or
        /// 目标库位所属仓库已冻结
        /// </exception>
        private void ValidateLocation(StorageLocation sourceLocation, StorageLocation targetLocation, StockTrans stockTrans)
        {
            if (sourceLocation != null)
            {
                if (sourceLocation.IsFrozen)
                    throw new ValidationException("来源库位[{0}]已冻结".L10nFormat(sourceLocation.Code));
                if (sourceLocation.AreaIsFrozen)
                    throw new ValidationException("来源库位[{0}]所属库区[{1}]已冻结".L10nFormat(sourceLocation.Code, sourceLocation.Area.Code));
                if (sourceLocation.WarehouseIsFrozen)
                    throw new ValidationException("来源库位[{0}]所属仓库[{1}]已冻结".L10nFormat(sourceLocation.Code, sourceLocation.Warehouse.Code));
                if (!stockTrans.FromOnhandState.HasValue)
                    throw new ValidationException("来源库存状态不能为空".L10N());
            }

            if (targetLocation != null)
            {
                if (targetLocation.IsFrozen)
                    throw new ValidationException("目标库位[{0}]已冻结".L10nFormat(targetLocation.Code));
                if (targetLocation.AreaIsFrozen)
                    throw new ValidationException("目标库位[{0}]所属库区[{1}]已冻结".L10nFormat(targetLocation.Code, targetLocation.Area.Code));
                if (targetLocation.WarehouseIsFrozen)
                    throw new ValidationException("目标库位[{0}]所属仓库[{1}]已冻结".L10nFormat(targetLocation.Code, targetLocation.Warehouse.Code));
            }
        }

        /// <summary>
        /// 保存库存交易记录
        /// </summary>
        /// <param name="item">物料</param>
        /// <param name="lot">批次</param>
        /// <param name="qty">数量</param>
        /// <param name="stockTrans">库存交易双方参数</param>
        /// <param name="invTransaction">库存交易记录</param>
        /// <param name="baseTransactionData">交易记录相关数据</param>
        private void SaveInvTransaction(Item item, Lot lot, decimal qty, StockTrans stockTrans, BaseTransaction invTransaction, BaseTransactionData baseTransactionData)
        {
            invTransaction.OrderType = baseTransactionData.OrderType;
            invTransaction.TransactionId = baseTransactionData.TransactionId;
            invTransaction.TransactionType = baseTransactionData.TransactionType;
            invTransaction.SpecialTransMark = baseTransactionData.SpecialTransMark;
            invTransaction.Item = item;
            invTransaction.ItemCode = item.Code;
            invTransaction.ItemName = item.Name;
            ////invTransaction.ItemDesc = item.Description;
            invTransaction.UnitId = item.UnitId.Value;
            invTransaction.StorerCode = baseTransactionData.StorerCode;
            invTransaction.Lot = lot;
            invTransaction.LotCode = lot.Code;
            invTransaction.FromLpn = stockTrans.FromLpn;
            invTransaction.FromOnhandSate = stockTrans.FromOnhandState;
            //目标货主默认是来源货主
            invTransaction.ToStorerCode = stockTrans.ToStorerCode.IsNullOrEmpty() ? baseTransactionData.StorerCode : stockTrans.ToStorerCode;
            invTransaction.ToLpn = stockTrans.ToLpn;
            invTransaction.ToOnhandState = stockTrans.ToOnhandState;
            invTransaction.Qty = qty;
            invTransaction.TransactionDate = baseTransactionData.TransactionDate ?? DateTime.Now;
            invTransaction.UploadFlag = false;
            invTransaction.BillId = baseTransactionData.BillId;
            invTransaction.BillNo = baseTransactionData.BillNo;
            invTransaction.BillDtlId = baseTransactionData.BillDetailId;
            invTransaction.BillDtlNo = baseTransactionData.BillDetailNo;
            invTransaction.SecondDtlId = baseTransactionData.SecondDtlId;
            invTransaction.SecondDtlLineNo = baseTransactionData.SecondDtlLineNo;
            invTransaction.ProjectNo = baseTransactionData.ProjectNo;
            invTransaction.TaskNo = baseTransactionData.TaskNo;
            invTransaction.EmployeeNo = baseTransactionData.EmployeeNo;
            invTransaction.Remark = baseTransactionData.Remark;
            invTransaction.SourceBillId = baseTransactionData.SourceBillId;
            invTransaction.SourceBillNo = baseTransactionData.SourceBillNo;
            invTransaction.SourceBillDtlId = baseTransactionData.SourceBillDtlId;
            invTransaction.SourceBillDtlNo = baseTransactionData.SourceBillDtlNo;
            invTransaction.Reason = baseTransactionData.Reason;
            invTransaction.ErpOrganizationName = baseTransactionData.ErpOrganizationName;
            invTransaction.ErpOrgName = baseTransactionData.ErpOrgName;
            invTransaction.ErpWarehouseCode = baseTransactionData.ErpWarehouseCode;
            invTransaction.TargetErpWarehouseCode = baseTransactionData.TargetErpWarehouseCode;
            invTransaction.TargetErpOrganizationName = baseTransactionData.TargetErpOrganizationName;
            invTransaction.OrderNo = baseTransactionData.OrderNo;
            invTransaction.OrderLineNo = baseTransactionData.OrderLineNo;
            invTransaction.ErpAccount = baseTransactionData.ErpAccount;
            invTransaction.AsnNo = baseTransactionData.AsnNo;
            invTransaction.AsnLineNo = baseTransactionData.AsnLineNo;
            if (baseTransactionData.PoId.HasValue)
            {
                invTransaction.PoNo = baseTransactionData.PoNo;
                invTransaction.PoId = baseTransactionData.PoId.Value;
                invTransaction.PoLineNo = baseTransactionData.PoLineNo;
                invTransaction.PoLineId = baseTransactionData.PoLineId;
            }
        }

        /// <summary>
        /// 交易，其他单据等需要额外保存交易数据的，可以重写此方法
        /// </summary>
        /// <param name="invTransaction">交易记录</param>
        /// <param name="baseTransactionData">交易记录相关数据</param>
        public virtual void SaveOptionalData(BaseTransaction invTransaction, BaseTransactionData baseTransactionData)
        {
            //交易，其他单据等需要额外保存交易数据的，可以重写此方法
        }

        /// <summary>
        /// 事务交易接口
        /// </summary>
        /// <param name="datas">库存事务交易数据集合</param>
        public virtual void InvTransaction(List<InvCollectData> datas)
        {
            if (datas == null || datas.Count == 0)
            {
                return;
            }
            var invCtl = RT.Service.Resolve<InvOnhandController>();
            var whCtl = RT.Service.Resolve<WarehouseController>();
            List<double> fromLocIdList = datas.Select(p => p.stockTrans.FromLocationId).Distinct().ToList();
            List<double> toLocIdList = datas.Select(p => p.stockTrans.ToLocationId).Distinct().ToList();

            var locList = whCtl.GetStorageLocations(fromLocIdList.Union(toLocIdList).Distinct().ToList(), new EagerLoadOptions().LoadWithViewProperty());

            //标识提交行在进行库存事务处理时，是否需要验证LPN多库位存放
            //如果存在整LPN移动的情况，则只验证该LPN最后一行提交记录
            System.Collections.Hashtable lpnHash = new System.Collections.Hashtable();
            datas.ForEach(p =>
            {
                if (p.stockTrans.ToLpn != null && p.stockTrans.ToLpn != "*" && !lpnHash.ContainsKey(p.stockTrans.ToLpn))
                {
                    lpnHash.Add(p.stockTrans.ToLpn, 1);
                    p.isValidateMulLoc = true;
                }
            });

            EntityList<InvTransaction> invTransactions = new EntityList<InvTransaction>();
            foreach (var data in datas.OrderBy(p => p.isValidateMulLoc))
            {
                InvTransaction invTransaction = new InvTransaction();
                StorageLocation sourceLocation = locList.FirstOrDefault(p => p.Id == data.stockTrans.FromLocationId);
                StorageLocation targetLocation = locList.FirstOrDefault(p => p.Id == data.stockTrans.ToLocationId);
                var qty = data.stockTrans.Qty;
                Validate(data);
                ValidateLocation(sourceLocation, targetLocation, data.stockTrans);
                InvOptionalParam param = new InvOptionalParam()
                {
                    StorerCode = data.baseTransactionData.StorerCode,
                    ProjectNo = data.baseTransactionData.ProjectNo,
                    TaskNo = data.baseTransactionData.TaskNo,
                    State = OnhandState.Ok,
                    ItemExtProp = data.ItemExtProp,
                    ItemExtPropName = data.ItemExtPropName,
                    IsIgnoreItemExtProp = data.IsIgnoreItemExtProp
                };

                LotLpnOnhand fromOnhand = null;
                if (sourceLocation != null)
                {
                    param.State = data.stockTrans.FromOnhandState.Value;
                    param.Lpn = data.stockTrans.FromLpn;
                    if (data.stockTrans.FromStorerCode.IsNotEmpty())
                        param.StorerCode = data.stockTrans.FromStorerCode;
                    fromOnhand = invCtl.AdjustOnhand(AdjustType.From, sourceLocation, data.item, data.lot, -qty, param, data.isFromAllottedQty, false, data.isValidateState, data.isReceive, data.isNotCheckNone);
                    param.StorerCode = data.baseTransactionData.StorerCode;//在这里重置货主值，不然会影响目标库存
                }

                if (targetLocation != null)
                {
                    param.Lpn = data.stockTrans.ToLpn;
                    if (data.stockTrans.ToOnhandState.HasValue)
                    {
                        //获取传入库存状态
                        param.State = data.stockTrans.ToOnhandState.Value;
                    }
                    else
                    {
                        //有来源，取来源，没有则默认OK
                        param.State = fromOnhand == null ? OnhandState.Ok : fromOnhand.State;
                    }
                    if (data.stockTrans.ToStorerCode.IsNotEmpty())
                        param.StorerCode = data.stockTrans.ToStorerCode;
                    invCtl.AdjustOnhand(AdjustType.To, targetLocation, data.item, data.lot, qty, param, data.isToAllottedQty, data.isValidateMulLoc, false, data.isReceive, data.isNotCheckNone);
                }

                var invData = data.baseTransactionData;
                invTransaction.FromWarehouseId = sourceLocation?.WarehouseId;
                invTransaction.FromAreaId = sourceLocation?.AreaId;
                invTransaction.FromLocationId = sourceLocation?.Id;
                invTransaction.ToWarehouseId = targetLocation?.WarehouseId;
                invTransaction.ToAreaId = targetLocation?.AreaId;
                invTransaction.ToLocationId = targetLocation?.Id;
                invTransaction.CustomerId = invData.CustomerId;
                invTransaction.EnterpriseId = invData.EnterpriseId;
                invTransaction.TransactionId = invData.TransactionId;
                invTransaction.SupplierId = invData.SupplierId;
                invTransaction.ItemExtProp = data.ItemExtProp;
                invTransaction.ItemExtPropName = data.ItemExtPropName;
                invTransaction.ConvertFigre = invData.ConvertFigre;
                invTransaction.PurchaseUnit = invData.PurchaseUnit;
                invTransaction.PurchaseQty = invData.PurchaseQty;
                invTransaction.AllotModel = invData.AllotModel;
                SaveInvTransaction(data.item, data.lot, Math.Abs(qty), data.stockTrans, invTransaction, invData);
                SaveOptionalData(invTransaction, invData);
                //if (data.InvOrgId.HasValue)
                //{
                //    invTransaction.SetInvOrgId(data.InvOrgId);
                //}
                invTransactions.Add(invTransaction);
            }

            ////批量保存
            RT.Service.Resolve<SIE.Core.Common.Controllers.CommonController>().BatchInsertSave(invTransactions);
        }

        /// <summary>
        /// 分配,取消分配,关闭事务交易接口
        /// </summary>
        /// <param name="datas">库存事务交易数据集合</param>
        public virtual void AllotInvTransaction(List<InvCollectData> datas)
        {
            var invCtl = RT.Service.Resolve<InvOnhandController>();
            var whCtl = RT.Service.Resolve<WarehouseController>();
            List<double> fromLocIdList = datas.Select(p => p.stockTrans.FromLocationId).Distinct().ToList();
            List<double> toLocIdList = datas.Select(p => p.stockTrans.ToLocationId).Distinct().ToList();

            EagerLoadOptions elo = new EagerLoadOptions();

            var locList = whCtl.GetStorageLocations(fromLocIdList.Union(toLocIdList).Distinct().ToList(), new EagerLoadOptions().LoadWithViewProperty());
            EntityList<InvTransaction> invTransactions = new EntityList<InvTransaction>();
            foreach (var data in datas.OrderBy(p => p.isValidateMulLoc))
            {
                InvTransaction invTransaction = new InvTransaction();
                StorageLocation sourceLocation = locList.FirstOrDefault(p => p.Id == data.stockTrans.FromLocationId);
                StorageLocation targetLocation = locList.FirstOrDefault(p => p.Id == data.stockTrans.ToLocationId);
                var qty = data.stockTrans.Qty;
                Validate(data);
                ValidateLocation(sourceLocation, targetLocation, data.stockTrans);
                InvOptionalParam param = new InvOptionalParam()
                {
                    StorerCode = data.baseTransactionData.StorerCode,
                    ProjectNo = data.baseTransactionData.ProjectNo,
                    TaskNo = data.baseTransactionData.TaskNo,
                    ItemExtProp = data.ItemExtProp,
                    ItemExtPropName = data.ItemExtPropName,
                    State = OnhandState.Ok,
                    IsIgnoreItemExtProp = data.IsIgnoreItemExtProp,
                };

                if (sourceLocation != null)
                {
                    param.State = data.stockTrans.FromOnhandState.HasValue ? data.stockTrans.FromOnhandState.Value : OnhandState.Ok;
                    param.Lpn = data.stockTrans.FromLpn;
                    invCtl.AllotOnhand(sourceLocation, data.item, data.lot, qty, param);
                }

                invTransaction.FromWarehouseId = sourceLocation?.WarehouseId;
                invTransaction.FromAreaId = sourceLocation?.AreaId;
                invTransaction.FromLocationId = sourceLocation?.Id;
                invTransaction.ToWarehouseId = targetLocation?.WarehouseId;
                invTransaction.ToAreaId = targetLocation?.AreaId;
                invTransaction.ToLocationId = targetLocation?.Id;
                invTransaction.ItemExtProp = data.ItemExtProp;
                invTransaction.ItemExtPropName = data.ItemExtPropName;
                invTransaction.ConvertFigre = data.baseTransactionData.ConvertFigre;
                invTransaction.PurchaseUnit = data.baseTransactionData.PurchaseUnit;
                invTransaction.PurchaseQty = data.baseTransactionData.PurchaseQty;
                SaveInvTransaction(data.item, data.lot, Math.Abs(qty), data.stockTrans, invTransaction, data.baseTransactionData);
                SaveOptionalData(invTransaction, data.baseTransactionData);
                invTransactions.Add(invTransaction);
            }
            RT.Service.Resolve<SIE.Core.Common.Controllers.CommonController>().BatchInsertSave(invTransactions);
        }

        /// <summary>
        /// 冻结，解冻事务交易接口
        /// </summary>
        /// <param name="datas">库存事务交易数据集合</param>
        public virtual void FrozenInvTransaction(List<InvCollectData> datas)
        {
            var invCtl = RT.Service.Resolve<InvOnhandController>();
            var whCtl = RT.Service.Resolve<WarehouseController>();
            List<double> fromLocIdList = datas.Select(p => p.stockTrans.FromLocationId).Distinct().ToList();
            List<double> toLocIdList = datas.Select(p => p.stockTrans.ToLocationId).Distinct().ToList();

            EagerLoadOptions elo = new EagerLoadOptions();

            var locList = whCtl.GetStorageLocations(fromLocIdList.Union(toLocIdList).Distinct().ToList(), new EagerLoadOptions().LoadWithViewProperty());
            EntityList<InvTransaction> invTransactions = new EntityList<InvTransaction>();
            foreach (var data in datas.OrderBy(p => p.isValidateMulLoc))
            {
                InvTransaction invTransaction = new InvTransaction();
                StorageLocation sourceLocation = locList.FirstOrDefault(p => p.Id == data.stockTrans.FromLocationId);
                StorageLocation targetLocation = locList.FirstOrDefault(p => p.Id == data.stockTrans.ToLocationId);
                var qty = data.stockTrans.Qty;
                Validate(data);
                ValidateLocation(sourceLocation, targetLocation, data.stockTrans);
                InvOptionalParam param = new InvOptionalParam()
                {
                    StorerCode = data.baseTransactionData.StorerCode,
                    ProjectNo = data.baseTransactionData.ProjectNo,
                    TaskNo = data.baseTransactionData.TaskNo,
                    State = OnhandState.Ok,
                    ItemExtProp = data.ItemExtProp,
                    ItemExtPropName = data.ItemExtPropName
                };

                if (sourceLocation != null)
                {
                    param.State = data.stockTrans.FromOnhandState.HasValue ? data.stockTrans.FromOnhandState.Value : OnhandState.Ok;
                    param.Lpn = data.stockTrans.FromLpn;
                    invCtl.FrozenOnhand(sourceLocation, data.item, data.lot, qty, param);
                }

                invTransaction.FromWarehouseId = sourceLocation?.WarehouseId;
                invTransaction.FromAreaId = sourceLocation?.AreaId;
                invTransaction.FromLocationId = sourceLocation?.Id;
                invTransaction.ToWarehouseId = targetLocation?.WarehouseId;
                invTransaction.ToAreaId = targetLocation?.AreaId;
                invTransaction.ToLocationId = targetLocation?.Id;
                invTransaction.ItemExtProp = data.ItemExtProp;
                invTransaction.ItemExtPropName = data.ItemExtPropName;
                SaveInvTransaction(data.item, data.lot, Math.Abs(qty), data.stockTrans, invTransaction, data.baseTransactionData);
                SaveOptionalData(invTransaction, data.baseTransactionData);

                invTransactions.Add(invTransaction);

                //新增库存事务交易物料扩展属性数据
                ////if (data.ItemExtProp.IsNotEmpty())
                ////    SaveInvTransItemExtPropValues(invTransaction.Id, data.item.Id, data.ItemExtProp);
            }
            RT.Service.Resolve<SIE.Core.Common.Controllers.CommonController>().BatchInsertSave(invTransactions);
        }

        /// <summary>
        /// 直接扣减来源库位可用数
        /// </summary>
        /// <param name="datas">库存事务交易数据集合</param>
        public virtual void DeductInvTransaction(List<InvCollectData> datas)
        {
            var invCtl = RT.Service.Resolve<InvOnhandController>();
            var whCtl = RT.Service.Resolve<WarehouseController>();
            List<double> fromLocIdList = datas.Select(p => p.stockTrans.FromLocationId).Distinct().ToList();
            List<double> toLocIdList = datas.Select(p => p.stockTrans.ToLocationId).Distinct().ToList();

            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();
            var locList = whCtl.GetStorageLocations(fromLocIdList.Union(toLocIdList).Distinct().ToList(), elo);
            EntityList<InvTransaction> invTransactions = new EntityList<InvTransaction>();
            foreach (var data in datas.OrderBy(p => p.isValidateMulLoc))
            {
                InvTransaction invTransaction = new InvTransaction();
                StorageLocation sourceLocation = locList.FirstOrDefault(p => p.Id == data.stockTrans.FromLocationId);
                StorageLocation targetLocation = locList.FirstOrDefault(p => p.Id == data.stockTrans.ToLocationId);
                var qty = data.stockTrans.Qty;
                Validate(data);
                ValidateLocation(sourceLocation, targetLocation, data.stockTrans);
                InvOptionalParam param = new InvOptionalParam()
                {
                    StorerCode = data.baseTransactionData.StorerCode,
                    ProjectNo = data.baseTransactionData.ProjectNo,
                    TaskNo = data.baseTransactionData.TaskNo,
                    ItemExtProp = data.ItemExtProp,
                    ItemExtPropName = data.ItemExtPropName,
                    State = OnhandState.Ok
                };

                if (sourceLocation != null)
                {
                    param.State = data.stockTrans.FromOnhandState.HasValue ? data.stockTrans.FromOnhandState.Value : OnhandState.Ok;
                    param.Lpn = data.stockTrans.FromLpn;
                    invCtl.UpdateOnhandAvailableQty(sourceLocation, data.item, data.lot, qty, param);
                }

                invTransaction.FromWarehouseId = sourceLocation?.WarehouseId;
                invTransaction.FromAreaId = sourceLocation?.AreaId;
                invTransaction.FromLocationId = sourceLocation?.Id;
                invTransaction.ToWarehouseId = targetLocation?.WarehouseId;
                invTransaction.ToAreaId = targetLocation?.AreaId;
                invTransaction.ToLocationId = targetLocation?.Id;
                invTransaction.CustomerId = data.baseTransactionData.CustomerId;
                invTransaction.EnterpriseId = data.baseTransactionData.EnterpriseId;
                invTransaction.SupplierId = data.baseTransactionData.SupplierId;
                invTransaction.ItemExtProp = data.ItemExtProp;
                invTransaction.ItemExtPropName = data.ItemExtPropName;
                invTransaction.ConvertFigre = data.baseTransactionData.ConvertFigre;
                invTransaction.PurchaseUnit = data.baseTransactionData.PurchaseUnit;
                invTransaction.PurchaseQty = data.baseTransactionData.PurchaseQty;
                SaveInvTransaction(data.item, data.lot, Math.Abs(qty), data.stockTrans, invTransaction, data.baseTransactionData);
                SaveOptionalData(invTransaction, data.baseTransactionData);

                invTransactions.Add(invTransaction);

                //新增库存事务交易物料扩展属性数据
                ////if (data.ItemExtProp.IsNotEmpty())
                ////    SaveInvTransItemExtPropValues(invTransaction.Id, data.item.Id, data.ItemExtProp);
            }
            RT.Service.Resolve<SIE.Core.Common.Controllers.CommonController>().BatchInsertSave(invTransactions);
        }

        /// <summary>
        /// 只写事务交易，应用于MES挪料到新库位，需要创建完成的收发单，做记录
        /// </summary>
        /// <param name="datas">库存事务交易数据集合</param>
        public virtual void OnlyInvTransaction(List<InvCollectData> datas)
        {
            if (datas == null || datas.Count == 0)
            {
                return;
            }

            var whCtl = RT.Service.Resolve<WarehouseController>();
            List<double> fromLocIdList = datas.Select(p => p.stockTrans.FromLocationId).Distinct().ToList();
            List<double> toLocIdList = datas.Select(p => p.stockTrans.ToLocationId).Distinct().ToList();

            var locList = whCtl.GetStorageLocations(fromLocIdList.Union(toLocIdList).Distinct().ToList(), new EagerLoadOptions().LoadWithViewProperty());

            //标识提交行在进行库存事务处理时，是否需要验证LPN多库位存放
            //如果存在整LPN移动的情况，则只验证该LPN最后一行提交记录
            System.Collections.Hashtable lpnHash = new System.Collections.Hashtable();
            datas.ForEach(p =>
            {
                if (p.stockTrans.ToLpn != null && p.stockTrans.ToLpn != "*" && !lpnHash.ContainsKey(p.stockTrans.ToLpn))
                {
                    lpnHash.Add(p.stockTrans.ToLpn, 1);
                    p.isValidateMulLoc = true;
                }
            });

            EntityList<InvTransaction> invTransactions = new EntityList<InvTransaction>();
            foreach (var data in datas.OrderBy(p => p.isValidateMulLoc))
            {
                InvTransaction invTransaction = new InvTransaction();
                StorageLocation sourceLocation = locList.FirstOrDefault(p => p.Id == data.stockTrans.FromLocationId);
                StorageLocation targetLocation = locList.FirstOrDefault(p => p.Id == data.stockTrans.ToLocationId);
                var qty = data.stockTrans.Qty;

                invTransaction.FromWarehouseId = sourceLocation?.WarehouseId;
                invTransaction.FromAreaId = sourceLocation?.AreaId;
                invTransaction.FromLocationId = sourceLocation?.Id;
                invTransaction.ToWarehouseId = targetLocation?.WarehouseId;
                invTransaction.ToAreaId = targetLocation?.AreaId;
                invTransaction.ToLocationId = targetLocation?.Id;
                invTransaction.CustomerId = data.baseTransactionData.CustomerId;
                invTransaction.EnterpriseId = data.baseTransactionData.EnterpriseId;
                invTransaction.TransactionId = data.baseTransactionData.TransactionId;
                invTransaction.SupplierId = data.baseTransactionData.SupplierId;
                invTransaction.ItemExtProp = data.ItemExtProp;
                invTransaction.ItemExtPropName = data.ItemExtPropName;
                invTransaction.ConvertFigre = data.baseTransactionData.ConvertFigre;
                invTransaction.PurchaseUnit = data.baseTransactionData.PurchaseUnit;
                invTransaction.PurchaseQty = data.baseTransactionData.PurchaseQty;

                SaveInvTransaction(data.item, data.lot, Math.Abs(qty), data.stockTrans, invTransaction, data.baseTransactionData);
                SaveOptionalData(invTransaction, data.baseTransactionData);

                invTransactions.Add(invTransaction);
            }

            ////批量保存
            RT.Service.Resolve<SIE.Core.Common.Controllers.CommonController>().BatchInsertSave(invTransactions);
        }

        /// <summary>
        /// 只更新库存，事务交易再另外写，应用于MES退料会先创发运单多笔发货事务交易、库存更新统一更新（扣减线边库位的库存）
        /// </summary>
        /// <param name="datas">库存事务交易数据集合</param>
        public virtual void OnlyInvOnhand(List<InvCollectData> datas)
        {
            if (datas == null || datas.Count == 0)
            {
                return;
            }
            var invCtl = RT.Service.Resolve<InvOnhandController>();
            var whCtl = RT.Service.Resolve<WarehouseController>();
            List<double> fromLocIdList = datas.Select(p => p.stockTrans.FromLocationId).Distinct().ToList();
            List<double> toLocIdList = datas.Select(p => p.stockTrans.ToLocationId).Distinct().ToList();

            var locList = whCtl.GetStorageLocations(fromLocIdList.Union(toLocIdList).Distinct().ToList(), new EagerLoadOptions().LoadWithViewProperty());

            //标识提交行在进行库存事务处理时，是否需要验证LPN多库位存放
            //如果存在整LPN移动的情况，则只验证该LPN最后一行提交记录
            System.Collections.Hashtable lpnHash = new System.Collections.Hashtable();
            datas.ForEach(p =>
            {
                if (p.stockTrans.ToLpn != null && p.stockTrans.ToLpn != "*" && !lpnHash.ContainsKey(p.stockTrans.ToLpn))
                {
                    lpnHash.Add(p.stockTrans.ToLpn, 1);
                    p.isValidateMulLoc = true;
                }
            });

            foreach (var data in datas.OrderBy(p => p.isValidateMulLoc))
            {
                StorageLocation sourceLocation = locList.FirstOrDefault(p => p.Id == data.stockTrans.FromLocationId);
                StorageLocation targetLocation = locList.FirstOrDefault(p => p.Id == data.stockTrans.ToLocationId);
                var qty = data.stockTrans.Qty;
                Validate(data);
                ValidateLocation(sourceLocation, targetLocation, data.stockTrans);
                InvOptionalParam param = new InvOptionalParam()
                {
                    StorerCode = data.baseTransactionData.StorerCode,
                    ProjectNo = data.baseTransactionData.ProjectNo,
                    TaskNo = data.baseTransactionData.TaskNo,
                    State = OnhandState.Ok,
                    ItemExtProp = data.ItemExtProp,
                    ItemExtPropName = data.ItemExtPropName,
                    IsIgnoreItemExtProp = data.IsIgnoreItemExtProp
                };

                LotLpnOnhand fromOnhand = null;
                if (sourceLocation != null)
                {
                    param.State = data.stockTrans.FromOnhandState.Value;
                    param.Lpn = data.stockTrans.FromLpn;
                    fromOnhand = invCtl.AdjustOnhand(AdjustType.From, sourceLocation, data.item, data.lot, -qty, param, data.isFromAllottedQty, false, data.isValidateState, data.isReceive, data.isNotCheckNone);
                }

                if (targetLocation != null)
                {
                    param.Lpn = data.stockTrans.ToLpn;

                    if (data.stockTrans.ToOnhandState.HasValue)
                    {
                        //获取传入库存状态
                        param.State = data.stockTrans.ToOnhandState.Value;
                    }
                    else
                    {
                        //有来源，取来源，没有则默认OK
                        param.State = fromOnhand == null ? OnhandState.Ok : fromOnhand.State;
                    }

                    invCtl.AdjustOnhand(AdjustType.To, targetLocation, data.item, data.lot, qty, param, data.isToAllottedQty, data.isValidateMulLoc, false, data.isReceive, data.isNotCheckNone);
                }
            }
        }

        /// <summary>
        /// 获取事务交易数据
        /// </summary>
        /// <param name="TransactionTypeList">交易类型</param>
        /// <param name="orderTypes">单据大类</param>
        /// <returns></returns>
        public virtual EntityList<InvTransaction> GetInvTransactionData(List<TransactionType> TransactionTypeList, List<OrderType> orderTypes)
        {
            var query = Query<InvTransaction>().Where(p => !p.UploadFlag && TransactionTypeList.Contains(p.TransactionType) && orderTypes.Contains(p.OrderType));
            return query.ToList();
        }
    }
}
