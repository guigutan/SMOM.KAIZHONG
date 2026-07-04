using SIE.Domain;
using SIE.Items;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using System;

namespace SIE.MES.WIP.PackRecombine.Relations
{
    /// <summary>
    /// 包装清单查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("包装清单查询实体")]
    public class PackingRelationQueryCriteria : Criteria
    {
        #region 包装号 PackageNo
        /// <summary>
        /// 包装号
        /// </summary>
        [Label("包装号")]
        public static readonly Property<string> PackageNoProperty = P<PackingRelationQueryCriteria>.Register(e => e.PackageNo);

        /// <summary>
        /// 包装号
        /// </summary>
        public string PackageNo
        {
            get { return GetProperty(PackageNoProperty); }
            set { SetProperty(PackageNoProperty, value); }
        }
        #endregion

        #region 产品条码 Sn
        /// <summary>
        /// 产品条码
        /// </summary>
        [Label("产品条码")]
        public static readonly Property<string> SnProperty = P<PackingRelationQueryCriteria>.Register(e => e.Sn);

        /// <summary>
        /// 产品条码
        /// </summary>
        public string Sn
        {
            get { return GetProperty(SnProperty); }
            set { SetProperty(SnProperty, value); }
        }
        #endregion

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<PackingRelationQueryCriteria>.Register(e => e.WorkOrderNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return GetProperty(WorkOrderNoProperty); }
            set { SetProperty(WorkOrderNoProperty, value); }
        }
        #endregion

        #region Product 产品
        /// <summary>
        /// 产品ID
        /// </summary>
        [Label("产品")]
        public static readonly IRefIdProperty ProductIdProperty =
            P<PackingRelationQueryCriteria>.RegisterRefId(e => e.ProductId, ReferenceType.Normal);

        /// <summary>
        /// 产品ID
        /// </summary>
        public double? ProductId
        {
            get { return (double?)this.GetRefNullableId(ProductIdProperty); }
            set { this.SetRefNullableId(ProductIdProperty, value); }
        }

        /// <summary>
        /// 产品
        /// </summary>
        public static readonly RefEntityProperty<Item> ProductProperty =
            P<PackingRelationQueryCriteria>.RegisterRef(e => e.Product, ProductIdProperty);

        /// <summary>
        ///  产品 
        /// </summary>
        public Item Product
        {
            get { return this.GetRefEntity(ProductProperty); }
            set { this.SetRefEntity(ProductProperty, value); }
        }
        #endregion

        #region Resource 产线
        /// <summary>
        /// 产线ID
        /// </summary>
        [Label("产线")]
        public static readonly IRefIdProperty ResourceIdProperty =
            P<PackingRelationQueryCriteria>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 产线ID
        /// </summary>
        public double? ResourceId
        {
            get { return (double?)this.GetRefNullableId(ResourceIdProperty); }
            set { this.SetRefNullableId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 产线
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty =
            P<PackingRelationQueryCriteria>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 产线
        /// </summary>
        public WipResource Resource
        {
            get { return this.GetRefEntity(ResourceProperty); }
            set { this.SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region Process 工序
        /// <summary>
        /// 工序ID
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty =
            P<PackingRelationQueryCriteria>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序ID
        /// </summary>
        public double? ProcessId
        {
            get { return (double?)this.GetRefNullableId(ProcessIdProperty); }
            set { this.SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty =
            P<PackingRelationQueryCriteria>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region Station 工位
        /// <summary>
        /// 工位ID
        /// </summary>
        [Label("工位")]
        public static readonly IRefIdProperty StationIdProperty =
            P<PackingRelationQueryCriteria>.RegisterRefId(e => e.StationId, ReferenceType.Normal);

        /// <summary>
        /// 工位ID
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
            P<PackingRelationQueryCriteria>.RegisterRef(e => e.Station, StationIdProperty);

        /// <summary>
        ///  工位 
        /// </summary>
        public Station Station
        {
            get { return this.GetRefEntity(StationProperty); }
            set { this.SetRefEntity(StationProperty, value); }
        }
        #endregion
        
        #region 是否已打包 IsPacked
        /// <summary>
        /// 是否已打包
        /// </summary>
        [Label("是否已打包")]
        public static readonly Property<YesNo?> IsPackedProperty = P<PackingRelationQueryCriteria>.Register(e => e.IsPacked);

        /// <summary>
        /// 是否已打包
        /// </summary> 
        public YesNo? IsPacked
        {
            get { return this.GetProperty(IsPackedProperty); }
            set { SetProperty(IsPackedProperty, value); }
        }
        #endregion

        #region 包装时间 PackedDate
        /// <summary>
        /// 包装时间
        /// </summary>
        [Label("包装时间")]
        public static readonly Property<DateRange> PackedDateProperty = P<PackingRelationQueryCriteria>.Register(e => e.PackedDate);

        /// <summary>
        /// 包装时间
        /// </summary>
        public DateRange PackedDate
        {
            get { return GetProperty(PackedDateProperty); }
            set { SetProperty(PackedDateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 获取包装关系列表
        /// </summary>
        /// <returns>包装关系列表</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<PackingRelationQueryController>().GetPackingRelationQuerys(this);
        }
    }
}
