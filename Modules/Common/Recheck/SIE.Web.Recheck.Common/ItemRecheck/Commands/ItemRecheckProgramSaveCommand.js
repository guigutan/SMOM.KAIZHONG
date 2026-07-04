/**
 * 保存按钮
 */
SIE.defineCommand('SIE.Web.Recheck.Common.ItemRecheck.Commands.ItemRecheckProgramSaveCommand', {
    extend: 'SIE.cmd.Save',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    canExecute: function (view) {
        return view.getData().isDirty() && view.getChildren().length > 0 && view.getChildren()[0].getData().data.items.length > 0;
    },
});