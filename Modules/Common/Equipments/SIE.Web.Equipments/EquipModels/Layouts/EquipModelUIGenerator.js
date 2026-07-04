Ext.define('SIE.Web.Equipments.EquipModels.Layouts.EquipModelUIGenerator', {
    extend: 'SIE.autoUI.AggtUIGeneratorDefault',

    /**
     * 生成控件重写框架的方法
     * @param aggtMeta 聚合块元数据
     * @param entity 实体
     * @returns 聚合控件
     */
    generateControl: function (aggtMeta, entity) {
        var mk = aggtMeta.mainBlock;
        var mainView = mk.gridConfig ? this._vf.createListView(mk) : this._vf.createDetailView(mk, entity);
        var aggtView = this._generateAggt(aggtMeta, mainView, true);
        if (mainView.hasListeners['isready']) {
            mainView.fireEvent('isReady', true);
        }
        var me = this;
        SIE.invokeDataQuery({
            method: '',
            params: [],
            action: '',
            type: '',
            token: mainView.token,
            success: function (res) {
                mainView.mon(mainView._control, 'rowclick', me.rowclick, mainView);
            }
        });
        //页面生成后，需要加载出当前活动的子页签数据;
        mainView._resetChildrenData();
        return aggtView;
    },
    rowclick: function (g, record, element, rowIndex, e, eOpts) {
        var me = this;
        var tabs = g.up().SIEView._children;
        var tabPanel = tabs ? tabs[0].getControl().ownerCt.ownerCt : null;

        if (!tabPanel) return;
        var tabPanelItems = tabPanel.tabBar.items.items;
        var currentTab = tabPanel.getActiveTab().title;
        var flag = false;

        var hideTabCount = 0;
        var isMeasureEquipment = false;//是否计量设备
        var isSpecial = false;//设备特殊设备

        SIE.invokeDataQuery({
            type: "SIE.Web.Equipments.EquipModels.DataQuery.EquipModelDataQueryer",
            method: "GetEquipmentTypeInfo",
            params: [record[0].TypeCategory],
            async: false,
            token: this.token,
            success: function (res) {
                if (res.Result != null) {
                    isMeasureEquipment = res.Result.Item1;
                    isSpecial = res.Result.Item2;
                }
            }
        });
        var view = record.belongsView;
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
            if (calibraView && calibraView.label === item.title) {
                if (isMeasureEquipment == true) {
                    if (!tabPanel.isVisible()) {
                        tabPanel.show();
                    }
                    item.show();
                } else {
                    item.hide();
                    hideTabCount++;
                }
            }
            else if (regularView && regularView.label === item.title) {
                if (isSpecial == true) {
                    if (!tabPanel.isVisible()) {
                        tabPanel.show();
                    }
                    item.show();
                } else {
                    item.hide();
                    hideTabCount++;
                }
            }

            else if (locationView && locationView.label === item.title) {
                if (record[0].IndustryCategory === 1) {
                    if (!tabPanel.isVisible()) {
                        tabPanel.show();
                    }
                    item.show();
                } else {
                    item.hide();
                    hideTabCount++;
                }
            }
            else if (electricityView && electricityView.label === item.title) {
                if (record[0].IndustryCategory === 1) {
                    if (!tabPanel.isVisible()) {
                        tabPanel.show();
                    }
                    item.show();
                } else {
                    item.hide();
                    hideTabCount++;
                }
            }
            //else if (("设备能力" + "PCB行业基础数据").indexOf(item.title) >= 0) {
            //    if (record[0].IndustryCategory === 2) {
            //        if (!tabPanel.isVisible()) {
            //            tabPanel.show();
            //        }
            //        item.show();
            //    } else {
            //        item.hide();
            //        hideTabCount++;
            //    }
            //}

            if (item.isHidden() && currentTab == item.title) {
                flag = true;
            }
        });

        if (hideTabCount == tabPanelItems.length) {
            tabPanel.hide();
        }
        else {
            if (flag) {
                tabPanel.setActiveTab(0);
            }
            else {
                tabPanel.setActiveTab(currentTab);
            }
        }
    }
});