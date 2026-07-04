SIE.defineCommand('SIE.Web.Recheck.Common.ItemRecheck.Commands.RecheckProgramDetailMoveUp', {
    extend: 'SIE.Web.Common.Sort.Commands.MoveUpCommand',
    meta: { text: "上移", group: "edit", iconCls: "icon-ArrowLongUp icon-blue" },
    execute: function (listView, source) {
        this.callParent(arguments);
        SIE.Web.Recheck.Common.ItemRecheck.Scripts.RecheckProgramDetailAction.ReSetDetailData(listView);
    },
});
