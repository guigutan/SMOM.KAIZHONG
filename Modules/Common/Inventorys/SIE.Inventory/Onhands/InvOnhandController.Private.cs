using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Core.Enums;
using SIE.Inventory.TransactionProcessing;
using SIE.Items;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIE.Inventory.Commom;

namespace SIE.Inventory.Onhands
{
    /// <summary>
    /// 库位库存控制器
    /// </summary>
    /// <seealso cref="SIE.DomainController" />
    partial class InvOnhandController
    {
        /// <summary>
        /// 获取库存提示信息
        /// </summary>
        /// <param name="loc">库位</param>
        /// <param name="item">物料</param>
        /// <param name="lotCode">批次</param>
        /// <param name="param">库存可选参数，如库存管控，则需要传入</param>
        /// <returns>库存提示信息</returns>
        private string GetValidationException(StorageLocation loc, Item item, string lotCode, InvOptionalParam param)
        {
            StringBuilder sb = new StringBuilder();
            if (loc != null)
            {
                sb.Append("库位[{0}]".L10nFormat(loc.Code));
            }

            if (item != null)
            {
                sb.Append("物料[{0}]".L10nFormat(item.Code));
            }

            if (!lotCode.IsNullOrEmpty())
            {
                sb.Append("批次[{0}]".L10nFormat(lotCode));
            }

            if (!param.StorerCode.IsNullOrWhiteSpace())
            {
                sb.Append("货主[{0}]".L10nFormat(param.StorerCode));
            }

            if (!param.Lpn.IsNullOrWhiteSpace())
            {
                sb.Append("LPN[{0}]".L10nFormat(param.Lpn));
            }

            if (!param.ProjectNo.IsNullOrWhiteSpace())
            {
                sb.Append("项目号[{0}]".L10nFormat(param.ProjectNo));
            }

            if (!param.TaskNo.IsNullOrWhiteSpace())
            {
                sb.Append("任务号[{0}]".L10nFormat(param.TaskNo));
            }

            return sb.ToString();
        }

        /// <summary>
        /// 更新库存可用数量
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="onhand">库存基类</param>
        /// <param name="qty">更新数量</param>
        private void UpdateAvailableQty<T>(T onhand, decimal qty) where T : BaseOnhand
        {
            ////验证库位物料可用数           
            if (onhand.AvailableQty + qty < 0)
            {
                throw new ValidationException("库位[{0}]物料[{1}]库存可用量不足".L10nFormat(onhand.StorageLocation.Code, onhand.Item.Code));
            }
            DB.Update<T>()
                .Set(p => p.AvailableQty, p => p.AvailableQty + qty)
                .Set(p => p.Qty, p => p.Qty + qty)
                .Where(p => p.Id == onhand.Id)
                .Execute();
        }

        /// <summary>
        /// 更新库存分配数量
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="onhand">库存基类</param>
        /// <param name="qty">更新数量</param>
        private void UpdateAllottedQty<T>(T onhand, decimal qty) where T : BaseOnhand
        {
            if (onhand.AllottedQty + qty < 0)
            {
                throw new ValidationException("库位[{0}]物料[{1}]库存分配量不足".L10nFormat(onhand.StorageLocation.Code, onhand.Item.Code));
            }

            DB.Update<T>()
                .Set(p => p.AllottedQty, p => p.AllottedQty + qty)
                .Set(p => p.Qty, p => p.Qty + qty)
                .Where(p => p.Id == onhand.Id)
                .Execute();
        }

        /// <summary>
        /// 更新库存状态
        /// </summary>
        /// <param name="dataList">参数集合</param>
        /// <param name="state">状态</param>
        public virtual void UpdateState(List<OnhandData> dataList, OnhandState state)
        {
            foreach (var data in dataList)
            {
                DB.Update<LotLpnOnhand>()
                .Set(p => p.State, state)
                .Where(p => p.StorageLocationId == data.LocationId)
                .Where(p => p.ItemId == data.ItemId)
                .Where(p => p.LotCode == data.LotCode)
                .Where(p => p.Lpn == data.Param.Lpn)
                .Execute();
            }
        }

