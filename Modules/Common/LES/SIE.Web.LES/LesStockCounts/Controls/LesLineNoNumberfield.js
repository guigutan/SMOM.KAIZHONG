Ext.define('SIE.Web.LES.LesStockCounts.Controls.LesLineNoNumberfield', {
    extend: 'Ext.form.field.Number',
    alias: 'widget.LesLineNoNumberfield',
    listeners: {
        blur: function () {
            //失去焦点事件
            var me = this;
            if (me.up("form")) {
                entity = me.up("form").SIEView.getData();
            } else {
                entity = me.up('container').context.record
            }
            var dtlItems = me.up("form").SIEView.associateCmd.view.getData().data.items;
            //SIE.Warehouses.CountState.Audit.value(审批)=10
            //SIE.Warehouses.CountState.PartCount.value(部分盘点)=30
            //SIE.Warehouses.CountState.FinishCount.value(已盘点)=40
            var dtlList = dtlItems.where(function (p) { return p.getState() == 10 || p.getState() == 40; });
            me.autoUpdateByLineNo(entity, dtlList);
            me.up("form").SIEView.findEditor("Qty").focus(true);
        },
        focus: function () {
            //获取焦点
            var me = this;
            me.selectText();
        },
        specialkey: function (field, e) {
            //回车事件
            var me = this;
            if (e.getKey() == e.ENTER) {
                if (me.up("form")) {
                    entity = me.up("form").SIEView.getData();
                } else {
                    entity = me.up('container').context.record
                }
                var dtlItems = me.up("form").SIEView.associateCmd.view.getData().data.items;
                var dtlList = dtlItems.where(function (p) { return p.getState() == 10 || p.getState() == 40; });
                me.autoUpdateByLineNo(entity, dtlList);
                me.up("form").SIEView.findEditor("Qty").focus(true);
            }
        }
    },
    autoUpdateByLineNo: function (curViewModel, dtlList) {
        var me = this;
        if (curViewModel.getLineNo() > 0) {
            var countDtl = dtlList.where(function (p) { return p.getLineNo() == (curViewModel.getLineNo()).toString(); }).first();
            if (countDtl != null) {
                curViewModel.setQty(countDtl.getActualCountQty());
                curViewModel.setItemMessage(countDtl.getItemCode() + ";" + countDtl.getItemName() + ";" + countDtl.getItemSpecificationModel());
                curViewModel.setMessage(null);
            }
            else {
                curViewModel.setQty(null);
                curViewModel.setItemMessage(null);
                curViewModel.setMessage("找不到对应的行号或明细已经关闭".t());
            }
        }
    }
});