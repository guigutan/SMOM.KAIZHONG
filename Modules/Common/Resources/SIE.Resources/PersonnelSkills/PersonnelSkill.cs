using SIE.Domain;
using SIE.MetaModel;
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
    /// 人员技能基础数据
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(PersonnelSkillCriteria))]
    [Label("人员技能基础数据")]
    public class PersonnelSkill : DataEntity
    {
        #region 员工 Employee
        /// <summary>
        /// 员工Id
        /// </summary>
        [Label("员工")]
        public static readonly IRefIdProperty EmployeeIdProperty =
            P<PersonnelSkill>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

        /// <summary>
        /// 员工Id
        /// </summary>
        public double EmployeeId
        {
            get { return (double)this.GetRefId(EmployeeIdProperty); }
            set { this.SetRefId(EmployeeIdProperty, value); }
        }

        /// <summary>
        /// 员工
        /// </summary>
        public static readonly RefEntityProperty<SIE.Resources.Employees.Employee> EmployeeProperty =
            P<PersonnelSkill>.RegisterRef(e => e.Employee, EmployeeIdProperty);

        /// <summary>
        /// 员工
        /// </summary>
        public SIE.Resources.Employees.Employee Employee
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
            P<PersonnelSkill>.RegisterRefId(e => e.SkillId, ReferenceType.Normal);

        /// <summary>
        /// 技能Id
        /// </summary>
        public double SkillId
        {
            get { return (double)this.GetRefId(SkillIdProperty); }
            set { this.SetRefId(SkillIdProperty, value); }
        }

        /// <summary>
        /// 技能
        /// </summary>
        public static readonly RefEntityProperty<Skill> SkillProperty =
            P<PersonnelSkill>.RegisterRef(e => e.Skill, SkillIdProperty);

        /// <summary>
        /// 技能
        /// </summary>
        public Skill Skill
        {
            get { return this.GetRefEntity(SkillProperty); }
            set { this.SetRefEntity(SkillProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 技能编码 SkillCode
        /// <summary>
        /// 技能编码
        /// </summary>
        [Label("技能编码")]
        public static readonly Property<string> SkillCodeProperty = P<PersonnelSkill>.RegisterView(e => e.SkillCode, p => p.Skill.Code);

        /// <summary>
        /// 技能编码
        /// </summary>
        public string SkillCode
        {
            get { return this.GetProperty(SkillCodeProperty); }
        }
        #endregion

        #region 技能名称 SkillName
        /// <summary>
        /// 技能名称
        /// </summary>
        [Label("技能名称")]
        public static readonly Property<string> SkillNameProperty = P<PersonnelSkill>.RegisterView(e => e.SkillName, p => p.Skill.Name);

        /// <summary>
        /// 技能名称
        /// </summary>
        public string SkillName
        {
            get { return this.GetProperty(SkillNameProperty); }
        }
        #endregion

        #region 员工工号 EmployeeCode
        /// <summary>
        /// 员工工号
        /// </summary>
        [Label("员工工号")]
        public static readonly Property<string> EmployeeCodeProperty = P<PersonnelSkill>.RegisterView(e => e.EmployeeCode, p => p.Employee.Code);

        /// <summary>
        /// 员工工号
        /// </summary>
        public string EmployeeCode
        {
            get { return this.GetProperty(EmployeeCodeProperty); }
        }
        #endregion

        #endregion

    }

    internal class PersonnelSkillConfig : EntityConfig<PersonnelSkill>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("PERSONNEL_SKILL").MapAllProperties();
        }
    }
}
