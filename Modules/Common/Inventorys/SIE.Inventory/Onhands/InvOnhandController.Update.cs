using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Inventory.Commom;
using SIE.Inventory.TransactionProcessing;
using SIE.Items;
using SIE.Warehouses;
using System;
using System.Collections.Generic;

namespace SIE.Inventory.Onhands
{
    /// <summary>
    /// 库位库存控制器（更新库存）
    /// </summary>
    public partial class InvOnhandController
    {
        /// <summary>
        /// 更新库存
        /// </summary>
        /// <param name="loc">库位</param>
        /// <param name="item">物料</param>
        /// <param name="lot">批次</param>
        /// <param name="qty">分配数量</param>
        /// <param name="param">库存可选参数，如库存管控，则需要传入</param>
        /// <exception cref="ValidationException">库位[{0}]物料[{1}]库存可用量不足[{2}]，无法分配</exception>
        public virtual void AllotOnhand(StorageLocation loc, Item item, Lot lot, decimal qty, InvOptionalParam param)
        {
            AdjustLotLpnOnhand(loc, item, lot, qty, param, AllotBaseOnhand);
            //AdjustLotOnhand(loc, item, lot, qty, param, AllotBaseOnhand);
            //AdjustLocationOnhand(loc, item, qty, param, AllotBaseOnhand);
        }

        /// <summary>
        /// 更新库存可用数
        /// </summary>
        /// <param name="loc">库位</param>
        /// <param name="item">物料</param>
        /// <param name="lot">批次</param>
        /// <param name="qty">分配数量</param>
        /// <param name="param">库存可选参数，如库存管控，则需要传入</param>
        /// <exception cref="ValidationException">库位[{0}]物料[{1}]库存可用量不足[{2}]，无法扣减</exception>
        public virtual void UpdateOnhandAvailableQty(StorageLocation loc, Item item, Lot lot, decimal qty, InvOptionalParam param)
        {
            AdjustLotLpnOnhand(loc, item, lot, qty, param, UpdateBaseOnhandAvailableQty);
            //AdjustLotOnhand(loc, item, lot, qty, param, UpdateBaseOnhandAvailableQty);
            //AdjustLocationOnhand(loc, item, qty, param, UpdateBaseOnhandAvailableQty);
        }

        /// <summary>
        /// 冻结库存
        /// </summary>
        /// <param name="loc">库位</param>
        /// <param name="item">物料</param>
        /// <param name="lot">批次</param>
        /// <param name="qty">分配数量</param>
        /// <param name="param">库存可选参数，如库存管控，则需要传入</param>
        /// <exception cref="ValidationException">库位[{0}]物料[{1}]库存可用量不足[{2}]，无法冻结</exception>
        public virtual void FrozenOnhand(StorageLocation loc, Item item, Lot lot, decimal qty, InvOptionalParam param)
        {
            AdjustLotLpnOnhand(loc, item, lot, qty, param, FrozenBaseOnhand);
            //AdjustLotOnhand(loc, item, lot, qty, param, FrozenBaseOnhand);
            //AdjustLocationOnhand(loc, item, qty, param, FrozenBaseOnhand);
        }

        /// <summary>
        /// 分配指定发运单明细
        /// </summary>
        /// <param name="baseAssign">基础分配信息</param>
        /// <param name="selLotLpnOnhand">库存</param>
        /// <param name="qty">数量</param>
        /// <returns>泛型集合</returns>
        public virtual List<T> AssignOnhand<T>(BaseAssign baseAssign, List<LotLpnOnhand> selLotLpnOnhand, decimal qty) where T : BaseAssign, new()
        {
            List<T> result = new List<T>();
            for (int i = 0; (i < selLotLpnOnhand.Count && qty > 0); i++)
            {
                LotLpnOnhand curr = selLotLpnOnhand[i];
                decimal tempQty = curr.AvailableQty > qty ? qty : curr.AvailableQty;
                T newObj = new T();
                newObj.Clone(baseAssign, CloneOptions.DeepClone());
                newObj.SourceStorageLocationId = curr.StorageLocationId;
                newObj.LotId = curr.LotId;
                newObj.LotLpnOnhandId = curr.Id;
                newObj.AssignQty = tempQty;
                newObj.ProjectNo = curr.ProjectNo;
                newObj.Lpn = curr.Lpn;
                result.Add(newObj);
                curr.AvailableQty -= tempQty;
                qty -= tempQty;
            }

            if (qty > 0)
            {
                throw new ValidationException("物料为:{0}没有足够库存分配".L10nFormat(baseAssign.Item.Code));
            }

            return result;
        }

