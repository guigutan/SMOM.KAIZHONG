using SIE.Domain;
using SIE.MES.QTimes.Services;
using SIE.MES.WorkOrders;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.QTimes
{
    /// <summary>
    /// QTime超时报表查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("QTime超时报表查询实体")]
    public class QTimeReportViewModelCriteria : Criteria
    {
        #region 产线 WipResource
        /// <summary>
        /// 产线Id
        /// </summary>
        [Label("产线")]
        public static readonly IRefIdProperty WipResourceIdProperty =
            P<QTimeReportViewModelCriteria>.RegisterRefId(e => e.WipResourceId, ReferenceType.Normal);

        /// <summary>
        /// 产线Id
        /// </summary>
        public double? WipResourceId
        {
            get { return (double?)this.GetRefNullableId(WipResourceIdProperty); }
            set { this.SetRefNullableId(WipResourceIdProperty, value); }
        }

        /// <summary>
        /// 产线
        /// </summary>
        public static readonly RefEntityProperty<WipResource> WipResourceProperty =
            P<QTimeReportViewModelCriteria>.RegisterRef(e => e.WipResource, WipResourceIdProperty);

        /// <summary>
        /// 产线
        /// </summary>
        public WipResource WipResource
        {
            get { return this.GetRefEntity(WipResourceProperty); }
            set { this.SetRefEntity(WipResourceProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty =
            P<QTimeReportViewModelCriteria>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double? WorkOrderId
        {
            get { return (double?)this.GetRefNullableId(WorkOrderIdProperty); }
            set { this.SetRefNullableId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty =
            P<QTimeReportViewModelCriteria>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkOrderProperty); }
            set { this.SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 条码 Sn
        /// <summary>
        /// 条码
        /// </summary>
        [Label("条码")]
        public static readonly Property<string> SnProperty = P<QTimeReportViewModelCriteria>.Register(e => e.Sn);

        /// <summary>
        /// 条码
        /// </summary>
        public string Sn
        {
            get { return this.GetProperty(SnProperty); }
            set { this.SetProperty(SnProperty, value); }
        }
        #endregion

        #region 产品编码 ProCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProCodeProperty = P<QTimeReportViewModelCriteria>.Register(e => e.ProCode);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProCode
        {
            get { return this.GetProperty(ProCodeProperty); }
            set { this.SetProperty(ProCodeProperty, value); }
        }
        #endregion

        #region 产品名称 ProName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProNameProperty = P<QTimeReportViewModelCriteria>.Register(e => e.ProName);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProName
        {
            get { return this.GetProperty(ProNameProperty); }
            set { this.SetProperty(ProNameProperty, value); }
        }
        #endregion

        #region 开始工序 StartProcess
        /// <summary>
        /// 开始工序Id
        /// </summary>
        [Label("开始工序")]
        public static readonly IRefIdProperty StartProcessIdProperty =
            P<QTimeReportViewModelCriteria>.RegisterRefId(e => e.StartProcessId, ReferenceType.Normal);

        /// <summary>
        /// 开始工序Id
        /// </summary>
        public double? StartProcessId
        {
            get { return (double?)this.GetRefNullableId(StartProcessIdProperty); }
            set { this.SetRefNullableId(StartProcessIdProperty, value); }
        }

        /// <summary>
        /// 开始工序
        /// </summary>
        public static readonly RefEntityProperty<Process> StartProcessProperty =
            P<QTimeReportViewModelCriteria>.RegisterRef(e => e.StartProcess, StartProcessIdProperty);

        /// <summary>
        /// 开始工序
        /// </summary>
        public Process StartProcess
        {
            get { return this.GetRefEntity(StartProcessProperty); }
            set { this.SetRefEntity(StartProcessProperty, value); }
        }
        #endregion

        #region 结束工序 EndProcess
        /// <summary>
        /// 结束工序Id
        /// </summary>
        [Label("结束工序")]
        public static readonly IRefIdProperty EndProcessIdProperty =
            P<QTimeReportViewModelCriteria>.RegisterRefId(e => e.EndProcessId, ReferenceType.Normal);

        /// <summary>
        /// 结束工序Id
        /// </summary>
        public double? EndProcessId
        {
            get { return (double?)this.GetRefNullableId(EndProcessIdProperty); }
            set { this.SetRefNullableId(EndProcessIdProperty, value); }
        }

        /// <summary>
        /// 结束工序
        /// </summary>
        public static readonly RefEntityProperty<Process> EndProcessProperty =
            P<QTimeReportViewModelCriteria>.RegisterRef(e => e.EndProcess, EndProcessIdProperty);

        /// <summary>
        /// 结束工序
        /// </summary>
        public Process EndProcess
        {
            get { return this.GetRefEntity(EndProcessProperty); }
            set { this.SetRefEntity(EndProcessProperty, value); }
        }
        #endregion

        #region 采集时间 CollectTime
        /// <summary>
        /// 采集时间
        /// </summary>
        [Label("采集时间")]
        public static readonly Property<DateRange> CollectTimeProperty = P<QTimeReportViewModelCriteria>.Register(e => e.CollectTime);

        /// <summary>
        /// 采集时间
        /// </summary>
        public DateRange CollectTime
        {
            get { return this.GetProperty(CollectTimeProperty); }
            set { this.SetProperty(CollectTimeProperty, value); }
        }
        #endregion

        #region 是否超时 IsOverTime
        /// <summary>
        /// 是否超时
        /// </summary>
        [Label("是否超时")]
        public static readonly Property<YesNo?> IsOverTimeProperty = P<QTimeReportViewModelCriteria>.Register(e => e.IsOverTime);

        /// <summary>
        /// 是否超时
        /// </summary>
        public YesNo? IsOverTime
        {
            get { return this.GetProperty(IsOverTimeProperty); }
            set { this.SetProperty(IsOverTimeProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<QTimeReportService>().QueryQTimeReports(this);
        }
    }
}
