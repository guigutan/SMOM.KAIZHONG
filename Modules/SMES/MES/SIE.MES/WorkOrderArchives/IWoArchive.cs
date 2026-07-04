using SIE.Domain;
using SIE.MES.Interfaces.TaskManages;
using SIE.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.WorkOrderArchives
{
    /// <summary>
    /// 报工记录接口实体
    /// </summary>
    [Serializable]
    public class ReportRecordSimpleInfo : DataEntity
    {
        /// <summary>
        /// 报工管理Id
        /// </summary>
        public double DispatchTaskId { get; set; }

        /// <summary>
        /// 报工管理任务单号
        /// </summary>
        public string DispatchTaskNo { get; set; }

        /// <summary>
        /// 任务单状态
        /// </summary>
        public string DispatchTaskState { get; set; }

        /// <summary>
        /// 任务单数量
        /// </summary>
        public decimal DispatchQty { get; set; }

        /// <summary>
        /// 任务单合格数量
        /// </summary>
        public decimal OkQty { get; set; }

        /// <summary>
        /// 任务单不合格数量
        /// </summary>
        public decimal NgQty { get; set; } 

        /// <summary>
        /// 任务单报工数量
        /// </summary>
        public decimal ReportQty { get; set; }

        /// <summary>
        /// 报工工单Id
        /// </summary>
        public double? WorkOrderID { get; set; }

        /// <summary>
        /// 报工数
        /// </summary>
        public decimal RecordReportQty { get; set; }

        /// <summary>
        /// 合格数
        /// </summary>
        public decimal RecordOkQty { get; set; }

        /// <summary>
        /// 不合格数
        /// </summary>
        public decimal RecordNgQty { get; set; }

        /// <summary>
        /// 统计小时
        /// </summary>
        public decimal Hour { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 报工时间
        /// </summary>
        public DateTime? ReportTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 是否已报工
        /// </summary>
        public bool IsReport { get; set; }

        /// <summary>
        /// 负责人Id
        /// </summary>
        public double PrincipalId { get; set; }

        /// <summary>
        /// 负责人名称
        /// </summary>
        public string PrincipalName { get; set; }

        /// <summary>
        /// 缺陷
        /// </summary>
        public string Defects { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public double? ProcessId { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// 工位Id
        /// </summary>
        public double? StationId { get; set; }

        /// <summary>
        /// 工位名称
        /// </summary>
        public string StationName { get; set; }

        /// <summary>
        /// 规格件编码
        /// </summary>
        public string SpecificationCode { get; set; }

        /// <summary>
        /// 规格件名称
        /// </summary>
        public string SpecificationName { get; set; }

        /// <summary>
        /// 是否虚拟件
        /// </summary>
        public bool IsVirtualPart { get; set; }

        /// <summary>
        /// 虚拟件编码
        /// </summary>
        public string VirtualPartCode { get; set; }

        /// <summary>
        /// 虚拟件名称
        /// </summary>
        public string VirtualPartName { get; set; }

        /// <summary>
        /// 报工方式
        /// </summary>
        public string ReportMode { get; set; }
    }

    /// <summary>
    /// 工单管理
    /// </summary>
    [Service(FallbackType = typeof(DefaultWoArchive))]
    public interface IWoArchive
    {
        /// <summary>
        /// 根据Id获取报工记录
        /// </summary>
        /// <returns></returns>
        EntityList<ReportRecordSimpleInfo> GetReportRecords(double workOrderId, IList<OrderInfo> sortInfo, PagingInfo pagingInfo);
    }

    /// <summary>
    /// 默认实现
    /// </summary>
    public class DefaultWoArchive: IWoArchive
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <param name="sortInfo"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public EntityList<ReportRecordSimpleInfo> GetReportRecords(double workOrderId, IList<OrderInfo> sortInfo, PagingInfo pagingInfo)
        {
            return new EntityList<ReportRecordSimpleInfo>();
        }
    }
}
