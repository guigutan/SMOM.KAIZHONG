using SIE.MES.WIP.Pressure;
using SIE.MetaModel.View;

namespace SIE.Web.MES.WIP.Pressure
{
    /// <summary>
    /// 视图配置
    /// </summary>
    internal class WipPressureViewConfig : WebViewConfig<WipPressure>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {

            View.UseCommands(WebCommandNames.ExportXls);
            using (View.OrderProperties())
            {
                View.Property(p => p.WorkOrderNo).ShowInList(150).Readonly();
                View.Property(p => p.BatchNo).ShowInList(150).Readonly();
                View.Property(p => p.ResourceCode).ShowInList(150).Readonly();
                View.Property(p => p.ResourceName).ShowInList(150).Readonly();
                View.Property(p => p.ProductCode).ShowInList(150).Readonly();
                View.Property(p => p.ProductName).ShowInList(200).Readonly();
                View.Property(p => p.Qty).ShowInList().Readonly();
                View.Property(p => p.OriginalQty).ShowInList().Readonly();
                View.Property(p => p.BeginTime).ShowInList(150).Readonly();
                View.Property(p => p.EndTime).ShowInList(150).Readonly();

                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);

                View.ChildrenProperty(p => p.WipPressureSnList).HasLabel("SN明细");

            }
        }

        protected override void ConfigQueryView()
        {
            View.Property(p => p.BatchNo).Show();
            View.Property(p => p.ProductCode).Show().Readonly(false);
            View.Property(p => p.ProductName).Show().Readonly(false);
            View.Property(p => p.ResourceCode).Show().Readonly(false);
            View.Property(p => p.ResourceName).Show().Readonly(false);
            View.Property(p => p.BeginTime).Show().UseDateRangeEditor(p => { p.DateRangeType = ObjectModel.DateRangeType.Week; });
        }
    }
}