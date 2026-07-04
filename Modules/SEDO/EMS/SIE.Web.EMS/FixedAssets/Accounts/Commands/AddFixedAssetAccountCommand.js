SIE.defineCommand('SIE.Web.EMS.FixedAssets.Accounts.Commands.AddFixedAssetAccountCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    showView: function (entity) {
        if (entity) {
            var model = entity.data;
            var me = this;
            this.view.execute({
                data: model,
                success: function (res) {
                    var data = res.Result;
                    CRT.Workbench.addPage({
                        entityType: me.view.model,
                        recordId: entity.getId(),
                        title: me.getEditViewTitle(entity),
                        isNew: true,
                        params: {
                            Code: data.Code,
                            token: me.view.token,
                            woTabId: CRT.Workbench.getTabPanel().getActiveTab().getId(),
                            action: 0
                        },
                        isDetail: true
                    });
                }
            }, me.view);
        }
    }
});