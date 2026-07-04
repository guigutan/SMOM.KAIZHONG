SIE.defineCommand("SIE.Web.LES.MaterialReturnApplys.Commands.MaterialReturnCancelCommand", {
    meta: { text: "取消", group: "edit", iconCls: "icon-FileReturn icon-red" },
    canExecute: function (view) {
        var entity = view.getCurrent();
        if (entity == null) {
            return false;
        }
        if (entity.getReStatus() != 1) {
            return false;
        }
        return true;
    },
    execute: function (view) {
        var entity = view.getCurrent().data;
        view.execute({
            data: entity,
            success: function (res) {
                if (res.Success) {
                    SIE.Msg.showMessage("取消成功".t());
                    view.reloadData();
                }
            }
        })
    }
})