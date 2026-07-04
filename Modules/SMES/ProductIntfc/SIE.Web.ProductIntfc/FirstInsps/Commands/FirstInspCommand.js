SIE.defineCommand('SIE.Web.ProductIntfc.FirstInsps.Commands.FirstInspCommand', {
    meta: { text: "报检", group: "edit", iconCls: "icon-TextRelease icon-blue" },
    canExecute: function (view) {  
        var curEntity = view.getCurrent();
        if (curEntity == null) { return false; }
        if (view.getSelection().length != 1) { return false; }
        var curData = curEntity.getData();
        if (curData.InspState == 1) { return false; }
        this.selectItems = view.getSelectedEntities();
        return true;
    },
    execute: function (view, source) {
        
        SIE.Msg.wait('首件报检中，请稍等…'.t());

        //将选中数据变脏，后台才能获取到改变的选中对象，不需要再传值，框架已处理
        this.selectItems.select(function (p) { return p.dirty = true; });
        view.execute({
            success: function (res) {
                var errMsg = res.Result;
                if (errMsg.length == 0)
                    view.reloadData();
                else
                    SIE.Msg.showError(errMsg);
                SIE.Msg.showMessage('报检成功'.t());
            }
        });
    }
});