SIE.defineCommand('SIE.Web.Defects.Measures.Commands.RepairMeasureCopyCommand', {
    extend: 'SIE.cmd.Copy',
    meta: { text: "复制新增", group: "edit", iconCls: "iconfont icon-ContentCopy icon-green" },
    canExecute: function (view) {
        if (view.getCurrent() != null && view.getSelection().length == 1) {
            return true;
        }
        else {
            return false;
        }
    },
    getEditEntity: function () {
        var repairMeasure = this.view.getCurrent();
        var copyEntity = this.view.copyEntity(repairMeasure);
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
        data.Name = oldData.Name + "-复制".t();
        data.Code = oldData.Code + "-复制".t();
        data.Description = oldData.Description;
    }
});