SIE.defineCommand('SIE.Web.EMS.SpareParts.Applys.Commands.SaveAppCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    canExecute: function (view) {
        return view.getData() && view.getData().isDirty();
    }
});