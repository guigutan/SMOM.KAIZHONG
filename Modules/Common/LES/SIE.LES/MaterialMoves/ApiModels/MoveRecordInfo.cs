using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.LES.MaterialMoves.ApiModels
{
    /// <summary>
    /// 挪料信息
    /// </summary>
    [Serializable]
    public class MoveRecordInfo
    {
        /// <summary>
        /// 工单id
        /// </summary>
        public double WoId { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal MoveQty { get; set; }
    }
}
