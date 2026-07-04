using SIE.Common.Configs;
using SIE.Domain;
using SIE.EMS.Faults.Configs;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Faults
{
    /// <summary>
    /// 故障类别
    /// </summary>
    [RootEntity, Serializable]
    [DisplayMember(nameof(Name))]
    //[ConditionQueryType(typeof(EquipLargeFaultCriteria))]
    [CriteriaQuery]
    [EntityWithConfig(typeof(EquipLargeFaultCodeConfig))]
    [Label("故障类别")]
    public partial class EquipLargeFault : DataEntity
    {
        #region 故障类别编码 Code
        /// <summary>
        /// 故障类别编码
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("故障类别编码")]
        public static readonly Property<string> CodeProperty = P<EquipLargeFault>.Register(e => e.Code);

        /// <summary>
        /// 故障类别编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 故障类别名称 Name
        /// <summary>
        /// 故障类别名称
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("故障类别名称")]
        public static readonly Property<string> NameProperty = P<EquipLargeFault>.Register(e => e.Name);

        /// <summary>
        /// 故障大类名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 故障中类 EquipMiddleFaultList
        /// <summary>
        /// 故障中类
        /// </summary>
        public static readonly ListProperty<EntityList<EquipMiddleFault>> MiddleFaultListProperty = P<EquipLargeFault>.RegisterList(e => e.MiddleFaultList);

        /// <summary>
        /// 故障中类
        /// </summary>
        public EntityList<EquipMiddleFault> MiddleFaultList
        {
            get { return this.GetLazyList(MiddleFaultListProperty); }
        }
        #endregion
    }

    /// <summary>
    /// 设备故障大类 实体配置
    /// </summary>
    internal class EquipLargeFaultConfig : EntityConfig<EquipLargeFault>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQUIP_LARGE_FAULT").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}