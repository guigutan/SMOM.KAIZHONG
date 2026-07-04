SIE.defineCommand("SIE.Web.EMS.EquipLends.Commands.EquipLendReturnCommand", {
    meta: { text: "归还".t(), group: "edit", iconCls: "iconfont icon-NetworkNormal icon-blue" },
    canExecute: function (view) {
        var sel = view.getSelection();
        if (sel == null || sel.length <= 0) {
            return false;
        }
        var flag = true;
        for (var i = 0; i < sel.length; i++) {
            var data = sel[i].getData();
            if (data.LendState != 2) { // 只有状态为已借出才能归还
                flag = false;
                break;
            }
            var dif = sel.find(item => { return item.getLendEmployeeId() !== data.LendEmployeeId }); // 借出人要一致
            if (dif != null) {
                flag = false;
                break;
            }
        }
        return flag;
    },
    execute: function (view) {
        var me = this;
        me.showWin(view);
        
    },
    showWin: function (view) {
        var win = SIE.Window.show({
            title: "归还".t(),
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
                    id: 'EquipLendManageReturn_Remark',
                    name: 'EquipLendManageReturnRemark',
                    margin: '5',
                    fieldLabel: '归还说明'.t(),
                    anchor: '100%'
                }]
            },
            id: "EquipLendManageReturnExamine_001",
            callback: function (btn) {
                if (btn == "确定".t()) {
                    var selectIds = view.getSelectionIds();
                    var remark = Ext.getCmp('EquipLendManageReturn_Remark');
                    var data = {
                        Remark: remark.value
                    }
                    view.execute({
                        data: data,
                        withIds: true,
                        selectIds: selectIds,
                        success: function (res) {
                            SIE.Msg.showInstantMessage("归还成功!".t());
                            win.close();
                            view.reloadData();
                        }
                    });
                }
            }
        });
    }
})
