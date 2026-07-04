SIE.defineCommand('SIE.Web.Fixtures.Demands.Commands.AddDemandCommand', {
    extend: 'SIE.cmd.Add',
    /**
     * @override 执行
     * @param {} view 视图
     * @param {} source 
     * @returns {} 
     */
    execute: function (view, source) {
        var me = this;
        var editEntity = this.getEditEntity();
        var entityId = editEntity.entityName + '-' + editEntity.data.Id;
        var tabId = ('tab_' + entityId.replace(/\./g, '')).replace(/[.|,]/g, '');
        CRT.Workbench.addPage({
            tabId: tabId,
            entityType: me.view.model,
            recordId: editEntity.data.Id,
            title: me.getEditViewTitle(editEntity),
            isDetail: true,
            params: {
                tabId: tabId,
            }
        });
    }
});