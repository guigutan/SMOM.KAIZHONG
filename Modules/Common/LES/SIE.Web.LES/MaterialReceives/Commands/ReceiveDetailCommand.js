SIE.defineCommand("SIE.Web.LES.MaterialReceives.Commands.ReceiveDetailCommand", {
    meta: { text: "接收", group: "edit", iconCls: "icon-Check icon-blue" },
    canExecute: function (view) {
        var sel = view.getSelection();
        if (sel == null || sel.length == 0) {
            return false;
        }
        return true;
    },
    
    execute: function (view) {
        var me = this;
        var msg = me.validationOverReceive(view);
        if (msg.length > 0) {
            SIE.Msg.askQuestion(msg + '; \r\n确认要继教接收吗?'.t(), function () {
                me.receive(view);
            });
        } else {
            SIE.Msg.askQuestion('确认要接收选中的数据吗?'.t(), function () {
                me.receive(view);
            });
        }
    },
    //接收
    receive(view) {

        SIE.Msg.wait("正在执行......".t());
        var indata = view.getSelection().map(p => p.data);
        view.execute({
            withIds: true,
            data: indata,
            success: function (res) { //回调
                SIE.Msg.showInstantMessage("接收成功".t());
                view.reloadData();
            }
        });
    },
    //校验备料单超收
    validationOverReceive(view) {
        var msg = "";
        var indata = view.getSelection().map(p => p.data);
        SIE.invokeDataQuery({
            type: "SIE.Web.LES.MaterialReceives.DataQueryer.MaterialReceiveQueryer",
            method: "ValidationOverReceiveDetail",
            params: [indata],
            async: false,
            token: view.token,
            callback: function (res) {
                if (res.Success) {
                    msg = res.Result;
                }
            }
        });
        return msg;
    }
})