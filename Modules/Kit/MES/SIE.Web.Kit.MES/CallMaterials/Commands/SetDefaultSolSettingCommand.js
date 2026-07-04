SIE.defineCommand('SIE.Web.Kit.MES.CallMaterials.Commands.SetDefaultSolSettingCommand', {
    meta: { text: "设置缺省", group: "edit" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length != 1) {
            return false;
        }
        var entity = view.getCurrent();
        if (entity != null && entity.data.IsDefault == 0 && entity.getCreateBy()) return true;
        return false;
    },
    execute: function (view, source) {
        var entity = view.getCurrent();
        view.execute({
            data: entity.getId(),
            success: function (res) {
                if (res.Result === true) {
                    view.loadData();
                    view.getData().data.items.forEach(function (item) {
                        item.markSaved();
                    });
                }
            },
            error: function (res) {
                SIE.Msg.showMessage(res.Result);
            }
        });
    }
});