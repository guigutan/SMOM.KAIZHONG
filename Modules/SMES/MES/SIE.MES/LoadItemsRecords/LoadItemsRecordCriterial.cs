using SIE.Domain;
using SIE.MES.LoadItemRecords;
using SIE.MES.LoadItems;
using SIE.MES.SingleLabels;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.WipResources;
using SIE.Tech.Stations;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.LoadItemsRecords
{
    /// <summary>
    /// 上料下料记录查询
    /// </summary>
    [QueryEntity, Serializable]
    [Label("上料下料记录查询")]
    public class LoadItemsRecordCriterial : Criteria
    {
        #region 操作类型 OpareteType
        /// <summary>
        /// 操作类型
        /// </summary>
        [Label("操作类型")]
        public static readonly Property<OpareteType?> OpareteTypeProperty = P<LoadItemsRecordCriterial>.Register(e => e.OpareteType);

        /// <summary>
        /// 操作类型
        /// </summary>
        public OpareteType? OpareteType
        {
            get { return this.GetProperty(OpareteTypeProperty); }
            set { this.SetProperty(OpareteTypeProperty, value); }
        }
        #endregion

        #region 资源 Resource
        /// <summary>
        /// 资源
        /// </summary>
        [Required]
        [Label("资源Id")]
        public static readonly IRefIdProperty ResourceIdProperty = P<LoadItemsRecordCriterial>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源
        /// </summary>
        public double?ResourceId
        {
            get { return (double?)GetRefNullableId(ResourceIdProperty); }
            set { SetRefNullableId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        [Label("资源")]
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<LoadItemsRecordCriterial>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 工位 Station
        /// <summary>
        /// 工位Id
        /// </summary>
        [Label("工位Id")]
        public static readonly IRefIdProperty StationIdProperty =
            P<LoadItemsRecordCriterial>.RegisterRefId(e => e.StationId, ReferenceType.Normal);

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
        [Label("工位")]
        public static readonly RefEntityProperty<Station> StationProperty =
            P<LoadItemsRecordCriterial>.RegisterRef(e => e.Station, StationIdProperty);

        /// <summary>
        /// 工位
        /// </summary>
        public Station Station
        {
            get { return this.GetRefEntity(StationProperty); }
            set { this.SetRefEntity(StationProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<LoadItemsRecordCriterial>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
            set { this.SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<LoadItemsRecordCriterial>.Register(e => e.ItemName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { this.SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 上料工单 LoadItemWokerOrder
        /// <summary>
        /// 上料工单
        /// </summary>
        [Label("上料工单")]
        public static readonly Property<string> LoadItemWokerOrderProperty = P<LoadItemsRecordCriterial>.Register(e => e.LoadItemWokerOrder);

        /// <summary>
        /// 上料工单
        /// </summary>
        public string LoadItemWokerOrder
        {
            get { return this.GetProperty(LoadItemWokerOrderProperty); }
            set { this.SetProperty(LoadItemWokerOrderProperty, value); }
        }
        #endregion


        #region 标签号 Sn
        /// <summary>
        /// 标签号
        /// </summary>
        [Label("标签号")]
        public static readonly Property<string> SnProperty = P<LoadItemsRecordCriterial>.Register(e => e.Sn);

        /// <summary>
        /// 标签号
        /// </summary>
        public string Sn
        {
            get { return this.GetProperty(SnProperty); }
            set { this.SetProperty(SnProperty, value); }
        }
        #endregion

        #region 下料类型 UnloadItemType
        /// <summary>
        /// 下料类型
        /// </summary>
        [Label("下料类型")]
        public static readonly Property<UnloadItemType?> UnloadItemTypeProperty = P<LoadItemsRecordCriterial>.Register(e => e.UnloadItemType);

        /// <summary>
        /// 下料类型
        /// </summary>
        public UnloadItemType? UnloadItemType
        {
            get { return this.GetProperty(UnloadItemTypeProperty); }
            set { this.SetProperty(UnloadItemTypeProperty, value); }
        }
        #endregion


        #region 只显示剩余数量不为0 IsDiaplayZero
        /// <summary>
        /// 只显示剩余数量不为0
        /// </summary>
        [Label("只显示剩余数量不为0")]
        public static readonly Property<bool?> IsDiaplayZeroProperty = P<LoadItemsRecordCriterial>.Register(e => e.IsDiaplayZero);

        /// <summary>
        /// 只显示剩余数量不为0
        /// </summary>
        public bool? IsDiaplayZero
        {
            get { return this.GetProperty(IsDiaplayZeroProperty); }
            set { this.SetProperty(IsDiaplayZeroProperty, value); }
        }
        #endregion

        #region 操作人 Oparetor
        /// <summary>
        /// 操作人Id
        /// </summary>
        [Label("操作人")]
        public static readonly IRefIdProperty OparetorIdProperty =
            P<LoadItemsRecordCriterial>.RegisterRefId(e => e.OparetorId, ReferenceType.Normal);

        /// <summary>
        /// 操作人Id
        /// </summary>
        public double? OparetorId
        {
            get { return (double?)this.GetRefNullableId(OparetorIdProperty); }
            set { this.SetRefNullableId(OparetorIdProperty, value); }
        }

        /// <summary>
        /// 操作人
        /// </summary>
        public static readonly RefEntityProperty<Employee> OparetorProperty =
            P<LoadItemsRecordCriterial>.RegisterRef(e => e.Oparetor, OparetorIdProperty);

        /// <summary>
        /// 操作人
        /// </summary>
        public Employee Oparetor
        {
            get { return this.GetRefEntity(OparetorProperty); }
            set { this.SetRefEntity(OparetorProperty, value); }
        }
        #endregion

        #region 操作时间 OparetorTime
        /// <summary>
        /// 操作时间
        /// </summary>
        [Label("操作时间")]
        public static readonly Property<DateRange> OparetorTimeProperty = P<LoadItemsRecordCriterial>.Register(e => e.OparetorTime);

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateRange OparetorTime
        {
            get { return this.GetProperty(OparetorTimeProperty); }
            set { this.SetProperty(OparetorTimeProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<LoadItemsRecordController>().Fetch(this);
        }

    }
}
