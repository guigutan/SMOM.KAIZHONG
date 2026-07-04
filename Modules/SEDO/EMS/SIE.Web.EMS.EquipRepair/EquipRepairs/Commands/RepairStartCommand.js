SIE.defineCommand('SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.RepairStartCommand', {
    extend: 'SIE.cmd.Save',
    meta: { text: "维修开始", group: "edit", iconCls: "icon-Play icon-blue" },
    selectedItems: [],
    canExecute: function (listview) {
        this.selectedItems = listview.getSelection();
        if (this.selectedItems.length === 0)
            return false;

        var curId = CRT.Context.GlobalContext.getContext('userInfo').EmployeeId.toString();
        for (i = 0; i < this.selectedItems.length; i++) {
            var item = this.selectedItems[i];
            var employeeIdsArr = [Ext.isEmpty(item.data.RepairMasterId) ? "" : item.data.RepairMasterId.toString()];
            if (!Ext.isEmpty(item.data.RepairEmployeeIds))
                employeeIdsArr = employeeIdsArr.concat(item.data.RepairEmployeeIds.split(','));
            if (item.data.RepairState != 1 || employeeIdsArr.indexOf(curId) < 0)
                return false;
        }
        return true;
    },
    execute: function (listView, source) {
        var me = this;
        var editEntity = this.view.getCurrent();

        SIE.AutoUI.getMeta({
            async: false,
            ignoreCommands: false,
            isDetail: true,
            ignoreQuery: true,
            viewGroup: "DetailsView",
            token: listView.token,
            model: "SIE.EMS.EquipRepair.EquipRepairs.ViewModels.RepairStartViewModel",
            callback: function (res) {
                var mainBlock;
                if (res.mainBlock)
                    mainBlock = res.mainBlock;
                else
                    mainBlock = res;
                var detailView = SIE.AutoUI.createDetailView(mainBlock);
                detailView.listView = listView;
                var ui = detailView.getControl();
                var curEntity = listView.getCurrent();
                var model = SIE.getModel('SIE.EMS.EquipRepair.EquipRepairs.ViewModels.RepairStartViewModel');
                var entity = new model();

                entity.setId(curEntity.getId());
                detailView.setData(entity);

                setTimeout(function () {
                    //备件维修时，不能选择停机
                    if (editEntity.data.RepairType == 1) {
                        console.log(detailView.getControl().form.monitor.items.items[0]);
                        detailView.getControl().form.monitor.items.items[0].setReadOnly(true);
                    }
                    else {
                        detailView.getControl().form.monitor.items.items[0].setReadOnly(false);
                    }
                }, 200);


                var win = SIE.Window.show({
                    title: "维修开始".t(),
                    width: 480,
                    height: 280,
                    items: ui,
                    buttons: [{
                        xtype: "button", text: "确认".t(), handler: function () {
                            //if (!detailView.validateData())
                            //    return;

                            var thisId = ui.viewModel.data.p.data.Id;
                            var isStopMachineRepair = ui.viewModel.data.p.data.IsStopMachineRepair;

                            var lv = detailView.listView;

                            var me = this;
                            lv.execute({
                                data: {
                                    Id: thisId,
                                    IsStopMachineRepair: isStopMachineRepair
                                },
                                success: function (res) {
                                    if (res.Result) {
                                        lv.reloadData();
                                        win.close();
                                    }
                                }
                            });
                        }
                    },
                    {
                        xtype: "button", text: "取消".t(), handler: function () {
                            this.up('window').close();
                        }
                    }]
                });
            }
        });
    },
});