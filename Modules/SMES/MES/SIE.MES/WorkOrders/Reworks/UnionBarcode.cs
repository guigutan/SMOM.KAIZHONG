using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.WorkOrders.Reworks
{
    /// <summary>
    /// 工单关联条码
    /// </summary>
    [RootEntity, Serializable]
    [Label("工单关联条码")]
    public partial class UnionBarcode : DataEntity
    {
        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty =
            P<UnionBarcode>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId
        {
            get { return (double)this.GetRefId(WorkOrderIdProperty); }
            set { this.SetRefId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty =
            P<UnionBarcode>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkOrderProperty); }
            set { this.SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 返修工单条码 ReworkBarcode
        /// <summary>
        /// 返修工单条码
        /// </summary>
        [Label("返修工单条码")]
        public static readonly Property<string> ReworkBarcodeProperty = P<UnionBarcode>.Register(e => e.ReworkBarcode);

        /// <summary>
        /// 返修工单条码
        /// </summary>
        public string ReworkBarcode
        {
            get { return GetProperty(ReworkBarcodeProperty); }
            set { SetProperty(ReworkBarcodeProperty, value); }
        }
        #endregion

        #region 原工单条码 OriginalBarcode
        /// <summary>
        /// 原工单条码
        /// </summary>
        [Label("原工单条码")]
        public static readonly Property<string> OriginalBarcodeProperty = P<UnionBarcode>.Register(e => e.OriginalBarcode);

        /// <summary>
        /// 原工单条码
        /// </summary>
        public string OriginalBarcode
        {
            get { return GetProperty(OriginalBarcodeProperty); }
            set { SetProperty(OriginalBarcodeProperty, value); }
        }
        #endregion

        #region 条码状态 CodeState
        /// <summary>
        /// 条码状态
        /// </summary>
        [Label("条码状态")]
        public static readonly Property<CodeState> CodeStateProperty = P<UnionBarcode>.Register(e => e.CodeState);

        /// <summary>
        /// 条码状态
        /// </summary>
        public CodeState CodeState
        {
            get { return GetProperty(CodeStateProperty); }
            set { SetProperty(CodeStateProperty, value); }
        }
        #endregion

        #region 原条码工单ID OldWorkOrderId
        /// <summary>
        /// 原条码工单ID
        /// </summary>
        [Label("原条码工单ID")]
        public static readonly Property<double> OldWorkOrderIdProperty = P<UnionBarcode>.Register(e => e.OldWorkOrderId);

        /// <summary>
        /// 原条码工单ID
        /// </summary>
        public double OldWorkOrderId
        {
            get { return this.GetProperty(OldWorkOrderIdProperty); }
            set { this.SetProperty(OldWorkOrderIdProperty, value); }
        }
        #endregion 
    }

    /// <summary>
    /// 工单关联条码 实体配置
    /// </summary>
    internal class UnionBarcodeConfig : EntityConfig<UnionBarcode>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("BC_UNION_BARCODE").MapAllProperties();
            Meta.Property(UnionBarcode.WorkOrderIdProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}