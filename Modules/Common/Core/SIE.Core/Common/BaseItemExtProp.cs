using SIE.Core.Items;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Core.Common
{
    /// <summary>
    /// 基本物料属性值
    /// </summary>
    [RootEntity, Serializable]
    [Label("基本扩展属性")]
    public partial class BaseItemExtProp : DataEntity
    {
        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<BaseItemExtProp>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
        }
        #endregion

        #region 值 Value
        /// <summary>
        /// 值
        /// </summary>
        [Required]
        [MaxLength(40)]
        [Label("值")]
        public static readonly Property<string> ValueProperty = P<BaseItemExtProp>.Register(e => e.Value);

        /// <summary>
        /// 值
        /// </summary>
        public string Value
        {
            get { return GetProperty(ValueProperty); }
            set { SetProperty(ValueProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料ID
        /// </summary>
        [Required]
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty = P<BaseItemExtProp>.RegisterRefId(e => e.ItemId, ReferenceType.Parent);

        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId
        {
            get { return (double)this.GetRefId(ItemIdProperty); }
            set { this.SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        [Label("物料")]
        public static readonly RefEntityProperty<Item> ItemProperty = P<BaseItemExtProp>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 外健 FId
        /// <summary>
        /// 外健
        /// </summary>
        public static readonly Property<double> FIdProperty = P<BaseItemExtProp>.Register(e => e.FId);

        /// <summary>
        /// 名称
        /// </summary>
        public double FId
        {
            get { return this.GetProperty(FIdProperty); }
            set { this.SetProperty(FIdProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 返回列表物料属性数据
    /// </summary>
    [Serializable]
    public class ItemExtPropData
    {
        /// <summary>
        /// 外键Id
        /// </summary>
        public double Fid { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 属性名值组合
        /// </summary>
        public string PropName { get; set; }

        /// <summary>
        /// Id值组合
        /// </summary>
        public string DefinitionValue { get; set; }
    }
}