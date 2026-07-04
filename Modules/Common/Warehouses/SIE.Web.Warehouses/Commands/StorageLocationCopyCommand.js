SIE.defineCommand('SIE.Web.Warehouses.Commands.StorageLocationCopyCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "复制新增", group: "edit", iconCls: "icon-AddEntity icon-green" },

    canExecute: function (view) {
        if (view.getCurrent() != null && view.getSelection().length == 1) {
            return true;
        }
        else {
            return false;
        }
    },
    getEditEntity: function () {
        var storageLocation = this.view.getCurrent();
        var copyEntity = this.view.copyEntity(storageLocation);
        this._setCopyEntity(copyEntity.data);
        var editmode = this.view.editMode;
        if (editmode === SIE.viewMeta.editMode.INLINE) {
            this.view.getData().insert(0, copyEntity);
        }
        copyEntity.isCopy = true;
        return copyEntity;
    },
    _setCopyEntity: function (data) {
        var oldData = this.view.getCurrent().data;
        data.CreateBy = null;
        data.CreateByName = null;
        data.CreateBy_Display = null;
        data.CreateDate = null;
        data.UpdateBy = null;
        data.UpdateByName = null;
        data.UpdateBy_Display = null;
        data.UpdateDate = null;
        data.SimpleCode = "";
        data.Code = oldData.Code + "-复制".t();
        data.Name = oldData.Name;
        data.State = 1;
        data.IsFrozen = false;
    }
});