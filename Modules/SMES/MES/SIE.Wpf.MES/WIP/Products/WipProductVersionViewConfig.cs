using SIE.MES.WIP.Products;
using SIE.MetaModel.View;
using SIE.WPF.MES.WIP.Products;

namespace SIE.Wpf.MES.WIP.Products
{
    /// <summary>
    /// 生产产品版本视图配置
    /// </summary> 
    internal class WipProductVersionViewConfig : WPFViewConfig<WipProductVersion>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(typeof(ExportReportCommand));
            View.Property(p => p.Sn).Readonly().FixColumn(ColumnFixedStyle.Left);
            View.Property(p => p.CombinedCode).Show(ShowInWhere.Hide).Readonly().FixColumn(ColumnFixedStyle.Left);
            View.Property(p => p.KeyLabel).Readonly().FixColumn(ColumnFixedStyle.Left).HasLabel("组件条码");
            View.Property(p => p.WorkOrderNo).Readonly().FixColumn(ColumnFixedStyle.Left);
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
            View.Property(p => p.IsFinish).Readonly();
            View.Property(p => p.IsScrapped).Readonly();
            View.ChildrenProperty(p => p.ProcessList).HasOrderNo(10).Readonly();
            View.ChildrenProperty(p => p.RepaireList).HasOrderNo(20).Readonly();
            View.ChildrenProperty(p => p.DefectList).HasOrderNo(60).Readonly();
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.WorkOrder.No).HasLabel("工单号").Readonly(false);
            View.Property(p => p.Sn);
        }
    }
}