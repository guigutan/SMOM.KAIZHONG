using SIE;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.EMS.Enums;
using SIE.EMS.InspectionRules;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts
{
    /// <summary>
    /// 计量校验规程
    /// </summary>
    [ChildEntity, Serializable]
    [Label("计量校验规程")]
    public partial class EquipAccountCalibration : DataEntity
    {
        #region 上次检验日期 PrevInspectionDate
        /// <summary>
        /// 上次检验日期
        /// </summary>
        [Label("上次检验日期")]
        public static readonly Property<DateTime?> PrevInspectionDateProperty = P<EquipAccountCalibration>.Register(e => e.PrevInspectionDate);

        /// <summary>
        /// 上次检验日期
        /// </summary>
        public DateTime? PrevInspectionDate
        {
            get { return GetProperty(PrevInspectionDateProperty); }
            set { SetProperty(PrevInspectionDateProperty, value); }
        }
        #endregion

        #region 下次检验时间 NextInspectionDate
        /// <summary>
        /// 下次检验时间
        /// </summary>
        [Label("下次检验时间")]
        public static readonly Property<DateTime?> NextInspectionDateProperty = P<EquipAccountCalibration>.Register(e => e.NextInspectionDate);

        /// <summary>
        /// 下次检验时间
        /// </summary>
        public DateTime? NextInspectionDate
        {
            get { return GetProperty(NextInspectionDateProperty); }
            set { SetProperty(NextInspectionDateProperty, value); }
        }
        #endregion

        #region 周期(天) PeriodDays
        /// <summary>
        /// 周期(天)
        /// </summary>
        [Label("周期(天)")]
        public static readonly Property<int> PeriodDaysProperty = P<EquipAccountCalibration>.Register(e => e.PeriodDays);

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
        public static readonly Property<int> WarningPeriodProperty = P<EquipAccountCalibration>.Register(e => e.WarningPeriod);

        /// <summary>
        /// 预警期(天)
        /// </summary>
        public int WarningPeriod
        {
            get { return GetProperty(WarningPeriodProperty); }
            set { SetProperty(WarningPeriodProperty, value); }
        }
        #endregion

        #region 是否未提交 NotSubmit
        /// <summary>
        /// 是否未提交 true:已提交,可再次查询到  false:未提交 不能再次生成
        /// </summary>
        [Label("是否未提交")]
        public static readonly Property<bool?> NotSubmitProperty = P<EquipAccountCalibration>.Register(e => e.NotSubmit);

        /// <summary>
        /// 是否未提交
        /// </summary>
        public bool? NotSubmit
        {
            get { return GetProperty(NotSubmitProperty); }
            set { SetProperty(NotSubmitProperty, value); }
        }
        #endregion

        #region 检验规程 InspectionRule
        /// <summary>
        /// 检验规程Id
        /// </summary>
        public static readonly IRefIdProperty InspectionRuleIdProperty = P<EquipAccountCalibration>.RegisterRefId(e => e.InspectionRuleId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<InspectionRule> InspectionRuleProperty = P<EquipAccountCalibration>.RegisterRef(e => e.InspectionRule, InspectionRuleIdProperty);

        /// <summary>
        /// 检验规程
        /// </summary>
        public InspectionRule InspectionRule
        {
            get { return GetRefEntity(InspectionRuleProperty); }
            set { SetRefEntity(InspectionRuleProperty, value); }
        }
        #endregion

        #region 计量校验台账 MeteringEquipmentAccount
        /// <summary>
        /// 计量校验台账Id
        /// </summary>
        public static readonly IRefIdProperty MeteringEquipmentAccountIdProperty = P<EquipAccountCalibration>.RegisterRefId(e => e.MeteringEquipmentAccountId, ReferenceType.Parent);

        /// <summary>
        /// 计量校验台账Id
        /// </summary>
        public double MeteringEquipmentAccountId
        {
            get { return (double)GetRefId(MeteringEquipmentAccountIdProperty); }
            set { SetRefId(MeteringEquipmentAccountIdProperty, value); }
        }

        /// <summary>
        /// 计量校验台账
        /// </summary>
        public static readonly RefEntityProperty<MeteringEquipmentAccount> MeteringEquipmentAccountProperty = P<EquipAccountCalibration>.RegisterRef(e => e.MeteringEquipmentAccount, MeteringEquipmentAccountIdProperty);

        /// <summary>
        /// 计量校验台账
        /// </summary>
        public MeteringEquipmentAccount MeteringEquipmentAccount
        {
            get { return GetRefEntity(MeteringEquipmentAccountProperty); }
            set { SetRefEntity(MeteringEquipmentAccountProperty, value); }
        }
        #endregion

        #region 引用属性

        #region 检验编码 Code
        /// <summary>
        /// 检验编码
        /// </summary>
        [Label("检验编码")]
        public static readonly Property<string> CodeProperty = P<EquipAccountCalibration>.RegisterView(e => e.Code, e => e.InspectionRule.Code);

        /// <summary>
        /// 检验编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 检验名称 Name
        /// <summary>
        /// 检验名称
        /// </summary>
        [Label("检验名称")]
        public static readonly Property<string> NameProperty = P<EquipAccountCalibration>.RegisterView(e => e.Name, e => e.InspectionRule.Name);

        /// <summary>
        /// 检验名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 项目类型 InspectionRuleType
        /// <summary>
        /// 项目类型
        /// </summary>
        [Label("项目类型")]
        public static readonly Property<InspectionRuleType> InspectionRuleTypeProperty = P<EquipAccountCalibration>.RegisterView(e => e.InspectionRuleType, e => e.InspectionRule.InspectionRuleType);

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
        public static readonly Property<CheckCategory> CheckCategoryProperty = P<EquipAccountCalibration>.RegisterView(e => e.CheckCategory, e => e.InspectionRule.CheckCategory);

        /// <summary>
        /// 校验类别
        /// </summary>
        public CheckCategory CheckCategory
        {
            get { return GetProperty(CheckCategoryProperty); }
            set { SetProperty(CheckCategoryProperty, value); }
        }
        #endregion

        #region 精度级别 PrecisionClass
        /// <summary>
        /// 精度级别
        /// </summary>
        [Label("精度级别")]
        public static readonly Property<string> PrecisionClassProperty = P<EquipAccountCalibration>.RegisterView(e => e.PrecisionClass, p => p.MeteringEquipmentAccount.PrecisionClass);

        /// <summary>
        /// 精度级别
        /// </summary>
        public string PrecisionClass
        {
            get { return this.GetProperty(PrecisionClassProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 计量校验规程 实体配置
    /// </summary>
    internal class EquipAccountCalibrationConfig : EntityConfig<EquipAccountCalibration>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQUIP_CALIBRATION").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}