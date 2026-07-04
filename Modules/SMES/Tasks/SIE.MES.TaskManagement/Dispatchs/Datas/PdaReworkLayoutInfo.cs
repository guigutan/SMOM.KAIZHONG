using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.Dispatchs.Datas
{
    /// <summary>
    /// 返工工艺路线工序信息
    /// </summary>
    [Serializable]
    public class PdaReworkLayoutInfo
    {
        /// <summary>
        /// 操作活动编号
        /// </summary>
        public string Vornr { get; set; }

        /// <summary>
        /// 标准文本码
        /// </summary>
        public string ProcessCode { get; set; }

        /// <summary>
        /// 工作中心编码
        /// </summary>
        public string WorkCenterCode { get; set; }

        /// <summary>
        /// 工序控制码
        /// </summary>
        public string Steus { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        public string Factory { get; set; }

        /// <summary>
        /// 分单数量
        /// </summary>
        public decimal Zcode { get; set; }
    }
}
