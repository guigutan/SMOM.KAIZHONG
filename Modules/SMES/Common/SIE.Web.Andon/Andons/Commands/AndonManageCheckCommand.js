SIE.defineCommand('SIE.Web.Andon.Andons.Commands.AndonManageCheckCommand', {
    meta: { text: "验收", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
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
        var andonManage = view.getCurrent();
        var nowDate = new Date();
        var defaultHour = ((nowDate - andonManage.data.TriggerTime) / 1000 / 60 / 60).toFixed(1);
        var win = SIE.Window.show({
            title: "实际影响时间（小时）".t(),
            width: 360,
            height: 140,
            items: {
                layout: {
                    type: "vbox",
                    align: "stretch",
                },
                items: [{
                    xtype: 'numberfield',
                    grow: true,
                    id: "Actual_Tiem",
                    name: "Actual_TiemValue",
                    margin: "15",
                    labelWidth: 20,
                    value: defaultHour,
                    minValue: '0',
                    step:0.1,
                    decimalPrecision: 1,
                    allowDecimals: true,
                    minText:'实际影响时间不能小于0！'.t(),
                }]
            },
            id: "AndonManageCheckCommand001",
            callback: function (btn) {
                if (btn == '确定'.t()) {
                    var andonManageId = andonManage.getId();
                    var actualTime = Ext.getCmp('Actual_Tiem');
                    if (actualTime.value < 0) {
                        SIE.Msg.showError("实际影响时间不能小于0！".t());
                        return;
                    }
                    var data = {
                        AndonManageId: andonManageId,
                        OperateType: 4,
                        ActualTime: actualTime.value,
                    }
                    view.execute({
                        data: data,
                        success: function (res) {
                            SIE.Msg.showMessage("验收成功!".t());
                            win.close();
                            if (view.viewGroup == 'LookUpViewGroup') { //查看界面逻辑
                                andonManage.setState(40);
                                andonManage.setCloseTime(nowDate);
                                andonManage.setLastTime(defaultHour);
                                andonManage.setActualTime(actualTime.value);
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
