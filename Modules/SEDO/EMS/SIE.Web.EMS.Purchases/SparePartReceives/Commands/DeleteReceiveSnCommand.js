SIE.defineCommand('SIE.Web.EMS.Purchases.SparePartReceives.Commands.DeleteReceiveSnCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    canExecute: function (view) {
        var fromEntity = view._parent.getCurrent();
        if (fromEntity.getSparePartReceiveDetailId() === null) {
            return false;
        }
        if (view.hasSelectedEntities()) {
            return true;
        }
        return false;
    },
    execute: function (view) {
        var msg = Ext.String.format('你确定删除选择的{0}条数据吗？'.t(), view.getSelection().length);
        SIE.Msg.askQuestion(msg, function () {
            var fromEntity = view._parent.getCurrent();
            var detailChildView = view._parent._children.first(function (p) { return p.model === "SIE.EMS.Purchases.SparePartReceives.SparePartReceiveDetail"; });
            var detail = detailChildView.getData().data.items.find(function (p) { return p.data.Id == fromEntity.getSparePartReceiveDetailId() });
            var selectModels = view.getSelection();
            var qty = detail.getRecivedQty() - selectModels.length;
            detail.setRecivedQty(qty);
            fromEntity.setRecivedQty(qty);
            view.removeSelection();
            var data = view.getData().data;
            if (data.length > 0) {
                view.getControl().setSelection(data.items[0]);
                view.setCurrent(data.items[0], true);
            } else {
                view.setCurrent(null, true);
            }
            var selectIds = view.getSelectionIds(selectModels);
            view.execute({
                withIds: true,
                selectIds: selectIds
            });
        });
    }
});