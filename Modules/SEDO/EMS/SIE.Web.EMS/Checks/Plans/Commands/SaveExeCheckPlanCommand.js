SIE.defineCommand('SIE.Web.EMS.Checks.Plans.Commands.SaveExeCheckPlanCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "保存", group: "edit" },
    /**
     * @override 是否可执行
     * @param {} view 
     * @returns {} 
     */
    canExecute: function (view) {
        var current = view.getCurrent();
        return current &&
            current.isDirty() && (current.getExeState() == 0 || current.getExeState() == 4);
    },
    onSavedMsg: function (view, res) {
        //SIE.Msg.showInstantMessage('保存成功'.t());
    }
});