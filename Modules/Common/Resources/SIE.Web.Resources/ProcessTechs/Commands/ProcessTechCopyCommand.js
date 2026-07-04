SIE.defineCommand('SIE.Web.Resources.ProcessTechs.Commands.ProcessTechCopyCommand', {
    extend: 'SIE.cmd.Copy',
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
        var processTech = this.view.getCurrent();
        var copyEntity = this.view.copyEntity(processTech);
        this.view.execute({
            data: copyEntity.data,
            isSubmmit: false,
            success: function (res) {
                copyEntity.setCode(res.Result.Code);
            }
        }, this.view);
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
    }
});