using SIE.DataAuth;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.Inventory.Transactions
{
    /// <summary>
    /// 单据大类与员工关系
    /// </summary>
    [ChildEntity, Serializable]
    [EmployeeAuth(nameof(EmployeeId), nameof(FunctionId))]
    public partial class FunctionEmployee : DataEntity
    {
        #region 仓库与员工关系 Employee
        /// <summary>
        /// 仓库与员工关系Id
        /// </summary>
        [Label("员工")]
        public static readonly IRefIdProperty EmployeeIdProperty = P<FunctionEmployee>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

        /// <summary>
        /// 仓库与员工关系Id
        /// </summary>
        public double EmployeeId
        {
            get { return (double)GetRefNullableId(EmployeeIdProperty); }
            set { SetRefNullableId(EmployeeIdProperty, value); }
        }

        /// <summary>
        /// 仓库与员工关系
        /// </summary>        
        public static readonly RefEntityProperty<Employee> EmployeeProperty = P<FunctionEmployee>.RegisterRef(e => e.Employee, EmployeeIdProperty);

        /// <summary>
        /// 仓库与员工关系
        /// </summary>
        public Employee Employee
        {
            get { return GetRefEntity(EmployeeProperty); }
            set { SetRefEntity(EmployeeProperty, value); }
        }
        #endregion

        #region 单据大类 Function
        /// <summary>
        /// 单据大类Id
        /// </summary>
        [Label("单据大类")]
        public static readonly IRefIdProperty FunctionIdProperty =
            P<FunctionEmployee>.RegisterRefId(e => e.FunctionId, ReferenceType.Parent);

        /// <summary>
        /// 单据大类Id
        /// </summary>
        public double FunctionId
        {
            get { return (double)this.GetRefId(FunctionIdProperty); }
            set { this.SetRefId(FunctionIdProperty, value); }
        }

        /// <summary>
        /// 单据大类
        /// </summary>
        public static readonly RefEntityProperty<Function> FunctionProperty =
            P<FunctionEmployee>.RegisterRef(e => e.Function, FunctionIdProperty);

        /// <summary>
        /// 单据大类
        /// </summary>
        public Function Function
        {
            get { return this.GetRefEntity(FunctionProperty); }
            set { this.SetRefEntity(FunctionProperty, value); }
        }
        #endregion

        #region 工号 EmployeeCode
        /// <summary>
        /// 工号
        /// </summary>
        [Label("工号")]
        public static readonly Property<string> EmployeeCodeProperty = P<FunctionEmployee>.RegisterView(e => e.EmployeeCode, p => p.Employee.Code);

        /// <summary>
        /// 工号
        /// </summary>
        public string EmployeeCode
        {
            get { return this.GetProperty(EmployeeCodeProperty); }
        }
        #endregion

        #region 姓名 EmployeeName
        /// <summary>
        /// 姓名
        /// </summary>
        [Label("姓名")]
        public static readonly Property<string> EmployeeNameProperty = P<FunctionEmployee>.RegisterView(e => e.EmployeeName, p => p.Employee.Name);

        /// <summary>
        /// 姓名
        /// </summary>
        public string EmployeeName
        {
            get { return this.GetProperty(EmployeeNameProperty); }
        }
        #endregion
    }

    /// <summary>
    /// 单据大类与员工关系 实体配置
    /// </summary>
    internal class FunctionEmployeeConfig : EntityConfig<FunctionEmployee>
    {
        /// <summary>
        /// 配置数据库的映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TRANS_FUNCTION_EMP").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
