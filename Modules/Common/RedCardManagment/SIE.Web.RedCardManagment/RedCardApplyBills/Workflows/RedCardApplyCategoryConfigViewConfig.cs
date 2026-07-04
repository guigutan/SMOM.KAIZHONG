using SIE.Items;
using SIE.RedCardManagment.WorkFlow.CategoryConfig;
using System.Collections.Generic;

namespace SIE.Web.RedCardManagment.RedCardApplyBills.WorkFlows
{
    /// <summary>
    /// 红牌申请-工作流分类视图配置
    /// </summary>
    public class RedCardApplyCategoryConfigViewConfig : WebViewConfig<RedCardApplyCategoryConfig>
    {
        /// <summary>
        /// 表单视图配置
        /// </summary>
        protected override void ConfigDetailsView()
        {
            //不提供分类配置，申请单启动流程时匹配默认流程
            //View.Property(p => p.ApplySource);
        }
    }
}
