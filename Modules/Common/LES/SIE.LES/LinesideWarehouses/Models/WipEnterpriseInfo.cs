using SIE.Resources.Enterprises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.LES.LinesideWarehouses.Models
{
    /// <summary>
    /// 产线线边仓信息
    /// </summary>
    [Serializable]
    public class WipEnterpriseInfo
    {
        /// <summary>
        /// 父节点
        /// </summary>
        public double TreePId { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public EnterpriseType Type { get; set; }

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double FactoryId { get; set; }

        /// <summary>
        /// 工厂名称
        /// </summary>
        public string FactoryName { get; set; }

        /// <summary>
        /// 车间Id
        /// </summary>
        public double WorkShopId { get; set; }

        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkShopName { get; set; }
    }
}
