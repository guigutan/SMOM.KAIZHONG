SIE.defineCommand('SIE.Web.EMS.Purchases.EquipmentSetups.Commands.FormSaveEquipSetupCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    onSaved: function (view, res) {
        this.callParent(arguments);
        //CRT.Workbench.closeCurrentTab();
    }
});