using SIE.MetaModel.View;
using SIE.Wpf.Editors;
using SIE.Wpf.Resources.CalendarSchemes.Editors;
using SIE.Wpf.Resources.Editors;
using System;

namespace SIE.Wpf.Resources
{
    /// <summary>
    /// 编辑器扩展类
    /// </summary>
    public static class EntityPropertyViewMetaExtension
    {
        /// <summary>
        /// 资源产线下拉编辑器扩展方法
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseLineEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = LineLookUpEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 资源车间下拉编辑器扩展方法
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseShopEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = ShopLookUpEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 资源部门下拉编辑器扩展方法
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseDepartmentpEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = DepartmentpLookUpEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 企业模型下拉编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">wpf实体属性视图元数据（参数）</param>
        /// <param name="action">委托</param>
        /// <returns>wpf实体属性视图元数据（返回）</returns>
        public static WPFEntityPropertyViewMeta<T> UseEnterpriseEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = EnterpriseEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 资源下拉编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">wpf实体属性视图元数据（参数）</param>
        /// <param name="action">委托</param>
        /// <returns>wpf实体属性视图元数据（返回）</returns>
        public static WPFEntityPropertyViewMeta<T> UseResourceEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = ResourceEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 区域下拉编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">wpf实体属性视图元数据（参数）</param>
        /// <param name="action">委托</param>
        /// <returns>wpf实体属性视图元数据（返回）</returns>
        public static WPFEntityPropertyViewMeta<T> UseAreaEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = AreaLookUpEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 生产资源车间编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">wpf实体属性视图元数据（参数）</param>
        /// <param name="action">委托</param>
        /// <returns>wpf实体属性视图元数据（返回）</returns>
        public static WPFEntityPropertyViewMeta<T> UseResourceWorkShopEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = ResourceWorkShopEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 客制化编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">wpf实体属性视图元数据（参数）</param>
        /// <param name="action">委托</param>
        /// <returns>wpf实体属性视图元数据（返回）</returns>
        public static WPFEntityPropertyViewMeta<T> UseCustomResourceEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = CustomResourceEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 设备编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">wpf实体属性视图元数据（参数）</param>
        /// <param name="action">委托</param>
        /// <returns>wpf实体属性视图元数据（返回）</returns>
        public static WPFEntityPropertyViewMeta<T> UseEquipmentResourceEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = EquipmentResourceEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 企业模型资源编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">wpf实体属性视图元数据（参数）</param>
        /// <param name="action">委托</param>
        /// <returns>wpf实体属性视图元数据（返回）</returns>
        public static WPFEntityPropertyViewMeta<T> UseEnterpriseResourceEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = EnterpriseResourceEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 生产资源编辑器：企业模型、设备台账
        /// 排除自定义类型的生产资源
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">wpf实体属性视图元数据（参数）</param>
        /// <param name="action">委托</param>
        /// <returns>wpf实体属性视图元数据（返回）</returns>
        public static WPFEntityPropertyViewMeta<T> UseEnterpriseEquipmentResourceEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = EnterpriseEquipmentResourceEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 生产资源编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">wpf实体属性视图元数据（参数）</param>
        /// <param name="action">委托</param>
        /// <returns>wpf实体属性视图元数据（返回）</returns>
        public static WPFEntityPropertyViewMeta<T> UseWipResourceEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = WipResourceEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 设置可筛选的枚举编辑器
        /// </summary>
        /// <returns></returns>
        public static WPFEntityPropertyViewMeta<T> UseCustomEnumEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<CustomEnumEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = CustomEnumEditor.EditorName;
            var config = new CustomEnumEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 可用的日历方案编辑器扩展方法
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseSchemeLookUpEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<LookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = SchemeLookUpEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 加载生产资源编码列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseWipResourceCodeLookUpEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<LookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = WipResourceCodeLookUpEditor.EditorName;
            var config = new LookUpEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 资源工厂下拉编辑器扩展方法
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseFactoryEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = FactoryLookUpEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }
    }
}
