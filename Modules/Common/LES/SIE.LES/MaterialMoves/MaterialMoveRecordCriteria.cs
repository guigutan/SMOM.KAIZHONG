using SIE.Items;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIE.Warehouses;
using SIE.LES.MaterialMoves.Enums;
using SIE.Resources.Employees;

namespace SIE.LES.MaterialMoves
{
    /// <summary>
    /// 工单挪料查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("工单挪料查询实体")]
    public class MaterialMoveRecordCriteria : Criteria
    {
        #region 挪料工单 SourceWo
        /// <summary>
        /// 挪料工单Id
        /// </summary>
        [Label("挪料工单")]
        public static readonly IRefIdProperty SourceWoIdProperty =
            P<MaterialMoveRecordCriteria>.RegisterRefId(e => e.SourceWoId, ReferenceType.Normal);

        /// <summary>
        /// 挪料工单Id
        /// </summary>
        public double? SourceWoId
        {
            get { return (double?)this.GetRefNullableId(SourceWoIdProperty); }
            set { this.SetRefNullableId(SourceWoIdProperty, value); }
        }

        /// <summary>
        /// 挪料工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> SourceWoProperty =
            P<MaterialMoveRecordCriteria>.RegisterRef(e => e.SourceWo, SourceWoIdProperty);

        /// <summary>
        /// 挪料工单
        /// </summary>
        public WorkOrder SourceWo
        {
            get { return this.GetRefEntity(SourceWoProperty); }
            set { this.SetRefEntity(SourceWoProperty, value); }
        }
        #endregion

        #region 目标工单 TargetWo
        /// <summary>
        /// 目标工单Id
        /// </summary>
        [Label("目标工单")]
        public static readonly IRefIdProperty TargetWoIdProperty =
            P<MaterialMoveRecordCriteria>.RegisterRefId(e => e.TargetWoId, ReferenceType.Normal);

        /// <summary>
        /// 目标工单Id
        /// </summary>
        public double? TargetWoId
        {
            get { return (double?)this.GetRefNullableId(TargetWoIdProperty); }
            set { this.SetRefNullableId(TargetWoIdProperty, value); }
        }

        /// <summary>
        /// 目标工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> TargetWoProperty =
            P<MaterialMoveRecordCriteria>.RegisterRef(e => e.TargetWo, TargetWoIdProperty);

        /// <summary>
        /// 目标工单
        /// </summary>
        public WorkOrder TargetWo
        {
            get { return this.GetRefEntity(TargetWoProperty); }
            set { this.SetRefEntity(TargetWoProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<MaterialMoveRecordCriteria>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double? ItemId
        {
            get { return (double?)this.GetRefNullableId(ItemIdProperty); }
            set { this.SetRefNullableId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty =
            P<MaterialMoveRecordCriteria>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 挪料原因 Reason
        /// <summary>
        /// 挪料原因
        /// </summary>
        [Label("挪料原因")]
        public static readonly Property<string> ReasonProperty = P<MaterialMoveRecordCriteria>.Register(e => e.Reason);

        /// <summary>
        /// 挪料原因
        /// </summary>
        public string Reason
        {
            get { return this.GetProperty(ReasonProperty); }
            set { this.SetProperty(ReasonProperty, value); }
        }
        #endregion

        #region 挪料仓库 Warehouse
        /// <summary>
        /// 挪料仓库Id
        /// </summary>
        [Label("挪料仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty =
            P<MaterialMoveRecordCriteria>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 挪料仓库Id
        /// </summary>
        public double? WarehouseId
        {
            get { return (double?)this.GetRefNullableId(WarehouseIdProperty); }
            set { this.SetRefNullableId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 挪料仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty =
            P<MaterialMoveRecordCriteria>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 挪料仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return this.GetRefEntity(WarehouseProperty); }
            set { this.SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 来源类型 SourceType
        /// <summary>
        /// 来源类型
        /// </summary>
        [Label("来源类型")]
        public static readonly Property<MoveSourceType?> SourceTypeProperty = P<MaterialMoveRecordCriteria>.Register(e => e.SourceType);

        /// <summary>
        /// 来源类型
        /// </summary>
        public MoveSourceType? SourceType
        {
            get { return this.GetProperty(SourceTypeProperty); }
            set { this.SetProperty(SourceTypeProperty, value); }
        }
        #endregion

        #region 创建人 Creater
        /// <summary>
        /// 创建人Id
        /// </summary>
        [Label("创建人")]
        public static readonly IRefIdProperty CreaterIdProperty =
            P<MaterialMoveRecordCriteria>.RegisterRefId(e => e.CreaterId, ReferenceType.Normal);

        /// <summary>
        /// 创建人Id
        /// </summary>
        public double? CreaterId
        {
            get { return (double?)this.GetRefNullableId(CreaterIdProperty); }
            set { this.SetRefNullableId(CreaterIdProperty, value); }
        }

        /// <summary>
        /// 创建人
        /// </summary>
        public static readonly RefEntityProperty<Employee> CreaterProperty =
            P<MaterialMoveRecordCriteria>.RegisterRef(e => e.Creater, CreaterIdProperty);

        /// <summary>
        /// 创建人
        /// </summary>
        public Employee Creater
        {
            get { return this.GetRefEntity(CreaterProperty); }
            set { this.SetRefEntity(CreaterProperty, value); }
        }
        #endregion

        #region 创建时间 CreateDate
        /// <summary>
        /// 创建时间
        /// </summary>
        [Label("创建时间")]
        public static readonly Property<DateRange> CreateDateProperty = P<MaterialMoveRecordCriteria>.Register(e => e.CreateDate);

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateRange CreateDate
        {
            get { return this.GetProperty(CreateDateProperty); }
            set { this.SetProperty(CreateDateProperty, value); }
        }
        #endregion


        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<MaterialMoveRecordController>().QueryMaterialMoveRecord(this);
        }
    }
}
