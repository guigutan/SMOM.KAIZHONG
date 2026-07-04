using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Base.Interfaces.Datas
{
    /// <summary>
    /// 物料标签数据
    /// </summary>
    [Serializable]
    public class ItemLabelData
    {
        /// <summary>
        /// 标签号
        /// </summary>
        public string EXIDV { get; set; }

        /// <summary>
        /// 绿标标签
        /// </summary>
        public string EXIDV2 { get; set; }

        /// <summary>
        /// 物料编号
        /// </summary>
        public string MATNR { get; set; }

        /// <summary>
        /// 批次
        /// </summary>
        public string CHARG { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal VEMNG { get; set; }
        
        /// <summary>
        /// 库位(库存地点)
        /// </summary>
        public string LGORT { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        public string WERKS { get; set; }

        /// <summary>
        /// 供应商批次
        /// </summary>
        public string LICHA { get; set; }

    }
}
