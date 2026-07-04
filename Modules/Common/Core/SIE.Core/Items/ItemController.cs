using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Core.Items
{
    /// <summary>
    /// 物料基类控制器
    /// </summary>
    public class ItemController : DomainController
    {
        /// <summary>
        /// 查询物料
        /// </summary>
        /// <param name="criteria">物料查询实体</param>
        /// <returns>物料类别</returns>
        /// <exception cref="ArgumentNullException">查询实体为空</exception>
        public virtual EntityList<Item> GetItems(ItemCriteria criteria)
        {
            if (criteria == null)
                throw new ArgumentNullException(nameof(criteria));
            var query = Query<Item>();
            if (criteria.Code.IsNotEmpty())
                query.Where(e => e.Code.Contains(criteria.Code));
            if (criteria.Name.IsNotEmpty())
                query.Where(e => e.Name.Contains(criteria.Name));
            if (criteria.Description.IsNotEmpty())
                query.Where(e => e.Description.Contains(criteria.Description));
            if (criteria.State != null)
                query.Where(e => e.State == criteria.State);
            return query.ToList(criteria.PagingInfo);
        }

        /// <summary>
        /// 根据物料Id去匹配物料表中的物料并返回
        /// </summary>
        /// <param name="itemIds">物料ID</param>
        /// <param name="criteria">查询实体</param>
        /// <returns>物料列表</returns>
        public virtual EntityList<Item> GetItemList(List<double> itemIds, ItemCriteria criteria = null)
        {
            var query = DB.Query<Item>("i");
            var meta = RF.Find<Item>().EntityMeta;
            return query.Where(p => p.SQL<bool>(new Data.FormattedSql(@"i.{0} in ({1})".FormatArgs(meta.Property(Item.IdProperty).ColumnMeta.ColumnName, string.Join(",", itemIds))))).ToList(criteria?.PagingInfo);
        }

        /// <summary>
        /// 获取物料数据
        /// </summary>
        /// <param name="codes">编码</param>
        /// <param name="elo"></param>
        /// <returns></returns>
        public virtual EntityList<Item> GetItems(List<string> codes, EagerLoadOptions elo = null)
        {
            return codes.SplitContains(p =>
            {
                return Query<Item>().Where(a => p.Contains(a.Code)).ToList(null, elo);
            });
        }

        /// <summary>
        /// 获取物料数据
        /// </summary>
        /// <param name="ids">Id</param>
        /// <param name="elo"></param>
        public virtual EntityList<Item> GetItems(List<double> ids, EagerLoadOptions elo = null)
        {
            return ids.SplitContains(p =>
            {
                return Query<Item>().Where(a => p.Contains(a.Id)).ToList(null, elo);
            });
        }

        /// <summary>
        /// 通过物料编码列表 获取物料列表(忽略库存组织）
        /// </summary>
        /// <param name="codes">物料编码列表</param>
        /// <returns>物料列表</returns>
        public virtual EntityList<Item> GetItemInvOrgList(List<string> codes)
        {
            using (SIE.Common.InvOrg.InvOrgs.WithAll())
            {
                return codes.SplitContains((tmpCodes) =>
                {
                    return Query<Item>().Where(p => tmpCodes.Contains(p.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                });
            }
        }
    }
}
