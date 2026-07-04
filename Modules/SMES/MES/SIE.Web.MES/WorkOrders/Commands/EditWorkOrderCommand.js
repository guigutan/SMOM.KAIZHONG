SIE.defineCommand('SIE.Web.MES.WorkOrders.EditWorkOrderCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", hierarchy: "工单生成" },
    canExecute: function (listView) {
        if (listView == null || listView.getCurrent() == null || listView.getSelection().length > 1) return false;
        var entity = listView.getCurrent();
        var eData = entity.data;
        return eData.IsPause == 1 && (eData.State == 0 || eData.State == 1);
    },
    showView: function (editEntity) {
        var me = this;
        CRT.Workbench.addPage({
            entityType: this.view.model,
            recordId: editEntity.getId(),
            viewGroup: "EditView",
            title: this.getEditViewTitle(editEntity),
            isDetail: true,
            params: {
                token: me.view.token,
                action: 2
            }
        });
    }
});