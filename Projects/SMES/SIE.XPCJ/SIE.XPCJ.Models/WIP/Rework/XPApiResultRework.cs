using SIE.XPCJ.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.XPCJ.Models.WIP
{
    /// <summary>
    /// XP端返工采集API方法返回值
    /// </summary>
    [Serializable]
    public class XPApiResultRework
    {
        /// <summary>
        /// 是否切换了工单
        /// </summary>
        public bool IsChangeOrder { get; set; }

        /// <summary>
        /// 工单信息
        /// </summary>
        public WorkOrder WorkOrder { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public ReworkOperate ReworkOperate { get; set; }
        /// <summary>
        /// 采集的条码
        /// </summary>
        public CollectBarcode CollectBarcode { get; set; }

        /// <summary>
        /// 结果类型
        /// </summary>
        public ResultType ResultType { get; set; }

        /// <summary>
        /// 采集步骤
        /// </summary>
        public XPReworkStep Step { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Tips { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public List<XPWipProductProcessKeyItem> WipKeyItems { get; set; }
    }
}
