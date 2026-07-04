SIE.defineCommand('SIE.Web.Resources.ShiftTypes.Commands.SetDefaultCommand', {
    meta: { text: "设置缺省", group: "edit", iconCls: "icon-ListConfig icon-blue" },

    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length !== 1) {
            return false;
        } 
        if (view.getCurrent() !== null && view.getCurrent().data.IsDefault !== 0)
            return false;
        if (view.getCurrent().isNew())
            return false;
        return true;
    },

    execute: function (view, source) {
        var entity = view.getCurrent();
        if (entity) {
            entity = entity.data;
            var msg = Ext.String.format('是否将班制{0}设置为缺省？'.t(), entity.Name);
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