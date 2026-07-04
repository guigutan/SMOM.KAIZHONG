using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.DashBoard.KzBoard.RegionBoards
{
    public enum RegionBoardType
    {
        /// <summary>
        /// 设备状态
        /// </summary>
        [Label("设备状态")]
        DeviceStatus=1,
        /// <summary>
        /// 热处理
        /// </summary>
        [Label("热处理")]
        HeatTreatment = 2,
        /// <summary>
        /// 生产产出
        /// </summary>
        [Label("生产产出")]
        ProductionOutput = 3,
    }
}
