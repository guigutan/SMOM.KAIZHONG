using SIE.Fixtures.Fixtures.Accounts;
using SIE.Fixtures.MaintainTasks;
using SIE.Web.Fixtures.MaintainTasks.Commands;

namespace SIE.Web.Fixtures.MaintainTasks
{
    /// <summary>
    /// 保养任务-界面
    /// </summary>
    public class MaintainTaskViewConfig : WebViewConfig<MaintainTask>
    {
        /// <summary>
        /// 自定义编码类工装台帐的保养履历视图
        /// </summary>
        public const string MaintainResumeView = "MaintainResumeView";

        /// <summary>
        /// 自定义ID类工装台帐的保养履历视图
        /// </summary>
        public const string TaskResumeView = "TaskResumeView";

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(MaintainResumeView, TaskResumeView);
            if (ViewGroup == MaintainResumeView || ViewGroup == TaskResumeView)
            {
                ConfigMaintainResumeView();
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommand("SIE.Web.Fixtures.MaintainTasks.Commands.MaintainCommand");
            View.FormEdit();
            View.Property(p => p.No).Readonly();
            View.Property(p => p.RelatedNo).Readonly();
            View.Property(p => p.MaintainType).Readonly();
            View.Property(p => p.AccountCode).Readonly();
            View.Property(p => p.EncodeCode).Readonly();
            View.Property(p => p.ModelCode).Readonly();
            View.Property(p => p.ModelName).Readonly();
            View.Property(p => p.FixtureType).Readonly();
            View.Property(p => p.Qty).Readonly();
            View.Property(p => p.PassQty).Readonly();
            View.Property(p => p.NgQty).Readonly();
            View.Property(p => p.State).Readonly();
            View.Property(p => p.ApplyDate).Readonly();
            View.Property(p => p.FinishDate).Readonly();
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.ChildrenProperty(p => p.Details);
            View.ChildrenProperty(p => p.Records);
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.UseDetail(4);
            View.AddBehavior("SIE.Web.Fixtures.MaintainTasks.MaintainTaskBehavior");
            View.UseCommand(typeof(MaintainSaveCommand).FullName);
            View.Property(p => p.No).Readonly();
            View.Property(p => p.RelatedNo).HasLabel("关联单据号").Readonly();
            View.Property(p => p.MaintainType).Readonly();
            View.Property(p => p.FixtureType).Readonly();
            View.Property(p => p.AccountCode).Readonly();
            View.Property(p => p.EncodeCode).Readonly();
            View.Property(p => p.ModelCode).Readonly();
            View.Property(p => p.ModelName).Readonly();
            View.Property(p => p.State).Readonly();
            View.Property(p => p.Qty).Readonly();
            View.Property(p => p.PassQty).Readonly(p => !p.HasPass).UseSpinEditor(p => { p.AllowNegative = false; p.AllowDecimals = false; });
            View.Property(p => p.NgQty).Readonly(p => !p.HasNg).UseSpinEditor(p => { p.AllowNegative = false; p.AllowDecimals = false; });
            View.Property(p => p.ApplyDate).Readonly();
            View.Property(p => p.FinishDate).Readonly();
            View.ChildrenProperty(p => p.Details).UseViewGroup(MaintainTaskDetailViewConfig.EditMaintainDetail);
            View.ChildrenProperty(p => p.Records).UseViewGroup(MaintainChangeRecordViewConfig.EditChangeRecord);
        }

        /// <summary>
        /// 配置工装台帐的保养履历视图
        /// </summary>
        void ConfigMaintainResumeView()
        {
            View.DomainName("保养履历");
            View.ClearCommands();
            View.AssignAuthorize(typeof(FixtureAccountModel));

            if (ViewGroup == MaintainResumeView)
            {
                View.UseCommands("SIE.Web.Fixtures.MaintainTasks.Commands.ShowMaintainTaskDetail");
            }
            else if (ViewGroup == TaskResumeView)
            {
                View.UseCommands("SIE.Web.Fixtures.MaintainTasks.Commands.ShowTaskDetail");
            }

            using (View.OrderProperties())
            {
                View.Property(p => p.No).Readonly().HasLabel("保养任务单号").ShowInList(150);
                View.Property(p => p.MaintainType).UseEnumEditor().Readonly().HasLabel("保养触发条件").Show(ShowInWhere.All);
                View.Property(p => p.State).UseEnumEditor().Readonly().Show(ShowInWhere.All);
                View.Property(p => p.FinishDate).HasLabel("保养完成时间").Readonly().ShowInList(150);
                View.ChildrenProperty(p => p.Details).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.Records).Show(ChildShowInWhere.Hide);
                View.Property(p => p.CreateByName).Readonly().Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Readonly().Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Readonly().Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Readonly().Show(ShowInWhere.Hide);
            }
        }

        protected override void ConfigSelectionView()
        {
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.No).Readonly();
                View.Property(p => p.RelatedNo).HasLabel("关联单据号").Readonly();
                View.Property(p => p.MaintainType).Readonly();
                View.Property(p => p.FixtureType).Readonly();
                View.Property(p => p.AccountCode).Readonly();
                View.Property(p => p.EncodeCode).Readonly();
                View.Property(p => p.ModelCode).Readonly();
                View.Property(p => p.ModelName).Readonly();
                View.Property(p => p.State).Readonly();
                View.Property(p => p.Qty).Readonly();
                View.Property(p => p.PassQty).UseSpinEditor(p => { p.AllowNegative = false; p.AllowDecimals = false; });
                View.Property(p => p.NgQty).UseSpinEditor(p => { p.AllowNegative = false; p.AllowDecimals = false; });
                View.Property(p => p.ApplyDate).Readonly();
                View.Property(p => p.FinishDate).Readonly();
            }
        }
    }
}
