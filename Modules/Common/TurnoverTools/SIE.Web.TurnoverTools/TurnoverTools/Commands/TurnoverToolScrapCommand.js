SIE.defineCommand('SIE.Web.MES.TurnoverTools.Commands.TurnoverToolScrapCommand', {
    meta: { text: "报废", group: "edit", hierarchy: "状态", iconCls: "icon-NetworkError icon-blue" },
    extend: 'SIE.Web.MES.TurnoverTools.Commands.TurnoverToolRecoveryCommand',
    canExecute: function (listView) {
        var current = listView.getCurrent();
        if (current == null)
            return false;
        else return current.data.State == 5;
    }
});