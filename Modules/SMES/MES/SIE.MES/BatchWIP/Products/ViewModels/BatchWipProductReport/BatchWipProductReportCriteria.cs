using SIE.Domain;
using SIE.Items;
using SIE.MES.WIP.Products;
using SIE.MES.WorkOrders;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.BatchWIP.Products.ViewModels.BatchWipProductReport
{
    /// <summary>
    /// 工位库存库龄查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("工位库存库龄查询实体")]
    public class BatchWipProductReportCriteria: Criteria
    {
        #region 当前工序 Process
        /// <summary>
        /// 当前工序Id
        /// </summary>
        [Label("当前工序")]
        public static readonly IRefIdProperty ProcessIdProperty = P<BatchWipProductReportCriteria>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);
        /// <summary>
        /// 当前工序Id
        /// </summary>
        public double? ProcessId
        {
            get { return (double?)GetRefNullableId(ProcessIdProperty); }
            set { SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty = P<BatchWipProductReportCriteria>.RegisterRef(e => e.Process, ProcessIdProperty);
        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 工位 Station
        /// <summary>
        /// 工位Id
        /// </summary>
        [Label("工位")]
        public static readonly IRefIdProperty StationIdProperty =
            P<BatchWipProductReportCriteria>.RegisterRefId(e => e.StationId, ReferenceType.Normal);

        /// <summary>
        /// 工位Id
        /// </summary>
        public double? StationId
        {
            get { return (double?)this.GetRefNullableId(StationIdProperty); }
            set { this.SetRefNullableId(StationIdProperty, value); }
        }

        /// <summary>
        /// 工位
        /// </summary>
        public static readonly RefEntityProperty<Station> StationProperty =
            P<BatchWipProductReportCriteria>.RegisterRef(e => e.Station, StationIdProperty);

        /// <summary>
        /// 工位
        /// </summary>
        public Station Station
        {
            get { return this.GetRefEntity(StationProperty); }
            set { this.SetRefEntity(StationProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty = P<BatchWipProductReportCriteria>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double? WorkOrderId
        {
            get { return (double)GetRefId(WorkOrderIdProperty); }
            set { SetRefId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<BatchWipProductReportCriteria>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 生产批次 BatchNo
        /// <summary>
        /// 生产批次
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> BatchNoProperty = P<BatchWipProductReportCriteria>.Register(e => e.BatchNo);

        /// <summary>
        /// 生产批次
        /// </summary>
        public string BatchNo
        {
            get { return GetProperty(BatchNoProperty); }
            set { SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 产品编码 ItemId
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly IRefIdProperty ItemIdProperty = P<BatchWipProductReportCriteria>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 产品编码
        /// </summary>
        public double? ItemId
        {
            get { return (double)GetRefId(ItemIdProperty); }
            set { SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 产品编码
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty = P<BatchWipProductReportCriteria>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 产品编码
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        /// <summary>
        /// 获取产品版本列表
        /// </summary>
        /// <returns>产品版本列表</returns>
        protected override EntityList Fetch()
        {
            
            return RT.Service.Resolve<BatchWipProductVersionController>().GetBatchWipProductVersionsByReport(this);
            
        }
    }

    
}
