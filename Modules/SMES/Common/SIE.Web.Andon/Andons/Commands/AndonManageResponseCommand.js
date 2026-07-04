SIE.defineCommand('SIE.Web.Andon.Andons.Commands.AndonManageResponseCommand', {
    meta: { text: "响应", group: "edit", iconCls: "icon-Play icon-blue" },
    canExecute: function (view) {
        var entity = view.getCurrent();
        if (entity == null)
            return false;
        if (entity.getState() != 10) {
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
        var nowHandler = CRT.Context.GlobalContext.getContext('userInfo');
        var oldHandler = andonManage.getHandlerId();
        var reason = "";
        if (oldHandler != null) {
            reason = Ext.String.format('处理人由{0}变更为{1}'.t(), andonManage.getHandlerId_Display(), nowHandler.Name)
        }
        else {
            reason = Ext.String.format('处理人更新为{0}'.t(), nowHandler.Name);
        }
        var data = {
            AndonManageId: andonManageId,
            OperateType: 1,
            Reason: reason,
        };
        view.execute({
            data: data,
            success: function (res) {
                SIE.Msg.showMessage("响应成功!".t());
                if (view.viewGroup == 'LookUpViewGroup') { //查看界面逻辑
                    andonManage.setState(20);
                    andonManage.setHandlerId_Display(nowHandler.Name);
                    andonManage.setHandlerId(nowHandler.EmployeeId);
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
