using SIE.Common.Configs;
using SIE.Domain;
using SIE.Equipments.EquipTypes;
using SIE.ObjectModel;
using System;

namespace SIE.Equipments.Configs
{
    /// <summary>
    /// 位置列表维护条件配置值
    /// </summary>
    [RootEntity, Serializable]
    public class EquipModelLocationConfigValue : ConfigValue
    {
        /// <summary>
        /// 把配置值显示出来
        /// </summary>
        /// <returns>配置值</returns>
        public override string Display()
        {
            return EquipTypeName == null ? "NIL" : EquipTypeName.L10N();
        }

        #region 设备类型 EquipType
        /// <summary>
        /// 设备类型Id
        /// </summary>
        public static readonly IRefIdProperty EquipTypeIdProperty = P<EquipModelLocationConfigValue>.RegisterRefId(e => e.EquipTypeId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<EquipType> EquipTypeProperty = P<EquipModelLocationConfigValue>.RegisterRef(e => e.EquipType, EquipTypeIdProperty);

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
        public static readonly Property<string> EquipTypeIdsProperty = P<EquipModelLocationConfigValue>.Register(e => e.EquipTypeIds);

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
        public static readonly Property<string> EquipTypeCodeProperty = P<EquipModelLocationConfigValue>.RegisterView(e => e.EquipTypeCode, p => p.EquipType.TypeCode);

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
        public static readonly Property<string> EquipTypeNameProperty = P<EquipModelLocationConfigValue>.RegisterView(e => e.EquipTypeName, p => p.EquipType.TypeName);

        /// <summary>
        /// 设备类型名称
        /// </summary>
        public string EquipTypeName
        {
            get { return this.GetProperty(EquipTypeNameProperty); }
        }
        #endregion 

        #endregion
    }
}