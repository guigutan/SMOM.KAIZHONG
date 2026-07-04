using SIE.MetaModel.View;
using SIE.Web.MetaModelPortal.ClientMetaModel.UIBlock.Form;
using System;

namespace SIE.Web.AbnormalInfo.AbnormalMonitors._Extentions_
{
	/// <summary>
	/// 扩展静态类，用于重写编辑器
	/// </summary>
	public static class EntityPropertyViewMetaExtension
    {
        
        /// <summary>
        /// 动态编辑器
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="meta">视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>实体属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseDynamicTypeEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<TextButtonFieldConfig> action = null)
        {
            var config = new TextButtonFieldConfig();
            config.ExtendJsObj = "SIE.Web.AbnormalInfo.AbnormalMonitors.Editors.DynamicTypeEditor";
            action?.Invoke(config);
            meta.ViewMeta.Config = config;
            return meta;
        }      
    }
}
