SIE.defineCommand('SIE.Web.EMS.Tpms.Commands.SearchScoreDetailCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "查看明细", group: "edit", iconCls: "icon-PageSearch icon-blue" },
    canExecute: function (view) {
        if (view.getCurrent() == null) { return false; }
        if (view.getSelection().length !== 1) { return false; }
        return true;
    },
    execute: function (view, source) {
        var me = this;
        var c = view.getCurrent();
        me.showView(c);
    },
    showView: function (editEntity) {
        var me = this;
        var key = '';
        if (!this.viewMeta) {
            CRT.Workbench.addPage({
                ignoreCommands: false,
                title: '查看明细',
                entityType: this.view.model,
                isDetail: true,
                ignoreQuery: true,
                recordId: editEntity.getId()
            });
        }
    }
});