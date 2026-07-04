SIE.defineCommand('SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.FinishEquipRepairBillCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "维修完成", group: "edit", iconCls: "icon-Release icon-blue" },
    selectedItems: [],
    executeIntervalMode: SIE.cmd.IntervalMode.Debounce.value,//使用防抖模式   
    canExecute: function (listview) {
        this.selectedItems = listview.getSelection();
        if (this.selectedItems.length != 1)
            return false;

        var curId = CRT.Context.GlobalContext.getContext('userInfo').EmployeeId.toString();
        for (i = 0; i < this.selectedItems.length; i++) {
            var item = this.selectedItems[i];
            var employeeIdsArr = [Ext.isEmpty(item.data.RepairMasterId) ? "" : item.data.RepairMasterId.toString()];
            if (!Ext.isEmpty(item.data.RepairEmployeeIds))
                employeeIdsArr = employeeIdsArr.concat(item.data.RepairEmployeeIds.split(','));
            if (item.data.RepairState != 2 || employeeIdsArr.indexOf(curId) < 0)
                return false;
        }
        return true;
    },
    onSaving: function (view) {
        view.getCurrent().dirty = true;
        return this.callParent(arguments);
    },

    setDetailData(data, detailData) {
        data.FaultDescriptionId = detailData.FaultDescriptionId;
        data.FaultReason = detailData.FaultReason;
        data.FaultCategoryId = detailData.FaultCategoryId;
        data.FaultLevel = detailData.FaultLevel;
        data.RepairCategory = detailData.RepairCategory;
        data.RepairLevel = detailData.RepairLevel;
        data.RepairMethod = detailData.RepairMethod;
    },
    /**
    * @override execute
    * @param {object} view
    * @param {object} source
    */
    execute: function (view, source) {
        var me = this;

        var isValid = this.onSaving(view);

        if (isValid) {
            var indata = {};
            var data = view.getCurrent().data;
            var isFillinReport = false;
            var reportView = Ext.Array.findBy(view._children, function (item) { return item.config.viewGroup == "RepairRepoterViewGroup" });
            var tabPanel = reportView.getControl().ownerCt.ownerCt;
            var reportTab = tabPanel.tabBar.items.items.first(function (e) {
                return e.title == reportView.label;
            });
            var isActive = false;
            var reportCurrent = reportView.getCurrent();
            if (reportCurrent) {
                var detailData = reportCurrent.data;
                isActive = true;
                me.setDetailData(data, detailData);
            }
            SIE.invokeDataQuery({
                type: "SIE.Web.EMS.EquipRepair.EquipRepairs.DataQuerys.EquipRepairDataQuery",
                method: "FinishiRepairValidate",
                params: [data, isActive],
                async: false,
                token: view.token,
                callback: function (res) {
                    if (res.Success) {
                        var errorStr = res.Result;
                        isFillinReport = errorStr.length === 0;
                        if (!isFillinReport) {
                            SIE.Msg.askQuestion("维修报告的".t() + errorStr + "，是否确认完成单据？".t(), function () {
                                indata = {
                                    EquipRepair: data,
                                    IsFillinReport: isFillinReport,
                                }
                                view.execute({
                                    async: false,
                                    data: indata,
                                    success: function (res) {
                                        SIE.Msg.showInstantMessage("维修完成".t());
                                        view.reloadData();
                                    }
                                });

                            })
                        }
                        else {
                            indata = {
                                EquipRepair: data,
                                IsFillinReport: isFillinReport,
                            }
                            view.execute({
                                async: false,
                                data: indata,
                                success: function (res) {
                                    SIE.Msg.showInstantMessage("维修完成".t());
                                    view.reloadData();
                                }
                            });
                        }
                    } else {
                        SIE.Msg.showError("维修报告的".t() + res.Message);
                    }
                }
            });
        }
    }
});