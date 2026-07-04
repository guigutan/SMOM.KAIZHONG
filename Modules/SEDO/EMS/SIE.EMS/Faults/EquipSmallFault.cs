using SIE.Common.Configs;
using SIE.Domain;
using SIE.EMS.Faults.Configs;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Faults
{
    /// <summary>
    /// 设备故障小类
    /// </summary>
    [ChildEntity, Serializable]
    //[CriteriaQuery]
    [Label("设备故障小类")]
    [DisplayMember(nameof(Name))]
    [EntityWithConfig(typeof(EquipSmallFaultCodeConfig))]
    public partial class EquipSmallFault : DataEntity
    {
        #region 故障小类编码 Code
        /// <summary>
        /// 故障小类编码
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("故障小类编码")]
        public static readonly Property<string> CodeProperty = P<EquipSmallFault>.Register(e => e.Code);

        /// <summary>
        /// 故障小类编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 故障小类名称 Name
        /// <summary>
        /// 故障小类名称
        /// </summary>
        [Required]
        [Label("故障小类名称")]
        public static readonly Property<string> NameProperty = P<EquipSmallFault>.Register(e => e.Name);

        /// <summary>
        /// 故障小类名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 说明 Description
        /// <summary>
        /// 说明
        /// </summary>
        [Label("说明")]
        public static readonly Property<string> DescriptionProperty = P<EquipSmallFault>.Register(e => e.Description);

        /// <summary>
        /// 说明
        /// </summary>
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }
        #endregion

        #region 故障中类 MiddleFault
        /// <summary>
        /// 故障中类Id
        /// </summary>
        [Label("故障中类")]
        public static readonly IRefIdProperty MiddleFaultIdProperty = P<EquipSmallFault>.RegisterRefId(e => e.MiddleFaultId, ReferenceType.Parent);

        /// <summary>
        /// 故障中类Id
        /// </summary>
        public double MiddleFaultId
        {
            get { return (double)GetRefId(MiddleFaultIdProperty); }
            set { SetRefId(MiddleFaultIdProperty, value); }
        }

        /// <summary>
        /// 故障小类
        /// </summary>
        public static readonly RefEntityProperty<EquipMiddleFault> MiddleFaultProperty = P<EquipSmallFault>.RegisterRef(e => e.MiddleFault, MiddleFaultIdProperty);

        /// <summary>
        /// 故障小类
        /// </summary>
        public EquipMiddleFault MiddleFault
        {
            get { return GetRefEntity(MiddleFaultProperty); }
            set { SetRefEntity(MiddleFaultProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 设备故障小类 实体配置
    /// </summary>
    internal class EquipSmallFaultConfig : EntityConfig<EquipSmallFault>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQUIP_SMALL_FAULT").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}