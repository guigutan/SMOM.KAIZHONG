using SIE.Inventory.Onhands;
using SIE.MetaModel.View;

namespace SIE.Web.Inventory.Onhands
{
    /// <summary>
    /// 选择库存查询实体视图配置
    /// </summary>
    public class SelectOnhandCritreiaViewConfig : WebViewConfig<SelectOnhandCriteria>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.ClearCommands();
            View.UseCommands(WebCommandNames.ExecuteQuery);
            using (View.OrderProperties())
            {
                View.Property(p => p.ItemCode).Show();
                View.Property(p => p.ItemName).Show();               
                View.Property(p => p.LocCode).Show();
                View.Property(p => p.Lpn).Show();
                View.Property(p => p.LotCode).Show();
                View.Property(p => p.OnhandState).Show();
                View.Property(p => p.ProjectNo).Show();
                View.Property(p => p.TaskNo).Show();
                View.Property(p => p.AutoExpectQty).Show();
            }
        }
    }
}