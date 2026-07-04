Ext.define('SIE.Web.EMS.Lubrications.Behaviors.LubricationWorkHourListBehavior',
    {
        /**
         * view生命周期函数--数据加载后
         * @param {any} view 逻辑视图
         */
        onDataLoaded: function (view) {
            var me = this;
            this.view = view;
            var entity = view.getData();
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
                    //var p_StartDateTime = this.view.getParent().getCurrent().getStartDateTime();
                    //var p_EndDateTime = this.view.getParent().getCurrent().getEndDateTime();
                    //if (p_StartDateTime && startTime) {
                    //    if (startTime < p_StartDateTime) {
                    //        SIE.Msg.showInstantMessage('工时记录润滑开始时间不能小于表头润滑开始时间!'.t());
                    //        entity.setStartDateTime(null);
                    //        entity.setHours("");
                    //        return;
                    //    }
                    //}
                    //if (p_EndDateTime && endTime) {
                    //    if (endTime > p_EndDateTime) {
                    //        SIE.Msg.showInstantMessage('工时记录润滑结束时间不能小于表头润滑结束时间!'.t());
                    //        entity.setEndDateTime(null);
                    //        entity.setHours("");
                    //        return;
                    //    }
                    //}

                    if (startTime != null && endTime != null) {
                        if (endTime < startTime) {
                            if (e.property === "StartDateTime") {
                                entity.setStartDateTime("");
                            }
                            if (e.property === "EndDateTime") {
                                entity.setEndDateTime("");
                            }
                            entity.setHours("");
                            SIE.Msg.showInstantMessage('润滑结束时间不能小于润滑开始时间!'.t());
                        } else {
                            var Hour = (endTime - startTime) / 1000 / 60 / 60; // 小时
                            entity.setHours(Hour.toFixed(2));
                        }
                    } else {
                        entity.setHours("");
                    }
                }
            }
        }
    });