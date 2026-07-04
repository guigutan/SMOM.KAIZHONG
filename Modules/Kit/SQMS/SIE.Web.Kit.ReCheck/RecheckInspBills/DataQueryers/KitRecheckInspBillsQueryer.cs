using SIE.Domain;
using SIE.Kit.ReCheck.RecheckInspBills;
using SIE.Recheck.RecheckInspBills;
using SIE.Web.Data;
using System;
using System.Linq;

namespace SIE.Web.Kit.ReCheck.RecheckInspBills.DataQueryers
{
    /// <summary>
    /// 超期复检查询器
    /// </summary>
    public class KitRecheckInspBillsQueryer : DataQueryer
    {
        /// <summary>
        /// 获取Asn号中的ReelID信息
        /// </summary>
        /// <param name="reelID"></param>
        /// <param name="billId"></param>
        /// <returns></returns>
        public RecheckInspBillReelIDInfo GetReelIDInAsnNo(string reelID, double billId)
        {
            var bill = RF.GetById<RecheckInspBill>(billId);
            Check.AssertNotNull(bill, "单据不能为空".L10N());
            if (bill.GetBillSourceReelList().IsNullOrEmpty())
                return null;
            var SourceReel = bill.GetBillSourceReelList().FirstOrDefault(p => p.ReelId == reelID);
            if (SourceReel != null)
            {
                return new RecheckInspBillReelIDInfo()
                {
                    ReelID = SourceReel.ReelId,
                    Qty = SourceReel.Quannity
                };
            }
            return null;
        }
    }
}
