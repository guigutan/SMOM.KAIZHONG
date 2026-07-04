SIE.defineCommand('SIE.Web.Kit.UrgentOrder.ItemUrgentOrders.Commands.ItemUrgentOrderCopyCommand', {
    extend: 'SIE.cmd.Copy',
    meta: { text: "复制新增", group: "edit", iconCls: "icon-AddEntity icon-green" },
    canExecute: function (view) {
        if (view.getCurrent() != null && view.getSelection().length == 1) {
            return true;
        }
        return false;
    },
    getEditEntity: function () {
        var processTech = this.view.getCurrent();
        var copyEntity = this.view.copyEntity(processTech);
        this.view.execute({
            data: copyEntity.data,
            success: function (res) {
                copyEntity.setNo(res.Result.No);
            }
        }, this.view);
        var editmode = this.view.editMode;
        if (editmode === SIE.viewMeta.editMode.INLINE) {
            this.view.getData().insert(0, copyEntity);
        }
        copyEntity.isCopy = true;
        return copyEntity;
    },
});