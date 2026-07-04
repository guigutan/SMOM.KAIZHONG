SIE.defineCommand('SIE.Web.MES.Outsourcing.Commands.InstockEditCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },

    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length !== 1) {
            return false;
        }

        var p = view.getCurrent();

        if (p == null) {
            return false;
        }

        // NotStarted = 10, 未开始 才能修改
        return (p.getState() === 10);
    },
});