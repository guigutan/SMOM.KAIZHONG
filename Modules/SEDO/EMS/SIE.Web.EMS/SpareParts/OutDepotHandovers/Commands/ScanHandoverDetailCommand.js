SIE.defineCommand('SIE.Web.EMS.SpareParts.OutDepotHandovers.Commands.ScanHandoverDetailCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "扫码交接", group: "edit", iconCls: "icon-PlaylistPlay icon-blue" },
    canExecute: function (view) {
        if (view.getSelection() == null) {
            return false;
        }
        return view.getSelection().length == 1
            && (view.getSelection()[0].data.HandOverStatus == 10 || view.getSelection()[0].data.HandOverStatus == 20);
    },
    execute: function (view, source) {

        var entity = view.getCurrent();
        var childStore = view.findChild('SIE.EMS.SpareParts.OutDepotHandovers.OutDepotHandoverDetail').getData();
        var model = SIE.getModel(view.model);
        var editEntity = new model();

        editEntity.data.Message = "请扫描【序列号】/【批次号】/【备件编码】！".t();
        editEntity.data.OutDepotHandoverBillId = entity.data.Id;
        editEntity.data.OutDepotHandoverBillId_Display = entity.data.HandoverNo;
        editEntity.data.OutDepotNo = entity.data.OutDepotNo;
        editEntity.data.IsSelectSparePart = false;
        editEntity.data.SparePartId = null;
        editEntity.data.SparePartId_Display = null;
        editEntity.data.SparePartName = null;
        editEntity.data.ControlMethod = null;
        editEntity.data.Barcode = null;
        editEntity.data.Qty = null;
        editEntity.data.ReceiveQty = null;

        SIE.AutoUI.getMeta({
            async: false,
            ignoreCommands: false,
            isDetail: true,
            ignoreQuery: true,
            model: view.model,
            viewGroup: "ScanHandoverDetailsViewGroup",
            callback: function (meta) {

                var detailView = SIE.AutoUI.generateAggtControl(meta);
                detailView._view.setData(editEntity);

                var dtlChildView = detailView._view.findChild('SIE.EMS.SpareParts.OutDepotHandovers.OutDepotHandoverDetail');
                var dtlStore = dtlChildView.getData();
                for (var i = 0; i < childStore.getCount(); i++) {
                    var record = childStore.getAt(i);
                    var childModel = SIE.getModel('SIE.EMS.SpareParts.OutDepotHandovers.OutDepotHandoverDetail');
                    var childRecord = new childModel();
                    var copyData = JSON.parse(JSON.stringify(record.data));  
                    childRecord.data = copyData;
                    dtlStore.add(childRecord);
                }

                var win = SIE.Window.show({
                    title: '扫码交接-备件交接单'.t(),
                    width: '80%',
                    height: '80%',
                    items: detailView.getControl(),
                    callback: function (btn) {
                        if (btn === "确定".t()) {

                            var dataArr = [];

                            var records = dtlStore.query("HandOverStatus", 30);
                            if (records.length == 0) {
                                SIE.Msg.showError('接收明细不存在已接收的数据，请确认！'.t());
                                return false;
                            }

                            for (var i = 0; i < records.length; i++) {
                                dataArr.push(records.items[i].data);
                            }

                            var indata = { Data: Ext.encode(dataArr) };
                            view.execute({
                                data: indata,
                                success: function (res) {
                                    SIE.Msg.showMessage('接收成功'.t());
                                    view.reloadData();
                                    win.close();
                                },
                                error: function (res) {
                                    SIE.Msg.showError(res.Message);
                                }
                            });
                        }
                    }
                });
            }
        });
    }
});