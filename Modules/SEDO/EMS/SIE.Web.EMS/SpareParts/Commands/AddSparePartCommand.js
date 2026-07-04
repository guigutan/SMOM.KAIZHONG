SIE.defineCommand('SIE.Web.EMS.SpareParts.Commands.AddSparePartCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    canVisible: function (view, source) {
        var isWmsControl = CRT.Context.PageContext.getContext('IsWmsControl');
        if (isWmsControl) {
            return false;
        }
        return true;
    },
    execute: function (view, source)
    {
        var me = this;
        SIE.invokeDataQuery({
            type: "SIE.Web.EMS.SpareParts.DataQuery.SparePartDataQueryer",
            method: "VerifyIsWmsControl",
            params: [],
            async: false,
            token: view.token,
            callback: function (res) {
                if (res.Result) {
                    SIE.Msg.showError('备件启用了WMS管控，不能手动添加备件！'.t());
                    return false;
                }
                else{
                    me.showView(me.getEditEntity());
                }
            }
        });
    }
});