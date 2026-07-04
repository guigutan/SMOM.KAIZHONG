using SIE.Andon.Andons.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Andon.Andons
{
    /// <summary>
    /// 安灯管理操作记录
    /// </summary>
    [Serializable]
    public class AndonManageOperate
    {
        /// <summary>
        /// 安灯管理父Id
        /// </summary>
        public double AndonManageId { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public AndonManageOperateType OperateType { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// 转派安灯Id
        /// </summary>
        public double ReassignAndonId { get; set; }

        /// <summary>
        /// 转派员工Id
        /// </summary>
        public double? ReassignEmployeeId { get; set; }

        /// <summary>
        /// 实际影响时间
        /// </summary>
        public double? ActualTime { get; set; }
    }
}
