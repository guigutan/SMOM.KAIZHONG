SIE.defineCommand('SIE.Web.EMS.Lubrications.Commands.LubricationDetailSaveCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },

    /**
     * @override 是否可执行
     * @param {} view 
     * @returns {} 
     */
    canExecute: function (view) {
        var current = view.getCurrent();
        if (current) {
            var LubricationStatus = current.getLubricationStatus();
            if ((LubricationStatus === SIE.EMS.Enums.LubricationStatus.Pending.value
                || LubricationStatus === SIE.EMS.Enums.LubricationStatus.Doing.value)
                && current.isDirty()
            ) {
                return true;
            } else {
                return false;
            }
        }
        return this.callParent(arguments);
    },

    /**
     * @override
     * @param {} view 
     * @returns {} 
     */
    onSaved: function (view, res) {
        var me = this;
        var ent = view.getCurrent();
        //数据已保存到服务器
        ent.markSaved();
        CRT.Event.fire(view.model + "_refresh", ent.data.Id);
        me.onSavedMsg(view, res);
        view.syncCmdState();
    }
});