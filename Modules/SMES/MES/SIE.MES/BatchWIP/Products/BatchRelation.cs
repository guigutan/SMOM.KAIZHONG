using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.BatchWIP.Products
{
    /// <summary>
    /// 批次关联关系
    /// </summary>
    [RootEntity, Serializable]
    [Label("批次关联关系")]
    public partial class BatchRelation : DataEntity
    {
        #region 批次号 Bid
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> BidProperty = P<BatchRelation>.Register(e => e.Bid);

        /// <summary>
        /// 批次号
        /// </summary>
        public string Bid
        {
            get { return GetProperty(BidProperty); }
            set { SetProperty(BidProperty, value); }
        }
        #endregion

        #region 父批次号 Pid
        /// <summary>
        /// 父批次号
        /// </summary>
        [Label("父批次号")]
        public static readonly Property<string> PidProperty = P<BatchRelation>.Register(e => e.Pid);

        /// <summary>
        /// 父批次号
        /// </summary>
        public string Pid
        {
            get { return GetProperty(PidProperty); }
            set { SetProperty(PidProperty, value); }
        }
        #endregion

        #region 载具号 ContainerNo
        /// <summary>
        /// 载具号
        /// </summary>
        [Label("载具号")]
        public static readonly Property<string> ContainerNoProperty = P<BatchRelation>.Register(e => e.ContainerNo);

        /// <summary>
        /// 载具号
        /// </summary>
        public string ContainerNo
        {
            get { return GetProperty(ContainerNoProperty); }
            set { SetProperty(ContainerNoProperty, value); }
        }
        #endregion

        #region 批次数量 Qty
        /// <summary>
        /// 批次数量
        /// </summary>
        [Label("批次数量")]
        public static readonly Property<decimal> QtyProperty = P<BatchRelation>.Register(e => e.Qty);

        /// <summary>
        /// 批次数量
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 剩余数量 RemainQty
        /// <summary>
        /// 剩余数量
        /// </summary>
        [Label("剩余数量")]
        public static readonly Property<decimal> RemainQtyProperty = P<BatchRelation>.Register(e => e.RemainQty);

        /// <summary>
        /// 剩余数量
        /// </summary>
        public decimal RemainQty
        {
            get { return this.GetProperty(RemainQtyProperty); }
            set { this.SetProperty(RemainQtyProperty, value); }
        }
        #endregion

        #region 生产批次号 WipBatch
        /// <summary>
        /// 生产批次号
        /// </summary>
        [Label("生产批次号")]
        public static readonly Property<string> WipBatchProperty = P<BatchRelation>.Register(e => e.WipBatch);

        /// <summary>
        /// 生产批次号
        /// </summary>
        public string WipBatch
        {
            get { return GetProperty(WipBatchProperty); }
            set { SetProperty(WipBatchProperty, value); }
        }
        #endregion

        #region 批次来源 BatchSource
        /// <summary>
        /// 批次来源
        /// </summary>
        [Label("批次来源")]
        public static readonly Property<BatchSource?> BatchSourceProperty = P<BatchRelation>.Register(e => e.BatchSource);

        /// <summary>
        /// 批次来源
        /// </summary>
        public BatchSource? BatchSource
        {
            get { return GetProperty(BatchSourceProperty); }
            set { SetProperty(BatchSourceProperty, value); }
        }
        #endregion

        #region 工单ID WorkOrderId
        /// <summary>
        /// 工单ID
        /// </summary>
        [Label("工单ID")]
        public static readonly Property<double> WorkOrderIdProperty = P<BatchRelation>.Register(e => e.WorkOrderId);

        /// <summary>
        /// 工单ID
        /// </summary>
        public double WorkOrderId
        {
            get { return this.GetProperty(WorkOrderIdProperty); }
            set { this.SetProperty(WorkOrderIdProperty, value); }
        }
        #endregion

        #region 是否完工 IsFinish
        /// <summary>
        /// 是否完工
        /// </summary>
        [Label("是否完工")]
        public static readonly Property<bool> IsFinishProperty = P<BatchRelation>.Register(e => e.IsFinish);

        /// <summary>
        /// 是否完工
        /// </summary>
        public bool IsFinish
        {
            get { return this.GetProperty(IsFinishProperty); }
            set { this.SetProperty(IsFinishProperty, value); }
        }
        #endregion

        #region 是否暂停 IsPause
        /// <summary>
        /// 是否暂停
        /// </summary>
        [Label("是否暂停")]
        public static readonly Property<YesNo> IsPauseProperty = P<BatchRelation>.Register(e => e.IsPause);

        /// <summary>
        /// 是否暂停
        /// </summary>
        public YesNo IsPause
        {
            get { return GetProperty(IsPauseProperty); }
            set { SetProperty(IsPauseProperty, value); }
        }
        #endregion

        #region 是否不良 IsNg
        /// <summary>
        /// 是否不良
        /// </summary>
        [Label("是否不良")]
        public static readonly Property<bool> IsNgProperty = P<BatchRelation>.Register(e => e.IsNg);

        /// <summary>
        /// 是否不良
        /// </summary>
        public bool IsNg
        {
            get { return this.GetProperty(IsNgProperty); }
            set { this.SetProperty(IsNgProperty, value); }
        }
        #endregion

        #region 资源id ResourceId
        /// <summary>
        /// 资源id
        /// </summary>
        [Label("资源id")]
        public static readonly Property<double?> ResourceIdProperty = P<BatchRelation>.Register(e => e.ResourceId);

        /// <summary>
        /// 资源id
        /// </summary>
        public double? ResourceId
        {
            get { return this.GetProperty(ResourceIdProperty); }
            set { this.SetProperty(ResourceIdProperty, value); }
        }
        #endregion

        #region 工序id ProcessId
        /// <summary>
        /// 工序id
        /// </summary>
        [Label("工序id")]
        public static readonly Property<double?> ProcessIdProperty = P<BatchRelation>.Register(e => e.ProcessId);

        /// <summary>
        /// 工序id
        /// </summary>
        public double? ProcessId
        {
            get { return this.GetProperty(ProcessIdProperty); }
            set { this.SetProperty(ProcessIdProperty, value); }
        }
        #endregion

        #region 工位id StationId
        /// <summary>
        /// 工位id
        /// </summary>
        [Label("工位id")]
        public static readonly Property<double?> StationIdProperty = P<BatchRelation>.Register(e => e.StationId);

        /// <summary>
        /// 工位id
        /// </summary>
        public double? StationId
        {
            get { return this.GetProperty(StationIdProperty); }
            set { this.SetProperty(StationIdProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 批次关联关系 实体配置
    /// </summary>
    internal class BatchRelationEntityConfig : EntityConfig<BatchRelation>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("BATCH_RELATION").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}