SIE.defineCommand('SIE.Web.DIST.GoodsIssueAddCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-EditEntity icon-blue" },
    showView: function (editEntity) {
        var me = this;
        CRT.Workbench.addPage({
            entityType: me.view.model,
            title: '添加-配送管理'.L10N(),
            recordId: editEntity.getId(),
            viewGroup: "EditView",
            isDetail: true,
            ignoreQuery: true,
            isNew: true,
            params: {
                token: me.view.token,
            }
        });
    }
});