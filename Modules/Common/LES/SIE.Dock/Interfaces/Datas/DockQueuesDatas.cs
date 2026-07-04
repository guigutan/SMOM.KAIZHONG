using SIE.Dock.DockAppoints;
using SIE.Dock.DockQueues;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Dock.Interfaces.Datas
{
    /// <summary>
    /// 月台排队数据
    /// </summary>
    public class DockQueuesDatas
    {
        /// <summary>
        /// 排队ID
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 排队号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 排队状态
        /// </summary>
        public QueueState QueueState { get; set; }

        /// <summary>
        /// 排队状态显示
        /// </summary>
        public string QueueStateStr { get; set; }

        /// <summary>
        /// 排队类型
        /// </summary>
        public AppointType AppointType { get; set; }

        /// <summary>
        /// 排队类型显示
        /// </summary>
        public string AppointTypeStr { get; set; }

        /// <summary>
        /// 优先级
        /// </summary>
        public QueuePriority QueuePriority { get; set; }

        /// <summary>
        /// 优先级显示
        /// </summary>
        public string QueuePriorityStr { get; set; }

        /// <summary>
        /// 单据号
        /// </summary>
        public string BillNo { get; set; }

        /// <summary>
        /// 车牌号码
        /// </summary>
        public string CarNum { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string Contacts { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactNum { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 创建时间显示
        /// </summary>
        public string CreateDateStr { get; set; }

        /// <summary>
        /// 分配时间
        /// </summary>
        public DateTime? DistributionTime { get; set; }

        /// <summary>
        /// 签到时间(装卸开始时间)
        /// </summary>
        public DateTime? CheckInTime { get; set; }

        /// <summary>
        /// 签到时间(装卸开始时间)显示
        /// </summary>
        public string CheckInTimeStr { get; set; }

        /// <summary>
        /// 签出时间(装卸结束时间)
        /// </summary>
        public DateTime? CheckOutTime { get; set; }

        /// <summary>
        /// 签出时间(装卸结束时间)显示
        /// </summary>
        public string CheckOutTimeStr { get; set; }

        /// <summary>
        /// 排队地点编码
        /// </summary>
        public string YardMaintainCode { get; set; }

        /// <summary>
        /// 排队地点名称
        /// </summary>
        public string YardMaintainName { get; set; }

        /// <summary>
        /// 排队地点ID
        /// </summary>
        public double YardMaintainId { get; set; }

        /// <summary>
        /// 分配月台名称
        /// </summary>
        public string AssignDockName { get; set; }

        /// <summary>
        /// 服务器时间
        /// </summary>
        public DateTime MachineTime { get; set; }

        /// <summary>
        /// 身份证号码
        /// </summary>
        public string IDNumber { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 预约号
        /// </summary>
        public string AppointNo { get; set; }
    }

    /// <summary>
    /// 月台数据
    /// </summary>
    public class DockData
    {
        /// <summary>
        /// 月台ID
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 月台编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 月台名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 占用情况
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 是否收货
        /// </summary>
        public bool IsReceive { get; set; }

        /// <summary>
        /// 是否发货
        /// </summary>
        public bool IsShip { get; set; }
    }

    /// <summary>
    /// 用户数据
    /// </summary>
    public class AppointsUserData
    {
        /// <summary>
        /// 联系人
        /// </summary>
        public string Contacts { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactNum { get; set; }

        /// <summary>
        /// 车牌号码
        /// </summary>
        public string CarNum { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string IDNumber { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 预约类型
        /// </summary>
        public AppointType AppointType { get; set; }

        /// <summary>
        /// 预约类型
        /// </summary>
        public string AppointTypeStr { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateDate { get; set; }

        /// <summary>
        /// 预约开始时间
        /// </summary>
        public string AppointStartDate { get; set; }

        /// <summary>
        /// 预约结束时间
        /// </summary>
        public string AppointEndDate { get; set; }

        /// <summary>
        /// 预约时间
        /// </summary>
        public string AppointDate { get; set; }
    }

    /// <summary>
    /// 圆片区
    /// </summary>
    public class YardZoneData: DockData
    {

    }

    /// <summary>
    /// 时间段数据
    /// </summary>
    public class TimePeriodData 
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// 显示时间
        /// </summary>
        public string AppointTimeDisplay { get; set; }

        /// <summary>
        /// 预计占用时间
        /// </summary>
        public double AppointUseTime { get; set; }

        /// <summary>
        /// 月台剩余最大时间
        /// </summary>
        public double MaxRestTime { get; set; }

    }

    /// <summary>
    /// 预约配置项数据
    /// </summary>
    public class AppointConfig
    {
        /// <summary>
        /// 配置项配置的最大时间
        /// </summary>
        public decimal? MaxTime { get; set; }

        /// <summary>
        /// 月台最大时间
        /// </summary>
        public string MaxDockTime { get; set; }

        /// <summary>
        /// 当前日期
        /// </summary>
        public string NowDate { get; set; }
    }

    /// <summary>
    /// 园区排队数据和等候数据
    /// </summary>
    public class ZonesAwaitData 
    {
        /// <summary>
        /// 送货排队数量
        /// </summary>
        public int ReceiveCount { get; set; }

        /// <summary>
        /// 提货排队数量
        /// </summary>
        public int ShipCount { get; set; }

        /// <summary>
        /// 送货等待时间
        /// </summary>
        public decimal ReceiveAwaitTime { get; set; }

        /// <summary>
        /// 提货等待时间
        /// </summary>
        public decimal ShipAwaitTime { get; set; }
    }

    /// <summary>
    /// 预约数据
    /// </summary>
    public class AppointData : AppointsUserData
    {
        /// <summary>
        /// 预约号ID
        /// </summary>
        public double DockAppointId { get; set; }

        /// <summary>
        /// 占用时间
        /// </summary>
        public double AppointUseTime { get; set; }

        /// <summary>
        /// 预约状态
        /// </summary>
        public ApointStatus ApointStatus { get; set; }

        /// <summary>
        /// 预约状态显示
        /// </summary>
        public string ApointStatusStr { get; set; }

        /// <summary>
        /// 类型显示
        /// </summary>
        public string AppointTypeStr { get; set; }

        /// <summary>
        /// 单据号
        /// </summary>
        public string BillNo { get; set; }

        /// <summary>
        /// 预约地点
        /// </summary>
        public double YardZoneId { get; set; }

        /// <summary>
        /// 预约地点名称
        /// </summary>
        public string YardZoneName { get; set; }

        /// <summary>
        /// 预约日期
        /// </summary>
        public string AppointDate { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 身份证号码
        /// </summary>
        public string IDNumber { get; set; }

        /// <summary>
        /// 预约号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 取消时间
        /// </summary>
        public string CancelDate { get; set; }
    }
}
