using SIE.MES.PackingPrints;
using SIE.Wpf.Common.ViewBehaviors;
using SIE.Wpf.MES.PackingPrints.Commonds;

namespace SIE.Wpf.MES.PackingPrints
{
    /// <summary>
    /// 包装号视图配置
    /// </summary>
    internal class PackingBarcodeViewConfig : WPFViewConfig<PackingBarcode>
    {
        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            // 配置视图
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(PackingWorkOrder));
            View.UseCommands(typeof(ReprintCommand));
            View.AddBehavior(typeof(MultipleRowViewBehavior));
            View.Property(p => p.Code).Readonly().ShowInList(150);
            View.Property(p => p.PackageUnitName).Readonly();
            View.Property(p => p.IsUse).Readonly();
            View.Property(p => p.PrintDate).Readonly().ShowInList(150);
            View.Property(p => p.PrintedState).Readonly();
            View.Property(p => p.PrintTimes).Readonly();
            View.Property(p => p.PrintBy).Readonly();
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }

        ///<summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            // 配置视图
        }

        ///<summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            // 配置视图
        }
    }
}
