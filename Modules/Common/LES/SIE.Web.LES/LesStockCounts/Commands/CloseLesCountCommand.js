SIE.defineCommand('SIE.Web.LES.LesStockCounts.Commands.CloseLesCountCommand', {
    meta: { text: "关闭", group: "edit", iconCls: "icon-Cancel icon-blue" },
    extend: 'SIE.cmd.FormSave',
    canExecute: function (view) {
        var cur = view.getCurrent();
        if (cur == null || cur.isNew()) {
            return false;
        }
        //SIE.Warehouses.CountState.Audit.value(审批)=10
        //SIE.Warehouses.CountState.PartCount.value(部分盘点)=30
        //SIE.Warehouses.CountState.FinishCount.value(已盘点)=40
        if (cur.data.State === 10 ||
            cur.data.State === 30 ||
            cur.data.State === 40) {
            return true;
        }

        return false;
    },
    onSaved: function (view, res) {
        var me = this;
        var cur = view.getCurrent();
        if (view.getChildren()[1] && view.getChildren()[1].getData().data.items.all(function (p) { return false })) {
            //SIE.Warehouses.CountState.Close.value(关闭)=60
            cur.setState(60);
            cur.markSaved();
        }
        this.callParent(arguments);
    },
    onSavedMsg: function (view, res) {
        SIE.Msg.hide();
    },
    execute: function (view, source) {
        var me = this;
        SIE.Msg.askQuestion('关闭单据后将不能进行编辑，是否继续？'.t(), function () {
            SIE.Msg.wait("正在关闭......".t());
            var isValidator = me.onSaving(view);
            if (isValidator) {
                //提交时，数据设置为脏，重新保存并校验所有内容,包括明细。
                view.getCurrent().dirty = true;
                me.doSave(view);
                view.getCurrent().markSaved();
            }
        });
    }
});