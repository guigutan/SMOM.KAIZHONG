SIE.defineCommand('SIE.Web.Equipments.EquipmentCards.Commands.SaveEquipCardCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    doSave: function (view) {
        var me = this;
        view.execute({
            success: function (res) {
                me.onSaved(view, res);
            }
        });
    }
});