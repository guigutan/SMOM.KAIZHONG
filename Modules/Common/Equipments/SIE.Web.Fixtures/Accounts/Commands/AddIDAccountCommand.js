SIE.defineCommand('SIE.Web.Fixtures.Accounts.Commands.AddIDAccountCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    showView: function (Entity) {
        CRT.Workbench.addPage({
            tabId: 'tab_AddIDAccount',
            entityType: 'SIE.Fixtures.Fixtures.Accounts.FixtureIDAccount',
            module: "SIE.Fixtures.Fixtures.Accounts.FixtureAccountModel,SIE.Fixtures",
            title: '添加-ID类工治具台帐'.t(),            
            isDetail: true,
        });
    }
});