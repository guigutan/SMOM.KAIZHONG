SIE.defineCommand('SIE.Web.EMS.EarlierStage.Budgets.Commands.EditSubmitBudgetCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "保存并提交", group: "edit", iconCls: "icon-Refuse icon-blue" },
    canExecute: function (view) {
        var current = view.getCurrent();

        if (!current) return false;

        //审核状态：10 待提交 50 驳回
        if (current.data.ApprovalStatus !== 10 && current.data.ApprovalStatus !== 50) {
            return false;
        }

        return current.isDirty();
    },
    /**
 *  保存后事件
 * @param {} view 
 * @returns {} 
 */
    onSaved: function (view, res) {
        var current = view.getCurrent();
        current.markSaved();
        SIE.Msg.showInstantMessage('保存并提交成功！'.t());
        window.setTimeout(function () {
            CRT.Event.fire(view.model + "_refresh", current.data.Id);
            CRT.Workbench.closeCurrentTab();
        }, 3000);
    }
});