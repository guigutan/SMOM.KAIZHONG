using SIE.MES.WIP.Products;

namespace SIE.Web.MES.WIP.Products
{
    /// <summary>
    /// 生产产品版本视图配置
    /// </summary> 
    internal class WipProductVersionViewConfig : WebViewConfig<WipProductVersion>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands("SIE.Web.MES.WIP.Products.ExportReportCommand");
            View.Property(p => p.Sn).ShowInList(150).Readonly().FixColumn(true);
            View.Property(p => p.KeyLabel).ShowInList(150).Readonly().FixColumn(true).HasLabel("组件条码");
            View.Property(p => p.WorkOrderNo).ShowInList(150).FixColumn(true).Readonly();
            View.Property(p => p.ProductCode).ShowInList(150).FixColumn(true).Readonly();
            View.Property(p => p.ProductName).ShowInList(150).FixColumn(true).Readonly();
            View.Property(p => p.ItemExtPropName).Readonly();
            View.Property(p => p.RelevanceSn).Readonly();
            View.Property(p => p.IsHold).Readonly();
            View.Property(p => p.WoType).UseEnumEditor().Readonly();
            View.Property(p => p.WoPlanQty).Readonly();
            View.Property(p => p.VersionName).Readonly();
            View.Property(p => p.WorkShopName).Readonly();
            View.Property(p => p.ModelName).Readonly();
            View.Property(p => p.NowProcessName).HasLabel("当前工序").Readonly();
            View.Property(p => p.NextProcess).Readonly();
            View.Property(p => p.ResourceName).Readonly();
            View.Property(p => p.Grade).Readonly();
            View.Property(p => p.IsOutsourcing).Readonly();
            View.Property(p => p.IsFinish).Readonly();
            View.Property(p => p.FinishDateTime).ShowInList(150).Readonly();
            View.Property(p => p.IsScrapped).Readonly();
            View.ChildrenProperty(p => p.InspectionItemList).HasOrderNo(1).Readonly();
            View.ChildrenProperty(p => p.ProcessList).HasOrderNo(10).Readonly();
            View.ChildrenProperty(p => p.RepaireList).HasOrderNo(20).Readonly();
            View.ChildrenProperty(p => p.DefectList).HasOrderNo(60).Readonly();
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.WorkOrderNo).HasLabel("工单号").Readonly(false);
            View.Property(p => p.Sn);
        }
    }
}