        /// <summary>
        /// 通过库存id更新库存状态
        /// </summary>
        /// <param name="onHandids"></param>
        /// <param name="state"></param>
        public virtual void UpdateStateByIds(List<double> onHandids, OnhandState state)
        {
            //return onHandids.SplitContains(ids =>
            //{
            //    return Query().Where(p => asnIdList.Contains(p.Id)).ToList(null, elo);
            //});
            DB.Update<LotLpnOnhand>().Set(p => p.State, state).Where(p => onHandids.Contains(p.Id)).Execute();
        }
        /// <summary>
        /// 更新库存状态
        /// </summary>
        /// <param name="onhand">库存</param>
        /// <param name="state">状态</param>
        public virtual void UpdateState(LotLpnOnhand onhand, OnhandState state)
        {
            DB.Update<LotLpnOnhand>()
                .Set(p => p.State, state)
                .Where(p => p.Id == onhand.Id)
                .Execute();
        }

        /// <summary>
        /// 分配库存
        /// </summary>
        /// <param name="onhand">库存基类</param>
        /// <param name="qty">分配数量</param>
        private void AllotBaseOnhand(BaseOnhand onhand, decimal qty)
        {
            if (onhand.AvailableQty - qty < 0)
            {
                throw new ValidationException("库位[{0}]物料[{1}]库存可用量不足[{2}]，无法分配".L10nFormat(onhand.StorageLocation.Code, onhand.Item.Code, qty));
            }

            if (onhand.AllottedQty + qty < 0)
            {
                throw new ValidationException("库位[{0}]物料[{1}]库存分配量不足[{2}]".L10nFormat(onhand.StorageLocation.Code, onhand.Item.Code, qty));
            }

            if (onhand is LotLpnOnhand)
            {
                DB.Update<LotLpnOnhand>()
                    .Set(p => p.AvailableQty, p => p.AvailableQty + (-qty))
                    .Set(p => p.AllottedQty, p => p.AllottedQty + qty)
                    .Where(p => p.Id == onhand.Id)
                    .Execute();
            }

            //if (onhand is LotOnhand)
            //{
            //    DB.Update<LotOnhand>()
            //        .Set(p => p.AvailableQty, p => p.AvailableQty + (-qty))
            //        .Set(p => p.AllottedQty, p => p.AllottedQty + qty)
            //        .Where(p => p.Id == onhand.Id)
            //        .Execute();
            //}

            //if (onhand is LocationOnhand)
            //{
            //    DB.Update<LocationOnhand>()
            //        .Set(p => p.AvailableQty, p => p.AvailableQty + (-qty))
            //        .Set(p => p.AllottedQty, p => p.AllottedQty + qty)
            //        .Where(p => p.Id == onhand.Id)
            //        .Execute();
            //}
            onhand.AvailableQty = onhand.AvailableQty - qty;
            onhand.AllottedQty = onhand.AllottedQty + qty;
        }

        /// <summary>
        /// 扣减库存可用数
        /// </summary>
        /// <param name="onhand">库存基类</param>
        /// <param name="qty">分配数量</param>
        private void UpdateBaseOnhandAvailableQty(BaseOnhand onhand, decimal qty)
        {
            if (onhand.AvailableQty - qty < 0)
            {
                throw new ValidationException("库位[{0}]物料[{1}]库存可用量不足[{2}]，无法扣减".L10nFormat(onhand.StorageLocation.Code, onhand.Item.Code, qty));
            }

            if (onhand is LotLpnOnhand)
            {
                DB.Update<LotLpnOnhand>()
                    .Set(p => p.AvailableQty, p => p.AvailableQty + (-qty))
                    .Set(p => p.Qty, p => p.Qty + (-qty))
                    .Where(p => p.Id == onhand.Id)
                    .Execute();
            }

            //if (onhand is LotOnhand)
            //{
            //    DB.Update<LotOnhand>()
            //        .Set(p => p.AvailableQty, p => p.AvailableQty + (-qty))
            //        .Set(p => p.Qty, p => p.Qty + (-qty))
            //        .Where(p => p.Id == onhand.Id)
            //        .Execute();
            //}

            //if (onhand is LocationOnhand)
            //{
            //    DB.Update<LocationOnhand>()
            //        .Set(p => p.AvailableQty, p => p.AvailableQty + (-qty))
            //        .Set(p => p.Qty, p => p.Qty + (-qty))
            //        .Where(p => p.Id == onhand.Id)
            //        .Execute();
            //}
        }

