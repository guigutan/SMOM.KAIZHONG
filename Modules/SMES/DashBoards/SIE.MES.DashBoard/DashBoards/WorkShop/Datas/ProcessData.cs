using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.DashBoard.DashBoards.WorkShop.Datas
{
    /// <summary>
    /// 看板实体工序
    /// </summary>
    public class ProcessData
    {
        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName { get; set; }


        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcessCode { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public double ProcessId { get; set; }
    }
}
