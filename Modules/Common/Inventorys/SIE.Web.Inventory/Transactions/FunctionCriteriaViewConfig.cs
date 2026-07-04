using SIE.Inventory.Transactions;
using SIE.Warehouses;

namespace SIE.Web.Inventory.Transactions
{
    /// <summary>
    /// 查询视图
    /// </summary>
    public class FcuntionCriteriaViewConfig : WebViewConfig<FunctionCriteria>
    {
        /// <summary>
        /// 默认视图
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show();
                View.Property(p => p.Name).Show();
                View.Property(p => p.Description).Show();                            
                View.Property(p => p.SourceType).Show();
                View.Property(p => p.State).Show();
                View.Property(p => p.FunctionType).Show();                
            }
        }
    }
}
