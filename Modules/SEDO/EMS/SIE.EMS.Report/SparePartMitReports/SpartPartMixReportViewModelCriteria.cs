using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Report.SparePartMitReports
{
    /// <summary>
    /// 备件库综合统计报表
    /// </summary>
    [QueryEntity, Serializable]
    [Label("备件库综合统计报表查询实体")]
    public class SparePartMixReportViewModelCriteria : Criteria
    {

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty =
            P<SparePartMixReportViewModelCriteria>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double? WarehouseId
        {
            get { return (double?)this.GetRefNullableId(WarehouseIdProperty); }
            set { this.SetRefNullableId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty =
            P<SparePartMixReportViewModelCriteria>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return this.GetRefEntity(WarehouseProperty); }
            set { this.SetRefEntity(WarehouseProperty, value); }
        }
        #endregion


        #region 月份 Month
        /// <summary>
        /// 月份
        /// </summary>
        [Label("月份从")]
        public static readonly Property<DateTime?> BeginMonthProperty = P<SparePartMixReportViewModelCriteria>.Register(e => e.BeginMonth);

        /// <summary>
        /// 月份
        /// </summary>
        public DateTime? BeginMonth
        {
            get { return GetProperty(BeginMonthProperty); }
            set { SetProperty(BeginMonthProperty, value); }
        }
        #endregion


        #region 月份 Month
        /// <summary>
        /// 月份
        /// </summary>
        [Label("到")]
        public static readonly Property<DateTime?> EndMonthProperty = P<SparePartMixReportViewModelCriteria>.Register(e => e.EndMonth);

        /// <summary>
        /// 月份
        /// </summary>
        public DateTime? EndMonth
        {
            get { return GetProperty(EndMonthProperty); }
            set { SetProperty(EndMonthProperty, value); }
        }
        #endregion

    }
}
