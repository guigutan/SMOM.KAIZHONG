using SIE.MES.LoadItems;

namespace SIE.Wpf.MES.LoadItems
{
    /// <summary>
    /// 物料校准日志视图配置
    /// </summary>
    internal class CalibrationLogViewConfig : WPFViewConfig<CalibrationLog>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.WorkOrderNo).HasLabel("工单");
            View.Property(p => p.ItemCode).HasLabel("物料编码");
            View.Property(p => p.Label);
            View.Property(p => p.Qty);
            View.Property(p => p.RemainQty);
            View.Property(p => p.CalibrationQty);
            View.Property(p => p.CreateDate).HasLabel("操作时间");
            View.Property(p => p.CreateByName).HasLabel("操作人");
            View.Property(p => p.Remark);
        }
    }
}