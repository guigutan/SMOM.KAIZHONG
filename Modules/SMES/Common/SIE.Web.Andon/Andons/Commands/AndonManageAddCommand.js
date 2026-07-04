SIE.defineCommand('SIE.Web.Andon.Andons.Commands.AndonManageAddCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "触发", group: "edit", iconCls: "icon-AddEntity icon-green" },
    showView: function (entity) {
        if (entity) {
            var model = entity.data;
            var me = this;
            this.view.execute({
                data: model,
                success: function (res) {
                    var code = res.Result.AndonManageCode;
                    var triggerId = res.Result.TriggerId;
                    var triggerTime = res.Result.TriggerTime;
                    var faultTime = res.Result.FaultTime;
                    var workGroup = res.Result.WorkGroup;
                    var state = res.Result.State
                    CRT.Workbench.addPage({
                        entityType: me.view.model,
                        recordId: entity.getId(),
                        title: me.getEditViewTitle(entity),
                        params: {
                            AndonManageCode: code,
                            TriggerId: triggerId,
                            TriggerTime: triggerTime,
                            FaultTime: faultTime,
                            WorkGroup: workGroup,
                            State:state,
                        },
                        isDetail: true
                    });
                }
            }, me.view);
        }
    },
});
