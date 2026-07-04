SIE.defineCommand("SIE.Web.EMS.EquipLends.Commands.EquipLendExamineCommand", {
    meta: { text: "审核".t(), group: "edit", iconCls: "iconfont icon-NetworkNormal icon-blue" },
    canExecute: function (view) {
        var sel = view.getSelection();
        if (sel == null || sel.length <= 0) {
            return false;
        }
        var flag = true;
        for (var i = 0; i < sel.length; i++) {
            var data = sel[i].getData();
            if (data.LendState != 1 && data.LendState != 3) { // 只有状态为借出待审核或归还待审核的单据才能点击
                flag = false;
                break;
            }
        }
        return flag;
    },
    execute: function (view) {
        var win = SIE.Window.show({
            title: "借还审核".t(),
            width: 500,
            height: 250,
            items: {
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },
                items: [{
                    xtype: 'combobox',
                    id: 'EquipLendManageExamine_Result',
                    name: 'EquipLendManageExamineResult',
                    margin: '5',
                    fieldLabel: '审核结果'.t(),
                    editable: false,
                    queryMode: 'local',
                    displayField: 'Name',
                    valueField: 'Value',
                    value: 10,
                    store: {
                        fields: ["Name", "Value"],
                        data: [
                            { Name: "通过".t(), Value: 10 },
                            { Name: "驳回".t(), Value: 20 }
                        ]
                    },
                }, {
                    xtype: 'textareafield',
                    grow: true,
                    id: 'EquipLendManageExamine_Remark',
                    name: 'EquipLendManageExamineRemark',
                    margin: '5',
                    fieldLabel: '审核意见'.t(),
                    anchor: '100%'
                }]
            },
            id: "EquipLendManageExamine_001",
            canClose: true,
            listeners: {
                beforeclose: function (win) {
                    return win.canClose; // 阻止窗口关闭
                }
            },
            callback: function (btn) {
                if (btn == "确定".t()) {
                    var selectIds = view.getSelectionIds();
                    var result = Ext.getCmp('EquipLendManageExamine_Result');
                    var remark = Ext.getCmp('EquipLendManageExamine_Remark');
                    if (result.value === 20 && remark.value.length <= 0) {
                        SIE.Msg.showError("审核结果为驳回时，审核意见必填".t());
                        win.canClose = false;
                        return;
                    }
                    var postdata = {
                        Result: result.value,
                        Remark: remark.value,
                    };
                    view.execute({
                        data: postdata,
                        withIds: true,
                        selectIds: selectIds,
                        success: function (res) {
                            SIE.Msg.showInstantMessage("审核成功!".t());
                            win.canClose = true;
                            win.close();
                            view.reloadData();
                        }
                    });
                }
                else {
                    win.canClose = true;
                    win.close();
                }
            }
        });
    }
})
