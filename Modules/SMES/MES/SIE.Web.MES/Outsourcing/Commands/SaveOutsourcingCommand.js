SIE.defineCommand('SIE.Web.MES.Outsourcing.Commands.SaveOutsourcingCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    canExecute: function (view) {
        var result = false;
        var datas = view.getData();
        if (datas.data) {
            for (var i = 0; i < datas.data.items.length; i++) {
                if (datas.data.items[i].dirty && datas.data.items[i].getOutsourcingState() == 10) {
                    result = true;
                    return result;
                }
            }
        } else {
            return false;
        }
        return result;
    }
});