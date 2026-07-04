SIE.defineCommand('SIE.Web.Resources.WipResources.Commands.WipResourceStopCommand', {
    meta: { text: "停用", group: "edit", iconCls: "icon-Pause icon-blue" },
    canExecute: function (view) {
        if (view.getSelection().length > 0 && view.getCurrent() !== null) {
            var flag = true;
            Ext.each(view.getSelection(), function (item) {
                if (item.data.ResourceState != 1) {
                    flag = false;
                }
            });
            return flag;
        }
        return false;
    },
    execute: function (view) {
        var me = view;
        var data = [];
        var entitys = view.getSelection();
        Ext.each(entitys, function (entity) { data.push(entity.data); });
        var msg = Ext.String.format('是否把选择的{0}条生产资源设置为停用状态?'.t(), entitys.length);
        SIE.Msg.askQuestion(msg, function () {
            view.execute({
                data: data,
                success: function (res) {
                    view.loadData();
                    SIE.Msg.showMessage('停用成功'.L10N());
                }
            }, view);
        });
    }
});


