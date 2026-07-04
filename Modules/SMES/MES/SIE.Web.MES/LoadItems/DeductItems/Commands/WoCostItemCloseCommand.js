SIE.defineCommand('SIE.Web.MES.LoadItems.DeductItems.Commands.WoCostItemCloseCommand', {
    meta: { text: "强制关闭", group: "edit", iconCls: "icon-CloseView icon-red" },
    canExecute: function (view) {
        var entity = view.getCurrent();
        if (entity != null && entity.isNew()) {
            return false;
        }
        //扣料状态: 30 失败/ 10 待提交
        if (entity == null || (entity.getState() != 30 && entity.getState() != 10)) {
            return false;
        }
        return true;
    },
    execute: function (view) {
        var data = view.getCurrent().data;
        SIE.Msg.wait("正在强制关闭......".t());
        view.execute({
            data: data,
            success: function (res) {
                SIE.Msg.showMessage("【强制关闭】完成!".t());
                view.reloadData();
            }
        });
    }
});
