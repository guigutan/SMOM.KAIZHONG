using SIE.Domain;
using SIE.Items;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Purchases.PurchaseRequisitions
{
    /// <summary>
    /// 采购对象编码
    /// </summary>
    [RootEntity, Serializable]
    [Label("采购对象编码")]
    [DisplayMember(nameof(Value))]
    public class ObjectCodeInfo : ViewModel
    {
        #region 编码 Value
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> ValueProperty = P<ObjectCodeInfo>.Register(e => e.Value);

        /// <summary>
        /// 编码
        /// </summary>
        public string Value
        {
            get { return GetProperty(ValueProperty); }
            set { SetProperty(ValueProperty, value); }
        }
        #endregion

        #region 型号编码 ModelCode
        /// <summary>
        /// 型号编码
        /// </summary>
        [Label("型号编码")]
        public static readonly Property<string> ModelCodeProperty = P<ObjectCodeInfo>.Register(e => e.ModelCode);

        /// <summary>
        /// 型号编码
        /// </summary>
        public string ModelCode
        {
            get { return this.GetProperty(ModelCodeProperty); }
            set { this.SetProperty(ModelCodeProperty, value); }
        }
        #endregion

        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<ObjectCodeInfo>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
        }
        #endregion

        #region 规格型号 Specification
        /// <summary>
        /// 规格型号
        /// </summary>
        [Label("规格型号")]
        public static readonly Property<string> SpecificationProperty = P<ObjectCodeInfo>.Register(e => e.Specification);

        /// <summary>
        /// 规格型号
        /// </summary>
        public string Specification
        {
            get { return this.GetProperty(SpecificationProperty); }
            set { this.SetProperty(SpecificationProperty, value); }
        }
        #endregion

        #region 单位 ItemUnit
        /// <summary>
        /// 单位Id
        /// </summary>
        public static readonly IRefIdProperty ItemUnitIdProperty = P<ObjectCodeInfo>.RegisterRefId(e => e.ItemUnitId, ReferenceType.Normal);

        /// <summary>
        /// 单位Id
        /// </summary>
        public double? ItemUnitId
        {
            get { return (double?)GetRefNullableId(ItemUnitIdProperty); }
            set { SetRefNullableId(ItemUnitIdProperty, value); }
        }

        /// <summary>
        /// 单位
        /// </summary>
        public static readonly RefEntityProperty<Unit> ItemUnitProperty = P<ObjectCodeInfo>.RegisterRef(e => e.ItemUnit, ItemUnitIdProperty);

        /// <summary>
        /// 单位
        /// </summary>
        public Unit ItemUnit
        {
            get { return GetRefEntity(ItemUnitProperty); }
            set { SetRefEntity(ItemUnitProperty, value); }
        }
        #endregion

        #region 单位名称 ItemUnitNmae
        /// <summary>
        /// 单位名称
        /// </summary>
        [Label("单位名称")]
        public static readonly Property<string> ItemUnitNmaeProperty = P<ObjectCodeInfo>.Register(e => e.ItemUnitNmae);

        /// <summary>
        /// 单位名称
        /// </summary>
        public string ItemUnitNmae
        {
            get { return this.GetProperty(ItemUnitNmaeProperty); }
            set { this.SetProperty(ItemUnitNmaeProperty, value); }
        }
        #endregion
    }
}
