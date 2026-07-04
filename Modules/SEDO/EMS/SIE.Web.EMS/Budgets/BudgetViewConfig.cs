using SIE.EMS.Budgets;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.Budgets
{
    /// <summary>
    /// 公共项目预算视图
    /// </summary>
    public class BudgetViewConfig : WebViewConfig<Budget>
    {
        /// <summary>
        /// 千分位
        /// </summary>
        private const string percent = "0,000.00";
        /// <summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.BudgetNo).ShowInList(130).FixColumn();
            View.Property(p => p.BudgetName).ShowInList(200).FixColumn();
            View.Property(p => p.Price).UseSpinEditor(c => c.Format = percent).ShowInList(110);
            View.Property(p => p.PriceBasis).ShowInList(200);
            View.Property(p => p.BudgetAmount).UseSpinEditor(c => c.Format = percent).ShowInList(110);
            View.Property(p => p.ReserveAmount).UseSpinEditor(c => c.Format = percent).ShowInList(110);
            View.Property(p => p.UsedAmount).UseSpinEditor(c => c.Format = percent).ShowInList(110);
            View.Property(p => p.CanUseAmount).UseSpinEditor(c => c.Format = percent).ShowInList(110);
        }
    }
}
