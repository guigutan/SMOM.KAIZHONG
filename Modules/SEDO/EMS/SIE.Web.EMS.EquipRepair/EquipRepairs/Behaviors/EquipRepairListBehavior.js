Ext.define('SIE.Web.EMS.EquipRepair.EquipRepairs.Behaviors.EquipRepairListBehavior',
    {
        onViewReady: function (view) {
            //维修报告
            var repairRepoterChildView = view.getChildren().first(function (e) {
                return e.viewGroup === "RepairRepoterViewGroup";
            });
            //备件更换
            var equipRepairSparePartChgChildView = view.getChildren().first(function (e) {
                return e.viewConfig === "SIE.Web.EMS.EquipRepair.EquipRepairs.EquipRepairSparePartChgViewConfig";
            });
            //维修报告的按钮集合
            var reportBtns = repairRepoterChildView.getControl().query('button');
            //备件更换的按钮集合
            var reportSparePartChgBtns = equipRepairSparePartChgChildView.getControl().query('button');

            var selectionModel = view.getControl().getSelectionModel();

            //主表绑定选中事件
            if (selectionModel) {
                selectionModel.mon(selectionModel, "selectionchange", function (selmodel, selection) {
                    if (selection.length > 0) {
                        SIE.invokeDataQuery({
                            type: "SIE.Web.EMS.EquipRepair.EquipRepairs.DataQuerys.EquipRepairDataQuery",
                            method: "GetRepairReportPermit",
                            params: [selection[0].data.Id],
                            async: false,
                            token: view.token,
                            callback: function (res) {
                                repairRepoterChildView.setIsReadonly(!res.Result.reportPermit);

                                repairRepoterChildView.getControl().form.findField("RepairWay").setEditable(false);
                                repairRepoterChildView.getControl().form.findField("RepairWay").setReadOnly(true);

                                repairRepoterChildView.getControl().form.findField("SourceType").setEditable(false);
                                repairRepoterChildView.getControl().form.findField("SourceType").setReadOnly(true);
                                //不显示字段的话无法设置只读
                              /*  repairRepoterChildView.getControl().form.findField("SparePartsCost").setEditable(false);
                                repairRepoterChildView.getControl().form.findField("SparePartsCost").setReadOnly(true);*/

                                repairRepoterChildView.getControl().form.findField("RepairHours").setEditable(false);
                                repairRepoterChildView.getControl().form.findField("RepairHours").setReadOnly(true);

                                //维修报告页按钮控制
                                if (reportBtns[0] && reportBtns[1]) {
                                    if (!res.Result.reportPermit) {
                                        selection[0].data['reportBtnDisable'] = true;
                                        setTimeout(function () {
                                            reportBtns[0].disable();
                                            reportBtns[1].disable();
                                        }, 200);
                                    }
                                    else {
                                        selection[0].data['reportBtnDisable'] = false;
                                        setTimeout(function () {
                                            reportBtns[0].enable();
                                            reportBtns[1].enable();
                                        }, 200);
                                    }
                                }
                                //备件更换页按钮控制
                                if (reportSparePartChgBtns[0] && reportSparePartChgBtns[1]) {
                                    if (!res.Result.reportPermit) {
                                        selection[0].data['reportBtnDisable'] = true;
                                        setTimeout(function () {
                                            reportSparePartChgBtns[0].disable();
                                            reportSparePartChgBtns[1].disable();
                                        }, 100);
                                    }
                                    else {
                                        selection[0].data['reportBtnDisable'] = false;
                                        setTimeout(function () {
                                            reportSparePartChgBtns[0].enable();
                                            reportSparePartChgBtns[1].enable();
                                        }, 100);
                                    }
                                }

                                if (!res.Result.repairPermit) {
                                    selection[0].data['repairBtnDisable'] = true;
                                }
                                else {
                                    selection[0].data['repairBtnDisable'] = false;
                                }
                            }
                        });
                    }
                }, this);
            }

            var me = this;
            me.view = view;
            me.showDetailTab(view);
            view.mon(view, 'currentChanged', me.currentChanged, me);

        },
        onShow: function (view) {
            var params = CRT.Context.PageContext.getParams();
            if (params) {
                //获取查询视图
                var conditionView = view.getConditionView();
                //获取查询实体元数据
                var criteria = conditionView.getData();
                //赋值传递过来的维修单
                criteria.setRepairNo(params.RepairNo);
                //清空所有时间范围控件的开始结束时间
                var dateRangeCtls = conditionView.getControl().items.items.filter(function (e) { return e.xtype === "dateRange"; })
                if (dateRangeCtls.length > 0) {
                    dateRangeCtls.forEach(function (ctl) {
                        ctl.setDataRangValue(null, null);
                    });
                }
                //执行查询
                conditionView.tryExecuteQuery();
                params.RepairNo = "";
            }
        },

        currentChanged: function (config) {
            var me = this;
            me.showDetailTab(me.view);
        },
        showDetailTab: function (view) {
            //维修规程
            var locChildView = view.findChild('SIE.EMS.EquipRepair.EquipRepairs.EquipRepairBillProject');
            if (locChildView == null) {
                return;
            }
            //故障信息
            var lotChildView = view.findChild('SIE.EMS.EquipRepair.EquipRepairs.EquipRepairBill');
            if (lotChildView == null) {
                return;
            }
            var tabPanel = locChildView.getControl().ownerCt.ownerCt;
            var locTab = locChildView.getControl().ownerLayout.owner.tab;
            var lotTab = lotChildView.getControl().ownerLayout.owner.tab;

            var curEntity = view.getCurrent();
            if (curEntity) {
                if (curEntity.data.SourceType == 6) {
                    lotTab.hide();
                    locTab.show();
                    if (tabPanel.getActiveTab().title.indexOf('故障信息'.t()) > -1 || tabPanel.getActiveTab().title.indexOf('维修规程'.t()) > -1) {
                        tabPanel.setActiveTab(0);
                    }
                } else {
                    lotTab.show();
                    locTab.hide();
                    if (tabPanel.getActiveTab().title.indexOf('故障信息'.t()) > -1 || tabPanel.getActiveTab().title.indexOf('维修规程'.t()) > -1) {
                        tabPanel.setActiveTab(1);
                    }
                }
            }
            else {
                lotTab.show();
                locTab.hide();
            }
        }
    });
