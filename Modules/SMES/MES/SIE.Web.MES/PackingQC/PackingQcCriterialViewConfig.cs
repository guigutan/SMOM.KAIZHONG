using SIE.MES.PackingQC;
using SIE.Resources.WipResources;

namespace SIE.Web.MES.PackingQC
{
    /// <summary>
    /// 包装QC确认实体查询
    /// </summary>
    public class PackingQcCriterialViewConfig : WebViewConfig<PackingQcCriterial>
    {
        /// <summary>
        /// 视图查询
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.BlueLabel).Show();
                View.Property(p => p.Confirm).Show();
                View.Property(p => p.PackIdent).Show();
                View.Property(p => p.WorkOrderNo).Show();
                View.Property(p => p.ItemCode).Show();
                View.Property(p => p.ProductLabel).Show();
                View.Property(p => p.BatchLabel).Show();
                View.Property(p => p.ReportsType).Show();
                View.Property(p => p.ResourceId).Show().UsePagingLookUpEditor(p =>
                {
                    p.SearchFieldList.Add(WipResource.CodeProperty.Name);
                    p.SearchFieldList.Add(WipResource.NameProperty.Name);
                });
                View.Property(p => p.CreateDate).UseDateRangeEditor().Show();
            }
        }
    }
}
