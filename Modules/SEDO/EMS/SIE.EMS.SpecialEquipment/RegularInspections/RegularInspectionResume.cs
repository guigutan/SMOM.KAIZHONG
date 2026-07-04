using SIE.Common;
using SIE.Domain;
using SIE.EMS.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.EMS.SpecialEquipment.RegularInspections
{
    /// <summary>
    /// 操作记录
    /// </summary>
    [ChildEntity, Serializable]
    //[CriteriaQuery]
    [Label("操作记录")]
    public partial class RegularInspectionResume : DataEntity
    {
        #region 操作类型 OperationType
        /// <summary>
        /// 操作类型
        /// </summary>
        [Label("操作类型")]
        public static readonly Property<OperationType> OperationTypeProperty = P<RegularInspectionResume>.Register(e => e.OperationType);

        /// <summary>
        /// 操作类型
        /// </summary>
        public OperationType OperationType
        {
            get { return GetProperty(OperationTypeProperty); }
            set { SetProperty(OperationTypeProperty, value); }
        }
        #endregion

        #region 操作时间 OperationDateTime
        /// <summary>
        /// 操作时间
        /// </summary>
        [Label("操作时间")]
        public static readonly Property<DateTime> OperationDateTimeProperty = P<RegularInspectionResume>.Register(e => e.OperationDateTime);

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperationDateTime
        {
            get { return GetProperty(OperationDateTimeProperty); }
            set { SetProperty(OperationDateTimeProperty, value); }
        }
        #endregion

        #region 检验结果 InspectionResult
        /// <summary>
        /// 检验结果
        /// </summary>
        [Label("检验结果")]
        public static readonly Property<InspectionResult?> InspectionResultProperty = P<RegularInspectionResume>.Register(e => e.InspectionResult);

        /// <summary>
        /// 检验结果
        /// </summary>
        public InspectionResult? InspectionResult
        {
            get { return GetProperty(InspectionResultProperty); }
            set { SetProperty(InspectionResultProperty, value); }
        }
        #endregion

        #region 操作人 Operator
        /// <summary>
        /// 操作人Id
        /// </summary>
        public static readonly IRefIdProperty OperatorIdProperty = P<RegularInspectionResume>.RegisterRefId(e => e.OperatorId, ReferenceType.Normal);

        /// <summary>
        /// 操作人Id
        /// </summary>
        public double OperatorId
        {
            get { return (double)GetRefId(OperatorIdProperty); }
            set { SetRefId(OperatorIdProperty, value); }
        }

        /// <summary>
        /// 操作人
        /// </summary>
        public static readonly RefEntityProperty<Employee> OperatorProperty = P<RegularInspectionResume>.RegisterRef(e => e.Operator, OperatorIdProperty);

        /// <summary>
        /// 操作人
        /// </summary>
        public Employee Operator
        {
            get { return GetRefEntity(OperatorProperty); }
            set { SetRefEntity(OperatorProperty, value); }
        }
        #endregion

        #region 特种设备定检 RegularInspection
        /// <summary>
        /// 特种设备定检Id
        /// </summary>
        public static readonly IRefIdProperty RegularInspectionIdProperty = P<RegularInspectionResume>.RegisterRefId(e => e.RegularInspectionId, ReferenceType.Parent);

        /// <summary>
        /// 特种设备定检Id
        /// </summary>
        public double RegularInspectionId
        {
            get { return (double)GetRefId(RegularInspectionIdProperty); }
            set { SetRefId(RegularInspectionIdProperty, value); }
        }

        /// <summary>
        /// 特种设备定检
        /// </summary>
        public static readonly RefEntityProperty<RegularInspection> RegularInspectionProperty = P<RegularInspectionResume>.RegisterRef(e => e.RegularInspection, RegularInspectionIdProperty);

        /// <summary>
        /// 特种设备定检
        /// </summary>
        public RegularInspection RegularInspection
        {
            get { return GetRefEntity(RegularInspectionProperty); }
            set { SetRefEntity(RegularInspectionProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 操作记录 实体配置
    /// </summary>
    internal class RegularInspectionResumeConfig : EntityConfig<RegularInspectionResume>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_REG_INS_RESU").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}