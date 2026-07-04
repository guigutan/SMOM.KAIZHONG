SIE.defineCommand("SIE.Web.MES.LoadItems.DeductItems.Commands.WoCostItemSubmitCommand", {
    meta: { text: "提交", group: "edit", iconCls: "icon-Check icon-blue" },
    canExecute: function (view) {
        var entity = view.getCurrent();
        if (entity != null && entity.isNew()) {
            return false;
        }
        //扣料状态 10 待提交
        if (entity == null || entity.getState() !== 10 || entity.getRecordType() === 30) {
            return false;
        }
        
        return true;
    },
    execute: function (view) {
        var data = view.getCurrent().data;
        SIE.Msg.wait("正在提交......".t());
        view.execute({
            data: data,
            success: function (res) {
                SIE.Msg.showMessage("提交成功!".t());
                view.reloadData();
            }
        });
    }
});
