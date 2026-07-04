SIE.defineCommand('SIE.Web.EMS.Equipments.AlarmStates.Commands.AlarmStateCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "关闭", group: "edit", iconCls: "icon-TextRelease icon-blue" },
    canExecute: function (view) {
        if (view.getCurrent() == null) {
            return false;
        }
        var state = view.getCurrent().getAlarmState();
        if (state != 1) {
            return false;
        }
        return true;
    },
    /**
 * @override 
 * @returns {} 
 */
    execute: function (view, source) {
        var me = this;
        if (!this.onSaving(view))
            return false;
        SIE.Msg.askQuestion("是否关闭。".t(),
            function () {
                //if (view.associateCmd) {
                //    operationView = view.associateCmd.view;
                //}
                //提交时，数据设置为脏，重新保存并校验所有内容,包括明细。
                view.getCurrent().dirty = true;
                me.doSave(view);
            });
    },

    /**
    * override 保存后提示信息
    * @param {type} view
    * @param {type} res
    */
    onSavedMsg: function (view, res) {
        SIE.Msg.showInstantMessage('关闭成功！'.t());
    }
});