using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Base.Interfaces.Datas
{
    [Serializable]
    public class ProcessData
    {
        /// <summary>
        /// 工序编码
        /// </summary>
        public string VLSCH { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string TXT { get; set; }
    }
}
