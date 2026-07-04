SIE.defineCommand("SIE.Web.LES.MaterialPreparations.Commands.AddWorkOrderPrepareCommand", {
    extend: 'SIE.cmd.Add',
    meta: { text: "工单备料申请", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    showView: function (entity) {
        if (entity) {
            var model = entity.data;
            var me = this;
            me.view.execute({
                data: model,
                success: function (res) {
                    var data = res.Result;
                    CRT.Workbench.addPage({
                        entityType: me.view.model,
                        title: me.getEditViewTitle(entity),
                        recordId: entity.getId(),
                        params: {
                            No: data.No,
                            PrepareType: data.PrepareType, // 车间领料
                        },
                        viewGroup: "WorkOrderModeViewStr",
                        isDetail: true
                    })
                }
            })

        }

    }
})
