using SIE.Defects.InspectionItems;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Fixtures.MaintainTasks;
using SIE.Fixtures.Projects;
using SIE.MetaModel.View;
using System.Collections.Generic;

namespace SIE.Web.Fixtures.MaintainTasks
{
    /// <summary>
    /// 保养执行详情-界面
    /// </summary>
    public class MaintainTaskDetailViewConfig : WebViewConfig<MaintainTaskDetail>
    {
        /// <summary>
        /// 保养任务维护界面-保养项目
        /// </summary>
        public const string EditMaintainDetail = "EditMaintainDetail";

        /// <summary>
        /// 自定义查看工装台帐保养任务明细视图
        /// </summary>
        public const string ShowMaintainDetailView = "ShowMaintainDetailView";

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(EditMaintainDetail, ShowMaintainDetailView);
            View.AssignAuthorize(typeof(MaintainTask), typeof(FixtureAccountModel));
            if (ViewGroup == EditMaintainDetail)
                EditMaintainDetailView();
            if (ViewGroup == ShowMaintainDetailView)
                ConfigShowMaintainDetailView();
        }

        /// <summary>
        /// 配置列表界面
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.MaintainProject).Readonly();
            View.Property(p => p.Consumable).Show().Readonly();
            View.Property(p => p.ConsumableQty).Show().Readonly();
            View.Property(p => p.Method).Show().Readonly();
            View.Property(p => p.Tool).Show().Readonly();
            View.Property(p => p.MinValue).Show().Readonly().ShowInList(width: 120);
            View.Property(p => p.MaxValue).Show().Readonly().ShowInList(width: 120);
            View.Property(p => p.CheckTag).Show().Readonly();
            View.Property(p => p.CheckValue).UseSpinEditor(p => p.DecimalPrecision = 3).Show().Readonly();
            View.Property(p => p.MaintainResult).Show().HasLabel("项目保养结论").Readonly();
            View.Property(p => p.Remark).Show().Readonly();
            View.Property(p => p.FinishDate).Readonly();
            View.Property(p => p.MaintainBy).Readonly();
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 保养任务维护界面-保养项目
        /// </summary>
        protected void EditMaintainDetailView()
        {
            View.AddBehavior("SIE.Web.Fixtures.MaintainTasks.MaintainTaskDetailBehavior");
            View.UseCommands(WebCommandNames.Add, "SIE.Web.Fixtures.MaintainTasks.Commands.DeleteDetailCommand", "SIE.Web.Fixtures.MaintainTasks.Commands.OnekeyPassCommand");
            using (View.OrderProperties())
            {
                View.Property(p => p.MaintainProjectId).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.Consumable), nameof(e.MaintainProject.Consumable));
                    keyValues.Add(nameof(e.ConsumableQty), nameof(e.MaintainProject.ConsumableQty));
                    keyValues.Add(nameof(e.Method), nameof(e.MaintainProject.Method));
                    keyValues.Add(nameof(e.Tool), nameof(e.MaintainProject.Tool));
                    keyValues.Add(nameof(e.MinValue), nameof(e.MaintainProject.MinValue));
                    keyValues.Add(nameof(e.MaxValue), nameof(e.MaintainProject.MaxValue));
                    keyValues.Add(nameof(e.CheckTag), nameof(e.MaintainProject.CheckTag));
                    m.DicLinkField = keyValues;
                }).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<MaintainProjectController>().GetMaintainProjects(pagingInfo, keyword);
                }).Show();
                View.Property(p => p.Consumable).Show().Readonly();
                View.Property(p => p.ConsumableQty).Show().Readonly();
                View.Property(p => p.Method).Show().Readonly();
                View.Property(p => p.Tool).Show().Readonly();
                View.Property(p => p.MinValue).Show().Readonly().ShowInList(width: 120);
                View.Property(p => p.MaxValue).Show().Readonly().ShowInList(width: 120);
                View.Property(p => p.CheckTag).Show().Readonly();
                View.Property(p => p.CheckValue).UseSpinEditor(p => p.DecimalPrecision = 3).Show();
                View.Property(p => p.MaintainResult).Show().HasLabel("项目保养结论")
                    .Readonly(p => p.CheckTag == CheckTag.Quantitative);
                View.Property(p => p.Remark).Show();
                View.Property(p => p.FinishDate).Show().Readonly();
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 配置查看工装台帐保养任务明细视图
        /// </summary>
        void ConfigShowMaintainDetailView()
        {
            using (View.OrderProperties())
            {
                View.ClearCommands();
                View.Property(p => p.MaintainProjectId).Readonly().HasLabel("保养项目").Show(ShowInWhere.All);
                View.Property(p => p.Consumable).HasLabel("耗材").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.ConsumableQty).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Method).HasLabel("方法").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Tool).HasLabel("工具").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.MinValue).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.MaxValue).Show(ShowInWhere.All).Readonly().ShowInList(width:120);
                View.Property(p => p.CheckValue).Show(ShowInWhere.All).Readonly().ShowInList(width: 120);
                View.Property(p => p.MaintainResult).UseEnumEditor().Show(ShowInWhere.All).HasLabel("保养项目结论").Readonly();
                View.Property(p => p.CreateByName).Readonly().Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Readonly().Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Readonly().Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Readonly().Show(ShowInWhere.Hide);
            }
        }
    }
}
