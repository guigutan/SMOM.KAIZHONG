using SIE.Domain;
using SIE.LES.Distributions;
using SIE.MetaModel.View;
using SIE.Resources.WipResources;
using SIE.Warehouses;
using SIE.Web.LES.Distributions.Commands;
using System;
using System.Collections.Generic;

namespace SIE.Web.LES.Distributions
{
    /// <summary>
    /// 配送设置
    /// </summary>
    public class DistributionSettingViewConfig : WebViewConfig<DistributionSetting>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(Distribution));
        }

        /// <summary>
        /// 视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.ReplaceCommands(WebCommandNames.Edit, typeof(EditDistributionSettingCommand).FullName);
            View.ReplaceCommands(WebCommandNames.Delete, typeof(DistributionSettingDeleteCommand).FullName);
            View.InlineEdit();
            using (View.OrderProperties())
            {
                View.Property(p => p.ProductLineId).UseDataSource((e, c, r) =>
                {
                    var stateList = new List<ResourceState>() { ResourceState.Actived, ResourceState.Stop, ResourceState.Unused };
                    var sourceType = new List<SyncSourceType>() { SyncSourceType.Enterprise, SyncSourceType.Equipment };
                    return RT.Service.Resolve<WipResourceController>().GetWipResources(stateList, null, sourceType, c, r);
                }).UsePagingLookUpEditor(p => p.DisplayField = "Name")
                .Readonly(p => p.State == State.Enable && p.CreateBy > 0).UseListSetting(p => p.HelpInfo = "禁用才可编辑".L10N()).ShowInList();
                View.Property(p => p.WarehouseId).UseDataSource((e, c, r) =>
                {
                    return RT.Service.Resolve<WarehouseController>().GetAllWarehouseByEmployee(c, r, LibraryType.Entity, true);
                }).Cascade(p => p.AreaId, null).Cascade(p => p.StorageLocationId, null).Readonly(p => p.State == State.Enable && p.CreateBy > 0);
                View.Property(p => p.AreaId).Readonly(p => p.State == State.Enable && p.CreateBy > 0 || p.WarehouseId == null).
                    UseDataSource((source, pagingInfo, keyword) =>
                    {
                        var storageLocation = source as DistributionSetting;
                        if (storageLocation != null)
                        {
                            return RT.Service.Resolve<WarehouseController>().GetStorageArea(null, storageLocation.WarehouseId.Value, keyword, pagingInfo);
                        }
                        return new EntityList<StorageArea>();
                    }).Cascade(p => p.StorageLocationId, null);
                View.Property(p => p.StorageLocationId).UseListSetting(p => p.HelpInfo = "发货暂存库位".L10N()).Readonly(p => p.State == State.Enable && p.CreateBy > 0 || p.AreaId == null).
               UseDataSource((source, pagingInfo, keyword) =>
               {
                   var storageLocation = source as DistributionSetting;
                   if (storageLocation != null)
                   {
                       return RT.Service.Resolve<WarehouseController>().GetStorageLocationDataListIsFocus(storageLocation.AreaId.Value, keyword, pagingInfo);
                   }
                   return new EntityList<StorageLocation>();
               });
                View.Property(p => p.State).Readonly();
                View.Property(p => p.CreateByName).Readonly();
                View.Property(p => p.CreateDate).Readonly();
                View.Property(p => p.UpdateByName).Readonly();
                View.Property(p => p.UpdateDate).Readonly();
            }
        }
    }
}
