SIE.defineCommand('SIE.Web.FMS.FileManage.Commands.SetAdminCommand', {
    meta: { text: "设为管理员", group: "edit", isOnlyAdmin: true, iconCls: "icon-PeopleSetting icon-blue" },
    canExecute: function (view) {
        var p = view.getCurrent();
        if (p == null) { return false; }
        if (view.getSelection().length !== 1) { return false; }
        if (p.data.IsAdmin == true) { return false; }
        if (p.isNew()) { return false; }
        return true;
    },
    execute: function (view, source) {
        var me = view;
        var entity = view.getCurrent().data;
        var indata = {};
        indata.Data = Ext.encode(entity);
        view.execute({
            data: indata,
            success: function (res) {
                me.reloadData();
            }
        });
    }
});