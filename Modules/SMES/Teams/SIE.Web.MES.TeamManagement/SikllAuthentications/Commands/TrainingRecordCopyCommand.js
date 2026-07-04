SIE.defineCommand('SIE.Web.MES.TeamManagement.SikllAuthentications.TrainingRecordCopyCommand', {
    extend: 'SIE.cmd.Copy',
    meta: { text: "复制新增", group: "edit", iconCls: "icon-AddEntity icon-green" },

    _setCopyEntity: function (data) {
        this.callParent(arguments);
        data.setIsHistory(false);
        data.setDuration(null);
    }
});