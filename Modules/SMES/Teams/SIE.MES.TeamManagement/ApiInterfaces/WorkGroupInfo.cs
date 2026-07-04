using System;

namespace SIE.MES.TeamManagement.ApiInterfaces
{
    /// <summary>
    /// 班组人员API信息类
    /// </summary>
    [Serializable]
    public class WorkGroupInfo
    {
        /// <summary>
        /// 班组ID
        /// </summary>
        public double WorkGroupId { get; set; }

        /// <summary>
        /// 班组编码
        /// </summary>
        public string WorkGroupCode { get; set; }

        /// <summary>
        /// 班组名称
        /// </summary>
        public string WorkGroupName { get; set; }

        /// <summary>
        /// 班长ID
        /// </summary>
        public double EmployeeId { get; set; }

        /// <summary>
        /// 班长名称
        /// </summary>
        public string EmployeeName { get; set; }

        /// <summary>
        /// 所在资源编码
        /// </summary>
        public string ResourceCode { get; set; }

        /// <summary>
        /// 所在资源名称
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// 班次ID
        /// </summary>
        public double ShiftId { get; set; }

        /// <summary>
        /// 班次开始时间
        /// </summary>
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// 班次结束时间
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 在编人数
        /// </summary>
        public decimal ActualQty { get; set; }

        /// <summary>
        /// 到岗人数
        /// </summary>
        public decimal ClockingInQty { get; set; }
    }
}
