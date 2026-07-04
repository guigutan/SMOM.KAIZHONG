using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.Fixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.EmpWork
{
    /// <summary>
    /// 人员与工作中心控制器
    /// </summary>
    public class EmpWorkCentrtController : DomainController
    {
        /// <summary>
        /// 根据工作中心编码获取人员与工作中心数据
        /// </summary>
        /// <param name="workCenterCodes"></param>
        /// <returns></returns>
        public virtual EntityList<EmpWorkCentrt> GetEmpWorkCentrtsByWorkCenterCodes(List<string> workCenterCodes)
        {
            var list = workCenterCodes.SplitContains(codes =>
            {
                return Query<EmpWorkCentrt>().Where(p => codes.Contains(p.WorkCenter.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return list;
        }

        /// <summary>
        /// 查询人员与工作中心
        /// </summary>
        /// <param name="criterial"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual EntityList<EmpWorkCentrt> CriterialEmpWorkCentrt(EmpWorkCentrtCriterial criterial)
        {
            if (criterial == null)
            {
                throw new ValidationException("人员与工作中心查询实体异常！".L10N());
            }
            var q = Query<EmpWorkCentrt>();
            if (criterial.EmployeeId.HasValue)
            {
                q.Where(p => p.EmployeeId == criterial.EmployeeId);
            }
            if (criterial.WorkCenterId.HasValue)
            {
                q.Where(p => p.WorkCenterId == criterial.WorkCenterId);
            }
            if (!criterial.EmpNo.IsNullOrEmpty())
            {
                q.Where(m => m.EmpNo.Contains("%" + criterial.EmpNo + "%"));
            }
            if (!criterial.WorkName.IsNullOrEmpty())
            {
                q.Where(m => m.WorkName.Contains("%" + criterial.WorkName + "%"));
            }
         
            return q.OrderBy(criterial.OrderInfoList).ToList(criterial.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

    }
}
