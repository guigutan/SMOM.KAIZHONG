using SIE.Resources.Enterprises;
using System;

namespace SIE.ERPInterface.Common.Datas
{
    /// <summary>
    /// ERP通信接口
    /// </summary>
    [Serializable]
    public class EnterpriseData : ErpInfoData
    {
        /// <summary>
        /// ERP存储的组织Id
        /// </summary>
        public int ErpOrgId { get; set; }

        /// <summary>
        /// 是否资源
        /// </summary>
        public bool IsResource { get; set; } = false;

        /// <summary>
        /// 企业层级
        /// </summary>
        public string LevelCode { get; set; }

        /// <summary>
        /// 父编码
        /// </summary>
        public string ParentCode { get; set; }
    }


    /// <summary>
    /// 更新企业模型父Id
    /// </summary>
    public class UpdateEnterpriseTreeData
    {
        /// <summary>
        /// 企业模型
        /// </summary>
        public Enterprise Enterprise { get; set; }

        /// <summary>
        /// 父编码
        /// </summary>
        public string ParentCode { get; set; }

        /// <summary>
        /// key
        /// </summary>
        public string Infkey { get; set; }
    }
}
