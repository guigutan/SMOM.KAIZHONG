using SIE.EMS.Tpms;
using SIE.MetaModel.View;
using SIE.Web.EMS.Tpms.Commands;

namespace SIE.Web.EMS.Tpms
{
    /// <summary>
    /// TPM检查评分项视图配置
    /// </summary>
    internal class TpmWeekInspectScoreViewConfig : WebViewConfig<TpmWeekInspectScore>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            //View.UseDefaultCommands().ReplaceCommands(WebCommandNames.Save, SaveWeekJobScoreItemCommand.CommandName);
            View.UseDefaultCommands();
            View.Property(p => p.ProjectName);
            View.Property(p => p.ScoreType);
            View.Property(p => p.IsPhoto);
            /* View.Property(p => p.CheckStandard).UseTextEditor(p => p.MaxLength = 1000).ShowInList(width: 350);
            View.Property(p => p.ScoreRate).UseSpinEditor(p =>
            {
                p.AllowNegative = false;
                p.AllowDecimals = false;
                p.AllowBlank = false;
                p.MinValue = 1;
                p.MaxValue = 100;
            });*/
            View.Property(p => p.UpdateDate).Show(ShowInWhere.All);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.All);
            View.Property(p => p.CreateDate).Show(ShowInWhere.All);
            View.Property(p => p.CreateByName).Show(ShowInWhere.All);
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.ProjectName);
            View.Property(p => p.ScoreType);
        }
    }
}
