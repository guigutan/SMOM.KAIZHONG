SIE.defineCommand('SIE.Web.MES.SpcFromMess.Commands.ReadonlyViewSpcFromMesCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "数据预览", group: "edit", iconCls: "icon-PageSearch icon-blue" },

    canExecute: function (view) {
        if (!view) { return false; }
        if (view.getSelection && typeof view.getSelection === 'function') {
            return view.getSelection().length === 1;
        }
        return true;
    },

    execute: function (view, source) {
        var selection = view.getSelection();
        if (selection && selection.length === 1) {
            this.showView(selection[0]);
        } else {
            SIE.Msg.showWarning('请选择一条记录');
        }
    },

    showView: function (editEntity) {
        if (!editEntity) { return; }
        var me = this;
        CRT.Workbench.addPage({
            tabId: 'spc_readonly_' + String(editEntity.getId()).replace(/[.|,]/g, ''),
            entityType: me.view.model,
            recordId: editEntity.getId(),
            viewGroup: "ReadonlyView",
            title: Ext.String.format('数据预览-{0}'.L10N(), editEntity.getNo()),
            isDetail: true,
            params: {
                token: me.view.token,
                action: 3
            }
        });
    }
});
