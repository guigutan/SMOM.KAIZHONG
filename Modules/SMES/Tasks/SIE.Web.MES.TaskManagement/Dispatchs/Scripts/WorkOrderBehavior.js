Ext.define('SIE.Web.MES.TaskManagement.WorkOrderBehavior', {
    /**
     * view生命周期函数--view生成后
     * @param {DetailView} view 生成的view
     */
    onViewReady: function (view) {
        var task = view._children.first(function (p) { return p.model === 'SIE.MES.TaskManagement.Dispatchs.DispatchTask'; });
        var taskPanel = task.getControl().up().up().up();
        var cmd = view._commands.items.first(function (p) { return p.meta.command === 'SIE.Web.MES.TaskManagement.Dispatchs.GenerateTaskBIllCommand'; });
        if (!taskPanel && !cmd)
            return;
        SIE.invokeDataQuery({
            method: 'GetWoIsGenerateTask',
            action: 'queryer',
            type: 'SIE.Web.MES.TaskManagement.Dispatchs.DispatchDataQueryer',
            token: view.token,
            success: function (res) {
                if (!res.Result) {
                    if (taskPanel)
                        taskPanel.setVisible(false);
                    if (cmd) {
                        cmd.canVisible = function (view, source) {
                            return false;
                        };
                        view.syncCmdState(view);
                    }
                }
            }
        });
    },
});