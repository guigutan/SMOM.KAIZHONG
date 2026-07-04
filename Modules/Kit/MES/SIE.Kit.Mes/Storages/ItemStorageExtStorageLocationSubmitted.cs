using SIE.Domain;

namespace SIE.Kit.MES.Storages
{
    /// <summary>
    /// 产线货区货位保存后需要保存产线物料货位
    /// </summary>
    [System.ComponentModel.DisplayName("产线货区货位保存后需要保存产线物料货位")]
    [System.ComponentModel.Description("产线货区货位保存后需要保存产线物料货位")]
    public class ItemStorageExtStorageLocationSubmitted : OnSubmitted<StorageLocation>
    {
        /// <summary>
        /// 保存后执行
        /// </summary>
        /// <param name="entity">产线货区货位</param>
        /// <param name="e">实体提交参数</param>
        protected override void Invoke(StorageLocation entity, EntitySubmittedEventArgs e)
        {
            if (e.Action == SubmitAction.Insert || e.Action == SubmitAction.Update)
            {
                EntityList<ItemStorage> itemStorages = entity.GetProperty(ItemStorageExtStorageLocation.ItemStorageExtListProperty);
                if (itemStorages == null)
                {
                    return;
                }

                RF.Save(itemStorages);
                itemStorages.MarkSaved();
            }
            else if (e.Action == SubmitAction.Delete)
            {
                var itemStorages = RT.Service.Resolve<StorageController>().GetItemStorages(entity.Id, null, null);
                if (itemStorages != null && itemStorages.Count > 0)
                {
                    foreach (var item in itemStorages)
                    {
                        item.PersistenceStatus = PersistenceStatus.Deleted;
                    }

                    RF.Save(itemStorages);
                }
            }
        }
    }
}