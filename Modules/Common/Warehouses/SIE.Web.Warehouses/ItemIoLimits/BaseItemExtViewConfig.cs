using SIE.Items;
using SIE.Warehouses;

namespace SIE.Web.Warehouses
{
    /// <summary>
    /// WMS物料扩展视图
    /// </summary>
    public class BaseItemExtViewConfig : WebViewConfig<Item>
    {
        /// <summary>
        /// 历史经验库物料视图
        /// </summary>
        public const string BaseExperienceItemView = "BaseExperienceItemView";

    }
}
