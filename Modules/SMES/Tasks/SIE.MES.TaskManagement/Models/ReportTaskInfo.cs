using SIE.MES.TaskManagement.Reports.Enums;
using System;
using System.Collections.Generic;

namespace SIE.MES.TaskManagement.Models
{
    /// <summary>
    /// 报工任务信息
    /// </summary>
    [Serializable]
    public class ReportTaskInfo
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        public double TaskId { get; set; }

        /// <summary>
        /// 任务单号
        /// </summary>
        public string TaskNo { get; set; }

        /// <summary>
        /// 任务单总数
        /// </summary>
        public decimal TaskQty { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 工单ID
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 车间
        /// </summary>
        public string WorkShopName { get; set; }

        /// <summary>
        /// 资源ID
        /// </summary>
        public double? ResourceId { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public double? ProcessId { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// 工位ID
        /// </summary>
        public double? StationId { get; set; }

        /// <summary>
        /// 已报工合格数
        /// </summary>
        public decimal OkQty { get; set; }

        /// <summary>
        /// 已报工不合格数
        /// </summary>
        public decimal NgQty { get; set; }

        /// <summary>
        /// 未报工数量
        /// </summary>
        public decimal ToReportQty { get; set; }

        /// <summary>
        /// 合格数
        /// </summary>
        public decimal ReportOkQty { get; set; }

        /// <summary>
        /// 不合格数
        /// </summary>
        public decimal ReportNgQty { get; set; }

        /// <summary>
        /// 统计工时
        /// </summary>
        public decimal Hour { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 是否共模
        /// </summary>
        public bool IsSyntype { get; set; }

        ///// <summary>
        ///// 主料共模模具数
        ///// </summary>
        //public double MainProportion { get; set; }

        /// <summary>
        /// 辅料共模模具数(当前是主料则为null)主料模具数在reportrecord中，不映射数据库，每次调用时候生成
        /// </summary>
        public double Proportion { get; set; }

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime PlanBeginDate { get; set; }

        /// <summary>
        /// 计划结束时间
        /// </summary>
        public DateTime PlanEndDate { get; set; }

        /// <summary>
        /// 实际开始时间
        /// </summary>
        public DateTime? ActualBeginDate { get; set; }

        /// <summary>
        /// 报工记录ID
        /// </summary>
        public double RecordId { get; set; }

        /// <summary>
        /// 缺陷描述名称，分号拼接
        /// </summary>
        public string DefectNames { get; set; }

        /// <summary>
        /// 关联设备信息
        /// </summary>
        public string Equipments { get; set; }

        /// <summary>
        /// 报工缺陷ID列表
        /// </summary>
        public List<double> DefectIds { get; set; } = new List<double>();

        /// <summary>
        /// 共模任务信息列表
        /// </summary>
        public List<ReportTaskInfo> SyntypeTaskInfos { get; set; } = new List<ReportTaskInfo>();

        /// <summary>
        /// PDA上显示计划时间范围
        /// </summary>
        public string PdaPlanTime { get; set; }

        /// <summary>
        /// PDA上显示实际时间范围
        /// </summary>
        public string PdaActualTime { get; set; }

        /// <summary>
        /// 关联工单号
        /// </summary>
        public string AssociatedWorkOrder { get; set; }

        /// <summary>
        /// 员工Id
        /// </summary>
        public double? StaffId { get; set; }

        /// <summary>
        /// 可疑品数量
        /// </summary>
        public decimal? SuspectQty { get; set; }
        /// <summary>
        /// 可疑品数量
        /// </summary>
        public decimal ReworkQty { get; set; }


        /// <summary>
        /// 是否可疑品处理
        /// </summary>
        public bool IsSuspect { get; set; }

        /// <summary>
        /// 报工员Id
        /// </summary>
        public double ReportEmployeeId { get; set; }


        /// <summary>
        /// 是否跳过前工序报工数量验证
        /// </summary>
        public bool IsSkipValidatePreQty { get; set; }

        /// <summary>
        /// 是否校验开机前置
        /// </summary>
        public bool IsValidatePrepare { get; set; }

        /// <summary>
        /// 是否完工(只有手动、扫码弹窗选择的时候否的时候才输入false)
        /// </summary>
        public bool IsTaskFinish { get; set; }
        /// <summary>
        /// 是否自动上料(适用于成品工单包装报工)
        /// </summary>
        public bool IsAutoFeeding { get; set; }
        /// <summary>
        /// 是否共模报工
        /// </summary>
        public bool IsCommonMode { get; set; }
        
        /// <summary>
        /// 来源类型
        /// </summary>
        public SourceType? SourceType { get; set; }
    }
}