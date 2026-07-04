SIE.defineCommand('SIE.Web.EMS.AssetRequisitions.Commands.DeleteAssetRequisitionEquipmentCommand', {
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
            view.removeSelection();
            var data = view.getData().data;
            if (data.length > 0) {
                view.getControl().setSelection(data.items[0]);
                view.setCurrent(data.items[0], true);
            } else {
                view.setCurrent(null, true);
            }

            var amount = 0;

            view.getData().data.items.forEach(function (p) {
                amount += p.getEstimatedAmount();
            });
            view._parent.getCurrent().setAmount(amount);
        });
    }
});