SIE.defineCommand('SIE.Web.Fixtures.Models.Commands.SyncFixtureEncodeCommand', {
    meta: { text: "同步", group: "edit", iconCls: "iconfont icon-SyncCode icon-blue" },
    canExecute: function (view) {
        var selection = view.getSelection();
        if (selection == null || selection.length < 1 || selection.any(function (p) { return p.isNew() })) {
            return false;
        }
        return true;
    },
    execute: function (view, source) {
        var me = this;
        var win = SIE.Window.show({
            title: '同步'.t(),
            autoScroll: false,
            layout: {
                type: 'table',
                columns: 2,
            },
            items: [
                {
                    colspan: 2,
                    xtype: 'tbtext',
                    text: Ext.String.format('您已选择了{0}行数据进行同步，请确定同步方式'.L10N(),view.getSelection().length)
                },
                {
                    xtype: 'button',
                    id: "compatibleSyncButton",
                    text: '兼容同步'.t(),
                    enableToggle: true,
                    toggleGroup: "同步".t(),
                    width: 200,
                    height: 50,
                    margin: '30 0 30 60',
                    colspan: 1
                }, {
                    xtype: 'button',
                    id: "coverSyncButton",
                    text: '覆盖同步'.t(),
                    enableToggle: true,
                    toggleGroup: "同步".t(),
                    width: 200,
                    height: 50,
                    margin: '30 0 30 60',
                    colspan: 1
                },
                {
                    xtype: 'tbtext',
                    colspan: 2,
                    text: '兼容同步：同步【工治具型号】中的设置，同时保留【工治具编码】的设置。即取【工治具型号】和【工治具编码】并集；'.t()
                },
                {
                    xtype: 'tbtext',
                    colspan: 2,
                    text: '覆盖同步：将【工治具型号】中的设置覆盖【工治具编码】的设置；'.t()
                }
            ],
            buttons: ["确定".t(), "取消".t()],
            callback: function (btn) {
                if (btn == "确定".t()) {
                    var compatibleSyncButton = Ext.getCmp('compatibleSyncButton');
                    var coverSyncButton = Ext.getCmp('coverSyncButton');
                    if (compatibleSyncButton.pressed == false && coverSyncButton.pressed == false) {
                        Ext.Msg.show({ title: '提示'.t(), message: '请确定同步方式'.t() });
                        return false;
                    }
                    if (compatibleSyncButton.pressed == true)
                        me.syncFixtureEncode(view, true);
                    else
                        me.syncFixtureEncode(view, false);
                }
                if (btn == "取消".t()) {
                    return true;
                }
            }
        });
    },
    /* 同步 */
    syncFixtureEncode: function (view, isCompatible) {
        var me = this;
        var indata = {};
        var listData = [];
        Ext.each(view.getSelection(), function (item) {
            listData.push(item.getId());
        });
        indata.Data = Ext.encode({
            IsCompatible: isCompatible,
            FixtureEncodeIdList: listData
        });
        view.execute({
            data: indata,
            success: function (res) {
                Ext.Msg.alert('提示'.t(), "同步成功！".t());
                view.refreshData();
            }
        });
    }
});