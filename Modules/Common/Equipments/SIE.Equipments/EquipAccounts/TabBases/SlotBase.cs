using SIE.Domain;
using SIE.Equipments.Enums;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Equipments.EquipAccounts.TabBases
{
    /// <summary>
    /// 台账缸槽基类
    /// </summary>
    [RootEntity, Serializable]
    //[CriteriaQuery]
    [Label("台账缸槽基类")]
    [DisplayMember(nameof(Code))]
    public partial class SlotBase : DataEntity
    {
        #region 缸槽编码 Code
        /// <summary>
        /// 缸槽编码
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("缸槽编码")]
        public static readonly Property<string> CodeProperty = P<SlotBase>.Register(e => e.Code);

        /// <summary>
        /// 缸槽编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 缸槽名称 Name
        /// <summary>
        /// 缸槽名称
        /// </summary>
        [Required]
        [Label("缸槽名称")]
        public static readonly Property<string> NameProperty = P<SlotBase>.Register(e => e.Name);

        /// <summary>
        /// 缸槽名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 缸槽体积 Volume
        /// <summary>
        /// 缸槽体积
        /// </summary>
        [Required]
        [Label("缸槽体积")]
        public static readonly Property<decimal?> VolumeProperty = P<SlotBase>.Register(e => e.Volume);

        /// <summary>
        /// 缸槽体积
        /// </summary>
        public decimal? Volume
        {
            get { return GetProperty(VolumeProperty); }
            set { SetProperty(VolumeProperty, value); }
        }
        #endregion

        #region 描述 Description
        /// <summary>
        /// 描述
        /// </summary>
        [Label("描述")]
        public static readonly Property<string> DescriptionProperty = P<SlotBase>.Register(e => e.Description);

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }
        #endregion

        #region 添加方式 ChemicalAddWay
        /// <summary>
        /// 添加方式
        /// </summary>
        [Required]
        [Label("添加方式")]
        public static readonly Property<ChemicalAddWay> ChemicalAddWayProperty = P<SlotBase>.Register(e => e.ChemicalAddWay);

        /// <summary>
        /// 添加方式
        /// </summary>
        public ChemicalAddWay ChemicalAddWay
        {
            get { return GetProperty(ChemicalAddWayProperty); }
            set { SetProperty(ChemicalAddWayProperty, value); }
        }
        #endregion

        #region 单位 Unit
        /// <summary>
        /// 单位Id
        /// </summary>
        [Label("单位")]
        public static readonly IRefIdProperty UnitIdProperty = P<SlotBase>.RegisterRefId(e => e.UnitId, ReferenceType.Normal);

        /// <summary>
        /// 单位Id
        /// </summary>
        public double UnitId
        {
            get { return (double)GetRefId(UnitIdProperty); }
            set { SetRefId(UnitIdProperty, value); }
        }

        /// <summary>
        /// 单位
        /// </summary>
        public static readonly RefEntityProperty<Unit> UnitProperty = P<SlotBase>.RegisterRef(e => e.Unit, UnitIdProperty);

        /// <summary>
        /// 单位
        /// </summary>        
        public Unit Unit
        {
            get { return GetRefEntity(UnitProperty); }
            set { SetRefEntity(UnitProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 缸槽 实体配置
    /// </summary>
    internal class SlotBaseConfig : EntityConfig<SlotBase>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("PCB_SLOT").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}