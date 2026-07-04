SIE.defineCommand('SIE.Web.EMS.SpareParts.OutDepots.Commands.SaveOutDepotCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "保存", group: "edit", iconCls: "icon-Check icon-blue" },
    canExecute: function (view) {
        if (view.getCurrent() == null) {
            return false;
        }
        return true;
    },
    execute: function (view, source) {
        var me = this;
        view.getCurrent().dirty = true;
        view.getChildren().filter(function (e) { return e.model === "SIE.EMS.SpareParts.OutDepots.OutDepot"; }).forEach(function (v) {
            v.getData().getData().items.forEach(function (detail) {
                if (detail.getIsEnable())
                    detail.dirty = true;
            });
        });
        me.doSave(view);
    },
    onSaved: function (view, res) {
        CRT.Event.fire(view.model + "_refresh", view.getCurrent().data.Id);
        SIE.Msg.showInstantMessage('保存成功！');
    }
});