using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.MES.Workbench.AlertLights
{
    /// <summary>
    /// 员工呼叫通知
    /// </summary>
    [ChildEntity, Serializable]
    ////[CriteriaQuery]
    [Label("员工呼叫通知")]
    public partial class EmpCallInform : DataEntity
    {
        #region 员工 Employee
        /// <summary>
        /// 员工Id
        /// </summary>
        [Label("员工")]
        public static readonly IRefIdProperty EmployeeIdProperty = P<EmpCallInform>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

        /// <summary>
        /// 员工Id
        /// </summary>
        public double EmployeeId
        {
            get { return (double)GetRefId(EmployeeIdProperty); }
            set { SetRefId(EmployeeIdProperty, value); }
        }

        /// <summary>
        /// 员工
        /// </summary>
        public static readonly RefEntityProperty<Employee> EmployeeProperty = P<EmpCallInform>.RegisterRef(e => e.Employee, EmployeeIdProperty);

        /// <summary>
        /// 员工
        /// </summary>
        public Employee Employee
        {
            get { return GetRefEntity(EmployeeProperty); }
            set { SetRefEntity(EmployeeProperty, value); }
        }
        #endregion

        #region 通知列表 EmpCallSetting
        /// <summary>
        /// 通知列表Id
        /// </summary>
        public static readonly IRefIdProperty EmpCallSettingIdProperty = P<EmpCallInform>.RegisterRefId(e => e.EmpCallSettingId, ReferenceType.Parent);

        /// <summary>
        /// 通知列表Id
        /// </summary>
        public double EmpCallSettingId
        {
            get { return (double)GetRefId(EmpCallSettingIdProperty); }
            set { SetRefId(EmpCallSettingIdProperty, value); }
        }

        /// <summary>
        /// 通知列表
        /// </summary>
        public static readonly RefEntityProperty<EmpCallSetting> EmpCallSettingProperty = P<EmpCallInform>.RegisterRef(e => e.EmpCallSetting, EmpCallSettingIdProperty);

        /// <summary>
        /// 通知列表
        /// </summary>
        public EmpCallSetting EmpCallSetting
        {
            get { return GetRefEntity(EmpCallSettingProperty); }
            set { SetRefEntity(EmpCallSettingProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 员工呼叫通知实体配置
    /// </summary>
    internal class EmpCallInformConfig : EntityConfig<EmpCallInform>
    {
        /// <summary>
        /// 员工呼叫通知实体配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("RES_EMP_CALL_INFORM").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}