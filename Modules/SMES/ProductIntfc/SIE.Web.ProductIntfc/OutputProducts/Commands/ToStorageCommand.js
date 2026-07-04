SIE.defineCommand('SIE.Web.ProductIntfc.OutputProducts.Commands.ToStorageCommand', {
    meta: { text: "入库", group: "edit", iconCls: "icon-WarehouseImport icon-blue" },
    extend: 'SIE.cmd.Save',
    canExecute: function (view) {
        var selectItems = view.getSelectedEntities();
        var successful = selectItems.length > 0;

        for (i = 0; i < view.getSelectedEntities().length; i++) {
            var label = view.getSelectedEntities()[i].data;
            if (label.InStorageState == 20) {
                return false;
            }
        }
        return successful;
    },
    execute: function (view, source) {
        var me = this;
        var indata = {};
        var entity = view.getSelectedEntities().map(c => c.data);
        indata.Data = Ext.encode(entity);
        SIE.Msg.wait("正在入库......".t());
        view.execute({
            data: entity,
            success: function (res) {
                me.onSaved(view, res);
                view.reloadData();
            }
        });
    },
    /**
     * @override
     * @param {} view 
     * @returns {} 
     */
    onSaved: function (view, res) {
        var me = this;
        me.onSavedMsg(view, res);
        for (i = 0; i < view.getSelectedEntities().length; i++) {
            view.getSelectedEntities()[i].markSaved();
        }
        view.syncCmdState();
        CRT.Event.fire(view.model + '_refresh', view.getCurrent().getId());
    },
    onSavedMsg: function (view, res) {
        SIE.Msg.showInstantMessage(res.Result.t());
    },
});