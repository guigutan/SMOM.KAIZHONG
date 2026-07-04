using System;
using System.Collections.Generic;

namespace SIE.MES.WIP
{
    /// <summary>
    /// 返工采集数据
    /// </summary>
    [Serializable]
    public class ReworkData
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ReworkData()
        {
            KeyItems = new List<double>();
            Context = new CollectionContext();
        }

        /// <summary>
        /// 待解绑关键件集合
        /// 存储产品关键件ID
        /// </summary>
        public List<double> KeyItems { get; set; }

        /// <summary>
        /// 返工工单条码
        /// </summary>
        public string ReworkBarcode { get; set; }

        /// <summary>
        /// 原工单条码
        /// </summary>
        public string OriginalBarcode { get; set; }

        /// <summary>
        /// 上下文
        /// </summary>
        public CollectionContext Context { get; }
    }
}