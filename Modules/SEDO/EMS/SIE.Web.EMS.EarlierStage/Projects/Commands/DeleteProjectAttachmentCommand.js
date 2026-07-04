SIE.defineCommand('SIE.Web.EMS.EarlierStage.Projects.Commands.DeleteProjectAttachmentCommand', {
    extend: 'SIE.Web.Common.Attachments.Commands.DeleteAttachmentCommand',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    /**
     * 是否可以执行
     * @param {*} view
     * @returns 总是可以执行，
     * 子类可以根据具体情况覆写
     */
    canExecute: function (view) {
        if (view.getParent().getCurrent() != null) {
            return view.getParent().getSelection().first().getProjectStatus() != 30;
        }
        return true;
    },
}); 