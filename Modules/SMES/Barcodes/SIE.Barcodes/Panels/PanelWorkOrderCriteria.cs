using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Barcodes.Panels
{
    /// <summary>
    /// 拼板码打印查询
    /// </summary>
    [QueryEntity, Serializable]
    [Label("拼板码打印工单查询")]
    public partial class PanelWorkOrderCriteria : Criteria
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public PanelWorkOrderCriteria()
        {
            PlanBeginDate = new DateRange();
            PlanBeginDate.DateTimePart = DateTimePart.Date;  //选择日期格式为天
            PlanBeginDate.DateRangeType = DateRangeType.Week;  //默认日期为本周
        }
        #endregion

        #region 工单号 No
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> NoProperty = P<PanelWorkOrderCriteria>.Register(e => e.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string No
        {
            get { return this.GetProperty(NoProperty); }
            set { this.SetProperty(NoProperty, value); }
        }
        #endregion

        #region 计划开始时间 PlanBeginDate
        /// <summary>
        /// 计划开始时间
        /// </summary>
        [Label("计划开始时间")]
        public static readonly Property<DateRange> PlanBeginDateProperty = P<PanelWorkOrderCriteria>.Register(e => e.PlanBeginDate, new PropertyMetadata<DateRange>() { DateTimePart = DateTimePart.Date, });

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateRange PlanBeginDate
        {
            get { return GetProperty(PlanBeginDateProperty); }
            set { SetProperty(PlanBeginDateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 重写此方法实现查询
        /// </summary>
        /// <returns>工单列表</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<PanelController>().GetWorkOrders(this);
        }
    }
}