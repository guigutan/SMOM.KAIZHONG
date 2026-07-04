using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.MES.WorkOrderArchives.Services;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.WorkOrderArchives
{
    /// <summary>
    /// 工单制造档案查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("工单制造档案查询实体")]
    public class WorkOrderArchiveCriteria : Criteria
    {
        #region 工单编号 No
        /// <summary>
        /// 工单编号
        /// </summary>
        [Label("工单编号")]
        public static readonly Property<string> NoProperty = P<WorkOrderArchiveCriteria>.Register(e => e.No);

        /// <summary>
        /// 工单编号
        /// </summary>
        public string No
        {
            get { return this.GetProperty(NoProperty); }
            set { this.SetProperty(NoProperty, value); }
        }
        #endregion

        #region 产品编码 ProCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProCodeProperty = P<WorkOrderArchiveCriteria>.Register(e => e.ProCode);

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
        public static readonly Property<string> ProNameProperty = P<WorkOrderArchiveCriteria>.Register(e => e.ProName);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProName
        {
            get { return this.GetProperty(ProNameProperty); }
            set { this.SetProperty(ProNameProperty, value); }
        }
        #endregion

        #region 工单状态 State
        /// <summary>
        /// 工单状态
        /// </summary>
        [Label("工单状态")]
        public static readonly Property<string> StateProperty = P<WorkOrderArchiveCriteria>.Register(e => e.State);

        /// <summary>
        /// 工单状态
        /// </summary>
        public string State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        #region 条码 BarCode
        /// <summary>
        /// 条码
        /// </summary>
        [Label("条码")]
        public static readonly Property<string> BarCodeProperty = P<WorkOrderArchiveCriteria>.Register(e => e.BarCode);

        /// <summary>
        /// 条码
        /// </summary>
        public string BarCode
        {
            get { return this.GetProperty(BarCodeProperty); }
            set { this.SetProperty(BarCodeProperty, value); }
        }
        #endregion

        #region 批次号 BatchNo
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> BatchNoProperty = P<WorkOrderArchiveCriteria>.Register(e => e.BatchNo);

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo
        {
            get { return this.GetProperty(BatchNoProperty); }
            set { this.SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty =
            P<WorkOrderArchiveCriteria>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double? FactoryId
        {
            get { return (double?)this.GetRefNullableId(FactoryIdProperty); }
            set { this.SetRefNullableId(FactoryIdProperty, value); }
        }

        /// <summary>
        /// 工厂
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> FactoryProperty =
            P<WorkOrderArchiveCriteria>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return this.GetRefEntity(FactoryProperty); }
            set { this.SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 车间 WorkShop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty WorkShopIdProperty =
            P<WorkOrderArchiveCriteria>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double? WorkShopId
        {
            get { return (double?)this.GetRefNullableId(WorkShopIdProperty); }
            set { this.SetRefNullableId(WorkShopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> WorkShopProperty =
            P<WorkOrderArchiveCriteria>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise WorkShop
        {
            get { return this.GetRefEntity(WorkShopProperty); }
            set { this.SetRefEntity(WorkShopProperty, value); }
        }
        #endregion

        #region 资源 Resource
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty ResourceIdProperty =
            P<WorkOrderArchiveCriteria>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源Id
        /// </summary>
        public double? ResourceId
        {
            get { return (double?)this.GetRefNullableId(ResourceIdProperty); }
            set { this.SetRefNullableId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty =
            P<WorkOrderArchiveCriteria>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return this.GetRefEntity(ResourceProperty); }
            set { this.SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 计划开始时间 PlanBeginDate
        /// <summary>
        /// 计划开始时间
        /// </summary>
        [Label("计划开始时间")]
        public static readonly Property<DateRange> PlanBeginDateProperty = P<WorkOrderArchiveCriteria>.Register(e => e.PlanBeginDate);

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateRange PlanBeginDate
        {
            get { return this.GetProperty(PlanBeginDateProperty); }
            set { this.SetProperty(PlanBeginDateProperty, value); }
        }
        #endregion

        #region 实际完成时间 ActuFinishDate
        /// <summary>
        /// 实际完成时间
        /// </summary>
        [Label("实际完成时间")]
        public static readonly Property<DateRange> ActuFinishDateProperty = P<WorkOrderArchiveCriteria>.Register(e => e.ActuFinishDate);

        /// <summary>
        /// 实际完成时间
        /// </summary>
        public DateRange ActuFinishDate
        {
            get { return this.GetProperty(ActuFinishDateProperty); }
            set { this.SetProperty(ActuFinishDateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询实现
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<WorkOrderArchiveService>().QueryWorkOrderArchiveList(this);
        }
    }
}
