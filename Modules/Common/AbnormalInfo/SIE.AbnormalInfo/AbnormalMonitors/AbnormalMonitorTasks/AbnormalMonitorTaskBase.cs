using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.AbnormalInfo.AbnormalMonitors
{
    /// <summary>
    /// 异常任务、清单 基类	
    /// </summary>
    [RootEntity, Serializable]
    public class AbnormalMonitorTaskBase : DataEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<AbnormalMonitorTaskBase>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 异常概要 ProblemCondition
        /// <summary>
        /// 异常概要
        /// </summary>
        [Label("异常概要")]
        [MaxLength(1000)]
        public static readonly Property<string> ProblemConditionProperty = P<AbnormalMonitorTaskBase>.Register(e => e.ProblemCondition);

        /// <summary>
        /// 异常概要
        /// </summary>
        public string ProblemCondition
        {
            get { return GetProperty(ProblemConditionProperty); }
            set { SetProperty(ProblemConditionProperty, value); }
        }
        #endregion

        #region 异常描述 ProblemDescription
        /// <summary>
        /// 异常描述
        /// </summary>
        [Label("异常描述")]
        [MaxLength(3000)]
        public static readonly Property<string> ProblemDescriptionProperty = P<AbnormalMonitorTaskBase>.Register(e => e.ProblemDescription);

        /// <summary>
        /// 问题描述
        /// </summary>
        public string ProblemDescription
        {
            get { return GetProperty(ProblemDescriptionProperty); }
            set { SetProperty(ProblemDescriptionProperty, value); }
        }
        #endregion

        #region 异常定义 AbnormalDefine
        /// <summary>
        /// 异常定义Id
        /// </summary>
        [Label("异常定义")]
        public static readonly IRefIdProperty AbnormalDefineIdProperty = P<AbnormalMonitorTaskBase>.RegisterRefId(e => e.AbnormalDefineId, ReferenceType.Normal);

        /// <summary>
        /// 异常定义Id
        /// </summary>
        public double? AbnormalDefineId
        {
            get { return (double?)GetRefNullableId(AbnormalDefineIdProperty); }
            set { SetRefNullableId(AbnormalDefineIdProperty, value); }
        }

        /// <summary>
        /// 异常定义
        /// </summary>
        public static readonly RefEntityProperty<AbnormalDefine> AbnormalDefineProperty = P<AbnormalMonitorTaskBase>.RegisterRef(e => e.AbnormalDefine, AbnormalDefineIdProperty);

        /// <summary>
        /// 异常定义
        /// </summary>
        public AbnormalDefine AbnormalDefine
        {
            get { return GetRefEntity(AbnormalDefineProperty); }
            set { SetRefEntity(AbnormalDefineProperty, value); }
        }
        #endregion

        #region 异常名称 AbnormalName
        /// <summary>
        /// 异常名称
        /// </summary>
        [Label("异常名称")]
        [Required]
        public static readonly Property<string> AbnormalNameProperty = P<AbnormalMonitorTaskBase>.Register(e => e.AbnormalName);

        /// <summary>
        /// 异常名称
        /// </summary>
        [MaxLength(200)]
        public string AbnormalName
        {
            get { return GetProperty(AbnormalNameProperty); }
            set { SetProperty(AbnormalNameProperty, value); }
        }
        #endregion

        #region 异常预警 AbnormalWarnDefine
        /// <summary>
        /// 异常预警Id
        /// </summary>
        [Label("异常预警")]
        public static readonly IRefIdProperty AbnormalWarnDefineIdProperty = P<AbnormalMonitorTaskBase>.RegisterRefId(e => e.AbnormalWarnDefineId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<AbnormalWarnDefine> AbnormalWarnDefineProperty = P<AbnormalMonitorTaskBase>.RegisterRef(e => e.AbnormalWarnDefine, AbnormalWarnDefineIdProperty);

        /// <summary>
        /// 异常预警
        /// </summary>
        public AbnormalWarnDefine AbnormalWarnDefine
        {
            get { return GetRefEntity(AbnormalWarnDefineProperty); }
            set { SetRefEntity(AbnormalWarnDefineProperty, value); }
        }
        #endregion

        #region 来源数据标识 SourceDataKeys
        /// <summary>
        /// 来源数据标识
        /// </summary>
        [Label("来源数据标识")]
        [MaxLength(1000)]
        public static readonly Property<string> SourceDataKeysProperty = P<AbnormalMonitorTaskBase>.Register(e => e.SourceDataKeys);

        /// <summary>
        /// 异常概要
        /// </summary>
        public string SourceDataKeys
        {
            get { return GetProperty(SourceDataKeysProperty); }
            set { SetProperty(SourceDataKeysProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 异常定义名称 AbnormalDefineName
        /// <summary>
        /// 异常定义名称
        /// </summary>
        [Label("异常分组")]
        public static readonly Property<string> AbnormalDefineNameProperty = P<AbnormalMonitorTaskBase>.RegisterView(e => e.AbnormalDefineName, p => p.AbnormalDefine.AbnormalRule.AbnomalSource.MonitorName);

        /// <summary>
        /// 异常定义名称
        /// </summary>
        public string AbnormalDefineName
        {
            get { return this.GetProperty(AbnormalDefineNameProperty); }
        }
        #endregion

        #region 异常预警名称 AbnormalWarnDefineName
        /// <summary>
        /// 异常预警名称
        /// </summary>
        [Label("异常预警")]
        public static readonly Property<string> AbnormalWarnDefineNameProperty = P<AbnormalMonitorTaskBase>.RegisterView(e => e.AbnormalWarnDefineName, p => p.AbnormalWarnDefine.Name);

        /// <summary>
        /// 异常预警名称
        /// </summary>
        public string AbnormalWarnDefineName
        {
            get { return this.GetProperty(AbnormalWarnDefineNameProperty); }
        }
        #endregion

        #region 异常预警编码 AbnormalWarnDefineCode
        /// <summary>
        /// 异常预警编码
        /// </summary>
        [Label("异常预警")]
        public static readonly Property<string> AbnormalWarnDefineCodeProperty = P<AbnormalMonitorTaskBase>.RegisterView(e => e.AbnormalWarnDefineCode, p => p.AbnormalWarnDefine.Code);

        /// <summary>
        /// 异常预警编码
        /// </summary>
        public string AbnormalWarnDefineCode
        {
            get { return this.GetProperty(AbnormalWarnDefineCodeProperty); }
        }
        #endregion

        #endregion
    }
}
