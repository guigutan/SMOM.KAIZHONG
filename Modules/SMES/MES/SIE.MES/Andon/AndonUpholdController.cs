using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.Fixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Andon
{
    /// <summary>
    /// 安灯区域控制器
    /// </summary>
    public class AndonUpholdController : DomainController
    {
        /// <summary>
        /// 查询工装维护
        /// </summary>
        /// <param name="criterial"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual EntityList<AndonUphold> CriterialAndonUphold(AndonUpholdCriterial criterial)
        {
            if (criterial == null)
            {
                throw new ValidationException("安灯区域查询实体异常！".L10N());
            }
            var q = Query<AndonUphold>();
            if (!criterial.AndonDesc.IsNullOrEmpty())
            {
                q.Where(m => m.AndonDesc.Contains("%" + criterial.AndonDesc + "%"));
            }
            if (!criterial.AndonCode.IsNullOrEmpty())
            {
                q.Where(m => m.AndonCode.Contains("%" + criterial.AndonCode + "%"));
            }
            if (!criterial.PlantCode.IsNullOrEmpty())
            {
                q.Where(m => m.PlantCode.Contains("%" + criterial.PlantCode + "%"));
            }
            if (!criterial.PlantName.IsNullOrEmpty())
            {
                q.Where(m => m.PlantName.Contains("%" + criterial.PlantName + "%"));
            }
            return q.OrderBy(criterial.OrderInfoList).ToList(criterial.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 是否有相同的安灯区域
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public virtual bool GetAndonUpholdBool(string desc,string ip)
        {
            var q = Query<AndonUphold>().Where(p => p.AndonDesc == desc && p.AndonCode==ip).ToList();
            if (q.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 安灯区域
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<AndonUphold> GetAndonUpholds(List<string> codes)
        {
            var list = codes.SplitContains(c =>
            {
                return Query<AndonUphold>().Where(p => c.Contains(p.AndonCode)).ToList();
            });
            return list;
        }

        /// <summary>
        /// 安灯区域
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<AndonUphold> GetAndonUpholdsByID(double id)
        {
            return Query<AndonUphold>().Where(p => p.Id==id).ToList();
        }

    }
}
