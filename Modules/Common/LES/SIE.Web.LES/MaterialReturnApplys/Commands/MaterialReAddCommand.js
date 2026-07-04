SIE.defineCommand("SIE.Web.LES.MaterialReturnApplys.Commands.MaterialReAddCommand", {
    extend: "SIE.cmd.Add",
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
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
                            ReStatus: data.ReStatus,
                            ReType: data.ReType,
                        },
                        viewGroup: "DetailsView",
                        isDetail: true
                    })
                }
            })

        }

    }
})