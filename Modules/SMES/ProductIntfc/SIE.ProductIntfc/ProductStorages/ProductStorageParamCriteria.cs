using SIE.Domain;
using SIE.Items;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IronPython.Modules._ast;

namespace SIE.ProductIntfc.ProductStorages
{
    [QueryEntity,Serializable]
    public class ProductStorageParamCriteria : Criteria
    {
        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<ProductStorageParamCriteria>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double? ItemId
        {
            get { return (double?)this.GetRefNullableId(ItemIdProperty); }
            set { this.SetRefNullableId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty =
            P<ProductStorageParamCriteria>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 产品类型 ItemType
        /// <summary>
        /// 产品类型
        /// </summary>
        [Label("产品类型")]
        public static readonly Property<ItemType?> ItemTypeProperty = P<ProductStorageParamCriteria>.Register(e => e.ItemType);

        /// <summary>
        /// 产品类型
        /// </summary>
        public ItemType? ItemType
        {
            get { return this.GetProperty(ItemTypeProperty); }
            set { this.SetProperty(ItemTypeProperty, value); }
        }
        #endregion

        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<ProductStorageParamController>().GetProductStorageParams(this);
        }
    }
}
