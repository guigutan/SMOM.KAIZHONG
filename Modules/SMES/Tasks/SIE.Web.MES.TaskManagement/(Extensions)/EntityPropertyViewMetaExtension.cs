using SIE.Common.Prints;
using SIE.Domain;
using SIE.MES.TaskManagement.Configs;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Dispatchs.ViewModels;
using SIE.MetaModel.View;
using SIE.Resources.Enterprises;
using SIE.Web.ClientMetaModel;
using System;
using System.Linq;

namespace SIE.Web.MES.TaskManagement
{
    /// <summary>
    /// 视图扩展类
    /// </summary>
    public static class EntityPropertyViewMetaExtension
    {
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
        /// 使用打印单据规则编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseDispatchTaskBillEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                if (source is DispatchTaskConfigValue)
                {
                    DispatchTaskConfigValue taskConfigValue = source as DispatchTaskConfigValue;
                    string qualifiedName = typeof(DispatchTaskBillPrintable).GetQualifiedName();
                    if (taskConfigValue != null && taskConfigValue.NumberRuleId.HasValue)
                    {
                        return RT.Service.Resolve<DispatchController>().GetPrintTemplates(taskConfigValue.NumberRuleId.Value, qualifiedName, keyword, pagingInfo);
                    }
                }

                if (source is DispatchTaskPrintViewModel)
                {
                    DispatchTaskConfigValue taskConfigValue = RT.Service.Resolve<DispatchController>().GetDispatchTaskConfigValue();
                    string qualifiedName = typeof(DispatchTaskBillPrintable).GetQualifiedName();
                    if (taskConfigValue != null && taskConfigValue.NumberRuleId.HasValue)
                    {
                        return RT.Service.Resolve<DispatchController>().GetPrintTemplates(taskConfigValue.NumberRuleId.Value, qualifiedName, keyword, pagingInfo);
                    }
                }

                return new EntityList<PrintTemplate>();
            }).UsePagingLookUpEditor(action);
            return meta;
        }
    }
}