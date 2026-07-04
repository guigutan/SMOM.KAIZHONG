SIE.defineCommand('SIE.Web.Kit.MES.Storages.Commands.ItemStoragePropertySaveCommand', {
    meta: { text: "产线物料货位属性值保存", group: "edit" },
    execute: function (view, source, win) {
        var me = view;
        me.execute({
            command: Ext.getClassName(this),
            data: {
                DetailList: source.Selecteds,
                DetailId: source.DetailId,
            },
            success: function (res) {
                if (res.Result == "保存成功")
                    win.close();
            }
        });
    }
});