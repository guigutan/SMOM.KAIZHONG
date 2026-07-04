SIE.defineCommand('SIE.Web.Core.UserAgreements.Commands.EnableAgreementCommand', {
    meta: { text: "启用", group: "edit", iconCls: "icon-Import icon-blue" },
    canExecute: function (view) {
        return true;
    },

    /**
     * 执行
     * @param {any} view
     * @param {any} entity
     */
    execute: function (view,entity) {
        var agreementId = entity.data.Id;
        view.execute({
            ParentType:"SIE.Core.UserAgreements.UserAgreement",
            command: "SIE.Web.Core.UserAgreements.Commands.EnableAgreementCommand",
            data: agreementId,
            success: function (res) {
                var control = view.routingControl;
                control.loadData(); //刷新数据
            },
            model: "SIE.Core.UserAgreements.UserAgreement"
        });
    }
});