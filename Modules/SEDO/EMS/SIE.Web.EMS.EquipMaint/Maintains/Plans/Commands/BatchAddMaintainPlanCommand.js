SIE.defineCommand('SIE.Web.EMS.EquipMaint.Maintains.Plans.Commands.BatchAddMaintainPlanCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "批量添加", group: "edit", iconCls: "icon-SectionExpandAll icon-green" },
    view: null,
    execute: function (view, source) {
        var me = this;
        SIE.AutoUI.getMeta({
            model: 'SIE.EMS.Maintains.Plans.MaintainPlan',
            module: 'SIE.EMS.Maintains.Plans.ViewModels.MaintainPlanViewModel,SIE.EMS',
            viewGroup: 'BatchAddMaintainPlanView',
            isDetail: true,
            isAggt: true,
            async: false,
            callback: function (res) {

                var model = SIE.getModel('SIE.EMS.Maintains.Plans.MaintainPlan');
                var entity = new model();
                entity.token = view.token;
                entity.setEquipMaintainType(2);

                var ui = SIE.AutoUI.generateAggtControl(res);
                ui._view._setDefaultValue(entity);
                ui._view.setData(entity);
                                
                me.view = ui._view;
                
                var win = SIE.Window.show({
                    title: '批量添加'.t(),
                    width: '55%',
                    height: '90%',
                    items: ui.getControl(),
                    callback: function (btn) {
                        if (btn == "确定".t()) {
                            me.save(view, win);
                            return false;
                        }
                    }
                });

                ui._view.mon(entity, "propertyChanged", me.onEntityPropertyChanged, me);
            }
        });
    },
    onEntityPropertyChanged: function (e) {

        // 设备保养类型变更时，清除设备
        if (e.property == 'EquipMaintainType') {
            var selEquipAccountView = this.view.findChild('SIE.Equipments.EquipAccounts.EquipAccountSelect');
            if (selEquipAccountView == null) {
                SIE.Msg.showInstantMessage('无设备台账子视图，请检查权限配置!'.t());
                return false;
            }

            selEquipAccountView.setData(null);
        }

        //车线改变量，获取产线下的设备
        if (e.property == 'ResourceId') {
            SIE.invokeDataQuery({
                type: "SIE.Web.EMS.EquipMaint.Maintains.Plans.DataQueryers.MaintainPlanQueryer",
                method: "GetEquipAccountByResourceId",
                params: [e.value],
                async: false,
                token: e.entity.belongsView.token,
                callback: function (res) {
                    store.setData(res.Result.data.items);                    
                }
            });
        }

        //保养周期类型变更时，清空已生成的保养计划清单
        if (e.property === "MaintainCycleType") {            
            var maintainPlanListView = this.view.findChild('SIE.EMS.Maintains.Plans.MaintainPlan');
            if (maintainPlanListView != null) {
                maintainPlanListView.setData(null);
            }
        }
    },
    validation: function (parent, selEquipAccounts) {
        var me = this;
        if (!parent.PlanBeginDate || !parent.PlanEndDate) {
            SIE.Msg.showInstantMessage('表单栏位不可为空!'.t());
            return false;
        }

        if (parent.PlanBeginDate - parent.PlanEndDate > 0) {
            SIE.Msg.showInstantMessage('计划开始时间不能大于计划结束时间!'.t());
            return false;
        }

        if (selEquipAccounts.length <= 0) {
            SIE.Msg.showInstantMessage('请选择设备!'.t());
            return false;
        }

        return true;
    },

    save: function (view, win) {
        var me = this;

        var parent = me.view.getData().getData();

        var selEquipAccountView = me.view.findChild('SIE.Equipments.EquipAccounts.EquipAccountSelect');
        if (selEquipAccountView == null) {
            SIE.Msg.showInstantMessage('无设备台账子视图，请检查权限配置!'.t());
            return false;
        }
        var selEquipAccounts = selEquipAccountView.getData().getData().items;
        var isValidation = me.validation(parent, selEquipAccounts);
        if (!isValidation)
            return false;

        var maintainPlanView = me.view.findChild('SIE.EMS.Maintains.Plans.MaintainPlan');
        if (maintainPlanView == null) {
            SIE.Msg.showInstantMessage('无保养计划视图，请检查权限配置!'.t());
            return false;
        }

        var maintainPlan = maintainPlanView.getData().getData().items;
        if (maintainPlan.length <= 0) {
            SIE.Msg.showInstantMessage('请生成保养计划!'.t());
            return false;
        }
        var plans = [];
        Ext.each(maintainPlan, function (item) {
            item.data.EquipMaintainType = parent.EquipMaintainType;
            plans.push(item.data);
        });

        var equipAccountIds = [];
        Ext.each(selEquipAccounts, function (item) {
            equipAccountIds.push(item.getId());
        });
        var indata = {};
        var data = { MaintainPlanList: plans, EquipAccountIds: equipAccountIds };
        console.log(data);
        indata.Data = Ext.encode(data);

        var emsCommonHelper = new SIE.Web.EMS.Common.Script.EmsCommonHelper();
        var mask = emsCommonHelper.showMask(win);

        view.execute({
            data: indata,
            success: function (res) { //回调

                mask.hide();
                win.close();
                if (res.Result.length <= 0) {
                    SIE.Msg.showInstantMessage('保存成功'.t());
                    view.reloadData();
                }
                else {
                    SIE.Msg.showMessage(res.Result);
                }
            },
            error: function (res) {
                mask.hide();
                SIE.Msg.showMessage(res.Message);
            }
        });
    }
});