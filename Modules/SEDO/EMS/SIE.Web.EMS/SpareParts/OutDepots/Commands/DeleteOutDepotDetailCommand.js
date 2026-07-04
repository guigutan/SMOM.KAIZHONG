SIE.defineCommand('SIE.Web.EMS.SpareParts.OutDepots.Commands.DeleteOutDepotDetailCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    canExecute: function (view) {
        if (view.getSelection() == null) {
            return false;
        }
        return view.getSelection().length == 1 && view.getSelection()[0].data.OutboundStatus == null;
    },
    execute: function (view, source) {

        if (view.getData().getData().items.length == 0) {
            SIE.Msg.showError("没有可删除的数据".t());
            view.getControl().getView().grid.query('button')[0].disable();
            return;
        }
        var records = view.getSelection();
        SIE.Msg.askQuestion('你确定删除这条数据吗？'.t(), function () {

            var outDepotChild = view.getData();
            outDepotChild.remove(records[0]);

            var formView = view.getParent();
            var formEntity = formView.getData();

            if (outDepotChild.getCount() == 0){
                formEntity.data.IsExistDetail = false;//标记当前界面是否有出库明细数据
                formView._control.form.monitor.items.items[2].setDisabled(false);
                formView._control.form.monitor.items.items[2].setReadOnly(false);
                formView._control.form.monitor.items.items[4].setDisabled(false);
                formView._control.form.monitor.items.items[4].setReadOnly(false);
            }

            var applyChild = formView.findChild('SIE.EMS.SpareParts.OutDepots.Details.OutDepotDetail').getData();

            for (var i = 0; i < applyChild.getCount(); i++) {

                var record = applyChild.getAt(i);
                if (record.data.SparePartCodeView == records[0].data.SparePartCodeView) {

                    var requireCount = 0;

                    //手动添加出库单可更改申请数量和拣货数、删除申请明细，取消拣货只能扣减拣货数
                    if (formEntity.data.CreateDate == null) {
                        requireCount = record.data.RequireCount - records[0].data.OutDepotCount;
                        if (requireCount == 0) {
                            applyChild.remove(record);
                        }
                        else {
                            record.set('RequireCount', requireCount);
                            record.set('PickedCount', requireCount);
                        }
                    }
                    else {
                        requireCount = record.data.PickedCount - records[0].data.OutDepotCount;
                        record.set('PickedCount', requireCount);
                    }
                    
                    break;
                }
            }

            if (view.getData().getData().items.length > 0) {
                view.getControl().getSelectionModel().select(0, true);
            }
            
        });
    }
});