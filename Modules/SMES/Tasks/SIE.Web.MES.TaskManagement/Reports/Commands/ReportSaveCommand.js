SIE.defineCommand('SIE.Web.MES.TaskManagement.Reports.ReportSaveCommand', {
    extend: 'SIE.cmd.Command',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    canExecute: function (view) {
        return view._parent.getSelection().length == 1 && view._parent._current.data.TaskStatus == 30 && view._parent._current.data.ReportMode == 1;
        //已开工的记录才能保存
    },
    /*
   * @override 执行
   * @returns{}
   */
    execute: function (view, source) {
        var me = this;
        SIE.Web.MES.TaskManagement.Reports.ReportCommon.saveOrSubmmitReport(view, true, me.meta.command);
    }
});