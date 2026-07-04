using SIE.Common.Catalogs;
using SIE.Domain;
using SIE.Equipments.Abnormal;
using SIE.MES.WorkBench.AlertLights;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.WipResources;
using SIE.Tech.Stations;
using System;

namespace SIE.MES.Workbench.AlertLights
{
    /// <summary>
    /// 按灯
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("按灯")]
    public partial class AlertLight : DataEntity
    {
        #region 触发时间 TriggerTime
        /// <summary>
        /// 触发时间
        /// </summary>
        [Required]
        [Label("触发时间")]
        public static readonly Property<DateTime?> TriggerTimeProperty = P<AlertLight>.Register(e => e.TriggerTime);

        /// <summary>
        /// 触发时间
        /// </summary>
        public DateTime? TriggerTime
        {
            get { return GetProperty(TriggerTimeProperty); }
            set { SetProperty(TriggerTimeProperty, value); }
        }
        #endregion

        #region 恢复时间 RestoreTime
        /// <summary>
        /// 恢复时间
        /// </summary>
        [Label("恢复时间")]
        public static readonly Property<DateTime?> RestoreTimeProperty = P<AlertLight>.Register(e => e.RestoreTime);

        /// <summary>
        /// 恢复时间
        /// </summary>
        public DateTime? RestoreTime
        {
            get { return GetProperty(RestoreTimeProperty); }
            set { SetProperty(RestoreTimeProperty, value); }
        }
        #endregion

        #region 呼叫类型 AlertType
        /// <summary>
        /// 呼叫类型
        /// </summary>
        [Label("呼叫类型")]
        public static readonly Property<AlertCallType> AlertTypeProperty = P<AlertLight>.Register(e => e.AlertType);

        /// <summary>
        /// 呼叫类型
        /// </summary>
        public AlertCallType AlertType
        {
            get { return GetProperty(AlertTypeProperty); }
            set { SetProperty(AlertTypeProperty, value); }
        }
        #endregion

        #region 异常类型 ExceptionType
        /// <summary>
        /// 异常类型Id
        /// </summary>
        [Label("异常类型")]
        public static readonly IRefIdProperty ExceptionTypeIdProperty =
            P<AlertLight>.RegisterRefId(e => e.ExceptionTypeId, ReferenceType.Normal);

        /// <summary>
        /// 异常类型Id
        /// </summary>
        public double ExceptionTypeId
        {
            get { return (double)this.GetRefId(ExceptionTypeIdProperty); }
            set { this.SetRefId(ExceptionTypeIdProperty, value); }
        }

        /// <summary>
        /// 异常类型
        /// </summary>
        public static readonly RefEntityProperty<Catalog> ExceptionTypeProperty =
            P<AlertLight>.RegisterRef(e => e.ExceptionType, ExceptionTypeIdProperty);

        /// <summary>
        /// 异常类型
        /// </summary>
        public Catalog ExceptionType
        {
            get { return this.GetRefEntity(ExceptionTypeProperty); }
            set { this.SetRefEntity(ExceptionTypeProperty, value); }
        }
        #endregion

        #region 异常类型名称 ExpTypeName
        /// <summary>
        /// 异常类型名称
        /// </summary>
        [Label("异常类型名称")]
        public static readonly Property<string> ExpTypeNameProperty = P<AlertLight>.Register(e => e.ExpTypeName);

        /// <summary>
        /// 异常类型名称
        /// </summary>
        public string ExpTypeName
        {
            get { return this.GetProperty(ExpTypeNameProperty); }
            set { this.SetProperty(ExpTypeNameProperty, value); }
        }
        #endregion

        #region 工位 Station
        /// <summary>
        /// 工位Id
        /// </summary>
        [Label("工位")]
        public static readonly IRefIdProperty StationIdProperty = P<AlertLight>.RegisterRefId(e => e.StationId, ReferenceType.Normal);

        /// <summary>
        /// 工位Id
        /// </summary>
        public double StationId
        {
            get { return (double)GetRefId(StationIdProperty); }
            set { SetRefId(StationIdProperty, value); }
        }

        /// <summary>
        /// 工位
        /// </summary>
        public static readonly RefEntityProperty<Station> StationProperty = P<AlertLight>.RegisterRef(e => e.Station, StationIdProperty);

        /// <summary>
        /// 工位
        /// </summary>
        public Station Station
        {
            get { return GetRefEntity(StationProperty); }
            set { SetRefEntity(StationProperty, value); }
        }
        #endregion

        #region 呼叫人员 CallEmployee
        /// <summary>
        /// 员工Id
        /// </summary>
        [Label("呼叫人员")]
        [Required]
        public static readonly IRefIdProperty CallEmployeeIdProperty = P<AlertLight>.RegisterRefId(e => e.CallEmployeeId, ReferenceType.Normal);

        /// <summary>
        /// 呼叫人员Id
        /// </summary>
        public double CallEmployeeId
        {
            get { return (double)GetRefId(CallEmployeeIdProperty); }
            set { SetRefId(CallEmployeeIdProperty, value); }
        }

        /// <summary>
        /// 呼叫人员
        /// </summary>
        public static readonly RefEntityProperty<Employee> CallEmployeeProperty = P<AlertLight>.RegisterRef(e => e.CallEmployee, CallEmployeeIdProperty);

        /// <summary>
        /// 呼叫人员
        /// </summary>
        public Employee CallEmployee
        {
            get { return GetRefEntity(CallEmployeeProperty); }
            set { SetRefEntity(CallEmployeeProperty, value); }
        }
        #endregion

        #region 实际处理人员 ProcessEmployee
        /// <summary>
        /// 实际处理人员Id
        /// </summary>
        [Label("实际处理人员")]
        public static readonly IRefIdProperty ProcessEmployeeIdProperty = P<AlertLight>.RegisterRefId(e => e.ProcessEmployeeId, ReferenceType.Normal);

        /// <summary>
        /// 实际处理人员Id
        /// </summary>
        public double? ProcessEmployeeId
        {
            get { return (double?)GetRefNullableId(ProcessEmployeeIdProperty); }
            set { SetRefNullableId(ProcessEmployeeIdProperty, value); }
        }

        /// <summary>
        /// 实际处理人员
        /// </summary>
        public static readonly RefEntityProperty<Employee> ProcessEmployeeProperty = P<AlertLight>.RegisterRef(e => e.ProcessEmployee, ProcessEmployeeIdProperty);

        /// <summary>
        /// 实际处理人员
        /// </summary>
        public Employee ProcessEmployee
        {
            get { return GetRefEntity(ProcessEmployeeProperty); }
            set { SetRefEntity(ProcessEmployeeProperty, value); }
        }
        #endregion

        #region 产线 ProductLine
        /// <summary>
        /// 产线
        /// </summary>
        [Label("产线")]
        public static readonly IRefIdProperty ProductLineIdProperty = P<AlertLight>.RegisterRefId(e => e.ProductLineId, ReferenceType.Normal);

        /// <summary>
        /// 产线Id
        /// </summary>
        public double ProductLineId
        {
            get { return (double)GetRefId(ProductLineIdProperty); }
            set { SetRefId(ProductLineIdProperty, value); }
        }

        /// <summary>
        /// 产线
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ProductLineProperty = P<AlertLight>.RegisterRef(e => e.ProductLine, ProductLineIdProperty);

        /// <summary>
        /// 产线
        /// </summary>
        public WipResource ProductLine
        {
            get { return GetRefEntity(ProductLineProperty); }
            set { SetRefEntity(ProductLineProperty, value); }
        }
        #endregion

        #region 处理状态 ProcessStatus
        /// <summary>
        /// 处理状态
        /// </summary>
        [Label("处理状态")]
        public static readonly Property<ProcessStatusType?> ProcessStatusProperty = P<AlertLight>.Register(e => e.ProcessStatus);

        /// <summary>
        /// 处理状态
        /// </summary>
        public ProcessStatusType? ProcessStatus
        {
            get { return GetProperty(ProcessStatusProperty); }
            set { SetProperty(ProcessStatusProperty, value); }
        }
        #endregion

        #region 签到时间 SignTime
        /// <summary>
        /// 签到时间
        /// </summary>
        [Label("签到时间")]
        public static readonly Property<DateTime?> SignTimeProperty = P<AlertLight>.Register(e => e.SignTime);

        /// <summary>
        /// 签到时间
        /// </summary>
        public DateTime? SignTime
        {
            get { return GetProperty(SignTimeProperty); }
            set { SetProperty(SignTimeProperty, value); }
        }
        #endregion

        #region 接收人员列表 AlertLightHanders
        /// <summary>
        /// 接收人员列表
        /// </summary>
        public static readonly ListProperty<EntityList<AlertLightHandler>> AlertLightHandersProperty = P<AlertLight>.RegisterList(e => e.AlertLightHanders);

        /// <summary>
        /// 接收人员列表
        /// </summary>
        public EntityList<AlertLightHandler> AlertLightHanders
        {
            get { return this.GetLazyList(AlertLightHandersProperty); }
        }
        #endregion
    }

    /// <summary>
    /// 按灯 实体配置
    /// </summary>
    internal class AlertLightConfig : EntityConfig<AlertLight>
    {
        /// <summary>
        /// 安灯呼叫的实体配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("WB_ALERT_LIGHT").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}