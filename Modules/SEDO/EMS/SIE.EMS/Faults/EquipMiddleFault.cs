using SIE.Common.Configs;
using SIE.Domain;
using SIE.EMS.Faults.Configs;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Faults
{
    /// <summary>
    /// 设备故障中类
    /// </summary>
    [ChildEntity, Serializable]
    //[CriteriaQuery]
    [Label("设备故障中类")]
    [DisplayMember(nameof(Name))]
    [EntityWithConfig(typeof(EquipMiddleFaultCodeConfig))]
    public partial class EquipMiddleFault : DataEntity
    {
        #region 故障中类编码 Code
        /// <summary>
        /// 故障中类编码
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("故障中类编码")]
        public static readonly Property<string> CodeProperty = P<EquipMiddleFault>.Register(e => e.Code);

        /// <summary>
        /// 故障中类编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 故障中类名称 Name
        /// <summary>
        /// 故障中类名称
        /// </summary>
        [Required]
        [Label("故障中类名称")]
        public static readonly Property<string> NameProperty = P<EquipMiddleFault>.Register(e => e.Name);

        /// <summary>
        /// 故障中类名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 故障大类 LargeFault
        /// <summary>
        /// 故障大类Id
        /// </summary>
        [Label("故障大类")]
        public static readonly IRefIdProperty LargeFaultIdProperty = P<EquipMiddleFault>.RegisterRefId(e => e.LargeFaultId, ReferenceType.Parent);

        /// <summary>
        /// 故障大类Id
        /// </summary>
        public double LargeFaultId
        {
            get { return (double)GetRefId(LargeFaultIdProperty); }
            set { SetRefId(LargeFaultIdProperty, value); }
        }

        /// <summary>
        /// 故障中类
        /// </summary>
        public static readonly RefEntityProperty<EquipLargeFault> LargeFaultProperty = P<EquipMiddleFault>.RegisterRef(e => e.LargeFault, LargeFaultIdProperty);

        /// <summary>
        /// 故障中类
        /// </summary>
        public EquipLargeFault LargeFault
        {
            get { return GetRefEntity(LargeFaultProperty); }
            set { SetRefEntity(LargeFaultProperty, value); }
        }
        #endregion

        #region 故障小类 SmallFaultList
        /// <summary>
        /// 故障小类
        /// </summary>
        public static readonly ListProperty<EntityList<EquipSmallFault>> SmallFaultListProperty = P<EquipMiddleFault>.RegisterList(e => e.SmallFaultList);
        /// <summary>
        /// 故障小类
        /// </summary>
        public EntityList<EquipSmallFault> SmallFaultList
        {
            get { return this.GetLazyList(SmallFaultListProperty); }
        }
        #endregion
    }

    /// <summary>
    /// 设备故障中类 实体配置
    /// </summary>
    internal class EquipMiddleFaultConfig : EntityConfig<EquipMiddleFault>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQUIP_MID_FAULT").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}