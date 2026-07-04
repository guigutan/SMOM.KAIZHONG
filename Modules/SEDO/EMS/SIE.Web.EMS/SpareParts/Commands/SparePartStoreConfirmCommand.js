SIE.defineCommand('SIE.Web.EMS.SpareParts.Commands.SparePartStoreConfirmCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "确认", group: "edit", iconCls: "icon-NetworkNormal icon-blue" },
    canExecute: function (view) {
        var entity = view.getCurrent();

        if (entity == null) {
            return false;
        }
        return entity.data.StorePartType != null && entity.data.WarehouseId != null && entity.data.SparePartId != null
            && entity.data.QualityStatus != null && entity.data.Number > 0 && entity.data.StorageLocationId != null;
    },
    execute: function (view, source) {
        var entity = view.getCurrent();

        if (entity.data.ControlMethod == 20 || entity.data.ControlMethod == 30) {
            if (Ext.isEmpty(entity.data.ScanValue) && !entity.data.IsCreateNewLabel) {
                SIE.Msg.showError('批次管控或序列号管控的无条码备件需勾选生成新标签！'.t());
                return false;
            }
        }

        if (entity.data.PartOutDepotDetailId != null && entity.data.Number > entity.data.CanReturnQty) {
            SIE.Msg.showError('退回数量须小于关联出库单行号的可退数量！'.t());
            return false;
        }

        var applyChildView = view.findChild('SIE.EMS.SpareParts.StoreDetail');
        var applyChildStore = applyChildView.getData();
        var data = {};

        var tempLineNoList = applyChildStore.getData().items.where(function (p) { return !Ext.isEmpty(p.getLineNo()); })
            .select(function (p) { return parseInt(p.getLineNo()); });
        var lineNo = tempLineNoList.length == 0 ? 1 : tempLineNoList.max() + 1;

        data.LineNo = lineNo;
        data.SparePartId = entity.data.SparePartId;
        data.SparePartId_Display = entity.data.SparePartId_Display;
        data.SparePartName = entity.data.SparePartName;
        data.ControlMethod = entity.data.ControlMethod;
        data.IsReplacement = entity.data.IsReplacement;
        data.IsOldPart = entity.data.StorePartType == 5;
        data.PartOutDepotDetailId = entity.data.PartOutDepotDetailId;
        data.PartOutDepotDetailId_Display = entity.data.PartOutDepotDetailId_Display;
        data.OutDepotLineNo = entity.data.PartOutDepotDetailId_Display;
        data.QualityStatus = entity.data.QualityStatus;
        data.UnitPrice = entity.data.UnitPrice;
        data.Number = entity.data.Number;
        data.StorageLocationId = entity.data.StorageLocationId;
        data.StorageLocationId_Display = entity.data.StorageLocationId_Display;
        data.BatchNumber = entity.data.ControlMethod == 20 ? entity.data.ScanValue : "";
        data.Sn = entity.data.ControlMethod == 30 ? entity.data.ScanValue : "";

        if (entity.data.IsCreateNewLabel && (entity.data.ControlMethod == 20 || entity.data.ControlMethod == 30)) {
            SIE.invokeDataQuery({
                type: "SIE.Web.EMS.SpareParts.DataQuery.SparePartDataQueryer",
                method: entity.data.ControlMethod == 20 ? "GetBatchCode" : "GetSnCode",
                params: [],
                async: false,
                token: view.token,
                success: function (res) {
                    if (res.Success) {
                        data.BatchNumber = entity.data.ControlMethod == 20 ? res.Result : "";
                        data.Sn = entity.data.ControlMethod == 30 ? res.Result : "";
                        applyChildStore.add(data);
                        entity.setIsExistDetail(true);//标记当前界面是否有出库明细数据

                        entity.setSparePartId(null);
                        entity.setSparePartId_Display("");
                        entity.setSparePartName("");
                        entity.setControlMethod("");
                        entity.setIsReplacement("");
                        entity.setQualityStatus("");
                        entity.setNumber("");
                        entity.setCanReturnQty("");
                        entity.setUnitPrice("");
                        entity.setStorageLocationId(null);
                        entity.setStorageLocationId_Display("");
                        entity.setPartOutDepotDetailId(null);
                        entity.setPartOutDepotDetailId_Display("");
                        entity.setIsCreateNewLabel(false);
                        entity.setScanValue("");
                        entity.setIsSelectSparePart(false);
                        entity.setMessage("请扫描【序列号】/【批次号】/【备件编码】！".t());
                    }
                }
            });
        }
        else {
            applyChildStore.add(data);
            entity.setIsExistDetail(true);//标记当前界面是否有出库明细数据

            entity.setSparePartId(null);
            entity.setSparePartId_Display("");
            entity.setSparePartName("");
            entity.setControlMethod("");
            entity.setIsReplacement("");
            entity.setQualityStatus("");
            entity.setNumber("");
            entity.setCanReturnQty("");
            entity.setUnitPrice("");
            entity.setStorageLocationId(null);
            entity.setStorageLocationId_Display("");
            entity.setPartOutDepotDetailId(null);
            entity.setPartOutDepotDetailId_Display("");
            entity.setIsCreateNewLabel(false);
            entity.setScanValue("");
            entity.setIsSelectSparePart(false);
            entity.setMessage("请扫描【序列号】/【批次号】/【备件编码】！".t());
        }
    }
});