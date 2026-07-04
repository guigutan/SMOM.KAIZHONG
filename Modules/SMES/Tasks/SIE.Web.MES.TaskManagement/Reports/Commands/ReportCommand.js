SIE.defineCommand('SIE.Web.MES.TaskManagement.Reports.ReportCommand', {
    extend: 'SIE.cmd.Command',
    meta: { text: "报工", group: "edit", iconCls: "icon-ArrowWithCircleDown icon-green" },
    executeIntervalMode: SIE.cmd.IntervalMode.Debounce.value,
    canExecute: function (view) {        
        return view._parent.getSelection().length == 1 && view._parent._current.data.TaskStatus == 30 && view._parent._current.data.ReportMode == 1;
    },
    /*
   * @override 执行
   * @returns{}
   */
    execute: function (view, source) {
        var me = this;
        SIE.Msg.wait("正在报工......".t());
        SIE.Web.MES.TaskManagement.Reports.ReportCommon.saveOrSubmmitReport(view, true, me.meta.command);
    },
});