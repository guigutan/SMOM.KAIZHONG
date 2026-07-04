SIE.defineCommand("SIE.Web.MES.LoadItems.DeductItems.Commands.WoCostItemAddCommand", {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },
    onItemCreated: function (entity) {
        var model = entity.model;
        var me = this;
        this.view.execute({
            data: model,
            success: function (res) {
                var data = res.Result;
                if (data.CostNo === null || data.CostNo === "") {
                    var me = this;
                }
                else {
                    entity.setCostNo(data.CostNo);
                    entity.setState(data.State);
                }
            }
        }, me.view);
    },
});