using SIE.Common.Catalogs;
using SIE.Common.Prints;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Resources.Employees;
using SIE.Warehouses;
using SIE.Warehouses.Configs;
using SIE.Warehouses.Printables;
using SIE.Web.ClientMetaModel;
using SIE.Web.Common.Editors;
using System;

namespace SIE.Web.Warehouses
{
    /// <summary>
    /// 实体属性视图元数据扩展
    /// </summary>
    public static class EntityPropertyViewMetaExtension
    {
        /// <summary>
        /// 使用仓库下拉查询编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseWarehouseLookUpEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig, T> action) where T : new()
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                if (source is StorageArea)
                {
                    var storagearea = source as StorageArea;
                    if (storagearea != null)
                    {
                        return RT.Service.Resolve<WarehouseController>().GetWarehouseDataList(storagearea.LibraryType, keyword, pagingInfo);
                    }
                }

                if (source is StorageLocation)
                {
                    var storageLocation = source as StorageLocation;
                    if (storageLocation != null)
                    {
                        return RT.Service.Resolve<WarehouseController>().GetWarehouseDataList(storageLocation.LibraryType, keyword, pagingInfo);
                    }
                }
                return new EntityList<Warehouse>();
            }).UsePagingLookUpEditor(action);
            return meta;
        }

        /// <summary>
        /// 使用仓库(可用，未冻结)下拉查询编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <param name="type">仓库类型</param>
        /// <param name="contianFrozen">包含冻结</param>
        /// <param name="containLineWareHouse">是否过滤线边仓</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseWarehouseEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null, LibraryType? type = null, bool contianFrozen = false, bool containLineWareHouse = false)
        {
            if (action != null)
                meta.UseDataSource((source, pagingInfo, keyword) =>
                {
                    if (source == null)
                        return new EntityList<Warehouse>();
                    var results = RT.Service.Resolve<WarehouseController>().GetWarehouseByEmployee(pagingInfo, keyword, type, contianFrozen);
                    return results;
                }).UsePagingLookUpEditor(action);
            else
                meta.UseDataSource((source, pagingInfo, keyword) =>
                {
                    if (source == null)
                        return new EntityList<Warehouse>();
                    var results = RT.Service.Resolve<WarehouseController>().GetWarehouseByEmployee(pagingInfo, keyword, type, contianFrozen);
                    return results;
                }).UsePagingLookUpEditor(p => p.DisplayField = "Name");
            return meta;
        }

        /// <summary>
        /// 使用仓库(可用，未冻结)下拉查询编辑器（默认只查询实体仓库）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <param name="type">仓库类型</param>
        /// <param name="contianFrozen">包含冻结</param>
        /// <param name="containLineWareHouse">是否过滤线边仓</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseWarehouseEditorWithOutLine<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null, LibraryType? type = null, bool contianFrozen = false, bool containLineWareHouse = true)
        {
            if (action != null)

                meta.UseDataSource((source, pagingInfo, keyword) =>
                {
                    if (source == null)
                        return new EntityList<Warehouse>();
                    var results = RT.Service.Resolve<WarehouseController>().GetWarehouseByEmployee(pagingInfo, keyword, type, contianFrozen);
                    return results;
                }).UsePagingLookUpEditor(action);
            else
                meta.UseDataSource((source, pagingInfo, keyword) =>
                {
                    if (source == null)
                        return new EntityList<Warehouse>();
                    var results = RT.Service.Resolve<WarehouseController>().GetWarehouseByEmployee(pagingInfo, keyword, type, contianFrozen);
                    return results;
                }).UsePagingLookUpEditor(p => { p.DisplayField = "Name"; });


            return meta;
        }

        /// <summary>
        /// 获取库存组织下的所有仓库（包括禁用、冻结）默认过滤掉线边仓
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <param name="state">仓库状态</param>
        /// <param name="type">仓库类型</param>
        /// <param name="contianFrozen">包含冻结</param>
        /// <param name="containLineWareHouse">是否过滤线边仓</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseAllWarehouseEditorWithOutLine<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null, State? state = null, LibraryType? type = null, bool contianFrozen = false, bool containLineWareHouse = true)
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                if (source == null)
                    return new EntityList<Warehouse>();
                var results = RT.Service.Resolve<WarehouseController>().GetAllWarehouses(pagingInfo, keyword, state, type, contianFrozen, containLineWareHouse);
                return results;
            }).UsePagingLookUpEditor(action);
            return meta;
        }

        /// <summary>
        /// 使用仓库（所有）下拉查询编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <param name="type">仓库类型</param>
        /// <param name="containLineWareHouse">是否过滤线边仓-默认不过滤</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseAllWarehouseEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null, LibraryType? type = null, bool containLineWareHouse = true)
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                if (source == null)
                    return new EntityList<Warehouse>();
                var results = RT.Service.Resolve<WarehouseController>().GetAllWarehouseByEmployee(pagingInfo, keyword, type, containLineWareHouse);
                return results;
            }).UsePagingLookUpEditor(action);
            return meta;
        }

        /// <summary>
        /// 使用库位下拉查询编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseStorageLocationLookUpEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                if (source is StorageAreaOperation)
                {
                    var storageareaOperation = source as StorageAreaOperation;
                    if (storageareaOperation != null)
                        return RT.Service.Resolve<WarehouseController>().GetStorageLocationDataList(storageareaOperation.StorageAreaId, keyword, pagingInfo);
                }
                return new EntityList<StorageAreaOperation>();
            }).UsePagingLookUpEditor(action);
            return meta;
        }

        /// <summary>
        /// 使用库区编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseStorageAreaEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                if (source is StorageLocation)
                {
                    var storageLocation = source as StorageLocation;
                    if (storageLocation != null)
                    {
                        return RT.Service.Resolve<WarehouseController>().GetStorageArea(storageLocation.LibraryType, storageLocation.WarehouseId, keyword, pagingInfo);
                    }
                }
                return new EntityList<StorageArea>();
            }).UsePagingLookUpEditor(action);
            return meta;
        }

        /// <summary>
        /// 使用打印编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UsePrintTemplateEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                string qualifiedName = typeof(StorageLocationPrintable).GetQualifiedName();
                return RT.Service.Resolve<WarehouseController>().GetPrintTemplates(qualifiedName, keyword, pagingInfo);
            }).UsePagingLookUpEditor(action);
            return meta;
        }

        /// <summary>
        /// 使用快码编辑器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="meta"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static WebEntityPropertyViewMeta<T> UseAddressTypeCatalogEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<CatalogConfig> action = null)
        {
            var config = new CatalogConfig();
            config.Editable = true;
            config.ValueField = Catalog.NameProperty.Name;
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 员工编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseEmployeeEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig, T> action = null) where T : new()
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<EmployeeController>().GetEmployees(pagingInfo, keyword);

            }).UsePagingLookUpEditor(action);
            return meta;
        }
    }
}
