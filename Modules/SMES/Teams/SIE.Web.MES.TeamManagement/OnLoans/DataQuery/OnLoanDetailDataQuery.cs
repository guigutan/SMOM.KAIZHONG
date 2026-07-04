using SIE.Domain;
using SIE.MES.TeamManagement.OnLoans;
using SIE.Web.Data;
using System.Linq;

namespace SIE.Web.MES.TeamManagement.OnLoans
{
    /// <summary>
    /// 借调明细数据查询类
    /// </summary>
    public class OnLoanDetailDataQuery : DataQueryer
    {
        /// <summary>
        /// 获取班组借调明细集合
        /// </summary>
        /// <param name="onLoanId"> 班组借调Id</param>
        /// <returns>班组借调明细集合</returns>
        public EntityList<OnLoanDetail> GetOnLoanDetails(double onLoanId)
        {
            EntityList<OnLoanDetail> onLoanDetails = null;
            var curWorkGroupOnLoan = RF.GetById<WorkGroupOnLoan>(onLoanId);
            if (curWorkGroupOnLoan != null)
            {
                var detailList = curWorkGroupOnLoan.DetailList.OrderBy(x => x.RowIndex).ToList();
                onLoanDetails = new EntityList<OnLoanDetail>();
                onLoanDetails.AddRange(detailList);
            }

            return onLoanDetails;
        }
    }
}
