using SIE.Domain;
using SIE.MES.PrepareProducts.Enums;
using SIE.MES.PrepareProducts.Services;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.PrepareProducts
{
    /// <summary>
    /// 产前准备记录查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("产前准备记录查询实体")]
    public class PrepareRecordCriteria : Criteria
    {
        #region 工单 No
        /// <summary>
        /// 工单
        /// </summary>
        [Label("工单")]
        public static readonly Property<string> NoProperty = P<PrepareRecordCriteria>.Register(e => e.No);

        /// <summary>
        /// 工单
        /// </summary>
        public string No
        {
            get { return this.GetProperty(NoProperty); }
            set { this.SetProperty(NoProperty, value); }
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty =
            P<PrepareRecordCriteria>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

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
            P<PrepareRecordCriteria>.RegisterRef(e => e.Factory, FactoryIdProperty);

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
            P<PrepareRecordCriteria>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

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
            P<PrepareRecordCriteria>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise WorkShop
        {
            get { return this.GetRefEntity(WorkShopProperty); }
            set { this.SetRefEntity(WorkShopProperty, value); }
        }
        #endregion

        #region 生产资源 Resource
        /// <summary>
        /// 生产资源Id
        /// </summary>
        [Label("生产资源")]
        public static readonly IRefIdProperty ResourceIdProperty =
            P<PrepareRecordCriteria>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 生产资源Id
        /// </summary>
        public double? ResourceId
        {
            get { return (double?)this.GetRefNullableId(ResourceIdProperty); }
            set { this.SetRefNullableId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 生产资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty =
            P<PrepareRecordCriteria>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 生产资源
        /// </summary>
        public WipResource Resource
        {
            get { return this.GetRefEntity(ResourceProperty); }
            set { this.SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<PrepareRecordCriteria>.Register(e => e.ProductName);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
            set { this.SetProperty(ProductNameProperty, value); }
        }
        #endregion

        #region 工单状态 State
        /// <summary>
        /// 工单状态
        /// </summary>
        [Label("工单状态")]
        public static readonly Property<string> StateProperty = P<PrepareRecordCriteria>.Register(e => e.State);

        /// <summary>
        /// 工单状态
        /// </summary>
        public string State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        #region 产前准备状态 PreState
        /// <summary>
        /// 产前准备状态
        /// </summary>
        [Label("产前准备状态")]
        public static readonly Property<PrepareRecordState?> PreStateProperty = P<PrepareRecordCriteria>.Register(e => e.PreState);

        /// <summary>
        /// 产前准备状态
        /// </summary>
        public PrepareRecordState? PreState
        {
            get { return this.GetProperty(PreStateProperty); }
            set { this.SetProperty(PreStateProperty, value); }
        }
        #endregion

        #region 计划开始时间 PlanBeginTime
        /// <summary>
        /// 计划开始时间
        /// </summary>
        [Label("计划开始时间")]
        public static readonly Property<DateRange> PlanBeginTimeProperty = P<PrepareRecordCriteria>.Register(e => e.PlanBeginTime);

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateRange PlanBeginTime
        {
            get { return this.GetProperty(PlanBeginTimeProperty); }
            set { this.SetProperty(PlanBeginTimeProperty, value); }
        }
        #endregion

        #region 确认时间 ConfirmTime
        /// <summary>
        /// 确认时间
        /// </summary>
        [Label("确认时间")]
        public static readonly Property<DateRange> ConfirmTimeProperty = P<PrepareRecordCriteria>.Register(e => e.ConfirmTime);

        /// <summary>
        /// 确认时间
        /// </summary>
        public DateRange ConfirmTime
        {
            get { return this.GetProperty(ConfirmTimeProperty); }
            set { this.SetProperty(ConfirmTimeProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<PrepareRecordService>().QueryPrepareRecordList(this);
        }
    }
}
