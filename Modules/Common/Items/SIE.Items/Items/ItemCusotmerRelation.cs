using SIE.Core.Algorithms.KZ;
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
    /// 客户料码数据
    /// </summary>
    [ChildEntity, Serializable]
    [Label("客户料码数据")]
    public class ItemCusotmerRelation : ItemCusotmerData
    {
        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static new readonly IRefIdProperty ItemIdProperty =
            P<ItemCusotmerRelation>.RegisterRefId(e => e.ItemId, ReferenceType.Parent);

        /// <summary>
        /// 物料Id
        /// </summary>
        public new double ItemId
        {
            get { return (double)this.GetRefId(ItemIdProperty); }
            set { this.SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static new readonly RefEntityProperty<Item> ItemProperty =
            P<ItemCusotmerRelation>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public new Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion


    }

    internal class ItemCusotmerRelationConfig : EntityConfig<ItemCusotmerRelation>
    {
        protected override void ConfigMeta()
        {
            base.ConfigMeta();
        }
    }
}
