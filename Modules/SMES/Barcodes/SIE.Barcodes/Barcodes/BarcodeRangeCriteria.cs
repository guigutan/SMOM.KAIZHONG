using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources;
using System;

namespace SIE.Barcodes
{
    /// <summary>
    /// 条码领用 查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("条码领用查询实体")]
    public class BarcodeRangeCriteria : Criteria
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public BarcodeRangeCriteria()
        {
            ReceiveDate = new DateRange
            {
                DateRangeType = DateRangeType.All
            };
        }
        #endregion

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<BarcodeRangeCriteria>.Register(e => e.WorkOrderNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
            set { this.SetProperty(WorkOrderNoProperty, value); }
        }
        #endregion

        #region 领用人 ReceiveBy
        /// <summary>
        /// 领用人Id
        /// </summary>
        [Label("领用人")]
        public static readonly IRefIdProperty ReceiveByIdProperty = P<BarcodeRangeCriteria>.RegisterRefId(e => e.ReceiveById, ReferenceType.Normal);

        /// <summary>
        /// 领用人Id
        /// </summary>
        public double? ReceiveById
        {
            get { return (double?)GetRefNullableId(ReceiveByIdProperty); }
            set { SetRefNullableId(ReceiveByIdProperty, value); }
        }

        /// <summary>
        /// 领用人
        /// </summary>
        public static readonly RefEntityProperty<Employee> ReceiveByProperty = P<BarcodeRangeCriteria>.RegisterRef(e => e.ReceiveBy, ReceiveByIdProperty);

        /// <summary>
        /// 领用人
        /// </summary>
        public Employee ReceiveBy
        {
            get { return GetRefEntity(ReceiveByProperty); }
            set { SetRefEntity(ReceiveByProperty, value); }
        }
        #endregion

        #region 领用时间
        /// <summary>
        /// 领用时间
        /// </summary>
        [Label("领用时间")]
        public static readonly Property<DateRange> ReceiveDateProperty = P<BarcodeRangeCriteria>.Register(e => e.ReceiveDate);

        /// <summary>
        /// 领用时间
        /// </summary>
        public DateRange ReceiveDate
        {
            get { return this.GetProperty(ReceiveDateProperty); }
            set { this.SetProperty(ReceiveDateProperty, value); }
        }
        #endregion

        #region 查询
        /// <summary>
        /// 重写查询方法
        /// </summary>
        /// <returns>条码范围列表</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<BarcodeController>().GetBarcodeRanges(this);
        }
        #endregion
    }
}
