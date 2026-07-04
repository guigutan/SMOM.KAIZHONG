SIE.defineCommand('SIE.Web.Fixtures.MaintainTasks.Commands.DeleteDetailCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length !== 1) {
            return false;
        }
        var p = view.getCurrent();
        if (p.isNew())
            return true;
        return false;
    },
    execute: function (view) {
        var me = this;
        SIE.Msg.askQuestion("你确定删除该条数据吗？".L10N(), function () {
            view.removeSelection();
            var data = view.getData().data;
            if (data.length > 0) {
                view.getControl().setSelection(data.items[0]);
                view.setCurrent(data.items[0], true);
            } else {
                view.setCurrent(null, true);
            }
            var details = data.items;
            var noMaintain = 0;
            var isHaveMaintain = 0;
            Ext.each(details, function (detail) {
                var result = detail.getMaintainResult();
                if (result == null || result == "null") {
                    noMaintain++;
                } else {
                    isHaveMaintain++;
                }
            });
            var entity = view._parent.getCurrent();
            if (isHaveMaintain == 0)//待保养
                entity.setState(5);
            if (noMaintain == 0)//保养完成
                entity.setState(15);
            if (isHaveMaintain > 0 && noMaintain > 0)//部分保养
                entity.setState(10);
        });
    }
});