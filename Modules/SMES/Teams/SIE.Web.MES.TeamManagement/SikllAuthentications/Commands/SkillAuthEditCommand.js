SIE.defineCommand('SIE.Web.MES.TeamManagement.SikllAuthentications.SkillAuthEditCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    getEditEntity: function () {
        var current = this.view.getCurrent();
        var data = current.data;
        data.SkillId_Display = data.SkillCode;
        return current;
    }, 
});