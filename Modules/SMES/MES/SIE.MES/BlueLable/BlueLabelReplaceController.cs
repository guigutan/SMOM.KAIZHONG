using SIE.Domain;
using SIE.MES.LineAndon;
using SIE.Resources.Enterprises;
using System;
using System.ComponentModel.DataAnnotations;

namespace SIE.MES.BlueLable
{
    public class BlueLabelReplaceController : DomainController
    {
        /// <summary>
        /// 查询方式
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<BlueLabelReplace> CriteriaBlueLabelReplace(BlueLabelReplaceCriteria criterial)
        {
            if (criterial == null)
            {
                throw new ValidationException("蓝标替换查询实体异常！".L10N());
            }
            var q = Query<BlueLabelReplace>();
            if (!criterial.OldBlueLabel.IsNullOrEmpty())
            {
                q.Where(m => m.OldBlueLabel.Contains("%" + criterial.OldBlueLabel + "%"));
            }
            if (!criterial.NewBlueLabel.IsNullOrEmpty())
            {
                q.Where(m => m.NewBlueLabel.Contains("%" + criterial.NewBlueLabel + "%"));
            }
            return q.OrderBy(criterial.OrderInfoList).ToList(criterial.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 新旧蓝标替换
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual void BlueLabelReplace(BlueLabelReplaceCriteria criterial)
        {
          
        }

    }
}