using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Scripting.Interpreter;
using Microsoft.Scripting.Utils;
using SIE.Domain;
using SIE.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Item = SIE.Items.Item;

namespace SIE.MES.MtartProcessLookups
{
    public class MtartProcessLookupsController : DomainController
    {
        /// <summary>
        /// 根据分类和工序获取物料分类与工序关系对照表和物料
        /// </summary>
        /// <param name="ItemCategoryIds"></param>
        /// <returns></returns>
        public virtual EntityList<MtartProcessLookup> GetMtartProcessLookupsByItemCategoryId(List<double> ItemCategoryIds, List<double> processIds)
        {
            var list = ItemCategoryIds.SplitContains(ids =>
            {
                return Query<MtartProcessLookup>().Where(p => ids.Contains((double)p.ItemCategoryId) && processIds.Contains(p.ProcessId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return list;
        }

        /// <summary>
        /// 根据工序Id获取物料分类与工序关系对照表和物料
        /// </summary>
        /// <param name="processIds"></param>
        /// <returns></returns>
        public virtual List<double> GetItemsByProcessIds(List<double> processIds,List<double> itemIds)
        {
            //查找当分类不为空的时候
            var idsA = Query<ItemCategoryRelation>().Join<MtartProcessLookup>((x, y) => x.ItemCategoryId == y.ItemCategoryId && y.ItemCategoryId != null && processIds.Contains(y.ProcessId) && itemIds.Contains(x.ItemId)).Select(p => p.ItemId).Distinct().ToList<double>().ToList();
            //当分类为空的时候
            //var idsB = Query<Item>().Join<MtartProcessLookup>((x, y) => x.Mtart == y.Mtart && x.MrpController == y.Dispo && processIds.Contains(y.ProcessId) && itemIds.Contains(x.Id)).Select(p => p.Id).Distinct().ToList<double>().ToList();

            //idsA.AddRange(idsB);
            idsA = idsA.Distinct().ToList();
            return idsA;
        }

        /// <summary>
        /// 根据工序Id获取物料分类与工序关系对照表
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<MtartProcessLookup> GetMtartProcessLookupsByProcessIds(List<double> processIds)
        {
            var list = processIds.SplitContains(ids =>
            {
                return Query<MtartProcessLookup>().Where(p => ids.Contains(p.ProcessId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return list;
        }

        /// <summary>
        /// 物料分类与工序关系对照表查询方法
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<MtartProcessLookup> CriteriaMtartProcessLookups(MtartProcessLookupCriteria criteria)
        {
            var q = Query<MtartProcessLookup>();

            if (criteria.ProcessId != null)
                q.Where(p => p.ProcessId == criteria.ProcessId);
            if (!criteria.Mtart.IsNullOrEmpty())
                q.Where(p => p.Mtart.Contains(criteria.Mtart));
            if (!criteria.Dispo.IsNullOrEmpty())
                q.Where(p => p.Dispo.Contains(criteria.Dispo));
            if (criteria.ItemCategoryId != null)
                q.Where(p => p.ItemCategoryId == criteria.ItemCategoryId);

            var list = q.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            return list;
        }
    }
}
