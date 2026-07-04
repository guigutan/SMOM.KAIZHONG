using SIE.MetaModel.View;
using SIE.Tech.Processs;

namespace SIE.Web.Tech.Processs
{
    /// <summary>
    /// 工序包装单位视图配置
    /// </summary>
    public class ProcessPackingUnitViewConfig : WebViewConfig<ProcessPackingUnit>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.FormEdit();
            View.UseCommands("SIE.Web.Tech.Processs.Commands.ProcessPackingUnitCommand", WebCommandNames.Delete);
            View.Property(p => p.PackageUnit).Readonly();
            View.Property(p => p.PackageUnitName).Readonly();
            View.Property(p => p.Description).Readonly();
            View.Property(p => p.IsMasterUnit).Readonly();
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.PackageUnit);
            View.Property(p => p.PackageUnitName);
            View.Property(p => p.Description);
            View.Property(p => p.IsMasterUnit);
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.PackageUnitCode);
            View.Property(p => p.PackageUnitName);
        }

        /// <summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.PackageUnitCode);
            View.Property(p => p.PackageUnitName);
        }
    }
}