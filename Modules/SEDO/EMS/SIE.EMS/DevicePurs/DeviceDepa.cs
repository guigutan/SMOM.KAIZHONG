using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using System;

namespace SIE.EMS.DevicePurs
{
    /// <summary>
    /// 责任部门
    /// </summary>
    [ChildEntity, Serializable]
    [Label("责任部门")]
    public partial class DeviceDepa : DataEntity
    {
        #region 设备与人员 DevicePur
        /// <summary>
        /// 设备与人员Id
        /// </summary>
        public static readonly IRefIdProperty DevicePurIdProperty = P<DeviceDepa>.RegisterRefId(e => e.DevicePurId, ReferenceType.Parent);

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
        public static readonly RefEntityProperty<DevicePur> DevicePurProperty = P<DeviceDepa>.RegisterRef(e => e.DevicePur, DevicePurIdProperty);

        /// <summary>
        /// 设备与人员Id
        /// </summary>
        public DevicePur DevicePur
        {
            get { return GetRefEntity(DevicePurProperty); }
            set { SetRefEntity(DevicePurProperty, value); }
        }
        #endregion

        #region 企业模型 Enterprise
        /// <summary>
        /// 企业模型Id
        /// </summary>
        public static readonly IRefIdProperty EnterpriseIdProperty = P<DeviceDepa>.RegisterRefId(e => e.EnterpriseId, ReferenceType.Normal);

        /// <summary>
        /// 企业模型Id
        /// </summary>
        public double EnterpriseId
        {
            get { return (double)GetRefId(EnterpriseIdProperty); }
            set { SetRefId(EnterpriseIdProperty, value); }
        }

        /// <summary>
        /// 企业模型Id
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> EnterpriseProperty = P<DeviceDepa>.RegisterRef(e => e.Enterprise, EnterpriseIdProperty);

        /// <summary>
        /// 企业模型Id
        /// </summary>
        public Enterprise Enterprise
        {
            get { return GetRefEntity(EnterpriseProperty); }
            set { SetRefEntity(EnterpriseProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 部门编码 EnterpriseCode
        /// <summary>
        /// 部门编码
        /// </summary>
        [Label("部门编码")]
        public static readonly Property<string> EnterpriseCodeProperty = P<DeviceDepa>.RegisterView(e => e.EnterpriseCode, p => p.Enterprise.Code);

        /// <summary>
        /// 部门编码
        /// </summary>
        public string EnterpriseCode
        {
            get { return this.GetProperty(EnterpriseCodeProperty); }
            set { this.SetProperty(EnterpriseCodeProperty, value); }
        }
        #endregion

        #region 部门名称 EnterpriseName
        /// <summary>
        /// 部门名称
        /// </summary>
        [Label("部门名称")]
        public static readonly Property<string> EnterpriseNameProperty = P<DeviceDepa>.RegisterView(e => e.EnterpriseName, p => p.Enterprise.Name);

        /// <summary>
        /// 部门名称
        /// </summary>
        public string EnterpriseName
        {
            get { return this.GetProperty(EnterpriseNameProperty); }
            set { this.SetProperty(EnterpriseNameProperty, value); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 责任部门 实体配置
    /// </summary>
    internal class DeviceDepaConfig : EntityConfig<DeviceDepa>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_DEVICE_DEPA").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}