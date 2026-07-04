using SIE.Domain;

namespace SIE.Kit.MES.Storages
{
    /// <summary>
    /// 工位货区保存后需要保存产线工位货区
    /// </summary>
    [System.ComponentModel.DisplayName("工位货区保存后需要保存产线工位货区")]
    [System.ComponentModel.Description("工位货区保存后需要保存产线工位货区")]
    public class StationStorageExtStorageAreaSubmitted : OnSubmitted<StorageArea>
    {
        /// <summary>
        /// 保存后执行
        /// </summary>
        /// <param name="entity">工位货区</param>
        /// <param name="e">实体提交参数</param>
        protected override void Invoke(StorageArea entity, EntitySubmittedEventArgs e)
        {
            //先注释掉提交后事件，框架实现了保存的时候同时把子也保存，删除没作用，需要删除子才能删除父
            //if (e.Action == SubmitAction.Insert || e.Action == SubmitAction.Update)
            //{
            //    EntityList<StationStorageArea> stationStorageAreas = entity.GetProperty(StationStorageExtStorageArea.StationStorageAreaExtListProperty);
            //    if (stationStorageAreas == null)
            //    {
            //        return;
            //    }

            //    RF.Save(stationStorageAreas);
            //    stationStorageAreas.MarkSaved();
            //}
            //else if (e.Action == SubmitAction.Delete)
            //{
            //    var stationStorageAreas = RT.Service.Resolve<StorageController>().GetStationAreas(entity.Id, null, null);
            //    if (stationStorageAreas != null && stationStorageAreas.Count > 0)
            //    {
            //        foreach (var item in stationStorageAreas)
            //        {
            //            item.PersistenceStatus = PersistenceStatus.Deleted;
            //        }

            //        RF.Save(stationStorageAreas);
            //    }
            //}
        }
    }
}