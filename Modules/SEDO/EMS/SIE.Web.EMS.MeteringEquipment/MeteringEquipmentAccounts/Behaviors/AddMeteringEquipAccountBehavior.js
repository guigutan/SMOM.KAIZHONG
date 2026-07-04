Ext.define('SIE.Web.EMS.MeteringEquipment.MeteringEquipmentAccounts.Behaviors.AddMeteringEquipAccountBehavior', {
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
                type: "SIE.Web.EMS.Equipments.Accounts.DataQuery.EquipAccountDataQueryer",
                method: "GetEquipAccountNo",
                params: [],
                async: false,
                token: view.token,
                callback: function (res) {
                    entity.setCode(res.Result);
                },
            });
        } else {
            SIE.invokeDataQuery({
                type: "SIE.Web.EMS.Equipments.Accounts.DataQuery.EquipAccountDataQueryer",
                method: "GetIsCalibration",
                params: [entity.data],
                async: false,
                token: view.token,
                callback: function (res) {
                    entity.setIsCalibration(res.Result);
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
            var modelId = entity.getEquipModelId();
            // 重新加载一次型号来带出明细
            entity.setEquipModelId(null);
            entity.setEquipModelId(modelId);
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
        // 仪器参数
        var equipParamView = view.findChild("SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts.MeteringEquipParam");
        // 位置列表
        var locationView = view.findChild("SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts.Tab.MeterEquipAccountLocation");
        // 电子行业
        var electricityView = view.getChildren().first(function (e) {
            return e.viewGroup === "EISBaseDataAddViewGroup";
        });
        // 缸槽管理
        var slotView = view.findChild("SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts.Tab.MeterEquipAccountSlot");
        Ext.each(tabPanelItems, function (item) {
            //if ("校验项目" == item.title) {
            //    entity.CheckCategory != null ? item.show() : item.hide();
            //}
            if (equipParamView && equipParamView.label == item.title) {
                entity.CheckCategory != null ? item.show() : item.hide();
            }
            else if (locationView && locationView.label == item.title) {
                entity.IndustryCategory == "1" ? item.show() : item.hide();
            }
            else if (electricityView && electricityView.label == item.title) {
                entity.IndustryCategory == "1" ? item.show() : item.hide();
            }
            //else if ("设备能力" == item.title) {
            //    entity.IndustryCategory == "2" ? item.show() : item.hide();
            //}
            else if (slotView && slotView.label == item.title) {
                entity.IndustryCategory == "2" ? item.show() : item.hide();
            }
            //else if ("PCB行业基础数据" == item.title) {
            //    entity.IndustryCategory == "2" ? item.show() : item.hide();
            //}

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
        view.childCheckView = view.findChild('SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts.MeteringEquipAccountCheckProject');
        //view.childCalibrationView = view.findChild('SIE.EMS.Equipments.Accounts.EquipAccountCalibrationProject');
        view.childMaintainView = view.findChild('SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts.MeteringEquipAccountMaintainProject');
        view.childLocationView = view.findChild('SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts.Tab.MeterEquipAccountLocation');
        view.childLubricationView = view.findChild('SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts.MeteringEquipAccountLubricationProject');
        view.childPCBView = view.getChildren().first(function (e) {
            return e.viewGroup === "PCBBaseDataAddViewGroup";
        });

        view.childEISView = view.getChildren().first(function (e) {
            return e.viewGroup === "EISBaseDataAddViewGroup";
        });

        //主要用于复制新增，由于主界面点检数据结构和表单的不一样，数据需要重新加载
        if (view.childCheckView && me.SourceEquipId != null) {
            SIE.invokeDataQuery({
                type: "SIE.Web.EMS.Equipments.Accounts.DataQuery.EquipAccountDataQueryer",
                method: "GetEquipCheckProjectInfos",
                params: [me.SourceEquipId],
                async: false,
                token: view.token,
                callback: function (res) {
                    var info = res.Result;

                    if (info) {
                        //设备点检页签
                        if (view.childCheckView) {
                            info.CheckPrjList.forEach(function (item) {
                                item.DepartmentId_Display = item.DepartmentNameView;
                            })

                            var controlCheckView = view.childCheckView.getControl();
                            var storeCheckView = controlCheckView.getStore();
                            storeCheckView.setData(info.CheckPrjList);
                        }
                    }
                },
            });
        }

        if (view.childLocationView)
            view.childLocationView.loadChildData(true);

        if (view.childPCBView)
            view.childPCBView.loadChildData(true);

        if (view.childEISView)
            view.childEISView.loadChildData(true);

        view.mon(entity, 'propertyChanged', me.onEntityPropertyChanged, view);
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
                    type: "SIE.Web.EMS.Equipments.Accounts.DataQuery.EquipAccountDataQueryer",
                    method: "GetEquipModelRelateInfos",
                    params: [e.entity.data],
                    async: false,
                    token: me.token,
                    callback: function (res) {
                        var info = res.Result;
                        if (info) {

                            //设备校验页签
                            if (me.childCalibrationView) {
                                var controlCalibrationView = me.childCalibrationView.getControl();
                                var storeCalibrationView = controlCalibrationView.getStore();
                                storeCalibrationView.setData(info.CalibrationPrjList);
                            }

                            //设备点检页签
                            if (me.childCheckView) {
                                info.CheckPrjList.forEach(function (item) {
                                    item.DepartmentId_Display = item.DepartmentNameView;
                                })
                                if (me.childCheckView._parent.getCurrent().CheckProjectList_2 != undefined) {
                                    me.childCheckView._parent.getCurrent().CheckProjectList_2.removeAll();
                                }
                                var controlCheckView = me.childCheckView.getControl();
                                var storeCheckView = controlCheckView.getStore();
                                storeCheckView.setData(null);
                                storeCheckView.setData(info.CheckPrjList);
                            }

                            //设备保养页签
                            if (me.childMaintainView) {
                                info.MaintainPrjList.forEach(function (item) {
                                    item.DepartmentId_Display = item.DepartmentNameView;
                                })

                                var controlMaintainView = me.childMaintainView.getControl();
                                var storeMaintainView = controlMaintainView.getStore();
                                storeMaintainView.setData(info.MaintainPrjList);
                            }
                            if (me.childUnitView) {
                                me.getCurrent()[me.childUnitView._childProperty]().setData(info.UnitList);
                                if (me.childUnitItemView)
                                    me.childUnit.unitItemInfos = info.UnitItemList;
                            }

                            //位置页签
                            if (me.childLocationView) {
                                var controlLocationView = me.childLocationView.getControl();
                                var storeLocationView = controlLocationView.getStore();
                                storeLocationView.setData(info.LocationList);
                            }

                            //PCB套件页签
                            if (me.childPCBView) {
                                var currentPCBView = me.childPCBView.getCurrent();
                                currentPCBView.setAverageBeat(info.AverageBeat);
                                currentPCBView.setStandardCapacity(info.StandardCapacity);
                                currentPCBView.setCapacityUnit(info.CapacityUnit);
                            }

                            //电子套件页签
                            if (me.childEISView) {
                                var currentEISView = me.childEISView.getCurrent();
                                currentEISView.setRailType(info.RailType);
                                currentEISView.setVirtualDevice(info.VirtualDevice);
                                currentEISView.setFeederBinding(info.FeederBinding);
                                currentEISView.setFeederLocFailSafe(info.FeederLocFailSafe);
                                currentEISView.setFeederBarcodeFailSafe(info.FeederBarcodeFailSafe);
                                currentEISView.setIsDisabled(info.IsDisabled);
                                currentEISView.setAgingType(info.AgingType);
                                currentEISView.setProductionType(info.ProductionType);
                            }

                            if (me.childLubricationView) {
                                info.LubricationProjectList.forEach(function (item) {
                                    item.DepartmentId_Display = item.DepartmentNameView;
                                })
                                if (me.childCheckView._parent.getCurrent().LubricationProjectList_4 != undefined) {
                                    me.childCheckView._parent.getCurrent().LubricationProjectList_4.removeAll();
                                }

                                var controlLubricationProjectView = me.childLubricationView.getControl();
                                var storeLubricationProject = controlLubricationProjectView.getStore();
                                storeLubricationProject.setData(info.LubricationProjectList);
                            }
                            var Current = me.getCurrent().data;
                            Current.IsCalibration = info.IsCalibration;
                            e.entity.Behavior.tabEvent(view, info);
                        }
                    },
                });
            };
        }
    },

})