        /// <summary>
        /// 冻结库存
        /// </summary>
        /// <param name="onhand">库存基类</param>
        /// <param name="qty">冻结数量</param>
        private void FrozenBaseOnhand(BaseOnhand onhand, decimal qty)
        {
            if (onhand.AvailableQty - qty < 0)
            {
                throw new ValidationException("库位[{0}]物料[{1}]库存可用量不足[{2}]，无法冻结".L10nFormat(onhand.StorageLocation.Code, onhand.Item.Code, qty));
            }

            if (onhand.FreezingQty + qty < 0)
            {
                throw new ValidationException("库位[{0}]物料[{1}]库存冻结量不足[{2}]".L10nFormat(onhand.StorageLocation.Code, onhand.Item.Code, qty));
            }

            if (onhand is LotLpnOnhand)
            {
                DB.Update<LotLpnOnhand>()
                    .Set(p => p.AvailableQty, p => p.AvailableQty + (-qty))
                    .Set(p => p.FreezingQty, p => p.FreezingQty + qty)
                    .Where(p => p.Id == onhand.Id)
                    .Execute();
            }

            //if (onhand is LotOnhand)
            //{
            //    DB.Update<LotOnhand>()
            //        .Set(p => p.AvailableQty, p => p.AvailableQty + (-qty))
            //        .Set(p => p.FreezingQty, p => p.FreezingQty + qty)
            //        .Where(p => p.Id == onhand.Id)
            //        .Execute();
            //}

            //if (onhand is LocationOnhand)
            //{
            //    DB.Update<LocationOnhand>()
            //        .Set(p => p.AvailableQty, p => p.AvailableQty + (-qty))
            //        .Set(p => p.FreezingQty, p => p.FreezingQty + qty)
            //        .Where(p => p.Id == onhand.Id)
            //        .Execute();
            //}
        }

