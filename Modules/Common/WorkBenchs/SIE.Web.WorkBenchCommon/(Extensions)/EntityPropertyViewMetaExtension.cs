using SIE.MetaModel.View;
using SIE.Web.ClientMetaModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.WorkBenchCommon._Extensions_
{
    /// <summary>
    /// 实体属性视图元数据扩展
    /// </summary>
    public static class EntityPropertyViewMetaExtension
    {
        /// <summary>
        /// 设置下拉编辑器
        /// </summary>
        /// <param name="meta">实体属性视图元数据 <see cref="WebEntityPropertyViewMeta"/></param>
        /// <param name="dataSource">数据源</param>
        /// <returns>实体属性视图元数据</returns>
        ///  <example> View.Property(p => p.Text).UseDropDownEditor();</example>
        public static WebEntityPropertyViewMeta<T> UseKpiDropDownEditor<T>(this WebEntityPropertyViewMeta<T> meta, Func<IEnumerable> dataSource)
        {
            var config = new ComboBoxConfig()
            {
                XType= "kpixcombobox",
                DisplayField = "Value",
                ValueField = "Key",
                Store = new StoreConfig()
                {
                    Fields = new string[] { "Key", "Value" },
                    Data = dataSource.Invoke()
                }
            };
            config.Editable = true;
            meta.ViewMeta.Config = config;
            return meta;
        }
    }
}
