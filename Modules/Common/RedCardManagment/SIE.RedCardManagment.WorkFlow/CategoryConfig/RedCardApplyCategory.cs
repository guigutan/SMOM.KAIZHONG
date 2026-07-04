using SIE.RedCardManagment.WorkFlow.Common;
using SIE.WorkFlow.Base.FlowDefinitions.Categorys;
using System;

namespace SIE.RedCardManagment.WorkFlow.CategoryConfig
{
    /// <summary>
    /// 工作流分类-红牌申请
    /// </summary>
    [Serializable]
    public class RedCardApplyCategory : WorkFlowCategoryBase
    {
        /// <summary>
        /// 名称
        /// </summary>
        public static readonly string CategoryName = RedCardWorkFlowText.RedCardWorkFlowName;

        /// <summary>
        /// 构造函数
        /// </summary>
        public RedCardApplyCategory()
        {
            Name = CategoryName;
            Config = new RedCardApplyCategoryConfig();
        }
    }
}
