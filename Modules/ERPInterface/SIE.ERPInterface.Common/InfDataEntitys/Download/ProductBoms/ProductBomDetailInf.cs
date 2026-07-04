using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.ERPInterface.Common.InfDataEntitys.Download
{
    /// <summary>
    /// 产品BOM明细中间表
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("产品BOM明细中间表")]
    public partial class ProductBomDetailInf : DownloadBaseEntity
    {
        #region 损耗率 LossRate
        /// <summary>
        /// 损耗率
        /// </summary>
        [Label("损耗率")]
        public static readonly Property<decimal> LossRateProperty = P<ProductBomDetailInf>.Register(e => e.LossRate);

        /// <summary>
        /// 损耗率
        /// </summary>
        public decimal LossRate
        {
            get { return GetProperty(LossRateProperty); }
            set { SetProperty(LossRateProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(1000)]
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<ProductBomDetailInf>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 单位耗用量 UnitQty
        /// <summary>
        /// 单位耗用量
        /// </summary>
        [Label("单位耗用量")]
        public static readonly Property<decimal> UnitQtyProperty = P<ProductBomDetailInf>.Register(e => e.UnitQty);

        /// <summary>
        /// 单位耗用量
        /// </summary>
        public decimal UnitQty
        {
            get { return GetProperty(UnitQtyProperty); }
            set { SetProperty(UnitQtyProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<ProductBomDetailInf>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return GetProperty(ItemCodeProperty); }
            set { SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 产品BOM编码 ProductBomCode
        /// <summary>
        /// 产品BOM编码
        /// </summary>
        [Label("产品BOM编码")]
        public static readonly Property<string> ProductBomCodeProperty = P<ProductBomDetailInf>.Register(e => e.ProductBomCode);

        /// <summary>
        /// 产品BOM编码
        /// </summary>
        public string ProductBomCode
        {
            get { return GetProperty(ProductBomCodeProperty); }
            set { SetProperty(ProductBomCodeProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 产品BOM明细中间表 实体配置
    /// </summary>
    internal class ProductBomDetailInfConfig : EntityConfig<ProductBomDetailInf>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("INF_PROD_BOM_DTL").MapAllProperties();
            Meta.Property(ProductBomDetailInf.RemarkProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }
    }
}