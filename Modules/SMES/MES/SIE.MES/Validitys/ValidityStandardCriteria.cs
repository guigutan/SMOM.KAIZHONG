using SIE.Domain;
using SIE.Items;
using SIE.MES.Validitys.Service;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Validitys
{
    /// <summary>
    /// 有效期标准维护查询
    /// </summary>
    [QueryEntity, Serializable]
    [Label("有效期标准维护查询")]
    public class ValidityStandardCriteria : Criteria
    {
        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<ValidityStandardCriteria>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
            P<ValidityStandardCriteria>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 物料类型 ItemType
        /// <summary>
        /// 物料类型
        /// </summary>
        [Label("物料类型")]
        public static readonly Property<ItemType?> ItemTypeProperty = P<ValidityStandardCriteria>.Register(e => e.ItemType);

        /// <summary>
        /// 物料类型
        /// </summary>
        public ItemType? ItemType
        {
            get { return this.GetProperty(ItemTypeProperty); }
            set { this.SetProperty(ItemTypeProperty, value); }
        }
        #endregion

        #region 生效日期 Effective
        /// <summary>
        /// 生效日期
        /// </summary>
        [Label("生效日期")]
        public static readonly Property<DateRange> EffectiveProperty = P<ValidityStandardCriteria>.Register(e => e.Effective);

        /// <summary>
        /// 生效日期
        /// </summary>
        public DateRange Effective
        {
            get { return this.GetProperty(EffectiveProperty); }
            set { this.SetProperty(EffectiveProperty, value); }
        }
        #endregion

        #region 失效日期 Expiration
        /// <summary>
        /// 失效日期
        /// </summary>
        [Label("失效日期")]
        public static readonly Property<DateRange> ExpirationProperty = P<ValidityStandardCriteria>.Register(e => e.Expiration);

        /// <summary>
        /// 失效日期
        /// </summary>
        public DateRange Expiration
        {
            get { return this.GetProperty(ExpirationProperty); }
            set { this.SetProperty(ExpirationProperty, value); }
        }
        #endregion


        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<ValidityService>().QueryValidityStandards(this);
        }
    }
}
