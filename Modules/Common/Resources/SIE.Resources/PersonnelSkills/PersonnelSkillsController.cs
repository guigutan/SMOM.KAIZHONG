using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Resources.PersonnelSkills
{
    public class PersonnelSkillsController : DomainController
    {
        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<PersonnelSkill> CriteriaPersonnelSkills(PersonnelSkillCriteria criteria)
        {
            var q = Query<PersonnelSkill>();
            if (criteria.EmployeeId != null)
                q.Where(p => p.EmployeeId == criteria.EmployeeId);
            if (criteria.SkillId != null)
                q.Where(p => p.SkillId == criteria.SkillId);

            var list = q.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }

        /// <summary>
        /// 根据工号获取人员与技能关系
        /// </summary>
        /// <param name="employeeCodes"></param>
        /// <returns></returns>
        public virtual EntityList<PersonnelSkill> GetPersonnelSkillsByEmployeeCodes(List<string> employeeCodes)
        {
            var list = employeeCodes.SplitContains(codes =>
            {
                return Query<PersonnelSkill>().Where(p => codes.Contains(p.Employee.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return list;
        }

        /// <summary>
        /// 根据员工ID获取人员与技能关系
        /// </summary>
        /// <param name="employeeIds"></param>
        /// <returns></returns>
        public virtual EntityList<PersonnelSkill> GetPersonnelSkillsByEmployeeIds(List<double> employeeIds)
        {
            var list = employeeIds.SplitContains(ids =>
            {
                return Query<PersonnelSkill>().Where(p => ids.Contains(p.EmployeeId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });

            return list;
        }
    }
}
