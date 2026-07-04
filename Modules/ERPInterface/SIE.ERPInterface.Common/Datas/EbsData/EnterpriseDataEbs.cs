using System;

namespace SIE.ERPInterface.Common.Datas
{
    /// <summary>
    /// 企业模型
    /// </summary>
    [Serializable]
    public class EnterpriseDataEbs : EbsDataBase
    {        
        /// <summary>
        /// 企业编码
        /// </summary>
        public string Enterprise_Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 组织ID
        /// </summary>
        public int Organization_Id { get; set; }

        /// <summary>
        /// 是否资源
        /// </summary>
        public string Is_Resource { get; set; }

        /// <summary>
        /// 父级代码
        /// </summary>
        public string Parent_Code { get; set; }

        /// <summary>
        /// 级别层级（0：集团；1：公司;2:工厂；3：部门；4：车间）
        /// </summary>
        public int? Level_Code { get; set; }
         
        /// <summary>
        /// 启用标志
        /// </summary>
        public string Enable_Flag { get; set; }

        /// <summary>
        /// 层级Id
        /// </summary>
        public double? LevelId { get; set; }       
    }

}
