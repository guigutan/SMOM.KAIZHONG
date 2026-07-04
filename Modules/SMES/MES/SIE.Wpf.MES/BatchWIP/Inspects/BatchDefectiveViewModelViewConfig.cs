using SIE.Wpf.MES.BatchWIP.Inspects.Commands;

namespace SIE.Wpf.MES.BatchWIP.Inspects
{
    /// <summary>
    /// 批次检验不良记录视图类
    /// </summary>
    internal class BatchDefectiveViewModelViewConfig : WPFViewConfig<BatchDefectiveViewModel>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseDefaultBehaviors();
            View.AssignAuthorize(typeof(BatchInspectViewModel));
            View.UseCommands(typeof(DeleteBatchDefectiveVmlCommand));
            using (View.OrderProperties())
            {
                View.Property(p => p.Barcode).Readonly();
                View.Property(p => p.ChildBarcode).Readonly();
                View.Property(p => p.Defects).HasLabel("缺陷代码").Readonly();
                View.Property(p => p.Descriptions).HasLabel("缺陷描述").Readonly();
                View.Property(p => p.NgQty).UseSpinEditor(e => { e.MinValue = 1; e.Decimals = 0; }).Readonly();
                View.Property(p => p.InspectDate).Readonly();
                View.ChildrenProperty(p => p.BatchWipPrdDrtDetails).Visible(false).HasLabel("缺陷代码列表");
            }
        }
    }
}