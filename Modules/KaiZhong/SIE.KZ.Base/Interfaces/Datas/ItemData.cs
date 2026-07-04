using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Base.Interfaces.Datas
{
    /// <summary>
    /// 物料
    /// </summary>
    [Serializable]
    public class ItemData
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        public string MATNR { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MAKTX { get; set; }

        /// <summary>
        /// 型号
        /// </summary>
        public string ZMODEL { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        public string ZGG { get; set; }

        /// <summary>
        /// 基本计量单位
        /// </summary>
        public string MEINS { get; set; }

        /// <summary>
        /// 物料组
        /// </summary>
        public string MATKL { get; set; }

        /// <summary>
        /// 基本类型
        /*
         MES成品对应SAP成品(KZ01)、准成品(KZ02)
         MES半成品对应SAP半成品(KZ03)
         MES原材料对应SAP原材料(KZ04)、客供料(KZ05)，生产辅料(KZ06)，副产品(KZ08)
         */
        /// </summary>
        public string MTART { get; set; }

        /// <summary>
        /// 来源类型
        /// </summary>
        public string SOBSL { get; set; }

        /// <summary>
        /// 集团状态
        /*
         MES可用对应SAP样件(Z1)、量产(Z2)
         MES禁用对应SAP冻结(Z3)、财务冻结(Z4)
         */
        /// </summary>
        public string MSTAE { get; set; }

        /// <summary>
        /// 工厂状态
         /*
         MES可用对应SAP样件(Z1)、量产(Z2)
         MES禁用对应SAP冻结(Z3)、财务冻结(Z4)
         */
        /// </summary>
        public string MMSTA { get; set; }

        /// <summary>
        /// 旧料号（凯中旧料号）
        /// </summary>
        public string BISMT { get; set; }

        /// <summary>
        /// MRP控制者
        /// </summary>
        public string DISPO { get; set; }

        /// <summary>
        /// 物料消耗类型（311:拉式物料;261:推式物料）
        /// </summary>
        //public int WERKS { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        public string WERKS { get; set;}

        /// <summary>
        /// 后继物料
        /// </summary>
        public string NFMAT { get; set; }

        /// <summary>
        /// 后继生效时间
        /// </summary>
        public string AUSDT { get; set; }

        /// <summary>
        /// 净重
        /// </summary>
        public decimal? NTGEW { get; set; }

        /// <summary>
        /// 净重单位
        /// </summary>
        public string GEWEI { get;set;}

        /// <summary>
        /// 最小包装量
        /// </summary>
        public decimal BSTRF { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string ZMC { get; set; }

        /// <summary>
        /// 行业标准描述
        /// </summary>
        public string NORMT { get; set; }

        /// <summary>
        /// 可焊性
        /// </summary>
        public string ZKHXHGY { get; set; }

        /// <summary>
        /// 胶带类型
        /// </summary>
        public string ZJDLX { get; set; }

        /// <summary>
        /// 客户编号
        /// </summary>
        public string ZKUNAG { get; set; }

        /// <summary>
        /// 其他特性
        /// </summary>
        public string ZQTTX { get; set; }
    }
}
