using SIE.Common.Configs;
using SIE.Domain;
using SIE.Equipments.EquipTypes;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.IdleArchives.Configs
{
    /// <summary>
    /// 闲置封存-闲置最长期限配置项
    /// </summary>
    [System.ComponentModel.DisplayName("启用时需要保养的设备类型配置项")]
    [System.ComponentModel.Description("启用时需要保养的设备类型配置项,具体规则详细请在配置项中进行配置")]
    public class MaintainedEquipmentTypeConfig : ModuleConfig<MaintainedEquipmentTypeConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly MaintainedEquipmentTypeConfigValue defaultValue = new MaintainedEquipmentTypeConfigValue();

        /// <summary>
        /// 默认值
        /// </summary>
        public override MaintainedEquipmentTypeConfigValue DefaultValue
        {
            get
            {
                return defaultValue;
            }
        }
    }

    /// <summary>
    /// 设备台账启用固定资产配置
    /// </summary>
    [RootEntity, Serializable]
    [Label("启用时需要保养的设备类型配置项")]
    public class MaintainedEquipmentTypeConfigValue : ConfigValue
    {
        #region
        /// <summary>
        /// 设备类型Id
        /// </summary>
        public static readonly IRefIdProperty EquipTypeIdProperty = P<MaintainedEquipmentTypeConfigValue>.RegisterRefId(e => e.EquipTypeId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<EquipType> EquipTypeProperty = P<MaintainedEquipmentTypeConfigValue>.RegisterRef(e => e.EquipType, EquipTypeIdProperty);

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
        public static readonly Property<string> EquipTypeIdsProperty = P<MaintainedEquipmentTypeConfigValue>.Register(e => e.EquipTypeIds);

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
        public static readonly Property<string> EquipTypeCodeProperty = P<MaintainedEquipmentTypeConfigValue>.RegisterView(e => e.EquipTypeCode, p => p.EquipType.TypeCode);

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
        [Label("启用时需要保养的设备类型")]
        public static readonly Property<string> EquipTypeNameProperty = P<MaintainedEquipmentTypeConfigValue>.RegisterView(e => e.EquipTypeName, p => p.EquipType.TypeName);

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
        /// <returns></returns>
        public override string Display()
        {
            if (EquipTypeIds == null)
                return "NIL";
            return "启用时需要保养的设备类型:{0}".L10nFormat(EquipTypeIds);
        }
    }
}
