SIE.defineCommand("SIE.Web.LES.MaterialReturnApplys.Commands.MaterialReturnSaveCommand", {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    doSave: function (view) {
        var me = this;
        var entity = view.getCurrent();
        if (entity) {
            var childStore = view.findChild("SIE.LES.MaterialReturnApplys.MaterialReturnApplyDetail").getData();
            var data = childStore.data.items;
            for (var i = data.length - 1; i >= 0; i--) {
                data[i].dirty = true;
            }
        }
        var children = view.getChildren();
        var withChildren = children.length > 0;
        view.execute({
            withChildren: withChildren,
            success: function (res) {
                me.onSaved(view, res);
            }
        });
    },
})