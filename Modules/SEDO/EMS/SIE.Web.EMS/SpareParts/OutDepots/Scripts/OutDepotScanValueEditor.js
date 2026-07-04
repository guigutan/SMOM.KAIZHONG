Ext.define('SIE.Web.EMS.SpareParts.OutDepots.Scripts.OutDepotScanValueEditor', {
    extend: 'Ext.form.FieldContainer',
    alias: 'widget.OutDepotScanValueEditor',
    items: [{
        xtype: 'textfield',
        id: 'OutDepotScanValue',
        name: 'OutDepotScanValue',
        hideLabel: true,
        style: 'width:100%;border-color:#3892D4;',
        fieldStyle: 'background-color:#90EE90;height:35px;',
        allowBlank: true,
        forceSelection: true,
        listeners: {
            specialkey: function (comp, e) {
                if (e.getKey() == e.ENTER) {

                    var scanValue = comp.getValue();
                    if (scanValue == "")
                        return;
                    var formView = this.up('form').SIEView;
                    var formEntity = formView.getData();
                    formView.outDepotComp = comp;

                    var applyChildView = formView.findChild('SIE.EMS.SpareParts.OutDepots.Details.OutDepotDetail');
                    var outDepotChildView = formView.findChild('SIE.EMS.SpareParts.OutDepots.Details.PartOutDepotDetail');

                    if (formEntity.data.WarehouseId == null || formEntity.data.QualityStatus == null) {
                        formEntity.setMessage("请先维护【出库仓库】/【质量状态】！".t());
                        comp.setValue("");
                        return;
                    }

                    if (formEntity.data.IsBarcode) {

                        var storageLocationId = formEntity.data.StorageLocationId;
                        var storageLocationName = formEntity.data.StorageLocationId_Display;
                        var storageLocationNum = formEntity.data.StorageLocationNum;

                        var spareSpartId = formEntity.data.SparePartId;

                        SIE.invokeDataQuery({
                            type: "SIE.Web.EMS.SpareParts.OutDepots.DataQuerys.OutDepotViewDataQuery",
                            method: "OutDepotBarcodeQuery",
                            params: [scanValue, formEntity.data],
                            async: false,
                            token: formView.token,
                            success: function (res) {
                                var info = res.Result;

                                if (info.Success) {

                                    formEntity.data.ScanValue = info.OutDepotInfo.ControlMethod == 30 ? "" : scanValue;//序列号管控执行成功后继续回到当前状态
                                    formEntity.data.IsBarcode = info.OutDepotInfo.ControlMethod == 30 ? true : false;//标记当前是否处于扫描条码的状态

                                    formEntity.setSparePartId(info.OutDepotInfo.SparePartId);
                                    formEntity.setSparePartId_Display(info.OutDepotInfo.SparePartCode);
                                    formEntity.setSparePartName(info.OutDepotInfo.SparePartName);
                                    formEntity.setControlMethod(info.OutDepotInfo.ControlMethod);
                                    formEntity.setAdviceStorageLocation(info.OutDepotInfo.AdviceStorageLocation);
                                    
                                    if (formEntity.data.StorageLocationId == null) {

                                        if (storageLocationId == null) {
                                            formEntity.setStorageLocationId(info.OutDepotInfo.StorageLocationId);
                                            formEntity.setStorageLocationId_Display(info.OutDepotInfo.StorageLocationName);
                                            formEntity.setStorageLocationNum(info.OutDepotInfo.StorageLocationNum);
                                        }
                                        else {
                                            if (spareSpartId == info.OutDepotInfo.SparePartId) {
                                                formEntity.setStorageLocationId(storageLocationId);
                                                formEntity.setStorageLocationId_Display(storageLocationName);
                                                formEntity.setStorageLocationNum(storageLocationNum);

                                            } else {
                                                formEntity.setStorageLocationId(info.OutDepotInfo.StorageLocationId);
                                                formEntity.setStorageLocationId_Display(info.OutDepotInfo.StorageLocationName);
                                                formEntity.setStorageLocationNum(info.OutDepotInfo.StorageLocationNum);
                                            }
                                        }
                                    }

                                    if (info.OutDepotInfo.ControlMethod == 30) {
                                        var isExistApplyDetail = false;
                                        var isExistOutDepotDetail = false;
                                        var isExistError = false;
                                        
                                        var applyChild = applyChildView.getData();
                                        var outDepotChild = outDepotChildView.getData();

                                        for (var i = 0; i < outDepotChild.getCount(); i++) {
                                            var record = outDepotChild.getAt(i);
                                            if (record.data.SeriaNo != "") {
                                                if (record.data.SeriaNo == info.OutDepotInfo.PartOutDepotDetailList[0].SeriaNo) {
                                                    isExistOutDepotDetail = true;
                                                    break;
                                                }
                                            }
                                        }

                                        if (isExistOutDepotDetail) {
                                            info.Message = Ext.String.format('出库明细中已存在序列号【{0}】备件，请确认后重新扫描！'.t(), info.OutDepotInfo.PartOutDepotDetailList[0].SeriaNo);
                                        }
                                        else {
                                            
                                            for (var i = 0; i < applyChild.getCount(); i++) {
                                                var record = applyChild.getAt(i);
                                                if (record.data.SparePartCodeView == info.OutDepotInfo.OutDepotDetailList[0].SparePartCodeView) {
                                                    isExistApplyDetail = true;
                                                    var requireCount = 0;
                                                    if (formEntity.data.CreateDate == null) {
                                                        requireCount = record.data.RequireCount + info.OutDepotInfo.OutDepotDetailList[0].RequireCount;
                                                        record.set('RequireCount', requireCount);
                                                        record.set('PickedCount', requireCount);
                                                    }
                                                    else {
                                                        requireCount = record.data.PickedCount + info.OutDepotInfo.OutDepotDetailList[0].RequireCount;
                                                        if (requireCount > record.data.RequireCount) {
                                                            isExistError = true;
                                                            info.Message = "该备件的拣货数已超过申请数量，请重新输入出库数量后回车！".t();
                                                        }
                                                        else {
                                                            record.set('PickedCount', requireCount);
                                                        }
                                                    }
                                                    break;
                                                }
                                            }
                                            if (!isExistError) {
                                                outDepotChild.add(info.OutDepotInfo.PartOutDepotDetailList[0]);

                                                if (!isExistApplyDetail) {
                                                    applyChild.add(info.OutDepotInfo.OutDepotDetailList[0]);
                                                }
                                            }
                                        }
                                        formEntity.data.IsExistDetail = isExistError ? formEntity.data.IsExistDetail : true;//标记当前界面是否有出库明细数据
                                    }
                                }
                                formEntity.setMessage(info.Message);
                                comp.setValue("");
                            }
                        });
                    }
                    else {

                        if (!/^\+?[1-9][0-9]*$/.test(scanValue)) {
                            formEntity.setMessage("出库【数量】需为正整数，请确认！".t());
                            comp.setValue("");
                            return;
                        }

                        if (Number(scanValue) > 2147483647) {
                            formEntity.setMessage("出库【数量】过大，已超出系统限制，请重新输入！".t());
                            comp.setValue("");
                            return;
                        }

                        if (formEntity.data.ControlMethod == 10) {
                            formEntity.data.ScanValue = formEntity.data.SparePartId_Display;
                            formEntity.data.IsBarcode = false;//标记当前是否处于扫描条码的状态
                        }

                        if (formEntity.data.StorageLocationId == null) {
                            formEntity.setMessage("请先维护出库【库位】！".t());
                            comp.setValue("");
                            return;
                        }

                        var applyChild = applyChildView.getData();
                        var amountList = applyChild.getData().items.where(function (p) { return !Ext.isEmpty(p.getPickedCount()); })
                            .select(function (p) { return parseInt(p.getPickedCount()); });
                        var pickedQty = amountList.sum();

                        SIE.invokeDataQuery({
                            type: "SIE.Web.EMS.SpareParts.OutDepots.DataQuerys.OutDepotViewDataQuery",
                            method: "OutDepotQtyQuery",
                            params: [scanValue, formEntity.data, pickedQty],
                            async: false,
                            token: formView.token,
                            success: function (res) {
                                var info = res.Result;

                                if (info.Success) {

                                    var orignScanValue = formEntity.data.ScanValue;
                                    formEntity.data.ScanValue = "";
                                    formEntity.data.IsBarcode = true;//标记当前是否处于扫描条码的状态

                                    var isExistApplyDetail = false;
                                    var isExistError = false;

                                    for (var i = 0; i < applyChild.getCount(); i++) {
                                        var record = applyChild.getAt(i);
                                        if (record.data.SparePartCodeView == info.OutDepotInfo.OutDepotDetailList[0].SparePartCodeView) {
                                            isExistApplyDetail = true;
                                            var requireCount = 0;
                                            if (formEntity.data.CreateDate == null) {
                                                requireCount = record.data.RequireCount + info.OutDepotInfo.OutDepotDetailList[0].RequireCount;
                                                record.set('RequireCount', requireCount);
                                                record.set('PickedCount', requireCount);
                                            }
                                            else {
                                                requireCount = record.data.PickedCount + info.OutDepotInfo.OutDepotDetailList[0].RequireCount;
                                                if (requireCount > record.data.RequireCount) {
                                                    formEntity.data.ScanValue = orignScanValue;
                                                    formEntity.setIsBarcode(false);//标记当前是否处于扫描条码的状态
                                                    isExistError = true;
                                                    info.Message = "该备件的拣货数已超过申请数量，请重新输入出库数量后回车！".t();
                                                }
                                                else {
                                                    record.set('PickedCount', requireCount);
                                                }
                                            }
                                            break;
                                        }
                                    }

                                    if (!isExistError) {
                                        if (!isExistApplyDetail) {
                                            applyChild.add(info.OutDepotInfo.OutDepotDetailList[0]);
                                        }

                                        var outDepotChild = outDepotChildView.getData();
                                        outDepotChild.add(info.OutDepotInfo.PartOutDepotDetailList[0]);

                                        formEntity.data.IsExistDetail = true;//标记当前界面是否有出库明细数据
                                    }

                                }
                                formEntity.setMessage(info.Message);
                                comp.setValue("");
                            }
                        });

                    }
                }
            }
        },
    }],

});