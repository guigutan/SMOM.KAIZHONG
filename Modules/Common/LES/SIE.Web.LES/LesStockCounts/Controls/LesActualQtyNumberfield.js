Ext.define('SIE.Web.LES.LesStockCounts.Controls.LesActualQtyNumberfield', {
    extend: 'Ext.form.field.Number',
    alias: 'widget.LesActualQtyNumberfield',
    listeners: {
        show: function () {
            this.focus(true);
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
                //SIE.Warehouses.CountState.Audit.value(审批)=10
                //SIE.Warehouses.CountState.PartCount.value(部分盘点)=30
                //SIE.Warehouses.CountState.FinishCount.value(已盘点)=40
                var dtlList = dtlItems.where(function (p) { return p.getState() == 10 || p.getState() == 40; });
                if (me.updateQty(entity, dtlList)) {
                    me.nextNotEmptySCDetail(entity, dtlList);
                    this.selectText();
                }
            }
        }
    },
    //更新数量
    updateQty: function (curViewModel, dtlList) {
        var me = this;
        var result = false;
        var countDtl = dtlList.where(function (p) { return p.getLineNo() == (curViewModel.getLineNo()).toString(); }).first();
        if (countDtl != null) {
            if (countDtl.getItemId() > 0 && curViewModel.getQty() < 0) {
                curViewModel.setMessage("输入的数量不能小于0".t());
            }
            else if ((countDtl.getItemId() == null || countDtl.getItemId() <= 0) && curViewModel.getQty() > 0) {
                curViewModel.setMessage("空库位的实盘数量不能大于0".t());
            }
            else {
                if (curViewModel.getQty() >= 0) {
                    countDtl.setActualCountQty(curViewModel.getQty());
                    countDtl.setDiffCountQty(countDtl.getActualCountQty() - countDtl.getQty());
                    var employee = CRT.Context.GlobalContext.getContext('userInfo');
                    countDtl.setCountById_Display(employee.Name);
                    countDtl.setCountById(employee.EmployeeId);
                    countDtl.setCountDate(new Date());
                    curViewModel.setMessage("第{0}行,输入成功！({1})".t().format(curViewModel.getLineNo(), curViewModel.getItemMessage()));
                }
                else {
                    countDtl.setActualCountQty(null);
                    countDtl.setDiffCountQty(null);
                    countDtl.setCountById(null);
                    countDtl.setCountDate(null);
                    curViewModel.setMessage("第{0}行,没有输入！".t().format(curViewModel.getLineNo()));
                }
                result = true;
            }
        }
        else {
            curViewModel.setQty(null);
            curViewModel.setItemMessage(null);
            curViewModel.setMessage("找不到对应的行号或明细已经关闭".t());
        }
        return result;
    },
    //更新
    updateViewModel1: function (curViewModel, countDtl) {
        curViewModel.setQty(countDtl.getActualCountQty());
        curViewModel.setLineNo(countDtl.getLineNo());
        curViewModel.setItemMessage(countDtl.getItemCode() + ";" + countDtl.getItemName() + ";" + countDtl.getItemSpecificationModel());
        //curViewModel.setMessage(null);
    },
    //输入下一条可空的数据
    nextNotEmptySCDetail: function (curViewModel, dtlList) {
        var me = this;
        var index = -1;
        if (curViewModel.getLineNo() > 0) {
            var lineNoList = dtlList.select(function (p) { return parseInt(p.getLineNo()); });
            index = lineNoList.indexOf(curViewModel.getLineNo());
        }
        var emptyQtyDtl = dtlList.where(function (p, i) { return p.getActualCountQty() == null && i > index; }).first();
        if (emptyQtyDtl != null) {
            var initDtl = emptyQtyDtl;
            me.updateViewModel1(curViewModel, initDtl);
        }
    },
});