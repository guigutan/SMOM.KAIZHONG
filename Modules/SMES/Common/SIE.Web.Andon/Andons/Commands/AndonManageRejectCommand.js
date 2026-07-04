SIE.defineCommand('SIE.Web.Andon.Andons.Commands.AndonManageRejectCommand', {
    meta: { text: "驳回", group: "edit", iconCls: "icon-CalendarRemove icon-blue" },
    canExecute: function (view) {
        var entity = view.getCurrent();
        if (entity == null)
            return false;
        if (entity.getState() != 30) {
            return false;
        }
        //列表界面防止多选
        if (view.viewGroup == 'ListView' && view.getSelection().length > 1) {
            return false;
        }
        return true;
    },
    execute: function (view, source) {
        var win = SIE.Window.show({
            title: "驳回".t(),
            width: 420,
            height: 180,
            items: {
                layout: {
                    type: "vbox",
                    align: "stretch",
                },
                items: [{
                    xtype: 'textareafield',
                    grow: true,
                    id: "AndonManageReject_Reason",
                    name: "Reject_Reason",
                    margin: "5",
                    fieldLabel: '驳回原因'.t(),
                    labelWidth: 70,
                    anchor: '100%'
                }]
            },
            id: "AndonManageRejectCommand001",
            callback: function (btn) {
                if (btn == "确定".t()) {
                    var andonManage = view.getCurrent();
                    var andonManageId = andonManage.getId();
                    var reason = Ext.getCmp("AndonManageReject_Reason");
                    var data = {
                        AndonManageId: andonManageId,
                        OperateType: 5,
                        Reason: reason.value,
                    };
                    view.execute({
                        data: data,
                        success: function (res) {
                            SIE.Msg.showMessage("驳回成功!".t());
                            win.close();
                            if (view.viewGroup == 'LookUpViewGroup') { //查看界面逻辑
                                andonManage.setState(20);
                                andonManage.markSaved();
                                andonManage._OperateLogList.reload();
                                CRT.Event.fire(view.model + '_refresh');
                            }
                            else {
                                view.reloadData();
                            }
                        }
                    });
                }
            }
        });
    }
});
