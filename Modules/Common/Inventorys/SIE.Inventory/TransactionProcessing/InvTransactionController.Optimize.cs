using SIE.Common;
using SIE.Core.Common;
using SIE.Core.Enums;
using SIE.Core.Logs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Inventory.Onhands;
using SIE.Inventory.TransactionProcessing;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace SIE.Inventory.Transactions
{
    /// <summary>
    /// 性能优化控制器
    /// </summary>
    public partial class InvTransactionController
    {
        /// <summary>
        /// 分配,关闭事务交易接口
        /// </summary>
        /// <param name="datas">库存事务交易数据集合</param>
        public virtual void AllotInvTransactionOptimize(List<InvCollectData> datas)
        {
            if (datas.Count <= 50)
            {
                AllotInvTransaction(datas);
                return;
            }
            var invCtl = RT.Service.Resolve<InvOnhandController>();
            var whCtl = RT.Service.Resolve<WarehouseController>();
            List<double> fromLocIdList = datas.Select(p => p.stockTrans.FromLocationId).Distinct().ToList();
            List<double> toLocIdList = datas.Select(p => p.stockTrans.ToLocationId).Distinct().ToList();
            //获取库位数据
            var locList = whCtl.GetStorageLocations(fromLocIdList.Union(toLocIdList).Distinct().ToList(),
                new EagerLoadOptions().LoadWithViewProperty());
            //事务交易及库存参数
            EntityList<InvTransaction> invTransactions = new EntityList<InvTransaction>();
            List<InvOptionalParamForOp> optionalParams = new List<InvOptionalParamForOp>();
            foreach (var data in datas.OrderBy(p => p.isValidateMulLoc))
            {
                StorageLocation sourceLocation = locList.FirstOrDefault(p => p.Id == data.stockTrans.FromLocationId);
                if (sourceLocation == null)
                    continue;
                InvOptionalParamForOp paramOp = new InvOptionalParamForOp()
                {
                    StorerCode = data.baseTransactionData.StorerCode,
                    ProjectNo = data.baseTransactionData.ProjectNo,
                    TaskNo = data.baseTransactionData.TaskNo,
                    State = data.stockTrans.FromOnhandState.HasValue ? data.stockTrans.FromOnhandState.Value : OnhandState.Ok,
                    ItemId = data.item.Id,
                    LocId = sourceLocation.Id,
                    Lpn = data.stockTrans.FromLpn,
                };
                //分配用到的参数值
                optionalParams.Add(paramOp);
            }
            var itemIds = datas.Select(a => a.item.Id).Distinct().ToList();
            List<LotLpnOnhand> lotLpnOnhands = new List<LotLpnOnhand>();
            optionalParams.GroupBy(p => new { p.LocId, p.StorerCode, p.ProjectNo, p.State, p.TaskNo }).ForEach(p =>
            {
                lotLpnOnhands.AddRange(GetLotLpnOnhandForOp(itemIds, p.Key.LocId, p.Key.StorerCode, p.Key.ProjectNo, p.Key.TaskNo, p.Key.State).ToList());
            });

            //获取库存并设置库存的数据
            InvOptionalParam param = new InvOptionalParam()
            {
                LotLpnOnhands = lotLpnOnhands,
            };

            foreach (var data in datas.OrderBy(p => p.isValidateMulLoc))
            {
                InvTransaction invTransaction = new InvTransaction();
                StorageLocation sourceLocation = locList.FirstOrDefault(p => p.Id == data.stockTrans.FromLocationId);
                StorageLocation targetLocation = locList.FirstOrDefault(p => p.Id == data.stockTrans.ToLocationId);
                var qty = data.stockTrans.Qty;

                Validate(data);
                ValidateLocationLoadWith(sourceLocation, targetLocation, data.stockTrans);

                param.StorerCode = data.baseTransactionData.StorerCode;
                param.ProjectNo = data.baseTransactionData.ProjectNo;
                param.TaskNo = data.baseTransactionData.TaskNo;
                param.ItemExtProp = data.ItemExtProp;
                param.ItemExtPropName = data.ItemExtPropName;
                param.State = OnhandState.Ok;
                param.IsIgnoreItemExtProp = data.IsIgnoreItemExtProp;

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
        /// 事务交易接口,拣货调用，其他不能用这个
        /// </summary>
        /// <param name="datas">库存事务交易数据集合</param>
        public virtual void InvTransactionOptimize(List<InvCollectData> datas)
        {
            if (datas == null || datas.Count == 0)
            {
                return;
            }
            if (datas.Count <= 50)
            {
                InvTransaction(datas);
                return;
            }
            var invCtl = RT.Service.Resolve<InvOnhandController>();
            var whCtl = RT.Service.Resolve<WarehouseController>();
            List<double> fromLocIdList = datas.Select(p => p.stockTrans.FromLocationId).Distinct().ToList();
            List<double> toLocIdList = datas.Select(p => p.stockTrans.ToLocationId).Distinct().ToList();

            var locList = whCtl.GetStorageLocations(fromLocIdList.Union(toLocIdList).Distinct().ToList(), new EagerLoadOptions().LoadWithViewProperty());

            //标识提交行在进行库存事务处理时，是否需要验证LPN多库位存放
            //如果存在整LPN移动的情况，则只验证该LPN最后一行提交记录


            EntityList<InvTransaction> invTransactions = new EntityList<InvTransaction>();
            List<InvOptionalParamForOp> optionalParams = new List<InvOptionalParamForOp>();
            foreach (var data in datas.OrderBy(p => p.isValidateMulLoc))
            {
                StorageLocation sourceLocation = locList.FirstOrDefault(p => p.Id == data.stockTrans.FromLocationId);
                StorageLocation targetLocation = locList.FirstOrDefault(p => p.Id == data.stockTrans.ToLocationId);
                InvOptionalParamForOp paramOp = new InvOptionalParamForOp()
                {
                    StorerCode = data.baseTransactionData.StorerCode,
                    ProjectNo = data.baseTransactionData.ProjectNo,
                    TaskNo = data.baseTransactionData.TaskNo,
                    State = data.stockTrans.FromOnhandState.HasValue ? data.stockTrans.FromOnhandState.Value : OnhandState.Ok,
                    ItemId = data.item.Id,
                    LocId = sourceLocation?.Id ?? 0,
                    Lpn = data.stockTrans.FromLpn,
                    TarLocId = targetLocation?.Id ?? 0,
                };

                optionalParams.Add(paramOp);
            }
            var itemIds = datas.Select(a => a.item.Id).Distinct().ToList();
            List<LotLpnOnhand> lotLpnOnhands = new List<LotLpnOnhand>();
            optionalParams.GroupBy(p => new { p.LocId, p.StorerCode, p.ProjectNo, p.State, p.TaskNo, p.TarLocId }).ForEach(p =>
             {
                 lotLpnOnhands.AddRange(GetLotLpnOnhandForOp(itemIds, p.Key.LocId, p.Key.StorerCode, p.Key.ProjectNo, p.Key.TaskNo, p.Key.State, p.Key.TarLocId).ToList());
             });

            InvOptionalParam param = new InvOptionalParam()
            {
                LotLpnOnhands = lotLpnOnhands,
            };
            //并发问题，就在这里把库存锁了
            //var lpnOnhandIds = = lotLpnOnhands.Select(a => a.Id);
            //DataProcessEx.SplitDataExecute(lpnOnhandIds, ids =>
            //{
            //    DB.Update<LotLpnOnhand>().Where(p => ids.Select(a => a.Id).Contains(p.Id)).Execute();
            //});

            foreach (var data in datas.OrderBy(p => p.isValidateMulLoc))
            {
                InvTransaction invTransaction = new InvTransaction();
                StorageLocation sourceLocation = locList.FirstOrDefault(p => p.Id == data.stockTrans.FromLocationId);
                StorageLocation targetLocation = locList.FirstOrDefault(p => p.Id == data.stockTrans.ToLocationId);
                var qty = data.stockTrans.Qty;
                Validate(data);
                ValidateLocation(sourceLocation, targetLocation, data.stockTrans);

                param.StorerCode = data.baseTransactionData.StorerCode;
                param.ProjectNo = data.baseTransactionData.ProjectNo;
                param.TaskNo = data.baseTransactionData.TaskNo;
                param.State = OnhandState.Ok;
                param.ItemExtProp = data.ItemExtProp;
                param.ItemExtPropName = data.ItemExtPropName;



                if (data.baseTransactionData.OrderType == OrderType.CustomerIn)
                    param.StorerCode = "*";

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
                    param.StorerCode = data.stockTrans.ToStorerCode;
                    invCtl.AdjustOnhand(AdjustType.To, targetLocation, data.item, data.lot, qty, param, data.isToAllottedQty, data.isValidateMulLoc, false, data.isReceive, data.isNotCheckNone);
                }


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

            var newOnhands_lpn = param.LotLpnOnhands.Where(f => f.PersistenceStatus == PersistenceStatus.New).AsEntityList();
            if (newOnhands_lpn.Any())
            {
                RT.Service.Resolve<SIE.Core.Common.Controllers.CommonController>().BatchInsertSave(newOnhands_lpn);
            }

            ////批量保存
            RT.Service.Resolve<SIE.Core.Common.Controllers.CommonController>().BatchInsertSave(invTransactions);

        }

        /// <summary>
        /// 性能优化库存获取
        /// </summary>
        public virtual EntityList<LotLpnOnhand> GetLotLpnOnhandForOp(List<double> itemIds, double? locId, string storerCode, string projectNo, string taskNo, OnhandState state, double? targetLocId = null)
        {
            //超过1000需要分批处理
            EntityList<LotLpnOnhand> result = itemIds.SplitContains(items =>
            {
                var query = Query<LotLpnOnhand>();
                List<double> stoLocId = new List<double>();
                if (locId.HasValue)
                    stoLocId.Add(locId.Value);
                if (targetLocId.HasValue)
                    stoLocId.Add(targetLocId.Value);
                query.Where(p => stoLocId.Contains(p.StorageLocationId) && p.State == state && items.Contains(p.ItemId));
                if (!storerCode.IsNullOrEmpty())
                    query.Where(p => p.StorerCode == storerCode);
                if (!projectNo.IsNullOrEmpty())
                    query.Where(p => p.ProjectNo == projectNo);
                if (!taskNo.IsNullOrEmpty())
                    query.Where(p => p.TaskNo == taskNo);

                return query.ToList();
            });
            return result;
        }

        /// <summary>
        /// 验证库位信息，库位有贪婪
        /// </summary>      
        private void ValidateLocationLoadWith(StorageLocation sourceLocation, StorageLocation targetLocation, StockTrans stockTrans)
        {
            if (sourceLocation != null)
            {
                if (sourceLocation.IsFrozen)
                    throw new ValidationException("来源库位[{0}]已冻结".L10nFormat(sourceLocation.Code));
                if (sourceLocation.AreaIsFrozen)
                    throw new ValidationException("来源库位[{0}]所属库区[{1}]已冻结".L10nFormat(sourceLocation.Code, sourceLocation.AreaCode));
                if (sourceLocation.WarehouseIsFrozen)
                    throw new ValidationException("来源库位[{0}]所属仓库[{1}]已冻结".L10nFormat(sourceLocation.Code, sourceLocation.WarehouseCode));
                if (!stockTrans.FromOnhandState.HasValue)
                    throw new ValidationException("来源库存状态不能为空".L10N());
            }

            if (targetLocation != null)
            {
                if (targetLocation.IsFrozen)
                    throw new ValidationException("目标库位[{0}]已冻结".L10nFormat(targetLocation.Code));
                if (targetLocation.AreaIsFrozen)
                    throw new ValidationException("目标库位[{0}]所属库区[{1}]已冻结".L10nFormat(targetLocation.Code, targetLocation.AreaCode));
                if (targetLocation.WarehouseIsFrozen)
                    throw new ValidationException("目标库位[{0}]所属仓库[{1}]已冻结".L10nFormat(targetLocation.Code, targetLocation.WarehouseCode));
            }
        }
    }
}
