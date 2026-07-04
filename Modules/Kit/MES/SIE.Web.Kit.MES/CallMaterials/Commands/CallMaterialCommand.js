SIE.defineCommand('SIE.Web.Kit.MES.CallMaterials.Commands.CallMaterialCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "叫料", group: "edit", iconCls: "icon-Click icon-blue" },
    canExecute: function (view) {
        var autoCallMaterialCM = view.getCmdControl("SIE.Web.Kit.MES.CallMaterials.Commands.AutoCallMaterialCommand");
        if (autoCallMaterialCM == null || !autoCallMaterialCM.arrowVisible) { return false; }
        var curEntity = this.view.getCurrent();
        if (curEntity == null) { return false; }
        if (view.getSelection().length != 1) { return false; }       
        var curData = curEntity.getData();
        if (curData != null && curData.WoState == 0 && curData.WoIsPause == 0 && curData.ChildNum == 0) { return true; }
        return false;
    },
    execute: function (view, source) {
        var me = this;
        var indata = {};
        var editEntity = this.getEditEntity();
        indata.Data = Ext.encode(editEntity.data);
        view.execute({
            data: indata,
            success: function (res) {
                var errMsg = res.Result;
                if (errMsg == '操作成功')
                    view.reloadData();
                SIE.Msg.showMessage(errMsg);
            }
        });
    }
});