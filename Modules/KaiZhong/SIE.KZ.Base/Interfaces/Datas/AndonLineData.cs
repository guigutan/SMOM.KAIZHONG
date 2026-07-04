using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Base.Interfaces.Datas
{
    /// <summary>
    /// 产线与安灯区域
    /// </summary>
    [Serializable]
    public class AndonLineData
    {
        /// <summary>
        /// 工厂
        /// </summary>
        public string WERKS { get; set; }

        /// <summary>
        /// 产线/机台编码
        /// </summary>
        public string MachineCode { get; set; }

        /// <summary>
        /// 产线/机台名称
        /// </summary>
        public string MachineName { get; set; }

        /// <summary>
        /// 设备编码(可空)
        /// </summary>
        public string EquipmentNo { get; set; }

        /// <summary>
        /// 工作中心编码
        /// </summary>
        public string WorkCenterCode { get; set; }

        /// <summary>
        /// 工厂编码
        /// </summary>
        public string FactoryCode { get; set; }

        /// <summary>
        /// 车间编码
        /// </summary>
        public string WorkShopCode { get; set; }

        /// <summary>
        /// 安灯编码(可空)
        /// </summary>
        public string AndonCode { get; set; }
    }
}
