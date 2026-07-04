using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Base.Interfaces.Datas
{
    public class EquipmentData
    {
        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipName { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipCode { get; set; }

        /// <summary>
        /// 功能位置
        /// </summary>
        public string FunctionalLocation { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        public string FactoryName { get; set; }

        /// <summary>
        /// 生产厂家
        /// </summary>
        public string Manufacturer { get; set; }
    }
}
