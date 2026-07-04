using SIE.Domain;
using SIE.Items;
using SIE.ManagedProperty;
using SIE.Wpf.Editors;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Wpf.Items.Editors
{
    /// <summary>
    /// 物料类型编辑器
    /// </summary>
    public class ItemTypeEditor : PagingLookUpEditor
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public const string EditorName = "ItemTypeEditor";

        /// <summary>
        /// 需要显示那些类型的物料(传入ItemType列表参数)
        /// </summary>
        public const string TypeList = "TypeList";

        /// <summary>
        /// 物料状态过滤
        /// </summary>
        public const string State = "State";

        /// <summary>
        /// 获取物料分类小类
        /// </summary>
        /// <param name="source">上下文</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">关键字</param>
        /// <param name="titleProperty">显示属性</param>
        /// <returns>物料分类小类列表</returns>
        protected override EntityList GetDataSourceCore(Entity source, PagingInfo pagingInfo, string keyword, IManagedProperty titleProperty)
        {
            List<ItemType> itemTypeList = Config.GetPropertyOrDefault<List<ItemType>>(TypeList, new List<ItemType>());
            List<int> itemTypeValueList = itemTypeList.Select(p => (int)p).ToList();
            State? state = Config.GetPropertyOrDefault<State?>(State, null);
            return RT.Service.Resolve<ItemController>().GetItemsFormType(itemTypeValueList, state, string.Format("{0}%", keyword), pagingInfo);
        }
    }
}
