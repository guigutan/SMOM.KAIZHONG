SIE.defineCommand('SIE.Web.Kit.MES.CallMaterials.Commands.AutoCallMaterialCommand', {
    meta: { text: "自动", group: "edit", iconCls: "icon-CheckboxBlankOutline icon-blue" },
    canExecute: function (view) {
        var queryData = view._relations[0]._target.getData().data;
        if (queryData != null && queryData.ResourceName != null) { return true; }
        return false;
    },
    execute: function (view, source) {
        var me = this;
        var resourceName = view._relations[0]._target.getData().data.ResourceName;
        var flag = 0;
        if (view._sourceCmd.iconCls == "iconfont icon-CheckboxMarkedOutline icon-blue")
            flag = 1;
        var indata = {};
        indata.data = Ext.encode({ Flag: flag, ResourceName: resourceName });
        view.execute({
            data: indata,
            success: function (res) {
                var errMsg = res.Result;
                if (errMsg == '-1' || errMsg == '0' || errMsg == '1')
                {
                    if (errMsg == '0')
                        view._sourceCmd.setIconCls("iconfont icon-CheckboxMarkedOutline icon-blue");
                    else if (errMsg == '1')
                        view._sourceCmd.setIconCls("iconfont icon-CheckboxBlankOutline icon-blue");
                }
                else
                    SIE.Msg.showMessage(errMsg);
            }
        });
    }
});