/**
 * 班组排班命令
 */
SIE.defineCommand('SIE.Web.MES.TeamManagement.ShiftSchedules.ScheduleCommand', {
    meta: { text: "排班", group: "business", iconCls: "icon-Edit icon-blue" },
    /**
     * 排班命令执行方法，创建排班界面
     * @param {ListLogicalView} view 排班表列表逻辑视图
     * @param {source} source 源
     */
    execute: function (view, source) {
        var id = 'menu_' + 'SIE.MES.TeamManagement.ShiftSchedules.ShiftScheduleViewModel,SIE.MES.TeamManagement'.replace(/[.|,]/g, '');
        var tabItem = CRT.Workbench.getTabById(id);
        if (tabItem) {
            CRT.Workbench.getTabPanel().setActiveItem(tabItem);
            return;
        }
        CRT.Workbench.addPage({
            tabId: id,
            title: '排班'.t(),
            pageClass: 'SIE.Web.MES.TeamManagement.ShiftSchedules.SchedulePage',
            entityType: 'SIE.MES.TeamManagement.ShiftSchedules.ShiftScheduleViewModel',
            params: {
                tabId: id,
                token: view.token,
                module: view.module
            }
        });
    }
});