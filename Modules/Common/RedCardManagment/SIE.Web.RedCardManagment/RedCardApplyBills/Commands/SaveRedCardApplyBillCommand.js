SIE.defineCommand('SIE.Web.RedCardManagment.RedCardApplyBills.Commands.SaveRedCardApplyBillCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },

    canExecute: function (view) {
        var result = false;
        var current = view.getCurrent();
        if (current) {
            result = current.isDirty() && !view.isSaved;
        }
        return result;
    },

    canVisible: function (view) {

        var current = view.getCurrent();
        var params = CRT.Context.PageContext.getParams();
        var isStart = false;
        if (params && params.IsStart) {
            isStart = true;
        }

        return current && current.getApplyType() == SIE.RedCardManagment.RedCardApplyBills.ApplyType.Manual && !isStart;

    },

    //onSaved: function (view, res) {
    //    var me = this;
    //    var current = view.getCurrent();
    //    me.onSavedMsg(view, res);
    //    view.isSaved = true;

    //    //单据提交后所有数据不允许修改
    //    view.getControl().items.items.forEach(function (ctl) {
    //        if (ctl.title != "追溯条件") {
    //            ctl.items.items.forEach(function (childrenCtl) {
    //                if (childrenCtl.setReadOnly) {
    //                    childrenCtl.setReadOnly(true);
    //                }
    //            })
    //        }
    //    });
    //    current.markSaved();
    //    CRT.Event.fire(view.model + "_refresh", view.getCurrent().data.Id);
    //},
});