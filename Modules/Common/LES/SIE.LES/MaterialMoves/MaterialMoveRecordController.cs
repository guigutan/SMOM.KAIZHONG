using SIE.Common;
using SIE.Domain;
using SIE.LES.MaterialMoves.ApiModels;
using SIE.LES.MaterialReturnApplys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SIE.LES.MaterialMoves
{
    /// <summary>
    /// 挪料控制器
    /// </summary>
    public partial class MaterialMoveRecordController : DomainController
    {
        /// <summary>
        /// 查询挪料记录
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<MaterialMoveRecord> QueryMaterialMoveRecord(MaterialMoveRecordCriteria criteria)
        {
            if (criteria == null)
            {
                return new EntityList<MaterialMoveRecord>();
            }
            var query = Query<MaterialMoveRecord>();
            if (criteria.SourceWoId != null && criteria.SourceWoId != 0)
            {
                query.Where(p => p.SourceWoId == criteria.SourceWoId);
            }
            if (criteria.TargetWoId != null && criteria.TargetWoId != 0)
            {
                query.Where(p => p.TargetWoId == criteria.TargetWoId);
            }
            if (criteria.ItemId != null && criteria.ItemId != 0)
            {
                query.Where(p => p.ItemId == criteria.ItemId);
            }
            if (criteria.Reason.IsNotEmpty())
            {
                query.Where(p => p.Reason == criteria.Reason);
            }
            if (criteria.WarehouseId != null && criteria.WarehouseId != 0)
            {
                query.Where(p => p.WarehouseId == criteria.WarehouseId);
            }
            if (criteria.SourceType.HasValue)
            {
                query.Where(p => p.MoveSourceType == criteria.SourceType);
            }
            if (criteria.CreaterId != null && criteria.CreaterId != 0)
            {
                query.Where(p => p.CreateBy == criteria.CreaterId);
            }
            if (criteria.CreateDate.BeginValue.HasValue)
            {
                query.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue);
            }
            if (criteria.CreateDate.EndValue.HasValue)
            {
                query.Where(p => p.CreateDate <= criteria.CreateDate.EndValue);
            }
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工单号获取退料申请明细
        /// </summary>
        /// <param name="woNo">工单号</param>
        /// <param name="elo">贪婪</param>
        /// <returns></returns>
        public virtual EntityList<MaterialMoveRecord> GetMaterialMoveRecordsByWoNo(string woNo, EagerLoadOptions elo = null)
        {
            var list = new EntityList<MaterialMoveRecord>();
            var query1 = Query<MaterialMoveRecord>().Where(p => p.SourceWo.No == woNo);
            list.AddRange(query1.ToList(null, elo));
            var query2 = Query<MaterialMoveRecord>().Where(p => p.TargetWo.No == woNo);
            list.AddRange(query2.ToList(null, elo));
            return list.DistinctBy(p => p.Id).AsEntityList();
        }

        /// <summary>
        /// 获取工单挪料记录统计挪出数
        /// </summary>
        /// <param name="woIds"></param>
        /// <returns></returns>
        public virtual Dictionary<Tuple<double, double, string>, decimal> GetWoItemMoveOutQtyDic(List<double> woIds)
        {
            List<MoveRecordInfo> moveRecordInfos = new List<MoveRecordInfo>();
            woIds.SplitDataExecute(temps =>
            {
                var list = Query<MaterialMoveRecord>()
                .Where(p => p.SourceWoId != null && temps.Contains((double)p.SourceWoId))
                .GroupBy(p => new { p.SourceWoId, p.ItemId, p.ItemExtProp })
                .Select(p => new
                {
                    WoId = p.SourceWoId,
                    ItemId = p.ItemId,
                    ItemExtProp = p.ItemExtProp,
                    MoveQty = p.MoveQty.SUM(),
                }).ToList<MoveRecordInfo>();
                moveRecordInfos.AddRange(list);
            });
            return moveRecordInfos.ToDictionary(p => new Tuple<double, double, string>(p.WoId, p.ItemId, p.ItemExtProp), p => p.MoveQty);
        }

        /// <summary>
        /// 获取工单挪料记录统计挪入数
        /// </summary>
        /// <param name="woIds"></param>
        /// <returns></returns>
        public virtual Dictionary<Tuple<double, double, string>, decimal> GetWoItemMoveInQtyDic(List<double> woIds)
        {
            List<MoveRecordInfo> moveRecordInfos = new List<MoveRecordInfo>();
            woIds.SplitDataExecute(temps =>
            {
                var list = Query<MaterialMoveRecord>()
                .Where(p => p.TargetWoId != null && temps.Contains((double)p.TargetWoId))
                .GroupBy(p => new { p.TargetWoId, p.ItemId, p.ItemExtProp })
                .Select(p => new
                {
                    WoId = p.TargetWoId,
                    ItemId = p.ItemId,
                    ItemExtProp = p.ItemExtProp,
                    MoveQty = p.MoveQty.SUM(),
                }).ToList<MoveRecordInfo>();
                moveRecordInfos.AddRange(list);
            });
            return moveRecordInfos.ToDictionary(p => new Tuple<double, double, string>(p.WoId, p.ItemId, p.ItemExtProp), p => p.MoveQty);
        }
    }
}
