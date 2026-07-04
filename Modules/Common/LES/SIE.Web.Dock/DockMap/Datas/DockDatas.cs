using SIE.Dock.Datas;
using SIE.Dock.DockAppoints;
using SIE.Dock.YardZones;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Dock.DockMap.Datas
{
    /// <summary>
    /// 查询月台数据参数
    /// </summary>
    public class DockSearchParams
    {
        /// <summary>
        /// 园区Id
        /// </summary>
        public double? YardZoneId { get; set; }

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double? WarehouseId { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public State? State { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndDate { get; set; }
    }

    /// <summary>
    /// 片区数据
    /// </summary>
    [Serializable]
    public class ZoneData
    {
        /// <summary>
        /// 园区编码
        /// </summary>
        public string YardZoneCode { get; set; }

        /// <summary>
        /// 园区名称
        /// </summary>
        public string YardZoneName { get; set; }

        /// <summary>
        /// 月台数据
        /// </summary>
        public List<DockData> DockDatas { get; set; }

        /// <summary>
        /// 装卸能力
        /// </summary>
        public List<DockHandling> DockHandlings { get; set; } = new List<DockHandling>();

    }


    /// <summary>
    /// 月台预约数据
    /// </summary>
    [Serializable]
    public class DockData
    {
        /// <summary>
        /// 月台Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 月台编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public State State { get; set; }

        /// <summary>
        /// 是否收货
        /// </summary>
        public bool IsReceive { get; set; }

        /// <summary>
        /// 是否发货
        /// </summary>
        public bool IsShip { get; set; }  
       
        /// <summary>
        /// 预约数据
        /// </summary>
        public List<DockAppoint> DockAppoints { get; set; } = new List<DockAppoint>();       
    }
}
