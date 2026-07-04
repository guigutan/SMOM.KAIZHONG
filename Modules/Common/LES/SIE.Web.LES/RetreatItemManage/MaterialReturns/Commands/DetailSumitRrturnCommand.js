SIE.defineCommand('SIE.Web.LES.RetreatItemManage.MaterialReturns.Commands.DetailSumitRrturnCommand', {
    meta: { text: "提交", group: "edit", iconCls: "iconfont icon-Check icon-blue" },
    extend: 'SIE.cmd.Save',
    canExecute: function (view) {
        return view.getCurrent() != null && view.getCurrent().data.ReturnState == "10" && view.getCurrent().getNO()!="";
    },
    onSaved: function (view, res) {
        var current = view.getCurrent();
        current.markSaved();
        SIE.Msg.showToast('提交成功'.t(), '完成');
        window.setTimeout(function () {
            CRT.Event.fire("SIE.LES.RetreatItemManage.MaterialReturns.MaterialReturn_refresh");
            CRT.Workbench.closeCurrentTab();
        }, 1000);
    },
});