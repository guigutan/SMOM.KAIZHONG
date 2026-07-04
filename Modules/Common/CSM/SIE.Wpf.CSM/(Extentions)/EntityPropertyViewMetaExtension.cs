using SIE.MetaModel.View;
using SIE.Wpf.CSM.Edtors;
using System;

namespace SIE.Wpf.CSM
{
    /// <summary>
    /// 实体属性视图元数据扩展
    /// </summary>
    public static class EntityPropertyViewMetaExtension
    {
        /// <summary>
        /// 使用区域信息编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseRegionalEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<RegionalInfoEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = RegionalInfoEditor.EditorName;
            var config = new RegionalInfoEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 使用客户信息编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseCustomerEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<CustomerLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = CustomerLookupEditor.EditorName;
            var config = new CustomerLookUpEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }
    }
}
