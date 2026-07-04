using SIE.Kit.APS.EngineerPlan.Settings;
using SIE.MetaModel.View;

namespace SIE.Web.Kit.APS.EngineerPlans.Configs
{
    /// <summary>
    /// 客户等级 视图配置
    /// </summary>
    internal class CustLevelViewConfig : WebViewConfig<CustLevel>
    {
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(WebCommandNames.Edit);
            View.UseCommands(WebCommandNames.Save);
            View.UseCommands(WebCommandNames.ExportXls);
            //View.RemoveCommands(WebCommandNames.Add);
            //View.RemoveCommands(WebCommandNames.Delete);

            using (View.OrderProperties())
            {
                View.Property(p => p.LevelName).Show(ShowInWhere.All).Readonly(true);
                View.Property(p => p.Hour).Show(ShowInWhere.All).Readonly(true);
                View.Property(p => p.DayWorkCapacity).Show(ShowInWhere.All);
            }
        }

        protected override void ConfigSelectionView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.LevelName).Show(ShowInWhere.All);
                View.Property(p => p.Hour).Show(ShowInWhere.All);
                View.Property(p => p.DayWorkCapacity).Show(ShowInWhere.All);
            }
        }

        protected override void ConfigQueryView()
        {
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.LevelName).Show(ShowInWhere.All);
                View.Property(p => p.Hour).Show(ShowInWhere.All);
                View.Property(p => p.DayWorkCapacity).Show(ShowInWhere.All);
            }
        }

    }
}


