using NPOI.POIFS.FileSystem;
using SIE.Domain;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.LES.StockOrders
{
    /// <summary>
    /// 备料单合并下发规则控制器
    /// </summary>
    public class StockOrderMergeIssuedController : DomainController
    {
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<StockOrderMergeIssued> GetMergeIssued(StockOrderMergeIssuedCriteria criteria)
        {
            IEntityQueryer<StockOrderMergeIssued> query = Query<StockOrderMergeIssued>();
            if (criteria.WipResource != null)
                query.Where(p => p.LinesideWarehouse.WipResouceId == criteria.WipResourceId);
            if (criteria.StockModel != null)
                query.Where(p => p.StockModel == criteria.StockModel);
            if (criteria.Warehouse != null)
                query.Where(p => p.LinesideWarehouse.WarehouseId == criteria.WarehouseId);
            //创建人
            if (criteria.CreateById.HasValue)
                query.Where(p => p.CreateBy == criteria.CreateById.Value);
            query.WhereIf(criteria.CreateDate.BeginValue.HasValue, p => p.CreateDate >= criteria.CreateDate.BeginValue);
            query.WhereIf(criteria.CreateDate.EndValue.HasValue, p => p.CreateDate <= criteria.CreateDate.EndValue);
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
       
        /// <summary>
        /// 获取可用的下发规则
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public virtual EntityList<StockOrderMergeIssued> GetMergeIssued(State state)
        {            
            return Query<StockOrderMergeIssued>().Where(p => p.State == state).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 启用
        /// </summary>
        /// <param name="stockOrderIds"></param>
        /// <returns></returns>
        public virtual int StockOrderMergeIssuedEnable(List<double> stockOrderIds)
        {
            var datas = Query<StockOrderMergeIssued>().Where(p => stockOrderIds.Contains(p.Id) && p.State == State.Disable).ToList();
            datas.ForEach(p =>
            {
                if (p.StockOrderMergeTimesList.Count == 0)
                {
                    throw new ValidationException("合并时间段未设置不能启用".L10N());
                }
                p.State = State.Enable;
            });
            if (datas.Any())
            {
                RF.Save(datas);
            }
            return 0;
        }

        /// <summary>
        /// 获取重复的数据
        /// </summary>
        /// <param name="issued"></param>
        /// <returns></returns>
        public virtual StockOrderMergeIssued Repeated(StockOrderMergeIssued issued)
        {
            return Query<StockOrderMergeIssued>().Where(p => p.Id != issued.Id && p.LinesideWarehouseId == issued.LinesideWarehouseId && p.StockModel == issued.StockModel).FirstOrDefault();
        }
        /// <summary>
        /// 根据产线线边仓ID获取下发规则
        /// </summary>
        /// <param name="linesideWarehouseId"></param>
        /// <returns></returns>
        public virtual StockOrderMergeIssued GetStockOrderMergeIssued(double linesideWarehouseId)
        {
            return Query<StockOrderMergeIssued>().Where(p => p.LinesideWarehouseId == linesideWarehouseId).FirstOrDefault();
        }

        /// <summary>
        /// 是否时间段交集
        /// </summary>
        /// <param name="stockOrderMergeTimes">合并时间段</param>
        /// <returns>是否</returns>
        public virtual bool IsIntersection(StockOrderMergeTimes stockOrderMergeTimes)
        {
            var stockOrderMergeTimesList = Query<StockOrderMergeTimes>().Where(p => p.StockOrderMergeIssuedId == stockOrderMergeTimes.StockOrderMergeIssuedId && p.Id != stockOrderMergeTimes.Id).ToList();
            if (stockOrderMergeTimesList.Count > 0)
            {
                foreach (var stockOrderTimes in stockOrderMergeTimesList)
                {
                    if (stockOrderMergeTimes.End >= stockOrderTimes.Start && stockOrderMergeTimes.Start <= stockOrderTimes.End)
                    {
                        return true;
                    }
                }
            }
            if (stockOrderMergeTimes.IsCrossDay)
            {
                var stocks = stockOrderMergeTimesList.Where(p => p.IsCrossDay).ToList();
                if (stocks.Count == 1 && stockOrderMergeTimes.IsCrossDay)
                {
                    throw new ValidationException("合并时间段只能存在一条跨日数据".L10N());
                }
            }
            return false;
        }
    }
}
