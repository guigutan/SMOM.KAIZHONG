SIE.defineCommand('SIE.Web.EMS.SpareParts.Commands.WholeBillStoreCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "整单入库", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    canExecute: function (view) {
        if (view.getSelection() == null) {
            return false;
        }
        return view.getSelection().length == 1
            && (view.getSelection()[0].data.InboundStatus == 10 || view.getSelection()[0].data.InboundStatus == 20);
    },
    execute: function (view, source) {

        var entity = view.getCurrent();
        var indata = { Data: Ext.encode(entity.data) };

        SIE.Msg.askQuestion("是否将此单据进行整单入库？".t(),
            function () {
                view.execute({
                    data: indata,
                    async: false,
                    success: function (res) {
                        view.reloadData();
                        SIE.Msg.showMessage('入库成功'.t());
                    },
                    error: function (res) {
                        SIE.Msg.showError(res.Message);
                    }
                });
            });
    }
});