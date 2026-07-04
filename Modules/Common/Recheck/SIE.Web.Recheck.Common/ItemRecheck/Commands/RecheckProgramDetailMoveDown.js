SIE.defineCommand('SIE.Web.Recheck.Common.ItemRecheck.Commands.RecheckProgramDetailMoveDown', {
    extend: 'SIE.Web.Common.Sort.Commands.MoveDownCommand',
    meta: { text: "下移", group: "edit", iconCls: "icon-ArrowLongDown icon-blue" },
    execute: function (listView, source) {
        this.callParent(arguments);
        SIE.Web.Recheck.Common.ItemRecheck.Scripts.RecheckProgramDetailAction.ReSetDetailData(listView);
    },
});
