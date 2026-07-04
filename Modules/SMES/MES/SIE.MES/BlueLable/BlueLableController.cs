using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.BlueLable
{
    public class BlueLableController: DomainController
    {
        /// <summary>
        /// 蓝标标签查询
        /// </summary>
        /// <param name="criterial"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual EntityList<BlueLable> CriterialBlueLable(BlueLableCriterial criterial)
        {
            if (criterial == null)
            {
                throw new ValidationException("蓝标标签查询实体异常！".L10N());
            }
            var query = Query<BlueLable>();

            if (criterial.ItemId.HasValue)
            {
                if (criterial.ItemId > 0)
                {
                    query.Where(p => p.ItemId == criterial.ItemId);
                }
            }
            if (!criterial.ExternalIdent.IsNullOrEmpty())
            {
                query.Where(m => m.ExternalIdent.Contains("%" + criterial.ExternalIdent + "%"));
            }

            if (!criterial.BatchNo.IsNullOrEmpty())
            {
                query.Where(m => m.BatchNo.Contains("%" + criterial.BatchNo + "%"));
            }

            if (!criterial.StorageLocation.IsNullOrEmpty())
            {
                query.Where(m => m.StorageLocation.Contains("%" + criterial.StorageLocation + "%"));
            }

            if (!criterial.BlueLableBox.IsNullOrEmpty())
            {
                query.Where(m => m.BlueLableBox.Contains("%" + criterial.BlueLableBox + "%"));
            }

            if (!criterial.ProductionNo.IsNullOrEmpty())
            {
                query.Where(m => m.ProductionNo.Contains("%" + criterial.ProductionNo + "%"));
            }

            if (criterial.CreateDate.BeginValue != null)
                query.Where(p => p.CreateDate >= criterial.CreateDate.BeginValue.Value);
            if (criterial.CreateDate.EndValue != null)
                query.Where(p => p.CreateDate <= criterial.CreateDate.EndValue.Value);

            return query.OrderBy(criterial.OrderInfoList).ToList(criterial.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 蓝标层级查询
        /// </summary>
        /// <param name="criterial"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual EntityList<BlueLableLevel> CriterialBlueLableLevel(BlueLableLevelCriterial criterial)
        {
            if (criterial == null)
            {
                throw new ValidationException("蓝标层级查询实体异常！".L10N());
            }
            var query = Query<BlueLableLevel>();
            if (criterial.ItemId.HasValue)
            {
                if (criterial.ItemId > 0)
                {
                    query.Where(p => p.ItemId == criterial.ItemId);
                }
            }
            return query.OrderBy(criterial.OrderInfoList).ToList(criterial.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取蓝标
        /// </summary>
        /// <param name="blueBoxList">蓝标</param>
        /// <returns>获取蓝标</returns>
        public virtual EntityList<BlueLable> GetBlueLableDatas(List<string> blueBoxList)
        {
            return blueBoxList.SplitContains(nos =>
            {
                return Query<BlueLable>().Where(p => nos.Contains(p.BlueLableBox)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 获取所有物料
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<Item> GetItems()
        {
            return Query<Item>().ToList();
        }
    }
}
