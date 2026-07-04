SIE.defineCommand('SIE.Web.EMS.Purchases.EquipmentAcceptances.Commands.SaveEquipAcceptCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    onSaved: function (view, res) {
        this.callParent(arguments);
        CRT.Workbench.closeCurrentTab();
    }
});