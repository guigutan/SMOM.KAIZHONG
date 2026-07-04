using SIE.Core.Enums;
using SIE.Domain;
using SIE.Items;
using SIE.ObjectModel;
using System;

namespace SIE.ShipPlan
{
    /// <summary>
    /// 查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("分配仓库规则查询")]
    public class AssignWarehouseRuleCriteria : Criteria
    {
        #region 单据类型 OrderType
        /// <summary>
        /// 单据类型
        /// </summary>
        [Label("单据类型")]
        public static readonly Property<OrderType?> OrderTypeProperty = P<AssignWarehouseRuleCriteria>.Register(e => e.OrderType);

        /// <summary>
        /// 单据类型
        /// </summary>
        public OrderType? OrderType
        {
            get { return GetProperty(OrderTypeProperty); }
            set { SetProperty(OrderTypeProperty, value); }
        }
        #endregion

        #region 基本分类 ItemType
        /// <summary>
        /// 基本分类
        /// </summary>
        [Label("基本分类")]
        public static readonly Property<ItemType?> ItemTypeProperty = P<AssignWarehouseRuleCriteria>.Register(e => e.ItemType);

        /// <summary>
        /// 基本分类
        /// </summary>
        public ItemType? ItemType
        {
            get { return GetProperty(ItemTypeProperty); }
            set { SetProperty(ItemTypeProperty, value); }
        }
        #endregion

        #region 库存类别 ItemCategory
        /// <summary>
        /// 库存类别Id
        /// </summary>
        [Label("库存类别")]
        public static readonly IRefIdProperty ItemCategoryIdProperty = P<AssignWarehouseRuleCriteria>.RegisterRefId(e => e.ItemCategoryId, ReferenceType.Normal);

        /// <summary>
        /// 库存类别Id
        /// </summary>
        public double? ItemCategoryId
        {
            get { return (double?)GetRefNullableId(ItemCategoryIdProperty); }
            set { SetRefNullableId(ItemCategoryIdProperty, value); }
        }

        /// <summary>
        /// 库存类别
        /// </summary>
        public static readonly RefEntityProperty<ItemCategory> ItemCategoryProperty = P<AssignWarehouseRuleCriteria>.RegisterRef(e => e.ItemCategory, ItemCategoryIdProperty);

        /// <summary>
        /// 库存分类
        /// </summary>
        public ItemCategory ItemCategory
        {
            get { return GetRefEntity(ItemCategoryProperty); }
            set { SetRefEntity(ItemCategoryProperty, value); }
        }
        #endregion

        /// <summary>
        /// 重写此方法实现查询
        /// </summary>
        /// <returns>返回结果</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<AssignWarehouseRuleController>().GetAssignWarehouseRules(this);
        }
    }
}
