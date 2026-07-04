Ext.define('SIE.Web.EMS.Equipments.Scripts.AddEquipModelLubricationProjectBehavior',
    {
        /**
         * view生命周期函数--数据加载后
         * @param {any} view 逻辑视图
         */
        onDataLoaded: function (view) {
            var me = this;
            this.view = view;
            var entity = view.getData();
            me.bindEvent(view, entity);
            view.mon(entity, "propertyChanged", me.onEntityPropertyChanged, me);
        },

        /**
         * bindEvent 绑定事件
         * @param {any} me
         * @param {any} entity
         */
        bindEvent: function (view, entity) {
            var me = this;
            view.childEquipModelLubricaSparePartView = view.findChild('SIE.EMS.Equipments.Models.EquipModelLubricaSparePart');
            view.mon(entity, 'propertyChanged', me.onEntityPropertyChanged, view);
        },
        /**
         * 属性变更处理
         * @param {any} 
         */
        onEntityPropertyChanged: function (e) {
            var me = this;
            var view = e.entity.belongsView;
            var entity = e.entity;
            if (e.property.length > 0) {
                if (e.property === 'ProjectDetailId' && e.entity.data.ProjectDetailId != null && e.entity.data.ProjectDetailId != 0) {
                    //加载点检保养维护的备件清单信息
                    SIE.invokeDataQuery({
                        type: "SIE.Web.EMS.Equipments.Models.DataQuery.EquipModelDataQueryer",
                        method: "GetSparePartItemInfos",
                        params: [e.entity.data],
                        async: false,
                        token: view.token,
                        callback: function (res) {
                            var info = res.Result;
                            if (info) {
                                if (me.childEquipModelLubricaSparePartView) {
                                    info.EquipModelLubricaSparePartList.forEach(function (item) {
                                        item.SparePartId_Display = item.SparePartCode;
                                    })

                                    var controlSparePartItemView = me.childEquipModelLubricaSparePartView.getControl();
                                    var storeSparePartItem = controlSparePartItemView.getStore();
                                    storeSparePartItem.setData(info.EquipModelLubricaSparePartList);
                                }
                            }
                        },
                    });
                }

                if (e.property === "ProjectCycle") {
                    if (entity.getProjectCycle() != null && entity.getProjectCycle() != "" && entity.getProjectCycle() != '0') {
                        if (entity.getLastDate() != null) {
                            var nextDate = new Date(entity.getLastDate());
                            var data = nextDate.setDate(nextDate.getDate() + entity.getProjectCycle());
                            entity.setNextDate(new Date(data));
                        }
                    } else {
                        entity.setProjectCycle(null);
                        entity.setLastDate(null);
                        entity.setNextDate(null);
                        SIE.Msg.showInstantMessage('请填写有效周期!'.t());
                    }
                }
            }
        }
    });