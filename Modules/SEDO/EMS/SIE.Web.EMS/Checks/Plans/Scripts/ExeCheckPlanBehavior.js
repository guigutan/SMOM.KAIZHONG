Ext.define('SIE.Web.EMS.Checks.Plans.Scripts.ExeCheckPlanBehavior',
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
                entity.setLastCheckSummary(params.LastCheckSummary);
                //只有待执行和执行中才赋值当前责任人
                if (entity.getExeState() == 0 || entity.getExeState() == 4) {
                    entity.setCheckEmployeeId(CurUserStateHelper.getSessionUser().EmployeeId);
                    entity.setCheckEmployeeId_Display(CurUserStateHelper.getSessionUser().Name);
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
            //获取点检项目列表，绑定属性变更事件
            var projView = view.getChildren().first(function (e) { return e.viewConfig === "SIE.Web.EMS.Checks.Projects.CheckProjectViewConfig"; });
            if (projView) {
                var records = projView.getData();
                if (records) {
                    projView.mon(records, 'propertyChanged', me.onPropertyChanged, me);
                }
            };
        },

        /**
        * 属性变更处理*/
        onPropertyChanged: function (e) {
            var me = this;
            var entity = e.entity;
            if (e.property.length > 0) {
                if (e.property === 'DepartmentId') {
                };
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
                                entity.setCheckResult(1);
                            }
                            else {
                                entity.setCheckResult(0);
                            }
                        }
                        else if (minVal != null && maxVal == null) {
                            isMinPass = minVal <= actualValue;
                            if (isMinPass) {
                                //设置为合格
                                entity.setCheckResult(1);
                            }
                            else {
                                entity.setCheckResult(0);
                            }

                        } else if (minVal == null && maxVal != null) {
                            isMaxPass = maxVal >= actualValue;
                            if (isMaxPass) {
                                //设置为合格
                                entity.setCheckResult(1);
                            }
                            else {
                                entity.setCheckResult(0);
                            }
                        }
                    } else {
                        entity.setCheckResult(null);
                    }
                }
                if (e.property === 'CheckResult') {
                    //修改点检项结果，清空缺陷描述
                    entity.setDefectDesc(null);
                    //动态修改点检单结果
                    var datas = entity.getCheckPlan().CheckProjectList().data;
                    var state = 10;
                    if (datas.items.any(function (p) { return p.getCheckResult() === 0; }))
                        state = 20;
                    if (datas.items.all(function (p) { return p.getCheckResult() === null; }))
                        state = null;
                    entity.getCheckPlan().setExeResult(state);
                }
            }
        },
        /**
        * 备件属性变更处理*/
        onSparePartPropertyChanged: function (e) {
            var me = this;
            var entity = e.entity;
            if (e.property.length > 0) {
                if (e.property === 'ApplyDetailId') {
                    if (SparePartAppId == null)
                        entity.setState(0);
                    else
                        entity.setState(entity.getAppState());
                }
            }
        },
    });
