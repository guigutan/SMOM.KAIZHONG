using SIE.Domain;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Resources.Skills
{
    /// <summary>
    /// 技能控制器
    /// </summary>
    public class SkillController : DomainController
    {

        /// <summary>
        /// 获取任意一个技能分类
        /// </summary>
        /// <returns></returns>
        public virtual SkillCategory GetSkillCategoryFirst()
        {
            var first = Query<SkillCategory>().FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            return first;
        }

        /// <summary>
        /// 通过技能Id列表获取员工技能列表
        /// </summary>
        /// <param name="employeeIds">员工Id列表</param>
        /// <returns>员工技能列表</returns>
        public virtual EntityList<EmployeeSkill> GetEmployeeSkillsBySkillIds(List<double> skillIds)
        {
            var list = skillIds.SplitContains(ids =>
            {
                return Query<EmployeeSkill>().Where(p => ids.Contains(p.SkillId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return list;
        }

        /// <summary>
        /// 获取技能
        /// </summary>
        /// <param name="code">技能编码</param>
        /// <returns>返回技能</returns>
        public virtual Skill GetSkill(string code)
        {
            return Query<Skill>().Where(p => p.Code == code).FirstOrDefault();
        }

        /// <summary>
        /// 获取技能
        /// </summary>
        /// <param name="code">技能编码</param>
        /// <returns>返回技能</returns>
        public virtual EntityList<Skill> GetSkills(List<string> codes)
        {
            var list = codes.SplitContains(cs =>
            {
                return Query<Skill>().Where(p => cs.Contains(p.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return list;
        }

        /// <summary>
        /// 获取技能列表
        /// </summary>
        /// <param name="categoryId">技能分类ID</param>
        /// <returns>技能列表</returns>
        public virtual EntityList<Skill> GetSkills(double categoryId)
        {
            return Query<Skill>().Where(p => p.CategoryId == categoryId).ToList();
        }

        #region 员工技能
        /// <summary>
        /// 根据员工获取认证技能
        /// </summary>
        /// <param name="employeeId">员工ID</param>
        /// <param name="sortInfo">排序</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>员工技能列表</returns>
        public virtual EntityList<EmployeeSkill> GetEmployeeSkillList(double employeeId, List<OrderInfo> sortInfo = null, PagingInfo pagingInfo = null)
        {
            var query = Query<EmployeeSkill>();
            if (sortInfo != null)
                query.OrderBy(sortInfo);
            return query.Where(p => p.EmployeeId == employeeId).ToList(pagingInfo, eagerLoad: new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据技能获取员工授予记录
        /// </summary>
        /// <param name="skillId">技能ID</param>
        /// <param name="sortInfo">排序</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>员工技能列表</returns>
        public virtual EntityList<EmployeeSkill> GetEmployeeSkillListBySkill(double skillId, List<OrderInfo> sortInfo = null, PagingInfo pagingInfo = null)
        {
            var query = Query<EmployeeSkill>();
            if (sortInfo != null)
                query.OrderBy(sortInfo);
            return query.Where(p => p.SkillId == skillId).ToList(pagingInfo, eagerLoad: new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取员工技能By员工ID集合
        /// </summary>
        /// <param name="empIds">员工ID集合</param>
        /// <returns>员工技能</returns>
        public virtual EntityList<EmployeeSkill> GetEmployeeSkill(List<double> empIds)
        {
            var dbTime = RF.Find<EmployeeSkill>().GetDbTime().Date;
            return Query<EmployeeSkill>().Where(p => empIds.Contains(p.EmployeeId) && (p.ExpireDate >= dbTime || p.ExpireDate == null)).ToList();
        }

        /// <summary>
        /// 获取员工技能By员工ID
        /// </summary>
        /// <param name="empId">员工ID</param>
        /// <returns>员工技能</returns>
        public virtual EntityList<EmployeeSkill> GetEmployeeSkill(double empId)
        {
            var dbTime = RF.Find<EmployeeSkill>().GetDbTime().Date;
            return Query<EmployeeSkill>().Where(p => empId == p.EmployeeId && (p.ExpireDate >= dbTime || p.ExpireDate == null)).ToList();
        }

        /// <summary>
        /// 获取员工技能
        /// </summary>
        /// <param name="employeeId">员工ID</param>
        /// <param name="skillId">技能ID</param>
        /// <returns>员工技能</returns>
        public virtual EmployeeSkill GetEmployeeSkill(double employeeId, double skillId)
        {
            return Query<EmployeeSkill>().Where(p => p.EmployeeId == employeeId && p.SkillId == skillId).FirstOrDefault();
        }

        /// <summary>
        /// 通过员工Id列表获取员工技能列表
        /// </summary>
        /// <param name="employeeIds">员工Id列表</param>
        /// <returns>员工技能列表</returns>
        public virtual EntityList<EmployeeSkill> GetEmployeeSkills(List<double> employeeIds)
        {
            return Query<EmployeeSkill>().Where(p => employeeIds.Contains(p.EmployeeId)).ToList(null, new EagerLoadOptions().LoadWith(EmployeeSkill.SkillProperty));
        }
        #endregion
    }
}