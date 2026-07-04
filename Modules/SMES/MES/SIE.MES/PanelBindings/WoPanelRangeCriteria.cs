using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.ObjectModel;
using SIE.Resources;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.PanelBindings
{
    [QueryEntity, Serializable]
    [Label("拼板码领用查询实体")]
    public class WoPanelRangeCriteria :Criteria
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public WoPanelRangeCriteria()
        {
            ReceiveDate = new DateRange
            {
                DateRangeType = DateRangeType.All
            };
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty =
            P<WoPanelRangeCriteria>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

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
            P<WoPanelRangeCriteria>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkOrderProperty); }
            set { this.SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 拼板码 PanelCode
        /// <summary>
        /// 拼板码
        /// </summary>
        [Label("拼板码")]
        public static readonly Property<string> PanelCodeProperty = P<WoPanelRangeCriteria>.Register(e => e.PanelCode);

        /// <summary>
        /// 拼板码
        /// </summary>
        public string PanelCode
        {
            get { return this.GetProperty(PanelCodeProperty); }
            set { this.SetProperty(PanelCodeProperty, value); }
        }
        #endregion


        #region 领用人 ReceiveBy
        /// <summary>
        /// 领用人Id
        /// </summary>
        [Label("领用人")]
        public static readonly IRefIdProperty ReceiveByIdProperty = P<WoPanelRangeCriteria>.RegisterRefId(e => e.ReceiveById, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Employee> ReceiveByProperty = P<WoPanelRangeCriteria>.RegisterRef(e => e.ReceiveBy, ReceiveByIdProperty);

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
        public static readonly Property<DateRange> ReceiveDateProperty = P<WoPanelRangeCriteria>.Register(e => e.ReceiveDate);

        /// <summary>
        /// 领用时间
        /// </summary>
        public DateRange ReceiveDate
        {
            get { return this.GetProperty(ReceiveDateProperty); }
            set { this.SetProperty(ReceiveDateProperty, value); }
        }
        #endregion
        #region 资源 WipResource
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty WipResourceIdProperty =
            P<WoPanelRangeCriteria>.RegisterRefId(e => e.WipResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源Id
        /// </summary>
        public double? WipResourceId
        {
            get { return (double?)this.GetRefNullableId(WipResourceIdProperty); }
            set { this.SetRefNullableId(WipResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> WipResourceProperty =
            P<WoPanelRangeCriteria>.RegisterRef(e => e.WipResource, WipResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource WipResource
        {
            get { return this.GetRefEntity(WipResourceProperty); }
            set { this.SetRefEntity(WipResourceProperty, value); }
        }
        #endregion


        #region 查询
        /// <summary>
        /// 重写查询方法
        /// </summary>
        /// <returns>条码范围列表</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<WoPanelRangeController>().GetPanelRanges(this);
        }
        #endregion

    }
}
