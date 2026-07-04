using SIE.Core.Items;
using SIE.CSM.ItemInspCharacteristicses;
using SIE.Domain;
using SIE.KZ.Base.Interfaces;
using SIE.KZ.Base.Interfaces.Enums;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ItemCPN
{
    /// <summary>
    /// 物料客户料码查询
    /// </summary>
    [QueryEntity, Serializable]
    [Label("物料客户料码查询")]
    public class ItemCustomerPartNoCriteria : Criteria
    {
        #region 物料编码 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料编码")]
        public static readonly IRefIdProperty ItemIdProperty = P<ItemCustomerPartNoCriteria>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double? ItemId
        {
            get { return (double?)GetRefNullableId(ItemIdProperty); }
            set { SetRefNullableId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料编码
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty = P<ItemCustomerPartNoCriteria>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料编码
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<ItemCustomerPartNoCriteria>.Register(e => e.ItemName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return GetProperty(ItemNameProperty); }
            set { SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<ItemCustomerPartNoCriteria>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return GetProperty(ItemCodeProperty); }
            set { SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 客户 Customer
        /// <summary>
        /// 客户
        /// </summary>
        [Label("客户")]
        public static readonly Property<string> CustomerProperty =
            P<ItemCustomerPartNoCriteria>.Register(e => e.Customer);

        /// <summary>
        /// 客户
        /// </summary>
        public string Customer
        {
            get { return this.GetProperty(CustomerProperty); }
            set { this.SetProperty(CustomerProperty, value); }
        }
        #endregion

        #region 客户料号 CodeAlias
        /// <summary>
        /// 客户料号
        /// </summary>
        [Label("客户料号")]
        public static readonly Property<string> CodeAliasProperty =
            P<ItemCustomerPartNoCriteria>.Register(e => e.CodeAlias);

        /// <summary>
        /// 客户料号
        /// </summary>
        public string CodeAlias
        {
            get { return this.GetProperty(CodeAliasProperty); }
            set { this.SetProperty(CodeAliasProperty, value); }
        }
        #endregion

        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<ItemCustomerPartNoController>().GetItemCusotmerDataAll(this);
        }
    }
}