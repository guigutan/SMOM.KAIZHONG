using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Base.Interfaces.Datas
{
    [Serializable]
    public class MdMapingVO
    {
        /// <summary>
        /// 对应下发的主数据的mdm_code
        /// </summary>
        public string mdmCode { get; set; }

        /// <summary>
        /// 业务系统的数据编码
        /// </summary>
        public string entityCode { get; set; }

        /// <summary>
        /// 业务系统的数据id
        /// </summary>
        public string busiDataId { get; set; }
    }
}
