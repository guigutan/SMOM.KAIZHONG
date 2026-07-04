using SIE.Common;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.EMS.Enums;
using SIE.EMS.SpecialEquipment.SpecialEquipmentAcounts;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using System;

namespace SIE.EMS.SpecialEquipment.RegularInspections.Criterias
{
    /// <summary>
    /// 检验规程查询实体
    /// </summary>
    [QueryEntity, Serializable]
    public class RegularInspectionCriteria : Criteria
    {
        #region 检验单号 InspectionNo
        /// <summary>
        /// 检验单号
        /// </summary>
        [Label("检验单号")]
        public static readonly Property<string> InspectionNoProperty = P<RegularInspectionCriteria>.Register(e => e.InspectionNo);

        /// <summary>
        /// 检验单号
        /// </summary>
        public string InspectionNo
        {
            get { return GetProperty(InspectionNoProperty); }
            set { SetProperty(InspectionNoProperty, value); }
        }
        #endregion

        #region 特种设备台账 SpecialEquipmentAccount
        /// <summary>
        /// 特种设备台账Id
        /// </summary>
        public static readonly IRefIdProperty SpecialEquipmentAccountIdProperty = P<RegularInspectionCriteria>.RegisterRefId(e => e.SpecialEquipmentAccountId, ReferenceType.Normal);

        /// <summary>
        /// 特种设备台账Id
        /// </summary>
        public double? SpecialEquipmentAccountId
        {
            get { return (double?)this.GetRefNullableId(SpecialEquipmentAccountIdProperty); }
            set { this.SetRefNullableId(SpecialEquipmentAccountIdProperty, value); }
        }

        /// <summary>
        /// 特种设备台账
        /// </summary>
        public static readonly RefEntityProperty<SpecialEquipmentAccount> SpecialEquipmentAccountProperty = P<RegularInspectionCriteria>.RegisterRef(e => e.SpecialEquipmentAccount, SpecialEquipmentAccountIdProperty);

        /// <summary>
        /// 特种设备台账
        /// </summary>
        public SpecialEquipmentAccount SpecialEquipmentAccount
        {
            get { return GetRefEntity(SpecialEquipmentAccountProperty); }
            set { SetRefEntity(SpecialEquipmentAccountProperty, value); }
        }
        #endregion

        #region 检验状态 InspectionStatus
        /// <summary>
        /// 检验状态
        /// </summary>
        [Label("检验状态")]
        public static readonly Property<InspectionStatus?> InspectionStatusProperty = P<RegularInspectionCriteria>.Register(e => e.InspectionStatus);

        /// <summary>
        /// 检验状态
        /// </summary>
        public InspectionStatus? InspectionStatus
        {
            get { return GetProperty(InspectionStatusProperty); }
            set { SetProperty(InspectionStatusProperty, value); }
        }
        #endregion

        #region 检验结果 InspectionResult
        /// <summary>
        /// 检验结果
        /// </summary>
        [Label("检验结果")]
        public static readonly Property<InspectionResult?> InspectionResultProperty = P<RegularInspectionCriteria>.Register(e => e.InspectionResult);

        /// <summary>
        /// 检验结果
        /// </summary>
        public InspectionResult? InspectionResult
        {
            get { return GetProperty(InspectionResultProperty); }
            set { SetProperty(InspectionResultProperty, value); }
        }
        #endregion

        //使用部门
        #region 使用部门 UseDepartment
        /// <summary>
        /// 使用部门Id
        /// </summary>
        [Label("使用部门")]
        public static readonly IRefIdProperty UseDepartmentIdProperty =
            P<RegularInspectionCriteria>.RegisterRefId(e => e.UseDepartmentId, ReferenceType.Normal);

        /// <summary>
        /// 使用部门Id
        /// </summary>
        public double? UseDepartmentId
        {
            get { return (double?)this.GetRefNullableId(UseDepartmentIdProperty); }
            set { this.SetRefNullableId(UseDepartmentIdProperty, value); }
        }

