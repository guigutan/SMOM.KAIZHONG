using System;
using System.Collections.Generic;

namespace SIE.MES.TeamManagement.ApiInterfaces
{
    /// <summary>
    /// 借调单API信息类
    /// </summary>
    [Serializable]
    public class OnLoanInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public OnLoanInfo()
        {
            DetailList = new List<OnLoanDetailInfo>();
        }

        /// <summary>
        /// 借调单ID
        /// </summary>
        public double? Id { get; set; }

        /// <summary>
        /// 借调单号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 借调开始时间
        /// </summary>
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// 借调结束时间
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 需求人数
        /// </summary>
        public decimal DemandQty { get; set; }

        /// <summary>
        /// 借出班组ID
        /// </summary>
        public double GroupOutId { get; set; }

        /// <summary>
        /// 借出班组名称
        /// </summary>
        public string GroupOutName { get; set; }

        /// <summary>
        /// 借出日期
        /// </summary>
        public DateTime OnLoanDate { get; set; }

        /// <summary>
        /// 班次ID
        /// </summary>
        public double ShiftId { get; set; }

        /// <summary>
        /// 班次开始时间
        /// </summary>
        public DateTime ShiftBeginTime { get; set; }

        /// <summary>
        /// 班次结束时间
        /// </summary>
        public DateTime ShiftEndTime { get; set; }

        /// <summary>
        /// 借入班组ID
        /// </summary>
        public double GroupInId { get; set; }

        /// <summary>
        /// 借入班组名称
        /// </summary>
        public string GroupInName { get; set; }

        /// <summary>
        /// 发起人
        /// </summary>
        public double InitiatorId { get; set; }

        /// <summary>
        /// 发起人姓名
        /// </summary>
        public string InitiateName { get; set; }

        /// <summary>
        /// 发起时间
        /// </summary>
        public DateTime InitiateDate { get; set; }

        /// <summary>
        /// 借调单状态
        /// 待批复、已同意、已拒绝、已撤销
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

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
        /// 在编人数
        /// </summary>
        public decimal ActualQty { get; set; }

        /// <summary>
        /// 到岗人数
        /// </summary>
        public decimal ClockingInQty { get; set; }

        /// <summary>
        /// 明细列表
        /// </summary>
        public List<OnLoanDetailInfo> DetailList { get; set; }
    }
}
