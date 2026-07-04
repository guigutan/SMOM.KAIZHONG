using SIE.Domain;
using SIE.MES.TeamManagement.OnLoans;
using SIE.MetaModel.View;
using SIE.Web.Common.Configs.Commands;
using System.Linq;

namespace SIE.Web.MES.TeamManagement.OnLoans
{
    /// <summary>
    /// 班组借调视图配置类
    /// </summary>
    public class WorkGroupOnLoanViewConfig : WebViewConfig<WorkGroupOnLoan>
    {
        /// <summary>
        ///  出勤工时统计
        /// </summary>
        public static readonly string AttentViewGroup = "AttentViewGroup";

        /// <summary>
        /// 配置通用视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(AttentViewGroup);
            View.ClearCommands();
            if (ViewGroup == AttentViewGroup)
            {
                ConfigAttentViewGroupView();
            }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.ExportXls, ConfigCommands.ModuleConfigCommand);
            using (View.OrderProperties())
            {
                View.Property(x => x.No).ShowInList(width: 150).Readonly()
                    .UseListSetting(e => { e.HelpInfo = string.Format("根据{0}(配置项--{0})生成{1}借调单号", "单号生成规则", "班组借调"); });
                View.Property(x => x.InitiatorName).HasLabel("发起人").Readonly();
                View.Property(x => x.GroupInName).HasLabel("借入班组").Readonly();
                View.Property(x => x.DemandQty).Readonly();
                View.Property(x => x.BeginDate).ShowInList(150).Readonly();
                View.Property(x => x.EndDate).ShowInList(150).Readonly();
                View.Property(x => x.ApproverName).HasLabel("审核人").Readonly();
                View.Property(x => x.GroupOutName).HasLabel("借出班组").Readonly();
                View.Property(x => x.State).Readonly();
                View.ChildrenProperty(x => x.DetailList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(x => x.EmployeeList).Show(ChildShowInWhere.Hide);
                View.AttachDetailChildrenProperty(typeof(OnLoanDetail), (c) =>
                 {
                     var parentObj = c.Parent as WorkGroupOnLoan;
                     var workGroupOnLoad = RF.GetById<WorkGroupOnLoan>(parentObj.Id);
                     var onLoadDetailDefault = workGroupOnLoad.DetailList.OrderBy(x => x.RowIndex).FirstOrDefault();
                     return onLoadDetailDefault;
                 }, ViewConfig.DetailsView).HasLabel("审批流程").Show(ChildShowInWhere.All);
            }
        }

        /// <summary>
        /// 出勤工时统计
        /// </summary>
        void ConfigAttentViewGroupView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.No).ShowInList(150).Readonly();
                View.Property(p => p.BeginDate).ShowInList(150).Readonly();
                View.Property(p => p.EndDate).ShowInList(150).Readonly();
                View.Property(p => p.LoanHour).ShowInList().Readonly();
                View.Property(p => p.GroupInName).ShowInList().Readonly();
                View.Property(p => p.GroupOutName).ShowInList().Readonly();
                View.ChildrenProperty(x => x.DetailList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(x => x.EmployeeList).Show(ChildShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(x => x.No);
                View.Property(x => x.Initiator);
                View.Property(x => x.GroupIn);
                View.Property(x => x.GroupOut);
                View.Property(x => x.Approver);
                View.Property(x => x.State);
            }
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            using (View.OrderProperties())
            {
                View.Property(x => x.No);
                View.Property(x => x.Initiator);
                View.Property(x => x.GroupIn);
                View.Property(x => x.GroupOut);
                View.Property(x => x.Approver);
                View.Property(x => x.State);
            }
        }
    }
}
