SIE.defineCommand('SIE.Web.Andon.Andons.Commands.AndonManageHandleCommand', {
    meta: { text: "处理完成", group: "edit", iconCls: "icon-CalendarCheck icon-blue" },
    canExecute: function (view) {
        var entity = view.getCurrent();
        if (entity == null)
            return false;
        if (entity.getState() != 20) {
            return false;
        }
        //列表界面防止多选
        if (view.viewGroup == 'ListView' && view.getSelection().length > 1) {
            return false;
        }
        return true;
    },
    execute: function (view, source) {
        var andonManage = view.getCurrent();
        var andonManageId = andonManage.getId();
        var data = {
            AndonManageId: andonManageId,
            OperateType: 3,
        };
        view.execute({
            data: data,
            success: function (res) {
                SIE.Msg.showMessage("处理完成成功!".t());
                if (view.viewGroup == 'LookUpViewGroup') { //查看界面逻辑
                    andonManage.setState(30);
                    andonManage.markSaved();
                    andonManage._OperateLogList.reload();
                    CRT.Event.fire(view.model + '_refresh');
                }
                else {
                    view.reloadData();
                }
            }
        });
    }
});
