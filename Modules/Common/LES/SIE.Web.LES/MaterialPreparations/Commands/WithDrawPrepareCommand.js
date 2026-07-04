SIE.defineCommand("SIE.Web.LES.MaterialPreparations.Commands.WithDrawPrepareCommand", {
    meta: { text: "取消", group: "edit", iconCls: "icon-FileReturn icon-red" },
    canExecute: function (view) {
        if (view.getSelection().length > 1) {
            return false;
        }
        var entity = view.getCurrent();
        if (entity == null) {
            return false;
        }
        var child = view.findChild("SIE.LES.MaterialPreparations.MaterialPreparationDetail");
        var store = child.getData().data.items;
        var flag = true;
        for (var i = 0; i < store.length; i++) {
            if (store[i].getPreDetailStatus() != 1) {
                flag = false;
                break;
            }
        }
        return flag;
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