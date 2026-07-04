using SIE.Domain;
using SIE.Equipments.DeviceIOTParas.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Equipments.DeviceIOTParas.Details
{
    /// <summary>
    /// 物联参数
    /// </summary>
    [RootEntity, Serializable]
    //[CriteriaQuery]
    [Label("物联参数")]
    [DisplayMember(nameof(PararCode))]
    public class PhysicalUnion : DataEntity
    {
        #region 设备物联参数 DeviceIOTPara
        /// <summary>
        /// 设备物联参数Id
        /// </summary>
        [Label("设备物联参数")]
        public static readonly IRefIdProperty DeviceIOTParaIdProperty =
            P<PhysicalUnion>.RegisterRefId(e => e.DeviceIOTParaId, ReferenceType.Parent);

        /// <summary>
        /// 设备物联参数Id
        /// </summary>
        public double DeviceIOTParaId
        {
            get { return (double)this.GetRefId(DeviceIOTParaIdProperty); }
            set { this.SetRefId(DeviceIOTParaIdProperty, value); }
        }

        /// <summary>
        /// 设备物联参数
        /// </summary>
        public static readonly RefEntityProperty<DeviceIOTPara> DeviceIOTParaProperty =
            P<PhysicalUnion>.RegisterRef(e => e.DeviceIOTPara, DeviceIOTParaIdProperty);

        /// <summary>
        /// 设备物联参数
        /// </summary>
        public DeviceIOTPara DeviceIOTPara
        {
            get { return this.GetRefEntity(DeviceIOTParaProperty); }
            set { this.SetRefEntity(DeviceIOTParaProperty, value); }
        }
        #endregion

        #region 启用 Enable
        /// <summary>
        /// 启用
        /// </summary>
        [Label("启用")]
        public static readonly Property<bool> EnableProperty = P<PhysicalUnion>.Register(e => e.Enable);

        /// <summary>
        /// 启用
        /// </summary>
        public bool Enable
        {
            get { return this.GetProperty(EnableProperty); }
            set { this.SetProperty(EnableProperty, value); }
        }
        #endregion

        #region MDC变量名 MDCVariableName
        /// <summary>
        /// MDC变量名
        /// </summary>
        [Label("MDC变量名")]
        public static readonly Property<string> MDCVariableNameProperty = P<PhysicalUnion>.Register(e => e.MDCVariableName);

        /// <summary>
        /// MDC变量名
        /// </summary>
        public string MDCVariableName
        {
            get { return GetProperty(MDCVariableNameProperty); }
            set { SetProperty(MDCVariableNameProperty, value); }
        }
        #endregion

        #region 参数编码 PararCode
        /// <summary>
        /// 参数编码
        /// </summary>
        [Label("参数编码")]
        [Required]
        public static readonly Property<string> PararCodeProperty = P<PhysicalUnion>.Register(e => e.PararCode);

        /// <summary>
        /// 参数编码
        /// </summary>
        public string PararCode
        {
            get { return GetProperty(PararCodeProperty); }
            set { SetProperty(PararCodeProperty, value); }
        }
        #endregion

        #region 参数名称 ParaName
        /// <summary>
        /// 参数名称
        /// </summary>
        [Label("参数名称")]
        public static readonly Property<string> ParaNameProperty = P<PhysicalUnion>.Register(e => e.ParaName);

        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParaName
        {
            get { return GetProperty(ParaNameProperty); }
            set { SetProperty(ParaNameProperty, value); }
        }
        #endregion

        #region 初始值 InitialValue
        /// <summary>
        /// 初始值
        /// </summary>
        [Label("初始值")]
        public static readonly Property<double> InitialValueProperty = P<PhysicalUnion>.Register(e => e.InitialValue);

        /// <summary>
        /// 初始值
        /// </summary>
        public double InitialValue
        {
            get { return GetProperty(InitialValueProperty); }
            set { SetProperty(InitialValueProperty, value); }
        }
        #endregion

        #region 最大值 MaxValue
        /// <summary>
        /// 最大值
        /// </summary>
        [Label("最大值")]
        public static readonly Property<double?> MaxValueProperty = P<PhysicalUnion>.Register(e => e.MaxValue);

        /// <summary>
        /// 最大值
        /// </summary>
        public double? MaxValue
        {
            get { return GetProperty(MaxValueProperty); }
            set { SetProperty(MaxValueProperty, value); }
        }
        #endregion

        #region 最小值 MinValue
        /// <summary>
        /// 最小值
        /// </summary>
        [Label("最小值")]
        public static readonly Property<double?> MinValueProperty = P<PhysicalUnion>.Register(e => e.MinValue);

        /// <summary>
        /// 最小值
        /// </summary>
        public double? MinValue
        {
            get { return GetProperty(MinValueProperty); }
            set { SetProperty(MinValueProperty, value); }
        }
        #endregion

        #region 单位 Unit
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitProperty = P<PhysicalUnion>.Register(e => e.Unit);

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit
        {
            get { return GetProperty(UnitProperty); }
            set { SetProperty(UnitProperty, value); }
        }
        #endregion

        #region MDC模型编码 MDCVariableCode
        /// <summary>
        /// MDC模型编码
        /// </summary>
        [Label("MDC模型编码")]
        public static readonly Property<string> MDCVariableCodeProperty = P<PhysicalUnion>.Register(e => e.MDCVariableCode);

        /// <summary>
        /// MDC模型编码
        /// </summary>
        public string MDCVariableCode
        {
            get { return GetProperty(MDCVariableCodeProperty); }
            set { SetProperty(MDCVariableCodeProperty, value); }
        }
        #endregion

        #region 来源 From
        /// <summary>
        /// 来源
        /// </summary>
        [Label("来源")]
        public static readonly Property<FromType> FromProperty = P<PhysicalUnion>.Register(e => e.From);

        /// <summary>
        /// 来源
        /// </summary>
        public FromType From
        {
            get { return this.GetProperty(FromProperty); }
            set { this.SetProperty(FromProperty, value); }
        }
        #endregion

        #region 是否点检 IsCheck
        /// <summary>
        /// 是否点检
        /// </summary>
        [Label("是否点检")]
        public static readonly Property<bool?> IsCheckProperty = P<PhysicalUnion>.Register(e => e.IsCheck);

        /// <summary>
        /// 是否点检
        /// </summary>
        public bool? IsCheck
        {
            get { return this.GetProperty(IsCheckProperty); }
            set { this.SetProperty(IsCheckProperty, value); }
        }
        #endregion

        #region 扩展字段，不映射数据库

        #region 实际值 RealValue
        /// <summary>
        /// 实际值
        /// </summary>
        [Label("实际值")]
        public static readonly Property<string> RealValueProperty = P<PhysicalUnion>.Register(e => e.RealValue);

        /// <summary>
        /// 实际值
        /// </summary>
        public string RealValue
        {
            get { return this.GetProperty(RealValueProperty); }
            set { this.SetProperty(RealValueProperty, value); }
        }
        #endregion

        #region 自动获取时间 AutoGetDate
        /// <summary>
        /// 自动获取时间
        /// </summary>
        [Label("自动获取时间")]
        public static readonly Property<DateTime?> AutoGetDateProperty = P<PhysicalUnion>.Register(e => e.AutoGetDate);

        /// <summary>
        /// 自动获取时间
        /// </summary>
        public DateTime? AutoGetDate
        {
            get { return this.GetProperty(AutoGetDateProperty); }
            set { this.SetProperty(AutoGetDateProperty, value); }
        }
        #endregion


        #endregion
    }

    /// <summary>
    /// 物联参数 实体配置
    /// </summary>
    internal class PhysicalUnionConfig : EntityConfig<PhysicalUnion>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_PHYSICAL_UNION").MapAllProperties();
            Meta.Property(PhysicalUnion.RealValueProperty).DontMapColumn();
            Meta.Property(PhysicalUnion.AutoGetDateProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}
