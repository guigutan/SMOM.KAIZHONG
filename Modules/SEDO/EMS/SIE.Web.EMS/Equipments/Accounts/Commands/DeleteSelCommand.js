SIE.defineCommand('SIE.Web.EMS.Equipments.Accounts.Commands.DeleteSelCommand', {
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    canExecute: function (view) {
        return view.getCurrent();
    },
    execute: function (view, source) {
        var sels = view.getSelection();
        Ext.each(sels, function (item) {
            view.getData().remove(item);
        });
    }
});
