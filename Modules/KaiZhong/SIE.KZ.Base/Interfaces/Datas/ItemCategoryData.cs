using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Base.Interfaces.Datas
{
    [Serializable]
    public class ItemCategoryData
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string MATKL { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string WGBEZ { get; set; }
    }
}
