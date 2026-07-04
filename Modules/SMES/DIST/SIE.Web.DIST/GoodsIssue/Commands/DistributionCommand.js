/*
 **载具关联布局JS
 * @class SIE.Web.DIST.GoodsIssue.Commands.DistributionCommand
 */
SIE.defineCommand('SIE.Web.DIST.GoodsIssue.Commands.DistributionCommand', {
    meta: { text: "载具关联", group: "edit", iconCls: "icon-People icon-blue" },
    extend: 'SIE.cmd.Edit',
    canExecute: function (view) {
        var current = view._current;
        return current;
    },
    execute: function (view, source) {
        var id = 'menu_' + 'SIE.Web.DIST.GoodsIssueViewModel,SIE.Web.DIST'.replace(/[.|,]/g, '');
        var curEntity = view.getCurrent();
        CRT.Workbench.addPage({
            tabId: id,
            entityType: 'SIE.Web.DIST.GoodsIssueViewModel',
            title: '载具关联'.L10N(),
            viewGroup: "EditView",
            module: view.module,
            isDetail: true,
            isNew: true,
            params: {
                token: view.token,
                goodsIssueId: curEntity.getId()
            }
        });
    }
});