SIE.defineCommand('SIE.Web.Kit.Mes.SingleLabels.Commands.CopySingleLabelCommand', {
    extend: 'SIE.cmd.Copy',
    meta: { text: "复制新增", group: "edit", iconCls: "icon-AddEntity icon-green" },
    _setCopyEntity: function (data) {
        this.callParent(arguments);
        data.setLabelState(0);
    }
});