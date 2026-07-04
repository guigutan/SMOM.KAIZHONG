SIE.defineCommand('SIE.Web.AbnormalInfo.AbnormalMonitors.Commands.AddTaskCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },

    execute: function (view, source) {

        var me = this;
        var indata = {};
        var entity = me.getEditEntity();
        indata.Data = Ext.encode(entity.data);
        view.execute({
            data: indata,
            success: function (res) {
                CRT.Workbench.addPage({
                    entityType: me.view.model,
                    recordId: entity.data.Id,
                    title: me.getEditViewTitle(entity),
                    isDetail: true,
                    params: {
                        Code: res.Result.Code,
                        TaskType: res.Result.TaskType,
                        TaskState: res.Result.TaskState,
                        IsAdd: true,
                    }
                });
            }
        });
    },
});