SIE.defineCommand('SIE.Web.EMS.SpareParts.OutDepots.Commands.CancelPickedCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "取消拣货", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    canExecute: function (view) {
        if (view.getSelection() == null) {
            return false;
        }
        if (view.getSelection().length == 1) {
            if (view.getSelection()[0].data.OutboundStatus == 10) 
                return true;
        }
        return false;
    },
    execute: function (view, source) {
        var records = view.getSelection();
        SIE.Msg.askQuestion('你确定对这条数据取消拣货吗？'.t(), function () {

            var outDepotChild = view.getData();
            outDepotChild.remove(records[0]);

            var applyChildView = view.getParent().findChild('SIE.EMS.SpareParts.OutDepots.Details.OutDepotDetail');

            var tabPanel = view.getControl().ownerCt.ownerCt;
            var applyDetailTab = applyChildView.getControl().ownerLayout.owner.tab;
            var outDepotDetailTab = view.getControl().ownerLayout.owner.tab;

            tabPanel.setActiveTab(tabPanel.items.keys.indexOf(applyDetailTab.card.id));
            tabPanel.setActiveTab(tabPanel.items.keys.indexOf(outDepotDetailTab.card.id));

            setTimeout(function () {
                var applyChild = applyChildView.getData();

                for (var i = 0; i < applyChild.getCount(); i++) {
                    var record = applyChild.getAt(i);
                    if (record.data.SparePartId == records[0].data.SparePartId) {
                        var pickedCount = record.data.PickedCount - records[0].data.OutDepotCount;
                        record.set('PickedCount', pickedCount);
                        break;
                    }
                }

                view.removeSelection();
                var data = view.getData().data;
                if (data.length > 0) {
                    view.getControl().setSelection(data.items[0]);
                    view.setCurrent(data.items[0], true);
                } else {
                    view.setCurrent(null, true);
                }
            }, 300);
        });
    }
});