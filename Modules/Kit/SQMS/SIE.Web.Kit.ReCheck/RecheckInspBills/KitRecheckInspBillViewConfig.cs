using SIE.Domain;
using SIE.Kit.ReCheck.RecheckInspBills;
using SIE.Recheck.RecheckInspBills;
using SIE.Web.Recheck.RecheckInspBills;

namespace SIE.Web.Kit.ReCheck.RecheckInspBills
{
    /// <summary>
    /// 超期复检单视图配置
    /// </summary>
    public class KitRecheckInspBillViewConfig : WebViewConfig<RecheckInspBill>
    {
        /// <summary>
        /// 逻辑视图配置
        /// </summary>
        protected override void ConfigView()
        {
            if (ViewGroup == RecheckInspBillViewConfig.WritingReportView)
            {
                ConfigReportView();
            }
            if (ViewGroup == RecheckInspBillViewConfig.ReadonlyView)
            {
                ConfigReadonlyView();
            }
        }

        /// <summary> 
        /// 填写报告视图
        /// </summary>
        private void ConfigReportView()
        {
            View.AssociateChildrenProperty(RecheckInspBillEx.ReelListProperty, c =>
            {
                var arg = c as ChildPagingDataArgs;
                var bill = c.Parent as RecheckInspBill;
                if (bill != null)
                {
                    var reelList = RT.Service.Resolve<KitRecheckInspBillController>().GetBillReelsByBillId(bill.Id, arg.PagingInfo, arg.SortInfo);
                    if (reelList != null)
                        return reelList;
                }
                return new EntityList<RecheckInspBill>();
            }).HasLabel("ReelID清单").Show(ChildShowInWhere.All).OrderNo = 4;
        }

        /// <summary>
        /// 只读视图配置
        /// </summary>
        void ConfigReadonlyView()
        {
            View.AssociateChildrenProperty(RecheckInspBillEx.ReelListProperty, c =>
            {
                var arg = c as ChildPagingDataArgs;
                var bill = c.Parent as RecheckInspBill;
                if (bill != null)
                {
                    var reelList = RT.Service.Resolve<KitRecheckInspBillController>().GetBillReelsByBillId(bill.Id, arg.PagingInfo, arg.SortInfo);
                    if (reelList != null)
                        return reelList;
                }
                return new EntityList<RecheckInspBill>();
            }, BillReelViewConfig.ReadonlyView).HasLabel("ReelID清单").Show(ChildShowInWhere.All).OrderNo = 4;
        }
    }
}
