using SIE.DataAuth;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.Warehouses
{
    /// <summary>
    /// 仓库与员工关系
    /// </summary>
    [ChildEntity, Serializable]
    [EmployeeAuth(nameof(EmployeeId), nameof(WarehouseId))]
    public partial class WarehouseEmployee : DataEntity
    {
        #region 仓库与员工关系 Employee
        /// <summary>
        /// 仓库与员工关系Id
        /// </summary>
        [Label("员工")]
        public static readonly IRefIdProperty EmployeeIdProperty = P<WarehouseEmployee>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Employee> EmployeeProperty = P<WarehouseEmployee>.RegisterRef(e => e.Employee, EmployeeIdProperty);

        /// <summary>
        /// 仓库与员工关系
        /// </summary>
        public Employee Employee
        {
            get { return GetRefEntity(EmployeeProperty); }
            set { SetRefEntity(EmployeeProperty, value); }
        }
        #endregion

        #region 仓库与员工关系 Warehouse
        /// <summary>
        /// 仓库与员工关系Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty = P<WarehouseEmployee>.RegisterRefId(e => e.WarehouseId, ReferenceType.Parent);

        /// <summary>
        /// 仓库与员工关系Id
        /// </summary>
        public double WarehouseId
        {
            get { return (double)GetRefNullableId(WarehouseIdProperty); }
            set { SetRefNullableId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 仓库与员工关系
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<WarehouseEmployee>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库与员工关系
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 工号 EmployeeCode
        /// <summary>
        /// 工号
        /// </summary>
        [Label("工号")]
        public static readonly Property<string> EmployeeCodeProperty = P<WarehouseEmployee>.RegisterView(e => e.EmployeeCode, p => p.Employee.Code);

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
        public static readonly Property<string> EmployeeNameProperty = P<WarehouseEmployee>.RegisterView(e => e.EmployeeName, p => p.Employee.Name);

        /// <summary>
        /// 姓名
        /// </summary>
        public string EmployeeName
        {
            get { return this.GetProperty(EmployeeNameProperty); }
        }
        #endregion

        #region 仓库编码 WarehouseCode
        /// <summary>
        /// 仓库编码
        /// </summary>
        [Label("仓库编码")]
        public static readonly Property<string> WarehouseCodeProperty = P<WarehouseEmployee>.RegisterView(e => e.WarehouseCode, p => p.Warehouse.Code);

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WarehouseCode
        {
            get { return this.GetProperty(WarehouseCodeProperty); }
        }
        #endregion

        #region 仓库名称 WarehouseName
        /// <summary>
        /// 仓库名称
        /// </summary>
        [Label("仓库名称")]
        public static readonly Property<string> WarehouseNameProperty = P<WarehouseEmployee>.RegisterView(e => e.WarehouseName, p => p.Warehouse.Name);

        /// <summary>
        /// 注释
        /// </summary>
        public string WarehouseName
        {
            get { return this.GetProperty(WarehouseNameProperty); }
        }
        #endregion

    }

    /// <summary>
    /// 仓库与员工关系 实体配置
    /// </summary>
    internal class WarehouseEmployeeConfig : EntityConfig<WarehouseEmployee>
    {
        /// <summary>
        /// 配置数据库的映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WH_EMP").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}