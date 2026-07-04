SIE.defineCommand('SIE.Web.Fixtures.Accounts.Commands.AddCodeAccCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    showView: function (Entity) {
        CRT.Workbench.addPage({
            tabId: 'tab_addCodeAcc',
            entityType: 'SIE.Fixtures.Fixtures.Accounts.FixtureCodeAccount',
            module: "SIE.Fixtures.Fixtures.Accounts.FixtureAccountModel,SIE.Fixtures",
            title: '添加-编码类工治具台帐'.t(),
            isDetail: true,
            params: {
                tabId: 'tab_addCodeAcc',
            }
        });
    }
});
