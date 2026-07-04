Ext.define('SIE.Web.EMS.SpareParts.OutDepotHandovers.Scripts.HandoverDetailScanValueEditor', {
    extend: 'Ext.form.FieldContainer',
    alias: 'widget.HandoverDetailScanValueEditor',
    items: [{
        xtype: 'textfield',
        id: 'HandoverDetailScanValue',
        name: 'HandoverDetailScanValue',
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
                    formView.outDepotHandoverComp = comp;

                    var dtlChildView = formView.findChild('SIE.EMS.SpareParts.OutDepotHandovers.OutDepotHandoverDetail');
                    var dtlStore = dtlChildView.getData();

                    if (formEntity.data.OutDepotHandoverBillId == null) {
                        formEntity.setMessage("请先维护【交接单号】！".t());
                        comp.setValue("");
                        return;
                    }

                    var record = {};

                    //10:物料管控
                    //20:批次管控
                    //30:序列号管控
                    if (formEntity.data.ControlMethod == 20 && formEntity.data.IsSelectSparePart) {
                        //参数3: 从哪个位置开始查询
                        //参数4：是否模糊匹配
                        //参数5：是否大小写敏感
                        //参数6：是否是精确查询
                        record = dtlStore.findRecord('BatchNo', scanValue, 0, false, true, true);
                    }
                    else if (formEntity.data.ControlMethod == 30 && formEntity.data.IsSelectSparePart) {
                        record = dtlStore.findRecord('SeriaNo', scanValue, 0, false, true, true);
                    }
                    else {
                        record = dtlStore.findRecord('BatchNo', scanValue, 0, false, true, true) || dtlStore.findRecord('SeriaNo', scanValue, 0, false, true, true);
                        if (record == null) {
                            record = dtlStore.findRecord('SparePartCode', scanValue, 0, false, true, true);
                            if (record != null) {
                                if (record.data.ControlMethod != 10) {
                                    record = null;
                                }
                            }
                        }
                    }
                    
                    if (record != null) {
                        if (record.data.HandOverStatus == 30) {
                            formEntity.setMessage("该条码已被接收，请确认后重新扫描！".t());
                        }
                        else {
                            formEntity.setIsSelectSparePart(false);
                            formEntity.setSparePartId(record.data.SparePartId);
                            formEntity.setSparePartId_Display(record.data.SparePartCode);
                            formEntity.setSparePartName(record.data.SparePartName);
                            formEntity.setControlMethod(record.data.ControlMethod);
                            formEntity.setBarcode(record.data.ControlMethod == 10 ? '' : scanValue);
                            formEntity.setQty(record.data.Qty);
                            formEntity.setReceiveQty(record.data.Qty);
                            formEntity.setMessage("接收成功，请继续扫描【序列号】/【批次号】/【备件编码】！".t());
                            record.set('ReceiveQty', record.data.Qty);
                            record.set('HandOverStatus', 30);
                        }
                    }
                    else {
                        SIE.invokeDataQuery({
                            type: "SIE.Web.EMS.SpareParts.OutDepotHandovers.DataQuerys.OutDepotHandoverDataQuery",
                            method: "OutDepotHandoverBarcodeQuery",
                            params: [scanValue],
                            async: false,
                            token: formView.token,
                            success: function (res) {
                                var info = res.Result;
                                
                                if (info.Success) {
                                    
                                    info.Message = formEntity.data.Message;
                                    SIE.Msg.askQuestion("所扫描内容存在于其他接收单，或者非所选备件的条码，是否切换交接单和该备件？切换将会取消本单本次已接收的明细！".t(),
                                        function () {

                                            var form = info.OutDepotHandoverInfoList[0];
                                            formEntity.setIsSelectSparePart(false);

                                            setTimeout(function () {
                                                formEntity.setSparePartId(form.SparePartId);
                                                formEntity.setSparePartId_Display(form.SparePartCode);
                                                formEntity.setSparePartCode(form.SparePartCode);
                                                formEntity.setSparePartName(form.SparePartName);
                                                formEntity.setControlMethod(form.ControlMethod);
                                            }, 0);

                                            if (info.OutDepotHandoverInfoList.length == 1) {

                                                formEntity.setOutDepotHandoverBillId(form.Id);
                                                formEntity.setOutDepotHandoverBillId_Display(form.HandoverNo);
                                                formEntity.setOutDepotNo(form.OutDepotNo);
                                                formEntity.setBarcode(form.ControlMethod == 10 ? '' : scanValue);
                                                formEntity.setQty(form.Qty);
                                                formEntity.setReceiveQty(form.ReceiveQty);

                                                info.Message = "接收成功，请继续扫描【序列号】/【批次号】/【备件编码】！".t();
                                            }
                                            else {
                                                formEntity.setOutDepotHandoverBillId(null);
                                                formEntity.setOutDepotHandoverBillId_Display(null);
                                                formEntity.setOutDepotNo(null);
                                                info.Message = "请选择【交接单号】！".t();
                                            }
                                            formEntity.setMessage(info.Message);
                                        });
                                }
                                formEntity.setMessage(info.Message);
                            }
                        });
                    }
                    comp.setValue("");
                }
            }
        },
    }],

});