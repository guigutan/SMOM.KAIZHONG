using SIE.Domain;
using SIE.Domain.Validation;
using System;

namespace SIE.Warehouses
{
    /// <summary>
    /// 库位保存后需要前验证
    /// </summary>
    [System.ComponentModel.DisplayName("库位保存后需要前验证")]
    [System.ComponentModel.Description("库位保存后需要前验证")]
    class StorageLocationSubmitting : OnSubmitting<StorageLocation>
    {
        protected override void Invoke(StorageLocation entity, EntitySubmittingEventArgs e)
        {
            if (e.Action == SubmitAction.Insert && entity.Area.IsAutomatedArea && entity.LibraryType != LibraryType.Fictitious)
            {
                if (entity.RowNo <= 0 || entity.ColumnNo <= 0 || entity.Depth < 0 || entity.LayerNo <= 0)
                    throw new ValidationException("立库库位必须维护排层列，且必须大于0，深度大于等于0".L10N());
                string code = "Cell:" + entity.RowNo + "_" + entity.LayerNo + "_" + entity.ColumnNo + "_" + entity.Depth + "_" + entity.Area.Code + "_" + entity.Warehouse.Code;
                if (entity.Code != code)
                    throw new ValidationException("当前库区是立库，编码号必须是[{0}]".L10nFormat(code));
                if (entity.Depth > 0 && !RT.Service.Resolve<WarehouseController>().CheckFrontDeepLocation(entity))
                    throw new ValidationException("当前库位深度有误[最小深度是0]".L10N());
                entity.IsAutomatedStorage = true;
            }
            else if (entity.RoutewayId.HasValue)
            {
                var route = RT.Service.Resolve<WarehouseController>().GetRouteway(entity.WarehouseId, entity.RoutewayId.Value);
                if (route?.StorageAreaId != entity.AreaId)
                    throw new ValidationException("巷道所在库区不是当前库位库区".L10N());
            }
        }
    }

    /// <summary>
    /// 库位保存后需要保存库位基本资料
    /// </summary>
    [System.ComponentModel.DisplayName("库位保存后需要保存库位基本资料")]
    [System.ComponentModel.Description("库位保存后需要保存库位基本资料")]
    class StorageLocationInfoSubmitted : OnSubmitted<StorageLocation>
    {
        /// <summary>
        ///  库位保存后需要保存库位基本资料
        /// </summary>
        /// <param name="entity">库位实体</param>
        /// <param name="e">由该事件生成的事件数据的类型</param>
        protected override void Invoke(StorageLocation entity, EntitySubmittedEventArgs e)
        {
            if (e.Action == SubmitAction.Insert || e.Action == SubmitAction.Update)
            {
                StorageLocationInfo storageLocationInfo = entity.GetProperty(StorageLocationDetailProperty.BaseInfoProperty);
                if (storageLocationInfo == null)
                {
                    storageLocationInfo = RT.Service.Resolve<WarehouseController>().GetStorageLocationInfo(entity.Id);
                    if (storageLocationInfo != null)
                    {
                        return;
                    }

                    storageLocationInfo = new StorageLocationInfo() { StorageLocation = entity, StorageLocationId = entity.Id };
                }

                if (storageLocationInfo.PersistenceStatus != PersistenceStatus.Unchanged)
                {
                    if (storageLocationInfo.Id == 0)
                    {
                        storageLocationInfo.PersistenceStatus = PersistenceStatus.New;
                    }

                    storageLocationInfo.StorageLocationId = entity.Id;
                    RF.Save(storageLocationInfo);
                }
            }
        }
    }

