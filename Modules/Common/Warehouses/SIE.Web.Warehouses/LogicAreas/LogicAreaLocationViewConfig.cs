using SIE.MetaModel.View;
using SIE.Warehouses;
using SIE.Web.Warehouses.Commands;

namespace SIE.Web.Warehouses
{
    /// <summary>
    /// 逻辑分区与库位关系表视图配置
    /// </summary>
    internal class LogicAreaLocationViewConfig : WebViewConfig<LogicAreaLocation>
    {
        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(LogicArea));
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();          
            View.DisableEditing();
            View.UseCommands(typeof(LogicAreaSelLocationCommand).FullName, typeof(LogicAreaLocDeleteCommand).FullName, WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            View.Property(p => p.StorageLocationCode);
            View.Property(p => p.StorageLocationName);
            View.Property(p => p.WarehouseCode);
            View.Property(p => p.AreaCode);
            View.Property(p => p.CreateByName);
            View.Property(p => p.CreateDate);
            View.Property(p => p.UpdateByName);
            View.Property(p => p.UpdateDate);
        }

        ///<summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            // 配置明细视图
        }

        ///<summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            // 配置下拉视图
        }

    }
}
