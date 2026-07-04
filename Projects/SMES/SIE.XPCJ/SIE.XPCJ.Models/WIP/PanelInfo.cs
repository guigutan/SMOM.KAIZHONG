using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.XPCJ.Models.WIP
{
    /// <summary>
    /// 拼板码信息
    /// </summary>
    [Serializable]
    public class PanelInfo
    {
        /// <summary>
        /// 工序是否绑定
        /// </summary> 
        public bool IsBinding { get; set; }

        /// <summary>
        /// 拼板码可绑定产品数量
        /// </summary>
        public int CanBindQty { get; set; }

        /// <summary>
        /// 拼板号
        /// </summary>
        public string PanelCode { get; set; }

        /// <summary>
        /// 叉板数
        /// </summary>
        public int ForkPlateQty { get; set; }

        /// <summary>
        /// 拼板码绑定条码集合
        /// </summary>
        public List<SnData> SnList { get; } = new List<SnData>();

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            IsBinding = false;
            CanBindQty = ForkPlateQty = 0;
            SnList.Clear();
        }
    }
}
