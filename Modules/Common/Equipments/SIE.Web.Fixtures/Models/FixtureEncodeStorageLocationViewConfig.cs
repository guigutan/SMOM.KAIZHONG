using SIE.Domain;
using SIE.Fixtures;
using SIE.Fixtures.Models;
using SIE.MetaModel.View;
using SIE.Warehouses;
using System.Collections.Generic;

namespace SIE.Web.Fixtures.Models
{
    /// <summary>
    /// 工治具编码（存储位置）视图配置
    /// </summary>
    internal class FixtureEncodeStorageLocationViewConfig : WebViewConfig<FixtureEncodeStorageLocation>
    {
        /// <summary>
        /// 显示宽度
        /// </summary>
        private const int displayCoulmnWidth = 20;

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands().RemoveCommands(WebCommandNames.Save);
            View.Property(p => p.WarehouseId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<WarehouseController>().GetWarehouses(pagingInfo, keyword);
            }).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.FixtureWarehouseName), nameof(e.Warehouse.Name));
                m.DicLinkField = keyValues;
            }).Cascade(p => p.StorageLocationId, null).Cascade(p => p.FixtureStorageLocationName, null).HasLabel("仓库编码").ShowInList(displayCoulmnWidth * 6);
            View.Property(p => p.FixtureWarehouseName).Readonly().ShowInList(displayCoulmnWidth * 6);
            View.Property(p => p.StorageLocationId).UseDataSource((o, e, r) =>
            {
                var location = o as FixtureEncodeStorageLocation;
                if (location == null || location?.Warehouse == null)
                    return new EntityList<StorageLocation>();
                return RT.Service.Resolve<CoreFixtureController>().GetFixtureStorageLocations(location.WarehouseId, e, r);
            }).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.FixtureStorageLocationName), nameof(e.StorageLocation.Name));
                m.DicLinkField = keyValues;
            }).HasLabel("库位编码").Readonly(p => p.FixtureWarehouseName == string.Empty).ShowInList(displayCoulmnWidth * 6);
            View.Property(p => p.FixtureStorageLocationName).Readonly().ShowInList(displayCoulmnWidth * 6);
        }
    }
}