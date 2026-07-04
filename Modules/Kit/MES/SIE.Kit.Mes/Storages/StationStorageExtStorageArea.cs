using SIE.Domain;
using SIE.ManagedProperty;
using SIE.MetaModel;

namespace SIE.Kit.MES.Storages
{
    /// <summary>
    /// 工位货区扩展属性
    /// </summary>
    [CompiledPropertyDeclarer]
    public class StationStorageExtStorageArea
    {
        #region StationStorageArea StationStorageAreaExtList (产线工位货区)
        /// <summary>
        /// 产线工位货区 扩展属性。
        /// </summary>
        public static ListProperty<EntityList<StationStorageArea>> StationStorageAreaExtListProperty { get; } =
            P<StorageArea>.RegisterExtensionList<EntityList<StationStorageArea>>("StationStorageAreaExtList", typeof(StationStorageExtStorageArea));

        /// <summary>
        /// 获取 产线工位货区 属性的值。
        /// </summary>
        /// <param name="me">要获取扩展属性值的对象</param>
        /// <returns>产线工位货区</returns>
        public static EntityList<StationStorageArea> GetStationStorageAreaExt(StorageArea me)
        {
            return me.GetProperty(StationStorageAreaExtListProperty);
        }

        /// <summary>
        /// 设置产线工位货区
        /// </summary>
        /// <param name="me">工位货区</param>
        /// <param name="value">产线工位货区</param>
        public static void SetStationStorageAreaExt(StorageArea me, EntityList<StationStorageArea> value)
        {
            me.SetProperty(StationStorageAreaExtListProperty, value);
        }
        #endregion

        /// <summary>
        /// 工位货区 实体配置
        /// </summary>
        internal class StationStorageExtStorageAreaConfig : EntityConfig<StorageArea>
        {
            /// <summary>
            /// 实体配置
            /// </summary>
            protected override void ConfigMeta()
            {
                Meta.Property(StationStorageExtStorageArea.StationStorageAreaExtListProperty).DontMapColumn();
            }
        }
    }
}