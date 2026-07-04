using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Base.Interfaces.Datas
{
    /// <summary>
    /// 蓝标数据
    /// </summary>
    [Serializable]
    public class BlueLabelData
    {
        /// <summary>
        /// 蓝标HU号
        /// </summary>
        public string EXIDV { get; set; }

        /// <summary>
        /// 物料编号
        /// </summary>
        public string MATNR { get; set; }

        /// <summary>
        /// 批号
        /// </summary>l
        public string CHARG { get; set; }

        /// <summary>
        /// 包装数量
        /// </summary>
        public int VEMNG { get; set; }

        /// <summary>
        /// 库存地点
        /// </summary>
        public string LGORT { get; set; }

        /// <summary>
        /// 箱号
        /// </summary>
        public string EXIDV2 { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        public string WERKS { get; set; }

        /// <summary>
        /// 生产订单号
        /// </summary>
        public string AUFNR { get; set; }

        /// <summary>
        /// 创建删除标识
        /// </summary>
        public string BIAOS { get; set; }
    }
}
