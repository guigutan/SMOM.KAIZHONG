SIE.defineCommand('SIE.Web.Kit.MES.CallMaterials.Commands.CancelCallMaterialCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "取消叫料", group: "edit", iconCls: "icon-Cancel icon-blue" },
    canExecute: function (view) {
        var curEntity = this.view.getCurrent();
        if (curEntity == null) { return false; }
        if (view.getSelection().length != 1) { return false; }
        var curData = curEntity.getData();
        if (curData != null && curData.Priority == 0 && (curData.Status == 0 ||curData.Status == 1 || curData.Status == 3 )) { return true; }
        return false;
    },
    execute: function (view, source) {
        var me = this;
        var indata = {};
        var editEntity = this.getEditEntity();
        indata.Data = Ext.encode(editEntity.data);
        var msg = Ext.String.format('确定要取消叫料？'.t());
        SIE.Msg.askQuestion(msg, function () {
            view.execute({
                data: indata,
                success: function (res) {
                    var errMsg = res.Result;
                    if (errMsg == '操作成功') {
                        //var curEntity = this.view.getCurrent();
                        //var curData = curEntity.getData();
                        //curData.Status = 4;
                        if (view.getParent() != null)
                            view.getParent().reloadData();
                        else
                            view.reloadData();
                    }
                    SIE.Msg.showMessage(errMsg);
                }
            });
        });
    }
});