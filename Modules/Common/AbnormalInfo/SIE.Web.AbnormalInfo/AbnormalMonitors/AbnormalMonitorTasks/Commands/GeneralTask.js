SIE.defineCommand('SIE.Web.AbnormalInfo.AbnormalMonitors.Commands.GeneralTask', {
    extend: 'SIE.cmd.Save',
    meta: { text: "生成任务", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    /**
     * @override 是否可执行
     * @param {} view 
     * @returns {} 
     */
    canExecute: function (view) {
        return view.getCurrent() 
    },

    execute: function (view, source) {
        var me = this;
        var indata = {};
        var entity = view.getSelectedEntities().map(c => c.data);
        indata.Data = Ext.encode(entity);
        view.execute({
            data: entity,
            success: function (res) {
                me.onSaved(view,res);
            }
        });
    },
    /**
     * @override
     * @param {} view 
     * @returns {} 
     */
    onSaved: function (view, res) {
        var me = this;
        SIE.Msg.showInstantMessage('生成成功'.t());
        view.syncCmdState();
        CRT.Event.fire(view.model + '_refresh', view.getCurrent().getId());
    },
});