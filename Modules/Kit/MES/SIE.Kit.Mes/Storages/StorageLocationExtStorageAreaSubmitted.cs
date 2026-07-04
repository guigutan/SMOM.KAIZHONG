using SIE.Domain;

namespace SIE.Kit.MES.Storages
{
    /// <summary>
    /// 工位货区保存后需要保存产线货区货位
    /// </summary>
    [System.ComponentModel.DisplayName("工位货区保存后需要保存产线货区货位")]
    [System.ComponentModel.Description("工位货区保存后需要保存产线货区货位")]
    public class StorageLocationExtStorageAreaSubmitted : OnSubmitted<StorageArea>
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
            //    EntityList<StorageLocation> storageLocations = entity.GetProperty(StorageLocationExtStorageArea.StorageLocationExtListProperty);
            //    if (storageLocations == null)
            //    {
            //        return;
            //    }

            //    RF.Save(storageLocations);
            //    storageLocations.MarkSaved();
            //}
            //else if (e.Action == SubmitAction.Delete)
            //{
            //    var storageLocations = RT.Service.Resolve<StorageController>().GetStorageLocations(entity.Id, null, null);
            //    if (storageLocations != null && storageLocations.Count > 0)
            //    {
            //        foreach (var item in storageLocations)
            //        {
            //            item.PersistenceStatus = PersistenceStatus.Deleted;
            //        }

            //        RF.Save(storageLocations);
            //    }
            //}
        }
    }
}