SIE.defineCommand('SIE.Web.EMS.FixedAssets.Accounts.Commands.SelEquipsCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    canExecute: function (view) {
        return true;
    },
    userConfig: {
        dataParams: {
            specKeyPrototyName: 'EquipAccountId',
            targetClassName: 'SIE.Equipments.EquipAccounts.EquipAccount'
        }
    },
    execute: function (view, source) {
        var me = this;
        SIE.AutoUI.getMeta({
            model: me.dataParams.targetClassName,
            viewGroup: 'ListView',
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
                    data.setEquipAccountId(sel.data.Id);
                    data.setCode(sel.data.Code);
                    data.setName(sel.data.Name);
                    data.setModelCode(sel.data.ModelCode);
                    data.setModelName(sel.data.ModelName);
                    data.setUseState(sel.data.UseState);
                    data.setFrozen(sel.data.Frozen);
                    data.setEquipTypeCode(sel.data.EquipTypeCode);
                    data.setEquipTypeName(sel.data.EquipTypeName);
                    data.setEquipTypeCategory(sel.data.EquipTypeCategory);
                    data.setIsCustomsSupervision(sel.data.IsCustomsSupervision);
                    data.setUseLevel(sel.data.UseLevel);
                    data.setUseDepartmentName(sel.data.UseDepartmentName);
                    data.setManufacturer(sel.data.Manufacturer);
                    data.setSupplierCode(sel.data.SupplierCode);
                    data.setSupplierName(sel.data.SupplierName);
                    data.setPurchaseOrderNo(sel.data.PurchaseOrderNo);
                    data.setEnterDate(sel.data.EnterDate);
                    data.setUsefulLife(sel.data.UsefulLife);
                    data.setWarrantyPeriod(sel.data.WarrantyPeriod);
                    data.setInstallationLocation(sel.data.InstallationLocation);
                    data.setResource(sel.data.ResourceName);
                    data.setProcess(sel.data.ProcessName);
                }
            });
            win.close();
        }
        me.view.mon(me._ownerView.getData(), 'propertyChanged', me.onEntityPropertyChanged, me.view);
    },
    onLoad: function (store, records, successful, operation, eOpts) {
        var me = this;
        var entitys = store.data.items.where(function (p) { return p.getAssetCode() == "" });
        store.setData(entitys);
        this.callParent(arguments);
    },
    onEntityPropertyChanged: function (e) {

        if (e.property == 'IsMajor') {

            if (e.entity.data.IsMajor) {

                for (var i = 0; i < e.entity.store.getCount(); i++) {
                    var record = e.entity.store.getAt(i);

                    if (e.entity.data.EquipAccountCode != record.data.EquipAccountCode) {
                        record.setIsMajor(false);
                    }
                }
            }
        }
    }
});