SIE.defineCommand('SIE.Web.Fixtures.MaintainTasks.Commands.OnekeyPassCommand', {
    meta: { text: "一键合格", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    canExecute: function (view) {
        var me = this;
        var entitys = view.getData().data.items;
        var res = true;
        //Ext.each(entitys, function (entity) {
        //    if (entity.getCheckTag() != "1") {
        //        res = false;
        //        return false;
        //    }
        //});
        return res;
    },
    execute: function (view, source) {
        var me = this;
        var entitys = view.getData().data.items;
        Ext.each(entitys, function (entity) {
            if (entity.getCheckTag() == "1") {
                entity.setMaintainResult(1);
            }
        });
    }
});