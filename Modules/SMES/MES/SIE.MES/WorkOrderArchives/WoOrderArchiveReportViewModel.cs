using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.WorkOrderArchives
{
    /// <summary>
    /// 工单制造档案报工记录
    /// </summary>
    [RootEntity, Serializable]
    [Label("工单制造档案报工记录")]
    public class WoOrderArchiveReportViewModel : ViewModel
    {
        #region 任务单号 TaskNo
        /// <summary>
        /// 任务单号
        /// </summary>
        [Label("任务单号")]
        public static readonly Property<string> TaskNoProperty = P<WoOrderArchiveReportViewModel>.Register(e => e.TaskNo);

        /// <summary>
        /// 任务单号
        /// </summary>
        public string TaskNo
        {
            get { return this.GetProperty(TaskNoProperty); }
            set { this.SetProperty(TaskNoProperty, value); }
        }
        #endregion

        #region 任务状态 TaskState
        /// <summary>
        /// 任务状态
        /// </summary>
        [Label("任务状态")]
        public static readonly Property<string> TaskStateProperty = P<WoOrderArchiveReportViewModel>.Register(e => e.TaskState);

        /// <summary>
        /// 任务状态
        /// </summary>
        public string TaskState
        {
            get { return this.GetProperty(TaskStateProperty); }
            set { this.SetProperty(TaskStateProperty, value); }
        }
        #endregion

        #region 任务数量 DispatchQty
        /// <summary>
        /// 任务数量
        /// </summary>
        [Label("任务数量")]
        public static readonly Property<decimal> DispatchQtyProperty = P<WoOrderArchiveReportViewModel>.Register(e => e.DispatchQty);

        /// <summary>
        /// 任务数量
        /// </summary>
        public decimal DispatchQty
        {
            get { return this.GetProperty(DispatchQtyProperty); }
            set { this.SetProperty(DispatchQtyProperty, value); }
        }
        #endregion

        #region 合格数(总和) OkQty
        /// <summary>
        /// 合格数(总和)
        /// </summary>
        [Label("合格数(总和)")]
        public static readonly Property<decimal> OkQtyProperty = P<WoOrderArchiveReportViewModel>.Register(e => e.OkQty);

        /// <summary>
        /// 合格数(总和)
        /// </summary>
        public decimal OkQty
        {
            get { return this.GetProperty(OkQtyProperty); }
            set { this.SetProperty(OkQtyProperty, value); }
        }
        #endregion

        #region 不合格数(总和) NgQty
        /// <summary>
        /// 不合格数(总和)
        /// </summary>
        [Label("不合格数(总和)")]
        public static readonly Property<decimal> NgQtyProperty = P<WoOrderArchiveReportViewModel>.Register(e => e.NgQty);

        /// <summary>
        /// 不合格数(总和)
        /// </summary>
        public decimal NgQty
        {
            get { return this.GetProperty(NgQtyProperty); }
            set { this.SetProperty(NgQtyProperty, value); }
        }
        #endregion

        #region 报工数量(总和) ReportQty
        /// <summary>
        /// 报工数量(总和)
        /// </summary>
        [Label("报工数量(总和)")]
        public static readonly Property<decimal> ReportQtyProperty = P<WoOrderArchiveReportViewModel>.Register(e => e.ReportQty);

        /// <summary>
        /// 报工数量(总和)
        /// </summary>
        public decimal ReportQty
        {
            get { return this.GetProperty(ReportQtyProperty); }
            set { this.SetProperty(ReportQtyProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序
        /// </summary>
        [Label("工序")]
        public static readonly Property<string> ProcessProperty = P<WoOrderArchiveReportViewModel>.Register(e => e.Process);

        /// <summary>
        /// 工序
        /// </summary>
        public string Process
        {
            get { return this.GetProperty(ProcessProperty); }
            set { this.SetProperty(ProcessProperty, value); }
        }
        #endregion

        #region 负责人 Charger
        /// <summary>
        /// 负责人
        /// </summary>
        [Label("负责人")]
        public static readonly Property<string> ChargerProperty = P<WoOrderArchiveReportViewModel>.Register(e => e.Charger);

        /// <summary>
        /// 负责人
        /// </summary>
        public string Charger
        {
            get { return this.GetProperty(ChargerProperty); }
            set { this.SetProperty(ChargerProperty, value); }
        }
        #endregion

        #region 报工时间 TaskTime
        /// <summary>
        /// 报工时间
        /// </summary>
        [Label("报工时间")]
        public static readonly Property<DateTime?> TaskTimeProperty = P<WoOrderArchiveReportViewModel>.Register(e => e.TaskTime);

        /// <summary>
        /// 报工时间
        /// </summary>
        public DateTime? TaskTime
        {
            get { return this.GetProperty(TaskTimeProperty); }
            set { this.SetProperty(TaskTimeProperty, value); }
        }
        #endregion


        #region 报工数 RecordReportQty
        /// <summary>
        /// 报工数
        /// </summary>
        [Label("报工数")]
        public static readonly Property<decimal> RecordReportQtyProperty = P<WoOrderArchiveReportViewModel>.Register(e => e.RecordReportQty);

        /// <summary>
        /// 报工数
        /// </summary>
        public decimal RecordReportQty
        {
            get { return this.GetProperty(RecordReportQtyProperty); }
            set { this.SetProperty(RecordReportQtyProperty, value); }
        }
        #endregion

        #region 合格数 RecordOkQty
        /// <summary>
        /// 合格数
        /// </summary>
        [Label("合格数")]
        public static readonly Property<decimal> RecordOkQtyProperty = P<WoOrderArchiveReportViewModel>.Register(e => e.RecordOkQty);

        /// <summary>
        /// 合格数
        /// </summary>
        public decimal RecordOkQty
        {
            get { return this.GetProperty(RecordOkQtyProperty); }
            set { this.SetProperty(RecordOkQtyProperty, value); }
        }
        #endregion

        #region 不合格数 RecordNgQty
        /// <summary>
        /// 不合格数
        /// </summary>
        [Label("不合格数")]
        public static readonly Property<decimal> RecordNgQtyProperty = P<WoOrderArchiveReportViewModel>.Register(e => e.RecordNgQty);

        /// <summary>
        /// 不合格数
        /// </summary>
        public decimal RecordNgQty
        {
            get { return this.GetProperty(RecordNgQtyProperty); }
            set { this.SetProperty(RecordNgQtyProperty, value); }
        }
        #endregion

        #region 统计工时(小时) Hour
        /// <summary>
        /// 统计工时(小时)
        /// </summary>
        [Label("统计工时(小时)")]
        public static readonly Property<decimal> HourProperty = P<WoOrderArchiveReportViewModel>.Register(e => e.Hour);

        /// <summary>
        /// 统计工时(小时)
        /// </summary>
        public decimal Hour
        {
            get { return this.GetProperty(HourProperty); }
            set { this.SetProperty(HourProperty, value); }
        }
        #endregion

        #region 工位 Station
        /// <summary>
        /// 工位
        /// </summary>
        [Label("工位")]
        public static readonly Property<string> StationProperty = P<WoOrderArchiveReportViewModel>.Register(e => e.Station);

        /// <summary>
        /// 工位
        /// </summary>
        public string Station
        {
            get { return this.GetProperty(StationProperty); }
            set { this.SetProperty(StationProperty, value); }
        }
        #endregion

        #region 批次号 BatchNo
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> BatchNoProperty = P<WoOrderArchiveReportViewModel>.Register(e => e.BatchNo);

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo
        {
            get { return this.GetProperty(BatchNoProperty); }
            set { this.SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 报工时间 ReportTime
        /// <summary>
        /// 报工时间
        /// </summary>
        [Label("报工时间")]
        public static readonly Property<DateTime?> ReportTimeProperty = P<WoOrderArchiveReportViewModel>.Register(e => e.ReportTime);

        /// <summary>
        /// 报工时间
        /// </summary>
        public DateTime? ReportTime
        {
            get { return this.GetProperty(ReportTimeProperty); }
            set { this.SetProperty(ReportTimeProperty, value); }
        }
        #endregion

        #region 缺陷 Defects
        /// <summary>
        /// 缺陷
        /// </summary>
        [Label("缺陷")]
        public static readonly Property<string> DefectsProperty = P<WoOrderArchiveReportViewModel>.Register(e => e.Defects);

        /// <summary>
        /// 缺陷
        /// </summary>
        public string Defects
        {
            get { return this.GetProperty(DefectsProperty); }
            set { this.SetProperty(DefectsProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<WoOrderArchiveReportViewModel>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return this.GetProperty(RemarkProperty); }
            set { this.SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 规格件编码 SpecificationCode
        /// <summary>
        /// 规格件编码
        /// </summary>
        [Label("规格件编码")]
        public static readonly Property<string> SpecificationCodeProperty = P<WoOrderArchiveReportViewModel>.Register(e => e.SpecificationCode);

        /// <summary>
        /// 规格件编码
        /// </summary>
        public string SpecificationCode
        {
            get { return this.GetProperty(SpecificationCodeProperty); }
            set { this.SetProperty(SpecificationCodeProperty, value); }
        }
        #endregion

        #region 规格件名称 SpecificationName
        /// <summary>
        /// 规格件名称
        /// </summary>
        [Label("规格件名称")]
        public static readonly Property<string> SpecificationNameProperty = P<WoOrderArchiveReportViewModel>.Register(e => e.SpecificationName);

        /// <summary>
        /// 规格件名称
        /// </summary>
        public string SpecificationName
        {
            get { return this.GetProperty(SpecificationNameProperty); }
            set { this.SetProperty(SpecificationNameProperty, value); }
        }
        #endregion

        #region 是否虚拟件 IsVirtualPart
        /// <summary>
        /// 是否虚拟件
        /// </summary>
        [Label("是否虚拟件")]
        public static readonly Property<bool> IsVirtualPartProperty = P<WoOrderArchiveReportViewModel>.Register(e => e.IsVirtualPart);

        /// <summary>
        /// 是否虚拟件
        /// </summary>
        public bool IsVirtualPart
        {
            get { return this.GetProperty(IsVirtualPartProperty); }
            set { this.SetProperty(IsVirtualPartProperty, value); }
        }
        #endregion

        #region 虚拟件编码 VirtualPartCode
        /// <summary>
        /// 虚拟件编码
        /// </summary>
        [Label("虚拟件编码")]
        public static readonly Property<string> VirtualPartCodeProperty = P<WoOrderArchiveReportViewModel>.Register(e => e.VirtualPartCode);

        /// <summary>
        /// 虚拟件编码
        /// </summary>
        public string VirtualPartCode
        {
            get { return this.GetProperty(VirtualPartCodeProperty); }
            set { this.SetProperty(VirtualPartCodeProperty, value); }
        }
        #endregion

        #region 虚拟件名称 VirtualPartName
        /// <summary>
        /// 虚拟件名称
        /// </summary>
        [Label("虚拟件名称")]
        public static readonly Property<string> VirtualPartNameProperty = P<WoOrderArchiveReportViewModel>.Register(e => e.VirtualPartName);

        /// <summary>
        /// 虚拟件名称
        /// </summary>
        public string VirtualPartName
        {
            get { return this.GetProperty(VirtualPartNameProperty); }
            set { this.SetProperty(VirtualPartNameProperty, value); }
        }
        #endregion

        #region 报工方式 ReportMode
        /// <summary>
        /// 报工方式
        /// </summary>
        [Label("报工方式")]
        public static readonly Property<string> ReportModeProperty = P<WoOrderArchiveReportViewModel>.Register(e => e.ReportMode);

        /// <summary>
        /// 报工方式
        /// </summary>
        public string ReportMode
        {
            get { return this.GetProperty(ReportModeProperty); }
            set { this.SetProperty(ReportModeProperty, value); }
        }
        #endregion

    }
}
