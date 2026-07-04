Ext.define('SIE.Web.EMS.Lubrications.Behaviors.LubricationDetailBehavior',
    {
        /**
       * view生命周期函数--view聚合后
       * @param {*} view 生成的view
       */
        onViewReady: function (view) {
            //移除关闭事件
            view.mon(view, 'beforeClosewin', this.beforeClosewin, view);
        },
        /**
         * view生命周期函数--数据加载后
         * @param {any} view 逻辑视图
         */
        onDataLoaded: function (view) {
            var me = this;
            this.view = view;
            var entity = view.getData();
            SIE.invokeDataQuery({
                method: 'InitExeLubricationPlan',
                params: [entity.data.EquipAccountId, entity.data.DepartmentId],
                action: 'queryer',
                type: 'SIE.Web.EMS.Lubrications.DataQuery.LubricationDataQuery',
                token: view.token,
                success: function (res) {
                    if (res.Success) {
                        entity.setLastLubricationSummary(res.Result);
                    }
                }
            });
            view.mon(entity, "propertyChanged", me.onPropertyChanged, me);
        },

        /**
         * 属性变更处理
         * @param {any} 
         */
        onPropertyChanged: function (e) {
            if (e.property.length > 0) {
                var entity = e.entity;
                if (e.property === "StartDateTime" || e.property === "EndDateTime") {
                    let startTime = entity.getStartDateTime(); // 开始时间
                    let endTime = entity.getEndDateTime(); // 结束时间
                    if (startTime != null && endTime != null) {
                        if (endTime < startTime) {
                            if (e.property === "StartDateTime") {
                                entity.setStartDateTime("");
                            }
                            if (e.property === "EndDateTime") {
                                entity.setEndDateTime("");
                            }
                            entity.setTotalHours("");
                            SIE.Msg.showInstantMessage('润滑结束时间不能小于润滑开始时间!'.t());
                        } else {
                            var Hour = (endTime - startTime) / 1000 / 60 / 60; // 小时
                            entity.setTotalHours(Hour);
                        }
                    } else {
                        entity.setTotalHours("");
                    }
                }
                if (e.property === "DepartmentId") {
                    e.entity.belongsView.findChild("SIE.EMS.Lubrications.LubricationDetail").getData().removeAll()
                }
            }
        },
        beforeClosewin: function (returnObj) {
            this.mun(this, 'beforeClosewin');
        }
    });