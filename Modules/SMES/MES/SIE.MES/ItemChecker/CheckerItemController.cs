using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.ItemLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ItemChecker
{
    /// <summary>
    /// 检具与产品关系控制器
    /// </summary>
    public class CheckerItemController : DomainController
    {
        #region 查询检具与产品关系
        /// <summary>
        /// 查询检具与产品关系
        /// </summary>
        /// <param name="criterial"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual EntityList<CheckerItem> CriterialCheckerItem(CheckerItemCriterial criterial)
        {
            if (criterial == null)
            {
                throw new ValidationException("检具与产品关系查询实体异常！".L10N());
            }
            var query = Query<CheckerItem>();
         
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
            if (criterial.CheckerUpholdId.HasValue)
            {
                query.Where(p => p.CheckerUpholdId == criterial.CheckerUpholdId);
            }
            if (!criterial.ItemName.IsNullOrEmpty())
            {
                query.Where(m => m.Item.Name.Contains("%" + criterial.ItemName + "%"));
            }
            if (!criterial.ProcessCode.IsNullOrEmpty())
            {
                query.Where(p => p.Process.Code.Contains(criterial.ProcessCode));
            }
            return query.OrderBy(criterial.OrderInfoList).ToList(criterial.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
        #endregion

        #region 启用检具与产品关系
        /// <summary>
        /// 启用检具与产品关系
        /// </summary>
        /// <returns></returns>
        public virtual void EnableCheckerItem(List<double> LineIds)
        {
          
        }
        #endregion

        #region 禁用检具与产品关系
        /// <summary>
        /// 禁用检具与产品关系
        /// </summary>
        /// <param name="LineIds"></param>
        public virtual void DisableCheckerItem(List<double> LineIds)
        {

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
        public virtual bool GetCheckerItemBool(double itemId, double processId, double checkerId)
        {
            var query = Query<CheckerItem>().Where(p => p.ItemId == itemId && p.ProcessId == processId && p.CheckerUpholdId == checkerId).ToList();
            if (query.Count > 0)
                return true;
            else
                return false;
        }
        #endregion
    }
}
