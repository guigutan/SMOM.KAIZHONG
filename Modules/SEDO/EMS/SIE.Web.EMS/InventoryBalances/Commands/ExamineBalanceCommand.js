SIE.defineCommand('SIE.Web.EMS.InventoryBalances.Commands.ExamineBalanceCommand', {
    meta: { text: "审核", group: "edit", iconCls: "icon-Check icon-blue" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length !== 1) {
            return false;
        }
        var p = view.getCurrent();
        if (p == null) return false;
        if (p.data.ApprovalStatus !== 20) return false;
        return true;
    },
    execute: function (view, source) {
        var win = SIE.Window.show({
            title: "盘点平账审核".t(),
            width: 500,
            height: 250,
            items: {
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },
                items: [{
                    xtype: 'combobox',
                    id: 'ExamineBalance_Result',
                    name: 'ExamineBalanceResult',
                    margin: '5',
                    fieldLabel: '审核结果'.t(),
                    editable: false,
                    queryMode: 'local',
                    displayField: 'Name',
                    valueField: 'Value',
                    store: {
                        fields: ["Name", "Value"],
                        data: [
                            { Name: "通过".t(), Value: 30 },
                            { Name: "驳回".t(), Value: 40 }
                        ]
                    },
                }, {
                    xtype: 'textareafield',
                    grow: true,
                    id: 'ExamineBalance_Opinions',
                    name: 'Opinions',
                    margin: '5',
                    fieldLabel: '审核意见'.t(),
                    anchor: '100%'
                }]
            },
            id: "ExamineBalanceCommand001",
            callback: function (btn) {
                if (btn == "确定".t()) {
                    var selectModels = view.getSelection();
                    var selectIds = view.getSelectionIds(selectModels);
                    var result = Ext.getCmp('ExamineBalance_Result');
                    var opinions = Ext.getCmp('ExamineBalance_Opinions');
                    var postdata = {
                        ApprovalResult: result.value,
                        Remark: opinions.value,
                    };
                    view.execute({
                        data: postdata,
                        withIds: true,
                        selectIds: selectIds,
                        success: function (res) {
                            SIE.Msg.showMessage("审核成功!".t());
                            win.close();
                            view.reloadData();
                        }
                    });
                    return false;
                }
            }
        });
    }
});