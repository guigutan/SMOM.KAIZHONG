SIE.defineCommand('SIE.Web.Resources.CalendarSchemes.Commands.CalendarSchemeSetDefaultCommand', {
    meta: { text: "设置缺省", group: "edit", iconCls: "icon-ListConfig icon-blue" },

    canExecute: function (view) {
        if (view.getSelection().length === 1 && view.getCurrent() !== null) {
            var curr = view.getCurrent().getData();
            return curr.IsDefault === 0 && curr.IsEnable === 1;
        }
        return false;
    },
    execute: function (view, source) {
        var entity = view.getCurrent();
        if (entity) {
            entity = entity.data;
            var msg = Ext.String.format('是否将日历方案{0}设置为缺省？'.t(), entity.Name);
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