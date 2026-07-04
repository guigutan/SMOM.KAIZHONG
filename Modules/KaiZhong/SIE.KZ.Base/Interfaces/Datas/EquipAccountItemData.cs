using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Base.Interfaces.Datas
{
    /// <summary>
    /// 模具与产品的关系
    /// </summary>
    [Serializable]
    public class EquipAccountItemData
    {
        /// <summary>
        /// 工厂
        /// </summary>
        public string WERKS { get; set; }

        /// <summary>
        /// 模具编码
        /// </summary>
        public string EquipAccountCode { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 工序编码(可空)
        /// </summary>
        public string ProcessCode { get; set; }

        /// <summary>
        /// 模具组(可空)
        /// </summary>
        public string UniqueCode { get; set; }
    }
}
