using SIE.Resources;
using System;

namespace SIE.ERPInterface.Common.Datas.EbsData
{
    /// <summary>
    /// 账户别名
    /// </summary>
    [Serializable]
    public class ErpAccountData : EbsDataBase
    {
        /// <summary>
        /// 账户别名ID
        /// </summary>
        public int DISPOSITION_ID { get; set; }

        /// <summary>
        /// 账户别名代码
        /// </summary>
        public string DISPOSITION_CODE { get; set; }

        /// <summary>
        /// 账户别名描述
        /// </summary>
        public string DESCRIPTION { get; set; }

        /// <summary>
        /// 库存组织ID
        /// </summary>
        public int ORGANIZATION_ID { get; set; }

        /// <summary>
        /// 库存组织编码
        /// </summary>
        public string ORGANIZATION_CODE { get; set; }

        /// <summary>
        /// 库存组织名称
        /// </summary>
        public string ORGANIZATION_NAME { get; set; }

        /// <summary>
        /// 生效日期
        /// </summary>
        public string EFFECTIVE_DATE { get; set; }

        /// <summary>
        /// 失效日期
        /// </summary>
        public string DISABLE_DATE { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CREATION_DATE { get; set; }

        /// <summary>
        /// ERP账户别名
        /// </summary>
        public ErpAccount? ErpAccount { get; set; }
    }

}
