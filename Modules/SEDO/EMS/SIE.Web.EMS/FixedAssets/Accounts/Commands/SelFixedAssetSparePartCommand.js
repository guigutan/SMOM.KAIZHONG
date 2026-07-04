SIE.defineCommand('SIE.Web.EMS.FixedAssets.Accounts.Commands.SelFixedAssetSparePartCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    canExecute: function (view) {
        return true;
    },
    userConfig: {
        dataParams: {
            specKeyPrototyName: 'StoreSummaryDetailId',
            targetClassName: 'SIE.EMS.SpareParts.StoreSummaryDetail'
        }
    },
    execute: function (view, source) {
        var me = this;
        SIE.AutoUI.getMeta({
            model: me.dataParams.targetClassName,
            viewGroup: 'AddStoreSummaryDetailViewGroup',
            ignoreChild: true,
            ignoreCommands: true,
            isReadonly: true,
            ignoreQuery: false,
            isAggt: true,
            callback: function (res) {
                var blocks = res;
                me._queryBlockProcess(blocks);
                me._gridBlockProcess(blocks);
                var ui = SIE.AutoUI.generateAggtControl(blocks);

                me._popupWin(ui, source);
                me.cloneStore = view.getData();
                me._reloadTargetViewData();
            }
        });
    },
    save: function (win) {
        var me = this;
        var selections = this._targetView.getSelection();
        if (selections && selections.length > 0) {
            selections.forEach(function (sel) {
                if (me._sourceViewSelectItems.indexOf(sel.getId()) === -1) {
                    var data = me._ownerView.createNewItem();
                    data.setFixedAssetsAccountId(me.view._parent.getCurrent().data.Id);
                    data.setStoreSummaryDetailId(sel.data.Id);
                    data.setOrderNumberCode(sel.data.OrderNumberCode);
                    data.setSparePartCode(sel.data.SparePartCode);
                    data.setSparePartName(sel.data.SparePartName);
                    data.setState(sel.data.State);
                    data.setSpecification(sel.data.Specification);
                    data.setSpartType(sel.data.SpartType);
                    data.setUnitName(sel.data.UnitName);
                    data.setWarehouseName(sel.data.WarehouseName);
                    data.setStorageLocationName(sel.data.StorageLocationName);
                    data.setSupplierCode(sel.data.SupplierCode);
                    data.setSupplierName(sel.data.SupplierName);
                }
            });
            win.close();
        }
        me.view.mon(me._ownerView.getData(), 'propertyChanged', me.onEntityPropertyChanged, me.view);
    },
    onEntityPropertyChanged: function (e) {

        if (e.property == 'IsMajor') {
            if (e.entity.data.IsMajor) {
                for (var i = 0; i < e.entity.store.getCount(); i++) {
                    var record = e.entity.store.getAt(i);
                    if (e.entity.data.StoreSummaryDetailId != record.data.StoreSummaryDetailId) {
                        record.setIsMajor(false);
                    }
                }
            }
        }
    }
});