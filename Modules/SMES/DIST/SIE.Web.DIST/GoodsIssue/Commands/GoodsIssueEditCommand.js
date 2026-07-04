SIE.defineCommand('SIE.Web.DIST.GoodsIssueEditCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "iconfont icon-EditEntity icon-blue" },
    canExecute: function (view) {
        if (view.getCurrent() == null) {//选中项修改
            return false;
        }
        var entity = view.getCurrent().data;
        if (entity.DistributionQty <= 0) {//选中项的配送数量小于等于0才能修改
            return true;
        }
        return false;
    },
    showView: function (editEntity) {
        var me = this;
        CRT.Workbench.addPage({
            entityType: me.view.model,
            title: '修改-配送管理'.L10N(),
            recordId: editEntity.getId(),
            viewGroup: "EditView",
            isDetail: true,
            params: {
                token: me.view.token,
                isEdit: true
            }
        });
    }
});