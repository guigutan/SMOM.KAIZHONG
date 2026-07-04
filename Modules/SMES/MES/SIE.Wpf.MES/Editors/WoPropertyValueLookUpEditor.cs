using SIE.Domain;
using SIE.Items;
using SIE.Items.ViewModels;
using SIE.ManagedProperty;
using SIE.Wpf.Editors;
using System.Linq;

namespace SIE.Wpf.MES.Editors
{
    /// <summary>
    /// 工单属性类型下拉选择器
    /// </summary>
    public class ItemPropertyDefinitionLookUpEditor : PagingLookUpEditor
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public const string EditorName = "ItemPropertyDefinitionLookUpEditor";

        /// <summary>
        /// 获取数据源
        /// </summary>
        /// <param name="source">属性值ViewModel</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">关键字</param>
        /// <param name="titleProperty">标题属性</param>
        /// <returns>物料属性定义列表</returns>
        protected override EntityList GetDataSourceCore(Entity source, PagingInfo pagingInfo, string keyword, IManagedProperty titleProperty)
        {
            EntityList<ItemPropertyDefinition> definitions = new EntityList<ItemPropertyDefinition>();
            var propertyValue = source as PropertyValueViewModel;
            if (propertyValue.ItemId <= 0)
                return definitions;
            var result = RT.Service.Resolve<ItemController>().GetItemPropertys(propertyValue.ItemId).Select(p => p.Definition).Distinct((x, y) => x.Name == y.Name);
            definitions.AddRange(result);
            return definitions;
        }
    }
}