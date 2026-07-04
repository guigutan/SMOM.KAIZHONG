using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Base.Interfaces.Datas
{
    public class EquipmentQuery
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? beginTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? endTime { get; set; }
    }
}
