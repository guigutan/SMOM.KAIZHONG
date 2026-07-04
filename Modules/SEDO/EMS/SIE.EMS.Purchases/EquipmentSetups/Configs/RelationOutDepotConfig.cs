using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.ComponentModel;

namespace SIE.EMS.Purchases.EquipmentSetups.Configs
{
    /// <summary>
    /// 备件使用是否必须关联出库单
    /// </summary>
    [DisplayName("备件使用是否必须关联出库单")]
    [Description("用于配置备件使用是否必须关联出库单")]
    public class RelationOutDepotConfig : ModuleConfig<RelationOutDepotConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly RelationOutDepotConfigValue defaultValue = new RelationOutDepotConfigValue();

        /// <summary>
        /// 默认值
        /// </summary>
        public override RelationOutDepotConfigValue DefaultValue
        {
            get
            {
                return defaultValue;
            }
        }
    }

    /// <summary>
    /// 备件使用是否必须关联出库单
    /// </summary>
    [RootEntity, Serializable]
    public class RelationOutDepotConfigValue : ConfigValue
    {
        #region 是否关联 Relation
        /// <summary>
        /// 是否关联
        /// </summary>
        [Label("备件使用是否必须关联出库单")]
        public static readonly Property<bool> RelationProperty = P<RelationOutDepotConfigValue>.Register(e => e.Relation);

        /// <summary>
        /// 是否关联
        /// </summary>
        public bool Relation
        {
            get { return this.GetProperty(RelationProperty); }
            set { this.SetProperty(RelationProperty, value); }
        }
        #endregion
    }
}
