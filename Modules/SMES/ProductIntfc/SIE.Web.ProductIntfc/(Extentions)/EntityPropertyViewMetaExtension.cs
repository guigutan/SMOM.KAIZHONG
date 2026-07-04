using SIE.Domain;
using SIE.MetaModel.View;
using SIE.ProductIntfc.ProductStorages;
using SIE.Resources.Enterprises;
using SIE.Web.ClientMetaModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.ProductIntfc._Extentions_
{


    /// <summary>
    /// 扩展属性
    /// </summary>
    public static class EntityPropertyViewMetaExtension
    {
        /// <summary>
        /// 工厂字段名
        /// </summary>
        private static string FactoryIdPropertyName = "FactoryId";
        /// <summary>
        /// 资源车间下拉编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseShopLookUpEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                var enterpriseList = RT.Service.Resolve<EnterpriseController>().GetEnterprises(EnterpriseType.Shop, pagingInfo, keyword);
                if (enterpriseList == null || enterpriseList.Count <= 0)
                    return new EntityList<Enterprise>();
                for (var i = 0; i < enterpriseList.Count; i++)
                {
                    enterpriseList[i].TreePId = null;
                }

                return enterpriseList;
            }).UsePagingLookUpEditor(action);
            return meta;
        }

        /// <summary>
        /// 物料编辑器名称
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseStorageParamLookUpEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<ProductStorageController>().GetItemIsProduct(keyword, pagingInfo);
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
                            tmpList = tmpList.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword)).ToList();
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
    }
}
