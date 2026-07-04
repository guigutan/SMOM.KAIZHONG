using SIE.AbnormalInfo.Common;
using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Common.Schdules;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.AbnormalInfo.AbnormalMonitors
{
    /// <summary>
    /// 异常定义
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(AbnormalDefineCriteria))]
    [EntityWithConfig(typeof(NoConfig), "异常定义编码配置项", "异常定义编码配置规则")]
    [Label("异常定义")]
    [DisplayMember(nameof(Code))]
    public partial class AbnormalDefine : DataEntity, IStateEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<AbnormalDefine>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 调度监控 JobConfig
        /// <summary>
        /// 调度监控Id
        /// </summary>
        [Label("调度监控")]
        [Required]
        public static readonly IRefIdProperty JobConfigIdProperty = P<AbnormalDefine>.RegisterRefId(e => e.JobConfigId, ReferenceType.Normal);

        /// <summary>
        /// 调度监控Id
        /// </summary>
        public double? JobConfigId
        {
            get { return (double?)GetRefNullableId(JobConfigIdProperty); }
            set { SetRefNullableId(JobConfigIdProperty, value); }
        }

        /// <summary>
        /// 调度监控
        /// </summary>
        public static readonly RefEntityProperty<JobConfig> JobConfigProperty = P<AbnormalDefine>.RegisterRef(e => e.JobConfig, JobConfigIdProperty);

        /// <summary>
        /// 调度监控
        /// </summary>
        public JobConfig JobConfig
        {
            get { return GetRefEntity(JobConfigProperty); }
            set { SetRefEntity(JobConfigProperty, value); }
        }
        #endregion

        #region 异常履历 ResumeList
        /// <summary>
        /// 异常履历
        /// </summary>
        public static readonly ListProperty<EntityList<AbnormalResume>> ResumeListProperty = P<AbnormalDefine>.RegisterList(e => e.ResumeList);
        /// <summary>
        /// 异常履历
        /// </summary>
        public EntityList<AbnormalResume> ResumeList
        {
            get { return this.GetLazyList(ResumeListProperty); }
        }
        #endregion

        #region 异常规则 AbnormalRule
        /// <summary>
        /// 异常规则Id
        /// </summary>
        [Label("异常规则")]
        [Required]
        public static readonly IRefIdProperty AbnormalRuleIdProperty = P<AbnormalDefine>.RegisterRefId(e => e.AbnormalRuleId, ReferenceType.Normal);

        /// <summary>
        /// 异常规则Id
        /// </summary>
        public double? AbnormalRuleId
        {
            get { return (double?)GetRefNullableId(AbnormalRuleIdProperty); }
            set { SetRefNullableId(AbnormalRuleIdProperty, value); }
        }

        /// <summary>
        /// 异常规则
        /// </summary>
        public static readonly RefEntityProperty<AbnormalDecisionRule> AbnormalRuleProperty = P<AbnormalDefine>.RegisterRef(e => e.AbnormalRule, AbnormalRuleIdProperty);

        /// <summary>
        /// 异常规则
        /// </summary>
        public AbnormalDecisionRule AbnormalRule
        {
            get { return GetRefEntity(AbnormalRuleProperty); }
            set { SetRefEntity(AbnormalRuleProperty, value); }
        }
        #endregion

        #region 异常预警 AbnormalWarnDefine
        /// <summary>
        /// 异常预警Id
        /// </summary>
        [Label("异常预警")]
        [Required]
        public static readonly IRefIdProperty AbnormalWarnDefineIdProperty = P<AbnormalDefine>.RegisterRefId(e => e.AbnormalWarnDefineId, ReferenceType.Normal);

        /// <summary>
        /// 异常预警Id
        /// </summary>
        public double? AbnormalWarnDefineId
        {
            get { return (double?)GetRefNullableId(AbnormalWarnDefineIdProperty); }
            set { SetRefNullableId(AbnormalWarnDefineIdProperty, value); }
        }

        /// <summary>
        /// 异常预警
        /// </summary>
        public static readonly RefEntityProperty<AbnormalWarnDefine> AbnormalWarnDefineProperty = P<AbnormalDefine>.RegisterRef(e => e.AbnormalWarnDefine, AbnormalWarnDefineIdProperty);

        /// <summary>
        /// 异常预警
        /// </summary>
        public AbnormalWarnDefine AbnormalWarnDefine
        {
            get { return GetRefEntity(AbnormalWarnDefineProperty); }
            set { SetRefEntity(AbnormalWarnDefineProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<State> StateProperty = P<AbnormalDefine>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 调度状态 JobConfigState
        /// <summary>
        /// 调度状态
        /// </summary>
        [Label("调度状态")]
        public static readonly Property<State> JobConfigStateProperty = P<AbnormalDefine>.RegisterView(e => e.JobConfigState, p => p.JobConfig.State);

        /// <summary>
        /// 调度状态
        /// </summary>
        public State JobConfigState
        {
            get { return this.GetProperty(JobConfigStateProperty); }
        }
        #endregion

        #region 异常类型 AbnormalType
        /// <summary>
        /// 异常类型
        /// </summary>
        [Label("异常类型")]
        public static readonly Property<AbnomalType> AbnormalTypeProperty = P<AbnormalDefine>.RegisterView(e => e.AbnormalType, p => p.AbnormalRule.AbnormalType);

        /// <summary>
        /// 异常类型
        /// </summary>
        public AbnomalType AbnormalType
        {
            get { return this.GetProperty(AbnormalTypeProperty); }
        }
        #endregion

        #region 规则名称 RuleName
        /// <summary>
        /// 规则名称
        /// </summary>
        [Label("规则名称")]
        public static readonly Property<string> RuleNameProperty = P<AbnormalDefine>.RegisterView(e => e.RuleName, p => p.AbnormalRule.RuleName);

        /// <summary>
        /// 规则名称
        /// </summary>
        public string RuleName
        {
            get { return this.GetProperty(RuleNameProperty); }
        }
        #endregion

        #region 监控名称 MonitorName
        /// <summary>
        /// 监控名称
        /// </summary>
        [Label("监控名称")]
        public static readonly Property<string> MonitorNameProperty = P<AbnormalDefine>.RegisterView(e => e.MonitorName, p => p.AbnormalRule.AbnomalSource.MonitorName);

        /// <summary>
        /// 监控名称
        /// </summary>
        public string MonitorName
        {
            get { return this.GetProperty(MonitorNameProperty); }
        }
        #endregion

        #region 预警定义名称 AbnormalWarnDefineName
        /// <summary>
        /// 预警名称
        /// </summary>
        [Label("预警名称")]
        public static readonly Property<string> AbnormalWarnDefineNameProperty = P<AbnormalDefine>.RegisterView(e => e.AbnormalWarnDefineName, p => p.AbnormalWarnDefine.Name);

        /// <summary>
        /// 预警名称
        /// </summary>
        public string AbnormalWarnDefineName
        {
            get { return this.GetProperty(AbnormalWarnDefineNameProperty); }
        }
        #endregion

        #region 预警定义编码 AbnormalWarnDefineCode
        /// <summary>
        /// 预警定义编码
        /// </summary>
        [Label("异常预警")]
        public static readonly Property<string> AbnormalWarnDefineCodeProperty = P<AbnormalDefine>.RegisterView(e => e.AbnormalWarnDefineCode, p => p.AbnormalWarnDefine.Code);

        /// <summary>
        /// 预警定义编码
        /// </summary>
        public string AbnormalWarnDefineCode
        {
            get { return this.GetProperty(AbnormalWarnDefineCodeProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 异常定义 实体配置
    /// </summary>
    internal class AbnomalDefineConfig : EntityConfig<AbnormalDefine>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ABNORMAL_DEFINE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}