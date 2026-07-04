SIE.defineCommand('SIE.Web.Dock.DockQueues.Commands.AddAppointDockQueueCommand', {
    extend: 'SIE.Web.Dock.DockQueues.Commands.AddSceneDockQueueCommand',
    meta: { text: "预约取号", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    showView: function (entity) {
        let me = this;
        if (entity) {
            var model = entity.data;
            this.view.execute({
                data: model,
                isSubmmit: false,
                success: function (res) {
                    var data = res.Result;
                    entity.setNo(data.No);
                    entity.setTakeNoWay(1);
                    me.addSceneDockQueueView(entity);
                }
            }); 
        }
    },
});