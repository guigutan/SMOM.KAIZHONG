using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Warehouses;
using SIE.Web.Common.Commands;
using SIE.Web.Warehouses.Commands;
using System.Collections.Generic;

namespace SIE.Web.Warehouses
{
    /// <summary>
    /// 库区视图配置
    /// </summary>
    internal class StorageAreaViewConfig : WebViewConfig<StorageArea>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.HasDelegate(StorageArea.NameProperty);
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(StorageAreaAddCommand).FullName, WebCommandNames.Edit, typeof(StorageAreaDeleteCommand).FullName, WebCommandNames.Save);
            View.ReplaceCommands(EnableCommand.CommandName, typeof(StorageAreaEnableCommand).FullName);
            View.ReplaceCommands(DisableCommand.CommandName, typeof(StorageAreaDisableCommand).FullName);
            View.UseCommands(typeof(StorageAreaFrozenCommand).FullName);
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            View.Property(p => p.Code).Readonly(p => p.PersistenceStatus == PersistenceStatus.Modified)
                .UseListSetting(e => { e.HelpInfo = "根据库区编码规则(配置项--库区编码规则)生成库区编码,修改模式不可编辑"; });
            View.Property(p => p.Name);
            View.Property(p => p.LibraryType).Readonly(p => p.PersistenceStatus == PersistenceStatus.Modified).Cascade(p => p.Warehouse, null)
                .UseListSetting(e => { e.HelpInfo = "更改类型清空仓库数据，修改模式不可编辑"; });
            View.Property(p => p.WarehouseId).UseDataSource((e, c, r) =>
            {
                return RT.Service.Resolve<WarehouseController>().GetWarehouseByEmployee(c, r);
            }).Readonly(p => p.PersistenceStatus == PersistenceStatus.Modified)
                .UseWarehouseLookUpEditor((p,r) => 
                { 
                    p.ReloadDataOnPopping = true;
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(r.WarehouseCode), nameof(r.Warehouse.Code));
                    p.DicLinkField = keyValues;
                }).HasLabel("仓库名称")
                .UseListSetting(e => { e.HelpInfo = "显示库区类型的仓库，修改模式不可编辑"; });
            View.Property(p => p.WarehouseCode).Readonly();
            View.Property(p => p.IsFrozen).Readonly();
            View.Property(p => p.State).Readonly();
            View.Property(p => p.IsAutomatedArea).Readonly(p => p.PersistenceStatus == PersistenceStatus.Modified);
            View.Property(p => p.GroundingType);          
            View.Property(p => p.IsAllowManualGrounding);
            View.Property(p => p.UpdateDate);
            View.AssociateChildrenProperty(StorageAreaInfoDetailProperty.StorageAreaInfoProperty, c =>
            {
                var storageArea = c.Parent as StorageArea;
                var storagearealist = RT.Service.Resolve<WarehouseController>().GetStorageAreaInfoDetail(storageArea.Id);
                return storagearealist == null ? new StorageAreaInfo() { StorageArea = storageArea, StorageAreaId = storageArea.Id } : storagearealist;
            }, ViewConfig.DetailsView).HasLabel("基本资料").OrderNo = 1;
            View.AssociateChildrenProperty(StorageAreaOperationDetailProperty.StorageAreaOperationProperty, c =>
            {
                var storageArea = c.Parent as StorageArea;
                var storagearealist = RT.Service.Resolve<WarehouseController>().GetStorageAreaOperationDetail(storageArea.Id);
                return storagearealist == null ? new StorageAreaOperation() { StorageArea = storageArea, StorageAreaId = storageArea.Id } : storagearealist;
            }, ViewConfig.DetailsView).HasLabel("操作管理").OrderNo = 2;

            View.AssociateChildrenProperty(StorageAreaWcsDetailProperty.StorageAreaWcsProperty, c =>
            {
                var storageArea = c.Parent as StorageArea;
                var storagearealist = RT.Service.Resolve<WarehouseController>().GetStorageAreaWcs(storageArea.Id);
                return storagearealist == null ? new StorageAreaWcs() { StorageArea = storageArea, StorageAreaId = storageArea.Id } : storagearealist;
            }, ViewConfig.DetailsView).HasLabel("立库配置").OrderNo = 3;
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code).Show();
            View.Property(p => p.Name).Show();
            View.Property(p => p.LibraryType).Show();
            View.Property(p => p.WarehouseCode);
            View.Property(p => p.WarehouseId).Show();
            View.Property(p => p.IsFrozen).Show();
        }
    }
}
