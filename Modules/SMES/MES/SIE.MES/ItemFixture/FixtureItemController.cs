using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.ItemChecker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ItemFixture
{
    /// <summary>
    /// 工装与产品的关系控制器
    /// </summary>
    public class FixtureItemController :DomainController
    {
        #region 查询工装与产品关系
        /// <summary>
        /// 查询工装与产品关系
        /// </summary>
        /// <param name="criterial"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual EntityList<FixtureItem> CriterialFixtureItem(FixtureItemCriterial criterial)
        {
            if (criterial == null)
            {
                throw new ValidationException("工装与产品关系查询实体异常！".L10N());
            }
            var query = Query<FixtureItem>();
            if (criterial.ItemId.HasValue)
            {
                if (criterial.ItemId > 0)
                {
                    query.Where(p => p.ItemId == criterial.ItemId);
                }
            }
            if (criterial.ProcessId.HasValue)
            {
                query.Where(p => p.ProcessId == criterial.ProcessId);
            }
            if (criterial.FixtureUpholdId.HasValue)
            {
                query.Where(p => p.FixtureUpholdId == criterial.FixtureUpholdId);
            }
            if (!criterial.ItemName.IsNullOrEmpty())
            {
                query.Where(m => m.Item.Name.Contains("%" + criterial.ItemName + "%"));
            }
            if (!criterial.FixtureName.IsNullOrEmpty())
            {
                query.Where(m => m.FixtureUphold.FixtureName.Contains("%" + criterial.FixtureName + "%"));
            }
            if (!criterial.ProcessCode.IsNullOrEmpty())
            {
                query.Where(p => p.Process.Code.Contains(criterial.ProcessCode));
            }
            if (!criterial.Drawn.IsNullOrEmpty())
                query.Where(p => p.FixtureUphold.Drawn.Contains(criterial.Drawn));
            return query.OrderBy(criterial.OrderInfoList).ToList(criterial.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
        #endregion

        #region 启用工装与产品关系
        /// <summary>
        /// 启用工装与产品关系
        /// </summary>
        /// <returns></returns>
        public virtual void EnableFixtureItem(List<double> LineIds)
        {
            //var lineList = LineIds.SplitContains(tempIds =>
            //{
            //    return Query<FixtureItem>().Where(p => tempIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            //});
            //lineList.ForEach(p =>
            //{
            //    p.State = State.Enable;
            //});
            //RF.Save(lineList);
        }
        #endregion

        #region 禁用工装与产品关系
        /// <summary>
        /// 禁用工装与产品关系
        /// </summary>
        /// <param name="LineIds"></param>
        public virtual void DisableFixtureItem(List<double> LineIds)
        {
            //var lineList = LineIds.SplitContains(tempIds =>
            //{
            //    return Query<FixtureItem>().Where(p => tempIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            //});
            //lineList.ForEach(p =>
            //{
            //    p.State = State.Disable;
            //});
            //RF.Save(lineList);
        }
        #endregion

        #region 是否有相同的数据
        /// <summary>
        /// 是否有相同的数据
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="processId"></param>
        /// <param name="wipId"></param>
        /// <returns></returns>
        public virtual bool GetFixtureItemBool(double itemId, double processId, double fixId)
        {
            var query = Query<FixtureItem>().Where(p => p.ItemId == itemId && p.ProcessId == processId && p.FixtureUpholdId == fixId).ToList();
            if (query.Count > 0)
                return true;
            else
                return false;
        }
        #endregion
    }
}
