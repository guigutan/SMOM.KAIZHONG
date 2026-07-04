using SIE.MetaModel.View;
using SIE.Wpf.Editors;
using SIE.Wpf.Items.Editors;
using SIE.Wpf.Warehouses.Editors;
using System;

namespace SIE.Wpf.Warehouses
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
        public static WPFEntityPropertyViewMeta<T> UseWarehouseLookUpEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = WarehouseLookUpEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 使用仓库（可用、非冻结状态）下拉查询编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseAvailableWarehouseEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = AvailableWarehouseEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            config.ReloadDataOnPopping = true;
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 使用RoHS等级编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseRoHsGradeEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<CustomInputEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = RoHsGradeEditor.EditorName;
            var config = new CustomInputEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 使用存储温度编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseStorageTemperatureEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<CustomInputEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = StorageTemperatureEditor.EditorName;
            var config = new CustomInputEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 使用存储湿度编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseStorageHumidityEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<CustomInputEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = StorageHumidityEditor.EditorName;
            var config = new CustomInputEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 使用库区编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseStorageAreaEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = StorageAreaEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 使用打印编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UsePrintTemplateEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = PrintTemplateEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 使用库位下拉查询编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseAreaLocationLookUpEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = AreaLocationLookUpEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }
    }
}
