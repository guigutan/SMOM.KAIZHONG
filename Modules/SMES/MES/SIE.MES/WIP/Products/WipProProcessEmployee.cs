using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.WIP.Products
{
    /// <summary>
    /// 条码工序指派员工信息
    /// </summary>
    [ChildEntity, Serializable]
    [Label("条码工序指派员工信息")]
    public partial class WipProProcessEmployee : DataEntity
    {
        #region 采集记录 WipProductProcess
        /// <summary>
        /// 采集记录Id
        /// </summary>
        [Label("采集记录")]
        public static readonly IRefIdProperty WipProductProcessIdProperty =
            P<WipProProcessEmployee>.RegisterRefId(e => e.WipProductProcessId, ReferenceType.Parent);

        /// <summary>
        /// 采集记录Id
        /// </summary>
        public double WipProductProcessId
        {
            get { return (double)this.GetRefId(WipProductProcessIdProperty); }
            set { this.SetRefId(WipProductProcessIdProperty, value); }
        }

        /// <summary>
        /// 采集记录
        /// </summary>
        public static readonly RefEntityProperty<WipProductProcess> WipProductProcessProperty =
            P<WipProProcessEmployee>.RegisterRef(e => e.WipProductProcess, WipProductProcessIdProperty);

        /// <summary>
        /// 采集记录
        /// </summary>
        public WipProductProcess WipProductProcess
        {
            get { return this.GetRefEntity(WipProductProcessProperty); }
            set { this.SetRefEntity(WipProductProcessProperty, value); }
        }
        #endregion

        #region 员工 Employee
        /// <summary>
        /// 员工Id
        /// </summary>
        [Label("员工")]
        public static readonly IRefIdProperty EmployeeIdProperty =
            P<WipProProcessEmployee>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

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
            P<WipProProcessEmployee>.RegisterRef(e => e.Employee, EmployeeIdProperty);

        /// <summary>
        /// 员工
        /// </summary>
        public Employee Employee
        {
            get { return this.GetRefEntity(EmployeeProperty); }
            set { this.SetRefEntity(EmployeeProperty, value); }
        }
        #endregion

        #region 员工工号 EmployeeCode
        /// <summary>
        /// 员工工号
        /// </summary>
        [Label("员工工号")]
        public static readonly Property<string> EmployeeCodeProperty = P<WipProProcessEmployee>.RegisterView(e => e.EmployeeCode, p => p.Employee.Code);

        /// <summary>
        /// 员工工号
        /// </summary>
        public string EmployeeCode
        {
            get { return this.GetProperty(EmployeeCodeProperty); }
        }
        #endregion

        #region 员工名称 EmployeeName
        /// <summary>
        /// 员工名称
        /// </summary>
        [Label("员工名称")]
        public static readonly Property<string> EmployeeNameProperty = P<WipProProcessEmployee>.RegisterView(e => e.EmployeeName, p => p.Employee.Name);

        /// <summary>
        /// 员工名称
        /// </summary>
        public string EmployeeName
        {
            get { return this.GetProperty(EmployeeNameProperty); }
        }
        #endregion

    }

    /// <summary>
    /// 数据库配置
    /// </summary>
    public class WipProProcessEmployeeConfig : EntityConfig<WipProProcessEmployee>
    {
        /// <summary>
        /// 数据源
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_PROD_PROEMP").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
