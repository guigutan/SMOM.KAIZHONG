SIE.defineCommand('SIE.Web.WorkBenchCommon.Workbench.TargetWarn.Commands.SaveTargetWarnSettingCommand', {
    extend: 'SIE.cmd.FormSave',
    executeIntervalMode: SIE.cmd.IntervalMode.Debounce.value,//使用防抖模式
    meta: { text: "保存", group: "edit", iconCls: "icon-EditEntity icon-blue" },
});