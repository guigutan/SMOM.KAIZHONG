Ext.define('SIE.Web.EMS.MeteringEquipment.MeteringEquipmentAccounts.Behaviors.EquipModelCalibrationChangedBehavior',
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
                if (e.property === "PeriodDays") {
                    if (entity.getPeriodDays() != null && entity.getPeriodDays() != "" && entity.getPeriodDays() != '0') {
                        if (entity.getPrevInspectionDate() != null) {
                            var nextInspectionDate = new Date(entity.getPrevInspectionDate());
                            var data = nextInspectionDate.setDate(nextInspectionDate.getDate() + entity.getPeriodDays());
                            entity.setNextInspectionDate(new Date(data));
                        }
                    } else {
                        entity.setPeriodDays(null);
                        entity.setPrevInspectionDate(null);
                        entity.setNextInspectionDate(null);
                        SIE.Msg.showInstantMessage('请填写有效周期!'.t());
                    }
                }

                if (e.property === "PrevInspectionDate") {
                    if (entity.getPrevInspectionDate() != null) {
                        var nextInspectionDate = new Date(entity.getPrevInspectionDate());
                        if (entity.getPeriodDays() != null && entity.getPeriodDays() != "" && entity.getPeriodDays() != '0') {
                            var data = nextInspectionDate.setDate(nextInspectionDate.getDate() + entity.getPeriodDays());
                            entity.setNextInspectionDate(new Date(data));
                        } else {
                            entity.setPrevInspectionDate(null);
                            SIE.Msg.showInstantMessage('请填写有效周期!'.t());
                        }
                    } else {
                        entity.setNextInspectionDate(null);
                    }
                }
            }
        }
    });