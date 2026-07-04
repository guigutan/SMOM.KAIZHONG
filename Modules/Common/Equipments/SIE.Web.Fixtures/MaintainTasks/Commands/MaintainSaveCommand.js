SIE.defineCommand('SIE.Web.Fixtures.MaintainTasks.Commands.MaintainSaveCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    onValidation: function (view) {
        var isValidation = true;
        var entity = view.getCurrent();
        var qty = entity.getQty();
        var passQty = entity.getPassQty();
        var ngQty = entity.getNgQty();

        if (passQty + ngQty !== qty) {
            SIE.Msg.showMessage("【合格数量】加【不合格数量】必须等于【治具数量】!".t());
            isValidation = false;
        }
        if (passQty != null && ngQty != null) {
            var details = entity._Details.data.items;
            Ext.each(details, function (detail) {
                var result = detail.getMaintainResult();
                if (result == null) {
                    SIE.Msg.showMessage("【保养项目】中所有【项目保养结论】都给出后才能维护【合格数量】和【不合格数量】!".t());
                    isValidation = false;
                    return;
                }
            });
        }
        return isValidation;
    }
});