        /// <summary>
        /// 根据获取批次和LPN库存
        /// </summary>
        /// <param name="lpn">LPN</param>
        /// <param name="excludeLocId">排除库位</param>
        /// <returns>批次和LPN库存</returns>
        private LotLpnOnhand GetLotLpnOnhand(string lpn, double excludeLocId)
        {
            var query = Query<LotLpnOnhand>();
            query.Where(p => p.Lpn == lpn && p.Qty > 0 && p.StorageLocationId != excludeLocId);
            return query.FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 调整批次和LPN库存
        /// </summary>
        /// <param name="adjustType">调整类型</param>
        /// <param name="loc">库位</param>
        /// <param name="item">物料</param>
        /// <param name="lot">批次</param>
        /// <param name="qty">调整数量：入库为正数，出库为负数</param>
        /// <param name="param">库存可选参数，如库存管控，则需要传入</param>
        /// <param name="isAllotted">是否调整分配数：是则调整库存分配数，否则调整可用数</param>
        /// <param name="isValidateMulLoc">是否验证LPN多库位存储</param>
        /// <param name="isValidateState">是否验证库存状态</param>
        /// <param name="isReceive">是否收货</param>
        /// <param name="isNotCheckNone">是否不验证未质检库存</param>
        /// <exception cref="ValidationException">异常数据</exception>
        private LotLpnOnhand AdjustLotLpnOnhand(AdjustType adjustType, StorageLocation loc, Item item, Lot lot, decimal qty, InvOptionalParam param, bool isAllotted, bool isValidateMulLoc = true, bool isValidateState = true, bool isReceive = false, bool isNotCheckNone = false)
        {
            LotLpnOnhand onhand = null;
            if (param.LotLpnOnhands != null)
            {
                onhand = param.LotLpnOnhands.FirstOrDefault(a => a.StorageLocationId == loc.Id && a.ItemId == item.Id && a.LotId == lot.Id
                && a.StorerCode == param.StorerCode && a.TaskNo == param.TaskNo && a.ProjectNo == param.ProjectNo && a.Lpn == param.Lpn && a.State == param.State && a.ItemExtProp == param.ItemExtProp);                   
            }
            else
            {
                //防止并发，锁定数据行
                LockLotLpnOnhand(loc.Id, item.Id, lot.Code, param);
                onhand = GetLotLpnOnhand(loc.Id, item.Id, lot.Code, param);

                if (!string.IsNullOrEmpty(param.Lpn) && param.Lpn != "*" && isValidateMulLoc && (onhand == null || onhand.Qty == 0))
                {
                    var zeroOnhand = GetLotLpnOnhand(param.Lpn, loc.Id);
                    if (zeroOnhand != null)
                    {
                        throw new ValidationException("LPN[{0}]已存储在库位[{1}],不能存放在多个库位".L10nFormat(param.Lpn, zeroOnhand.StorageLocation.Name));
                    }
                }
            }
            if (onhand == null)
            {
                onhand = new LotLpnOnhand
                {
                    Warehouse = loc.Warehouse,
                    StorageAreaId = loc.AreaId,
                    StorageLocation = loc,
                    Item = item,
                    StorerCode = param.StorerCode,
                    Lot = lot,
                    LotCode = lot.Code,
                    Lpn = param.Lpn,
                    ProjectNo = param.ProjectNo,
                    TaskNo = param.TaskNo,
                    Qty = qty,
                    AvailableQty = isAllotted ? 0 : qty,
                    AllottedQty = isAllotted ? qty : 0,
                    FreezingQty = 0,
                    ItemExtProp = param.ItemExtProp,
                    ItemExtPropName = param.ItemExtPropName,
                    State = param.State
                };

                ////验证库位物料可用数               
                if (onhand.Qty < 0)
                {
                    throw new ValidationException("库位[{0}]物料[{1}]库存可用量不足".L10nFormat(onhand.StorageLocation.Code, onhand.Item.Code));
                }
                RF.Save(onhand);
                if (param.LotLpnOnhands != null)
                    param.LotLpnOnhands.Add(onhand);
            }
            else
            {
                HandleOnhandData(onhand, lot, qty, param, adjustType, isReceive, isNotCheckNone, isAllotted, isValidateState);
            }

            return onhand;
        }

        /// <summary>
        /// 分配或冻结批次和LPN库存
        /// </summary>
        /// <param name="loc">库位</param>
        /// <param name="item">物料</param>
        /// <param name="lot">批次</param>
        /// <param name="qty">分配数量</param>
        /// <param name="param">库存可选参数，如库存管控，则需要传入</param>
        /// <param name="action">更新库存委托</param>
        /// <exception cref="ValidationException">库位[{0}]物料[{1}]库存可用量不足[{2}]，无法分配</exception>
        private void AdjustLotLpnOnhand(StorageLocation loc, Item item, Lot lot, decimal qty, InvOptionalParam param, Action<BaseOnhand, decimal> action)
        {
            if (param.LotLpnOnhands != null)
            {
                LotLpnOnhand onhand = param.LotLpnOnhands.FirstOrDefault(a => a.StorageLocationId == loc.Id && a.ItemId == item.Id && a.LotId == lot.Id
                && a.StorerCode == param.StorerCode && a.TaskNo == param.TaskNo && a.ProjectNo == param.ProjectNo && a.Lpn == param.Lpn && a.State == param.State && a.ItemExtProp == param.ItemExtProp);
                if (onhand != null)
                {
                    action(onhand, qty);
                }
                else
                {
                    throw new ValidationException(GetValidationException(loc, item, lot.Code, param) + "不存在库存".L10N());
                }
            }
            else
            {
                LotLpnOnhand onhand = GetLotLpnOnhand(loc.Id, item.Id, lot.Code, param);
                if (onhand != null)
                {
                    action(onhand, qty);
                }
                else
                {
                    throw new ValidationException(GetValidationException(loc, item, lot.Code, param) + "不存在库存".L10N());
                }
            }
        }

        /// <summary>
        /// 处理库存数据
        /// </summary>
        /// <param name="onhand">库存数据</param>
        /// <param name="lot">批次</param>
        /// <param name="qty">数量</param>
        /// <param name="param">库存可选参数，如库存管控，则需要传入</param>
        /// <param name="adjustType">调整类型</param>
        /// <param name="isReceive">是否收货</param>
        /// <param name="isNotCheckNone">是否不验证未质检库存</param>
        /// <param name="isAllotted">是否调整分配数：是则调整库存分配数，否则调整可用数</param>
        /// <param name="isValidateState">是否验证库存状态</param>
        /// <exception cref="ValidationException">异常数据</exception>
        private void HandleOnhandData(LotLpnOnhand onhand, Lot lot, decimal qty, InvOptionalParam param, AdjustType adjustType, bool isReceive, bool isNotCheckNone, bool isAllotted, bool isValidateState)
        {
            if (!isReceive)
            {
                if (!isNotCheckNone && onhand.State == OnhandState.None)
                {
                    throw new ValidationException("库位[{0}]物料[{1}]未质检，库存不可使用".L10nFormat(onhand.StorageLocation.Code, onhand.Item.Code));
                }

                //更新来源库位库存
                if (adjustType == AdjustType.From)
                {
                    //根据配置决定是否验证来源库存状态
                    if (isValidateState && onhand.State == OnhandState.Ng)
                        throw new ValidationException("库位[{0}]物料[{1}]质检不合格，库存不可使用".L10nFormat(onhand.StorageLocation.Code, onhand.Item.Code));
                }
                else
                {
                    if (!(lot.Code == Lot.LotDefault && param.Lpn == "*"))
                    {
                        if (onhand.Qty > 0)
                        {
                            //如果库存已有数量，则需验证库存状态是否一致
                            if (onhand.State != param.State)
                                throw new ValidationException("库位[{0}]物料[{1}]库存状态不一致，库存不可使用".L10nFormat(onhand.StorageLocation.Code, onhand.Item.Code));
                        }
                        else
                        {
                            //如果库存数量为0，则更新原有库存状态
                            UpdateState(onhand, param.State);
                        }
                    }
                }
            }

            if (isAllotted)
            {
                UpdateAllottedQty(onhand, qty);
            }
            else
            {
                UpdateAvailableQty(onhand, qty);
            }
        }

        ///// <summary>
        ///// 调整批次
        ///// </summary>
        ///// <param name="loc">库位</param>
        ///// <param name="item">物料</param>
        ///// <param name="lot">批次</param>
        ///// <param name="qty">调整数量：入库为正数，出库为负数</param>
        ///// <param name="param">库存可选参数，如库存管控，则需要传入</param>
        ///// <param name="isAllotted">是否调整分配数：是则调整库存分配数，否则调整可用数</param>
        //private void AdjustLotOnhand(StorageLocation loc, Item item, Lot lot, decimal qty, InvOptionalParam param, bool isAllotted)
        //{
        //    LotOnhand onhand;
        //    if (param.LotOnhands != null)
        //    {
        //        onhand = param.LotOnhands.FirstOrDefault(a => a.StorageLocationId == loc.Id && a.ItemId == item.Id && a.LotId == lot.Id
        //       && a.StorerCode == param.StorerCode && a.TaskNo == param.TaskNo && a.ProjectNo == param.ProjectNo);
        //    }
        //    else
        //        onhand = GetLotOnhand(loc.Id, item.Id, lot.Code, param);
        //    if (onhand == null)
        //    {
        //        onhand = new LotOnhand
        //        {
        //            Warehouse = loc.Warehouse,
        //            StorageAreaId = loc.AreaId,
        //            StorageLocation = loc,
        //            Item = item,
        //            StorerCode = param.StorerCode,
        //            Lot = lot,                   
        //            ProjectNo = param.ProjectNo,
        //            TaskNo = param.TaskNo,
        //            Qty = qty,
        //            AvailableQty = isAllotted ? 0 : qty,
        //            AllottedQty = isAllotted ? qty : 0,
        //            FreezingQty = 0
        //        };

        //        ////验证库位物料可用数                
        //        if (onhand.Qty < 0)
        //        {
        //            throw new ValidationException("库位[{0}]物料[{1}]库存可用量不足".L10nFormat(onhand.StorageLocation.Code, onhand.Item.Code));
        //        }
        //        RF.Save(onhand);
        //        if (param.LotOnhands != null)
        //            param.LotOnhands.Add(onhand);
        //    }
        //    else
        //    {
        //        if (isAllotted)
        //        {
        //            UpdateAllottedQty(onhand, qty);
        //            if (param.LotOnhands != null)
        //            {
        //                var paOnhand = param.LotOnhands.FirstOrDefault(a => a.Id == onhand.Id);
        //                paOnhand.AllottedQty += qty;
        //                paOnhand.Qty += qty;
        //                paOnhand.MarkSaved();
        //            }
        //        }
        //        else
        //        {
        //            UpdateAvailableQty(onhand, qty);
        //            if (param.LotOnhands != null)
        //            {
        //                var paOnhand = param.LotOnhands.FirstOrDefault(a => a.Id == onhand.Id);
        //                paOnhand.AvailableQty += qty;
        //                paOnhand.Qty += qty;
        //                paOnhand.MarkSaved();
        //            }
        //        }
        //    }
        //}

        ///// <summary>
        ///// 分配或冻结批次库存
        ///// </summary>
        ///// <param name="loc">库位</param>
        ///// <param name="item">物料</param>
        ///// <param name="lot">批次</param>
        ///// <param name="qty">分配数量</param>
        ///// <param name="param">库存可选参数，如库存管控，则需要传入</param>
        ///// <param name="action">更新库存委托</param>
        ///// <exception cref="ValidationException">库位[{0}]物料[{1}]库存可用量不足[{2}]，无法分配</exception>
        //private void AdjustLotOnhand(StorageLocation loc, Item item, Lot lot, decimal qty, InvOptionalParam param, Action<BaseOnhand, decimal> action)
        //{
        //    if (param.LotOnhands != null)
        //    {
        //        LotOnhand onhand = param.LotOnhands.FirstOrDefault(a => a.StorageLocationId == loc.Id && a.ItemId == item.Id && a.LotId == lot.Id
        //        && a.StorerCode == param.StorerCode && a.TaskNo == param.TaskNo && a.ProjectNo == param.ProjectNo);
        //        if (onhand != null)
        //        {
        //            action(onhand, qty);
        //        }
        //        else
        //        {
        //            throw new ValidationException(GetValidationException(loc, item, lot.Code, param) + "不存在库存".L10N());
        //        }
        //    }
        //    else
        //    {
        //        LotOnhand onhand = GetLotOnhand(loc.Id, item.Id, lot.Code, param);
        //        if (onhand != null)
        //        {
        //            action(onhand, qty);
        //        }
        //        else
        //        {
        //            throw new ValidationException(GetValidationException(loc, item, lot.Code, param) + "不存在库存".L10N());
        //        }
        //    }
        //}

        ///// <summary>
        ///// 调整库位库存
        ///// </summary>
        ///// <param name="loc">库位</param>
        ///// <param name="item">物料</param>
        ///// <param name="qty">调整数量：入库为正数，出库为负数</param>
        ///// <param name="param">库存可选参数，如库存管控，则需要传入</param>
        ///// <param name="isAllotted">是否调整分配数：是则调整库存分配数，否则调整可用数</param>
        //private void AdjustLocationOnhand(StorageLocation loc, Item item, decimal qty, InvOptionalParam param, bool isAllotted)
        //{
        //    LocationOnhand onhand;
        //    if (param.LocOnhands != null)
        //    {
        //        onhand = param.LocOnhands.FirstOrDefault(a => a.StorageLocationId == loc.Id && a.ItemId == item.Id
        //       && a.StorerCode == param.StorerCode && a.TaskNo == param.TaskNo && a.ProjectNo == param.ProjectNo);
        //    }
        //    else
        //        onhand = GetLocationOnhand(loc.Id, item.Id, param);
        //    if (onhand == null)
        //    {
        //        onhand = new LocationOnhand
        //        {
        //            Warehouse = loc.Warehouse,
        //            StorageAreaId = loc.AreaId,
        //            StorageLocation = loc,
        //            Item = item,
        //            StorerCode = param.StorerCode,
        //            ProjectNo = param.ProjectNo,
        //            TaskNo = param.TaskNo,
        //            Qty = qty,
        //            AvailableQty = isAllotted ? 0 : qty,
        //            AllottedQty = isAllotted ? qty : 0,
        //            FreezingQty = 0
        //        };

        //        ////验证库位物料可用数                
        //        if (onhand.Qty < 0)
        //        {
        //            throw new ValidationException("库位[{0}]物料[{1}]库存可用量不足".L10nFormat(onhand.StorageLocation.Code, onhand.Item.Code));
        //        }
        //        RF.Save(onhand);
        //        if (param.LocOnhands != null)
        //        {
        //            param.LocOnhands.Add(onhand);
        //        }
        //    }
        //    else
        //    {
        //        if (isAllotted)
        //        {
        //            UpdateAllottedQty(onhand, qty);
        //            if (param.LocOnhands != null)
        //            {
        //                var paOnhand = param.LocOnhands.FirstOrDefault(a => a.Id == onhand.Id);
        //                paOnhand.AvailableQty += qty;
        //                paOnhand.Qty += qty;
        //                paOnhand.MarkSaved();
        //            }
        //        }
        //        else
        //        {
        //            UpdateAvailableQty(onhand, qty);
        //            if (param.LocOnhands != null)
        //            {
        //                var paOnhand = param.LocOnhands.FirstOrDefault(a => a.Id == onhand.Id);
        //                paOnhand.AllottedQty += qty;
        //                paOnhand.Qty += qty;
        //                paOnhand.MarkSaved();
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// 分配或冻结库位库存
        /// </summary>
        /// <param name="loc">库位</param>
        /// <param name="item">物料</param>
        /// <param name="qty">分配数量</param>
        /// <param name="param">库存可选参数，如库存管控，则需要传入</param>
        /// <param name="action">更新库存委托</param>
        /// <exception cref="ValidationException">库位[{0}]物料[{1}]库存可用量不足[{2}]，无法分配</exception>
        //private void AdjustLocationOnhand(StorageLocation loc, Item item, decimal qty, InvOptionalParam param, Action<BaseOnhand, decimal> action)
        //{
        //    if (param.LocOnhands != null)
        //    {
        //        var onhand = param.LocOnhands.FirstOrDefault(a => a.StorageLocationId == loc.Id && a.ItemId == item.Id
        //        && a.StorerCode == param.StorerCode && a.TaskNo == param.TaskNo && a.ProjectNo == param.ProjectNo);
        //        if (onhand != null)
        //        {
        //            action(onhand, qty);
        //        }
        //        else
        //        {
        //            throw new ValidationException(GetValidationException(loc, item, string.Empty, param) + "不存在库存".L10N());
        //        }
        //    }
        //    else
        //    {
        //        LocationOnhand onhand = GetLocationOnhand(loc.Id, item.Id, param);
        //        if (onhand != null)
        //        {
        //            action(onhand, qty);
        //        }
        //        else
        //        {
        //            throw new ValidationException(GetValidationException(loc, item, string.Empty, param) + "不存在库存".L10N());
        //        }
        //    }
        //}
    }
}