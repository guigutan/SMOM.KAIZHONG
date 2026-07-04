using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources;
using System;

namespace SIE.Barcodes.WipBatchs
{
    /// <summary>
    /// 批次物料工单查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("批次物料工单查询实体")]
    public class BatchWorkOrderCriteria : WorkOrderCriteria
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BatchWorkOrderCriteria()
        {
            PlanBeginDate = new DateRange() { DateRangeType = DateRangeType.Today, DateTimePart = DateTimePart.Date };
            CreateDate = new DateRange() { DateRangeType = DateRangeType.Today, DateTimePart = DateTimePart.Date };
        }

        #region 创建时间 CreateDate
        /// <summary>
        /// 创建时间
        /// </summary>
        [Label("创建时间")]
        public static readonly Property<DateRange> CreateDateProperty = P<BatchWorkOrderCriteria>.Register(e => e.CreateDate);

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateRange CreateDate
        {
            get { return this.GetProperty(CreateDateProperty); }
            set { this.SetProperty(CreateDateProperty, value); }
        }
        #endregion

        #region 创建人 CreateBy
        /// <summary>
        /// 创建人
        /// </summary>
        [Label("创建人")]
        public static readonly IRefIdProperty CreateByIdProperty =
           P<BatchWorkOrderCriteria>.RegisterRefId(e => e.CreateById, ReferenceType.Normal);

        /// <summary>
        /// 创建人
        /// </summary>
        public double? CreateById
        {
            get { return (double?)this.GetRefNullableId(CreateByIdProperty); }
            set { this.SetRefNullableId(CreateByIdProperty, value); }
        }

        /// <summary>
        /// 创建人
        /// </summary>
        public static readonly RefEntityProperty<Employee> CreateByProperty =
            P<BatchWorkOrderCriteria>.RegisterRef(e => e.CreateBy, CreateByIdProperty);

        /// <summary>
        /// 创建人
        /// </summary>
        public Employee CreateBy
        {
            get { return this.GetRefEntity(CreateByProperty); }
            set { this.SetRefEntity(CreateByProperty, value); }
        }
        #endregion

        #region 批次号 BatchNo
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> BatchNoProperty = P<BatchWorkOrderCriteria>.Register(e => e.BatchNo);

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo
        {
            get { return GetProperty(BatchNoProperty); }
            set { SetProperty(BatchNoProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns>列表</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<WipBatchController>().GetWorkOrders(this);
        }
    }
}