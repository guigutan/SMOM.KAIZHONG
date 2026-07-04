using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.WIP.Repairs
{
    /// <summary>
    /// 暂存维修记录
    /// </summary>
    [RootEntity, Serializable]
    public partial class RepairRecord : DataEntity
    {
        #region 产品缺陷ID ProductDefectId
        /// <summary>
        /// 产品缺陷ID
        /// </summary>
        [Label("产品缺陷ID")]
        public static readonly Property<double> ProductDefectIdProperty = P<RepairRecord>.Register(e => e.ProductDefectId);

        /// <summary>
        /// 产品缺陷ID
        /// </summary>
        public double ProductDefectId
        {
            get { return this.GetProperty(ProductDefectIdProperty); }
            set { this.SetProperty(ProductDefectIdProperty, value); }
        }
        #endregion

        #region 是否批次 IsBatch
        /// <summary>
        /// 是否批次
        /// </summary>
        [Label("是否批次")]
        public static readonly Property<bool> IsBatchProperty = P<RepairRecord>.Register(e => e.IsBatch);

        /// <summary>
        /// 是否批次
        /// </summary>
        public bool IsBatch
        {
            get { return this.GetProperty(IsBatchProperty); }
            set { this.SetProperty(IsBatchProperty, value); }
        }
        #endregion 

        #region 报废数量 ScrapQty
        /// <summary>
        /// 报废数量
        /// </summary>
        [Label("报废数量")]
        public static readonly Property<decimal> ScrapQtyProperty = P<RepairRecord>.Register(e => e.ScrapQty);

        /// <summary>
        /// 报废数量
        /// </summary>
        public decimal ScrapQty
        {
            get { return this.GetProperty(ScrapQtyProperty); }
            set { this.SetProperty(ScrapQtyProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<RepairRecord>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return this.GetProperty(RemarkProperty); }
            set { this.SetProperty(RemarkProperty, value); }
        }
        #endregion 

        #region 报废原因 ScrapReason
        /// <summary>
        /// 报废原因
        /// </summary>
        [Label("报废原因")]
        public static readonly Property<string> ScrapReasonProperty = P<RepairRecord>.Register(e => e.ScrapReason);

        /// <summary>
        /// 报废原因
        /// </summary>
        public string ScrapReason
        {
            get { return this.GetProperty(ScrapReasonProperty); }
            set { this.SetProperty(ScrapReasonProperty, value); }
        }
        #endregion

        #region 入站时间 InputDate
        /// <summary>
        /// 入站时间
        /// </summary>
        [Label("入站时间")]
        public static readonly Property<DateTime> InputDateProperty = P<RepairRecord>.Register(e => e.InputDate);

        /// <summary>
        /// 入站时间
        /// </summary>
        public DateTime InputDate
        {
            get { return this.GetProperty(InputDateProperty); }
            set { this.SetProperty(InputDateProperty, value); }
        }
        #endregion 

        #region 维修措施 MeasureList
        /// <summary>
        /// 维修措施
        /// </summary>
        [Label("维修措施")]
        public static readonly ListProperty<EntityList<MeasureRecord>> MeasureListProperty = P<RepairRecord>.RegisterList(e => e.MeasureList);

        /// <summary>
        /// 维修措施
        /// </summary>
        public EntityList<MeasureRecord> MeasureList
        {
            get { return this.GetLazyList(MeasureListProperty); }
        }
        #endregion

        #region 责任 ResponsibilityList
        /// <summary>
        /// 责任
        /// </summary>
        [Label("责任")]
        public static readonly ListProperty<EntityList<ResponsibilityRecord>> ResponsibilityListProperty = P<RepairRecord>.RegisterList(e => e.ResponsibilityList);

        /// <summary>
        /// 责任
        /// </summary>
        public EntityList<ResponsibilityRecord> ResponsibilityList
        {
            get { return this.GetLazyList(ResponsibilityListProperty); }
        }
        #endregion
    }

    /// <summary>
    /// 暂存维修记录实体配置
    /// </summary>
    internal class RepairRecordEntityConfig : EntityConfig<RepairRecord>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_REPAIR_RECORD").MapAllProperties();
            Meta.Property(RepairRecord.RemarkProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }
    }
}