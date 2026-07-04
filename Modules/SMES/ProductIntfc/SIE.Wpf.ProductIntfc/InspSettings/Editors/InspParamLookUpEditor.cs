using SIE.Domain;
using SIE.Items;
using SIE.ManagedProperty;
using SIE.Wpf.Editors;

namespace SIE.Wpf.ProductIntfc.InspSettings.Editors
{
    /// <summary>
    /// 报检参数物料下拉编辑器
    /// </summary>
    public class InspParamLookUpEditor : PagingLookUpEditor
    {
        /// <summary>
        /// 物料编辑器名称
        /// </summary>
        public const string EditorName = "InspParamLookUpEditor";

        /// <summary>
        /// 重新加载数据源
        /// </summary>
        /// <param name="source">对象</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        /// <param name="titleProperty">托管属性标记</param>
        /// <returns>只含成品和半成品物料的列表</returns>
        protected override EntityList GetDataSourceCore(Entity source, PagingInfo pagingInfo, string keyword, IManagedProperty titleProperty)
        {
            return RT.Service.Resolve<ItemController>().GetItemNotBatch(keyword, pagingInfo);
        }
    }
}