using SIE.Common.Prints;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Inventory.Commom;
using SIE.Items;
using SIE.LES.LesStockCounts;
using SIE.LES.LesStockCounts.ViewModels;
using SIE.MetaModel.View;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Warehouses;
using SIE.Warehouses.ItemStockData;
using SIE.Web.ClientMetaModel;
using SIE.Web.Items.Common.DataQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CountDimension = SIE.LES.LesStockCounts.CountDimension;

namespace SIE.Web.LES.Extensions
{
    /// <summary>
    /// 视图扩展类
    /// </summary>
    public static class EntityPropertyViewMetaExtension
    {
        /// <summary>
        /// 工厂字段名
        /// </summary>
        private static string FactoryIdPropertyName = "FactoryId";

        /// <summary>
        ///部门编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseDepartmentEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                var workshop = RT.Service.Resolve<EnterpriseController>().GetResourceDepartments(pagingInfo, keyword);
                workshop.ForEach(p => p.TreePId = null);
                return workshop;
            }).UsePagingLookUpEditor(action);
            return meta;
        }

        /// <summary>
        /// 车间编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseResourceWorkShopEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                var workshop = RT.Service.Resolve<EnterpriseController>().GetResourceWorkShops(pagingInfo, keyword);
                workshop.ForEach(p => p.TreePId = null);
                return workshop;
            }).UsePagingLookUpEditor(action);
            return meta;
        }

        /// <summary>
        /// 获取工厂下的部门下拉编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseFactoryDepartmentsEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                EntityList<Enterprise> departments = new EntityList<Enterprise>();

                var factoryIdProperty = source.PropertyContainer.FindProperty(FactoryIdPropertyName);

                if (factoryIdProperty != null)
                {
                    var factoryIdObject = source.GetProperty(factoryIdProperty);
                    if (factoryIdObject != null)
                    {
                        var factoryId = factoryIdObject as double?;
                        departments = RT.Service.Resolve<EnterpriseController>().GetDepartments(pagingInfo, keyword, factoryId);
                    }
                }
                departments.ForEach(p =>
                {
                    p.TreePId = null;
                });
                return departments;
            }).UsePagingLookUpEditor(action);

            return meta;
        }

        /// <summary>
        /// 获取工厂下的车间下拉编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <param name="factoryIdPropertyName"></param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseFactoryWorkshopEditor<T>(this WebEntityPropertyViewMeta<T> meta,
            Action<PagingLookUpBaseConfig> action = null, string factoryIdPropertyName = "")
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                EntityList<Enterprise> departments = new EntityList<Enterprise>();

                if (!factoryIdPropertyName.IsNullOrEmpty())
                {
                    FactoryIdPropertyName = factoryIdPropertyName;
                }

                var factoryIdProperty = source.PropertyContainer.FindProperty(FactoryIdPropertyName);

                if (factoryIdProperty != null)
                {
                    var factoryIdObject = source.GetProperty(factoryIdProperty);
                    if (factoryIdObject != null)
                    {
                        var factoryId = factoryIdObject as double?;
                        var tmpList = RT.Service.Resolve<EnterpriseController>().GetWorkShopByFactoryId(factoryId, new List<Enterprise>());
                        if (keyword.IsNotEmpty())
                        {
                            // 将用户输入的 % 替换为正则中的 .* 以实现 SQL 中 % 的效果
                            string pattern = "^" + Regex.Escape(keyword).Replace("%", ".*") + "$";

                            // 使用正则表达式进行匹配
                            tmpList = tmpList.Where(p =>
                                Regex.IsMatch(p.Code, pattern, RegexOptions.IgnoreCase) ||
                                Regex.IsMatch(p.Name, pattern, RegexOptions.IgnoreCase)).ToList();
                        }
                        departments.AddRange(tmpList);
                        departments.ForEach(p =>
                        {
                            p.TreePId = null;
                        });
                    }
                }
                return departments;
            }).UsePagingLookUpEditor(action);

            return meta;
        }

        /// <summary>
        /// 获取车间下的产线下拉编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <param name="workShopIdPropertyName"></param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseWorkShopResourceEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null,
            string workShopIdPropertyName = "WorkShopId"
            )
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                EntityList<Enterprise> departments = new EntityList<Enterprise>();

                var workShopIdProperty = source.PropertyContainer.FindProperty(workShopIdPropertyName);

                if (workShopIdProperty != null)
                {
                    var workShopIdObject = source.GetProperty(workShopIdProperty);
                    if (workShopIdObject != null)
                    {
                        var workShopId = workShopIdObject as double?;
                        departments = RT.Service.Resolve<EnterpriseController>().GetEnterpriseByParentId(pagingInfo, keyword, workShopId, EnterpriseType.Line);
                        departments.ForEach(p =>
                        {
                            p.TreePId = null;
                        });
                    }
                }
                return departments;
            }).UsePagingLookUpEditor(action);
            return meta;
        }

        /// <summary>
        /// 根据车间获取生产资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="meta"></param>
        /// <param name="action"></param>
        /// <param name="workShopIdPropertyName"></param>
        /// <returns></returns>
        public static WebEntityPropertyViewMeta<T> UseWorkShopWipResourceEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null,
           string workShopIdPropertyName = "WorkShopId"
           )
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                EntityList<WipResource> departments = new EntityList<WipResource>();

                var workShopIdProperty = source.PropertyContainer.FindProperty(workShopIdPropertyName);

                if (workShopIdProperty != null)
                {
                    var workShopIdObject = source.GetProperty(workShopIdProperty);
                    if (workShopIdObject != null)
                    {
                        var workShopId = workShopIdObject as double?;
                        departments = RT.Service.Resolve<WipResourceController>().GetWipResources(new List<ResourceState>() { ResourceState.Actived },
                            workShopId, new List<SyncSourceType>() { SyncSourceType.Enterprise, SyncSourceType.Equipment }, pagingInfo, keyword);

                        departments.ForEach(p =>
                        {
                            p.TreePId = null;
                        });
                    }
                }
                return departments;
            }).UsePagingLookUpEditor(action);
            return meta;
        }

        /// <summary>
        /// 使用盘点物料编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseSCDetailItemEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig, T> action = null) where T : new()
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                LesStockCountDetail scDetail = source as LesStockCountDetail;
                var bill = RF.GetById<LesStockCount>(scDetail.LesStockCountId);
                var range = bill.LesStockCountRangeList.FirstOrDefault();
                if (string.IsNullOrEmpty(range.Items))
                {
                    if (string.IsNullOrEmpty(range.ItemCategorys))
                    {
                        List<ItemType> itemTypeList = new List<ItemType>() { ItemType.Material, ItemType.Product, ItemType.SemiFinished };
                        List<int> itemTypeValueList = itemTypeList.Select(p => (int)p).ToList();
                        return RT.Service.Resolve<ItemController>().GetItemsFormType(itemTypeValueList, State.Enable, string.Format("{0}%", keyword), pagingInfo);
                    }

                    var itemCategorys = range.ItemCategorys.Split(';').ToList();
                    return RT.Service.Resolve<ItemController>().GetItemByCategorys(itemCategorys, keyword, pagingInfo);
                }
                var list = range.Items.Split(';').ToList();
                return RT.Service.Resolve<ItemController>().GetItems(list, keyword, pagingInfo);
            }).UsePagingLookUpEditor(action);
            return meta;
        }

        /// <summary>
        /// 使用盘点仓库编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseSCDetailWarehouseEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                LesStockCountDetail scDetail = source as LesStockCountDetail;
                var bill = RF.GetById<LesStockCount>(scDetail.LesStockCountId);
                var range = bill.LesStockCountRangeList.FirstOrDefault();
                if (string.IsNullOrEmpty(range.Warehouses))
                {
                    return RT.Service.Resolve<WarehouseController>().GetAvailableWarehouses(pagingInfo, keyword);
                }
                else
                {
                    var list = range.Warehouses.Split(';').ToList();
                    return RT.Service.Resolve<WarehouseController>().GetUserWarehouses(list, keyword, pagingInfo);
                }
            }).UsePagingLookUpEditor(action);
            return meta;
        }

        /// <summary>
        /// 使用盘点库位编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseSCDetailLocationEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                LesStockCountDetail scDetail = source as LesStockCountDetail;
                if (scDetail.WarehouseId > 0)
                {
                    return RT.Service.Resolve<WarehouseController>().GetUnFrozenStorageLocations(scDetail.WarehouseId, keyword, pagingInfo);
                }
                return new EntityList<StorageLocation>();
            }).UsePagingLookUpEditor(action);
            return meta;
        }

        /// <summary>
        /// 使用盘点批次编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseSCDetailLotEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                LesStockCountDetail scDetail = source as LesStockCountDetail;
                if (scDetail == null || scDetail.ItemId <= 0)
                {
                    return new EntityList<Lot>();
                }
                else
                {
                    if (scDetail.EnableExtPro)
                    {
                        if (scDetail.ItemExtProp.IsNullOrEmpty())
                        {
                            return new EntityList<Lot>();
                        }
                        else
                        {
                            var isBatch = RT.Service.Resolve<ItemStockBaseController>().CheckItemIsBatch(scDetail.ItemId);
                            if (!isBatch)
                            {
                                return RT.Service.Resolve<LotController>().GetItemLots(scDetail.ItemId, keyword, pagingInfo);
                            }
                        }
                    }
                }
                return RT.Service.Resolve<LotController>().GetItemLots(scDetail.ItemId, keyword, pagingInfo, scDetail.ItemExtProp);
            }).UsePagingLookUpEditor(action);
            return meta;
        }

        /// <summary>
        /// 使用打印单据规则编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseBillPrintEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                if (source is LesStockCountPrintViewModel)
                {
                    string qualifiedName = typeof(LesStockCountPrintable).GetQualifiedName();
                    return RT.Service.Resolve<WarehouseController>().GetPrintTemplatesByType(qualifiedName, keyword, pagingInfo, PrintType.Bill);
                }
                return new EntityList<PrintTemplate>();
            }).UsePagingLookUpEditor(action);
            return meta;
        }
    }
}
