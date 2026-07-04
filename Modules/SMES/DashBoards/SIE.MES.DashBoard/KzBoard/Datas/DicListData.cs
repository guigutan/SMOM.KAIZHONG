using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.DashBoard.KzBoard.Datas
{

    [Serializable]
    public class DicListData
    {
        /// <summary>
        /// 产品编码
        /// </summary>
        public double? ProductId { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }
    }
}
