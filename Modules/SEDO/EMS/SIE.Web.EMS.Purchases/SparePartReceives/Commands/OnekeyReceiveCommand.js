SIE.defineCommand('SIE.Web.EMS.Purchases.SparePartReceives.Commands.OnekeyReceiveCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "一键接收", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    canExecute: function (view) {
        if (view.getParent() == null)
            return false;
        var cur = view.getParent().getCurrent();
        if (cur == null)
            return false;
        if (cur.data.ReceiveBillStatus !== 10)
            return false;
        return true;
    },
    execute: function (view, source) {
        var me = this;

        var cur = view.getParent().getCurrent();

        //电子签名信息
        var signdata = {
            command: me.meta.command,
            entityType: me.view.model,
            parentType: me.view.getParent() ? me.view.getParent().model : ""
        }

        SIE.invokeDataQuery({
            method: 'OnekeyReceive',
            params: [cur.data.Id],
            action: 'queryer',
            type: 'SIE.Web.EMS.Purchases.SparePartReceives.SparePartReceiveDataQueryer',
            token: view.token,
            logInfo: signdata,
            success: function (res) {
                SIE.Msg.showMessage("接收成功!".t());
                view.reloadData();
            }
        });
    }
});