SIE.defineCommand('SIE.Web.EMS.SpareParts.Applys.Commands.SubmitAppCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "提交", group: "edit", iconCls: "icon-EnableUsers icon-blue" },
    canExecute: function (view) {
        if (view.getSelection() == null) {
            return false;
        }
        return view.getSelection().length == 1
            && !(view.getSelection()[0].data.AuditState > 0 && view.getSelection()[0].data.AuditState != 2);
    },
    execute: function (view, source) {
        var me = this;

        var dtlChildView = view.findChild('SIE.EMS.SpareParts.Applys.Details.ApplyDetail');
        var dtlStore = dtlChildView.getData();

        if (dtlStore.getCount() == 0) {
            SIE.Msg.showError("申请明细未添加备件信息，无法提交！".t());
            return false;
        }

        
        for (var i = 0; i < dtlStore.getCount(); i++) {
            var record = dtlStore.getAt(i);
            var k = 0;
            for (var j = 0; j < dtlStore.getCount(); j++) {
                var dtlRecord = dtlStore.getAt(j);
                if (record.data.SparePartId == dtlRecord.data.SparePartId && record.data.WarehouseId == dtlRecord.data.WarehouseId) {
                    k++;
                    if (k == 2) {
                        SIE.Msg.showError("请合并相同【备件编码】和【出库仓库】的申请明细！".t());
                        return false;
                    }
                }
            }
        }
            
        SIE.Msg.askQuestion("是否提交？提交后单据不能修改。".t(),
            function () {
                //提交时，数据设置为脏，重新保存并校验所有内容,包括明细。
                view.getCurrent().dirty = true;
                view.getChildren().filter(function (e) { return e.model === "SIE.EMS.SpareParts.Applys"; }).forEach(function (v) {
                    v.getData().getData().items.forEach(function (detail) {
                        detail.dirty = true;
                    });
                });
                me.doSave(view);
            });
    },
    onSaved: function (view, res) {
        CRT.Event.fire(view.model + "_refresh", view.getCurrent().data.Id);
        SIE.Msg.showInstantMessage('提交成功！');
    }
});