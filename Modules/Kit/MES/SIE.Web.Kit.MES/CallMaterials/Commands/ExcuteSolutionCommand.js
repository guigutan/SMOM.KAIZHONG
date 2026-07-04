SIE.defineCommand('SIE.Web.Kit.MES.CallMaterials.Commands.ExcuteSolutionCommand', {
    meta: { text: "执行方案", group: "edit", iconCls: "icon-Implementation icon-blue" },
    execute: function (view, source) {
        var me = this;
        var resourceName = view._relations[0]._target.getData().data.ResourceName;
        var flag = 0;
        var indata = {};
        indata.data = Ext.encode({ Flag: flag, ResourceName: resourceName });
        view.execute({
            data: indata,
            success: function (res) {
                var errMsg = res.Result;
                if (errMsg == '执行成功')
                    view.reloadData();
                    SIE.Msg.showMessage(errMsg);
            }
        });
    }
});