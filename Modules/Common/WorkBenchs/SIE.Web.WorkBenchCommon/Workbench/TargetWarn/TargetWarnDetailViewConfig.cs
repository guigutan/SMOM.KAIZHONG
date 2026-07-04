using SIE.Domain;
using SIE.ManagedProperty;
using SIE.WorkBenchCommon.Workbench.TargetWarn;
using SIE.Web.Command;
using SIE.MetaModel.View;

namespace SIE.Web.WorkBenchCommon.Workbench.TargetWarn
{
    /// <summary>
    /// 达成率区间视图配置
    /// </summary>
    [CompiledPropertyDeclarer]
    public class TargetWarnDetailViewConfig : WebViewConfig<TargetWarnDetail>
    {
        

        /// <summary>
        /// 只读视图
        /// </summary>
        public const string ReadonlyView = "ReadonlyView";

        /// <summary>
        /// 无按钮的达成率目标设定视图名字
        /// </summary>
        protected override void ConfigView()
        {
            View.InlineEdit();
            View.DeclareExtendViewGroup(ReadonlyView);
            if (ViewGroup == ReadonlyView)
            {
                ConfigReadonlyView();
            }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.InlineEdit();
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Delete);
            using (View.OrderProperties())
            {
                View.Property(p => p.TargetOpetators).HasLabel("条件");
                View.Property(p => p.MinValue).ShowInList().HasLabel("值1(%)").Readonly(p => p.TargetOpetators == TargetOpetators.GreaterOrEqual)
                .UseListSetting(e => { e.HelpInfo = "目标条件[大于或等于]不可编辑"; });
                View.Property(p => p.MaxValue).ShowInList().HasLabel("值2(%)").Readonly(p => p.TargetOpetators == TargetOpetators.LessOrEqual)
                .UseListSetting(e => { e.HelpInfo = "目标条件[小于或等于]不可编辑"; });
                View.Property(p => p.TargetColor).HasLabel("颜色");
                //View.Property(DataEntity.CreateByNameProperty).Show(ShowInWhere.Hide);
                //View.Property(DataEntity.CreateDateProperty).Show(ShowInWhere.Hide);
                //View.Property(DataEntity.UpdateByNameProperty).Show(ShowInWhere.Hide);
                //View.Property(DataEntity.UpdateDateProperty).Show(ShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 配置有按钮的列表视图
        /// </summary>
        private void ConfigReadonlyView()
        {
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.TargetOpetators).ShowInList().HasLabel("条件");
                View.Property(p => p.MinValue).ShowInList().HasLabel("值1(%)").Readonly(p=>p.TargetOpetators == TargetOpetators.GreaterOrEqual)
                .UseListSetting(e => { e.HelpInfo = "目标条件[大于或等于]不可编辑"; });
                View.Property(p => p.MaxValue).ShowInList().HasLabel("值2(%)").Readonly(p => p.TargetOpetators == TargetOpetators.LessOrEqual)
                .UseListSetting(e => { e.HelpInfo = "目标条件[小于或等于]不可编辑"; });
                View.Property(p => p.TargetColor).ShowInList().HasLabel("颜色").Readonly();
                //View.Property(DataEntity.UpdateDateProperty).Show(ShowInWhere.Hide);
                //View.Property(DataEntity.CreateByNameProperty).Show(ShowInWhere.Hide);
                //View.Property(DataEntity.CreateDateProperty).Show(ShowInWhere.Hide);
                //View.Property(DataEntity.UpdateByNameProperty).Show(ShowInWhere.Hide);
            }
        }
    }
}