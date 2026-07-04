using System;
using System.Collections.Generic;
using SIE.Domain;
using SIE.ObjectModel;

namespace SIE.AbnormalInfo.AbnormalMonitors
{
    /// <summary>
    /// 异常定义查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("异常定义查询实体")]
    public partial class AbnormalDefineCriteria : Criteria
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<AbnormalDefineCriteria>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 异常来源 AbnomalSource
        /// <summary>
        /// 异常来源Id
        /// </summary>
        [Label("异常来源")]
        public static readonly IRefIdProperty AbnomalSourceIdProperty = P<AbnormalDefineCriteria>.RegisterRefId(e => e.AbnomalSourceId, ReferenceType.Normal);

        /// <summary>
        /// 异常来源Id
        /// </summary>
        public double? AbnomalSourceId
        {
            get { return (double?)GetRefNullableId(AbnomalSourceIdProperty); }
            set { SetRefNullableId(AbnomalSourceIdProperty, value); }
        }

        /// <summary>
        /// 异常来源
        /// </summary>
        public static readonly RefEntityProperty<AbnormalSource> AbnomalSourceProperty = P<AbnormalDefineCriteria>.RegisterRef(e => e.AbnomalSource, AbnomalSourceIdProperty);

        /// <summary>
        /// 异常来源
        /// </summary>
        public AbnormalSource AbnomalSource
        {
            get { return GetRefEntity(AbnomalSourceProperty); }
            set { SetRefEntity(AbnomalSourceProperty, value); }
        }
        #endregion

        #region 异常规则 AbnormalRule
        /// <summary>
        /// 异常规则Id
        /// </summary>
        [Label("异常规则")]
        public static readonly IRefIdProperty AbnormalRuleIdProperty = P<AbnormalDefineCriteria>.RegisterRefId(e => e.AbnormalRuleId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<AbnormalDecisionRule> AbnormalRuleProperty = P<AbnormalDefineCriteria>.RegisterRef(e => e.AbnormalRule, AbnormalRuleIdProperty);

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
        public static readonly IRefIdProperty AbnormalWarnDefineIdProperty = P<AbnormalDefineCriteria>.RegisterRefId(e => e.AbnormalWarnDefineId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<AbnormalWarnDefine> AbnormalWarnDefineProperty = P<AbnormalDefineCriteria>.RegisterRef(e => e.AbnormalWarnDefine, AbnormalWarnDefineIdProperty);

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
        public static readonly Property<State?> StateProperty = P<AbnormalDefineCriteria>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State? State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 创建时间 CreateDate
        /// <summary>
        /// 创建时间
        /// </summary>
        [Label("创建时间")]
        public static readonly Property<DateRange> CreateDateProperty = P<AbnormalDefineCriteria>.Register(e => e.CreateDate);

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateRange CreateDate
        {
            get { return GetProperty(CreateDateProperty); }
            set { SetProperty(CreateDateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询异常定义
        /// </summary>
        /// <returns>来料检验单列表</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<AbnormalDefineService>().GetAbnormalDefines(this);
        }
    }
}