    /// <summary>
    /// 库位保存后需要保存库位仓储资料
    /// </summary>
    [System.ComponentModel.DisplayName("库位保存后需要保存库位仓储资料")]
    [System.ComponentModel.Description("库位保存后需要保存库位仓储资料")]
    class StorageLocationLayinInfoSubmitted : OnSubmitted<StorageLocation>
    {
        /// <summary>
        ///  库位保存后需要保存库位仓储资料
        /// </summary>
        /// <param name="entity">库位实体</param>
        /// <param name="e">由该事件生成的事件数据的类型</param>
        protected override void Invoke(StorageLocation entity, EntitySubmittedEventArgs e)
        {
            if (e.Action == SubmitAction.Insert || e.Action == SubmitAction.Update)
            {
                StorageLocationLayinInfo layinInfo = entity.GetProperty(StorageLocationDetailProperty.LayinInfoProperty);
                if (layinInfo == null)
                {
                    layinInfo = RT.Service.Resolve<WarehouseController>().GetStorageLocationLayinInfo(entity.Id);
                    if (layinInfo != null)
                    {
                        return;
                    }

                    layinInfo = new StorageLocationLayinInfo() { StorageLocation = entity, StorageLocationId = entity.Id };
                }

                if (layinInfo.PersistenceStatus != PersistenceStatus.Unchanged)
                {
                    if (layinInfo.Id == 0)
                    {
                        layinInfo.PersistenceStatus = PersistenceStatus.New;
                    }

                    layinInfo.StorageLocationId = entity.Id;
                    RF.Save(layinInfo);
                }
            }
        }
    }

    /// <summary>
    /// 库位保存后需要保存库位操作管理信息
    /// </summary>
    [System.ComponentModel.DisplayName("库位保存后需要保存库位操作管理信息")]
    [System.ComponentModel.Description("库位保存后需要保存库位操作管理信息")]
    class StorageLocationOperationSubmitted : OnSubmitted<StorageLocation>
    {
        /// <summary>
        ///  库位保存后需要保存库位操作管理信息
        /// </summary>
        /// <param name="entity">库位实体</param>
        /// <param name="e">由该事件生成的事件数据的类型</param>
        protected override void Invoke(StorageLocation entity, EntitySubmittedEventArgs e)
        {
            if (e.Action == SubmitAction.Insert || e.Action == SubmitAction.Update)
            {
                StorageLocationOperation operationInfo = entity.GetProperty(StorageLocationDetailProperty.OperationInfoProperty);
                if (operationInfo == null)
                {
                    operationInfo = RT.Service.Resolve<WarehouseController>().GetStorageLocationOperation(entity.Id);
                    if (operationInfo != null)
                    {
                        return;
                    }

                    operationInfo = new StorageLocationOperation() { IsLayIn = true, IsPick = true, StorageLocation = entity, StorageLocationId = entity.Id };
                }

                if (entity.Warehouse.LibraryType == LibraryType.Entity)
                {
                    if (e.Action == SubmitAction.Insert && entity.Code == Warehouse.STAGE)
                    {
                        operationInfo.IsLayIn = false;
                        operationInfo.IsPick = false;
                        operationInfo.IsFocus = false;
                        operationInfo.IsTemporary = true;
                    }

                    if (e.Action == SubmitAction.Insert && entity.Code == Warehouse.PICKTO)
                    {
                        operationInfo.IsLayIn = false;
                        operationInfo.IsPick = false;
                        operationInfo.IsFocus = true;
                        operationInfo.IsTemporary = false;
                    }
                }
                else
                {
                    if (e.Action == SubmitAction.Insert && entity.Code == Warehouse.STAGE)
                    {
                        operationInfo.IsLayIn = true;
                        operationInfo.IsPick = true;
                        operationInfo.IsFocus = true;
                        operationInfo.IsTemporary = true;
                    }

                    if (e.Action == SubmitAction.Insert && entity.Code == Warehouse.PICKTO)
                    {
                        operationInfo.IsLayIn = true;
                        operationInfo.IsPick = true;
                        operationInfo.IsFocus = true;
                        operationInfo.IsTemporary = true;
                    }
                }

                if (operationInfo.PersistenceStatus != PersistenceStatus.Unchanged)
                {
                    if (operationInfo.Id == 0)
                    {
                        operationInfo.PersistenceStatus = PersistenceStatus.New;
                    }

                    operationInfo.StorageLocationId = entity.Id;
                    if (e.Action == SubmitAction.Insert && (entity.Code.StartsWith("InterPutaway_") || entity.Code.StartsWith("InterPick_")))
                    {
                        RF.Save(operationInfo);
                        return;
                    }
                    DB.Update<StorageLocation>()
                        .Set(p => p.IsLayIn, operationInfo.IsLayIn)
                        .Set(p => p.IsPick, operationInfo.IsPick)
                        .Set(p => p.IsFocus, operationInfo.IsFocus)
                        .Set(p => p.IsTemporary, operationInfo.IsTemporary)
                        .Where(p => p.Id == entity.Id)
                        .Execute();
                    RF.Save(operationInfo);
                }
            }
        }
    }
}
