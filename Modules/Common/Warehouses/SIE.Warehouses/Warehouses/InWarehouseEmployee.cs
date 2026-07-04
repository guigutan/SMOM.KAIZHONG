using SIE.DataAuth;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.Warehouses
{
    /// <summary>
    /// 调拨调入仓库与员工关系
    /// </summary>
    [RootEntity, Serializable]
    [Label("可调拨至仓库")]
    public partial class InWarehouseEmployee : DataEntity
    {
        #region 仓库与员工关系 Employee
        /// <summary>
        /// 仓库与员工关系Id
        /// </summary>
        [Label("员工")]
        public static readonly IRefIdProperty EmployeeIdProperty = P<InWarehouseEmployee>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Employee> EmployeeProperty = P<InWarehouseEmployee>.RegisterRef(e => e.Employee, EmployeeIdProperty);

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
        public static readonly IRefIdProperty WarehouseIdProperty = P<InWarehouseEmployee>.RegisterRefId(e => e.WarehouseId, ReferenceType.Parent);

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
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<InWarehouseEmployee>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

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
        public static readonly Property<string> EmployeeCodeProperty = P<InWarehouseEmployee>.RegisterView(e => e.EmployeeCode, p => p.Employee.Code);

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
        public static readonly Property<string> EmployeeNameProperty = P<InWarehouseEmployee>.RegisterView(e => e.EmployeeName, p => p.Employee.Name);

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
        public static readonly Property<string> WarehouseCodeProperty = P<InWarehouseEmployee>.RegisterView(e => e.WarehouseCode, p => p.Warehouse.Code);

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
        public static readonly Property<string> WarehouseNameProperty = P<InWarehouseEmployee>.RegisterView(e => e.WarehouseName, p => p.Warehouse.Name);

        /// <summary>
        /// 注释
        /// </summary>
        public string WarehouseName
        {
            get { return this.GetProperty(WarehouseNameProperty); }
        }
        #endregion

        #region 库存组织名称
        /// <summary>
        /// 库存组织名称
        /// </summary>
        [Label("库存组织名称")]
        public static readonly Property<string> InvOrgNameProperty = P<InWarehouseEmployee>.Register(e => e.InvOrgName);

        /// <summary>
        /// 库存组织名称
        /// </summary>
        public string InvOrgName
        {
            get { return GetProperty(InvOrgNameProperty); }
            set { SetProperty(InvOrgNameProperty, value); }
        }
        #endregion

        #region 库存组织编码
        /// <summary>
        /// 仓库库存组织编码
        /// </summary>
        [Label("库存组织编码")]
        public static readonly Property<int> InvOrgCodeProperty = P<InWarehouseEmployee>.Register(e => e.InvOrgCode);

        /// <summary>
        /// 仓库库存组织编码
        /// </summary>
        public int InvOrgCode
        {
            get { return GetProperty(InvOrgCodeProperty); }
            set { SetProperty(InvOrgCodeProperty, value); }
        }
        #endregion

        #region 直接调拨 IsDirectAllocate
        /// <summary>
        /// 直接调拨
        /// </summary>
        [Label("直接调拨")]
        public static readonly Property<bool> IsDirectAllocateProperty = P<InWarehouseEmployee>.Register(e => e.IsDirectAllocate);

        /// <summary>
        /// 直接调拨
        /// </summary>
        public bool IsDirectAllocate
        {
            get { return GetProperty(IsDirectAllocateProperty); }
            set { SetProperty(IsDirectAllocateProperty, value); }
        }
        #endregion

        #region 两步调拨 IsTwoAllocate
        /// <summary>
        /// 是否两步调拨
        /// </summary>
        [Label("两步调拨")]
        public static readonly Property<bool> IsTwoAllocateProperty = P<InWarehouseEmployee>.Register(e => e.IsTwoAllocate);

        /// <summary>
        /// 两步调拨
        /// </summary>
        public bool IsTwoAllocate
        {
            get { return GetProperty(IsTwoAllocateProperty); }
            set { SetProperty(IsTwoAllocateProperty, value); }
        }
        #endregion

        #region 跨组织调拨 IsCrossOrgTransferIn
        /// <summary>
        /// 跨组织调拨
        /// </summary>
        [Label("跨组织调拨")]
        public static readonly Property<bool> IsCrossOrgTransferInProperty = P<InWarehouseEmployee>.Register(e => e.IsCrossOrgTransferIn);

        /// <summary>
        /// 跨组织调拨
        /// </summary>
        public bool IsCrossOrgTransferIn
        {
            get { return GetProperty(IsCrossOrgTransferInProperty); }
            set { SetProperty(IsCrossOrgTransferInProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 仓库与员工关系 实体配置
    /// </summary>
    internal class InWarehouseEmployeeConfig : EntityConfig<InWarehouseEmployee>
    {
        /// <summary>
        /// 配置数据库的映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("IN_WH_EMP").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}