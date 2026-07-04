SIE.defineCommand('SIE.Web.EMS.EquipMaint.Maintains.Plans.Commands.AddMaintainPlanCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit" },
    _mainView: null,
    canExecute: function (view) {
        var me = this;
        var select = view.getSelection();
        if (select != null && select.length > 1) {
            return false;
        }
        return true;
    },
    execute: function (view, source) {
        var me = this;
        SIE.AutoUI.getMeta({
            model: 'SIE.EMS.Maintains.Plans.MaintainPlan',
            module: 'SIE.EMS.Maintains.Plans.ViewModels.MaintainPlanViewModel,SIE.EMS',
            viewGroup: 'AddMaintainPlan',
            isDetail: true,
            isAggt: true,
            async: false,
            callback: function (res) {
                var model = SIE.getModel('SIE.EMS.Maintains.Plans.MaintainPlan');
                var entity = new model();
                entity.token = view.token;
                entity.setEquipMaintainType(0);
                //将设备设为当前选中行的使用中设备
                var curr = view.getCurrent();
                if (curr  && (curr.getUseState() == 5 || curr.getUseState() == 40 || curr.getUseState() == 25)) {
                    entity.setEquipAccountId(curr.getEquipAccountId());
                    entity.setEquipAccountId_Display(curr.getEquipAccountCode());
                    entity.setMachineNo(curr.getEquipAccountName());
                }
                var ui = SIE.AutoUI.generateAggtControl(res);
                me._mainView = ui._view;
                ui._view._setDefaultValue(entity);
                ui._view.setData(entity);

                var win = SIE.Window.show({
                    title: '新增保养计划'.t(),
                    width: '55%',
                    height: '90%',
                    items: ui.getControl(),
                    callback: function (btn) {
                        if (btn === "确定".t()) {
                            me.save(view, win);
                            return false;
                        }
                    }
                });

                ui._view.mon(entity, "propertyChanged", me.onEntityPropertyChanged, me);
            }
        });
    },
    save: function (view, win) {
        var me = this;
        var plans = [];

        if (!me._mainView.getCurrent().data.EquipAccountId) {
            SIE.Msg.showInstantMessage('设备不可为空!'.t());
            return false;
        }

        var maintainPlanView = me._mainView.findChild('SIE.EMS.Maintains.Plans.MaintainPlan');
        if (maintainPlanView == null) {
            SIE.Msg.showInstantMessage('无保养计划视图，请检查权限配置!'.t());
            return false;
        }

        if (me._mainView.getCurrent().data.MaintainTime < 0) {
            SIE.Msg.showInstantMessage('保养计划时长不允许小于0，请修改!'.t());
            return false;
        }

        var maintainPlan = maintainPlanView.getData().getData().items;
        if (maintainPlan.length <= 0) {
            SIE.Msg.showInstantMessage('请生成保养计划!'.t());
            return false;
        }

        Ext.each(maintainPlan, function (item) {
            item.data.EquipMaintainType = me._mainView.getCurrent().data.EquipMaintainType;
            plans.push(item.data);
        });

        var indata = {};

        var data = {
            MaintainPlanList: plans,
            EquipAccountIds: [me._mainView.getCurrent().data.EquipAccountId]
        };

        indata.Data = Ext.encode(data);

        console.log(indata.Data);

        var mask = new Ext.LoadMask({
            target: view.getControl(),
            msg: "保存中".t(),
        });

        mask.show();

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
    },

    onEntityPropertyChanged: function (e) {
        var me = this;

        if (e.property === "MaintainCycleType") {
            //保养周期类型变更时，清空已生成的保养计划清单
            var maintainPlanListView = me._mainView.findChild('SIE.EMS.Maintains.Plans.MaintainPlan');
            if (maintainPlanListView != null) {
                maintainPlanListView.setData(null);
            }            
        }
    }
});