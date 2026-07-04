namespace SIE.EventMessages.StationStorage
{
    /// <summary>
    /// 工位库存帮助类
    /// </summary>
    public static class StationStorageHelper
    {
        /// <summary>
        /// 物料库存变更
        /// </summary>
        /// <param name="stationId">工位ID</param>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="itemId">物料ID</param>
        /// <param name="actStoreQty">实际库存变更数</param>
        /// <param name="budgetQty">预库存变更数</param>
        /// <param name="sendingQty">在途库存变更数</param>
        public static void ItemStoreChanged(double stationId, double? workOrderId, double itemId, decimal actStoreQty, decimal budgetQty, decimal sendingQty)
        {
            ////当体条码和半成品不支持提前上料，且未指定工单，不计入库存
            if (!workOrderId.HasValue)
                return;
            if (actStoreQty == 0 && budgetQty == 0 && sendingQty == 0)
                return;
            var store = new StationItemStoreEvent()
            {
                StationId = stationId,
                ItemId = itemId,
                ActStoreQty = actStoreQty,
                BudgetQty = budgetQty,
                SendingQty = sendingQty,
                WorkOrderId = workOrderId.Value
            };
            RT.Service.Resolve<IStationItemStore>().UpdateStationStorage(store);
        }
    }
}