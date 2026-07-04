SIE.defineCommand('SIE.Web.EMS.EquipMaint.Maintains.Plans.Commands.SubmitExeMaintainPlanCommand', {
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
            (current.getExeState() == 0 || current.getExeState() == 4) && current.getWhetherBegin();
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
        var maintainEndDate = new Date();
        if (view.getCurrent().getActBeginDate() > maintainEndDate) {
            SIE.Msg.showError("保养开始时间不能大于当前时间！".t());
            return;
        }
        else {
            view.getCurrent().setActEndDate(maintainEndDate);
        }
        //提交时，数据设置为脏，重新保存并校验所有内容,包括明细。
        view.getCurrent().dirty = true;
        view.getChildren().filter(function (e) {
            return e.model === "SIE.EMS.Maintains.Projects.MaintainProject";
        }).forEach(function (v) {
            v.getData().getData().items.forEach(function (detail) {
                detail.dirty = true;
                if (detail.getMaintainResult() == 0) {
                    isAbnormalExist = true;
                }
            });
        });

        if (isAbnormalExist) { // 存在不合格结果时，如果没有维修单，提示是否打开维修界面
            SIE.invokeDataQuery({
                type: "SIE.Web.EMS.EquipRepair.EquipRepairs.DataQuerys.EquipRepairDataQuery",
                method: "CheckPlanWithRepairBill",
                params: [view.getData().getEquipAccountId(), view.getData().getMaintainNo(), 1],
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
                            SIE.Msg.askQuestion("设备保养不合格是否需要报修".t(), function () {
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
    /**
    * 保存后，更新前端状态
    * @override
    * @param {} view 
    * @returns {} 
    */
    onSaved: function (view, res) {
        var me = this;
        var current = view.getCurrent();
        current.markSaved();

        CRT.Event.fire(view.model + '_refresh', view.getCurrent().getId());
        CRT.Event.fire(view.model + '_' + view.getCurrent().getId() + '_refresh', view.getCurrent().getId());
        CRT.Event.fire("SIE.EMS.Maintains.Plans.ViewModels.MaintainPlanViewModel_refresh");
        me.onSavedMsg(view, res);
    },

    /**
    * override 重写保存后提示信息
    * @param {type} view
    * @param {type} res
    */
    onSavedMsg: function (view, res) {
        Ext.Msg.show({
            title: '保养执行'.t(),
            message: '提交成功'.t(),
            buttons: Ext.MessageBox.OK,
            icon: Ext.Msg.INFO,
            callback: function () {
                CRT.Workbench.closeCurrentTab();
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
                //来源单号 保养单号
                sourceNo: this.view.getData().getMaintainNo(),
                //来源类型 保养=1
                sourceType: 1
            }
        })
    },
});