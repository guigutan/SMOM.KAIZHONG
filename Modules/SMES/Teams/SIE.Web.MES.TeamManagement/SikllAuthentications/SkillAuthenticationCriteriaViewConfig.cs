using SIE.MES.TeamManagement.SikllAuthentications;

namespace SIE.Web.MES.TeamManagement.SikllAuthentications
{
    /// <summary>
    /// 认证技能查询视图配置类
    /// </summary>
    internal class SkillAuthenticationCriteriaViewConfig : WebViewConfig<SkillAuthenticationCriteria>
    {
        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Skill).ShowInList();
            View.Property(p => p.SkillCategory).ShowInList();
        }
    }
}