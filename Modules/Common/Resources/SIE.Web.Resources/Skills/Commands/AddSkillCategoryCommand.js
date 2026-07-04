SIE.defineCommand('SIE.Web.Resources.Skills.Commands.AddSkillCategoryCommand', {
    meta: { text: "分类维护", group: "edit", iconCls: "icon-Repair icon-blue" },
    execute: function (view, source) {
        CRT.Workbench.addPage({
            entityType: 'SIE.Resources.Skills.SkillCategory',
            title: '技能分类'.L10N(),
            module: view.module,
            isAggt: true
        });
    }
});