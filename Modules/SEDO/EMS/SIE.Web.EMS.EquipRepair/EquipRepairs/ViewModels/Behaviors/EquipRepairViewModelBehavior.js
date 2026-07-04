Ext.define('SIE.Web.EMS.EquipRepair.EquipRepairs.ViewModels.Behaviors.EquipRepairBehavior',
    {
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
            var projView = view.getChildren().first(function (e) { return e.viewConfig === "SIE.Web.EMS.EquipRepair.EquipRepairs.EquipRepairViewConfig"; });
            if (projView) {
                var records = projView.getData();
                if (records) {
                    projView.mon(records, 'propertyChanged', me.onPropertyChanged, me);
                }
            };
        },
        onPropertyChanged: function (e) {
            
            var me = this;
            var entity = e.entity;
            if (e.property.length > 0) {
                if (e.property === 'EquipAccountId') {
                    if (this._children != undefined) {
                        this._children[0].getData().setEquipAccountId(e.entity.getEquipAccountId());
                    }
                }
                if (e.property === 'ProduceState') {
                    if (e.entity.getProduceState() == 0) {
                        e.entity.setUrgentDegree(0)
                    } else {
                        e.entity.setUrgentDegree(2)
                    }
                }
            }
        }

    });
