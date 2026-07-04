using SIE.EMS.Purchases.EquipmentAcceptances;
using SIE.EMS.Purchases.EquipmentSetups;
using SIE.EMS.Purchases.PaymentPlans;
using SIE.EMS.Purchases.PurchaseOrders;
using SIE.EMS.Purchases.PurchaseRequisitions;
using SIE.EMS.Purchases.SparePartAcceptances;
using SIE.Equipments.WorkFlows;
using SIE.Web.ClientMetaModel;

namespace SIE.Web.EMS.Purchases.WorkFlows
{
    /// <summary>
    /// 审核记录-界面
    /// </summary>
    public class WorkFlowRecordViewConfig : WebViewConfig<WorkFlowRecord>
    {
        /// <summary>
        /// 附加查看-审核进度
        /// </summary>
        public const string PurSeeView = "PurSeeView";

        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(PurSeeView);
            if (ViewGroup == PurSeeView)
            {
                ConfigPurSeeView();
            }
        }

        /// <summary>
        /// 附加查看-审核进度
        /// </summary>
        void ConfigPurSeeView()
        {
            View.AssignAuthorize(typeof(PurchaseRequisition), typeof(PurchaseOrder), typeof(PaymentPlan), typeof(EquipmentAcceptance), typeof(SparePartAcceptance),
                typeof(EquipmentSetup));
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.ApproverId).UseTextEditor(p =>p.AllowAsterisk = AllowAsterisks.close).ShowInList(80);
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
