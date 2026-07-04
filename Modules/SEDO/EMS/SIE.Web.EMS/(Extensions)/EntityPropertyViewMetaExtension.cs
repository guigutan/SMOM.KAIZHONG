using SIE.Domain;
using SIE.EMS.Common.Entity;
using SIE.EMS.DevicePurs;
using SIE.EMS.Equipments;
using SIE.EMS.MainenanceProjects;
using SIE.MetaModel.View;
using SIE.Resources.Enterprises;
using SIE.Web.ClientMetaModel;
using System;

namespace SIE.Web.EMS.Extensions
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
        /// 需校验设备台账编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseCalibrationEquipAccountEditor<T>(this WebEntityPropertyViewMeta<T> meta,
            Action<PagingLookUpBaseConfig> action = null)
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                var accounts = RT.Service.Resolve<EquipController>().GetCalibrationEquipAccounts(pagingInfo, keyword);
                return accounts;
            }).UsePagingLookUpEditor(action);
            return meta;
        }

        /// <summary>
        /// 用户预算部门下拉编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <param name="factoryIdPropertyName">工厂字段</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseUserBudgetDepartmentEditor<T>(this WebEntityPropertyViewMeta<T> meta,
            Action<PagingLookUpBaseConfig> action = null,string factoryIdPropertyName="")
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                EntityList<Enterprise> departments = new EntityList<Enterprise>();

                if(!factoryIdPropertyName.IsNullOrEmpty())//如传参后 则使用外部工厂参数
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

                        departments = RT.Service.Resolve<DevicePurController>()
                            .GetUserBudgetDepartments(RT.Identity.UserId, pagingInfo, keyword, factoryId);

                        for (var i = 0; i < departments.Count; i++)
                        {
                            departments[i].TreePId = null;
                        }
                    }
                }
                else
                {
                    departments = RT.Service.Resolve<DevicePurController>()
                        .GetUserBudgetDepartments(RT.Identity.UserId, pagingInfo, keyword);

                    for (var i = 0; i < departments.Count; i++)
                    {
                        departments[i].TreePId = null;
                    }
                }

                return departments;
            }).UsePagingLookUpEditor(action);

            return meta;
        }

        /// <summary>
        /// 用户预算部门下拉编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <param name="factoryIdPropertyName">工厂Id字段名</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseUserBussinessDepartmentEditor<T>(this WebEntityPropertyViewMeta<T> meta,
            Action<PagingLookUpBaseConfig> action = null,string factoryIdPropertyName="")
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

                        departments = RT.Service.Resolve<DevicePurController>()
                            .GetUserBussinessDepartments(RT.Identity.UserId, pagingInfo, keyword, factoryId);

                        for (var i = 0; i < departments.Count; i++)
                        {
                            departments[i].TreePId = null;
                        }
                    }
                }
                else
                {
                    departments = RT.Service.Resolve<DevicePurController>()
                        .GetUserBussinessDepartments(RT.Identity.UserId, pagingInfo, keyword);

                    for (var i = 0; i < departments.Count; i++)
                    {
                        departments[i].TreePId = null;
                    }
                }

                return departments;

            }).UsePagingLookUpEditor(action);

            return meta;
        }

        /// <summary>
        /// 获取周期类型下拉编辑器
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="meta">meta</param>
        /// <param name="action">action</param>
        /// <returns>列表</returns>
        public static WebEntityPropertyViewMeta<T> UseCycleTypeEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            meta.ViewMeta.SelectionViewMeta = new SelectionViewMeta();
            meta.ViewMeta.SelectionViewMeta.SelectionEntityType = typeof(CycleTypeInfo);
            meta.ViewMeta.SelectionViewMeta.DisplayMemberPath = CycleTypeInfo.ValueProperty;
            meta.ViewMeta.SelectionViewMeta.SelectedValuePath = CycleTypeInfo.IdProperty;
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                var sourceList = new EntityList<CycleTypeInfo>();
                dynamic regional = source as ProjectDetail;

                if (regional.ProjectType != null)
                {
                    ProjectType upperLevel = regional.ProjectType;
                    sourceList = RT.Service.Resolve<ProjectDetailController>().GetCycleTypeInfoList(upperLevel, pagingInfo, keyword);
                }
                return sourceList;
            }).UsePagingLookUpEditor(action);

            return meta;
        }
    }
}
