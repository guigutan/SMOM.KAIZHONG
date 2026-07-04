using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.DashBoard.KzBoard.Datas
{
    /// <summary>
    /// 产线设备状态看板数据
    /// </summary>
    [Serializable]
    public class LineEquipmentStatusData
    {
        /// <summary>
        /// 排产状态
        /// </summary>
        public StatuType StatuType { get; set; }

        /// <summary>
        /// 产线名称
        /// </summary>
        public string LineName { get; set; }
    }

    /// <summary>
    /// 排产状态
    /// </summary>
    public enum StatuType
    {
        /// <summary>
        /// 未排产
        /// </summary>
        [Label("未排产")]
        Unscheduled = 0,
        /// <summary>
        /// 正常排产
        /// </summary>
        [Label("正常排产")]
        Scheduling = 1,
        /// <summary>
        /// 安灯异常
        /// </summary>
        [Label("安灯异常")]
        Abnormal = 2,
    }
}
