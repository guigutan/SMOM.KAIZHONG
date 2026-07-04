Ext.define('SIE.Web.EMS.EquipMaint.Maintains.Plans.Scripts.ExeMaintainPlanBehavior',
    {
        /**
         * view生命周期函数--view生成前
         * @param {*} meta 实体视图元数据
         * @param {*} curEntity 当前操作实体(可空)
         */
        beforeCreate: function (meta, curEntity) {

        },
        /**
        * view生命周期函数--view生成后
        * @param {*} view 生成的view
        */
        onCreated: function (view) {
            var entity = CRT.Context.PageContext.getCurrentRecord();
            var params = CRT.Context.PageContext.getParams();
            if (params) {
                entity.setSelectBeginTime(params.PrecisePlanBeginDate);
                entity.setSelectEndTime(params.PrecisePlanEndDate);
                entity.setUpMaintainSummary(params.UpMaintainSummary);
                if (params.ProjectList != undefined) {
                    entity.ProjectList = params.ProjectList;
                }
            }
        },
        onViewReady: function (view) {
            var me = this;
            var current = view.getCurrent();
            view.mon(current, "propertyChanged", me.onPropertyChanged, view);
        },

        /**
         * view生命周期函数--数据加载后
         * @param {any} view 逻辑视图
         */
        onDataLoaded: function (view) {
            var me = this;
            //获取保养项目列表，绑定属性变更事件 
            var projView1 = view.getChildren().first(function (e) {
                return e.model === "SIE.EMS.Maintains.Projects.MaintainProject";
            });

            if (projView1) {
                var records = projView1.getData();

                if (records) {
                    projView1.mon(records, 'propertyChanged', me.onPropertyChanged, me);
                }
            };
        },
        /**
        * 属性变更处理*/
        onPropertyChanged: function (e) {
            var me = this;
            var entity = e.entity;
            if (e.property.length > 0) {
                if (e.property === 'ActualValue') {
                    var isMinPass = null;
                    var isMaxPass = null;
                    var minVal = entity.getMinValue();
                    var maxVal = entity.getMaxValue();
                    var actualValue = entity.getActualValue();

                    if (actualValue != null) {
                        if (minVal != null && maxVal != null) {
                            isMinPass = minVal <= actualValue;
                            isMaxPass = maxVal >= actualValue;
                            if (isMinPass && isMaxPass) {
                                //设置为合格
                                entity.setMaintainResult(1);
                            }
                            else {
                                entity.setMaintainResult(0);
                            }
                        }
                        else if (minVal != null && maxVal == null) {
                            isMinPass = minVal <= actualValue;
                            if (isMinPass) {
                                //设置为合格
                                entity.setMaintainResult(1);
                            }
                            else {
                                entity.setMaintainResult(0);
                            }

                        } else if (minVal == null && maxVal != null) {
                            isMaxPass = maxVal >= actualValue;
                            if (isMaxPass) {
                                //设置为合格
                                entity.setMaintainResult(1);
                            }
                            else {
                                entity.setMaintainResult(0);
                            }
                        }
                    } else {
                        entity.setMaintainResult(null);
                    }

                }
                if (e.property === 'MaintainResult') {
                    //保养执行主表根据保养项目的状态自动实时更新主表状态
                    var data = entity._MaintainPlan._ProjectList.data;
                    var state = true;
                    for (var i = 0; i < data.length; i++) {
                        if (data.items[0].data.MaintainResult == 0) {
                            state = false;
                        }
                    }
                    if (state) {
                        entity._MaintainPlan.setExeResult(10);
                    } else {
                        entity._MaintainPlan.setExeResult(20);
                    }
                }
                if (e.property === 'MaintainResult') {
                    //动态修改点检单结果
                    var datas = entity.getMaintainPlan().ProjectList().data;
                    var state = 10;
                    if (datas.items.any(function (p) { return p.getMaintainResult() === 0; }))
                        state = 20;
                    if (datas.items.all(function (p) { return p.getMaintainResult() === null; }))
                        state = null;
                    entity.getMaintainPlan().setExeResult(state);
                }
                if (e.property == 'ActBeginDate' || e.property == 'ActEndDate') {
                    if (entity.getExeState() == 0) {
                        if (entity.getActBeginDate() != null && entity.getActEndDate() != null) {
                            if (entity.getActBeginDate() > entity.getActEndDate()) {
                                entity.setActEndDate();
                                SIE.MessageBox.showMessage("保养结束时间不能小于保养开始时间".t());
                                return;
                            }
                        }
                    }
                }
            }
        }
    });
