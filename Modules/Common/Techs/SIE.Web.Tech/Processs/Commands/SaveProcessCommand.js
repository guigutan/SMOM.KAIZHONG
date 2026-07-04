SIE.defineCommand('SIE.Web.Tech.Processs.Commands.SaveProcessCommand', {
    extend: 'SIE.cmd.Save',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    doSave: function (view) {
        var me = this;
        var store = view.getData();
        store.data.each(function (p) {
            if (p.isEntityChildrenDirty()) {
                if (p._ParameterList && p._ParameterList.isDirty()) {
                    p._ParameterList.getData().items.forEach(function (p) { return p.dirty = true; });
                }
                if (p._CollectStepList && p._CollectStepList.isDirty()) {
                    p._CollectStepList.getData().items.forEach(function (p) { return p.dirty = true; });
                }
            }
        });
        me.callParent(arguments);
    },
});
