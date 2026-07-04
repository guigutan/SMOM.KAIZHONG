using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Resources.PersonnelSkills
{
    /// <summary>
    /// 人员技能基础数据查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("人员技能基础数据查询实体")]
    public class PersonnelSkillCriteria : Criteria
    {
        #region 员工 Employee
        /// <summary>
        /// 员工Id
        /// </summary>
        [Label("员工")]
        public static readonly IRefIdProperty EmployeeIdProperty =
            P<PersonnelSkillCriteria>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

        /// <summary>
        /// 员工Id
        /// </summary>
        public double? EmployeeId
        {
            get { return (double?)this.GetRefNullableId(EmployeeIdProperty); }
            set { this.SetRefNullableId(EmployeeIdProperty, value); }
        }

        /// <summary>
        /// 员工
        /// </summary>
        public static readonly RefEntityProperty<Employee> EmployeeProperty =
            P<PersonnelSkillCriteria>.RegisterRef(e => e.Employee, EmployeeIdProperty);

        /// <summary>
        /// 员工
        /// </summary>
        public Employee Employee
        {
            get { return this.GetRefEntity(EmployeeProperty); }
            set { this.SetRefEntity(EmployeeProperty, value); }
        }
        #endregion

        #region 技能 Skill
        /// <summary>
        /// 技能Id
        /// </summary>
        [Label("技能")]
        public static readonly IRefIdProperty SkillIdProperty =
            P<PersonnelSkillCriteria>.RegisterRefId(e => e.SkillId, ReferenceType.Normal);

        /// <summary>
        /// 技能Id
        /// </summary>
        public double? SkillId
        {
            get { return (double?)this.GetRefNullableId(SkillIdProperty); }
            set { this.SetRefNullableId(SkillIdProperty, value); }
        }

        /// <summary>
        /// 技能
        /// </summary>
        public static readonly RefEntityProperty<Skill> SkillProperty =
            P<PersonnelSkillCriteria>.RegisterRef(e => e.Skill, SkillIdProperty);

        /// <summary>
        /// 技能
        /// </summary>
        public Skill Skill
        {
            get { return this.GetRefEntity(SkillProperty); }
            set { this.SetRefEntity(SkillProperty, value); }
        }
        #endregion

        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<PersonnelSkillsController>().CriteriaPersonnelSkills(this);
        }
    }
}
