SIE.defineCommand('SIE.Web.Packages.Packages.Commands.EditItemPkgRuleDtlCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    onEditting: function (entity) {
        if (entity) {
            this.mon(entity, 'propertyChanged', SIE.Web.Packages.Packages.Scripts.PackageRuleDetailAction.onEntityPropertyChanged, this);
        }
    },
});