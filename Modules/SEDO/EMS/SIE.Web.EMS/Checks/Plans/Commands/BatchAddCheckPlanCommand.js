SIE.defineCommand('SIE.Web.EMS.Checks.Plans.Commands.BatchAddCheckPlanCommand', {
    meta: { text: "批量添加", group: "edit", iconCls: "icon-SectionExpandAll icon-green" },
    /**
    * 执行保存
    * @param {*} view 视图
    * @param {*} source 源
    */
    execute: function (view, source) {
        var me = this;

        var opt = {
            model: 'SIE.EMS.Checks.Plans.ViewModels.AddCheckPlanViewModel',
            module: 'SIE.EMS.Checks.Plans.ViewModels.CheckPlanViewModel,SIE.EMS',
            viewGroup: "BatchAddCheckPlanView",
            title: '批量添加'.t()
        };

        me.getPageMetaForBatchAdd(opt, view);
    },
    // 添加验证
    saveValidate: function (data) {
        if (data.EquipCheckType == 1 && (data.ResourceId == 0 || data.ResourceId == null)) {
            SIE.Msg.showError("点检类型为产线时产线编码不能为空，请确认！".t());
            return false;
        }


        if (!data.BeginDate || !data.EndDate) {
            SIE.Msg.showError("计划开始日期和计划结束日期不能为空，请确认！".t());
            return false;
        }

        if (data.BeginDate - data.EndDate > 0) {
            SIE.Msg.showError("计划开始日期不能大于计划结束日期，请确认！".t());
            return false;
        }

        if (data.CheckTime != null) {
            if (data.CheckTime <= 0) {
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
        var data = ui._view.getData().getData();
        me.saveValidate(data);

        me.doSave(view, win, ui);
    },

    /**
    * 保存中
    * @param {*} view 父级视图
    * @param {*} win 弹窗
    * @param {*} ui
    */
    doSave: function (view, win, ui) {
        //1.0 获取点检计划
        var data = ui._view.getData().getData();
        var addCheckPlan = {};
        addCheckPlan.CheckCycleType = data.CheckCycleType;
        addCheckPlan.BeginDate = new Date(data.BeginDate);
        addCheckPlan.EndDate = new Date(data.EndDate);
        addCheckPlan.CheckBeginDate = new Date(data.CheckBeginDate);
        addCheckPlan.CheckEndDate = new Date(data.CheckEndDate);
        addCheckPlan.WhetherAcrossDay = data.WhetherAcrossDay;
        addCheckPlan.CheckTime = data.CheckTime;
        addCheckPlan.ResourceName = "1";//有值则代表需要校验
        //2.0 获取设备台账数据
        var selEquipAccounts = ui._view.getChildren()[0].getData().getData().items;
        if (selEquipAccounts.length <= 0) {
            SIE.Msg.showInstantMessage('请选择设备!'.t());
            return false;
        }
        var equipAccountIds = [];
        Ext.each(selEquipAccounts, function (item) {
            equipAccountIds.push(item.data.Id);
        });
        var indata = {};
        var data = { AddCheckPlan: addCheckPlan, EquipAccountsIds: equipAccountIds };
        indata.Data = Ext.encode(data);

        //win.setLoading(true); //开始提交
        //3.0执行保存
        SIE.invokeDataQuery({
            type: "SIE.Web.EMS.Checks.Plans.DataQuery.CheckPlanDataQueryer",
            method: "BatchAddCheckPlanToVerifyRepeat",
            params: [addCheckPlan, equipAccountIds],
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
    },

    onEntityPropertyChanged: function (e) {
        var store = e.entity.belongsView._children[0].getControl().store;
        if (e.property == 'EquipCheckType') {

            e.entity.belongsView._children[0].getControl().store.data.each(function (item) {
                store.remove(item);
            });
        }
        if (e.property == 'ResourceId') {
            SIE.invokeDataQuery({
                type: "SIE.Web.EMS.Checks.Plans.DataQuery.CheckPlanDataQueryer",
                method: "GetEquipAccountByResourceId",
                params: [e.value],
                async: false,
                token: e.entity.belongsView.token,
                callback: function (res) {
                    store.setData(res.Result.data.items);
                    store.data.each(function (item) {
                        item.commit();
                    });
                    e.entity.belongsView._children[0].syncCmdState();
                }
            });
        }
    },

    /**
    * 获取批量添加点检计划元数据
    * @param {*} opt 请求参数【model，viewGroup都可以在最上层配】
    */
    getPageMetaForBatchAdd: function (opt, view) {
        var me = this;
        SIE.AutoUI.getMeta({
            model: opt.model,
            module: opt.module,
            viewGroup: opt.viewGroup ? opt.viewGroup : null,
            isDetail: true,
            isAggt: true,
            async: false,
            callback: function (res) {
                var model = SIE.getModel('SIE.EMS.Checks.Plans.ViewModels.AddCheckPlanViewModel');
                var entity = new model();
                entity.token = view.token;
                entity.setEquipCheckType(2);
                var ui = SIE.AutoUI.generateAggtControl(res);
                ui._view._setDefaultValue(entity);
                ui._view.setData(entity);
                var win = SIE.Window.show({
                    title: opt.title,
                    width: '70%',
                    height: '90%',
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
                ui._view.mon(entity, "propertyChanged", me.onEntityPropertyChanged, me);
            }
        });
    },
});