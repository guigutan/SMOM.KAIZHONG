using SIE.Common.Users;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.CSM.Suppliers
{
    /// <summary>
    /// 供应商与用户关系
    /// </summary>
    [RootEntity, Serializable]
    [Label("供应商与用户关系")]
    public partial class SupplierUser : DataEntity
    {
        #region 用户 User
        /// <summary>
        /// 用户Id
        /// </summary>
        [Label("用户")]
        public static readonly IRefIdProperty UserIdProperty = P<SupplierUser>.RegisterRefId(e => e.UserId, ReferenceType.Normal);

        /// <summary>
        /// 用户Id
        /// </summary>
        public double UserId
        {
            get { return (double)GetRefId(UserIdProperty); }
            set { SetRefId(UserIdProperty, value); }
        }

        /// <summary>
        /// 用户
        /// </summary>
        public static readonly RefEntityProperty<User> UserProperty = P<SupplierUser>.RegisterRef(e => e.User, UserIdProperty);

        /// <summary>
        /// 用户
        /// </summary>
        public User User
        {
            get { return GetRefEntity(UserProperty); }
            set { SetRefEntity(UserProperty, value); }
        }
        #endregion

        #region 供应商与用户关系 Supplier
        /// <summary>
        /// 供应商与用户关系Id
        /// </summary>
        [Label("供应商")]
        public static readonly IRefIdProperty SupplierIdProperty = P<SupplierUser>.RegisterRefId(e => e.SupplierId, ReferenceType.Parent);

        /// <summary>
        /// 供应商与用户关系Id
        /// </summary>
        public double SupplierId
        {
            get { return (double)GetRefNullableId(SupplierIdProperty); }
            set { SetRefNullableId(SupplierIdProperty, value); }
        }

        /// <summary>
        /// 供应商与用户关系
        /// </summary>
        public static readonly RefEntityProperty<Supplier> SupplierProperty = P<SupplierUser>.RegisterRef(e => e.Supplier, SupplierIdProperty);

        /// <summary>
        /// 供应商与用户关系
        /// </summary>
        public Supplier Supplier
        {
            get { return GetRefEntity(SupplierProperty); }
            set { SetRefEntity(SupplierProperty, value); }
        }
        #endregion

        #region 注册视图属性(关联实体属性平铺显示，一般用于Web)

        #region 工号 EmployeeCode
        /// <summary>
        /// 工号
        /// </summary>
        [Label("工号")]
        public static readonly Property<string> EmployeeCodeProperty = P<SupplierUser>.RegisterView(e => e.EmployeeCode, p => p.User.Employee.Code);

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
        public static readonly Property<string> EmployeeNameProperty = P<SupplierUser>.RegisterView(e => e.EmployeeName, p => p.User.Employee.Name);

        /// <summary>
        /// 姓名
        /// </summary>
        public string EmployeeName
        {
            get { return this.GetProperty(EmployeeNameProperty); }
        }
        #endregion

        #region 用户账号 UserCode
        /// <summary>
        /// 用户账号
        /// </summary>
        [Label("用户账号")]
        public static readonly Property<string> UserCodeProperty = P<SupplierUser>.RegisterView(e => e.UserCode, p => p.User.Code);

        /// <summary>
        /// 用户账号
        /// </summary>
        public string UserCode
        {
            get { return this.GetProperty(UserCodeProperty); }
        }
        #endregion

        #region 用户状态 UserState
        /// <summary>
        /// 用户状态
        /// </summary>
        [Label("用户状态")]
        public static readonly Property<State> UserStateProperty = P<SupplierUser>.RegisterView(e => e.UserState, p => p.User.State);

        /// <summary>
        /// 用户状态
        /// </summary>
        public State UserState
        {
            get { return this.GetProperty(UserStateProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 供应商与用户关系 实体配置
    /// </summary>
    internal class SupplierUserConfig : EntityConfig<SupplierUser>
    {
        /// <summary>
        /// 供应商表名
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("CSM_SUPPLIER_USER").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}