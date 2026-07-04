SIE.defineCommand('SIE.Web.ProductIntfc.InspRecords.Commands.ShippingInspCommand', {
    meta: { text: "报检", group: "edit", iconCls: "icon-TextRelease icon-blue" },
    canExecute: function (view) {
        this.selectItems = view.getSelectedEntities();
        return this.selectItems.length > 0;
    },
    execute: function (view, source) {
        var me = view;
        SIE.Msg.wait('成品报检中，请稍等…'.t());

        //将选中数据变脏，后台才能获取到改变的选中对象，不需要再传值，框架已处理
        this.selectItems.select(function (p) { return p.dirty = true; });
        view.execute({
            success: function (res) {
                var errMsg = res.Result;
                if (errMsg == '报检成功'.t())
                    view.getParent().reloadData();                
                SIE.Msg.showMessage(errMsg);
                //SIE.Msg.close();
            }
        });
    }
});