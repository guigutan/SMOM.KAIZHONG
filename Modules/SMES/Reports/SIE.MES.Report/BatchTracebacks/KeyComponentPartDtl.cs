using SIE.Domain;
using SIE.MES.WIP.Products;
using SIE.MetaModel;
using SIE.ObjectModel;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Report.BatchTracebacks
{
    /// <summary>
    /// 生产采集记录
    /// </summary>
    [RootEntity, Serializable]
    [Label("关键件记录")]
    public class KeyComponentPartDtl : DataEntity
    {
        #region 批次号 BacthNo
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> BacthNoProperty = P<KeyComponentPartDtl>.Register(e => e.BacthNo);

        /// <summary>
        /// 批次号
        /// </summary>
        public string BacthNo
        {
            get { return this.GetProperty(BacthNoProperty); }
            set { this.SetProperty(BacthNoProperty, value); }
        }
        #endregion

        #region 工序 ProcessCode
        /// <summary>
        /// 工序
        /// </summary>
        [Label("工序")]
        public static readonly Property<string> ProcessCodeProperty = P<KeyComponentPartDtl>.Register(e => e.ProcessCode);

        /// <summary>
        /// 工序
        /// </summary>
        public string ProcessCode
        {
            get { return this.GetProperty(ProcessCodeProperty); }
            set { this.SetProperty(ProcessCodeProperty, value); }
        }
        #endregion

        #region 来源条码 SourceSn
        /// <summary>
        /// 来源条码
        /// </summary>
        [Label("来源条码")]
        public static readonly Property<string> SourceSnProperty = P<KeyComponentPartDtl>.Register(e => e.SourceSn);

        /// <summary>
        /// 来源条码
        /// </summary>
        public string SourceSn
        {
            get { return this.GetProperty(SourceSnProperty); }
            set { this.SetProperty(SourceSnProperty, value); }
        }
        #endregion

        #region 批次标签 ItemLabelLot
        /// <summary>
        /// 批次标签
        /// </summary>
        [Label("批次标签")]
        public static readonly Property<string> ItemLabelLotProperty = P<KeyComponentPartDtl>.Register(e => e.ItemLabelLot);

        /// <summary>
        /// 批次标签
        /// </summary>
        public string ItemLabelLot
        {
            get { return this.GetProperty(ItemLabelLotProperty); }
            set { this.SetProperty(ItemLabelLotProperty, value); }
        }
        #endregion

        #region 用料量 DeductedQty
        /// <summary>
        /// 用料量
        /// </summary>
        [Label("用料量")]
        public static readonly Property<decimal> DeductedQtyProperty = P<KeyComponentPartDtl>.Register(e => e.DeductedQty);

        /// <summary>
        /// 用料量
        /// </summary>
        public decimal DeductedQty
        {
            get { return this.GetProperty(DeductedQtyProperty); }
            set { this.SetProperty(DeductedQtyProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<KeyComponentPartDtl>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
            set { this.SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<KeyComponentPartDtl>.Register(e => e.ItemName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { this.SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 扣料物料旧料号 ShortDescription
        /// <summary>
        /// 扣料物料旧料号
        /// </summary>
        [Label("扣料物料旧料号")]
        public static readonly Property<string> ShortDescriptionProperty = P<KeyComponentPartDtl>.Register(e => e.ShortDescription);

        /// <summary>
        /// 扣料物料旧料号
        /// </summary>
        public string ShortDescription
        {
            get { return this.GetProperty(ShortDescriptionProperty); }
            set { this.SetProperty(ShortDescriptionProperty, value); }
        }
        #endregion

        #region 单位 Unit
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitProperty = P<KeyComponentPartDtl>.Register(e => e.Unit);

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit
        {
            get { return this.GetProperty(UnitProperty); }
            set { this.SetProperty(UnitProperty, value); }
        }
        #endregion

        #region 批次用量 BacthQty
        /// <summary>
        /// 批次用量
        /// </summary>
        [Label("批次用量")]
        public static readonly Property<decimal?> BacthQtyProperty = P<KeyComponentPartDtl>.Register(e => e.BacthQty);

        /// <summary>
        /// 批次用量
        /// </summary>
        public decimal? BacthQty
        {
            get { return this.GetProperty(BacthQtyProperty); }
            set { this.SetProperty(BacthQtyProperty, value); }
        }
        #endregion
    }
    /// <summary>
    /// 关键件记录 实体配置
    /// </summary>
    internal class KeyComponentPartDtlConfig : EntityConfig<KeyComponentPartDtl>
    {
        /// <summary>
        /// 配置数据库映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("KEY_COMPONENT_PART_DTL").MapAllProperties();
            Meta.Property(KeyComponentPartDtl.BacthNoProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}
