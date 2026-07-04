Ext.define('SIE.Web.EMS.SpareParts.Scripts.SparePartStoreScanValueEditor', {
    extend: 'Ext.form.FieldContainer',
    alias: 'widget.SparePartStoreScanValueEditor',
    items: [{
        xtype: 'textfield',
        id: 'SparePartStoreScanValue',
        name: 'SparePartStoreScanValue',
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
                    formView.SparePartStoreComp = comp;

                    if (formEntity.data.WarehouseId == null || formEntity.data.StorePartType == null) {
                        formEntity.setMessage("请先维护【拆机件/原件】/【仓库】！".t());
                        comp.setValue("");
                        return;
                    }

                    SIE.invokeDataQuery({
                        type: "SIE.Web.EMS.SpareParts.DataQuery.SparePartDataQueryer",
                        method: "SparePartStoreBarcodeQuery",
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
                                formEntity.setNumber(info.SparePartStoreInfo.ControlMethod == 30 ? 1 : null);
                                formEntity.setIsCreateNewLabel(info.SparePartStoreInfo.IsCreateNewLabel);
                                formEntity.data.ScanValue = scanValue;

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