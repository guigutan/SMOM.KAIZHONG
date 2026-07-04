using SIE.MetaModel.View;
using SIE.Web.Core.Configs;
using SIE.Web.Core.Editors;
using System;

namespace SIE.Web.Core
{
    /// <summary>
    /// 视图属性扩展类
    /// </summary>
    public static class EntityPropertyViewMetaExtension
    {
        /// <summary>
        /// 多分类枚举筛选编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseMultipleCagetoryEnumEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<MultipleCagetoryEnumConfig> action = null)
        {
            meta.ViewMeta.EditorName = WebEditorNames.Enum;
            var config = new MultipleCagetoryEnumConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 使用年月编辑器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="meta">实体属性视图元数据 <see cref="WebEntityPropertyViewMeta"/></param>
        /// <param name="action">枚举</param>
        /// <returns>实体属性元数据</returns>  
        public static WebEntityPropertyViewMeta<T> UseYearMonthEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<YearMonthConfig> action = null)
        {
            meta.ViewMeta.EditorName = "YearMonthEditor";
            var config = new YearMonthConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 使用年编辑器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="meta">实体属性视图元数据 <see cref="WebEntityPropertyViewMeta"/></param>
        /// <param name="action">委托</param>
        /// <returns>实体属性元数据</returns>  
        public static WebEntityPropertyViewMeta<T> UseYearEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<YearConfig> action = null)
        {
            meta.ViewMeta.EditorName = "YearEditor";
            var config = new YearConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 货币编辑器配置文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="meta"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static WebEntityPropertyViewMeta<T> UseCurrencyFieldEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<CurrencyConfig> action = null)
        {
            meta.ViewMeta.EditorName = "CurrencyFieldEditor";
            var config = new CurrencyConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 使用库存组织编辑器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="meta"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static WebEntityPropertyViewMeta<T> UseInvOrgEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<InvOrgConfig> action = null)
        {
            var config = new InvOrgConfig();
            config.Editable = true;
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 使用时间选择编辑器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="meta"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static WebEntityPropertyViewMeta<T> UseSelectTimeEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<SelectTimeConfig> action = null)
        {
            var config = new SelectTimeConfig();
            config.Editable = true;
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }
    }
}
