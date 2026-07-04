using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Items.Items
{
    /// <summary>
    /// 父级物料
    /// </summary>
    [ChildEntity, Serializable]
    [Label("父级物料")]
    public class ParentItem : DataEntity
    {
        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<ParentItem>.RegisterRefId(e => e.ItemId, ReferenceType.Parent);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get { return (double)this.GetRefId(ItemIdProperty); }
            set { this.SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty =
            P<ParentItem>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 上层物料号 ParentItemCode
        /// <summary>
        /// 上层物料号
        /// </summary>
        [Label("上层物料号")]
        public static readonly Property<string> ParentItemCodeProperty = P<ParentItem>.Register(e => e.ParentItemCode);

        /// <summary>
        /// 上层物料号
        /// </summary>
        public string ParentItemCode
        {
            get { return this.GetProperty(ParentItemCodeProperty); }
            set { this.SetProperty(ParentItemCodeProperty, value); }
        }
        #endregion

        #region 工厂 Werks
        /// <summary>
        /// 工厂
        /// </summary>
        [Label("工厂")]
        public static readonly Property<string> WerksProperty = P<ParentItem>.Register(e => e.Werks);

        /// <summary>
        /// 工厂
        /// </summary>
        public string Werks
        {
            get { return this.GetProperty(WerksProperty); }
            set { this.SetProperty(WerksProperty, value); }
        }
        #endregion

        #region 物料类型 Mtart
        /// <summary>
        /// 物料类型
        /// </summary>
        [Label("物料类型")]
        public static readonly Property<string> MtartProperty = P<ParentItem>.Register(e => e.Mtart);

        /// <summary>
        /// 物料类型
        /// </summary>
        public string Mtart
        {
            get { return this.GetProperty(MtartProperty); }
            set { this.SetProperty(MtartProperty, value); }
        }
        #endregion

        #region 上层旧物料号 Bismt
        /// <summary>
        /// 上层旧物料号
        /// </summary>
        [Label("上层旧物料号")]
        public static readonly Property<string> BismtProperty = P<ParentItem>.Register(e => e.Bismt);

        /// <summary>
        /// 上层旧物料号
        /// </summary>
        public string Bismt
        {
            get { return this.GetProperty(BismtProperty); }
            set { this.SetProperty(BismtProperty, value); }
        }
        #endregion

    }

    internal class ParentItemConfig : EntityConfig<ParentItem>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("PARENT_ITEM").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
