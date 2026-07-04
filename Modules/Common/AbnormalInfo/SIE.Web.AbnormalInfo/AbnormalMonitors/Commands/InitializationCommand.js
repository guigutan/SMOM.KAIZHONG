SIE.defineCommand('SIE.Web.AbnormalInfo.AbnormalMonitors.Commands.InitializationCommand', {    meta: { text: "初始化", group: "edit", iconCls: "icon-Refresh icon-green" },    executeIntervalMode: SIE.cmd.IntervalMode.Debounce.value,//使用防抖模式    canExecute: function (view) {        return true;    },    execute: function (view) {        SIE.Msg.askQuestion("是否对异常来源进行初始化".L10N(),            function () {
                SIE.Msg.wait("正在初始化，请稍候...".L10N());
                SIE.invokeDataQuery({                    type: "SIE.Web.AbnormalInfo.AbnormalMonitors.DataQuerys.AnomalyMonitorQueryer",                    method: "Initialization",                    async: false,                    token: view.token,                    callback: function (res) {                        SIE.Msg.hide();                        if (res.Success) {                            if (res.Result) {
                                SIE.Msg.showInstantMessage("初始化完成".t());                            } else {
                                SIE.Msg.showWarning("初始化错误！".t());
                            }                        } else {
                            SIE.Msg.showError(res.Message);
                        }                    }                });
            },        );    }});