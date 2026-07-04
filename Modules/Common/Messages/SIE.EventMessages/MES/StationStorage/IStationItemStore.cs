using SIE.Services;

namespace SIE.EventMessages.StationStorage
{
    /// <summary>
    /// 工位物料库存接口
    /// </summary>
    [Service(FallbackType = typeof(DefaultStationItemStore))]
    public interface IStationItemStore
    {
        /// <summary>
        /// 更新工位物料库存
        /// </summary>
        /// <param name="storeEvent">工位物料库存事件</param>
        void UpdateStationStorage(StationItemStoreEvent storeEvent);
    }

    /// <summary>
    /// 工位物料库存接口默认实现
    /// </summary>
    public class DefaultStationItemStore : IStationItemStore
    {
        /// <summary>
        /// 更新工位物料库存
        /// </summary>
        /// <param name="storeEvent">工位物料库存事件</param>
        public void UpdateStationStorage(StationItemStoreEvent storeEvent)
        {
            // 更新工位物料库存
        }
    }
}