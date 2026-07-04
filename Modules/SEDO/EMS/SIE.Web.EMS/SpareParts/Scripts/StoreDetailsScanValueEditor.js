Ext.define('SIE.Web.EMS.SpareParts.Scripts.StoreDetailsScanValueEditor', {
    extend: 'Ext.form.FieldContainer',
    alias: 'widget.StoreDetailsScanValueEditor',
    items: [{
        xtype: 'textfield',
        id: 'StoreDetailsScanValue',
        name: 'StoreDetailsScanValue',
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
                    formView.StoreDetailsComp = comp;

                    var dtlChildView = formView.findChild('SIE.EMS.SpareParts.StoreDetail');
                    var dtlStore = dtlChildView.getData();

                    if (formEntity.data.StorageLocationId == null || formEntity.data.QualityStatus == null) {
                        formEntity.setMessage("请先维护【库位】/【质量状态】！".t());
                        comp.setValue("");
                        return;
                    }

                    SIE.invokeDataQuery({
                        type: "SIE.Web.EMS.SpareParts.DataQuery.SparePartDataQueryer",
                        method: "StoreDetailsBarcodeQuery",
                        params: [scanValue, formEntity.data],
                        async: false,
                        token: formView.token,
                        success: function (res) {

                            var info = res.Result;

                            if (info.Success) {

                                formEntity.setIsSelectSparePart(false);
                                formEntity.setSparePartId(info.SparePartStoreInfo.SparePartId);
                                formEntity.setSparePartId_Display(info.SparePartStoreInfo.SparePartCode);
                                formEntity.setSparePartName(info.SparePartStoreInfo.SparePartName);
                                formEntity.setControlMethod(info.SparePartStoreInfo.ControlMethod);
                                formEntity.setIsReplacement(info.SparePartStoreInfo.IsReplacement);
                                formEntity.setNumber(info.SparePartStoreInfo.Number);
                                formEntity.setUnitPrice(info.SparePartStoreInfo.UnitPrice);

                                for (var i = 0; i < dtlStore.getCount(); i++) {

                                    var record = dtlStore.getAt(i);
                                    if (record.data.QualityStatus == formEntity.data.QualityStatus
                                        && ((record.data.SparePartId_Display == scanValue && record.data.ControlMethod == 10)
                                            || record.data.BatchNumber == scanValue || record.data.Sn == scanValue)) {

                                        record.setUnitPrice(info.SparePartStoreInfo.UnitPrice);
                                        record.setStorageLocationId_Display(formEntity.data.StorageLocationId_Display);
                                        record.setStorageLocationId(formEntity.data.StorageLocationId);
                                        break;
                                    }
                                }
                            }
                            formEntity.setMessage(info.Message);
                            comp.setValue("");
                        }
                    });

                }
            }
        },
    }],

});