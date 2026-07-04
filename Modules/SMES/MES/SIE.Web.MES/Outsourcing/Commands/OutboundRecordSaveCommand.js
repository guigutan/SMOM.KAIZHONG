SIE.defineCommand('SIE.Web.MES.Outsourcing.Commands.OutboundRecordSaveCommand', {
    extend: 'SIE.cmd.Save',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    canExecute: function (view) {
        var result = false;
        var datas = view.getData();
        if (datas.data) {
            for (var i = 0; i < datas.data.items.length; i++) {
                if (datas.data.items[i].dirty && datas.data.items[i].getSN() == "" && datas.data.items[i].getLotNo() == "") {
                    result = true;
                    return result;
                }
            }
        } else {
            return false;
        }
        return result;
    },
    onSaved: function (view, res) {
        var me = this;
        SIE.Msg.showInstantMessage('保存成功'.t());
        view.syncCmdState();
        CRT.Event.fire(view.model + '_refresh', view.getCurrent().getId());
        view._parent.reloadData();
    },
    

});