        /// <summary>
        /// 使用部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> UseDepartmentProperty =
            P<RegularInspectionCriteria>.RegisterRef(e => e.UseDepartment, UseDepartmentIdProperty);

        /// <summary>
        /// 使用部门
        /// </summary>
        public Enterprise UseDepartment
        {
            get { return this.GetRefEntity(UseDepartmentProperty); }
            set { this.SetRefEntity(UseDepartmentProperty, value); }
        }
        #endregion


        //资产责任人
        #region 资产责任人 ResPerson
        /// <summary>
        /// 资产责任人Id
        /// </summary>
        [Label("资产责任人")]
        public static readonly IRefIdProperty ResPersonIdProperty = P<RegularInspectionCriteria>.RegisterRefId(e => e.ResPersonId, ReferenceType.Normal);

        /// <summary>
        /// 资产责任人Id
        /// </summary>
        public double? ResPersonId
        {
            get { return (double?)this.GetRefNullableId(ResPersonIdProperty); }
            set { this.SetRefNullableId(ResPersonIdProperty, value); }
        }

        /// <summary>
        /// 责任人
        /// </summary>
        public static readonly RefEntityProperty<Employee> ResPersonProperty =
            P<RegularInspectionCriteria>.RegisterRef(e => e.ResPerson, ResPersonIdProperty);

        /// <summary>
        /// 责任人
        /// </summary>
        public Employee ResPerson
        {
            get { return this.GetRefEntity(ResPersonProperty); }
            set { this.SetRefEntity(ResPersonProperty, value); }
        }
        #endregion

        #region 检验机构 Agency
        /// <summary>
        /// 检验机构Id
        /// </summary>
        public static readonly IRefIdProperty AgencyIdProperty = P<RegularInspectionCriteria>.RegisterRefId(e => e.AgencyId, ReferenceType.Normal);

        /// <summary>
        /// 检验机构Id
        /// </summary>
        public double? AgencyId
        {
            get { return (double?)GetRefNullableId(AgencyIdProperty); }
            set { SetRefNullableId(AgencyIdProperty, value); }
        }

        /// <summary>
        /// 检验机构
        /// </summary>
        public static readonly RefEntityProperty<Supplier> AgencyProperty = P<RegularInspectionCriteria>.RegisterRef(e => e.Agency, AgencyIdProperty);

        /// <summary>
        /// 检验机构
        /// </summary>
        public Supplier Agency
        {
            get { return GetRefEntity(AgencyProperty); }
            set { SetRefEntity(AgencyProperty, value); }
        }
        #endregion

        #region 计划检验日期 PlanInspectionDate
        /// <summary>
        /// 计划检验日期
        /// </summary>
        [Label("计划检验日期")]
        public static readonly Property<DateRange> PlanInspectionDateProperty = P<RegularInspectionCriteria>.Register(e => e.PlanInspectionDate);

        /// <summary>
        /// 计划检验日期
        /// </summary>
        public DateRange PlanInspectionDate
        {
            get { return GetProperty(PlanInspectionDateProperty); }
            set { SetProperty(PlanInspectionDateProperty, value); }
        }
        #endregion

        #region 实际检验日期 ActualInspectionDate
        /// <summary>
        /// 实际检验日期
        /// </summary>
        [Label("实际检验日期")]
        public static readonly Property<DateRange> ActualInspectionDateProperty = P<RegularInspectionCriteria>.Register(e => e.ActualInspectionDate);

        /// <summary>
        /// 实际检验日期
        /// </summary>
        public DateRange ActualInspectionDate
        {
            get { return GetProperty(ActualInspectionDateProperty); }
            set { SetProperty(ActualInspectionDateProperty, value); }
        }
        #endregion


        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns>检验规程列表</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<RegularInspectionController>().GetRegularInspectionList(this);
        }
    }
}