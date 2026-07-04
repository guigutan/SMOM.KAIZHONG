SIE.defineCommand('SIE.Web.Andon.Andons.Commands.AndonSaveCommand', {
    extend: 'SIE.cmd.Save',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    onValidation: function (view) {
        if (view.getCurrent() && view.getCurrent().data.PushPlugId == null) {
            var canexecute = true;
            var messageSendList = view.getCurrent()._MessageSendList.data.items;
            //messageSendList.forEach(function (item) {
            //    if (item.getPushPlugId() == null) {
            //        SIE.Msg.showError('主表的推送模块为空时,消息推送子表的推送模块字段必输！'.t());
            //        canexecute =  false;

            //    }
            //});
            return canexecute;
        }
        else {
            return true;
        }
    }
});
