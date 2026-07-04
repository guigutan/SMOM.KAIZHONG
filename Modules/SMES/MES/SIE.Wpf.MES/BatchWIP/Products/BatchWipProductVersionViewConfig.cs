using SIE.MES.BatchWIP.Products;

namespace SIE.WPF.MES.BatchWIP.Products
{
    /// <summary>
    /// 生产批次版本视图配置
    /// </summary>
    internal class BatchWipProductVersionViewConfig : WPFViewConfig<BatchWipProductVersion>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseLayoutSize(-4, -6);
            View.ClearCommands();
            View.Property(p => p.BatchNo).UseListSetting(e => e.ListGridWidth = 150);
            View.Property(p => p.Qty);
            View.Property(p => p.ScrapQty).HasLabel("批次报废数");
            View.Property(p => p.WorkOrderNo).HasLabel("工单编号");
            View.Property(p => p.IsHold).HasLabel("是否Hold");
            View.Property(p => p.WoType).HasLabel("工单类型").UseEnumEditor();
            View.Property(p => p.ModelName).HasLabel("产品型号");
            View.Property(p => p.WoPlanQty).HasLabel("工单数量");
            View.Property(p => p.WoFinishQty).HasLabel("工单完工数");
            View.Property(p => p.WoScrapQty).HasLabel("工单报废数");
            View.Property(p => p.WorkShopName).HasLabel("车间");
            View.Property(p => p.ResourceName).HasLabel("当前资源");
            View.Property(p => p.IsFinish);
            View.ChildrenProperty(p => p.ProcessList).HasOrderNo(10).HasLabel("批次采集记录");
            View.ChildrenProperty(p => p.RepaireList).HasOrderNo(20).HasLabel("产品维修记录");
            View.ChildrenProperty(p => p.DefectList).HasOrderNo(30).HasLabel("产品缺陷记录");
            View.ChildrenProperty(p => p.BatchSplitMergeList).HasOrderNo(40).HasLabel("批次合并拆分记录");
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.WorkOrder.No).HasLabel("工单号").Readonly(false);
            View.Property(p => p.BatchNo);
        }
    }
}