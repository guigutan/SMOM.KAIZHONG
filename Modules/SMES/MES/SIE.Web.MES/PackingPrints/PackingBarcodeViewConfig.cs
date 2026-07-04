using SIE.MES.PackingPrints;

namespace SIE.Web.MES.PackingPrints
{
    /// <summary>
    /// 包装号视图配置
    /// </summary>
    internal class PackingBarcodeViewConfig : WebViewConfig<PackingBarcode>
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
            View.UseGridSelectionModel();
            View.UseCommands(typeof(Commands.ReprintCommand).FullName);
            View.Property(p => p.Code).Readonly().ShowInList(width: 150);
            View.Property(p => p.PackageUnitName).Readonly();
            View.Property(p => p.IsUse).Readonly();
            View.Property(p => p.PrintDate).Readonly().ShowInList(width: 150);
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
