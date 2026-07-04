using DocumentFormat.OpenXml.Wordprocessing;
using SIE.Domain;
using SIE.Domain.Query;
using SIE.EventMessages;
using SIE.Items;
using SIE.MES.TaskManagement.FeedingRecords;
using SIE.MES.TaskManagement.Reports;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Packages.ItemLabels;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Report.BatchTracebacks
{
    /// <summary>
    /// 产品生产关键件
    /// </summary>
    [RootEntity, Serializable]
    [Label("产品生产关键件")]
    public class BatchTracebackKeyDtl : Entity<double>
    {
        #region 报工记录 ReportRecord
        /// <summary>
        /// 报工记录Id
        /// </summary>
        [Label("报工记录")]
        public static readonly IRefIdProperty ReportRecordIdProperty =
            P<BatchTracebackKeyDtl>.RegisterRefId(e => e.ReportRecordId, ReferenceType.Normal);

        /// <summary>
        /// 报工记录Id
        /// </summary>
        public double? ReportRecordId
        {
            get { return (double?)this.GetRefNullableId(ReportRecordIdProperty); }
            set { this.SetRefNullableId(ReportRecordIdProperty, value); }
        }

        /// <summary>
        /// 报工记录
        /// </summary>
        public static readonly RefEntityProperty<ReportRecord> ReportRecordProperty =
            P<BatchTracebackKeyDtl>.RegisterRef(e => e.ReportRecord, ReportRecordIdProperty);

        /// <summary>
        /// 报工记录
        /// </summary>
        public ReportRecord ReportRecord
        {
            get { return this.GetRefEntity(ReportRecordProperty); }
            set { this.SetRefEntity(ReportRecordProperty, value); }
        }
        #endregion

        #region 工序 ProcessCode
        /// <summary>
        /// 工序
        /// </summary>
        [Label("工序")]
        public static readonly Property<string> ProcessCodeProperty = P<BatchTracebackKeyDtl>.Register(e => e.ProcessCode);

        /// <summary>
        /// 工序
        /// </summary>
        public string ProcessCode
        {
            get { return this.GetProperty(ProcessCodeProperty); }
            set { this.SetProperty(ProcessCodeProperty, value); }
        }
        #endregion

        #region 来源条码 SourceSn
        /// <summary>
        /// 来源条码
        /// </summary>
        [Label("来源条码")]
        public static readonly Property<string> SourceSnProperty = P<BatchTracebackKeyDtl>.Register(e => e.SourceSn);

        /// <summary>
        /// 来源条码
        /// </summary>
        public string SourceSn
        {
            get { return this.GetProperty(SourceSnProperty); }
            set { this.SetProperty(SourceSnProperty, value); }
        }
        #endregion

        #region 批次标签 ItemLabelLot
        /// <summary>
        /// 批次标签
        /// </summary>
        [Label("批次标签")]
        public static readonly Property<string> ItemLabelLotProperty = P<BatchTracebackKeyDtl>.Register(e => e.ItemLabelLot);

        /// <summary>
        /// 批次标签
        /// </summary>
        public string ItemLabelLot
        {
            get { return this.GetProperty(ItemLabelLotProperty); }
            set { this.SetProperty(ItemLabelLotProperty, value); }
        }
        #endregion

        #region 用料量 DeductedQty
        /// <summary>
        /// 用料量
        /// </summary>
        [Label("用料量")]
        public static readonly Property<decimal> DeductedQtyProperty = P<BatchTracebackKeyDtl>.Register(e => e.DeductedQty);

        /// <summary>
        /// 用料量
        /// </summary>
        public decimal DeductedQty
        {
            get { return this.GetProperty(DeductedQtyProperty); }
            set { this.SetProperty(DeductedQtyProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<BatchTracebackKeyDtl>.Register(e => e.ItemCode);

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
        public static readonly Property<string> ItemNameProperty = P<BatchTracebackKeyDtl>.Register(e => e.ItemName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { this.SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 扣料物料旧料号 ShortDescription
        /// <summary>
        /// 扣料物料旧料号
        /// </summary>
        [Label("扣料物料旧料号")]
        public static readonly Property<string> ShortDescriptionProperty = P<BatchTracebackKeyDtl>.Register(e => e.ShortDescription);

        /// <summary>
        /// 扣料物料旧料号
        /// </summary>
        public string ShortDescription
        {
            get { return this.GetProperty(ShortDescriptionProperty); }
            set { this.SetProperty(ShortDescriptionProperty, value); }
        }
        #endregion

        #region 单位 Unit
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitProperty = P<BatchTracebackKeyDtl>.Register(e => e.Unit);

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit
        {
            get { return this.GetProperty(UnitProperty); }
            set { this.SetProperty(UnitProperty, value); }
        }
        #endregion
    }

    internal class BatchTracebackKeyDtlConfig : EntityConfig<BatchTracebackKeyDtl>
    {
        protected override void ConfigMeta()
        {
            Func<IQuery> view = () => DB.Query<DeductionRecord>("dr")
            .Join<ReportRecord>("rr", (x, y) => x.ReportRecordId == y.Id && y.SQL<int>("rr.IS_PHANTOM") == 0)
            .Join<ReportRecord, Process>("p", (x, y) => x.ProcessId == y.Id && y.SQL<int>("p.IS_PHANTOM") == 0)
            .Join<ItemLabel>("il", (x, y) => x.ItemLabelId == y.Id && y.SQL<int>("il.IS_PHANTOM") == 0)
            .Join<ItemLabel, Item>("i", (x, y) => x.ItemId == y.Id && y.SQL<int>("i.IS_PHANTOM") == 0)
            .Join<Item, Unit>("u", (x, y) => x.UnitId == y.Id && y.SQL<int>("u.IS_PHANTOM") == 0)
            .Where(p => p.SQL<int>("dr.IS_PHANTOM") == 0 && p.SQL<int?>("dr.INV_ORG_ID") == RT.InvOrg)
            .Select<ReportRecord, Process, ItemLabel, Item, Unit>((dr, rr, p, il, i, u) => new
            {
                Id = dr.Id,
                Report_Record_Id = dr.ReportRecordId,
                Process_Code = p.Code,
                Source_Sn = dr.FeedingItemLabel,
                Item_Label_Lot = il.Lot,
                Deducted_Qty = dr.DeductedQty,
                Item_Code = i.Code,
                Item_Name = i.Name,
                Short_Description = i.ShortDescription,
                Unit = u.Code
            })
            .ToQuery();
            Meta.MapView(view).MapAllProperties();
        }
    }
}
