SIE.defineCommand('SIE.Web.LES.RetreatItemManage.MaterialReturns.Commands.SaveReturnCommand', {
    meta: { text: "保存", group: "edit", iconCls: "iconfont icon-SaveEntity icon-blue" },
    extend: 'SIE.cmd.Save',
    canExecute: function (view) {
        return view.getCurrent() != null && view.getCurrent().isDirty() && view.getCurrent().getNO() != "";
    },
    onSaved: function (view, res) {
        var current = view.getCurrent();
        current.markSaved();
        SIE.Msg.showToast('保存成功'.t(), '完成');
        window.setTimeout(function () {
            CRT.Event.fire("SIE.LES.RetreatItemManage.MaterialReturns.MaterialReturn_refresh");
            CRT.Workbench.closeCurrentTab();
        }, 1000);
    },
});