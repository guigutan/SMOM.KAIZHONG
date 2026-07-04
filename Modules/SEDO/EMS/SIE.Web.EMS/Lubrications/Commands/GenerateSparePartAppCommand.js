SIE.defineCommand('SIE.Web.EMS.Lubrications.Commands.GenerateSparePartAppCommand', {
    meta: { text: "申请", group: "edit", iconCls: "icon-Check" },
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
                SIE.Msg.showInstantMessage('备件申请成功'.t());
                view.loadChildData(true);
            }
        });
    }
});
