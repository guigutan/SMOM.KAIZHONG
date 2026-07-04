SIE.defineCommand('SIE.Web.Kit.UrgentOrder.ItemUrgentOrders.Commands.ItemUrgentOrderSaveCommand', {
    extend: 'SIE.cmd.Save',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    execute: function (view, source) {
        var me = this;
        var current = view.getCurrent();
        var demandTime = current.getDemandTime();
        var createTime = new Date();

        if (demandTime != null && createTime != null) {//改变制程路线的保存
            SIE.invokeDataQuery({
                async: false,
                type: "SIE.Web.Kit.UrgentOrder.ItemUrgentOrders.DataQuery.ItemUrgentOrderDataQueryer",
                method: 'GetItemUrgentOrderDate',
                token: view.token,
                params: [],
                callback: function (res) {
                    if (res.Success && res.Result != null) {//有被引用则先出提示再保存
                        //前端界面弹框,用户选择是否继续执行
                        //var ms = Math.abs(demandTime.getTime() - createTime.getTime());
                        //var diff = (ms / 1000 / 60 / 60).toFixed(2);
                        //if (diff > res.Result) {
                        //    SIE.Msg.showError('创建时间和需求时间的最小间隔数大于配置时间!'.t());
                        //    return false;
                        //}
                        me.doSave(view);
                    }
                }
            });
        }
    }
})