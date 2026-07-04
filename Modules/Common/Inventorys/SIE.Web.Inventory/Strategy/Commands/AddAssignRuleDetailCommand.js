SIE.defineCommand('SIE.Web.Inventory.Strategy.Commands.AddAssignRuleDetailCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    canExecute: function (view) {
        var rule = view.getParent();
        if (rule == null) return false;
        var ruleCur = view.getParent().getCurrent();
        if (ruleCur == null) return false;
        if (ruleCur != null) {
            if (ruleCur.data.IsDefault === true) return false;
        }

        return true;
    },
    onItemCreated: function (entity) {
        var model = entity.data;
        var me = this;
        var lineNo = me.view.getData().count();
        if (me.view.getData().count() > 1) {
            var tempLineNoList = me.view.getData().getData().items.where(function (p) { return p.getLineNo() != null; }).select(function (p) { return p.getLineNo(); });
            lineNo = tempLineNoList.max() + 1;
        }

        this.view.execute({
            data: model,
            isSubmmit: false,
            success: function (res) {
                var data = res.Result;
                entity.setLineNo(lineNo);
                entity.setState(10);
            }
        }, me.view);
    },
});