SIE.defineCommand('SIE.Web.Andon.Andons.Commands.CopyAndonTypeMessageSendCommand', {
    extend: 'SIE.cmd.Copy',
    meta: { text: "复制新增", group: "edit", iconCls: "icon-ContentCopy icon-green", splitTo: "添加" },

    canExecute: function (view) {
        if ((view.getCurrent() != null && view.getSelection().length == 1)) {
            return true;
        }
        return false;
    },
    getEditEntity: function () {
        var entity = this.view.getCurrent();
        var copyEntity = this.view.copyEntity(entity);
        this._setCopyEntity(copyEntity.data);
        var editmode = this.view.editMode;
        if (editmode === SIE.viewMeta.editMode.INLINE) {
            //找旧数据所在行
            var index = this.view.getData().data.items.findIndex(m => m.getId() === entity.getId());
            this.view.getData().insert(index, copyEntity);
        }
        copyEntity.isCopy = true;
        return copyEntity;
    },
    _setCopyEntity: function (data) {
        //var oldData = this.view.getCurrent().data;


    }
});