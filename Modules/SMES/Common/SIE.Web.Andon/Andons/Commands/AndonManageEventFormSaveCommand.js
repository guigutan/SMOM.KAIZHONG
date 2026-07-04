SIE.defineCommand('SIE.Web.Andon.Andons.Commands.AndonManageEventFormSaveCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    execute: function (view, source) {
        var me = this;
        var entity = view.getCurrent();
        if (entity.getExperienceFlag()) {
            if (entity.getReason() == "") {
                SIE.Msg.showError('经验库标识字段为【是】,事件原因不能为空'.t());
                return false;
            }
            if (entity.getHandleMethod() == "") {
                SIE.Msg.showError('经验库标识字段为【是】,处理方式不能为空'.t());
                return false;
            }
            if (entity.getMeasures() == "") {
                SIE.Msg.showError('经验库标识字段为【是】,预防措施不能为空'.t());
                return false;
            }
        }
        me.doSave(view);
    },
});
