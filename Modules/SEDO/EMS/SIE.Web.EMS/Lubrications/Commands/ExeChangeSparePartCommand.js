SIE.defineCommand('SIE.Web.EMS.Lubrications.Commands.ExeChangeSparePartCommand', {
    extend: 'SIE.cmd.Save',
    meta: { text: "更换", group: "edit", iconCls: "icon-Check" },
    canExecute: function (view) {
        var entity = view.getParent().getCurrent();
        if (entity == null) {
            return false;
        }
        return (entity.getLubricationStatus() == 10 || entity.getLubricationStatus() == 20);
    },
    execute: function (view) {
        var datas = [];
        Ext.each(view.getData().data.items, function (data) { datas.push(data.data); });
        view.execute({
            data: datas,
            success: function (res) {
                SIE.Msg.showInstantMessage('备件更换成功'.t());
                view.loadChildData(true);
            }
        });
    }
});
