SIE.defineCommand('SIE.Web.AbnormalInfo.AbnormalMonitors.Commands.SaveProcessCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    /**
     * @override 是否可执行
     * @param {} view 
     * @returns {} 
     */
    canExecute: function (view) {
        return view.getCurrent() &&
            view.getCurrent().getTaskState() != SIE.AbnormalInfo.Common.TaskStateEnum.Done &&
            view.getCurrent().isDirty();
    },
    /**
     * @override
     * @param {} view 
     * @returns {} 
     */
    onSaved: function (view, res) {
        var me = this;
        var ent = view.getCurrent();

        if (ent.getTaskState() === SIE.AbnormalInfo.Common.TaskStateEnum.ToDo) {
            ent.setTaskState(SIE.AbnormalInfo.Common.TaskStateEnum.Doing); //改为处理中
        }
        //数据已保存到服务器，修改状态
        SIE.Web.Core.CommonFuns.markViewSaved(view);
        me.onSavedMsg(view, res);
        view.syncCmdState();
        CRT.Event.fire(view.model + '_refresh', view.getCurrent().getId());
    },
});