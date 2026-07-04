using SIE;
using SIE.Common;
using SIE.Domain;
using SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.EMS.MeteringEquipment.Calibrations
{
    /// <summary>
    /// 检验明细
    /// </summary>
    [ChildEntity, Serializable]
    //[CriteriaQuery]
    [Label("检验明细")]
    public partial class CalibrationDetail : DataEntity
    {
        #region 设备 MeteringEquipmentAccount
        /// <summary>
        /// 设备Id
        /// </summary>
        [Label("设备编码")]
        public static readonly IRefIdProperty MeteringEquipmentAccountIdProperty = P<CalibrationDetail>.RegisterRefId(e => e.MeteringEquipmentAccountId, ReferenceType.Normal);

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
        [Label("设备编码")]
        public static readonly RefEntityProperty<MeteringEquipmentAccount> MeteringEquipmentAccountProperty = P<CalibrationDetail>.RegisterRef(e => e.MeteringEquipmentAccount, MeteringEquipmentAccountIdProperty);

        /// <summary>
        /// 设备
        /// </summary>
        public MeteringEquipmentAccount MeteringEquipmentAccount
        {
            get { return GetRefEntity(MeteringEquipmentAccountProperty); }
            set { SetRefEntity(MeteringEquipmentAccountProperty, value); }
        }
        #endregion

        #region 设备名称 MeteringEquipmentAccountName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> MeteringEquipmentAccountNameProperty = P<CalibrationDetail>.RegisterView(e => e.MeteringEquipmentAccountName, e => e.MeteringEquipmentAccount.Name);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string MeteringEquipmentAccountName
        {
            get { return GetProperty(MeteringEquipmentAccountNameProperty); }
            set { SetProperty(MeteringEquipmentAccountNameProperty, value); }
        }
        #endregion

        #region 定检项目 CalibrationItem
        /// <summary>
        /// 定检项目Id
        /// </summary>
        [Label("项目名称")]
        public static readonly IRefIdProperty CalibrationItemIdProperty = P<CalibrationDetail>.RegisterRefId(e => e.CalibrationItemId, ReferenceType.Normal);

        /// <summary>
        /// 定检项目Id
        /// </summary>
        public double CalibrationItemId
        {
            get { return (double)GetRefId(CalibrationItemIdProperty); }
            set { SetRefId(CalibrationItemIdProperty, value); }
        }

        /// <summary>
        /// 定检项目
        /// </summary>
        [Label("项目名称")]
        public static readonly RefEntityProperty<CalibrationItem> CalibrationItemProperty = P<CalibrationDetail>.RegisterRef(e => e.CalibrationItem, CalibrationItemIdProperty);

        /// <summary>
        /// 定检项目
        /// </summary>
        public CalibrationItem CalibrationItem
        {
            get { return GetRefEntity(CalibrationItemProperty); }
            set { SetRefEntity(CalibrationItemProperty, value); }
        }
        #endregion

        #region 检验结果 InspectionResult
        /// <summary>
        /// 检验结果
        /// </summary>
        [Label("检验结果")]
        public static readonly Property<InspectionResult> InspectionResultProperty = P<CalibrationDetail>.Register(e => e.InspectionResult);

        /// <summary>
        /// 检验结果
        /// </summary>
        public InspectionResult InspectionResult
        {
            get { return GetProperty(InspectionResultProperty); }
            set { SetProperty(InspectionResultProperty, value); }
        }
        #endregion

        #region 检验日期 InspectionDateTime
        /// <summary>
        /// 检验日期
        /// </summary>
        [Label("检验日期")]
        public static readonly Property<DateTime> InspectionDateTimeProperty = P<CalibrationDetail>.Register(e => e.InspectionDateTime);

        /// <summary>
        /// 检验日期
        /// </summary>
        public DateTime InspectionDateTime
        {
            get { return GetProperty(InspectionDateTimeProperty); }
            set { SetProperty(InspectionDateTimeProperty, value); }
        }
        #endregion

        #region 检验人 Inspector
        /// <summary>
        /// 检验人Id
        /// </summary>
        [Label("检验人")]
        public static readonly IRefIdProperty InspectorIdProperty = P<CalibrationDetail>.RegisterRefId(e => e.InspectorId, ReferenceType.Normal);

        /// <summary>
        /// 检验人Id
        /// </summary>
        public double InspectorId
        {
            get { return (double)GetRefId(InspectorIdProperty); }
            set { SetRefId(InspectorIdProperty, value); }
        }

        /// <summary>
        /// 检验人
        /// </summary>
        [Label("检验人")]
        public static readonly RefEntityProperty<Employee> InspectorProperty = P<CalibrationDetail>.RegisterRef(e => e.Inspector, InspectorIdProperty);

        /// <summary>
        /// 检验人
        /// </summary>
        public Employee Inspector
        {
            get { return GetRefEntity(InspectorProperty); }
            set { SetRefEntity(InspectorProperty, value); }
        }
        #endregion

        #region 检验明细 Calibration
        /// <summary>
        /// 检验明细Id
        /// </summary>
        public static readonly IRefIdProperty CalibrationIdProperty = P<CalibrationDetail>.RegisterRefId(e => e.CalibrationId, ReferenceType.Parent);

        /// <summary>
        /// 检验明细Id
        /// </summary>
        public double CalibrationId
        {
            get { return (double)GetRefId(CalibrationIdProperty); }
            set { SetRefId(CalibrationIdProperty, value); }
        }

        /// <summary>
        /// 检验明细
        /// </summary>
        public static readonly RefEntityProperty<Calibration> CalibrationProperty = P<CalibrationDetail>.RegisterRef(e => e.Calibration, CalibrationIdProperty);

        /// <summary>
        /// 检验明细
        /// </summary>
        public Calibration Calibration
        {
            get { return GetRefEntity(CalibrationProperty); }
            set { SetRefEntity(CalibrationProperty, value); }
        }
        #endregion

        #region 数据列 CalibrationValue
        /// <summary>
        /// 数据列
        /// </summary>
        [Label("数据列")]
        public static readonly Property<string> CalibrationValueProperty = P<CalibrationDetail>.Register(e => e.CalibrationValue);

        /// <summary>
        /// 数据列
        /// </summary>
        public string CalibrationValue
        {
            get { return GetProperty(CalibrationValueProperty); }
            set { SetProperty(CalibrationValueProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 计量设备检验明细 实体配置
    /// </summary>
    internal class CalibrationDetailConfig : EntityConfig<CalibrationDetail>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_CAL_DTL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}