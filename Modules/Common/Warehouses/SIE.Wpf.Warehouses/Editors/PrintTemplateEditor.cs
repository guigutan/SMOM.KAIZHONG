using SIE.Domain;
using SIE.ManagedProperty;
using SIE.Warehouses;
using SIE.Warehouses.Configs;
using SIE.Wpf.Editors;
using SIE.Wpf.Warehouses.Printables;
using System;

namespace SIE.Wpf.Warehouses.Editors
{
    /// <summary>
    /// 打印模板编辑器
    /// </summary>
    public class PrintTemplateEditor : PagingLookUpEditor
    {
        /// <summary>
        /// 打印模板编辑器名称
        /// </summary>
        public const string EditorName = "PrintTemplateEditor";

        /// <summary>
        /// 设置数据源
        /// </summary>
        /// <param name="source">实体</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">关键词</param>
        /// <param name="titleProperty">托管属性</param>
        /// <returns>EntityList</returns>
        protected override EntityList GetDataSourceCore(Entity source, PagingInfo pagingInfo, string keyword, IManagedProperty titleProperty)
        {
            var configValue = source as StorageLocationCodeConfigValue;
            string qualifiedName = typeof(StorageLocationPrintable).GetQualifiedName();
            return RT.Service.Resolve<WarehouseController>().GetPrintTemplates(qualifiedName, keyword, pagingInfo);
        }
    }
}
