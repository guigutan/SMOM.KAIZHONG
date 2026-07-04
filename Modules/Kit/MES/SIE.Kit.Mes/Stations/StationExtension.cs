using SIE.Domain;
using SIE.ManagedProperty;
using SIE.Tech.Stations;

namespace SIE.Kit.MES.Stations
{
    /// <summary>
    /// 
    /// </summary>
    [CompiledPropertyDeclarer]
    public static class StationExtension
    {
        #region 工位物料清单
        /// <summary>
        /// 工位物料清单
        /// </summary>
        public static readonly ListProperty<EntityList<StationItem>> StationItemListProperty =
            P<Station>.RegisterExtensionList<EntityList<StationItem>>("StationItemList", typeof(StationExtension));

        /// <summary>
        /// 获取工位物料清单
        /// </summary>
        /// <param name="me">工位</param>
        /// <returns>工位物料清单</returns>
        public static EntityList<StationItem> GetStationItemList(this Station me)
        {
            return me.GetProperty(StationItemListProperty);
        }

        /// <summary>
        /// 设置工位物料清单
        /// </summary>
        /// <param name="me">工位</param>
        /// <param name="value">工位物料清单</param>
        public static void SetStationItemList(this Station me, EntityList<StationItem> value)
        {
            me.SetProperty(StationItemListProperty, value);
        }
        #endregion
 
    }
}
