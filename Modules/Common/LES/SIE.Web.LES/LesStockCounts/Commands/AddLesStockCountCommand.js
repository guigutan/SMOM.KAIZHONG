SIE.defineCommand('SIE.Web.LES.LesStockCounts.Commands.AddLesStockCountCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    showView: function (entity) {
        if (entity) {
            var model = entity.data;
            var me = this;
            this.view.execute({
                data: model,
                isSubmmit: false,
                success: function (res) {
                    var data = res.Result;
                    CRT.Workbench.addPage({
                        entityType: me.view.model,
                        recordId: entity.getId(),
                        title: me.getEditViewTitle(entity),
                        params: {
                            No: data.No,
                            SourceType: data.SourceType,
                            OrderType: data.OrderType,
                            TaskLevel: data.TaskLevel,
                        },
                        isDetail: true
                    })
                }
            }, me.view);
        }
    },
});