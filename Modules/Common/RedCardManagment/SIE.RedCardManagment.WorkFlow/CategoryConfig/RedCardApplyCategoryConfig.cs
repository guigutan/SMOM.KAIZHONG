using Newtonsoft.Json;
using SIE.Core.RedCardManagments;
using SIE.Domain;
using SIE.ObjectModel;
using SIE.WorkFlow.Base.FlowDefinitions.Categorys;
using System;
using System.Text;

namespace SIE.RedCardManagment.WorkFlow.CategoryConfig
{
    /// <summary>
    /// 红牌申请工作流分类配置(标品不提供详细的分类配置，可自行扩展)
    /// </summary>
    [RootEntity, Serializable]
    [Label("红牌申请工作流分类配置")]
    public class RedCardApplyCategoryConfig : WorkFlowCategoryConfigBase
    {

        /// <summary>
        /// 转换成Json配置字符串
        /// </summary>
        /// <returns></returns>
        public override string ToConfigJson()
        {
            return "{}";
        }

        /// <summary>
        /// 获取展示内容
        /// </summary>
        /// <returns></returns>
        public override string GetDisplayConfigStr()
        {
            return string.Empty;
        }
    }
}
