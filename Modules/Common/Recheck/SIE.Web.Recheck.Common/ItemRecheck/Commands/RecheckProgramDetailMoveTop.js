SIE.defineCommand('SIE.Web.Recheck.Common.ItemRecheck.Commands.RecheckProgramDetailMoveTop', {
    extend: 'SIE.Web.Common.Sort.Commands.MoveTopCommand',
    meta: { text: "置顶", group: "business", iconCls: "iconfont icon-AlignTop icon-blue" },
    execute: function (listView, source) {
        this.callParent(arguments);
        SIE.Web.Recheck.Common.ItemRecheck.Scripts.RecheckProgramDetailAction.ReSetDetailData(listView);
    },
});