SIE.defineCommand('SIE.Web.LES.LesStockCounts.Commands.AduitLesCountFromCommand', {
    meta: { text: "生成明细", group: "edit", iconCls: "iconfont icon-NetworkNormal icon-blue" },
    extend: 'SIE.cmd.FormSave',
    canExecute: function (view) {
        var cur = view.getCurrent();
        if (cur == null || cur.isNew()) {
            return false;
        }
        if (view.getData().isDirty()) return false;
        if (cur.data.State != 0) {
            return false;
        }
        return true;
    },
    onSaved: function (view, res) {
        var me = this;
        var current = view.getCurrent();
        current.setState(10);
        var employee = CRT.Context.GlobalContext.getContext('userInfo');
        current.setAuditById_Display(employee.Name);
        current.setAuditById(employee.EmployeeId);
        current.setAuditDate(new Date());
        current.markSaved();             
        CRT.Event.fire(view.model + '_refresh', view.getCurrent().getId());
        view.getChildren().forEach(function (item) {
            item._curPid = "";
        });
        view.setCurrent(view.getCurrent(), true);//重新刷新store保持当前行数据跟数据库相同
        me.onSavedMsg(view, res);
        if (view.getChildren().length > 1)
            view.getChildren()[1].reloadData();
        var rangeView = me.view.findChild("SIE.LES.LesStockCounts.LesStockCountRange");
        if (rangeView) {
            var range = rangeView.getCurrent();
            range.setState(10);
        }
        view.syncCmdState();
    },
    onSavedMsg: function (view, res) {
        SIE.Msg.showInstantMessage('生成明细成功'.t());
    },
    execute: function (view, source) {
        var me = this;
        SIE.Msg.wait('正在生成明细......'.t());
        var isValidator = me.onSaving(view);
        if (isValidator) {
            //提交时，数据设置为脏，重新保存并校验所有内容,包括明细。
            view.getCurrent().dirty = true;
            me.doSave(view);
            view.getCurrent().markSaved();
        }
    }
});