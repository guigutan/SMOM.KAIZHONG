using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Skills;
using System;

namespace SIE.Tech.Processs
{
    /// <summary>
    /// 工序技能
    /// </summary>
    [ChildEntity, Serializable]
    [Label("工序技能")]
    public partial class ProcessSkill : DataEntity
    {
        #region 技能清单 Skill
        /// <summary>
        /// 技能清单Id
        /// </summary>
        public static readonly IRefIdProperty SkillIdProperty = P<ProcessSkill>.RegisterRefId(e => e.SkillId, ReferenceType.Normal);

        /// <summary>
        /// 技能清单Id
        /// </summary>
        public double SkillId
        {
            get { return (double)GetRefId(SkillIdProperty); }
            set { SetRefId(SkillIdProperty, value); }
        }

        /// <summary>
        /// 技能清单
        /// </summary>
        public static readonly RefEntityProperty<Skill> SkillProperty = P<ProcessSkill>.RegisterRef(e => e.Skill, SkillIdProperty);

        /// <summary>
        /// 技能清单
        /// </summary>
        public Skill Skill
        {
            get { return GetRefEntity(SkillProperty); }
            set { SetRefEntity(SkillProperty, value); }
        }
        #endregion

        #region 工序技能要求 Process
        /// <summary>
        /// 工序技能要求Id
        /// </summary>
        public static readonly IRefIdProperty ProcessIdProperty = P<ProcessSkill>.RegisterRefId(e => e.ProcessId, ReferenceType.Parent);

        /// <summary>
        /// 工序技能要求Id
        /// </summary>
        public double ProcessId
        {
            get { return (double)GetRefId(ProcessIdProperty); }
            set { SetRefId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序技能要求
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty = P<ProcessSkill>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序技能要求
        /// </summary>
        public Process Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 技能清单编码 SkillCode
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> SkillCodeProperty = P<ProcessSkill>.RegisterView(e => e.SkillCode, e => e.Skill.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string SkillCode
        {
            get { return GetProperty(SkillCodeProperty); }
        }
        #endregion

        #region 技能清单名称 SkillName
        /// <summary>
        /// 编码
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> SkillNameProperty = P<ProcessSkill>.RegisterView(e => e.SkillName, e => e.Skill.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string SkillName
        {
            get { return GetProperty(SkillNameProperty); }
        }
        #endregion

        #region 技能清单技能分类 SkillCategory
        /// <summary>
        /// 技能分类
        /// </summary>
        [Label("技能分类")]
        public static readonly Property<string> SkillCategoryProperty = P<ProcessSkill>.RegisterView(e => e.SkillCategory, e => e.Skill.Category.Name);

        /// <summary>
        /// 技能分类
        /// </summary>
        public string SkillCategory
        {
            get { return GetProperty(SkillCategoryProperty); }
        }
        #endregion

        #region 技能清单描述 SkillDesc
        /// <summary>
        /// 技能描述
        /// </summary>
        [Label("技能描述")]
        public static readonly Property<string> SkillDescProperty = P<ProcessSkill>.RegisterView(e => e.SkillDesc, e => e.Skill.Remark);

        /// <summary>
        /// 技能描述
        /// </summary>
        public string SkillDesc
        {
            get { return GetProperty(SkillDescProperty); }
        }
        #endregion

        #region 是否验证 IsCheck
        /// <summary>
        /// 是否验证
        /// </summary>
        [Label("是否验证")]
        public static readonly Property<bool> IsCheckProperty = P<ProcessSkill>.Register(e => e.IsCheck);

        /// <summary>
        /// 是否验证
        /// </summary>
        public bool IsCheck
        {
            get { return this.GetProperty(IsCheckProperty); }
            set { this.SetProperty(IsCheckProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 工序技能 实体配置
    /// </summary>
    internal class ProcessSkillConfig : EntityConfig<ProcessSkill>
    {
        /// <summary>
        /// 实体元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TECH_PROCESS_SKILL").MapAllProperties();
            Meta.Property(ProcessSkill.ProcessIdProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}