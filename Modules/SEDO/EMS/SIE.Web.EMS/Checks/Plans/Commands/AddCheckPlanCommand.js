SIE.defineCommand('SIE.Web.EMS.Checks.Plans.Commands.AddCheckPlanCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit" },
    block: null,
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
        var opt = {
            model: 'SIE.EMS.Checks.Plans.ViewModels.AddCheckPlanViewModel',
            module: 'SIE.EMS.Checks.Plans.ViewModels.CheckPlanViewModel,SIE.EMS',
            viewGroup: 'DetailsView',
            title: '新增点检计划'.t()
        };

        me.getPageMeta(opt, view);
    },
    /**
    * 获取添加点检计划元数据
    * @param {*} opt 请求参数【model，viewGroup都可以在最上层配】
    */
    getPageMeta: function (opt, view) {
        var me = this;
        SIE.AutoUI.getMeta({
            model: opt.model,
            module: opt.module,
            isDetail: true,
            isAggt: true,
            async: false,
            callback: function (res) {
                var model = SIE.getModel('SIE.EMS.Checks.Plans.ViewModels.AddCheckPlanViewModel');
                var entity = new model();
                entity.token = view.token;
                entity.setEquipCheckType(0);
                //将设备设为当前选中行的使用中设备
                var curr = view.getCurrent();
                if (curr && curr.getUseState() == 5){
                    entity.setEquipAccountId(curr.getEquipAccountId());
                    entity.setEquipAccountId_Display(curr.getEquipAccountCode());
                    entity.setMachineNo(curr.getEquipAccountName());
                }
                var ui = SIE.AutoUI.generateAggtControl(res);
                ui._view._setDefaultValue(entity);
                ui._view.setData(entity);
                me.showDialog(ui, view, opt);
            }
        });
    },

    /**
    * 打开弹窗
    * @param {*} ui 获取的视图组件
    * @param {*} view 父级视图
    * @param {*} opt 弹窗参数设置-execute中配置
    */
    showDialog: function (ui, view, opt) {
        var me = this;
        var win = SIE.Window.show({
            title: opt.title,
            width: '50%',
            height: '30%',
            layout: 'fit',
            plain: true,
            buttonAlign: 'right',
            items: ui.getControl(),
            callback: function (btn) {
                if (btn === "确定".t()) {
                    me.save(view, win, ui);
                    return false;
                }
            }
        });
        return win;
    },
    // 添加验证
    saveValidate: function (entity) {
        if (entity.EquipAccountId == null || entity.EquipAccountId == 0) {
            SIE.Msg.showError("设备编码不能为空，请确认！".t());
            return false;
        }

        if (entity.BeginDate == null || entity.EndDate == null) {
            SIE.Msg.showError("计划开始日期和计划结束日期不能为空，请确认！".t());
            return false;
        }

        if (entity.BeginDate > entity.EndDate) {
            SIE.Msg.showError("计划开始日期不能大于计划结束日期，请确认！".t());
            return false;
        }

        if (entity.CheckTime != null) {
            if (entity.CheckTime <= 0) {
                SIE.Msg.showError("点检时长不能为负数或0，请确认！".t());
                return false;
            }
        }
    },
    /**
    * 弹窗-确定---执行保存
    * @param {*} view 父级视图
    * @param {*} win 弹窗
    * @param {*} opt 弹窗参数设置-execute中配置
    */
    save: function (view, win, ui) {
        var me = this;
        var entity = ui._view._current.data;

        // 数据验证
        me.saveValidate(entity);

        var data = { AddCheckPlan: entity };
        var indata = {};
        indata.Data = Ext.encode(data);

        SIE.invokeDataQuery({
            type: "SIE.Web.EMS.Checks.Plans.DataQuery.CheckPlanDataQueryer",
            method: "AddCheckPlanToVerifyRepeat",
            params: [entity],
            async: false,
            token: view.token,
            callback: function (res) {
                if (res.Result.ErrMsg != "") {
                    SIE.Msg.askQuestion(res.Result.ErrMsg + "，是否跳过添加？".t(), function () {
                        view.execute({
                            async: false,
                            data: indata,
                            isSubmmit: false,
                            success: function (res) {
                                win.close();
                                SIE.Msg.showInstantMessage('添加成功'.t());
                                view.reloadData();
                            }
                        });

                    })
                }
                else {
                    view.execute({
                        async: false,
                        data: indata,
                        isSubmmit: false,
                        success: function (res) {
                            win.close();
                            SIE.Msg.showInstantMessage('添加成功'.t());
                            view.reloadData();
                        }
                    });
                }
            }
        });
    }
});