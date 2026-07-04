using SIE.LES.Distributions;
using SIE.MetaModel.View;

namespace SIE.Web.LES.Distributions
{
    /// <summary>
    /// 配送管理
    /// </summary>
    public class DistributionLabelViewConfig : WebViewConfig<DistributionLabel>
    {
        /// <summary>
        /// 视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();            
            View.DisableEditing();
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                //View.Property(p => p.LineNo);             
                View.Property(p => p.ItemCode).ShowInList(120);
                View.Property(p => p.ItemName).ShowInList(120);               
                View.Property(p => p.HighestNo).ShowInList(120);
                View.Property(p => p.LabelNo).ShowInList(120);
                View.Property(p => p.Qty);
                View.Property(p => p.AssignId);
                View.Property(p => p.IsSerialNumber).Readonly();               
                View.Property(p => p.CreateByName).Readonly();
                View.Property(p => p.CreateDate).Readonly();
                View.Property(p => p.UpdateByName).Readonly();
                View.Property(p => p.UpdateDate).Readonly();                 
            }
        }
    }
}
