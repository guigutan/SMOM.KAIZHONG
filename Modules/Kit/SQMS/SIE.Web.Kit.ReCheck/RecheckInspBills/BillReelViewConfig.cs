using SIE.Kit.ReCheck.RecheckInspBills;
using SIE.Recheck.RecheckInspBills;
using SIE.Web.Kit.Recheck.RecheckInspBills.Commands.ReelIDs;

namespace SIE.Web.Kit.ReCheck.RecheckInspBills
{
    /// <summary>
    /// 电子行业单据ReelID实体配置
    /// </summary>
    public class BillReelViewConfig : WebViewConfig<BillReel>
    {
        /// <summary>
        /// 设置只读视图
        /// </summary>
        public const string ReadonlyView = "ReadonlyView";

        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            switch (ViewGroup)
            {
                case ReadonlyView:
                    ConfigReadonlyView();
                    break;
            }
            View.AssignAuthorize(typeof(RecheckInspBill));
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.Kit.Recheck.RecheckInspBills.Behaviors.RecheckInspBillReelIdBehavior");
            View.UseCommands(ReelIDCommands.AddCommand, ReelIDCommands.EditCommand, ReelIDCommands.DeleteCommand);
            using (View.OrderProperties())
            {
                View.Property(p => p.ReelId).UseTextEditor(p => p.XType = "recheckEeelIDTextField");
                View.Property(p => p.Quannity);
            }
        }

        /// <summary>
        /// 配置只读列表视图
        /// </summary>
        protected void ConfigReadonlyView()
        {
            View.ClearCommands();
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.ReelId).Show(ShowInWhere.List);
                View.Property(p => p.Quannity).Show(ShowInWhere.List);
            }
        }
    }
}
