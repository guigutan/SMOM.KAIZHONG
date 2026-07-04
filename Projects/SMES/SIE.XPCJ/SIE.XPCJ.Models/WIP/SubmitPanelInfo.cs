using SIE.XPCJ.Models.Enums;
using System;
using System.Collections.Generic;

namespace SIE.XPCJ.Models.WIP
{
    /// <summary>
    /// 采集过站拼板码信息
    /// </summary>
    [Serializable]
    public class SubmitPanelInfo
    {

        /// <summary>
        /// 条码类型
        /// </summary>
        public BarcodeType? BarcodeType { get; set; }

        /// <summary>
        /// 拼板码
        /// </summary>
        public string PanelCode { get; set; }

        /// <summary>
        /// 拼板数
        /// </summary>
        public int PanelQty { get; set; }

        /// <summary>
        /// 叉板数
        /// </summary>
        public int ForkPlateQty { get; set; }

        /// <summary>
        /// SN绑定方式
        /// 可空标识不需要绑定
        /// </summary>
        public BindingMode? BindingMode { get; set; }

        /// <summary>
        /// 剩余待绑定数量
        /// </summary>
        public int ToBindingQty
        {
            get
            {
                if (BindingMode == WIP.BindingMode.Manual)
                {
                    return NeetToBindingQty - SnList.Count;
                }
                return 0;
            }
        }

        /// <summary>
        /// 是否需要绑定SN
        /// </summary>
        public bool NeetToBindingSn
        {
            get
            {
                if (BindingMode == WIP.BindingMode.Manual)
                {
                    return ToBindingQty > 0;
                }
                return false;
            }
        }

        /// <summary>
        /// 总共需要绑定的SN数量
        /// </summary>
        public int NeetToBindingQty
        {
            get
            {
                if (BindingMode == WIP.BindingMode.Manual)
                {
                    return PanelQty - ForkPlateQty;
                }
                return 0;
            }
        }

        /// <summary>
        /// 拼板码绑定条码集合
        /// </summary>
        public List<SnData> SnList { get; } = new List<SnData>();

        /// <summary>
        /// 清楚数据
        /// </summary>
        public virtual void Clear()
        {
            SnList.Clear();
            PanelCode = "";
            PanelQty = ForkPlateQty = 0;
            BindingMode = null;
        }
    }
    
}
