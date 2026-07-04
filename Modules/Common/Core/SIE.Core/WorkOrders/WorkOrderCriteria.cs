using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Core.WorkOrders
{
    /// <summary>
    /// 公共工单查询实体
    /// </summary>
    [QueryEntity, Serializable]
    public partial class WorkOrderCriteria : Criteria
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public WorkOrderCriteria()
        {
            this.PlanBeginDate = new DateRange();
            this.PlanBeginDate.DateTimePart = DateTimePart.Date;
            this.PlanBeginDate.DateRangeType = DateRangeType.All;
        }

        #endregion

        #region 工单号 No
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> NoProperty = P<WorkOrderCriteria>.Register(e => e.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string No
        {
            get { return GetProperty(NoProperty); }
            set { SetProperty(NoProperty, value); }
        }
        #endregion

        #region 计划开始日期 PlanBeginDate
        /// <summary>
        /// 计划开始日期
        /// </summary>
        [Label("计划开始日期")]
        public static readonly Property<DateRange> PlanBeginDateProperty = P<WorkOrderCriteria>.Register(e => e.PlanBeginDate);

        /// <summary>
        /// 计划开始日期
        /// </summary>
        public DateRange PlanBeginDate
        {
            get { return GetProperty(PlanBeginDateProperty); }
            set { SetProperty(PlanBeginDateProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料
        /// </summary>
        [Label("物料")]
        public static readonly Property<string> ItemProperty = P<WorkOrderCriteria>.Register(e => e.Item);

        /// <summary>
        /// 物料
        /// </summary>
        public string Item
        {
            get { return GetProperty(ItemProperty); }
            set { SetProperty(ItemProperty, value); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<WorkOrderCriteria>.Register(e => e.ProductCode);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return GetProperty(ProductCodeProperty); }
            set { SetProperty(ProductCodeProperty, value); }
        }
        #endregion

        #region 物料 ItemName
        /// <summary>
        /// 物料
        /// </summary>
        [Label("物料")]
        public static readonly Property<string> ItemNameProperty = P<WorkOrderCriteria>.Register(e => e.ItemName);

        /// <summary>
        /// 物料
        /// </summary>
        public string ItemName
        {
            get { return GetProperty(ItemNameProperty); }
            set { SetProperty(ItemNameProperty, value); }
        }
        #endregion

        /// <summary>
        /// 重写查询方法
        /// </summary>
        /// <returns>查询列表</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<WorkOrderController>().GetWorkOrders(this);
        }
    }

    /// <summary>
    /// 实体页面配置
    /// </summary>
    internal class WorkOrderCriteriaViewConfig : WPFViewConfig<WorkOrderCriteria>
    {
        /// <summary>
        /// 页面配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DomainName("工单查询视图").HasDelegate(WorkOrderCriteria.IdProperty);

            ////View.UseDefaultCommands();

            using (View.OrderProperties())
            {
                View.Property(p => p.Item).HasLabel("物料编码").Show(ShowInWhere.All);
                View.Property(p => p.No).HasLabel("工单号").Show(ShowInWhere.All);
                View.Property(p => p.PlanBeginDate).HasLabel("计划开始时间").Show(ShowInWhere.All).UseEditor(WPFEditorNames.DateRange);
            }
        }
    }
}
