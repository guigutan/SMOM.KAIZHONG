using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Barcodes.WipBatchs
{
    /// <summary>
	/// 批次工单
	/// </summary>
	[RootEntity, Serializable]
    [ConditionQueryType(typeof(BatchWorkOrderCriteria))]
    [Label("批次工单")]
    public partial class BatchWorkOrder : WorkOrder
    {
        #region 已生成数量 GeneratedQty
        /// <summary>
        /// 已生成数量
        /// </summary>
        [Label("已生成数量")]
        public static readonly Property<decimal?> GeneratedQtyProperty = P<BatchWorkOrder>.Register(e => e.GeneratedQty);

        /// <summary>
        /// 已生成数量
        /// </summary>
        public decimal? GeneratedQty
        {
            get { return this.GetProperty(GeneratedQtyProperty); }
            set { this.SetProperty(GeneratedQtyProperty, value); }
        }
        #endregion

        #region 报废数量 ScrapQty

        /// <summary>
        /// 报废数量
        /// </summary>
        [Label("报废数量")]
        public static readonly Property<decimal> ScrapQtyProperty = P<BatchWorkOrder>.Register(e => e.ScrapQty);

        /// <summary>
        /// 报废数量
        /// </summary>
        public decimal ScrapQty
        {
            get { return GetProperty(ScrapQtyProperty); }
            set { SetProperty(ScrapQtyProperty, value); }
        }

        #endregion

        #region BS注册视图

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<BatchWorkOrder>.RegisterView(e => e.ProductCode, e => e.Product.Code);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return GetProperty(ProductCodeProperty); }
        }
        #endregion

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<BatchWorkOrder>.RegisterView(e => e.ProductName, e => e.Product.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return GetProperty(ProductNameProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 批次工单 实体配置
    /// </summary>
    internal class BatchWorkOrderConfig : EntityConfig<BatchWorkOrder>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WO").MapAllProperties();
            Meta.Property(BatchWorkOrder.GeneratedQtyProperty).ColumnMeta.DefaultValue = 0;
            Meta.EnablePhantoms();
        }
    }
}