SIE.defineCommand('SIE.Web.MES.TeamManagement.SikllAuthentications.SkillAuthCopyCommand', {
    extend: 'SIE.cmd.Copy',
    meta: { text: "复制新增", group: "edit", iconCls: "icon-AddEntity icon-green" }, 

    _setCopyEntity: function (data) {
        this.callParent(arguments);
        data.SkillId_Display = data.SkillCode;
    }
});