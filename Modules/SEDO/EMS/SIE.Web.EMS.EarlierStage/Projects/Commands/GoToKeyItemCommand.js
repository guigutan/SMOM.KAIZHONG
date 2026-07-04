SIE.defineCommand('SIE.Web.EMS.EarlierStage.Projects.Commands.GoToKeyItemCommand', {
    meta: { text: "项目事项", group: "edit", iconCls: "icon-FormatListBulletedType icon-blue" },
    canExecute: function (view) {

        // 修改 B0033851 张俊杰2
        //【SEDO集成测试】项目管理功能的项目事项页签的【项目事项】按钮，需要点击一个项目事项才能点击，改成不需要选择事项就可点击
        //当前视图的上级视图不为空且只选中一笔资料时，可用
        if (view.getParent() == null
            || view.getParent().getSelection().length !== 1
            || view.getParent().getCurrent() == null) {
            return false;
        }

        return true;
    },
    execute: function (view, source) {

        if (view.getParent() != null
            && view.getParent().getSelection().length == 1
            && view.getParent().getCurrent() != null) {
            
            var projectCode = view.getParent().getCurrent().getCode();

            var title = '项目事项-'.t() + projectCode;

            var tabId = ('GoToKeyItem_001' );

            CRT.Workbench.addPage({
                title: title,
                tabId: tabId,
                entityType: 'SIE.EMS.EarlierStage.Projects.ProjectKeyItem',
                params: {
                    tabId: tabId,
                    ProjectCode: projectCode
                }
            });
        }


    }
});