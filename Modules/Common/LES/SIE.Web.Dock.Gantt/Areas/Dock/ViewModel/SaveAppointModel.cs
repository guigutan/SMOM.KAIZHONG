using SIE.Dock.DockQueues;
using System;

namespace SIE.Web.Dock.Gantt.Areas.Dock.ViewModel
{
    /// <summary>
    /// 保存的预约信息
    /// </summary>
    [Serializable]
    public class SaveAppointModel
    {
        /// <summary>
        /// 预约Id
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public string startDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string endDate { get; set; }

        /// <summary>
        /// 时间单位
        /// </summary>
        public string durationUnit { get; set; }

        /// <summary>
        /// 单位值
        /// </summary>
        public double duration { get; set; }

        /// <summary>
        /// 开始时间段
        /// </summary>
        public string startDateRange { get; set; }

        /// <summary>
        /// 结束时间段
        /// </summary>
        public string endDateRange { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        public string eventColor { get; set; }

        /// <summary>
        /// 月台Id
        /// </summary>
        public double resourceId { get; set; }

        /// <summary>
        /// 预约类型
        /// </summary>
        public int orderType { get; set; }

        /// <summary>
        /// 单号
        /// </summary>
        public string billNo { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string orderPlace { get; set; }

        /// <summary>
        /// 预约号
        /// </summary>
        public string orderNo { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        public string carNumber { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string contacts { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string phone { get; set; }

        /// <summary>
        /// 身份证
        /// </summary>
        public string identity { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public string orderDate { get; set; }

        /// <summary>
        /// 时段
        /// </summary>
        public string orderTime { get; set; }

        /// <summary>
        /// 数据状态 null就是添加 modify 、delete
        /// </summary> 
        public string operateType { get; set; }

        /// <summary>
        /// 是否锁定
        /// </summary>
        public bool locked { get; set; }

        /// <summary>
        /// 是否已排队
        /// </summary>
        public bool isQueue { get; set; }

        /// <summary>
        /// 月台排队状态
        /// </summary>
        public QueueState QueueState { get; set; }
        
        /// <summary>
        /// 锁定
        /// </summary>
        public bool draggable { get; set; }

        /// <summary>
        /// 锁定
        /// </summary>
        public bool resizable { get; set; }
    }
}
