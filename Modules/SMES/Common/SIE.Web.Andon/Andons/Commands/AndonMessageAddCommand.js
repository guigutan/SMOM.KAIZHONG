SIE.defineCommand('SIE.Web.Andon.Andons.Commands.AndonMessageAddCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },
    onItemCreated: function (entity) {
        var model = entity.data;
        var me = this;
        this.view.execute({
            data: model,
            success: function (res) {
                var data = res.Result;
                entity.setPushPlugId_Display(data.PushPlugName);
                entity.setPushPlugId(data.PushPlugId);
            }
        }, me.view);
    }
});
