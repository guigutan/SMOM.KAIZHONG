SIE.defineCommand("SIE.Web.MES.WorkOrders.Commands.OneKeyToPrepareCommand", {
    meta: { text: "一键备料", group: "edit", iconCls: "icon-TextEdit icon-green" },
    canExecute: function (view) {
        var sel = view.getSelection();
        if (sel == null || sel.length <= 0) {
            return false;
        }
        return true;
    },
    execute: function (view) {
        var win = SIE.Window.show({
            title: "备料".t(),
            width: 300,
            height: 150,
            items: {
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },
                items: [{
                    xtype: 'combobox',
                    id: 'WorkOrderMpTypeId001',
                    name: 'WorkOrderMpTypeName001',
                    margin: '5',
                    fieldLabel: '备料类型'.t(),
                    editable: false,
                    queryMode: 'local',
                    displayField: 'Name',
                    valueField: 'Value',
                    value: 20,
                    store: {
                        fields: ["Name", "Value"],
                        data: [
                            { Name: "创建发运单".t(), Value: 10 },
                            { Name: "创建备料单".t(), Value: 20 }
                        ]
                    },
                }]
            },
            callback: function (btn) {
                if (btn == "确定".t()) {
                    var selectModels = view.getSelection();
                    var selectIds = view.getSelectionIds(selectModels);
                    var woList = [];
                    for (var i = 0; i < selectModels.length; i++) {
                        var data = selectModels[i].getData();
                        woList.push({Id:data.Id,Code:data.No})
                    }
                    var type = Ext.getCmp('WorkOrderMpTypeId001').value;
                    var data = {
                        WoList: woList,
                        Type: type,
                    }
                    view.execute({
                        data: data,
                        withIds: true,
                        selectIds: selectIds,
                        success: function (res) {
                            if (res.Success) {
                                SIE.Msg.showMessage(`创建成功`);
                                win.close();
                                view.reloadData();
                            }
                        }
                    });
                }
            }
        })
    }
})