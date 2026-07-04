SIE.defineCommand('SIE.Web.Dock.YardZones.Commands.SaveYardZoneCommand', {
    meta: { text: "保存", group: "edit", iconCls: "iconfont icon-SaveEntity icon-blue" },
    extend: 'SIE.cmd.Save',
    canExecute: function (view) {
        var data = view.getData();
        if (!data.isDirty()) {
            return false;
        }
        return true;
    },
    execute: function (view, source) {
        var me = this;
        var isValidator = me.onSaving(view);
        if (isValidator) {
            me.doSave(view);
        }
    }
});