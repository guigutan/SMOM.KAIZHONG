SIE.defineCommand('SIE.Web.Tech.Stations.Commands.StationEquipmentSetMasterCommand', {
    meta: { text: "设置为主设备", group: "edit", iconCls: "icon-ListConfig icon-blue" },

    canExecute: function (view) {
        if (view.getSelection().length === 1 && view.getCurrent() !== null) {
            var curr = view.getCurrent();
            return curr.isNew() != true && curr.getData().IsMaster === false;
        }
        return false;
    },
    execute: function (view, source) {
        var entity = view.getCurrent();
        if (entity) {
            entity = entity.data;
            var msg = Ext.String.format('是否将{0}设置为主设备？'.t(), entity.Name);
            SIE.Msg.askQuestion(msg, function () {
                view.execute({
                    data: entity,
                    success: function (res) {
                        view.loadData();
                    }
                }, view);
            });
        }
    }
});