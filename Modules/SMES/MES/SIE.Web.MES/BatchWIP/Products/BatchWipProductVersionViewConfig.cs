using SIE.Domain;
using SIE.Items;
using SIE.MES.BatchWIP.Products;
using SIE.MES.WIP;

namespace SIE.Web.MES.BatchWIP.Products
{
    /// <summary>
    /// 生产批次版本视图配置
    /// </summary>
    internal class BatchWipProductVersionViewConfig : WebViewConfig<BatchWipProductVersion>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseLayoutSize(-4, -6);
            View.ClearCommands();
            View.UseClientOrder();
            View.UseCommands("SIE.Web.MES.BatchWIP.Products.ExportReportCommand");
            View.Property(p => p.BatchNo).Readonly().ShowInList(width: 150);
            View.Property(p => p.ContainerNo).Readonly().ShowInList(width: 150);
            View.Property(p => p.RemainQty).Readonly();
            View.Property(p => p.DefectState).Readonly();
            View.Property(p => p.Qty).Readonly();
            View.Property(p => p.ScrapQty).Readonly().HasLabel("批次报废数");
            View.Property(p => p.WorkOrderNo).Readonly().HasLabel("工单编号").ShowInList(width: 150);
            View.Property(p => p.IsHold).Readonly().HasLabel("是否Hold");
            View.Property(p => p.WoType).Readonly().HasLabel("工单类型").UseEnumEditor();
            View.Property(p => p.ProductCode).Readonly().HasLabel("产品编码");
            View.Property(p => p.ModelName).Readonly().HasLabel("产品型号");
            View.Property(p => p.WoPlanQty).Readonly().HasLabel("工单数量");
            View.Property(p => p.WoFinishQty).Readonly().HasLabel("工单完工数");
            View.Property(p => p.WoScrapQty).Readonly().HasLabel("工单报废数");
            View.Property(p => p.WorkShopName).Readonly().HasLabel("车间");
            View.Property(p => p.ResourceName).Readonly().HasLabel("当前资源");
            View.Property(p => p.ProcessName).Readonly().HasLabel("当前工序");
            View.Property(p => p.StationName).Readonly().HasLabel("当前工位");
            View.Property(p => p.NextProcessId).Readonly();
            View.Property(p => p.IsFinish).Readonly();
            View.Property(p => p.IsOutsourcing).Readonly();
            View.ChildrenProperty(p => p.ProcessList).Show(ChildShowInWhere.Hide).HasLabel("批次采集记录");
            View.ChildrenProperty(p => p.BatchWipRecordList).HasOrderNo(10).HasLabel("批次采集记录");
            View.ChildrenProperty(p => p.RepaireList).HasOrderNo(20).HasLabel("产品维修记录");
            View.ChildrenProperty(p => p.DefectList).HasOrderNo(30).HasLabel("产品缺陷记录");
            View.ChildrenProperty(p => p.BatchSplitMergeList).Show(ChildShowInWhere.Hide);

        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.WorkOrderNo).HasLabel("工单号");
            View.Property(p => p.BatchNo);
            View.Property(p => p.ProductCode).HasLabel("产品编码");
            View.Property(p => p.WorkShopName);
            View.Property(p => p.Resource).HasLabel("当前生产资源");
            View.Property(p => p.Process);
            View.Property(p => p.NextProcess);
            View.Property(p => p.IsFinish);
        }
    }
}
