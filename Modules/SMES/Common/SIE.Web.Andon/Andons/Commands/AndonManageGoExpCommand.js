SIE.defineCommand('SIE.Web.Andon.Andons.Commands.AndonManageGoExpCommand', {
    meta: { text: "查看经验库", group: "edit", iconCls: "icon-FileEye icon-blue" },
    execute: function (view, source) {
        CRT.Workbench.addPage({
            entityType: 'SIE.Andon.Andons.AndonExperience',
            title: '安灯经验库'.L10N(),
            module: 'SIE.Andon.Andons.AndonExperience,SIE.Andon',
            isAggt: true
        });
    }
});
