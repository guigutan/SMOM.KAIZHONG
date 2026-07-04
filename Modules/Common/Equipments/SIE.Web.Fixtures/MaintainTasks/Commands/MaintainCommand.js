SIE.defineCommand('SIE.Web.Fixtures.MaintainTasks.Commands.MaintainCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "保养", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    canExecute: function (view) {
        var isCanExecute = this.callParent(arguments);
        if (isCanExecute)
            return view.getCurrent().getState() !== 15;
        return isCanExecute;
    }
});