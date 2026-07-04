SIE.defineCommand('SIE.Web.MES.TeamManagement.ClockingIns.Commands.MachineSaveCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    canExecute: function (view) {
        return view.getCurrent() != null;//&& view.getCurrent().data.Model != null && view.getCurrent().data.Model != "";
    }
});