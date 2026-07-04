SIE.defineCommand("SIE.Web.EMS.EquipLends.Commands.EquipLendAddCommand", {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },
    showView: function (entity) {
        if (entity) {
            var model = entity.data;
            var me = this;
            this.view.execute({
                data: model,
                success: function (res) {
                    var no = res.Result;
                    CRT.Workbench.addPage({
                        entityType: me.view.model,
                        recordId: entity.getId(),
                        title: me.getEditViewTitle(entity),
                        params: {
                            No: no
                        },
                        isDetail: true
                    });
                }
            })
        }
    }
})
