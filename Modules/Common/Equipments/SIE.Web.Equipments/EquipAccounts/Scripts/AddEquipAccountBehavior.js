Ext.define('SIE.Web.Equipments.EquipAccounts.Scripts.AddEquipAccountBehavior',
    {
        SourceEquipId: null,
        /**
        * view生命周期函数--view聚合后
        * @param {*} view 生成的view
        */
        onViewReady: function (view) {
            var me = this;
            //设置主表model
            var entity = view.getCurrent();
            var params = CRT.Context.PageContext.getParams();
            if (params) {
                entity.data.TreePId = params.TreePId;
                me.SourceEquipId = params.SourceEquipId;
            }
            if (entity.getCode() == "") {
                SIE.invokeDataQuery({
                    type: "SIE.Web.Equipments.EquipAccounts.DataQuery.EquipAccountDataQueryer",
                    method: "GetEquipAccountNo",
                    params: [],
                    async: false,
                    token: view.token,
                    callback: function (res) {
                        entity.setCode(res.Result);
                    },
                });
            }

            entity.Behavior = me;
            me.tabEvent(view, entity.data);
            me.bindEvent(view, entity);
        },
        /**
         * view生命周期函数--数据加载后
         * @param {any} view 逻辑视图
         */
        onDataLoaded: function (view) {
            var me = this;
            if (view) {
                var entity = view.getCurrent();
                if (entity) {
                    me.tabEvent(view, entity.data);
                }
            }
        },
        tabEvent: function (view, entity) {
            var tabs = view._children;
            var tabPanel = tabs ? tabs[0].getControl().ownerCt.ownerCt : null;
            if (!tabPanel) return;
            var tabPanelItems = tabPanel.tabBar.items.items;
            var currentTab = tabPanel.getActiveTab().title;
            var flag = false;
            Ext.each(tabPanelItems, function (item) {
                if (("校验项目" + "仪器参数").indexOf(item.title) >= 0) {
                    (entity.CheckCategory != null && entity.CheckCategory != "") ? item.show() : item.hide();
                } else if (("位置列表" + "电子行业").indexOf(item.title) >= 0) {
                    entity.IndustryCategory == "1" ? item.show() : item.hide();
                } else if (("设备能力" + "PCB行业基础数据").indexOf(item.title) >= 0) {
                    entity.IndustryCategory == "2" ? item.show() : item.hide();
                } else if (("缸槽管理" + "PCB行业基础数据").indexOf(item.title) >= 0) {
                    record.data.IndustryCategory == "2" ? item.show() : item.hide();
                }

                if (item.isHidden() && currentTab == item.title)
                    flag = true;
            });

            if (flag)
                tabPanel.setActiveTab(0);
            else
                tabPanel.setActiveTab(currentTab);
        },
        /**
         * bindEvent 绑定事件
         * @param {any} me
         * @param {any} entity
         */
        bindEvent: function (view, entity) {
            var me = this;

            view.childLocationView = view
                .findChild('SIE.Equipments.EquipAccountLocations.EquipAccountLocation');

            view.childPCBView = view
                .getChildren().first(function (e) {
                    return e.viewGroup === "PCBBaseDataAddViewGroup";
                });

            view.childEISView = view
                .getChildren().first(function (e) {
                    return e.viewGroup === "EISBaseDataAddViewGroup";
                });

            if (view.childLocationView)
                view.childLocationView.loadChildData(true);

            if (view.childPCBView)
                view.childPCBView.loadChildData(true);

            if (view.childEISView)
                view.childEISView.loadChildData(true);

            view.mon(entity, 'propertyChanged', me.onEntityPropertyChanged, view);
        },
        /**
         * 选中数据变更处理事件
         * @param {any} g
         * @param {any} row
         * @param {any} col
         * @param {any} record
         * @param {any} tr
         * @param {any} rowindex
         */
        onControlCellClick: function (g, row, col, record, tr, rowindex) {
            var me = this;
            if (!record.data)
                return;
            var data = record.data;
            if (!me.childUnitItemView)
                return;
            if (data.IsNewStatus && data.IsNewStatus === true) {
                if (!me.unitItemInfos)
                    return;
                me.page.bindUnitItems(me, data);
            }
            else {
                me.page.loadUnitItems(me, data);
            }
        },
        /**
         * 属性变更事件
         * @param {any} e
         */
        onEntityPropertyChanged: function (e) {
            var me = this;
            var view = e.entity.belongsView;
            if (e.property.length > 0) {
                if (e.property === 'EquipModelId' && e.entity.data.EquipModelId != null && e.entity.data.EquipModelId != 0) {
                    //加载设备台账相关信息
                    SIE.invokeDataQuery({
                        type: "SIE.Web.Equipments.EquipAccounts.DataQuery.EquipAccountDataQueryer",
                        method: "GetEquipModelRelateInfos",
                        params: [e.entity.data],
                        async: false,
                        token: me.token,
                        callback: function (res) {
                            var info = res.Result;
                            if (info) {

                                //位置页签
                                if (me.childLocationView) {
                                    var control = me.childLocationView.getControl();
                                    var store = control.getStore();
                                    store.setData(info.LocationList);
                                }

                                //PCB套件页签
                                if (me.childPCBView) {
                                    var current = me.childPCBView.getCurrent();
                                    current.setAverageBeat(info.AverageBeat);
                                    current.setStandardCapacity(info.StandardCapacity);
                                    current.setCapacityUnit(info.CapacityUnit);
                                }

                                //电子套件页签
                                if (me.childEISView) {
                                    current = me.childEISView.getCurrent();
                                    current.setRailType(info.RailType);
                                    current.setVirtualDevice(info.VirtualDevice);
                                    current.setFeederBinding(info.FeederBinding);
                                    current.setFeederLocFailSafe(info.FeederLocFailSafe);
                                    current.setFeederBarcodeFailSafe(info.FeederBarcodeFailSafe);
                                    current.setIsDisabled(info.IsDisabled);
                                    current.setAgingType(info.AgingType);
                                    current.setProductionType(info.ProductionType);
                                }
                                e.entity.Behavior.tabEvent(view, info);
                            }
                        },
                    });
                }
            }
        },
    });