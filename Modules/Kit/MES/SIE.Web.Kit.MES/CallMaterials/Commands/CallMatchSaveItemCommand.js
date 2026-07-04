SIE.defineCommand('SIE.Web.Kit.MES.CallMaterials.Commands.CallMatchSaveItemCommand', {
    extend: 'SIE.cmd.Save',
    meta: { text: "保存", group: "edit" },
    canExecute: function (view) { 
        var length = view.getData().data.items.length;
        var items = view.getData().data.items;
        if (length <= 0) { return false; }

        for (var i = 0; i < length; i++) {
            if (items[i].data.IsChange == 1)
                return true;
        }
        return false;
    },
    onSavedMsg: function (view, res) {
        SIE.Msg.showInstantMessage('操作成功'.t());
    },
    onSaved: function (view, res) {
        var me = this;
        var items = view.getData().data.items;
        for (var i = 0; i < length; i++) {
            items[i].data.IsChange = 0;
            items[i].markSaved();
        }
        
        this.callParent(arguments);
    },
});