SIE.defineCommand('SIE.Web.EMS.SpareParts.Applys.Commands.CancelAuditAppCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "取消审核", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    canExecute: function (view) {
        if (view.getCurrent() == null) {
            return false;
        }
        var state = view.getCurrent().getAuditState();
        if (state == 3) {
            return true;
        }

        return false;
    },
    execute: function (view, source) {
        var me = this;
        SIE.Msg.askQuestion("是否取消审核？提交后单据不能修改。".t(),
            function () {
                //提交时，数据设置为脏，重新保存并校验所有内容,包括明细。
                view.getCurrent().dirty = true;
                view.getChildren().filter(function (e) { return e.model === "SIE.EMS.SpareParts.Applys"; }).forEach(function (v) {
                    v.getData().getData().items.forEach(function (detail) {
                        if (detail.getIsEnable())
                            detail.dirty = true;
                    });
                });
                me.doSave(view);
            });
    },
    onSaved: function (view, res) {
        var me = this;
        var operationView = view;
        var current = view.getCurrent();
        current.setInspectionResult(res.Result.AuditState);
        current.markSaved();
        CRT.Event.fire(view.model + "_refresh", view.getCurrent().data.Id);
        SIE.Msg.showInstantMessage('取消审核成功！');
    }
});