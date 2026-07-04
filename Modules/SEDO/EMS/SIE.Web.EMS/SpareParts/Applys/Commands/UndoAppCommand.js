SIE.defineCommand('SIE.Web.EMS.SpareParts.Applys.Commands.UndoAppCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "撤销", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    canExecute: function (view) {
        if (view.getCurrent() == null) {
            return false;
        }
        var state = view.getCurrent().getAuditState();
        if (state == 1 ) {
            return true;
        }

        return false;
    },
    //canVisible: function (view, source) {
    //    var current = view.getCurrent();
    //    if (current && current.getInspectionStatus() === SIE.Enum.QMS.Common.InspectionStatus.Inspectioned)
    //        return false;
    //    return true;
    //},
    execute: function (view, source) {
        var me = this;
        SIE.Msg.askQuestion("是否撤销？提交后单据不能修改。".t(),
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

        //var current = view.getCurrent();
        //current.setInspectionStatus(SIE.Enum.QMS.Common.InspectionStatus.Inspectioned);
        //if (res.Result) {
        //    current.setInspectionResult(res.Result.InspectionResult);   //检验中状态数据，填写报告，直接提交时，结果没有计算。所以把后台处理的结果再赋值到前端一次。
        //}
        //current.markSaved();

        //单据提交后所有数据不允许修改
        //view.getControl().items.items.forEach(function (ctl) {
        //    if (ctl.setReadOnly) {
        //        ctl.setReadOnly(true);
        //    }
        //});
        //var viewAttach = view.getChildren()[1];//附件清单
        //if (viewAttach) { SIE.QMS.CommonFuns.setAttachViewCommandVisible(viewAttach); }
        //view.getChildren().forEach(function (checkView) {
        //    checkView.setIsReadonly(true);
        //});
        CRT.Event.fire(view.model + "_refresh", view.getCurrent().data.Id);
        SIE.Msg.showInstantMessage('撤销成功！');
    }
});