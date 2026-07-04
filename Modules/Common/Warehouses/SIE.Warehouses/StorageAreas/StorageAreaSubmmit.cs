using SIE.Domain;

namespace SIE.Warehouses.Common
{
    /// <summary>
    /// 库区保存后需要保存库区基本资料
    /// </summary>
    [System.ComponentModel.DisplayName("库区保存后需要保存库区基本资料")]
    [System.ComponentModel.Description("库区保存后需要保存库区基本资料")]
    public class StorageAreaInfoSubmmit : OnSubmitted<StorageArea>
    {
        /// <summary>
        /// 保存仓库后执行
        /// </summary>
        /// <param name="entity">仓库实体</param>
        /// <param name="e">实体提交参数</param>
        protected override void Invoke(StorageArea entity, EntitySubmittedEventArgs e)
        {
            if (e.Action == SubmitAction.Insert || e.Action == SubmitAction.Update)
            {
                #region 库区基本资料
                StorageAreaInfo storageareaInfo = entity.GetProperty(StorageAreaInfoDetailProperty.StorageAreaInfoProperty);
                if (storageareaInfo == null)
                {
                    storageareaInfo = RT.Service.Resolve<WarehouseController>().GetStorageAreaInfoDetail(entity.Id);
                    if (storageareaInfo != null)
                    {
                        return;
                    }

                    storageareaInfo = new StorageAreaInfo() { StorageArea = entity, StorageAreaId = entity.Id };
                }
                if (storageareaInfo.PersistenceStatus != PersistenceStatus.Unchanged)
                {
                    storageareaInfo.StorageAreaId = entity.Id;
                    RF.Save(storageareaInfo);
                    storageareaInfo.MarkSaved();
                }
                #endregion
            }
        }
    }

    /// <summary>
    /// 库区保存后需要保存库区基本资料
    /// </summary>
    [System.ComponentModel.DisplayName("库区保存后需要保存库区操作管理")]
    [System.ComponentModel.Description("库区保存后需要保存库区操作管理")]
    public class StorageAreaOperationSubmmit : OnSubmitted<StorageArea>
    {
        /// <summary>
        /// 保存仓库后执行
        /// </summary>
        /// <param name="entity">仓库实体</param>
        /// <param name="e">实体提交参数</param>
        protected override void Invoke(StorageArea entity, EntitySubmittedEventArgs e)
        {
            double? loc1 = null;
            double? loc2 = null;
            if (e.Action == SubmitAction.Insert)
            {
                var whCode = entity.Warehouse.Code;
                StorageLocation storageLocation = new StorageLocation()
                {
                    Code = "InterPutaway_" + entity.Code + "_" + whCode,
                    Name = "InterPutaway_" + entity.Code + "_" + whCode,
                    LibraryType = LibraryType.Fictitious,
                    IsTemporary = true,
                    AreaId = entity.Id,
                    IsAutomatedStorage = entity.IsAutomatedArea,
                    WarehouseId = entity.WarehouseId,
                };
                RF.Save(storageLocation);
                loc1 = storageLocation.Id;
                var op = RT.Service.Resolve<WarehouseController>().GetStorageLocationOperation(storageLocation.Id);
                op.IsTemporary = true;
                op.IsFocus = false;
                op.IsPick = false;
                op.IsLayIn = false;
                RF.Save(op);
                StorageLocation storageLocationPick = new StorageLocation()
                {
                    Code = "InterPick_" + entity.Code + "_" + whCode,
                    LibraryType = LibraryType.Fictitious,
                    Name = "InterPick_" + entity.Code + "_" + whCode,
                    IsFocus = true,
                    AreaId = entity.Id,
                    IsAutomatedStorage = entity.IsAutomatedArea,
                    WarehouseId = entity.WarehouseId,
                };
                RF.Save(storageLocationPick);
                loc2 = storageLocationPick.Id;
                var op2 = RT.Service.Resolve<WarehouseController>().GetStorageLocationOperation(storageLocationPick.Id);
                op2.IsTemporary = false;
                op2.IsFocus = true;
                op2.IsPick = false;
                op2.IsLayIn = false;
                RF.Save(op2);
            }
            if (e.Action == SubmitAction.Insert || e.Action == SubmitAction.Update)
            {
                #region 库区操作管理
                StorageAreaOperation storageareaOperation = entity.GetProperty(StorageAreaOperationDetailProperty.StorageAreaOperationProperty);
                if (storageareaOperation == null)
                {
                    storageareaOperation = RT.Service.Resolve<WarehouseController>().GetStorageAreaOperationDetail(entity.Id);
                    if (storageareaOperation != null)
                    {
                        return;
                    }

                    storageareaOperation = new StorageAreaOperation() { StorageArea = entity, StorageAreaId = entity.Id };
                }
                if (storageareaOperation.PersistenceStatus != PersistenceStatus.Unchanged)
                {
                    storageareaOperation.StorageAreaId = entity.Id;
                    if (loc1.HasValue)
                        storageareaOperation.UpTransitLocationId = loc1.Value;
                    if (loc2.HasValue)
                        storageareaOperation.DownTransitLocationId = loc2.Value;
                    RF.Save(storageareaOperation);
                    storageareaOperation.MarkSaved();
                }
                #endregion

            }
        }
    }

    /// <summary>
    /// 库区保存后需要保存库区立库配置
    /// </summary>
    [System.ComponentModel.DisplayName("库区保存后需要保存库区立库配置")]
    [System.ComponentModel.Description("库区保存后需要保存库区立库配置")]
    public class StorageAreaWcsSubmmit : OnSubmitted<StorageArea>
    {
        /// <summary>
        /// 保存仓库后执行
        /// </summary>
        /// <param name="entity">仓库实体</param>
        /// <param name="e">实体提交参数</param>
        protected override void Invoke(StorageArea entity, EntitySubmittedEventArgs e)
        {
            if (e.Action == SubmitAction.Insert || e.Action == SubmitAction.Update)
            {
                #region 库区立库配置
                StorageAreaWcs storageareaWcs = entity.GetProperty(StorageAreaWcsDetailProperty.StorageAreaWcsProperty);
                if (storageareaWcs == null)
                {
                    storageareaWcs = RT.Service.Resolve<WarehouseController>().GetStorageAreaWcs(entity.Id);
                    if (storageareaWcs != null)
                    {
                        return;
                    }

                    storageareaWcs = new StorageAreaWcs() { StorageArea = entity, StorageAreaId = entity.Id };
                }
                if (storageareaWcs.PersistenceStatus != PersistenceStatus.Unchanged)
                {
                    storageareaWcs.StorageAreaId = entity.Id;
                    RF.Save(storageareaWcs);
                    storageareaWcs.MarkSaved();
                }
                #endregion
            }
        }
    }
}
