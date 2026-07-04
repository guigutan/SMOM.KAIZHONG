SIE.defineCommand('SIE.Web.MES.WorkReportPlans.Commands.InitializationCommand', {
    meta: { text: "初始化", group: "edit", iconCls: "icon-ArrowWithCircleDown icon-green" },
    canExecute: function (view) {
        return true;
    },
    execute: function (view, source) {
        view.execute({
            data: {},
            success: function (res) {
                SIE.Msg.showMessage("初始化成功!".t());
                view.reloadData();
            },
            error: function (res) {
                SIE.Msg.showError(res.Message);
            }
        });
    }
});