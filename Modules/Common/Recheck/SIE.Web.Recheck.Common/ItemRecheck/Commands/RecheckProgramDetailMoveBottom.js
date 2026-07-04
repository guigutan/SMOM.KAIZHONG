SIE.defineCommand('SIE.Web.Recheck.Common.ItemRecheck.Commands.RecheckProgramDetailMoveBottom', {
    extend: 'SIE.Web.Common.Sort.Commands.MoveBottomCommand',
    meta: { text: "置底", group: "business", iconCls: "iconfont icon-AlignBottom icon-blue" },
    execute: function (listView, source) {
        this.callParent(arguments);
        SIE.Web.Recheck.Common.ItemRecheck.Scripts.RecheckProgramDetailAction.ReSetDetailData(listView);
    },
});