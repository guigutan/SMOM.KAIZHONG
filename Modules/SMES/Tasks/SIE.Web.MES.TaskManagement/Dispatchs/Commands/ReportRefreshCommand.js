SIE.defineCommand('SIE.Web.MES.TaskManagement.Dispatchs.Commands.ReportRefreshCommand', {
    extend: 'SIE.cmd.Command',
    meta: { text: "时效重算", group: "edit", iconCls: "icon-Reload icon-green" },
    canExecute: function (listView) {
        return listView._parent.getCurrent() != null;
    },
    /*
   * @override 执行
   * @returns{}
   */
    execute: function (view, source) {
        view.reloadData()
    },
});