using SIE.Domain;
using SIE.Fixtures.Models;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.Fixtures.FixtureRecords
{
    /// <summary>
    /// 工治具出入库查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("工治具出入库查询实体")]
    public class FixtureRecordCriteria : Criteria
    {
        #region 任务编号
        /// <summary>
        /// 任务编号
        /// </summary>
        [Label("任务编号")]
        public static readonly Property<string> CodeProperty = P<FixtureRecordCriteria>.Register(e => e.Code);

        /// <summary>
        /// 任务编号
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 类型 RecordType
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<RecordType?> RecordTypeProperty = P<FixtureRecordCriteria>.Register(e => e.RecordType);

        /// <summary>
        /// 类型
        /// </summary>
        public RecordType? RecordType
        {
            get { return GetProperty(RecordTypeProperty); }
            set { SetProperty(RecordTypeProperty, value); }
        }
        #endregion


        #region 业务类型 BusinessType
        /// <summary>
        /// 业务类型
        /// </summary>
        [Label("业务类型")]
        public static readonly Property<BusinessType?> BusinessTypeProperty = P<FixtureRecordCriteria>.Register(e => e.BusinessType);

        /// <summary>
        /// 业务类型
        /// </summary>
        public BusinessType? BusinessType
        {
            get { return GetProperty(BusinessTypeProperty); }
            set { SetProperty(BusinessTypeProperty, value); }
        }
        #endregion


        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        public static readonly IRefIdProperty FixtureWarehouseIdProperty = P<FixtureRecordCriteria>.RegisterRefId(e => e.FixtureWarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double? FixtureWarehouseId
        {
            get { return (double?)GetRefNullableId(FixtureWarehouseIdProperty); }
            set { SetRefNullableId(FixtureWarehouseIdProperty, value); }
        }

        /// <summary>
        /// 仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> FixtureWarehouseProperty = P<FixtureRecordCriteria>.RegisterRef(e => e.Warehouse, FixtureWarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(FixtureWarehouseProperty); }
            set { SetRefEntity(FixtureWarehouseProperty, value); }
        }
        #endregion


        #region 工治具型号 FixtureModel
        /// <summary>
        /// 工治具型号
        /// </summary>
        [Label("工治具型号")]
        public static readonly IRefIdProperty FixtureModelIdProperty =
            P<FixtureRecordCriteria>.RegisterRefId(e => e.FixtureModelId, ReferenceType.Normal);

        /// <summary>
        /// 工治具型号Id
        /// </summary>
        public double? FixtureModelId
        {
            get { return (double?)this.GetRefNullableId(FixtureModelIdProperty); }
            set { this.SetRefNullableId(FixtureModelIdProperty, value); }
        }

        /// <summary>
        /// 工治具型号
        /// </summary>
        public static readonly RefEntityProperty<FixtureModel> FixtureModelProperty =
            P<FixtureRecordCriteria>.RegisterRef(e => e.FixtureModel, FixtureModelIdProperty);

        /// <summary>
        /// 工治具型号
        /// </summary>
        public FixtureModel FixtureModel
        {
            get { return this.GetRefEntity(FixtureModelProperty); }
            set { this.SetRefEntity(FixtureModelProperty, value); }
        }
        #endregion

        #region 工治具编码 FixtureEncode
        /// <summary>
        /// 工治具编码Id
        /// </summary>
        [Label("工治具编码")]
        public static readonly IRefIdProperty FixtureEncodeIdProperty =
            P<FixtureRecordCriteria>.RegisterRefId(e => e.FixtureEncodeId, ReferenceType.Normal);

        /// <summary>
        /// 工治具编码Id
        /// </summary>
        public double? FixtureEncodeId
        {
            get { return (double?)this.GetRefNullableId(FixtureEncodeIdProperty); }
            set { this.SetRefNullableId(FixtureEncodeIdProperty, value); }
        }

        /// <summary>
        /// 工治具编码
        /// </summary>
        public static readonly RefEntityProperty<FixtureEncode> FixtureEncodeProperty =
            P<FixtureRecordCriteria>.RegisterRef(e => e.FixtureEncode, FixtureEncodeIdProperty);

        /// <summary>
        /// 工治具编码
        /// </summary>
        public FixtureEncode FixtureEncode
        {
            get { return this.GetRefEntity(FixtureEncodeProperty); }
            set { this.SetRefEntity(FixtureEncodeProperty, value); }
        }
        #endregion


        #region 单据创建时间 ApplyDate
        /// <summary>
        /// 单据创建时间
        /// </summary>
        [Label("单据创建时间")]
        public static readonly Property<DateRange> ApplyDateProperty = P<FixtureRecordCriteria>.Register(e => e.ApplyDate);

        /// <summary>
        /// 单据创建时间
        /// </summary>
        public DateRange ApplyDate
        {
            get { return GetProperty(ApplyDateProperty); }
            set { SetProperty(ApplyDateProperty, value); }
        }
        #endregion


        #region 任务执行时间 ComplyDate
        /// <summary>
        /// 任务执行时间
        /// </summary>
        [Label("任务执行时间")]
        public static readonly Property<DateRange> ComplyDateProperty = P<FixtureRecordCriteria>.Register(e => e.ComplyDate);

        /// <summary>
        /// 任务执行时间
        /// </summary>
        public DateRange ComplyDate
        {
            get { return GetProperty(ComplyDateProperty); }
            set { SetProperty(ComplyDateProperty, value); }
        }
        #endregion

        #region 工治具ID FixtureAccountCode
        /// <summary>
        /// 工治具ID
        /// </summary>
        [Label("工治具ID")]
        public static readonly Property<string> FixtureAccountCodeProperty = P<FixtureRecordCriteria>.Register(e => e.FixtureAccountCode);

        /// <summary>
        /// 工治具ID
        /// </summary>
        public string FixtureAccountCode
        {
            get { return this.GetProperty(FixtureAccountCodeProperty); }
            set { this.SetProperty(FixtureAccountCodeProperty, value); }
        }
        #endregion


        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<FixtureRecordController>().Fetch(this);
        }
    }
}
