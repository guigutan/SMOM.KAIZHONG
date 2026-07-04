using SIE.MES.TeamManagement.ScoreRecords;
using SIE.MetaModel.View;

namespace SIE.Web.MES.TeamManagement.ScoreRecords
{
    /// <summary>
    /// 绩效等级配置视图配置类
    /// </summary>
    internal class AchieveLevelSettingViewConfig : WebViewConfig<AchieveLevelSetting>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.InlineEdit();
            View.UseDefaultCommands();
            View.AssignAuthorize(typeof(ScoreRecord));
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.RemoveCommands(WebCommandNames.Copy, WebCommandNames.ExportXls);
            View.RemoveCommands(WebCommandNames.Add, WebCommandNames.Delete);
            View.RemoveCommands(WebCommandNames.Edit);
            View.UseCommands("SIE.Web.MES.TeamManagement.ScoreRecords.AchieveLevelSetEditCommand");
            View.UseCommands(typeof(AchieveLevelSetIniCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(x => x.AchiLevel).Readonly(true);
                View.Property(x => x.Operator).Readonly(true);
                View.Property(x => x.MinValue);
                View.Property(x => x.MaxValue);
            }
        }
    }
}
