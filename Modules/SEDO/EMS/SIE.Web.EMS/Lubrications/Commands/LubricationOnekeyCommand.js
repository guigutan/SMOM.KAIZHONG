SIE.defineCommand('SIE.Web.EMS.Lubrications.Commands.LubricationOnekeyCommand', {
    meta: { text: "一键润滑", group: "edit", iconCls: "icon-EditEntity icon-green" },
    execute: function (view) {
        var entity = view.getData().data.items;
        Ext.Array.forEach(entity, function (data) {
            if (data.getActualValue() == null || data.getActualValue() == "") {
                data.setActualValue(data.getMinValue());
            }
        })
    }
});