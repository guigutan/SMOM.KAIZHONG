SIE.defineCommand("SIE.Web.Andon.Andons.Commands.AndonManageAttachmentClearCommand", {
    meta: { text: "清空附件", group: "edit", iconCls: "icon-PageDelete icon-red" },
    canExecute: function (view) {
        var cur = view.getCurrent();
        if (cur && cur.getAttachment().length != 0) {
            return true;
        }
        return false;
    },
    execute: function (view) {
        var cur = view.getCurrent();
        cur.setAttachment("");
    }
})