SIE.defineCommand('SIE.Web.EMS.Purchases.FixtureReceives.Commands.DeleteReceiveSnCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    canExecute: function (view) {
        if (view.hasSelectedEntities()) {
            return true;
        }
        return false;
    },
    execute: function (view) {
        var msg = Ext.String.format('你确定删除选择的{0}条数据吗？'.t(), view.getSelection().length);
        SIE.Msg.askQuestion(msg, function () {
            var fromEntity = view._parent.getCurrent();
           
            var selectModels = view.getSelection();
            var detailChildView = view._parent._children.first(function (p) { return p.model === "SIE.EMS.Purchases.FixtureReceives.FixtureReceiveDetail"; });
            selectModels.forEach(sel => {

                var receiveDetail= detailChildView.getData().data.items.find(function (p) { return p.data.Id === sel.getFixtureReceiveDetailId() });
                receiveDetail.setRecivedQty(receiveDetail.getRecivedQty() - 1);
                if (fromEntity.getFixtureReceiveDetailId()== receiveDetail.getId()) {
                    fromEntity.setRecivedQty(fromEntity.getRecivedQty() - 1);
                }
            });
            view.removeSelection();
            var data = view.getData().data;
            if (data.length > 0) {
                view.getControl().setSelection(data.items[0]);
                view.setCurrent(data.items[0], true);
            } else {
                view.setCurrent(null, true);
            }
            //var selectIds = view.getSelectionIds(selectModels);
            //view.execute({
            //    withIds: true,
            //    selectIds: selectIds,
            //    data: fromEntity.getReceiveType()
            //});
        });
    }
});