using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Skills;
using System;

namespace SIE.MES.TeamManagement.SikllAuthentications
{
    /// <summary>
	/// 员工技能认证管理
	/// </summary>
	[RootEntity, Serializable]
    [ConditionQueryType(typeof(SkillAuthenticationCriteria))]
    [Label("员工技能认证管理")]
    public partial class SkillAuthentication : DataEntity
    {
        #region 技能分类 SkillCategory
        /// <summary>
        /// 技能分类Id
        /// </summary>
        [Label("技能分类")]
        public static readonly IRefIdProperty SkillCategoryIdProperty =
            P<SkillAuthentication>.RegisterRefId(e => e.SkillCategoryId, ReferenceType.Normal);

        /// <summary>
        /// 技能分类Id
        /// </summary>
        public double SkillCategoryId
        {
            get { return (double)this.GetRefId(SkillCategoryIdProperty); }
            set { this.SetRefId(SkillCategoryIdProperty, value); }
        }

        /// <summary>
        /// 技能分类
        /// </summary>
        public static readonly RefEntityProperty<SkillCategory> SkillCategoryProperty =
            P<SkillAuthentication>.RegisterRef(e => e.SkillCategory, SkillCategoryIdProperty);

        /// <summary>
        /// 技能分类
        /// </summary>
        public SkillCategory SkillCategory
        {
            get { return this.GetRefEntity(SkillCategoryProperty); }
            set { this.SetRefEntity(SkillCategoryProperty, value); }
        }
        #endregion 

        #region 技能清单 Skill
        /// <summary>
        /// 技能清单Id
        /// </summary>
        [Label("技能")]
        public static readonly IRefIdProperty SkillIdProperty = P<SkillAuthentication>.RegisterRefId(e => e.SkillId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Skill> SkillProperty = P<SkillAuthentication>.RegisterRef(e => e.Skill, SkillIdProperty);

        /// <summary>
        /// 技能清单
        /// </summary>
        public Skill Skill
        {
            get { return GetRefEntity(SkillProperty); }
            set { SetRefEntity(SkillProperty, value); }
        }
        #endregion

        #region 培训要求 TrainingRequired
        /// <summary>
        /// 培训要求
        /// </summary>
        [Label("培训要求")]
        [Required]
        public static readonly Property<TrainingRequired> TrainingRequiredProperty = P<SkillAuthentication>.Register(e => e.TrainingRequired);

        /// <summary>
        /// 培训要求
        /// </summary>
        public TrainingRequired TrainingRequired
        {
            get { return GetProperty(TrainingRequiredProperty); }
            set { SetProperty(TrainingRequiredProperty, value); }
        }
        #endregion

        #region 考试要求 ExamRequired
        /// <summary>
        /// 考试要求
        /// </summary>
        [Label("考试要求")]
        [Required]
        public static readonly Property<ExamRequired> ExamRequiredProperty = P<SkillAuthentication>.Register(e => e.ExamRequired);

        /// <summary>
        /// 考试要求
        /// </summary>
        public ExamRequired ExamRequired
        {
            get { return GetProperty(ExamRequiredProperty); }
            set { SetProperty(ExamRequiredProperty, value); }
        }
        #endregion

        #region 实操要求 OperationRequired
        /// <summary>
        /// 实操要求
        /// </summary>
        [Label("实操要求")]
        [Required]
        public static readonly Property<OperationRequired> OperationRequiredProperty = P<SkillAuthentication>.Register(e => e.OperationRequired);

        /// <summary>
        /// 实操要求
        /// </summary>
        public OperationRequired OperationRequired
        {
            get { return GetProperty(OperationRequiredProperty); }
            set { SetProperty(OperationRequiredProperty, value); }
        }
        #endregion

        #region 实操记录列表 OperationList
        /// <summary>
        /// 实操记录列表
        /// </summary>
        public static readonly ListProperty<EntityList<OperationRecord>> OperationListProperty = P<SkillAuthentication>.RegisterList(e => e.OperationList);

        /// <summary>
        /// 实操记录列表
        /// </summary>
        public EntityList<OperationRecord> OperationList
        {
            get { return this.GetLazyList(OperationListProperty); }
        }
        #endregion

        #region 考试结果列表 ExamList
        /// <summary>
        /// 考试结果列表
        /// </summary>
        public static readonly ListProperty<EntityList<ExamResult>> ExamListProperty = P<SkillAuthentication>.RegisterList(e => e.ExamList);

        /// <summary>
        /// 考试结果列表
        /// </summary>
        public EntityList<ExamResult> ExamList
        {
            get { return this.GetLazyList(ExamListProperty); }
        }
        #endregion

        #region 培训记录列表 TrainList
        /// <summary>
        /// 培训记录列表
        /// </summary>
        public static readonly ListProperty<EntityList<TrainingRecord>> TrainListProperty = P<SkillAuthentication>.RegisterList(e => e.TrainList);

        /// <summary>
        /// 培训记录列表
        /// </summary>
        public EntityList<TrainingRecord> TrainList
        {
            get { return this.GetLazyList(TrainListProperty); }
        }
        #endregion

        #region 注册视图属性(关联实体属性平铺显示，一般用于Web)
        #region 技能编码 SkillCode
        /// <summary>
        /// 技能编码
        /// </summary>
        [Label("技能编码")]
        public static readonly Property<string> SkillCodeProperty = P<SkillAuthentication>.RegisterView(e => e.SkillCode, p => p.Skill.Code);

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
        public static readonly Property<string> SkillNameProperty = P<SkillAuthentication>.RegisterView(e => e.SkillName, p => p.Skill.Name);

        /// <summary>
        /// 技能名称
        /// </summary>
        public string SkillName
        {
            get { return this.GetProperty(SkillNameProperty); }
        }
        #endregion

        #region 技能分类名称 SkillCategoryName
        /// <summary>
        /// 技能名称
        /// </summary>
        [Label("技能分类")]
        public static readonly Property<string> SkillCategoryNameProperty = P<SkillAuthentication>.RegisterView(e => e.SkillCategoryName, p => p.SkillCategory.Name);

        /// <summary>
        /// 技能分类
        /// </summary>
        public string SkillCategoryName
        {
            get { return this.GetProperty(SkillCategoryNameProperty); }
        }
        #endregion

        #region 技能备注 SkillRemark
        /// <summary>
        /// 技能备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> SkillRemarkProperty = P<SkillAuthentication>.RegisterView(e => e.SkillRemark, p => p.Skill.Remark);

        /// <summary>
        /// 技能描述
        /// </summary>
        public string SkillRemark
        {
            get { return this.GetProperty(SkillRemarkProperty); }
        }
        #endregion

        #region 有效期(天) SkillValidity
        /// <summary>
        /// 有效期(天)
        /// </summary>
        [Label("有效期(天)")]
        public static readonly Property<int?> SkillValidityProperty = P<SkillAuthentication>.RegisterView(e => e.SkillValidity, p => p.Skill.Validity);

        /// <summary>
        /// 有效期(天)
        /// </summary>
        public int? SkillValidity
        {
            get { return this.GetProperty(SkillValidityProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 员工技能认证管理 实体配置
    /// </summary>
    internal class SkillAuthenticationConfig : EntityConfig<SkillAuthentication>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WG_SKILL_AUTH").MapAllProperties();
            Meta.Property(SkillAuthentication.SkillIdProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}