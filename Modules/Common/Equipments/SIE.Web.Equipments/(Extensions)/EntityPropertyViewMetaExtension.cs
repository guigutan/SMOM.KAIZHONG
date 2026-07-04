using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Resources.Enterprises;
using SIE.Web.ClientMetaModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SIE.Web.Equipments.Extensions
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
            string workShopIdPropertyName= "WorkShopId"
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
    }

}
