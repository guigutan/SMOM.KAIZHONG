Ext.define('SIE.Web.EMS.Equipments.Scripts.EquipModelBehavior',
    {
        /**
        * 数据加载完毕的处理事件
        * @param {*} view 生成的view
        */
        onDataLoaded: function (view) {
            var me = this;
            if (view) {
                var entity = view.getCurrent();
                if (entity) {
                    entity.Behavior = me;
                    me.tabEvent(view, entity);
                    me.bindEvent(view, entity);
                }
            }
        },

        /**
         * bindEvent 绑定事件
         * @param {any} me
         * @param {any} entity
         */
        bindEvent: function (view, entity) {
            var me = this;
            view.mon(entity, 'propertyChanged', me.onEntityPropertyChanged, view);
        },


        tabEvent: function (view, entity) {
            var isMeasureEquipment = false;
            var isSpecial = false;
            if (entity.getTypeCategory() != "") {
                SIE.invokeDataQuery({
                    type: "SIE.Web.Equipments.EquipModels.DataQuery.EquipModelDataQueryer",
                    method: "GetEquipmentTypeInfo",
                    params: [entity.getTypeCategory()],
                    async: false,
                    token: view.token,
                    success: function (res) {
                        if (res.Result != null) {
                            isMeasureEquipment = res.Result.Item1;
                            isSpecial = res.Result.Item2;
                        }
                    }
                });
            }
            var tabs = view._children;
            var tabPanel = tabs ? tabs[0].getControl().ownerCt.ownerCt : null;
            if (!tabPanel) return;
            var tabPanelItems = tabPanel.tabBar.items.items;
            var currentTab = tabPanel.getActiveTab().title;
            var flag = false;

            // 位置列表
            var locationView = view.findChild("SIE.Equipments.EquipModels.EquipModelLocation");
            // 电子行业
            var electricityView = view.getChildren().first(function (e) {
                return e.viewGroup === "EISBaseDataViewGroup";
            });
            // 计量校验规程
            var calibraView = view.findChild("SIE.EMS.MeteringEquipment.EquipModelExtensions.EquipModelCalibration")
            // 设备定检规程
            var regularView = view.findChild("SIE.EMS.SpecialEquipment.Models.EquipModelRegularInspection")

            Ext.each(tabPanelItems, function (item) {
                if (locationView && locationView.label === item.title) {
                    entity.getData().IndustryCategory === 1 ? item.show() : item.hide();
                }
                if (electricityView && electricityView.label === item.title) {
                    entity.getData().IndustryCategory === 1 ? item.show() : item.hide();
                }
                //else if (("设备能力".t() + "PCB行业基础数据".t()).indexOf(item.title) >= 0) {
                //    entity.getData().IndustryCategory === 2 ? item.show() : item.hide();
                //}
                else if (calibraView && calibraView.label === item.title) {
                    isMeasureEquipment == true ? item.show() : item.hide();
                }
                else if (regularView && regularView.label === item.title) {
                    isSpecial == true ? item.show() : item.hide();
                }

                if (item.isHidden() && currentTab == item.title) {
                    flag = true;
                }
            });

            if (flag) {
                tabPanel.setActiveTab(0);
            }
            else {
                tabPanel.setActiveTab(currentTab);
            }
        },

        /**
         * 属性变更事件
         * @param {any} e
         */
        onEntityPropertyChanged: function (e) {
            var view = e.entity.belongsView;
            var entity = view.getCurrent();
            if (entity) {
                e.entity.Behavior.tabEvent(view, entity);
            }
        },
    });