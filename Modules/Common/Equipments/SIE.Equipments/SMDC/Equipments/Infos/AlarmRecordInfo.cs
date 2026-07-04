using System;

namespace SIE.Equipments.SMDC.Equipments.Infos
{
    /// <summary>
    /// 报警信息
    /// </summary>
    public class AlarmRecordInfo
    {
        /// <summary>
        /// 报警规则类型
        /// </summary>
        public int AlarmRuleType { get; set; }

        /// <summary>
        /// 报警唯一ID
        /// </summary>
        public string Uid { get; set; }

        /// <summary>
        /// 报警Id
        /// </summary>
        public int AlarmId { get; set; }

        /// <summary>
        /// 报警路径
        /// </summary>
        public string LinkAlarmPath { get; set; }

        /// <summary>
        /// TAG全称
        /// </summary>
        public string LinkTagFullName { get; set; }

        /// <summary>
        /// 报警类型
        /// </summary>
        public string AlarmType { get; set; }

        /// <summary>
        /// 报警级别
        /// </summary>
        public int AlarmLevel { get; set; }

        /// <summary>
        /// 报警值
        /// </summary>
        public double AlarmValue { get; set; }

        /// <summary>
        /// 明细值
        /// </summary>
        public string AlarmChildrenValue { get; set; }

        /// <summary>
        /// 限制值
        /// </summary>
        public double LimitValue { get; set; }

        /// <summary>
        /// 恢复值
        /// </summary>
        public double? RecoveryValue { get; set; }

        /// <summary>
        /// 报警状态
        /// </summary>
        public int AlarmStatus { get; set; }

        /// <summary>
        /// 报警来源
        /// </summary>
        public string AlarmSource { get; set; }

        /// <summary>
        /// 报警内容
        /// </summary>
        public string AlarmContent { get; set; }

        /// <summary>
        /// 报警原因
        /// </summary>
        public string AlarmReason { get; set; }

        /// <summary>
        /// 触发时间
        /// </summary>
        public DateTime TriggerTime { get; set; }

        /// <summary>
        /// 确认时间
        /// </summary>
        public DateTime? AckedTime { get; set; }

        /// <summary>
        /// 恢复时间
        /// </summary>
        public DateTime? RecoveryTime { get; set; }

        /// <summary>
        /// 确认原因
        /// </summary>
        public string AckReason { get; set; }

        /// <summary>
        /// 确认来源
        /// </summary>
        public string AckSource { get; set; }

        /// <summary>
        /// 改善策略
        /// </summary>
        public string ImproveStrategy { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime TimeStamp { get; set; }
    }
}
