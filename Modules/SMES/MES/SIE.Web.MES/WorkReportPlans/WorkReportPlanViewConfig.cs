using SIE.Domain;
using SIE.MES.WorkReportPlans;
using SIE.Web.MES.WorkReportPlans.Commands;

namespace SIE.Web.MES.WorkReportPlans
{
    /// <summary>
    /// 报工方案视图配置
    /// </summary>
    public class WorkReportPlanViewConfig : WebViewConfig<WorkReportPlan>
    {
        /// <summary>
        /// 报工按钮明细
        /// </summary>
        public const string ReportButtonDetailGroup = "ReportButtonDetailGroup";

        /// <summary>
        /// 开工按钮明细
        /// </summary>
        public const string StartWorkButtonDetailGroup = "StartWorkButtonDetailGroup";

        /// <summary>
        /// 按钮区域明细
        /// </summary>
        public const string ButtonAreaDetailGroup = "ButtonAreaDetailGroup";
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(ReportButtonDetailGroup, StartWorkButtonDetailGroup);
            if (ViewGroup == StartWorkButtonDetailGroup)
            {
                StartWorkButtonDetailView();
            }
            if (ViewGroup == ReportButtonDetailGroup)
            {
                ReportButtonDetailView();
            }
            if (ViewGroup == ButtonAreaDetailGroup) {
                ButtonAreaDetailView();
            }
        }

        /// <summary>
        /// 报工按钮逻辑限制
        /// </summary>
        private void ReportButtonDetailView()
        {
            using (View.DeclareGroup("校验方法配置(强制报错)", 4, false, false))
            {
                View.Property(p => p.IsReportCheckWOStatus);
                View.Property(p => p.IsRepCheckEmpSkills);
                //View.Property(p => p.IsReportMaterialKitCompleteness);
                //View.Property(p => p.IsReportEquipmentStatus);
                //View.Property(p => p.IsReportMoldStatus);
                View.Property(p => p.IsReportQuantity);
                View.Property(p => p.IsNeedCheck).HasLabel("启用报工确认");
            }
        }

        /// <summary>
        /// 开始按钮逻辑控制
        /// </summary>
        private void StartWorkButtonDetailView()
        {
            using (View.DeclareGroup("校验方法配置(强制报错)", 4, false,false))
            {
                View.Property(p => p.IsCheckWOStatus);
                View.Property(p => p.IsCheckEmployeeSkills);
                //View.Property(p => p.IsMaterialKitCompleteness);
                //View.Property(p => p.IsEquipmentSpotCheck);
                //View.Property(p => p.IsMoldSpotCheck);
            }
        }

        private void ButtonAreaDetailView()
        {
            using (View.DeclareGroup("根据现场管理需求选配(选配了，报工主界面才会显示)", 4, false, false))
            {
                View.Property(p => p.IsDispatchTask);
                View.Property(p => p.IsProductionReport);
                View.Property(p => p.IsShowFirstInsp);
                //View.Property(p => p.IsDeviceInspection);
                //View.Property(p => p.IsMoldOperation);
                //View.Property(p => p.IsMaterialOperation);
                //View.Property(p => p.ExceptionCall);
                //View.Property(p => p.ExceptionResponse);

            }
        }
        /// <summary>
        /// 
        /// </summary>

        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.UseCommands(typeof(SetDefaultCommand).FullName, typeof(InitializationCommand).FullName);
            View.Property(p => p.PlanCode).Show(ShowInWhere.Detail).HasOrderNo(10);
            View.Property(p => p.PlanName).Show(ShowInWhere.Detail).HasOrderNo(20);
            View.Property(p => p.PlanTemplateName).Show(ShowInWhere.Detail).HasOrderNo(30);
            View.Property(p => p.Description).Show(ShowInWhere.Detail).HasOrderNo(40);
            View.Property(p => p.EnableStatus).Show(ShowInWhere.Detail).HasOrderNo(50);
            View.Property(p => p.IsDefault).Show(ShowInWhere.Detail).HasOrderNo(60).Readonly();

            View.ChildrenProperty(p => p.ProcessInfoList).HasOrderNo(1).Show(ChildShowInWhere.List);

            View.AttachDetailChildrenProperty(typeof(WorkReportPlan), (c) =>
            {
                var account = c.Parent as WorkReportPlan;
                account = RF.GetById<WorkReportPlan>(account.Id, new EagerLoadOptions().LoadWithViewProperty());
                return account;
            }, StartWorkButtonDetailGroup).HasLabel("开工按钮设置").HasOrderNo(100).Show(ChildShowInWhere.All);

            View.AttachDetailChildrenProperty(typeof(WorkReportPlan), (c) =>
            {
                var account = c.Parent as WorkReportPlan;
                account = RF.GetById<WorkReportPlan>(account.Id, new EagerLoadOptions().LoadWithViewProperty());
                return account;
            },ReportButtonDetailGroup).HasLabel("报工按钮设置").HasOrderNo(100).Show(ChildShowInWhere.All);
            View.AttachDetailChildrenProperty(typeof(WorkReportPlan), (c) =>
            {
                var account = c.Parent as WorkReportPlan;
                account = RF.GetById<WorkReportPlan>(account.Id, new EagerLoadOptions().LoadWithViewProperty());
                return account;
            }, ButtonAreaDetailGroup).HasLabel("按钮区域配置").HasOrderNo(100).Show(ChildShowInWhere.All);

            
        }
    }
}
