using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Equipments.DeviceIOTParas.ConfigValues
{
    /// <summary>
    /// 设备WebApi地址配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("设备WebApi地址配置值")]
    public class EquipTagMDCConfigValue : ConfigValue
    {
        #region Url Url
        /// <summary>
        /// Url
        /// </summary>
        [Label("Url")]
        public static readonly Property<string> UrlProperty = P<EquipTagMDCConfigValue>.Register(e => e.Url);

        /// <summary>
        /// Url
        /// </summary>
        public string Url
        {
            get { return GetProperty(UrlProperty); }
            set { SetProperty(UrlProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示 
        /// </summary>
        /// <returns>Url</returns>
        public override string Display()
        {
            if (Url.IsNullOrEmpty())
                return "NIL";
            return Url;
        }
    }
}
