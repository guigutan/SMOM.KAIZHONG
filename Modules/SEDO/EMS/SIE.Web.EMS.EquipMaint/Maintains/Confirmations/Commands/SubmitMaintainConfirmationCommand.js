SIE.defineCommand('SIE.Web.EMS.EquipMaint.Maintains.Confirmations.Commands.SubmitMaintainConfirmationCommand', {
    //extend: 'SIE.cmd.FormSave',
    meta: { text: "保养确认", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },

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
            SIE.Msg.showError("保养确认结果不能为空！".t());
            return;
        }

        var confirmations = [];
        var confirmationControl = view.findChild("SIE.EMS.Maintains.Confirmations.MaintainConfirmation").getControl();
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
            method: 'SubmitMaintainConfirmation',
            params: [jsonString, checkPlanInfo],
            action: 'queryer',
            type: 'SIE.Web.EMS.EquipMaint.Maintains.Confirmations.DataQuery.MaintainConfirmationQueryer',
            token: view.token,
            success: function (res) {
                if (res.Success) {
                    (function (view, res) {
                        var me = this;
                        var current = view.getCurrent();
                        current.markSaved();
                        view.setIsReadonly(true);
                        SIE.Msg.showInstantMessage('保养确认成功！'.t());

                        //保存成功后关闭当前页签
                        window.setTimeout(function () {
                            CRT.Event.fire(view.model + "_refresh", view.getCurrent().data.Id);
                            CRT.Workbench.closeCurrentTab();
                        }, 1500);                        
                    })(view, res);
                }
            }
        })
    },

    /**
     * @override 保存后处理
     * @param {any} view
     * @param {any} res
     */
    onSaved: function (view, res) {
        var me = this;
        var current = view.getCurrent();
        current.markSaved();
        view.setIsReadonly(true);
        me.onSavedMsg(view, res);

        //保存成功后关闭当前页签
        window.setTimeout(function () {
            CRT.Event.fire(view.model + "_refresh", view.getCurrent().data.Id);
            CRT.Workbench.closeCurrentTab();
        }, 1500);
        return;
    },

    /**
    * override 重写保存后提示信息
    * @param {type} view
    * @param {type} res
    */
    onSavedMsg: function (view, res) {
        SIE.Msg.showInstantMessage('保养确认成功！'.t());
    }
});