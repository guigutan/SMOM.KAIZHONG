using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Maintains.Plans
{
    /// <summary>
    /// 保养评分
    /// </summary>
    [RootEntity, Serializable]
    [Label("保养评分")]
    public partial class MaintainScore : DataEntity
    {
        #region 所属保养计划 MaintainPlan
        /// <summary>
        /// 所属保养计划Id
        /// </summary>
        [Label("所属保养计划")]
        public static readonly IRefIdProperty MaintainPlanIdProperty = P<MaintainScore>.RegisterRefId(e => e.MaintainPlanId, ReferenceType.Parent);

        /// <summary>
        /// 所属保养计划Id
        /// </summary>
        public double MaintainPlanId
        {
            get { return (double)GetRefId(MaintainPlanIdProperty); }
            set { SetRefId(MaintainPlanIdProperty, value); }
        }

        /// <summary>
        /// 所属保养计划
        /// </summary>
        public static readonly RefEntityProperty<MaintainPlan> MaintainPlanProperty = P<MaintainScore>.RegisterRef(e => e.MaintainPlan, MaintainPlanIdProperty);

        /// <summary>
        /// 所属保养计划
        /// </summary>
        public MaintainPlan MaintainPlan
        {
            get { return GetRefEntity(MaintainPlanProperty); }
            set { SetRefEntity(MaintainPlanProperty, value); }
        }
        #endregion

        #region 项目名称 ProjectName
        /// <summary>
        /// 项目名称
        /// </summary>
        [Required]
        [Label("项目名称")]
        public static readonly Property<string> ProjectNameProperty = P<MaintainScore>.Register(e => e.ProjectName);

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName
        {
            get { return GetProperty(ProjectNameProperty); }
            set { SetProperty(ProjectNameProperty, value); }
        }
        #endregion

        #region 检查标准 CheckStandard
        /// <summary>
        /// 检查标准
        /// </summary>
        [Required]
        [Label("检查标准")]
        public static readonly Property<string> CheckStandardProperty = P<MaintainScore>.Register(e => e.CheckStandard);

        /// <summary>
        /// 检查标准
        /// </summary>
        public string CheckStandard
        {
            get { return GetProperty(CheckStandardProperty); }
            set { SetProperty(CheckStandardProperty, value); }
        }
        #endregion

        #region 分值比（%） Rate
        /// <summary>
        /// 分值比（%）
        /// </summary>
        [Required]
        [MinValue(0), MaxValue(100)]
        [Label("分值比（%）")]
        public static readonly Property<decimal> RateProperty = P<MaintainScore>.Register(e => e.Rate);

        /// <summary>
        /// 分值比（%）
        /// </summary>
        public decimal Rate
        {
            get { return GetProperty(RateProperty); }
            set { SetProperty(RateProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(2000)]
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<MaintainScore>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 得分 Score
        /// <summary>
        /// 得分
        /// </summary>
        [Label("得分")]
        public static readonly Property<double> ScoreProperty = P<MaintainScore>.Register(e => e.Score);

        /// <summary>
        /// 得分
        /// </summary>
        public double Score
        {
            get { return GetProperty(ScoreProperty); }
            set { SetProperty(ScoreProperty, value); }
        }
        #endregion

        #region 存在问题 ExistProblem
        /// <summary>
        /// 存在问题ExistProblem
        /// </summary>
        [MaxLength(1000)]
        [Label("存在问题")]
        public static readonly Property<string> ExistProblemProperty = P<MaintainScore>.Register(e => e.ExistProblem);

        /// <summary>
        /// 存在问题ExistProblem
        /// </summary>
        public string ExistProblem
        {
            get { return GetProperty(ExistProblemProperty); }
            set { SetProperty(ExistProblemProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 保养评分项维护 实体配置
    /// </summary>
    internal class MaintainScoreConfig : EntityConfig<MaintainScore>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_MAINTAIN_SCORE").MapAllProperties();
            Meta.Property(MaintainScore.CheckStandardProperty).ColumnMeta.HasLength(2000);
            Meta.Property(MaintainScore.RemarkProperty).ColumnMeta.HasLength(4000);

            Meta.EnablePhantoms();
        }
    }
}