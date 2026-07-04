using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Warehouses;
using SIE.Web.Warehouses.Commands;

namespace SIE.Web.Warehouses
{
    /// <summary>
    /// 逻辑分区视图配置
    /// </summary>
    internal class LogicAreaViewConfig : WebViewConfig<LogicArea>
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
            View.UseCommands(typeof(LogicAreaAddCommand).FullName, WebCommandNames.Edit, typeof(LogicAreaDeleteCommand).FullName, WebCommandNames.Save);
            View.UseCommands(typeof(LogicAreaImportCommand).FullName);
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            View.Property(p => p.Code).Readonly(p => p.PersistenceStatus == PersistenceStatus.Modified);
            View.Property(p => p.Name);
            View.Property(p => p.IsAutomatedArea).Readonly();
            View.Property(p => p.Description);
            View.Property(p => p.WarehouseId).UseWarehouseEditor(contianFrozen: true).Readonly(p => p.PersistenceStatus == PersistenceStatus.Modified);
            View.Property(p => p.State).Readonly();
            View.Property(p => p.CreateByName);
            View.Property(p => p.CreateDate);
            View.Property(p => p.UpdateByName);
            View.Property(p => p.UpdateDate);
            View.AttachChildrenProperty(typeof(LogicAreaLocation), (w) =>
            {
                var arg = w as ChildPagingDataArgs;
                var entity = arg.Parent as LogicArea;
                if (entity == null)
                    return new EntityList<LogicAreaLocation>();
                return RT.Service.Resolve<WarehouseController>().GetLogicAreaLocations(entity.Id, arg.PagingInfo);
            }, ListView, true).Show(ChildShowInWhere.All);
        }

        /// <summary>
        /// 导入视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.Property(p => p.Code).Show();
            View.Property(p => p.Name).Show();
            View.Property(p => p.IsAutomatedArea).Show();
            View.Property(p => p.WarehouseCode).Show();
            View.Property(p => p.Description).Show();
            View.PropertyRef(p => p.WarehouseId).Show().HasLabel("库位编码");
        }

        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code).Show();
            View.Property(p => p.Name).Show();
            View.Property(p => p.WarehouseId).Show();
            View.Property(p => p.IsAutomatedArea).Show();
        }

        /// <summary>
        /// 选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code).Readonly(p => p.PersistenceStatus == PersistenceStatus.Modified);
            View.Property(p => p.Name);
            View.Property(p => p.Description);
            View.Property(p => p.State).Readonly();
            View.Property(p => p.IsAutomatedArea).Readonly();
        }
    }
}
