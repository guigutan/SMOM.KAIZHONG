SIE.defineCommand('SIE.Web.EMS.ViceTransfers.Commands.ExecutSaveComamns', {
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    extend: 'SIE.cmd.Save',

    canExecute: function (view) {
        var entity = view.getCurrent();
        var res = false;
        if (entity.data.ViceAssetObject == 10)//备件
        {
            var partChildView = view.findChild('SIE.EMS.ViceTransfers.ViceTransferSparePartDetail');
            if (partChildView != null) {
                var partChildViewStores = partChildView.getSelectedEntities();
                res = partChildViewStores.length > 0;
            }
        }
        if (entity.data.ViceAssetObject == 20)//工治具
        {
            var fixtureChildView = view.findChild('SIE.EMS.ViceTransfers.ViceTransferFixtureDetail');
            if (fixtureChildView != null) {
                var fixtureChildViewStores = fixtureChildView.getSelectedEntities();
                res = fixtureChildViewStores.length > 0;
            }
        }
        return view.getCurrent().isDirty() && res;
    },
    execute: function (view, source) {
        var me = this;
        var entity = view.getCurrent();
        entity.dirty = true;
        var viceTransferPartDetailList = [];
        if (entity.data.ViceAssetObject == 10)//备件
        {
            var partChildView = view.findChild('SIE.EMS.ViceTransfers.ViceTransferSparePartDetail');

            var partChildViewStores = partChildView.getSelectedEntities();
            partChildViewStores.forEach(it => {
                viceTransferPartDetailList.push(it.getData());
            });
        }
        var viceTransferFixtureDetailList = [];
        if (entity.data.ViceAssetObject == 20)//工治具
        {
            var fixtureChildView = view.findChild('SIE.EMS.ViceTransfers.ViceTransferFixtureDetail');
            var fixtureChildViewStores = fixtureChildView.getSelectedEntities();
            fixtureChildViewStores.forEach(it => {
                viceTransferFixtureDetailList.push(it.getData());
            });
        }


        //单独保存属性值
        SIE.invokeDataQuery({
            method: 'ExecutSave',
            params: [entity.data, viceTransferFixtureDetailList, viceTransferPartDetailList],
            action: 'queryer',
            type: 'SIE.Web.EMS.ViceTransfers.ViceTransfersDataQueryer',
            token: view.token,
            success: function (res) {
                view.getCurrent().markSaved();
                Ext.Msg.show({
                    title: '提示'.t(),
                    message: '保存成功'.t(),
                    buttons: Ext.MessageBox.OK,
                    icon: Ext.Msg.INFO,
                    callback: function () {

                        CRT.Workbench.closeCurrentTab();
                        CRT.Event.fire("SIE.EMS.ViceTransfers.ViceTransfer_refresh");
                    }
                });
            }
        });
    }
});
