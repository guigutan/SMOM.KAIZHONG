using SIE.Domain;
using SIE.EMS.EquipLends.Enums;
using SIE.Equipments.EquipModels;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipLends
{
    /// <summary>
    /// 设备借还管理查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("设备借还管理查询实体")]
    public class EquipLendManageCriteria : Criteria
    {
        #region 设备编码 EquipCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> EquipCodeProperty = P<EquipLendManageCriteria>.Register(e => e.EquipCode);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipCode
        {
            get { return this.GetProperty(EquipCodeProperty); }
            set { this.SetProperty(EquipCodeProperty, value); }
        }
        #endregion

        #region 资产编码 FixCode
        /// <summary>
        /// 资产编码
        /// </summary>
        [Label("资产编码")]
        public static readonly Property<string> FixCodeProperty = P<EquipLendManageCriteria>.Register(e => e.FixCode);

        /// <summary>
        /// 资产编码
        /// </summary>
        public string FixCode
        {
            get { return this.GetProperty(FixCodeProperty); }
            set { this.SetProperty(FixCodeProperty, value); }
        }
        #endregion

        #region RFID RFID
        /// <summary>
        /// RFID
        /// </summary>
        [Label("RFID")]
        public static readonly Property<string> RFIDProperty = P<EquipLendManageCriteria>.Register(e => e.RFID);

        /// <summary>
        /// RFID
        /// </summary>
        public string RFID
        {
            get { return this.GetProperty(RFIDProperty); }
            set { this.SetProperty(RFIDProperty, value); }
        }
        #endregion

        #region 设备型号 EquipModel
        /// <summary>
        /// 设备型号Id
        /// </summary>
        [Label("设备型号")]
        public static readonly IRefIdProperty EquipModelIdProperty =
            P<EquipLendManageCriteria>.RegisterRefId(e => e.EquipModelId, ReferenceType.Normal);

        /// <summary>
        /// 设备型号Id
        /// </summary>
        public double? EquipModelId
        {
            get { return (double?)this.GetRefNullableId(EquipModelIdProperty); }
            set { this.SetRefNullableId(EquipModelIdProperty, value); }
        }

        /// <summary>
        /// 设备型号
        /// </summary>
        public static readonly RefEntityProperty<EquipModel> EquipModelProperty =
            P<EquipLendManageCriteria>.RegisterRef(e => e.EquipModel, EquipModelIdProperty);

        /// <summary>
        /// 设备型号
        /// </summary>
        public EquipModel EquipModel
        {
            get { return this.GetRefEntity(EquipModelProperty); }
            set { this.SetRefEntity(EquipModelProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<LendState?> StateProperty = P<EquipLendManageCriteria>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public LendState? State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        #region 借机对象 LendObject
        /// <summary>
        /// 借机对象
        /// </summary>
        [Label("借机对象")]
        public static readonly Property<LendObject?> LendObjectProperty = P<EquipLendManageCriteria>.Register(e => e.LendObject);

        /// <summary>
        /// 借机对象
        /// </summary>
        public LendObject? LendObject
        {
            get { return this.GetProperty(LendObjectProperty); }
            set { this.SetProperty(LendObjectProperty, value); }
        }
        #endregion

        #region 借机部门 LendEnterprise
        /// <summary>
        /// 借机部门Id
        /// </summary>
        [Label("借机部门")]
        public static readonly IRefIdProperty LendEnterpriseIdProperty =
            P<EquipLendManageCriteria>.RegisterRefId(e => e.LendEnterpriseId, ReferenceType.Normal);

        /// <summary>
        /// 借机部门Id
        /// </summary>
        public double? LendEnterpriseId
        {
            get { return (double?)this.GetRefNullableId(LendEnterpriseIdProperty); }
            set { this.SetRefNullableId(LendEnterpriseIdProperty, value); }
        }

        /// <summary>
        /// 借机部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> LendEnterpriseProperty =
            P<EquipLendManageCriteria>.RegisterRef(e => e.LendEnterprise, LendEnterpriseIdProperty);

        /// <summary>
        /// 借机部门
        /// </summary>
        public Enterprise LendEnterprise
        {
            get { return this.GetRefEntity(LendEnterpriseProperty); }
            set { this.SetRefEntity(LendEnterpriseProperty, value); }
        }
        #endregion

        #region 借机人 LendEmployee
        /// <summary>
        /// 借机人Id
        /// </summary>
        [Label("借机人")]
        public static readonly IRefIdProperty LendEmployeeIdProperty =
            P<EquipLendManageCriteria>.RegisterRefId(e => e.LendEmployeeId, ReferenceType.Normal);

        /// <summary>
        /// 借机人Id
        /// </summary>
        public double? LendEmployeeId
        {
            get { return (double?)this.GetRefNullableId(LendEmployeeIdProperty); }
            set { this.SetRefNullableId(LendEmployeeIdProperty, value); }
        }

        /// <summary>
        /// 借机人
        /// </summary>
        public static readonly RefEntityProperty<Employee> LendEmployeeProperty =
            P<EquipLendManageCriteria>.RegisterRef(e => e.LendEmployee, LendEmployeeIdProperty);

        /// <summary>
        /// 借机人
        /// </summary>
        public Employee LendEmployee
        {
            get { return this.GetRefEntity(LendEmployeeProperty); }
            set { this.SetRefEntity(LendEmployeeProperty, value); }
        }
        #endregion

        #region 借机日期 LendDate
        /// <summary>
        /// 借机日期
        /// </summary>
        [Label("借机日期")]
        public static readonly Property<DateRange> LendDateProperty = P<EquipLendManageCriteria>.Register(e => e.LendDate);

        /// <summary>
        /// 借机日期
        /// </summary>
        public DateRange LendDate
        {
            get { return this.GetProperty(LendDateProperty); }
            set { this.SetProperty(LendDateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询逻辑
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<EquipLendController>().CriteriaQueryEntityList(this);
        }
    }
}
