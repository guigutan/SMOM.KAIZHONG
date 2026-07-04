using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Recheck.RecheckInspBills;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Kit.ReCheck.RecheckInspBills
{
    /// <summary>
    /// 超期复检单控制器
    /// </summary>
    public class KitRecheckInspBillController : RecheckInspBillController
    {
        /// <summary>
        /// 根据单据ID获取ReelID清单
        /// </summary>
        /// <param name="billId">单据id</param>
        /// <param name="paging">分页信息</param>
        /// <param name="sortInfo">排序信息</param>
        /// <returns></returns>
        public virtual EntityList<BillReel> GetBillReelsByBillId(double billId, PagingInfo paging = null, IList<OrderInfo> sortInfo = null)
        {
            var q = Query<BillReel>()
                .Where(p => p.RecheckInspBillId == billId);
            if (sortInfo != null)
                q.OrderBy(sortInfo);
            return q.ToList(paging, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 提交超期复检单
        /// </summary>
        /// <param name="bill">超期复检单</param>
        /// <returns>超期复检单</returns>
        public override RecheckInspBill SumbitBill(RecheckInspBill bill)
        {
            //判断源reelId列表中的reelId是否已全部添加ReelId清单中检验
            var sourceBill = RF.GetById<RecheckInspBill>(bill.Id);
            if (!sourceBill.GetBillSourceReelList().IsNullOrEmpty())
            {
                var sourcrReelIds = sourceBill?.GetBillSourceReelList().Select(x => x.ReelId).ToList();
                var reelIds = bill.GetReelList().Select(x => x.ReelId).ToList();
                string error = string.Join(",", sourcrReelIds.Except(reelIds).ToArray());
                if (!string.IsNullOrEmpty(error))
                {
                    string message = $"数据提交失败，源ReelId值[{error}]需要添加到RellId检验清单！";
                    throw new ValidationException(message: message);
                }
            }
            return base.SumbitBill(bill);
        }
    }
}
