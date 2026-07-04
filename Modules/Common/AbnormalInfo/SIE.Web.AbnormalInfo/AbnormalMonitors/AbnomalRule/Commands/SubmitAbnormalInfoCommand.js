SIE.defineCommand('SIE.Web.AbnormalInfo.AbnormalMonitors.AbnomalRule.Commands.SubmitAbnormalRuleCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "提交", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    /**
     * 是否可执行
     * @param {any} view
     */
    canExecute: function (view) {
        return true;
    },
    /**
     * @override 执行
     * @param {any} view
     * @param {any} source
     */
    execute: function (view, source) {
        var me = this;
        SIE.Msg.askQuestion("是否提交？提交后信息不能修改。".t(),
            function () {
                //提交时，数据设置为脏，重新保存并校验所有内容。
                view.getCurrent().dirty = true;
                me.doSave(view);
            });
    },

    /**
     * @override    保存后处理
     * @param {any} view
     * @param {any} res
     */
    onSaved: function (view, res) {
        var me = this;
        var current = view.getCurrent();
        current.setAbnormalStatus(SIE.AbnormalInfo.AbnormalInfos.AbnormalStatus.Close);
        current.markSaved();
        view.setIsReadonly(true);
        CRT.Event.fire(view.model + "_refresh", view.getCurrent().data.Id);
        me.onSavedMsg(view, res);
    },

    /**
    * override 重写保存后提示信息
    * @param {type} view
    * @param {type} res
    */
    onSavedMsg: function (view, res) {
        SIE.Msg.showInstantMessage('提交成功！');
    }
});