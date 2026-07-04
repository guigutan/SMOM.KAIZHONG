using SIE.Domain;
using SIE.Items;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Tech.ViewModels
{
    /// <summary>
    /// 工序BOM明细ViewModel
    /// </summary>
    [RootEntity, Serializable]
    [Label("工序BOM明细ViewModel")]
    public class BomDetailViewModel : ViewModel
    {
        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty = P<BomDetailViewModel>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get { return (double)GetRefId(ItemIdProperty); }
            set { SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        [Label("物料")]
        public static readonly RefEntityProperty<Item> ItemProperty = P<BomDetailViewModel>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<BomDetailViewModel>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return GetProperty(ItemCodeProperty); }
            set { SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<BomDetailViewModel>.Register(e => e.ItemName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return GetProperty(ItemNameProperty); }
            set { SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 单位精度  Precision 
        /// <summary>
        /// 单位精度
        /// </summary>
        [Label("单位精度")]
        public static readonly Property<int?> PrecisionProperty = P<BomDetailViewModel>.Register(e => e.Precision);

        /// <summary>
        /// 单位精度
        /// </summary>
        public int? Precision
        {
            get { return GetProperty(PrecisionProperty); }
            set { SetProperty(PrecisionProperty, value); }
        }
        #endregion

        #region 规格型号 SpecificationModel
        /// <summary>
        /// 规格型号
        /// </summary>
        [Label("规格型号")]
        public static readonly Property<string> SpecificationModelProperty = P<BomDetailViewModel>.Register(e => e.SpecificationModel);

        /// <summary>
        /// 规格型号
        /// </summary>
        public string SpecificationModel
        {
            get { return GetProperty(SpecificationModelProperty); }
            set { SetProperty(SpecificationModelProperty, value); }
        }
        #endregion

        #region 单位用量 UnitQty
        /// <summary>
        /// 单位用量
        /// </summary>
        [Label("单位用量")]
        public static readonly Property<decimal> UnitQtyProperty = P<BomDetailViewModel>.Register(e => e.UnitQty);

        /// <summary>
        /// 单位用量
        /// </summary>
        public decimal UnitQty
        {
            get { return GetProperty(UnitQtyProperty); }
            set { SetProperty(UnitQtyProperty, value); }
        }
        #endregion

        #region 单位 ItemUnitCode
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> ItemUnitCodeProperty = P<BomDetailViewModel>.Register(e => e.ItemUnitCode);

        /// <summary>
        /// 单位
        /// </summary>
        public string ItemUnitCode
        {
            get { return GetProperty(ItemUnitCodeProperty); }
            set { SetProperty(ItemUnitCodeProperty, value); }
        }
        #endregion

        #region 是否替代料 IsAltMaterial

        /// <summary>
        /// 是否替代料
        /// </summary>
        [Label("是否替代料")]
        public static readonly Property<bool> IsAltMaterialProperty = P<BomDetailViewModel>.Register(e => e.IsAltMaterial);

        /// <summary>
        /// 是否替代料
        /// </summary>
        public bool IsAltMaterial
        {
            get { return GetProperty(IsAltMaterialProperty); }
            set { SetProperty(IsAltMaterialProperty, value); }
        }
        #endregion

        #region 主料编码 MainMaterialCode
        /// <summary>
        /// 主料编码
        /// </summary>
        [Label("主料编码")]
        public static readonly Property<string> MainMaterialCodeProperty = P<BomDetailViewModel>.Register(e => e.MainMaterialCode);

        /// <summary>
        /// 主料编码
        /// </summary>
        public string MainMaterialCode
        {
            get { return GetProperty(MainMaterialCodeProperty); }
            set { SetProperty(MainMaterialCodeProperty, value); }
        }
        #endregion


        #region 物料属性值 ItemPropertyValue
        /// <summary>
        /// 物料属性值
        /// </summary>
        [Label("物料属性值")]
        public static readonly Property<string> ItemPropertyValueProperty = P<BomDetailViewModel>.Register(e => e.ItemPropertyValue);

        /// <summary>
        /// 物料属性值
        /// </summary>
        public string ItemPropertyValue
        {
            get { return GetProperty(ItemPropertyValueProperty); }
            set { SetProperty(ItemPropertyValueProperty, value); }
        }
        #endregion

        #region 工段Id ProcessSegmentId
        /// <summary>
        /// 工段Id
        /// </summary>
        [Label("工段Id")]
        public static readonly Property<double?> ProcessSegmentIdProperty = P<BomDetailViewModel>.Register(e => e.ProcessSegmentId);

        /// <summary>
        /// 工段Id
        /// </summary>
        public double? ProcessSegmentId
        {
            get { return GetProperty(ProcessSegmentIdProperty); }
            set { SetProperty(ProcessSegmentIdProperty, value); }
        }
        #endregion

        #region 工段 ProcessSegmentName
        /// <summary>
        /// 工段
        /// </summary>
        [Label("工段")]
        public static readonly Property<string> ProcessSegmentNameProperty = P<BomDetailViewModel>.Register(e => e.ProcessSegmentName);

        /// <summary>
        /// 制程工艺
        /// </summary>
        public string ProcessSegmentName
        {
            get { return GetProperty(ProcessSegmentNameProperty); }
            set { SetProperty(ProcessSegmentNameProperty, value); }
        }
        #endregion

        #region 是否拆分 IsSplit
        /// <summary>
        /// 是否拆分
        /// </summary>
        [Label("是否拆分")]
        public static readonly Property<bool?> IsSplitProperty = P<BomDetailViewModel>.Register(e => e.IsSplit);

        /// <summary>
        /// 是否拆分
        /// </summary>
        public bool? IsSplit
        {
            get { return GetProperty(IsSplitProperty); }
            set { SetProperty(IsSplitProperty, value); }
        }
        #endregion

        #region BomDetailId BomDetailId
        /// <summary>
        /// BomDetailId
        /// </summary>
        [Label("BomDetailId")]
        public static readonly Property<double> BomDetailIdProperty = P<BomDetailViewModel>.Register(e => e.BomDetailId);

        /// <summary>
        /// BomDetailId
        /// </summary>
        public double BomDetailId
        {
            get { return GetProperty(BomDetailIdProperty); }
            set { SetProperty(BomDetailIdProperty, value); }
        }
        #endregion

        #region 损耗率 LossRate
        /// <summary>
        /// 损耗率
        /// </summary>
        [Label("损耗率")]
        public static readonly Property<decimal> LossRateProperty = P<BomDetailViewModel>.Register(e => e.LossRate);

        /// <summary>
        /// 损耗率
        /// </summary>
        public decimal LossRate
        {
            get { return GetProperty(LossRateProperty); }
            set { SetProperty(LossRateProperty, value); }
        }
        #endregion
    }
}
