SIE.defineCommand('SIE.Web.WorkBenchCommon.Workbench.Base.Commands.WorkbenchSaveCommand',
    {
        extend: 'SIE.cmd.Save',
        meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
        canExecute: function (view) {
            var result = view.getData().isDirty() || view.getData().dirty;
            return result;
        }
    });