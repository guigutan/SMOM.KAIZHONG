using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.LES
{
    /// <summary>
    /// LES批次与WMS批次共用一个表，这里是为了抽出来LES用
    /// </summary>
    [RootEntity, Serializable]
    [DisplayMember(nameof(LesLot.Code))]
    [Label("批次")]
    public partial class LesLot : DataEntity
    {
        /// <summary>
        /// 默认批次编号（无批次管理）
        /// </summary>
        public const string LotDefault = "LotDefault";

        #region 批次 Code
        /// <summary>
        /// 批次
        /// </summary>
        [Label("批次")]
        [MaxLength(80)]
        [NotDuplicate]
        public static readonly Property<string> CodeProperty = P<LesLot>.Register(e => e.Code);

        /// <summary>
        /// 批次
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty = P<LesLot>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double? ItemId
        {
            get { return (double?)GetRefNullableId(ItemIdProperty); }
            set { SetRefNullableId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty = P<LesLot>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary> 
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 物料扩展属性 ItemExtProp
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [Label("物料扩展属性")]
        [MaxLength(120)]
        public static readonly Property<string> ItemExtPropProperty = P<LesLot>.Register(e => e.ItemExtProp);

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp
        {
            get { return this.GetProperty(ItemExtPropProperty); }
            set { this.SetProperty(ItemExtPropProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 批次 实体配置
    /// </summary>
    internal class LesLotConfig : EntityConfig<LesLot>
    {
        /// <summary>
        /// 子类重写此方法，并完成对 Meta 属性的配置。
        /// 注意：
        /// * 为了给当前类的子类也运行同样的配置，这个方法可能会被调用多次。
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("LOT").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}