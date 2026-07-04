using SIE.Api;
using SIE.Core.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Inventory.Onhands;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Inventory.Commom
{
    /// <summary>
    /// 批次控制器
    /// </summary>
    public class LotController : DomainController
    {
        /// <summary>
        /// 根据批次编码获取批次信息
        /// </summary>
        /// <param name="lotCodeList">批次编码</param>
        /// <returns>返回批次信息</returns>
        public virtual EntityList<Lot> GetLot(List<string> lotCodeList)
        {
            EntityList<Lot> lots = new EntityList<Lot>();
            lotCodeList = lotCodeList.Distinct().ToList();
            DataProcessEx.SplitDataExecute(lotCodeList, sons =>
            {
                var lotList = Query<Lot>().Where(p => sons.Contains(p.Code)).ToList();
                lots.AddRange(lotList);
            });
            return lots;
        }
      
        /// <summary>
        /// 获取默认批次
        /// </summary>
        /// <returns></returns>
        public virtual Lot GetLotDefault()
        {
            return Query<Lot>().Where(p => p.Code == Lot.LotDefault).FirstOrDefault();
        }

        /// <summary>
        /// 获取批次
        /// </summary>
        /// <param name="lot">批次</param>
        /// <param name="itemId">物料Id</param>
        /// <param name="itemextprop">扩展属性</param>
        /// <param name="elo">贪婪</param>
        /// <returns></returns>
        public virtual Lot GetLot(string lot, double itemId, string itemextprop, EagerLoadOptions elo = null)
        {
            if (lot == Lot.LotDefault)
                return GetLotDefault();
            return Query<Lot>().Where(p => p.ItemId == itemId && p.Code == lot && p.ItemExtProp == itemextprop).FirstOrDefault(null);
        }        

        /// <summary>
        /// 根据批次ID集合获取批次信息
        /// </summary>
        /// <param name="lotIdList">批次ID集合</param>
        /// <returns>批次信息</returns>
        public virtual EntityList<Lot> GetLots(List<double> lotIdList)
        {
            EntityList<Lot> Lots = new EntityList<Lot>();
            DataProcessEx.SplitDataExecute(lotIdList, sons =>
            {
                var customersList = Query<Lot>().Where(p => sons.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                Lots.AddRange(customersList);
            });
            return Lots;
        }

        /// <summary>
        /// 根据批次ID集合获取批次信息
        /// </summary>
        /// <param name="lotIdList">批次ID集合</param>
        /// <param name="elo">贪婪加载</param>
        /// <returns>批次信息</returns>
        public virtual EntityList<Lot> GetLots(List<double> lotIdList, EagerLoadOptions elo = null)
        {
            EntityList<Lot> Lots = new EntityList<Lot>();
            DataProcessEx.SplitDataExecute(lotIdList, sons =>
            {
                var customersList = Query<Lot>().Where(p => sons.Contains(p.Id)).ToList(null, elo);
                Lots.AddRange(customersList);
            });
            return Lots;
        }

        /// <summary>
        /// 根据批次ID集合获取批次信息
        /// </summary>
        /// <param name="lotCodeList">批次集合</param>
        /// <returns>批次信息</returns>
        public virtual EntityList<Lot> GetLots(List<string> lotCodeList)
        {
            return lotCodeList.Distinct().SplitContains(lots =>
            {
                return Query<Lot>().Where(p => lots.Contains(p.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 根据批次ID集合获取批次信息
        /// </summary>
        /// <param name="itemId">物料Id</param>
        /// <param name="itemExtPropName">扩展属性</param>
        /// <param name="keyword">关键字</param>
        /// <param name="pagingInfo">分页</param>      
        /// <returns>批次信息</returns>
        public virtual EntityList<Lot> GetLots(double itemId, string itemExtPropName, string keyword, PagingInfo pagingInfo)
        {
            var query = Query<Lot>().Where(p => p.ItemId == itemId && p.ItemExtPropName == itemExtPropName);
            if (keyword.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains(keyword));
            }
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取指定仓库、物料、库位、lpn下的批次信息
        /// </summary>
        /// <param name="wareHouseId">仓库ID</param>
        /// <param name="itemId">物料ID</param>
        /// <param name="locationCode">库位编号</param>
        /// <param name="lpn">LPN</param>
        /// <returns>返回批次信息</returns>
        public virtual EntityList<Lot> GetLots(double wareHouseId, double itemId, string locationCode, string lpn)
        {
            return Query<Lot>().Exists<LotLpnOnhand>((a, b) =>
                    b.Join<StorageLocation>((c, d) => c.StorageLocationId == d.Id).
                    Where<StorageLocation>((e, f) => a.Id == e.LotId && e.Qty > 0 && e.ItemId == itemId
                    && e.Lpn.Contains(lpn) && f.Code.Contains(locationCode) &&
                    f.WarehouseId == wareHouseId && e.WarehouseId == wareHouseId)).OrderBy(p => p.Code).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }
        
        /// <summary>
        /// 插入默认无批次管理的批次信息
        /// </summary>
        public virtual Lot InsertDefaultLot()
        {
            Lot defaultLot = new Lot();
            defaultLot.Code = Lot.LotDefault;
            RF.Save(defaultLot);
            return defaultLot;
        }

        /// <summary>
        /// 获取库存物料的批次信息
        /// </summary>       
        /// <param name="itemId">物料ID</param>
        /// <param name="keyword">关键字</param>
        /// <param name="info">分页信息</param>
        /// <param name="itemExtProp">物料扩展属性</param>
        /// <param name="isZero">包含现有量0</param>
        /// <returns>返回批次信息</returns>
        public virtual EntityList<Lot> GetItemLots(double itemId, string keyword, PagingInfo info, string itemExtProp = "", bool isZero = false)
        {
            var query = Query<Lot>();
            if (isZero)
                query.Exists<LotLpnOnhand>((a, b) => b.Where(p => p.LotId == a.Id && p.ItemId == itemId));
            else
                query.Exists<LotLpnOnhand>((a, b) => b.Where(p => p.LotId == a.Id && p.Qty > 0 && p.ItemId == itemId));
            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(keyword));

            if (itemExtProp.IsNotEmpty())
                query.Where(p => p.ItemExtProp == itemExtProp);

            return query.OrderBy(p => p.Code).ToList(info, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取库存物料的批次信息
        /// </summary>       
        /// <param name="itemId">物料ID</param>
        /// <param name="keyword">关键字</param>
        /// <param name="info">分页信息</param>
        /// <param name="itemExtProp">物料扩展属性</param>
        /// <param name="isZero">包含现有量0</param>
        /// <param name="locId">库位</param>
        /// <returns>返回批次信息</returns>
        public virtual EntityList<Lot> GetItemLots(double itemId, double locId, string keyword, PagingInfo info, string itemExtProp = "", bool isZero = false)
        {
            var query = Query<Lot>();
            if (isZero)
                query.Exists<LotLpnOnhand>((a, b) => b.Where(p => p.LotId == a.Id && p.ItemId == itemId && p.StorageLocationId == locId));
            else
                query.Exists<LotLpnOnhand>((a, b) => b.Where(p => p.LotId == a.Id && p.Qty > 0 && p.ItemId == itemId && p.StorageLocationId == locId));
            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(keyword));

            if (itemExtProp.IsNotEmpty())
                query.Where(p => p.ItemExtProp == itemExtProp);

            return query.OrderBy(p => p.Code).ToList(info, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据物料获取批次信息
        /// </summary>
        /// <param name="itemId">物料Id</param>
        /// <param name="itemExtPropName">物料扩展属性</param>
        /// <param name="keyword">关键字</param>
        /// <param name="info">分页对象</param>
        /// <returns>返回批次信息</returns>
        public virtual EntityList<Lot> GetLotsByItem(double itemId, string itemExtPropName, string keyword, PagingInfo info)
        {
            var query = Query<Lot>();
            query.Where(p => p.ItemId == itemId);
            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(keyword));

            if (itemExtPropName.IsNotEmpty())
                query.Where(p => p.ItemExtPropName == itemExtPropName);

            return query.ToList(info, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据物料获取批次信息
        /// </summary>
        /// <param name="itemId">物料Id</param>
        /// <param name="elo">贪婪加载</param>
        /// <returns>返回批次信息</returns>
        public virtual EntityList<Lot> GetLotsByItem(double itemId, EagerLoadOptions elo = null)
        {
            var query = Query<Lot>();
            query.Where(p => p.ItemId == itemId);
            return query.ToList(null, elo);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>返回结果</returns>
        public virtual EntityList<Lot> GetLotData(LotCriteria criteria)
        {
            var query = Query<Lot>();
            if (!criteria.Code.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(criteria.Code));
            if (criteria.ItemId > 0)
                query.Where(p => p.ItemId == criteria.ItemId);
            if (criteria.LotAtt01.HasValue)
                query.Where(p => p.LotAtt01 == criteria.LotAtt01);
            if (criteria.LotAtt02.HasValue)
                query.Where(p => p.LotAtt02 == criteria.LotAtt02);
            if (criteria.LotAtt03.HasValue)
                query.Where(p => p.LotAtt03 == criteria.LotAtt03);
            if (!criteria.LotAtt04.IsNullOrEmpty())
                query.Where(p => p.LotAtt04 == criteria.LotAtt04);
            if (!criteria.AsnNo.IsNullOrEmpty())
                query.Where(p => p.AsnNo == criteria.AsnNo);
            if (criteria.IsNotDefault)
                query.Where(p => p.Code != Lot.LotDefault);

            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();
            query.OrderBy(criteria.OrderInfoList);
            return query.ToList(criteria.PagingInfo, elo);
        }

        /// <summary>
        /// 获取批次数据
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">查询关键字</param>
        /// <returns>批次列表</returns>
        public virtual EntityList<Lot> GetLotDatas(PagingInfo pagingInfo, string keyword)
        {
            var q = Query<Lot>();
            if (!keyword.IsNullOrEmpty())
            {
                q.Where(p => p.Code.Contains(keyword));
            }
            return q.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据物料获取批次信息
        /// </summary>
        /// <param name="itemId">物料Id</param>
        /// <param name="itemExtProp">物料扩展属性</param>
        /// <param name="keyword">关键字</param>
        /// <param name="info">分页对象</param>
        /// <param name="isIgnoreItemExtProp">是否忽略物料扩展属性</param>
        /// <returns>返回批次信息</returns>
        public virtual EntityList<Lot> GetLotsByItemExtProp(double itemId, string itemExtProp, string keyword, PagingInfo info, bool? isIgnoreItemExtProp = false)
        {
            var query = Query<Lot>();
            query.Where(p => p.ItemId == itemId);
            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(keyword));

            if (isIgnoreItemExtProp.HasValue && !isIgnoreItemExtProp.Value)
            {
                query.Where(p => p.ItemExtProp == itemExtProp);
            }

            return query.ToList(info, new EagerLoadOptions().LoadWithViewProperty());
        }

    }
}
