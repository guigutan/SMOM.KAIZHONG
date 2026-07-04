SIE.defineCommand('SIE.Web.Packages.Packages.Commands.PackageRuleDefaultCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "设为缺省", group: "edit" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length != 1) {
            return false;
        }
        var entity = view.getCurrent();
        if (entity != null && entity.data.IsDefault == 0) return true;
        return false;
    },
    onEditting: function (entity) {
        var me = this;
        var items = me.view.getData().data.items;
        if (entity) {
            for (var i = 0; i < items.length; i++) {
                items[i].setIsDefault(0);
            }
            entity.setIsDefault(1);
        }
    }
});