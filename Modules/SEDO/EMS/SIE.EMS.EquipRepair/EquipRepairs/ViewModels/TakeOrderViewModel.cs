using SIE.Domain;
using SIE.EMS.DevicePurs;
using SIE.EMS.Equipments.Accounts;
using SIE.Equipments.EquipAccounts;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Web.EMS.EquipRepair.EquipRepairs.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.EquipRepairs.ViewModels
{
    /// <summary>
    /// 接单ViewModel
    /// </summary>
    [RootEntity, Serializable]
    [Label("接单")]
    //[DisplayMember(nameof())]
    public class TakeOrderViewModel  : ViewModel
    {

        #region 设备台账 EquipAccount
        /// <summary>
        /// 设备台账Id
        /// </summary>
        [Label("设备台账")]
        public static readonly IRefIdProperty EquipAccountIdProperty =
            P<TakeOrderViewModel>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 设备台账Id
        /// </summary>
        public double EquipAccountId
        {
            get { return (double)this.GetRefId(EquipAccountIdProperty); }
            set { this.SetRefId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备台账
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty =
            P<TakeOrderViewModel>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备台账
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return this.GetRefEntity(EquipAccountProperty); }
            set { this.SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 维修责任人 MaintenancePerson
        /// <summary>
        /// 维修责任人Id
        /// </summary>
        [Label("维修责任人")]
        public static readonly IRefIdProperty MaintenancePersonIdProperty =
            P<TakeOrderViewModel>.RegisterRefId(e => e.MaintenancePersonId, ReferenceType.Normal);

        /// <summary>
        /// 维修责任人Id
        /// </summary>
        public double MaintenancePersonId
        {
            get { return (double)this.GetRefId(MaintenancePersonIdProperty); }
            set { this.SetRefId(MaintenancePersonIdProperty, value); }
        }

        /// <summary>
        /// 维修责任人
        /// </summary>
        public static readonly RefEntityProperty<Employee> MaintenancePersonProperty =
            P<TakeOrderViewModel>.RegisterRef(e => e.MaintenancePerson, MaintenancePersonIdProperty);

        /// <summary>
        /// 维修责任人
        /// </summary>
        public Employee MaintenancePerson
        {
            get { return this.GetRefEntity(MaintenancePersonProperty); }
            set { this.SetRefEntity(MaintenancePersonProperty, value); }
        }
        #endregion

        #region 维修人员 EmployeeName
        /// <summary>
        /// 维修人员
        /// </summary>
        [Label("维修人员")]
        public static readonly Property<string> EmployeeNameProperty = P<TakeOrderViewModel>.Register(e => e.EmployeeName);

        /// <summary>
        /// 维修人员
        /// </summary>
        public string EmployeeName
        {
            get { return this.GetProperty(EmployeeNameProperty); }
            set { this.SetProperty(EmployeeNameProperty, value); }
        }
        #endregion

        #region 派工类型 DispatchType
        /// <summary>
        /// 派工类型
        /// </summary>
        [Label("派工类型")]
        public static readonly Property<DispatchType> DispatchTypeProperty = P<TakeOrderViewModel>.Register(e => e.DispatchType);

        /// <summary>
        /// 派工类型
        /// </summary>
        public DispatchType DispatchType
        {
            get { return this.GetProperty(DispatchTypeProperty); }
            set { this.SetProperty(DispatchTypeProperty, value); }
        }
        #endregion

        #region 设备保修期限 GuaranteeRange
        /// <summary>
        /// 设备保修期限
        /// </summary>
        [Label("设备保修期限")]
        public static readonly Property<DateTime?> GuaranteeRangeProperty = P<TakeOrderViewModel>.Register(e => e.GuaranteeRange);

        /// <summary>
        /// 设备保修期限
        /// </summary>
        public DateTime? GuaranteeRange
        {
            get { return this.GetProperty(GuaranteeRangeProperty); }
            set { this.SetProperty(GuaranteeRangeProperty, value); }
        }
        #endregion

        #region 保修期类型 GuaranteeRangeType
        /// <summary>
        /// 保修期类型
        /// </summary>
        [Label("保修期类型")]
        public static readonly Property<GuaranteeRangeType> GuaranteeRangeTypeProperty = P<TakeOrderViewModel>.Register(e => e.GuaranteeRangeType);

        /// <summary>
        /// 保修期类型
        /// </summary>
        public GuaranteeRangeType GuaranteeRangeType
        {
            get { return this.GetProperty(GuaranteeRangeTypeProperty); }
            set { this.SetProperty(GuaranteeRangeTypeProperty, value); }
        }
        #endregion

        #region 预计完成时间 ExpectCompleteTime
        /// <summary>
        /// 预计完成时间
        /// </summary>
        [Label("预计完成时间")]
        [Required]
        public static readonly Property<DateTime> ExpectCompleteTimeProperty = P<TakeOrderViewModel>.Register(e => e.ExpectCompleteTime);

        /// <summary>
        /// 预计完成时间
        /// </summary>
        public DateTime ExpectCompleteTime
        {
            get { return this.GetProperty(ExpectCompleteTimeProperty); }
            set { this.SetProperty(ExpectCompleteTimeProperty, value); }
        }
        #endregion

    }
}
