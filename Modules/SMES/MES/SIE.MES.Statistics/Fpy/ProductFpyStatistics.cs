using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Statistics.Fpy
{
    /// <summary>
    /// 产品直通率统计
    /// </summary>
    [RootEntity, Serializable]
    [Label("产品直通率统计")]
    public partial class ProductFpyStatistics : FpyStatistics
    {
        #region 库存组织 InvOrgId
        /// <summary>
        /// 库存组织
        /// </summary>
        [Label("库存组织")]
        public static readonly Property<int?> InvOrgIdProperty = P<ProductFpyStatistics>.Register(e => e.InvOrgId);

        /// <summary>
        /// 库存组织
        /// </summary>
        public int? InvOrgId
        {
            get { return this.GetProperty(InvOrgIdProperty); }
            set { this.SetProperty(InvOrgIdProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 产品直通率统计 实体配置
    /// </summary>
    internal class ProductFpyStatisticsConfig : EntityConfig<ProductFpyStatistics>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_STATS_PRODUCT_FPY").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.DisableInvOrg();
            Meta.Property(ProductFpyStatistics.WorkOrderIdProperty).ColumnMeta.HasIndex();
        }
    }
}