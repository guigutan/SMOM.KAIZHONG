SIE.defineCommand("SIE.Web.LES.MaterialReceives.Commands.RejectDetailCommand", {
    meta: { text: "拒收", group: "edit", iconCls: "icon-CloseView icon-red" },
    canExecute: function (view) {
        var sel = view.getSelection();
        if (sel == null || sel.length == 0) {
            return false;
        }
        return true;
    },
    execute: function (view) {
        var me = this;

        SIE.Msg.askQuestion('确认要拒收选中的数据吗?'.t(), function () {
            me.reject(view);
        });
    },

    //拒收
    reject(view) {
        SIE.Msg.wait("正在执行......".t());
        var indata = view.getSelection().map(p => p.data);
        view.execute({
            withIds: true,
            data: indata,
            success: function (res) { //回调
                SIE.Msg.showInstantMessage("拒收成功".t());
                view.reloadData();
            }
        });
    },
})