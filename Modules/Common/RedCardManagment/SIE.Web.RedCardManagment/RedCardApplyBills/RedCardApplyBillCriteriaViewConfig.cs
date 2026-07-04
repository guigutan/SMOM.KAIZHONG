using SIE.RedCardManagment.RedCardApplyBills;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.RedCardManagment.RedCardApplyBills
{

    /// <summary>
    /// 红牌申请单查询实体视图配置
    /// </summary>
    public class RedCardApplyBillCriteriaViewConfig : WebViewConfig<RedCardApplyBillCriteria>
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.No).Show();
                View.Property(p => p.ItemId).Show();
                View.Property(p => p.SupplierId).Show();
                View.Property(p => p.ApplyType).Show();
                View.Property(p => p.BillStatus).Show();
                View.Property(p => p.CreateDate).UseDateRangeEditor(p => { p.DateFormat = "Y/m/d"; p.DateRangeType = ObjectModel.DateRangeType.Month; }).Show();
            }
        }
    }
}
