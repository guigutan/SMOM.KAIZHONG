SIE.defineCommand('SIE.Web.Equipments.EquipmentCards.Commands.AuditEquipCardCommand', {
    meta: { text: "审核", group: "edit", iconCls: "icon-Check icon-blue" },
    canExecute: function (view) {
        var selectModels = view.getSelection();
        if (selectModels.length == 0) {
            return false;
        }
        var res = true;
        SIE.each(selectModels, function (model) {
            if (model.data.ApprovalStatus !== SIE.Equipments.Enums.ApprovalStatus.PendingReview.value) {
                res = false;
                return false;
            }
        });
        return res;
    },
    execute: function (view, source) {
        var win = SIE.Window.show({
            title: "设备立卡审核".t(),
            width: 500,
            height: 250,
            items: {
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },
                items: [{
                    xtype: 'combobox',
                    id: 'EquipmentCardApproval_Result',
                    name: 'EquipmentCardApprovalResult',
                    margin: '5',
                    fieldLabel: '审核结果'.t(),
                    editable: false,
                    queryMode: 'local',
                    displayField: 'Name',
                    valueField: 'Value',
                    store: {
                        fields: ["Name", "Value"],
                        data: [
                            { Name: "审核通过".t(), Value: 30 },
                            { Name: "审核不通过".t(), Value: 40 }
                        ]
                    },
                }, {
                    xtype: 'textareafield',
                    grow: true,
                        id: 'EquipmentCardApproval_Opinions',
                    name: 'Opinions',
                    margin: '5',
                    fieldLabel: '审核意见'.t(),
                    anchor: '100%'
                }]
            },
            id: "EquipmentCardApprovalCommand001",
            callback: function (btn) {
                if (btn == "确定".t()) {
                    var selectModels = view.getSelection();
                    var selectIds = view.getSelectionIds(selectModels);
                    var result = Ext.getCmp('EquipmentCardApproval_Result');
                    var opinions = Ext.getCmp('EquipmentCardApproval_Opinions');
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