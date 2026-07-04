SIE.defineCommand('SIE.Web.AbnormalInfo.AbnormalInfos.Commands.SaveAbnormalInfoCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    /**
     * 是否可执行
     * @param {any} view
     */
    canExecute: function (view) {
        var current = view.getCurrent();
        if (!current || current.getAbnormalStatus() == SIE.AbnormalInfo.AbnormalInfos.AbnormalStatus.Close)
            return false;
        return this.callParent(arguments);
    },
    /**
     * @override 保存后处理
     * @param {any} view
     * @param {any} res
     */
    onSaved: function (view, res) {
        var me = this;
        var current = view.getCurrent();
        CRT.Event.fire(view.model + "_refresh", current.data.Id);
        current.setAbnormalStatus(SIE.AbnormalInfo.AbnormalInfos.AbnormalStatus.Processing);
        current.markSaved();
        view.syncCmdState();
        me.onSavedMsg(view, res);
    }
});