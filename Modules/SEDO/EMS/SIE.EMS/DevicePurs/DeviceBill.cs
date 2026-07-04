using SIE.Domain;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.DevicePurs
{
    /// <summary>
    /// 设备清单
    /// </summary>
    [RootEntity, Serializable]
    //[CriteriaQuery]
    [Label("设备清单")]
    public partial class DeviceBill : DataEntity
    {
        #region 设备台账  EquipAccount
        /// <summary>
        /// 设备台账Id
        /// </summary>
        public static readonly IRefIdProperty EquipAccountIdProperty = P<DeviceBill>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 设备台账Id
        /// </summary>
        public double EquipAccountId
        {
            get { return (double)GetRefId(EquipAccountIdProperty); }
            set { SetRefId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备台账Id
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty = P<DeviceBill>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备台账Id
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return GetRefEntity(EquipAccountProperty); }
            set { SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 设备与人员 DevicePur
        /// <summary>
        /// 设备与人员Id
        /// </summary>
        public static readonly IRefIdProperty DevicePurIdProperty = P<DeviceBill>.RegisterRefId(e => e.DevicePurId, ReferenceType.Parent);

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
        public static readonly RefEntityProperty<DevicePur> DevicePurProperty = P<DeviceBill>.RegisterRef(e => e.DevicePur, DevicePurIdProperty);

        /// <summary>
        /// 设备与人员Id
        /// </summary>
        public DevicePur DevicePur
        {
            get { return GetRefEntity(DevicePurProperty); }
            set { SetRefEntity(DevicePurProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 设备编码 EquipAccountCode
        /// <summary>
        /// 项目名称
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> EquipAccountCodeProperty = P<DeviceBill>.RegisterView(e => e.EquipAccountCode, p => p.EquipAccount.Code);

        /// <summary>
        /// 项目名称
        /// </summary>
        public string EquipAccountCode
        {
            get { return this.GetProperty(EquipAccountCodeProperty); }
            set { this.SetProperty(EquipAccountCodeProperty, value); }
        }
        #endregion

        #region 设备名称 EquipAccountName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> EquipAccountNameProperty = P<DeviceBill>.RegisterView(e => e.EquipAccountName, p => p.EquipAccount.Name);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipAccountName
        {
            get { return this.GetProperty(EquipAccountNameProperty); }
            set { this.SetProperty(EquipAccountNameProperty, value); }
        }

        #endregion

        #region 设备型号编码 ModelCode
        /// <summary>
        /// 设备型号编码
        /// </summary>
        [Label("设备型号编码")]
        public static readonly Property<string> ModelCodeProperty = P<DeviceBill>.RegisterView(e => e.ModelCode, p => p.EquipAccount.EquipModel.Code);

        /// <summary>
        /// 设备型号编码
        /// </summary>
        public string ModelCode
        {
            get { return this.GetProperty(ModelCodeProperty); }
            set { SetProperty(ModelCodeProperty, value); }
        }
        #endregion 

        #region 设备型号名称 ModelName
        /// <summary>
        /// 设备型号名称
        /// </summary>
        [Label("设备型号名称")]
        public static readonly Property<string> ModelNameProperty = P<DeviceBill>.RegisterView(e => e.ModelName, p => p.EquipAccount.EquipModel.Name);

        /// <summary>
        /// 设备型号名称
        /// </summary>
        public string ModelName
        {
            get { return this.GetProperty(ModelNameProperty); }
            set { SetProperty(ModelNameProperty, value); }
        }
        #endregion

        #region 设备类型 EquipTypeCode
        /// <summary>
        /// 设备类型
        /// </summary>
        [Label("设备类型")]
        public static readonly Property<string> EquipTypeCodeProperty = P<DeviceBill>.RegisterView(e => e.EquipTypeCode, p => p.EquipAccount.EquipModel.EquipType.TypeCode);

        /// <summary>
        /// 设备类型
        /// </summary>
        public string EquipTypeCode
        {
            get { return this.GetProperty(EquipTypeCodeProperty); }
        }
        #endregion

        #region 设备类型名称 EquipTypeName
        /// <summary>
        /// 设备类型名称
        /// </summary>
        [Label("设备类型")]
        public static readonly Property<string> EquipTypeNameProperty = P<DeviceBill>.RegisterView(e => e.EquipTypeName, p => p.EquipAccount.EquipModel.EquipType.TypeName);

        /// <summary>
        /// 设备类型名称
        /// </summary>
        public string EquipTypeName
        {
            get { return this.GetProperty(EquipTypeNameProperty); }
        }
        #endregion

        #region 产线 Resource
        /// <summary>
        /// 产线
        /// </summary>
        [Label("产线")]
        public static readonly Property<string> ResourceProperty = P<DeviceBill>.RegisterView(e => e.Resource, p => p.EquipAccount.Resource.Name);

        /// <summary>
        /// 产线
        /// </summary>
        public string Resource
        {
            get { return this.GetProperty(ResourceProperty); }
            set { SetProperty(ResourceProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序
        /// </summary>
        [Label("工序")]
        public static readonly Property<string> ProcessProperty = P<DeviceBill>.RegisterView(e => e.Process, p => p.EquipAccount.Process.Name);

        /// <summary>
        /// 工序
        /// </summary>
        public string Process
        {
            get { return this.GetProperty(ProcessProperty); }
            set { SetProperty(ProcessProperty, value); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 设备清单 实体配置
    /// </summary>
    internal class DeviceBillConfig : EntityConfig<DeviceBill>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_DEVICE_BILL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}