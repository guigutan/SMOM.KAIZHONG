SIE.defineCommand('SIE.Web.ERPInterface.UploadTransactionRules.Commands.InitTransRuleCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "初始化", group: "business", iconCls: "icon-NetworkNormal icon-green" },

    canExecute: function (view) {
        return true;
    },
    execute: function (view, source) {
        SIE.Msg.askQuestion(Ext.String.format('确定初始化交易上传规则?'.t()), function () {
            view.execute({
                data: {},
                withIds: true,
                success: function (res) {
                    view.reloadData();
                }
            });
        });
    }
});