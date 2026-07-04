SIE.defineCommand('SIE.Web.Kit.MES.CallMaterials.Commands.SetUrgencyCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "设为紧急", group: "edit"},
    canExecute: function (view) {        
        var curEntity = this.view.getCurrent();
        if (curEntity == null) { return false; }
        if (view.getSelection().length != 1) { return false; }
        var curData = curEntity.getData();
        if (curData != null && curData.Priority == 0 && (curData.Status == 0 || curData.Status == 1 || curData.Status == 3)) { return true; }
        return false;
    },
    execute: function (view, source) {
        var me = this;
        var indata = {};
        var editEntity = this.getEditEntity();
        indata.Data = Ext.encode(editEntity.data);
        var msg = Ext.String.format('确定要设为紧急吗？'.t());
        SIE.Msg.askQuestion(msg, function () {
            view.execute({
                data: indata,
                success: function (res) {
                    var errMsg = res.Result;
                    if (errMsg == '操作成功') {
                        if (view.getParent() != null)
                            view.getParent().reloadData();
                        else
                            view.reloadData();
                    }
                    else
                        SIE.Msg.showMessage(errMsg);
                }
            });
        });        
    }
});