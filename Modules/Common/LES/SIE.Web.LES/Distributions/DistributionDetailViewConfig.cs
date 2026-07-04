using SIE.LES.Distributions;

namespace SIE.Web.LES.Distributions
{
    /// <summary>
    /// 配送管理
    /// </summary>
    public class DistributionDetailViewConfig : WebViewConfig<DistributionDetail>
    {
        /// <summary>
        /// 视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();            
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.LineNo);
                View.Property(p => p.AssignId);
                View.Property(p => p.Qty);
                View.Property(p => p.ItemCode).ShowInList(120);
                View.Property(p => p.ItemName).ShowInList(120);
                View.Property(p => p.ItemSpec);
                View.Property(p => p.ItemExtPropName).ShowInList(180);
                View.Property(p => p.UnitName);
                View.Property(p => p.LotCode);
                View.Property(p => p.OnhandState);
                View.Property(p => p.OrderNo);
                View.Property(p => p.OrderLineNo);
                View.Property(p => p.SoLineNo);
                View.Property(p => p.CreateByName).Readonly();
                View.Property(p => p.CreateDate).Readonly();
                View.Property(p => p.UpdateByName).Readonly();
                View.Property(p => p.UpdateDate).Readonly();                 
            }
        }
    }
}
