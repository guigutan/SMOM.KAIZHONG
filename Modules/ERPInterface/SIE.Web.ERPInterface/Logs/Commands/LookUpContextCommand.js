 SIE.defineCommand('SIE.Web.ERPInterface.Logs.Commands.LookUpContextCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "查看报文", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length != 1) {
            return false;
        }
        var entity = view.getCurrent();
        return entity != null;
    },
    execute: function (view, source) {
        var entity = view.getCurrent();
        var me = this;

        CRT.Workbench.addPage({
            entityType: "SIE.ERPInterface.Common.Logs.ErpUploadLog",
            title: me.getEditViewTitle(entity),
            /*  viewGroup: "AsnCheckViewGroup",*/
            isDetail: true,
            ignoreQuery: true,
            recordId: entity.getId()
        });
    }
});