SIE.defineCommand("SIE.Web.MES.Outsourcing.Commands.InStockRecordAddCommand", {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加入库记录", group: "edit", iconCls: "icon-AddEntity icon-green" },
    canExecute: function (view) {
        const outsourcingRequest = view._parent.getCurrent();
        if (outsourcingRequest == null) {
            return false;
        }
        if (view._parent != null && view._parent.getCurrent() != null && view._parent.getCurrent().getData().OutsourcingState != 30) {
            return false;
        }
        return !(outsourcingRequest.isNew());
    },
    onItemCreated: function (entity) {
        var model = entity.model;
        var me = this;
        this.view.execute({
            data: model,
            success: function (res) {
                var data = res.Result;
                if (data) {
                    entity.setState(data.State);
                }

            }
        }, me.view);
    },
});