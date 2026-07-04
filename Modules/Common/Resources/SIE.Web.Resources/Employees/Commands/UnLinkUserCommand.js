SIE.defineCommand('SIE.Web.Resources.Employees.Commands.UnLinkUserCommand', {
    meta: { text: "解除关联", group: "edit", iconCls: "icon-LinkVariantOff icon-blue" },

    canExecute: function (view) {
        return view.getCurrent() !== null && view.getSelection().length == 1 && view.getCurrent().data.UserId !== null;
    },

    execute: function (view, source) {
        var entity = view.getCurrent();
        if (entity) {
            entity = entity.data;
            if (entity.Id == CRT.Context.GlobalContext.getContext('userInfo').EmployeeId) {
                SIE.Msg.showMessage('无法取消关联当前账号！'.t());
                return;
            }
            var msg = Ext.String.format('确认解除 ({0}){1}与用户{2} 的关联？'.t(), entity.Code, entity.Name, entity.UserId_Display);
            SIE.Msg.askQuestion(msg, function () {
                view.execute({
                    data: entity,
                    success: function (res) {
                        view.loadData();
                    }
                }, view);
            });
        }
    }
});