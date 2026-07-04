SIE.defineCommand('SIE.Web.Packages.QrCodeParseRules.Commands.AddQrCodeParseRuleDetailCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    canExecute: function (view) {
        if (view.getParent() == null || view.getParent().getCurrent() == null)
            return false;

        if (view.getParent().getCurrent().isNew()) return false;

        return true;
    },
    onItemCreated: function (entity) {
        var model = entity.data;
        var me = this;
        var parent = me.view.getParent().getCurrent();
        this.view.execute({
            data: model,
            isSubmmit: false,
            success: function (res) {
                var data = res.Result;
                var lineNo = me.view.getData().count();
                if (me.view.getData().count() > 1) {
                    var tempLineNoList = me.view.getData().getData().items.where(function (p) { return p.getLineNo() != null && p.getLineNo() != ""; }).select(function (p) { return parseInt(p.getLineNo()); });
                    lineNo = tempLineNoList.max() + 1;
                }

                if (parent)
                    entity.setInterceptWay(parent.getInterceptWay());

                entity.setLineNo(lineNo);
            }
        }, me.view);
    },
});