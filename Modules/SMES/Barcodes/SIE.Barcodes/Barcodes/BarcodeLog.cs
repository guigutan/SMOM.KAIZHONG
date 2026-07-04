using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources;
using System;

namespace SIE.Barcodes
{
    /// <summary>
    /// 条码日志
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(BarcodeLogCriteria))]
    [Label("条码打印日志")]
    [DisplayMember(nameof(BarcodeLog.Id))]
    public partial class BarcodeLog : DataEntity
    {
        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<int> QtyProperty = P<BarcodeLog>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public int Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 操作日期 OperatDate
        /// <summary>
        /// 操作日期
        /// </summary>
        [Label("操作日期")]
        public static readonly Property<DateTime> OperatDateProperty = P<BarcodeLog>.Register(e => e.OperatDate);

        /// <summary>
        /// 操作日期
        /// </summary>
        public DateTime OperatDate
        {
            get { return GetProperty(OperatDateProperty); }
            set { SetProperty(OperatDateProperty, value); }
        }
        #endregion

        #region 原因 Reason
        /// <summary>
        /// 原因
        /// </summary>
        [Label("原因")]
        public static readonly Property<string> ReasonProperty = P<BarcodeLog>.Register(e => e.Reason);

        /// <summary>
        /// 原因
        /// </summary>
        public string Reason
        {
            get { return GetProperty(ReasonProperty); }
            set { SetProperty(ReasonProperty, value); }
        }
        #endregion

        #region 条码 Barcode
        /// <summary>
        /// 条码Id
        /// </summary>
        [Label("条码")]
        public static readonly IRefIdProperty BarcodeIdProperty =
            P<BarcodeLog>.RegisterRefId(e => e.BarcodeId, ReferenceType.Normal);

        /// <summary>
        /// 条码Id
        /// </summary>
        public double BarcodeId
        {
            get { return (double)this.GetRefId(BarcodeIdProperty); }
            set { this.SetRefId(BarcodeIdProperty, value); }
        }

        /// <summary>
        /// 条码
        /// </summary>
        public static readonly RefEntityProperty<Barcode> BarcodeProperty =
            P<BarcodeLog>.RegisterRef(e => e.Barcode, BarcodeIdProperty);

        /// <summary>
        /// 条码
        /// </summary>
        public Barcode Barcode
        {
            get { return this.GetRefEntity(BarcodeProperty); }
            set { this.SetRefEntity(BarcodeProperty, value); }
        }
        #endregion

        #region 操作人 Operator
        /// <summary>
        /// 操作人Id
        /// </summary>
        [Label("操作人")]
        public static readonly IRefIdProperty OperatorIdProperty = P<BarcodeLog>.RegisterRefId(e => e.OperatorId, ReferenceType.Normal);

        /// <summary>
        /// 操作人Id
        /// </summary>
        public double OperatorId
        {
            get { return (double)GetRefId(OperatorIdProperty); }
            set { SetRefId(OperatorIdProperty, value); }
        }

        /// <summary>
        /// 操作人
        /// </summary>
        public static readonly RefEntityProperty<Employee> OperatorProperty = P<BarcodeLog>.RegisterRef(e => e.Operator, OperatorIdProperty);

        /// <summary>
        /// 操作人
        /// </summary>
        public Employee Operator
        {
            get { return GetRefEntity(OperatorProperty); }
            set { SetRefEntity(OperatorProperty, value); }
        }
        #endregion

        #region 类型 Type
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<BarcodeLogType> TypeProperty = P<BarcodeLog>.Register(e => e.Type);

        /// <summary>
        /// 类型
        /// </summary>
        public BarcodeLogType Type
        {
            get { return GetProperty(TypeProperty); }
            set { SetProperty(TypeProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty = P<BarcodeLog>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId
        {
            get { return (double)GetRefId(WorkOrderIdProperty); }
            set { SetRefId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<PrintWorkOrder> WorkOrderProperty = P<BarcodeLog>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public PrintWorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 注册视图属性(关联实体属性平铺显示，一般用于Web)
        #region 条码日志工单号 BarcodeLogWONo
        /// <summary>
        /// 条码日志工单号
        /// </summary>
        [Label("条码日志工单号")]
        public static readonly Property<string> BarcodeLogWONoProperty = P<BarcodeLog>.RegisterView(e => e.BarcodeLogWONo, p => p.WorkOrder.No);

        /// <summary>
        /// 条码日志工单号
        /// </summary>
        public string BarcodeLogWONo
        {
            get { return this.GetProperty(BarcodeLogWONoProperty); }
        }
        #endregion

        #region 条码日志条码号 BarcodeLogBarcodeSn
        /// <summary>
        /// 条码日志条码号
        /// </summary>
        [Label("条码日志条码号")]
        public static readonly Property<string> BarcodeLogBarcodeSnProperty = P<BarcodeLog>.RegisterView(e => e.BarcodeLogBarcodeSn, p => p.Barcode.Sn);

        /// <summary>
        /// 条码日志条码号
        /// </summary>
        public string BarcodeLogBarcodeSn
        {
            get { return this.GetProperty(BarcodeLogBarcodeSnProperty); }
        }
        #endregion

        #region 条码日志操作人 BarcodeLogOperatorName
        /// <summary>
        /// 条码日志操作人
        /// </summary>
        [Label("条码日志操作人")]
        public static readonly Property<string> BarcodeLogOperatorNameProperty = P<BarcodeLog>.RegisterView(e => e.BarcodeLogOperatorName, p => p.Operator.Name);

        /// <summary>
        /// 条码日志操作人
        /// </summary>
        public string BarcodeLogOperatorName
        {
            get { return this.GetProperty(BarcodeLogOperatorNameProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 条码日志 实体配置
    /// </summary>
    internal class BarcodeLogConfig : EntityConfig<BarcodeLog>
    {
        /// <summary>
        /// 对Meta属性的配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("BC_LOG").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}