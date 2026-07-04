using SIE.Common.Catalogs;
using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Linq;

namespace SIE.Equipments.Configs
{
    /// <summary>
    /// 维护类别配置值
    /// </summary>
    [RootEntity, Serializable]
    public class EquipModelEquipmentCategoryConfigValue : ConfigValue
    {
        /// <summary>
        /// 把配置值显示出来
        /// </summary>
        /// <returns>配置值</returns>
        public override string Display()
        {
            string display;

            if (SpecialIds.IsNullOrEmpty())
            {
                display = "特殊设备类别:NIL;".L10N();
            }
            else
            {
                display = "特殊设备类别:{0}".L10nFormat(SpecialCategoryName);
            }

            if (EquipmentMeteringIds.IsNullOrEmpty())
            {
                display += ",计量设备类别:NIL;".L10N();
            }
            else
            {
                display += "计量设备类别:{0}".L10nFormat(EquipmentMeteringName);
            }

            return display;
        }

        #region 特种设备名称 SpecialCategoryName
        /// <summary>
        /// 特种设备名称
        /// </summary>
        [Label("属性名")]
        public static readonly Property<string> SpecialCategoryNameProperty = P<EquipModelEquipmentCategoryConfigValue>.Register(e => e.SpecialCategoryName);

        /// <summary>
        /// 特种设备名称
        /// </summary>
        public string SpecialCategoryName
        {
            get { return this.GetProperty(SpecialCategoryNameProperty); }
            set { this.SetProperty(SpecialCategoryNameProperty, value); }
        }
        #endregion

        #region 计量设备名称 EquipmentMeteringName
        /// <summary>
        /// 计量设备名称
        /// </summary>
        [Label("计量设备名称")]
        public static readonly Property<string> EquipmentMeteringNameProperty = P<EquipModelEquipmentCategoryConfigValue>.Register(e => e.EquipmentMeteringName);

        /// <summary>
        /// 计量设备名称
        /// </summary>
        public string EquipmentMeteringName
        {
            get { return this.GetProperty(EquipmentMeteringNameProperty); }
            set { this.SetProperty(EquipmentMeteringNameProperty, value); }
        }
        #endregion

        #region 特种设备类别集合 SpecialCategorys
        /// <summary>
        /// 特种设备类别集合
        /// </summary>
        [Label("特种设备类别集合")]
        public static readonly Property<string> SpecialIdsProperty = P<EquipModelEquipmentCategoryConfigValue>.Register(e => e.SpecialIds);

        /// <summary>
        /// 特种设备类别集合
        /// </summary>
        public string SpecialIds
        {
            get { return GetProperty(SpecialIdsProperty); }
            set { SetProperty(SpecialIdsProperty, value); }
        }
        #endregion

        #region 计量设备类别集合 EquipmentMeterings
        /// <summary>
        /// 计量设备类别集合
        /// </summary>
        [Label("计量设备类别集合")]
        public static readonly Property<string> EquipmentMeteringIdsProperty = P<EquipModelEquipmentCategoryConfigValue>.Register(e => e.EquipmentMeteringIds);

        /// <summary>
        /// 计量设备类别集合
        /// </summary>
        public string EquipmentMeteringIds
        {
            get { return GetProperty(EquipmentMeteringIdsProperty); }
            set { SetProperty(EquipmentMeteringIdsProperty, value); }
        }
        #endregion

    }
}