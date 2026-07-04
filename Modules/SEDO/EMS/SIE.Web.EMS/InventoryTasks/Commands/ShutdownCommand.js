SIE.defineCommand('SIE.Web.EMS.InventoryTasks.Commands.ShutdownCommand', {
    meta: { text: "关闭", group: "edit", iconCls: "icon-CloseView icon-red" },
    canExecute: function (view) {
        var selectModels = view.getSelection();
        if (selectModels.length == 0) return false;
        var res = true;
        SIE.each(selectModels, function (model) {
            if (model.data.CloseRemark=="") {
                res = true
                return res;
            }
            else {
                res = false;
                return res;
            }
        });
        return res;
    },
    execute: function (view, source) {
        var me = this;
        var sel = view.getSelection();
        SIE.Msg.askQuestion(Ext.String.format('你确定关闭选择的{0}条数据吗？'.t(), sel.length), function () {
            var win = SIE.Window.show({
                title: "关闭".t(),
                width: 500,
                height: 250,
                items: {
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [{
                        xtype: 'textareafield',
                        grow: true,
                        id: 'ShutdownDemand_Opinions',
                        name: 'Opinions',
                        margin: '5',
                        fieldLabel: '关单原因'.t(),
                        anchor: '100%'
                    }]
                },
                id: "ShutdownDemandCommand001",
                callback: function (btn) {
                    if (btn == "确定".t()) {
                        var selectModels = view.getSelection();
                        var selectIds = view.getSelectionIds(selectModels);
                        var opinions = Ext.getCmp('ShutdownDemand_Opinions');
                        var postdata = {
                            ApprovalResult: 0,
                            Remark: opinions.value,
                        };
                        view.execute({
                            data: postdata,
                            withIds: true,
                            selectIds: selectIds,
                            success: function (res) {
                                SIE.Msg.showMessage("关闭成功!".t());
                                win.close();
                                view.reloadData();
                            }
                        });
                        return false;
                    }
                }
            });
        });
    }
});