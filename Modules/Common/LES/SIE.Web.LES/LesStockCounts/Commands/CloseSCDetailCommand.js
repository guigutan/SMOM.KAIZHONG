SIE.defineCommand('SIE.Web.LES.LesStockCounts.Commands.CloseSCDetailCommand', {
    meta: { text: "关闭", group: "edit", iconCls: "icon-Cancel icon-blue" },
    extend: 'SIE.cmd.Save',
    canExecute: function (view) {
        if (!view.hasSelectedEntities()) {
            return false;
        }
        else {
            if (view.getData().isDirty()) return false;
            var curParent = view.getParent().getCurrent();
            if (curParent == null) return false;
            //SIE.Warehouses.CountState.Finished.value(完工)=50
            if (curParent.getState() === 50)
                return false;

            var flag = true;
            Ext.each(view.getSelection(), function (item) {
                //SIE.Warehouses.CountState.Audit.value(审批)=10
                //SIE.Warehouses.CountState.FinishCount.value(已盘点)=40
                if (item.isNew() || item.isDirty() || (item.data.State != 10 &&
                    item.data.State != 40)) {
                    flag = false;
                }
            });
            return flag;
        }
        return true;
    },
    onSavedMsg: function (view, res) {
        SIE.Msg.showInstantMessage('关闭成功'.t());
    },
    onSaved: function (view, res) {
        var me = this;
        var curParent = view.getParent().getCurrent();
        curParent.setState(res.Result.State);
        curParent.markSaved();
        this.callParent(arguments);
    },
    execute: function (view, source) {
        var me = this;
        SIE.Msg.askQuestion('关闭单据后将不能进行编辑，是否继续？'.t(), function () {
            var isValidator = me.onSaving(view);
            if (isValidator) {
                //提交时，数据设置为脏，重新保存并校验所有内容,包括明细。
                Ext.each(view.getSelection(), function (item) {
                    item.dirty = true;
                });
                me.doSave(view);
                Ext.each(view.getSelection(), function (item) {
                    item.markSaved();
                });
            }
        });
    }
});