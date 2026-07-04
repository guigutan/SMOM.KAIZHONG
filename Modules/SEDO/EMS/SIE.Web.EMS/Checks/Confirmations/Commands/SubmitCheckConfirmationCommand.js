SIE.defineCommand('SIE.Web.EMS.Checks.Confirmations.Commands.SubmitCheckConfirmationCommand', {
    meta: { text: "点检确认", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    /**
     * @override 是否可执行
     * @param {} view 
     * @returns {} 
     */
    canExecute: function (view) {
        var current = view.getCurrent();
        return current && current.getExeState() == 5 && current.isDirty() && view.getIsReadonly() === false;
    },

    /**
    * @override 执行提交
    * @returns {} 
    */
    execute: function (view, source) {
        var me = this;

        var entity = view.getData().data;

        if (entity.ConfirmResult == null) {
            SIE.Msg.showError("点检确认结果不能为空！".t());
            return;
        }

        var confirmations = [];
        var confirmationControl = view.findChild("SIE.EMS.Checks.Confirmations.CheckConfirmation").getControl();
        var store = confirmationControl.getStore().data.items;
        if (store.length > 0) {
            Ext.each(store, function (el) {
                el.data.ConfirmResult = entity.ConfirmResult;
                el.data.ConfirmNote = entity.ConfirmNote;
                confirmations.push(el.data);
            });
        }

        var checkPlanInfo = {
            Id: entity.Id,
            ConfirmDeptId: entity.ConfirmDeptId,
            ConfirmResult: entity.ConfirmResult,
            ConfirmNote: entity.ConfirmNote,
        }
        var jsonString = JSON.stringify(confirmations);
        SIE.invokeDataQuery({
            method: 'SubmitCheckConfirmation',
            params: [jsonString, checkPlanInfo],
            action: 'queryer',
            type: 'SIE.Web.EMS.Checks.Confirmations.DataQuery.CheckConfirmationQueryer',
            token: view.token,
            success: function (res) {
                if (res.Success) {
                    var current = view.getCurrent();
                    current.markSaved();
                    view.setIsReadonly(true);

                    SIE.Msg.showInstantMessage('点检确认成功！'.t());

                    //保存成功后关闭当前页签
                    CRT.Workbench.closeCurrentTab();

                    //刷新点检记录主界面
                    CRT.Event.fire("SIE.EMS.Checks.Records.CheckRecord_refresh");

                    //刷新点检主界面
                    CRT.Event.fire("SIE.EMS.Checks.Plans.ViewModels.CheckPlanViewModel_refresh");
                }
            }
        })
    },
});