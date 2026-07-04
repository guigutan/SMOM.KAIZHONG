SIE.defineCommand('SIE.Web.EMS.Checks.Plans.Commands.SubmitExeCheckPlanCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "提交", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    /**
     * @override 是否可执行
     * @param {} view 
     * @returns {} 
     */
    canExecute: function (view) {
        var current = view.getCurrent();
        return current &&
            (current.getExeState() == 0 || current.getExeState() == 4);
    },
    /**
    * @override 执行提交
    * @returns {} 
    */
    execute: function (view, source) {
        var me = this;
        if (!me.onSaving(view))
            return;
        var isAbnormalExist = false;

        //提交时，数据设置为脏，重新保存并校验所有内容,包括明细。
        view.getCurrent().dirty = true;
        view.getChildren().filter(function (e) { return e.model === "SIE.EMS.Checks.Projects.CheckProject"; }).forEach(function (v) {
            v.getData().getData().items.forEach(function (detail) {
                detail.dirty = true;
                if (detail.getCheckResult() == 0) isAbnormalExist = true;
            });
        });
        if (isAbnormalExist) { // 存在不合格结果时，如果没有维修单，提示是否打开维修界面
            SIE.invokeDataQuery({
                type: "SIE.Web.EMS.EquipRepair.EquipRepairs.DataQuerys.EquipRepairDataQuery",
                method: "CheckPlanWithRepairBill",
                params: [view.getData().getEquipAccountId(), view.getData().getCheckPlanNo(), 0],
                async: false,
                token: view.token,
                callback: function (res) {
                    if (res.Success) {
                        if (res.Result) {
                            // 存在维修单直接保存
                            me.doSave(view);
                        }
                        else {
                            // 不存在提示打开维修界面
                            SIE.Msg.askQuestion("设备点检不合格是否需要报修".t(), function () {
                                me.AddRepairPage(me, view)
                            }, function () {
                                me.doSave(view);
                            });
                        }
                    }
                    else {
                        SIE.Msg.showError(res.Message);
                    }
                }
            })
        }
        else {
            me.doSave(view);
        }
        
    },
    onSaved: function (view, res) {
        this.callParent(arguments);
        
    },
    /**
    * override 重写保存后提示信息
    * @param {type} view
    * @param {type} res
    */
    onSavedMsg: function (view, res) {
        Ext.Msg.show({
            title: '点检执行'.t(),
            message: '提交成功'.t(),
            buttons: Ext.MessageBox.OK,
            icon: Ext.Msg.INFO,
            callback: function () {
                CRT.Workbench.closeCurrentTab();

                //刷新点检记录主界面
                CRT.Event.fire("SIE.EMS.Checks.Records.CheckRecord_refresh");

                //刷新点检主界面
                CRT.Event.fire("SIE.EMS.Checks.Plans.ViewModels.CheckPlanViewModel_refresh");
            }
        });
    },
    AddRepairPage: function (me, view) {
        CRT.Workbench.addPage({
            entityType: 'SIE.EMS.EquipRepair.EquipRepairs.EquipRepairBill',
            viewGroup: 'CreateRepairBillView',
            isDetail: true,
            isNew: true,
            ignoreQuery: true,
            title: '报修'.L10N(),
            module: view.module,
            isAggt: true,
            params: {
                //设备台账ID
                equipmentAccountID: this.view.getData().getEquipAccountId(),
                //来源单号 点检单号
                sourceNo: this.view.getData().getCheckPlanNo(),
                //来源类型 点检=0
                sourceType: 0
            }
        })
    },
});