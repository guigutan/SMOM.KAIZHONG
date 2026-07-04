using SIE.EMS.InventoryBalances;
using SIE.EMS.InventoryPlans;
using SIE.Equipments.WorkFlows;

namespace SIE.Web.EMS.WorkFlows
{
    /// <summary>
    /// 审核记录-界面
    /// </summary>
    public class WorkFlowRecordViewConfig : WebViewConfig<WorkFlowRecord>
    {
        /// <summary>
        /// 附加查看-审核进度
        /// </summary>
        public const string EmsSeeView = "EmsSeeView";

        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(EmsSeeView);
            if (ViewGroup == EmsSeeView)
            {
                ConfigEmsSeeView();
            }
        }

        /// <summary>
        /// 附加查看-审核进度
        /// </summary>
        void ConfigEmsSeeView()
        {
            View.AssignAuthorize(typeof(InventoryPlan), typeof(InventoryBalance));
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.ApproverId).ShowInList(80);
                View.Property(p => p.ApprovalResult).ShowInList(100);
                View.Property(p => p.ApprovalDatetime).ShowInList(150);
                View.Property(p => p.Remark).ShowInList(600);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}
