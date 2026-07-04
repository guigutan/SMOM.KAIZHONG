using System;

namespace SIE.ERPInterface.Common.Datas
{
    /// <summary>
    /// 库存组织数据
    /// </summary>
    [Serializable]
    public class InvOrgData: EbsDataBase
    {
        /// <summary>
        /// 库存组织Id
        /// </summary>
        public int ORGANIZATION_ID { get; set; }

        /// <summary>
        /// 库存组织编码
        /// </summary>
        public string ORGANIZATION_CODE { get; set; }

        /// <summary>
        /// 库存组织名称
        /// </summary>
        public string ORGANIZATION_NAME { get; set;}
    }
}
