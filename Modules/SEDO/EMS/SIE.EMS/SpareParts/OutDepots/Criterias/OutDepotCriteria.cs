using SIE.Domain;
using SIE.EMS.SpareParts.OutDepots.Controllers;
using SIE.EMS.SpareParts.OutDepots.Enums;
using SIE.Equipments.Enums;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.SpareParts.OutDepots.Criterias
{
    /// <summary>
    /// 备件出库单查询器
    /// </summary>
    [QueryEntity, Serializable]
    public class OutDepotCriteria : Criteria
    {
        #region 备件出库单号 No
        /// <summary>
        /// 备件出库单号
        /// </summary>
        [Label("备件出库单号")]
        public static readonly Property<string> NoProperty = P<OutDepotCriteria>.Register(e => e.No);

        /// <summary>
        /// 备件出库单号
        /// </summary>
        public string No
        {
            get { return this.GetProperty(NoProperty); }
            set { this.SetProperty(NoProperty, value); }
        }
        #endregion

        #region 出库类型 OutDepotType
        /// <summary>
        /// 出库类型
        /// </summary>
        [Label("出库类型")]
        public static readonly Property<OutDepotType?> OutDepotTypeProperty = P<OutDepotCriteria>.Register(e => e.OutDepotType);

        /// <summary>
        /// 出库类型
        /// </summary>
        public OutDepotType? OutDepotType
        {
            get { return this.GetProperty(OutDepotTypeProperty); }
            set { this.SetProperty(OutDepotTypeProperty, value); }
        }
        #endregion

        #region 状态 OutDepotState
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<OutDepotState?> OutDepotStateProperty = P<OutDepotCriteria>.Register(e => e.OutDepotState);

        /// <summary>
        /// 状态
        /// </summary>
        public OutDepotState? OutDepotState
        {
            get { return this.GetProperty(OutDepotStateProperty); }
            set { this.SetProperty(OutDepotStateProperty, value); }
        }
        #endregion

        #region 质量状态 QualityStatus
        /// <summary>
        /// 质量状态
        /// </summary>
        [Label("质量状态")]
        public static readonly Property<QualityStatus?> QualityStatusProperty = P<OutDepotCriteria>.Register(e => e.QualityStatus);

        /// <summary>
        /// 质量状态
        /// </summary>
        public QualityStatus? QualityStatus
        {
            get { return this.GetProperty(QualityStatusProperty); }
            set { this.SetProperty(QualityStatusProperty, value); }
        }
        #endregion

        #region 申请单号 ReleDoc
        /// <summary>
        /// 申请单号
        /// </summary>
        [Label("申请单号")]
        public static readonly Property<string> ReleDocProperty = P<OutDepotCriteria>.Register(e => e.ReleDoc);

        /// <summary>
        /// 申请单号
        /// </summary>
        public string ReleDoc
        {
            get { return this.GetProperty(ReleDocProperty); }
            set { this.SetProperty(ReleDocProperty, value); }
        }
        #endregion

        #region 来源单号 SourceNo
        /// <summary>
        /// 来源单号
        /// </summary>
        [Label("来源单号")]
        public static readonly Property<string> SourceNoProperty = P<OutDepotCriteria>.Register(e => e.SourceNo);

        /// <summary>
        /// 来源单号
        /// </summary>
        public string SourceNo
        {
            get { return this.GetProperty(SourceNoProperty); }
            set { this.SetProperty(SourceNoProperty, value); }
        }
        #endregion

        #region 领用部门 GetDepartment
        /// <summary>
        /// 领用部门Id
        /// </summary>
        [Label("领用部门")]
        public static readonly IRefIdProperty GetDepartmentIdProperty =
            P<OutDepotCriteria>.RegisterRefId(e => e.GetDepartmentId, ReferenceType.Normal);

        /// <summary>
        /// 领用部门Id
        /// </summary>
        public double GetDepartmentId
        {
            get { return (double)this.GetRefId(GetDepartmentIdProperty); }
            set { this.SetRefId(GetDepartmentIdProperty, value); }
        }

        /// <summary>
        /// 领用部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> GetDepartmentProperty =
            P<OutDepotCriteria>.RegisterRef(e => e.GetDepartment, GetDepartmentIdProperty);

        /// <summary>
        /// 领用部门
        /// </summary>
        public Enterprise GetDepartment
        {
            get { return this.GetRefEntity(GetDepartmentProperty); }
            set { this.SetRefEntity(GetDepartmentProperty, value); }
        }
        #endregion

        #region 出库仓库 Warehouse
        /// <summary>
        /// 出库仓库Id
        /// </summary>
        [Label("出库仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty =
            P<OutDepotCriteria>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 出库仓库Id
        /// </summary>
        public double? WarehouseId
        {
            get { return (double?)this.GetRefNullableId(WarehouseIdProperty); }
            set { this.SetRefNullableId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 出库仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty =
            P<OutDepotCriteria>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 出库仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return this.GetRefEntity(WarehouseProperty); }
            set { this.SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 备件 SparePart
        /// <summary>
        /// 备件编码Id
        /// </summary>
        [Label("备件编码")]
        public static readonly IRefIdProperty SparePartIdProperty =
            P<OutDepotCriteria>.RegisterRefId(e => e.SparePartId, ReferenceType.Normal);

        /// <summary>
        /// 备件编码Id
        /// </summary>
        public double? SparePartId
        {
            get { return (double?)this.GetRefNullableId(SparePartIdProperty); }
            set { this.SetRefNullableId(SparePartIdProperty, value); }
        }

        /// <summary>
        /// 备件编码
        /// </summary>
        public static readonly RefEntityProperty<SparePart> SparePartProperty =
            P<OutDepotCriteria>.RegisterRef(e => e.SparePart, SparePartIdProperty);

        /// <summary>
        /// 备件编码
        /// </summary>
        public SparePart SparePart
        {
            get { return this.GetRefEntity(SparePartProperty); }
            set { this.SetRefEntity(SparePartProperty, value); }
        }
        #endregion

        #region 备件编码 SparePartCode
        /// <summary>
        /// 备件编码
        /// </summary>
        [Label("备件编码")]
        public static readonly Property<string> SparePartCodeProperty = P<OutDepotCriteria>.RegisterView(e => e.SparePartCode, p => p.SparePart.SparePartCode);

        /// <summary>
        /// 备件编码
        /// </summary>
        public string SparePartCode
        {
            get { return this.GetProperty(SparePartCodeProperty); }
        }
        #endregion

        #region 备件名称 SparePartName
        /// <summary>
        /// 备件名称
        /// </summary>
        [Label("备件名称")]
        public static readonly Property<string> SparePartNameProperty = P<OutDepotCriteria>.Register(e => e.SparePartName);

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparePartName
        {
            get { return this.GetProperty(SparePartNameProperty); }
            set { this.SetProperty(SparePartNameProperty, value); }
        }
        #endregion

        #region 创建日期 CreateDate
        /// <summary>
        /// 创建日期
        /// </summary>
        [Label("创建日期")]
        public static readonly Property<DateRange> CreateDateProperty = P<OutDepotCriteria>.Register(e => e.CreateDate);

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateRange CreateDate
        {
            get { return this.GetProperty(CreateDateProperty); }
            set { this.SetProperty(CreateDateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<OutDepotController>().GetOutDepotList(this);
        }
    }
}
