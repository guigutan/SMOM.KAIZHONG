SIE.defineCommand('SIE.Web.EMS.EquipMaint.Maintains.Confirmations.Commands.RevokeUploadPicCommand', {
    meta: { text: "撤销上传", group: "edit", iconCls: "icon-PlaylistCheck icon-red" },
    canExecute: function (view) {
        var me = this;
        var entity = view.getSelection();
        if (entity == null || entity.length === 0) {
            return false;
        }
        var confirm = view.getParent().getCurrent();
        if (confirm == null || confirm.getExeState() !== 5) {
            return false;
        }
        return true;
    },
    execute: function (view, source) {
        var me = this;
        var select = view.getSelection();
        select.forEach(entity => {
            entity.setFileName("_");
            entity.setFileExtesion("");
            entity.setFilePath("");
            entity.setFileSize("");
            entity.setContent("");
        })
    },
});