SIE.defineCommand('SIE.Web.Packages.Packages.Commands.PackageRuleSaveCommand', {
    extend: 'SIE.cmd.Save',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    canExecute: function (view) {
        return view.getData().isDirty();
    }
});