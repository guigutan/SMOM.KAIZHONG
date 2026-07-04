using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.EMS.Purchases.EquipmentSetups
{
    /// <summary>
    /// 操作记录
    /// </summary>
    [ChildEntity, Serializable]
    [Label("操作记录")]
    public partial class SetupLog : DataEntity
    {
        #region 操作记录 EquipmentSetup
        /// <summary>
        /// 操作记录Id
        /// </summary>
        public static readonly IRefIdProperty EquipmentSetupIdProperty = P<SetupLog>.RegisterRefId(e => e.EquipmentSetupId, ReferenceType.Parent);

        /// <summary>
        /// 操作记录Id
        /// </summary>
        public double EquipmentSetupId
        {
            get { return (double)GetRefId(EquipmentSetupIdProperty); }
            set { SetRefId(EquipmentSetupIdProperty, value); }
        }

        /// <summary>
        /// 操作记录
        /// </summary>
        public static readonly RefEntityProperty<EquipmentSetup> EquipmentSetupProperty = P<SetupLog>.RegisterRef(e => e.EquipmentSetup, EquipmentSetupIdProperty);

        /// <summary>
        /// 操作记录
        /// </summary>
        public EquipmentSetup EquipmentSetup
        {
            get { return GetRefEntity(EquipmentSetupProperty); }
            set { SetRefEntity(EquipmentSetupProperty, value); }
        }
        #endregion

        #region 操作 OperationText
        /// <summary>
        /// 操作
        /// </summary>
        [Label("操作")]
        public static readonly Property<string> OperationTextProperty = P<SetupLog>.Register(e => e.OperationText);

        /// <summary>
        /// 操作
        /// </summary>
        public string OperationText
        {
            get { return GetProperty(OperationTextProperty); }
            set { SetProperty(OperationTextProperty, value); }
        }
        #endregion

        #region 操作时间 OperationDateTime
        /// <summary>
        /// 操作时间
        /// </summary>
        [Label("操作时间")]
        public static readonly Property<DateTime> OperationDateTimeProperty = P<SetupLog>.Register(e => e.OperationDateTime);

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperationDateTime
        {
            get { return GetProperty(OperationDateTimeProperty); }
            set { SetProperty(OperationDateTimeProperty, value); }
        }
        #endregion

        #region 操作人 Employee
        /// <summary>
        /// 操作人Id
        /// </summary>
        [Label("操作人")]
        public static readonly IRefIdProperty EmployeeIdProperty = P<SetupLog>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

        /// <summary>
        /// 操作人Id
        /// </summary>
        public double EmployeeId
        {
            get { return (double)GetRefId(EmployeeIdProperty); }
            set { SetRefId(EmployeeIdProperty, value); }
        }

        /// <summary>
        /// 操作人
        /// </summary>
        public static readonly RefEntityProperty<Employee> EmployeeProperty = P<SetupLog>.RegisterRef(e => e.Employee, EmployeeIdProperty);

        /// <summary>
        /// 操作人
        /// </summary>
        public Employee Employee
        {
            get { return GetRefEntity(EmployeeProperty); }
            set { SetRefEntity(EmployeeProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 操作人编码 EmployeeCode
        /// <summary>
        /// 操作人编码
        /// </summary>
        [Label("操作人编码")]
        public static readonly Property<string> EmployeeCodeProperty = P<SetupLog>.RegisterView(e => e.EmployeeCode, p => p.Employee.Code);

        /// <summary>
        /// 操作人编码
        /// </summary>
        public string EmployeeCode
        {
            get { return this.GetProperty(EmployeeCodeProperty); }
        }
        #endregion

        #region 操作人名称 EmployeeName
        /// <summary>
        /// 操作人名称
        /// </summary>
        [Label("操作人名称")]
        public static readonly Property<string> EmployeeNameProperty = P<SetupLog>.RegisterView(e => e.EmployeeName, p => p.Employee.Name);

        /// <summary>
        /// 操作人名称
        /// </summary>
        public string EmployeeName
        {
            get { return this.GetProperty(EmployeeNameProperty); }
        }
        #endregion

        #region 操作人 EmployeeShow
        /// <summary>
        /// 操作人
        /// </summary>
        [Label("操作人")]
        public static readonly Property<string> EmployeeShowProperty = P<SetupLog>.RegisterReadOnly(
            e => e.EmployeeShow, e => e.GetEmployeeShow(), EmployeeCodeProperty);
        /// <summary>
        /// 操作人
        /// </summary>

        public string EmployeeShow
        {
            get { return this.GetProperty(EmployeeShowProperty); }
        }
        private string GetEmployeeShow()
        {
            return EmployeeName + "(" + EmployeeCode + ")";
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 操作记录 实体配置
    /// </summary>
    internal class SetupLogConfig : EntityConfig<SetupLog>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_SETUP_LOG").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}