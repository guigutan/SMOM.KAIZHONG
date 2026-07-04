SIE.defineCommand('SIE.Web.EMS.SpareParts.Applys.Commands.CancelAppCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "取消", group: "edit", iconCls: "icon-NetworkError icon-red" },
    canExecute: function (view) {
        if (view.getSelection() == null) {
            return false;
        }
        return view.getSelection().length == 1
            && view.getSelection()[0].data.AuditState == 1;
    },
    execute: function (view, source) {
        var me = this;

        SIE.invokeDataQuery({
            type: "SIE.Web.EMS.SpareParts.Applys.DataQuerys.SparePartAppDataQuery",
            method: "VerifyIsEnableAuditFlow",
            params: [],
            async: false,
            token: view.token,
            callback: function (res) {
                if (res.Result) {
                    SIE.Msg.showError('申请单启用了审批流，需通过审批流程取消！'.t());
                    return false;
                }
                else {
                    SIE.Msg.askQuestion("是否取消？提交后单据不能修改。".t(),
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
                }
            }
        });
    },
    onSaved: function (view, res) {
        CRT.Event.fire(view.model + "_refresh", view.getCurrent().data.Id);
        SIE.Msg.showInstantMessage('取消成功！');
    }
});