SIE.defineCommand('SIE.Web.EMS.Purchases.FixtureReceives.Commands.SaveFixtureReceiveCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    /**
     * @override 保存后处理
     * @param {any} view
     * @param {any} res
     */
    onSaved: function (view, res) {
        var me = this;
        var current = view.getCurrent();
        current.markSaved();
        me.onSavedMsg(view, res);
        CRT.Workbench.closeCurrentTab();
        CRT.Event.fire("SIE.EMS.Purchases.FixtureReceives.FixtureReceive_refresh");
    },
    execute: function (view, source) {
        var me = this;
        view.getCurrent().dirty = true;
        var childView = view._children.first(function (p) { return p.model === "SIE.EMS.Purchases.FixtureReceives.FixtureReceiveDetail"; });
        if (childView) {
            childView.getData().getData().items.forEach(function (item) {
                item.dirty = true;
            });
        }
        me.doSave(view);
    }
});