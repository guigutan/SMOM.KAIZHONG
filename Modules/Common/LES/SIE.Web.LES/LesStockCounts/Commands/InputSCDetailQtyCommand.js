SIE.defineCommand('SIE.Web.LES.LesStockCounts.Commands.InputSCDetailQtyCommand', {
    extend: 'SIE.cmd.Save',
    meta: { text: "输入实盘", group: "edit", iconCls: "icon-PullIn icon-blue" },
    canExecute: function (view) {
        var parentCur = view.getParent().getCurrent();
        if (parentCur == null) return false;
        //SIE.Warehouses.CountState.Audit.value(审批)=10
        //SIE.Warehouses.CountState.PartCount.value(部分盘点)=30
        //SIE.Warehouses.CountState.FinishCount.value(已盘点)=40
        if (parentCur != null) {
            if (parentCur.data.State != 10 &&
                parentCur.data.State != 30 &&
                parentCur.data.State != 40)
                return false;
        }
        return true;
    },
    //更新
    updateViewModel: function (curViewModel, countDtl) {
        curViewModel.setQty(countDtl.getActualCountQty());
        curViewModel.setLineNo(countDtl.getLineNo());
        curViewModel.setLabelNo(countDtl.getLabelNo());
        curViewModel.setItemMessage(countDtl.getItemCode() + ";" + countDtl.getItemName() + ";" + countDtl.getItemSpecificationModel());
        curViewModel.setMessage(null);
    },
    //输入下一条可空的数据
    nextNotEmptySCDetail: function (curViewModel, dtlList) {
        var me = this;
        var emptyQtyDtl = dtlList.where(function (p) { return p.getActualCountQty() == null; }).first();
        if (emptyQtyDtl != null) {
            var initDtl = emptyQtyDtl;
            me.updateViewModel(curViewModel, initDtl);
        }
    },
    //上一行
    preStockCount: function (curViewModel, dtlList) {
        var me = this;
        var lastDtl;
        var lineNoList = dtlList.select(function (p) { return parseInt(p.getLineNo()); });
        var lastIndex = lineNoList.indexOf(curViewModel.getLineNo()) - 1;
        if (lastIndex < 0) {
            curViewModel.setMessage("当前行为首行".t());
        }
        else {
            lastDtl = dtlList[lastIndex];
            me.updateViewModel(curViewModel, lastDtl);
        }
    },
    //下一行
    nextStockCount: function (curViewModel, dtlList) {
        var me = this;
        var nextDtl;
        var lineNoList = dtlList.select(function (p) { return parseInt(p.getLineNo()); });
        var nextIndex = lineNoList.indexOf(curViewModel.getLineNo()) + 1;
        if (nextIndex > dtlList.length - 1) {
            curViewModel.setMessage("当前行为尾行".t());
        }
        else {
            nextDtl = dtlList[nextIndex];
            me.updateViewModel(curViewModel, nextDtl);
        }
    },
    execute: function (view, source) {
        var me = this;
        var dtlList = view.getData().data.items.where(function (p) { return p.getState() == 10 || p.getState() == 40; });
        if (dtlList.length == 0) {
            SIE.Msg.showError('盘点单还没有明细数据'.t());
            return;
        }
        SIE.AutoUI.getMeta({
            model: 'SIE.Web.LES.LesStockCounts.ViewModels.LesStockCountDetailQtyViewModel',
            ignoreCommands: true,
            isDetail: true,
            ignoreQuery: false,
            callback: function (res) {
                var mainBlock;
                if (res.mainBlock)
                    mainBlock = res.mainBlock;
                else
                    mainBlock = res;
                var detailView = SIE.AutoUI.createDetailView(mainBlock);
                detailView.associateCmd = me;
                var entity = new detailView._model();
                me.nextNotEmptySCDetail(entity, dtlList);
                detailView.setData(entity);
                var ui = detailView.getControl();
                var win = SIE.Window.show({
                    title: "输入实盘".t(),
                    width: 500,
                    height: 260,
                    items: ui,
                    buttons: ['上一行'.t(), '下一行'.t(), '关闭'.t()],
                    listeners: {
                        beforeClose: function (sender, handlers) {
                            me.view.syncCmdState();
                        }
                    },
                    callback: function (btn) {
                        if (btn == "上一行".t()) {
                            var curViewModel = detailView.getCurrent();
                            me.preStockCount(curViewModel, dtlList);
                            return false;
                        }
                        if (btn == "下一行".t()) {
                            var curViewModel = detailView.getCurrent();
                            me.nextStockCount(curViewModel, dtlList);
                            return false;
                        }
                        me.view.syncCmdState();
                        return true;
                    }
                });
            },
        });
    }
});