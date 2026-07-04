using SIE.Core.Enums;
using SIE.Domain;
using SIE.EMS.Enums;
using SIE.EMS.InspectionRules;
using SIE.Equipments.EquipModels;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.MeteringEquipment.EquipModelExtensions
{
    /// <summary>
    /// 计量校验规程
    /// </summary>
    [RootEntity, Serializable]
    //[CriteriaQuery]
    [Label("计量校验规程")]
    public partial class EquipModelCalibration : DataEntity
    {
        #region 检验规程 InspectionRule
        /// <summary>
        /// 检验规程Id
        /// </summary>
        public static readonly IRefIdProperty InspectionRuleIdProperty = P<EquipModelCalibration>.RegisterRefId(e => e.InspectionRuleId, ReferenceType.Normal);

        /// <summary>
        /// 检验规程Id
        /// </summary>
        public double InspectionRuleId
        {
            get { return (double)GetRefId(InspectionRuleIdProperty); }
            set { SetRefId(InspectionRuleIdProperty, value); }
        }

        /// <summary>
        /// 检验规程
        /// </summary>
        public static readonly RefEntityProperty<InspectionRule> InspectionRuleProperty = P<EquipModelCalibration>.RegisterRef(e => e.InspectionRule, InspectionRuleIdProperty);

        /// <summary>
        /// 检验规程
        /// </summary>
        public InspectionRule InspectionRule
        {
            get { return GetRefEntity(InspectionRuleProperty); }
            set { SetRefEntity(InspectionRuleProperty, value); }
        }
        #endregion

        #region 设备型号维护 EquipModel
        /// <summary>
        /// 设备型号维护Id
        /// </summary>
        public static readonly IRefIdProperty EquipModelIdProperty
            = P<EquipModelCalibration>.RegisterRefId(e => e.EquipModelId, ReferenceType.Parent);

        /// <summary>
        /// 设备型号维护Id
        /// </summary>
        public double EquipModelId
        {
            get { return (double)GetRefId(EquipModelIdProperty); }
            set { SetRefId(EquipModelIdProperty, value); }
        }

        /// <summary>
        /// 设备型号维护
        /// </summary>
        public static readonly RefEntityProperty<EquipModel> EquipModelProperty
            = P<EquipModelCalibration>.RegisterRef(e => e.EquipModel, EquipModelIdProperty);

        /// <summary>
        /// 设备型号维护
        /// </summary>
        public EquipModel EquipModel
        {
            get { return GetRefEntity(EquipModelProperty); }
            set { SetRefEntity(EquipModelProperty, value); }
        }
        #endregion

        #region 引用属性
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<EquipModelCalibration>.RegisterView(e => e.Code, p => p.InspectionRule.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<EquipModelCalibration>.RegisterView(e => e.Name, p => p.InspectionRule.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 周期(天) PeriodDays
        /// <summary>
        /// 周期(天)
        /// </summary>
        [Label("周期(天)")]
        public static readonly Property<int> PeriodDaysProperty = P<EquipModelCalibration>.RegisterView(e => e.PeriodDays, p => p.InspectionRule.PeriodDays);

        /// <summary>
        /// 周期(天)
        /// </summary>
        public int PeriodDays
        {
            get { return GetProperty(PeriodDaysProperty); }
            set { SetProperty(PeriodDaysProperty, value); }
        }
        #endregion

        #region 预警期(天) WarningPeriod
        /// <summary>
        /// 预警期(天)
        /// </summary>
        [Label("预警期(天)")]
        public static readonly Property<int> WarningPeriodProperty = P<EquipModelCalibration>.RegisterView(e => e.WarningPeriod, p => p.InspectionRule.WarningPeriod);

        /// <summary>
        /// 预警期(天)
        /// </summary>
        public int WarningPeriod
        {
            get { return GetProperty(WarningPeriodProperty); }
            set { SetProperty(WarningPeriodProperty, value); }
        }
        #endregion

        #region 项目类型 InspectionRuleType
        /// <summary>
        /// 项目类型
        /// </summary>
        [Label("项目类型")]
        public static readonly Property<InspectionRuleType> InspectionRuleTypeProperty = P<EquipModelCalibration>.RegisterView(e => e.InspectionRuleType, p => p.InspectionRule.InspectionRuleType);

        /// <summary>
        /// 项目类型
        /// </summary>
        public InspectionRuleType InspectionRuleType
        {
            get { return GetProperty(InspectionRuleTypeProperty); }
            set { SetProperty(InspectionRuleTypeProperty, value); }
        }
        #endregion

        #region 校验类别 CheckCategory
        /// <summary>
        /// 校验类别
        /// </summary>
        [Label("校验类别")]
        public static readonly Property<CheckCategory> CheckCategoryProperty = P<EquipModelCalibration>.RegisterView(e => e.CheckCategory, p => p.InspectionRule.CheckCategory);

        /// <summary>
        /// 校验类别
        /// </summary>
        public CheckCategory CheckCategory
        {
            get { return GetProperty(CheckCategoryProperty); }
            set { SetProperty(CheckCategoryProperty, value); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 计量校验规程 实体配置
    /// </summary>
    internal class EquipModelCalibrationConfig : EntityConfig<EquipModelCalibration>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_MODEL_CALIBRATION").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}