        /// <summary>
        /// 分配指定发运单明细
        /// </summary>
        /// <param name="baseAssign">基础分配信息</param>
        /// <param name="selLotLpnOnhand">库存</param>
        /// <param name="qty">数量</param>
        /// <param name="assignOnhandDic">已分配库存key:库存ID,value:库存剩余可用量</param>
        /// <param name="isAllot">是否足够分配</param>
        /// <param name="isPack">按整包分配</param>
        /// <returns>泛型集合</returns>
        public virtual List<T> AssignOnhand<T>(BaseAssign baseAssign, List<LotLpnOnhand> selLotLpnOnhand, decimal qty, Dictionary<double, decimal> assignOnhandDic, bool? isAllot = null
            , bool isPack = false, List<double> assignIds = null) where T : BaseAssign, new()
        {
            List<T> result = new List<T>();
            if (isPack)
            {
                for (int i = 0; (i < selLotLpnOnhand.Count && qty > 0); i++)
                {
                    LotLpnOnhand curr = selLotLpnOnhand[i];
                    var tempQty = curr.AvailableQty > qty ? qty : curr.AvailableQty;
                    T newObj = new T();
                    newObj.Clone(baseAssign, CloneOptions.DeepClone());
                    newObj.SourceStorageLocationId = curr.StorageLocationId;
                    newObj.LotId = curr.LotId;
                    newObj.LotLpnOnhandId = curr.Id;
                    newObj.LotLpnOnhand = curr;
                    newObj.AssignQty = tempQty;
                    newObj.ProjectNo = curr.ProjectNo;
                    newObj.TaskNo = curr.TaskNo;
                    newObj.StorerCode = curr.StorerCode;
                    newObj.OnhandState = curr.State;
                    newObj.Lpn = curr.Lpn;
                    newObj.ItemExtProp = curr.ItemExtProp;
                    newObj.ItemExtPropName = curr.ItemExtPropName;
                  
                    result.Add(newObj);

                    qty -= tempQty;
                }
            }
            else
            {
                for (int i = 0; (i < selLotLpnOnhand.Count && qty > 0); i++)
                {
                    LotLpnOnhand curr = selLotLpnOnhand[i];
                    decimal tempQty = 0;
                    if (assignOnhandDic.ContainsKey(curr.Id))
                    {
                        if (assignOnhandDic[curr.Id] <= 0)
                            continue;
                        else
                        {
                            tempQty = assignOnhandDic[curr.Id] > qty ? qty : assignOnhandDic[curr.Id];
                            curr.AvailableQty = assignOnhandDic[curr.Id] > qty ? (assignOnhandDic[curr.Id] - qty) : 0;
                        }

                    }
                    else
                    {
                        tempQty = curr.AvailableQty > qty ? qty : curr.AvailableQty;
                        curr.AvailableQty -= tempQty;
                    }
                    T newObj = new T();
                    newObj.Clone(baseAssign, CloneOptions.DeepClone());
                    newObj.SourceStorageLocationId = curr.StorageLocationId;
                    newObj.LotId = curr.LotId;
                    newObj.LotLpnOnhandId = curr.Id;
                    newObj.LotLpnOnhand = curr;
                    newObj.AssignQty = tempQty;
                    newObj.ProjectNo = curr.ProjectNo;
                    newObj.TaskNo = curr.TaskNo;
                    newObj.StorerCode = curr.StorerCode;
                    newObj.OnhandState = curr.State;
                    newObj.Lpn = curr.Lpn;
                    newObj.ItemExtProp = curr.ItemExtProp;
                    newObj.ItemExtPropName = curr.ItemExtPropName;
                    
                    result.Add(newObj);

                    qty -= tempQty;

                    if (!assignOnhandDic.ContainsKey(curr.Id))
                    {
                        assignOnhandDic.Add(curr.Id, curr.AvailableQty);
                    }
                    else
                    {
                        assignOnhandDic[curr.Id] = curr.AvailableQty;
                    }
                }
            }
            if (!isAllot.HasValue && qty > 0)
            {
                throw new ValidationException("物料为:{0}没有足够库存分配".L10nFormat(baseAssign.Item.Code));
            }

            return result;
        }

    }
}
