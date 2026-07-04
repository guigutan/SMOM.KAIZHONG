using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.EquipRepairs
{
    /// <summary>
    /// 设备维修人员
    /// </summary>
    [RootEntity, Serializable]
    [Label("设备维修人员")]
    public partial class EquipRepairEmployee : DataEntity
    {
        #region 员工 Employee
        /// <summary>
        /// 员工Id
        /// </summary>
        [Label("员工编码")]
        public static readonly IRefIdProperty EmployeeIdProperty =
            P<EquipRepairEmployee>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

        /// <summary>
        /// 员工Id
        /// </summary>
        public double EmployeeId
        {
            get { return (double)this.GetRefId(EmployeeIdProperty); }
            set { this.SetRefId(EmployeeIdProperty, value); }
        }

        /// <summary>
        /// 员工
        /// </summary>
        public static readonly RefEntityProperty<Employee> EmployeeProperty =
            P<EquipRepairEmployee>.RegisterRef(e => e.Employee, EmployeeIdProperty);

        /// <summary>
        /// 员工
        /// </summary>
        public Employee Employee
        {
            get { return this.GetRefEntity(EmployeeProperty); }
            set { this.SetRefEntity(EmployeeProperty, value); }
        }
        #endregion

        #region 员工名称 EmployeeName
        /// <summary>
        /// 员工名称
        /// </summary>
        [Label("员工名称")]
        public static readonly Property<string> EmployeeNameProperty = P<EquipRepairEmployee>.Register(e => e.EmployeeName);

        /// <summary>
        /// 员工名称
        /// </summary>
        public string EmployeeName
        {
            get { return this.GetProperty(EmployeeNameProperty); }
            set { this.SetProperty(EmployeeNameProperty, value); }
        }
        #endregion

        #region 工作状态 WorkState
        /// <summary>
        /// 工作状态
        /// </summary>
        [Label("工作状态")]
        public static readonly Property<string> WorkStateProperty = P<EquipRepairEmployee>.Register(e => e.WorkState);

        /// <summary>
        /// 工作状态
        /// </summary>
        public string WorkState
        {
            get { return this.GetProperty(WorkStateProperty); }
            set { this.SetProperty(WorkStateProperty, value); }
        }
        #endregion

        #region 待维修任务 WaitRepairTask
        /// <summary>
        /// 待维修任务
        /// </summary>
        [Label("待维修任务")]
        public static readonly Property<int> WaitRepairTaskProperty = P<EquipRepairEmployee>.Register(e => e.WaitRepairTask);

        /// <summary>
        /// 待维修任务
        /// </summary>
        public int WaitRepairTask
        {
            get { return this.GetProperty(WaitRepairTaskProperty); }
            set { this.SetProperty(WaitRepairTaskProperty, value); }
        }
        #endregion

        #region 维修中任务 RepairingTask
        /// <summary>
        /// 维修中任务
        /// </summary>
        [Label("维修中任务")]
        public static readonly Property<int> RepairingTaskProperty = P<EquipRepairEmployee>.Register(e => e.RepairingTask);

        /// <summary>
        /// 维修中任务
        /// </summary>
        public int RepairingTask
        {
            get { return this.GetProperty(RepairingTaskProperty); }
            set { this.SetProperty(RepairingTaskProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 设备维修人员 实体配置
    /// </summary>
    internal class EquipRepairEmployeeConfig : EntityConfig<EquipRepairEmployee>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQUIP_REPAIR_EMPL").MapAllProperties();
            Meta.Property(EquipRepairEmployee.EmployeeIdProperty).DontMapColumn();
            Meta.Property(EquipRepairEmployee.EmployeeProperty).DontMapColumn();
            Meta.Property(EquipRepairEmployee.EmployeeNameProperty).DontMapColumn();
            Meta.Property(EquipRepairEmployee.WorkStateProperty).DontMapColumn();
            Meta.Property(EquipRepairEmployee.WaitRepairTaskProperty).DontMapColumn();
            Meta.Property(EquipRepairEmployee.RepairingTaskProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}
