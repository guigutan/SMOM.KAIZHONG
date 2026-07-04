SIE.defineCommand('SIE.Web.EMS.AssetDisposals.Commands.SaveAssetDisposalCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "保存", group: "edit" },
    canExecute: function (view) {
        if (view.getCurrent() == null) {
            return false;
        }
        return true;
    },
    onSaving: function (view, res) {
        var entity = view.getCurrent();

        var equipChildView = view.findChild('SIE.EMS.AssetDisposals.AssetDisposalEquipment');
        var fixtureChildView = view.findChild('SIE.EMS.AssetDisposals.AssetDisposalFixture');
        var equipChildStore = equipChildView.getData();
        var fixtureChildStore = fixtureChildView.getData();

        if (entity.data.AssetObject == null || entity.data.AssetObject == 0) {
            SIE.Msg.showError('资产对象不能为空！'.t());
            return false;
        }

        if (entity.data.AssetObject == 10) {
            fixtureChildStore.removeAll();
        }

        if (entity.data.AssetObject == 20) {
            equipChildStore.removeAll();
        }

        return this.callParent(arguments);
    },
    execute: function (view, source) {

        var me = this;
        var entity = view.getCurrent();
        var equipChildView = view.findChild('SIE.EMS.AssetDisposals.AssetDisposalEquipment');
        var fixtureChildView = view.findChild('SIE.EMS.AssetDisposals.AssetDisposalFixture');

        var store = entity.data.AssetObject == 10 ? equipChildView.getData() : fixtureChildView.getData();
        if (store.getCount() == 0) {
            SIE.Msg.showError(entity.data.AssetObject == 10 ? '设备清单不能为空！'.t() : '工治具清单不能为空！'.t());
            return false;
        }

        for (var i = 0; i < store.getCount(); i++) {
            var record = store.getAt(i);

            if (entity.data.AssetObject == 10 && record.data.EquipAccountId == null) {
                SIE.Msg.showError('设备清单中的设备编码不能为空！'.t());
                return false;
            }

            if (entity.data.AssetObject == 20) {

                if (record.data.FixtureEncodeId == null) {
                    SIE.Msg.showError('工治具清单中的工治具编码不能为空！'.t());
                    return false;
                }

                if (record.data.FixtureAccountId == null) {
                    SIE.Msg.showError('工治具清单中的序列号不能为空！'.t());
                    return false;
                }
            }
        }

        me.doSave(view);
    },
    onSaved: function (view, res) {
        var me = this;
        me.onSavedMsg();
        me.view.getCurrent().markSaved();
    },
    onSavedMsg: function (view, res) {

        Ext.Msg.show({
            title: '提示'.t(),
            message: '保存成功'.t(),
            buttons: Ext.MessageBox.OK,
            icon: Ext.Msg.INFO,
            callback: function () {
                CRT.Workbench.closeCurrentTab();
                CRT.Event.fire("SIE.EMS.AssetDisposals.AssetDisposal_refresh");
            }
        });
    }
});