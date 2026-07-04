SIE.defineCommand('SIE.Web.EMS.ViceTransfers.Commands.ViceCopyTransferCommand', {
    extend: 'SIE.cmd.Copy',
    meta: { text: "复制新增", group: "edit", iconCls: "icon-AddEntity icon-green" },

    canExecute: function (view) {
        if ((view.getCurrent() != null && view.getSelection().length == 1)) {
            var parent = view.getParent().getCurrent();
            if (parent.getViceAssetObject() == 10 && (view.getCurrent().getControlMethod() == 10 || view.getCurrent().getControlMethod() == 20)) {//备件
                return true;
            }
            if (parent.getViceAssetObject() == 20 && view.getCurrent().getManageMode() == 10) {//工治具
                return true;
            }
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
            var index = this.view.getData().data.items.findIndex(m => m.getLineNo() === entity.getLineNo());;
            this.view.getData().insert(index, copyEntity);
        }
        copyEntity.isCopy = true;
        return copyEntity;
    },
    _setCopyEntity: function (data) {
        var oldData = this.view.getCurrent().data;


    }
});