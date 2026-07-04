using SIE.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.WorkOrders.Models
{
    /// <summary>
    /// 一键备料数据
    /// </summary>
    [Serializable]
    public class OneKeyInfo
    {
        /// <summary>
        /// 工单信息
        /// </summary>
        public List<BaseDataInfo> WoList { get; set; } = new List<BaseDataInfo>();

        /// <summary>
        /// 类型 10-创建发运单 20-创建备料单
        /// </summary>
        public int Type { get; set; }
    }

}
