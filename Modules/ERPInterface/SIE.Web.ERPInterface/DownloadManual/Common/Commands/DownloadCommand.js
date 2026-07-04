SIE.defineCommand('SIE.Web.ERPInterface.DownloadManual.Commands.DownloadCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "手动下载", group: "business", iconCls: "iconfont icon-Download icon-green" },
    mainMeta: null,
    ui: null,
    canExecute: function (view) {
        return true;
    },
    execute: function (view, source) {
        SIE.Msg.wait("正在下载数据，请稍候...".L10N());
        view.execute({
            data: "1",
            success: function (res) {
                SIE.Msg.hide();
                SIE.Msg.showMessage(res.Result);
                view.getParent().reloadData();
            },
            error: function (res) {
                //遇到逻辑捕捉不到的异常触发，平台封装错误信息弹出逻辑，此回调用于关闭滚动条
                SIE.Msg.hide();
            }
        });
    }
});