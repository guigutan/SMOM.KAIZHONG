using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Tech.Processs.Models
{
    /// <summary>
    /// 工序名称集合
    /// </summary>
    [Serializable]
    public class ProcessIdName
    {
        /// <summary>
        /// 工序id
        /// </summary>
        public double ProcessId { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName { get; set; }
    }
}
