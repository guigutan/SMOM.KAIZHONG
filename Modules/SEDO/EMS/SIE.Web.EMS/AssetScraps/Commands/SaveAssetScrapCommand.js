SIE.defineCommand('SIE.Web.EMS.AssetScraps.Commands.SaveAssetScrapCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "保存", group: "edit" },
    onSaving: function (view, res) {
        var entity = view.getCurrent();

        var equipChildView = view.findChild('SIE.EMS.AssetScraps.AssetScrapEquipment');
        var fixtureChildView = view.findChild('SIE.EMS.AssetScraps.AssetScrapFixture');
        var equipChildStore = equipChildView.getData();
        var fixtureChildStore = fixtureChildView.getData();

        var amountList = [];
        var amount = 0;
        if (entity.data.AssetObject == 10) {

            fixtureChildStore.removeAll();
            amountList = equipChildStore.getData().items.where(function (p) { return !Ext.isEmpty(p.getScrapNetValue()); })
                .select(function (p) { return parseFloat(p.getScrapNetValue()); });
            amount = amountList.sum();
            entity.setAmount(amount);
        }

        if (entity.data.AssetObject == 20) {

            equipChildStore.removeAll();
            amountList = fixtureChildStore.getData().items.where(function (p) { return !Ext.isEmpty(p.getScrapNetValue()); })
                .select(function (p) { return parseFloat(p.getScrapNetValue()); });
            amount = amountList.sum();
            entity.setAmount(amount);
        }

        return this.callParent(arguments);
    },
    execute: function (view, source) {

        var me = this;
        var entity = view.getCurrent();
        var equipChildView = view.findChild('SIE.EMS.AssetScraps.AssetScrapEquipment');
        var fixtureChildView = view.findChild('SIE.EMS.AssetScraps.AssetScrapFixture');

        var store = entity.data.AssetObject == 10 ? equipChildView.getData() : fixtureChildView.getData();
        if (store.getCount() == 0) {
            SIE.Msg.showError(entity.data.AssetObject == 10 ? '设备清单不能为空！'.t() : '工治具清单不能为空！'.t());
            return false;
        }
        if (entity.data.AssetObject == 20) {
            var flag = true;
            for (var i = 0; i < store.getData().items.length; i++) {
                if (store.getData().items[i].getQty() == null) {
                    SIE.Msg.showError('工治具报废数最小为1'.t());
                    flag = false;
                    return;
                }
            }
            if (!flag) {
                return;
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
                CRT.Event.fire("SIE.EMS.AssetScraps.AssetScrap_refresh");
            }
        });
    }
});