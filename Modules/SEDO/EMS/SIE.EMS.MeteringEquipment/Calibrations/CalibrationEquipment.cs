using SIE;
using SIE.Common;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.MeteringEquipment.Calibrations
{
    /// <summary>
    /// 设备明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("设备明细")]
    public partial class CalibrationEquipment : DataEntity
    {
        /// <summary>
        /// 快码类型：精度级别
        /// </summary>
        public const string PrecisionClassType = "PRECISION_CLASS_TYPE";


        #region 是否降级 IsDowngrade
        /// <summary>
        /// 是否降级
        /// </summary>
        [Label("是否降级")]
        public static readonly Property<bool> IsDowngradeProperty = P<CalibrationEquipment>.Register(e => e.IsDowngrade);

        /// <summary>
        /// 是否降级
        /// </summary>
        public bool IsDowngrade
        {
            get { return GetProperty(IsDowngradeProperty); }
            set { SetProperty(IsDowngradeProperty, value); }
        }
        #endregion

        #region 精度级别 PrecisionClass
        /// <summary>
        /// 精度级别
        /// </summary>
        [Label("精度级别")]
        public static readonly Property<string> PrecisionClassProperty = P<CalibrationEquipment>.Register(e => e.PrecisionClass);

        /// <summary>
        /// 精度级别
        /// </summary>
        public string PrecisionClass
        {
            get { return GetProperty(PrecisionClassProperty); }
            set { SetProperty(PrecisionClassProperty, value); }
        }
        #endregion

        #region 检验日期 InspectionDate
        /// <summary>
        /// 检验日期
        /// </summary>
        [Label("检验日期")]
        public static readonly Property<DateTime?> InspectionDateProperty = P<CalibrationEquipment>.Register(e => e.InspectionDate);

        /// <summary>
        /// 检验日期
        /// </summary>
        public DateTime? InspectionDate
        {
            get { return GetProperty(InspectionDateProperty); }
            set { SetProperty(InspectionDateProperty, value); }
        }
        #endregion

        #region 设备 MeteringEquipmentAccount
        /// <summary>
        /// 设备Id
        /// </summary>
        public static readonly IRefIdProperty MeteringEquipmentAccountIdProperty = P<CalibrationEquipment>.RegisterRefId(e => e.MeteringEquipmentAccountId, ReferenceType.Normal);

        /// <summary>
        /// 设备Id
        /// </summary>
        public double MeteringEquipmentAccountId
        {
            get { return (double)GetRefId(MeteringEquipmentAccountIdProperty); }
            set { SetRefId(MeteringEquipmentAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备
        /// </summary>
        public static readonly RefEntityProperty<MeteringEquipmentAccount> MeteringEquipmentAccountProperty = P<CalibrationEquipment>.RegisterRef(e => e.MeteringEquipmentAccount, MeteringEquipmentAccountIdProperty);

        /// <summary>
        /// 设备
        /// </summary>
        public MeteringEquipmentAccount MeteringEquipmentAccount
        {
            get { return GetRefEntity(MeteringEquipmentAccountProperty); }
            set { SetRefEntity(MeteringEquipmentAccountProperty, value); }
        }
        #endregion

        #region 检验结果 InspectionResult
        /// <summary>
        /// 检验结果
        /// </summary>
        [Label("检验结果")]
        public static readonly Property<InspectionResult?> InspectionResultProperty = P<CalibrationEquipment>.Register(e => e.InspectionResult);

        /// <summary>
        /// 检验结果
        /// </summary>
        public InspectionResult? InspectionResult
        {
            get { return GetProperty(InspectionResultProperty); }
            set { SetProperty(InspectionResultProperty, value); }
        }
        #endregion

        #region 计量设备定检 Calibration
        /// <summary>
        /// 计量设备定检Id
        /// </summary>
        public static readonly IRefIdProperty CalibrationIdProperty = P<CalibrationEquipment>.RegisterRefId(e => e.CalibrationId, ReferenceType.Parent);

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
        public static readonly RefEntityProperty<Calibration> CalibrationProperty = P<CalibrationEquipment>.RegisterRef(e => e.Calibration, CalibrationIdProperty);

        /// <summary>
        /// 计量设备定检
        /// </summary>
        public Calibration Calibration
        {
            get { return GetRefEntity(CalibrationProperty); }
            set { SetRefEntity(CalibrationProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 设备编码  MeteringEquipmentAccountCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> MeteringEquipmentAccountCodeProperty = P<CalibrationEquipment>.RegisterView(e => e.MeteringEquipmentAccountCode, p => p.MeteringEquipmentAccount.Code);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string MeteringEquipmentAccountCode
        {
            get { return this.GetProperty(MeteringEquipmentAccountCodeProperty); }
            set { this.SetProperty(MeteringEquipmentAccountCodeProperty, value); }
        }
        #endregion

        #region 设备名称  MeteringEquipmentAccountName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> MeteringEquipmentAccountNameProperty = P<CalibrationEquipment>.RegisterView(e => e.MeteringEquipmentAccountName, p => p.MeteringEquipmentAccount.Name);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string MeteringEquipmentAccountName
        {
            get { return this.GetProperty(MeteringEquipmentAccountNameProperty); }
            set { this.SetProperty(MeteringEquipmentAccountNameProperty, value); }
        }
        #endregion

        #region 技术规格 Specifications
        /// <summary>
        /// 技术规格
        /// </summary>
        [Label("技术规格")]
        public static readonly Property<string> SpecificationsProperty = P<CalibrationEquipment>.RegisterView(e => e.Specifications, p => p.MeteringEquipmentAccount.EquipModel.Specifications);

        /// <summary>
        /// 技术规格
        /// </summary>
        public string Specifications
        {
            get { return this.GetProperty(SpecificationsProperty); }
            set { this.SetProperty(SpecificationsProperty, value); }
        }
        #endregion

        #region 设备状态 MeteringEquipmentAccountState
        /// <summary>
        /// 设备状态
        /// </summary>
        [Label("设备状态")]
        public static readonly Property<AccountState> MeteringEquipmentAccountStateProperty = P<CalibrationEquipment>.RegisterView(e => e.MeteringEquipmentAccountState, p => p.MeteringEquipmentAccount.State);

        /// <summary>
        /// 设备状态
        /// </summary>
        public AccountState MeteringEquipmentAccountState
        {
            get { return this.GetProperty(MeteringEquipmentAccountStateProperty); }
            set { this.SetProperty(MeteringEquipmentAccountStateProperty, value); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 计量设备定检设备明细 实体配置
    /// </summary>
    internal class CalibrationEquipmentConfig : EntityConfig<CalibrationEquipment>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_CAL_EQP").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}