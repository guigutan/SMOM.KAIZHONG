using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using System;

namespace SIE.EMS.DevicePurs
{
    /// <summary>
    /// 业务部门
    /// </summary>
    [ChildEntity, Serializable]
    [Label("业务部门")]
    public partial class DeviceUseDepartment : DataEntity
    {
        #region 设备与人员 DevicePur
        /// <summary>
        /// 设备与人员Id
        /// </summary>
        public static readonly IRefIdProperty DevicePurIdProperty
            = P<DeviceUseDepartment>.RegisterRefId(e => e.DevicePurId, ReferenceType.Parent);

        /// <summary>
        /// 设备与人员Id
        /// </summary>
        public double DevicePurId
        {
            get { return (double)GetRefId(DevicePurIdProperty); }
            set { SetRefId(DevicePurIdProperty, value); }
        }

        /// <summary>
        /// 设备与人员Id
        /// </summary>
        public static readonly RefEntityProperty<DevicePur> DevicePurProperty
            = P<DeviceUseDepartment>.RegisterRef(e => e.DevicePur, DevicePurIdProperty);

        /// <summary>
        /// 设备与人员Id
        /// </summary>
        public DevicePur DevicePur
        {
            get { return GetRefEntity(DevicePurProperty); }
            set { SetRefEntity(DevicePurProperty, value); }
        }
        #endregion

        #region 业务部门 Enterprise
        /// <summary>
        /// 业务部门Id
        /// </summary>
        [Label("业务部门")]
        [Required]
        public static readonly IRefIdProperty EnterpriseIdProperty =
            P<DeviceUseDepartment>.RegisterRefId(e => e.EnterpriseId, ReferenceType.Normal);

        /// <summary>
        /// 业务部门Id
        /// </summary>
        public double? EnterpriseId
        {
            get { return (double?)this.GetRefNullableId(EnterpriseIdProperty); }
            set { this.SetRefNullableId(EnterpriseIdProperty, value); }
        }

        /// <summary>
        /// 业务部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> EnterpriseProperty =
            P<DeviceUseDepartment>.RegisterRef(e => e.Enterprise, EnterpriseIdProperty);

        /// <summary>
        /// 业务部门
        /// </summary>
        public Enterprise Enterprise
        {
            get { return this.GetRefEntity(EnterpriseProperty); }
            set { this.SetRefEntity(EnterpriseProperty, value); }
        }
        #endregion


        #region 业务部门 Enterprise
        ///// <summary>
        ///// 业务部门Id
        ///// </summary>
        //[Label("业务部门")]
        //public static readonly IRefIdProperty EnterpriseIdProperty =
        //    P<DeviceUseDepartment>.RegisterRefId(e => e.EnterpriseId, ReferenceType.Normal);

        ///// <summary>
        ///// 业务部门Id
        ///// </summary>
        //public double EnterpriseId
        //{
        //    get { return (double)this.GetRefId(EnterpriseIdProperty); }
        //    set { this.SetRefId(EnterpriseIdProperty, value); }
        //}

        ///// <summary>
        ///// 业务部门
        ///// </summary>
        //public static readonly RefEntityProperty<Enterprise> EnterpriseProperty =
        //    P<DeviceUseDepartment>.RegisterRef(e => e.Enterprise, EnterpriseIdProperty);

        ///// <summary>
        ///// 业务部门
        ///// </summary>
        //public Enterprise Enterprise
        //{
        //    get { return this.GetRefEntity(EnterpriseProperty); }
        //    set { this.SetRefEntity(EnterpriseProperty, value); }
        //}
        #endregion

        #region 视图属性
        #region 部门编码 EnterpriseCode
        /// <summary>
        /// 部门编码
        /// </summary>
        [Label("部门编码")]
        public static readonly Property<string> EnterpriseCodeProperty = P<DeviceUseDepartment>.RegisterView(e => e.EnterpriseCode, p => p.Enterprise.Code);

        /// <summary>
        /// 部门编码
        /// </summary>
        public string EnterpriseCode
        {
            get { return this.GetProperty(EnterpriseCodeProperty); }
        }
        #endregion

        #region 部门名称 EnterpriseName
        /// <summary>
        /// 部门名称
        /// </summary>
        [Label("部门名称")]
        public static readonly Property<string> EnterpriseNameProperty = P<DeviceUseDepartment>.RegisterView(e => e.EnterpriseName, p => p.Enterprise.Name);

        /// <summary>
        /// 部门名称
        /// </summary>
        public string EnterpriseName
        {
            get { return this.GetProperty(EnterpriseNameProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 业务部门 实体配置
    /// </summary>
    internal class DeviceUseDepartmentConfig : EntityConfig<DeviceUseDepartment>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_DEV_USE_DEPA").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
