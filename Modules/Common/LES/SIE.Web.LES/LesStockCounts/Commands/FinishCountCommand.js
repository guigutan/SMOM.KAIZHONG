SIE.defineCommand('SIE.Web.LES.LesStockCounts.Commands.FinishCountCommand', {
    meta: { text: "完工", group: "edit", iconCls: "icon-CalendarCheck icon-blue" },
    extend: 'SIE.Web.LES.LesStockCounts.Commands.FinishStockCountsCommand',
    canExecute: function (view) {
        var cur = view.getCurrent();
        if (cur == null || cur.isDirty()) {
            return false;
        }
        if (view.getData().isDirty())
            return false;
        if (cur.data.State !== 40) {
            return false;
        }
        return cur != null;
    },
    onSaved: function (view, res) {
        var current = view.getCurrent();
        current.setState(50);
        current.setLesStockCountResult(0);
        current.markSaved();
        
        CRT.Event.fire(view.model + '_refresh', view.getCurrent().getId());
        view.getChildren().forEach(function (item) {
            item._curPid = "";
        });
        view.setCurrent(view.getCurrent(), true);//重新刷新store保持当前行数据跟数据库相同
        SIE.Msg.showMessage("执行成功".t());
        if (view.getChildren().length > 1)
            view.getChildren()[1].reloadData();
    },
    reloadView: function () {
        this.view.refreshData(this.view.getCurrent().getId());
    },
     
    execute: function (view, source) {
        var me = this;
        var curBill = me.view.getCurrent();
        SIE.invokeDataQuery({
            async: false,
            type: "SIE.Web.LES.LesStockCounts.DataQueryer.LesStockCountDataQueryer",
            method: 'GetFinishCountDetail',
            token: me.view.token,
            params: [curBill.getId()],
            callback: function (res) {
                if (res.Success) {                    
                    if (res.Result == null) {
                        //var isValidator = me.onSaving(view);
                        //if (isValidator) {
                        //提交时，数据设置为脏，重新保存并校验所有内容,包括明细。

                        view.getCurrent().dirty = false;
                        me.onSaved(view);
                        view.getCurrent().markSaved();
                        // }
                    }
                    else {
                        me.finishDiffCount(curBill, res.Result);
                    }
                }
                if (!res.Success) {
                    SIE.Msg.showError(res.Message);
                }
            }
        });
    }
});