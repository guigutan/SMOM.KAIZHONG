SIE.defineCommand('SIE.Web.RedCardManagment.RedCards.Commands.ImportRedCardItemSnCommand', {
    extend: 'SIE.Web.Common.Import.Commands.ImportCommandBase',
    meta: { text: "数据导入", group: "business", iconCls: "icon-Download icon-blue" },
    canExecute: function (view) {
        var entity = view.getParent().getCurrent();
        return entity && !entity.getItemSN();
    },
});