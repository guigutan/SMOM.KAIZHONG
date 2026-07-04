using SIE.Common.Configs;
using SIE.Domain;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipTypes;
using SIE.ObjectModel;
using System;
using System.ComponentModel;

namespace SIE.Equipments.Configs
{
    /// <summary>
    /// 用于配置需具体设备类型的设备台账需维护位置列表信息
    /// </summary>
    [DisplayName("位置列表维护条件配置")]
    [Description("用于配置需具体设备类型的设备台账需维护位置列表信息")]
    [ConfigForEntity(typeof(EquipAccount))]
    public class EquipAccountsLocationConfig : ModuleConfig<EquipAccountsLocationConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly EquipAccountsLocationConfigValue defaultValue = new EquipAccountsLocationConfigValue();

        /// <summary>
        /// 默认值
        /// </summary>
        public override EquipAccountsLocationConfigValue DefaultValue
        {
            get
            {
                return defaultValue;
            }
        }
    }

    /// <summary>
    /// 设备台账编码
    /// </summary>
    [RootEntity, Serializable]
    [Label("设备台账编码")]
    public class EquipAccountsLocationConfigValue : ConfigValue
    {
        #region 设备类型 EquipType
        /// <summary>
        /// 设备类型Id
        /// </summary>
        public static readonly IRefIdProperty EquipTypeIdProperty = P<EquipAccountsLocationConfigValue>.RegisterRefId(e => e.EquipTypeId, ReferenceType.Normal);

        /// <summary>
        /// 设备类型Id
        /// </summary>
        public double EquipTypeId
        {
            get { return (double)GetRefId(EquipTypeIdProperty); }
            set { SetRefId(EquipTypeIdProperty, value); }
        }

        /// <summary>
        /// 设备类型
        /// </summary>
        public static readonly RefEntityProperty<EquipType> EquipTypeProperty = P<EquipAccountsLocationConfigValue>.RegisterRef(e => e.EquipType, EquipTypeIdProperty);

        /// <summary>
        /// 设备类型
        /// </summary>
        public EquipType EquipType
        {
            get { return GetRefEntity(EquipTypeProperty); }
            set { SetRefEntity(EquipTypeProperty, value); }
        }
        #endregion

        #region 设备类型ID集合 EquipTypeIds
        /// <summary>
        /// 设备类型ID集合
        /// </summary>
        [Label("设备类型ID集合")]
        public static readonly Property<string> EquipTypeIdsProperty = P<EquipAccountsLocationConfigValue>.Register(e => e.EquipTypeIds);

        /// <summary>
        /// 设备类型ID集合
        /// </summary>
        public string EquipTypeIds
        {
            get { return GetProperty(EquipTypeIdsProperty); }
            set { SetProperty(EquipTypeIdsProperty, value); }
        }
        #endregion

        #region 注册视图
        #region 设备类型编码 EquipTypeCode
        /// <summary>
        /// 设备类型编码
        /// </summary>
        [Label("设备类型编码")]
        public static readonly Property<string> EquipTypeCodeProperty = P<EquipAccountsLocationConfigValue>.RegisterView(e => e.EquipTypeCode, p => p.EquipType.TypeCode);

        /// <summary>
        /// 设备类型编码
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
        [Label("设备类型名称")]
        public static readonly Property<string> EquipTypeNameProperty = P<EquipAccountsLocationConfigValue>.RegisterView(e => e.EquipTypeName, p => p.EquipType.TypeName);

        /// <summary>
        /// 设备类型名称
        /// </summary>
        public string EquipTypeName
        {
            get { return this.GetProperty(EquipTypeNameProperty); }
        }
        #endregion 
        #endregion

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns>编码规则名称</returns>
        public override string Display()
        {
            if (EquipTypeIds == null)
                return "NIL";
            return EquipTypeIds;
        }
    }
}
