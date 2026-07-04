using SIE.Domain;
using SIE.EMS.MainenanceProjects;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.MeteringEquipment.Calibrations
{
    /// <summary>
    /// 检验项目
    /// </summary>
    [ChildEntity, Serializable]
    [Label("检验项目")]
    [DisplayMember(nameof(Name))]
    public partial class CalibrationItem : DataEntity
    {
        #region 项目名称 Name
        /// <summary>
        /// 项目名称
        /// </summary>
        [Label("项目名称")]
        public static readonly Property<string> NameProperty = P<CalibrationItem>.Register(e => e.Name);

        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 部位 Part
        /// <summary>
        /// 部位
        /// </summary>
        [Label("部位")]
        public static readonly Property<string> PartProperty = P<CalibrationItem>.Register(e => e.Part);

        /// <summary>
        /// 部位
        /// </summary>
        public string Part
        {
            get { return GetProperty(PartProperty); }
            set { SetProperty(PartProperty, value); }
        }
        #endregion

        #region 项目耗材 Consumable
        /// <summary>
        /// 项目耗材
        /// </summary>
        [Label("项目耗材")]
        public static readonly Property<string> ConsumableProperty = P<CalibrationItem>.Register(e => e.Consumable);

        /// <summary>
        /// 项目耗材
        /// </summary>
        public string Consumable
        {
            get { return GetProperty(ConsumableProperty); }
            set { SetProperty(ConsumableProperty, value); }
        }
        #endregion

        #region 操作方法 Method
        /// <summary>
        /// 操作方法
        /// </summary>
        [Label("操作方法")]
        public static readonly Property<string> MethodProperty = P<CalibrationItem>.Register(e => e.Method);

        /// <summary>
        /// 操作方法
        /// </summary>
        public string Method
        {
            get { return GetProperty(MethodProperty); }
            set { SetProperty(MethodProperty, value); }
        }
        #endregion

        #region 标准 Standard
        /// <summary>
        /// 标准
        /// </summary>
        [Label("标准")]
        public static readonly Property<string> StandardProperty = P<CalibrationItem>.Register(e => e.Standard);

        /// <summary>
        /// 标准
        /// </summary>
        public string Standard
        {
            get { return GetProperty(StandardProperty); }
            set { SetProperty(StandardProperty, value); }
        }
        #endregion

        #region 最小值 MinValue
        /// <summary>
        /// 最小值
        /// </summary>
        [Label("最小值")]
        public static readonly Property<decimal?> MinValueProperty = P<CalibrationItem>.Register(e => e.MinValue);

        /// <summary>
        /// 最小值
        /// </summary>
        public decimal? MinValue
        {
            get { return GetProperty(MinValueProperty); }
            set { SetProperty(MinValueProperty, value); }
        }
        #endregion

        #region 最大值 MaxValue
        /// <summary>
        /// 最大值
        /// </summary>
        [Label("最大值")]
        public static readonly Property<decimal?> MaxValueProperty = P<CalibrationItem>.Register(e => e.MaxValue);

        /// <summary>
        /// 最大值
        /// </summary>
        public decimal? MaxValue
        {
            get { return GetProperty(MaxValueProperty); }
            set { SetProperty(MaxValueProperty, value); }
        }
        #endregion

        #region 单位 Unit
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitProperty = P<CalibrationItem>.Register(e => e.Unit);

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit
        {
            get { return GetProperty(UnitProperty); }
            set { SetProperty(UnitProperty, value); }
        }
        #endregion

        #region 用时 UseTime
        /// <summary>
        /// 用时
        /// </summary>
        [Label("用时")]
        public static readonly Property<string> UseTimeProperty = P<CalibrationItem>.Register(e => e.UseTime);

        /// <summary>
        /// 用时
        /// </summary>
        public string UseTime
        {
            get { return GetProperty(UseTimeProperty); }
            set { SetProperty(UseTimeProperty, value); }
        }
        #endregion

        #region 点检保养项目 ProjectDetail
        /// <summary>
        /// 点检保养项目Id
        /// </summary>
        public static readonly IRefIdProperty ProjectDetailIdProperty = P<CalibrationItem>.RegisterRefId(e => e.ProjectDetailId, ReferenceType.Normal);

        /// <summary>
        /// 点检保养项目Id
        /// </summary>
        public double ProjectDetailId
        {
            get { return (double)GetRefId(ProjectDetailIdProperty); }
            set { SetRefId(ProjectDetailIdProperty, value); }
        }

        /// <summary>
        /// 点检保养项目
        /// </summary>
        public static readonly RefEntityProperty<ProjectDetail> ProjectDetailProperty = P<CalibrationItem>.RegisterRef(e => e.ProjectDetail, ProjectDetailIdProperty);

        /// <summary>
        /// 点检保养项目
        /// </summary>
        public ProjectDetail ProjectDetail
        {
            get { return GetRefEntity(ProjectDetailProperty); }
            set { SetRefEntity(ProjectDetailProperty, value); }
        }
        #endregion

        #region 计量设备定检 Calibration
        /// <summary>
        /// 计量设备定检Id
        /// </summary>
        public static readonly IRefIdProperty CalibrationIdProperty = P<CalibrationItem>.RegisterRefId(e => e.CalibrationId, ReferenceType.Parent);

        /// <summary>
        /// 计量设备定检Id
        /// </summary>
        public double CalibrationId
        {
            get { return (double)GetRefId(CalibrationIdProperty); }
            set { SetRefId(CalibrationIdProperty, value); }
        }

        /// <summary>
        /// 计量设备定检
        /// </summary>
        public static readonly RefEntityProperty<Calibration> CalibrationProperty = P<CalibrationItem>.RegisterRef(e => e.Calibration, CalibrationIdProperty);

        /// <summary>
        /// 计量设备定检
        /// </summary>
        public Calibration Calibration
        {
            get { return GetRefEntity(CalibrationProperty); }
            set { SetRefEntity(CalibrationProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 计量设备定检项目 实体配置
    /// </summary>
    internal class CalibrationItemConfig : EntityConfig<CalibrationItem>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_CAL_ITEM").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}