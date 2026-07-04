using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources;
using System;

namespace SIE.Barcodes
{
    /// <summary>
    /// 条码打印日志 查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("条码打印日志查询实体")]
    public partial class BarcodeLogCriteria : Criteria
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public BarcodeLogCriteria()
        {
            OperatDate = new DateRange { DateRangeType = DateRangeType.All };
        }
        #endregion

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单")]
        public static readonly Property<string> WorkOrderNoProperty = P<BarcodeLogCriteria>.Register(e => e.WorkOrderNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
            set { this.SetProperty(WorkOrderNoProperty, value); }
        }
        #endregion

        #region 操作日期 OperatDate
        /// <summary>
        /// 操作日期
        /// </summary>
        [Label("操作日期")]
        public static readonly Property<DateRange> OperatDateProperty = P<BarcodeLogCriteria>.Register(e => e.OperatDate);

        /// <summary>
        /// 操作日期
        /// </summary>
        public DateRange OperatDate
        {
            get { return GetProperty(OperatDateProperty); }
            set { SetProperty(OperatDateProperty, value); }
        }
        #endregion

        #region 条码 Barcode
        /// <summary>
        /// 条码
        /// </summary>
        [Label("条码")]
        public static readonly Property<string> BarcodeProperty = P<BarcodeLogCriteria>.Register(e => e.Barcode);

        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode
        {
            get { return GetProperty(BarcodeProperty); }
            set { SetProperty(BarcodeProperty, value); }
        }
        #endregion

        #region 操作人 Operator
        /// <summary>
        /// 操作人Id
        /// </summary>
        [Label("操作人")]
        public static readonly IRefIdProperty OperatorIdProperty =
            P<BarcodeLogCriteria>.RegisterRefId(e => e.OperatorId, ReferenceType.Normal);

        /// <summary>
        /// 操作人Id
        /// </summary>
        public double? OperatorId
        {
            get { return (double?)this.GetRefNullableId(OperatorIdProperty); }
            set { this.SetRefNullableId(OperatorIdProperty, value); }
        }

        /// <summary>
        /// 操作人
        /// </summary>
        public static readonly RefEntityProperty<Employee> OperatorProperty =
            P<BarcodeLogCriteria>.RegisterRef(e => e.Operator, OperatorIdProperty);

        /// <summary>
        /// 操作人
        /// </summary>
        public Employee Operator
        {
            get { return this.GetRefEntity(OperatorProperty); }
            set { this.SetRefEntity(OperatorProperty, value); }
        }
        #endregion

        #region 类型 Type
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<BarcodeLogType?> TypeProperty = P<BarcodeLogCriteria>.Register(e => e.Type);

        /// <summary>
        /// 类型
        /// </summary>
        public BarcodeLogType? Type
        {
            get { return GetProperty(TypeProperty); }
            set { SetProperty(TypeProperty, value); }
        }
        #endregion

        /// <summary>
        /// 重写查询方法
        /// </summary>
        /// <returns>条码日志列表</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<BarcodeController>().GetBarcodeLogs(this);
        }
    }
}