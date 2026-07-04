SIE.defineCommand('SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.SuspendCommand', {
    extend: 'SIE.cmd.Save',
    meta: { text: "维修暂停", group: "edit", iconCls: "icon-Pause icon-blue" },
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
            if (item.data.RepairState != 2 || employeeIdsArr.indexOf(curId) < 0)
                return false;
        }
        return true;
    },
    execute: function (listView, source) {
        var me = this;

        SIE.AutoUI.getMeta({
            async: false,
            ignoreCommands: false,
            isDetail: true,
            ignoreQuery: true,
            viewGroup: "DetailsView",
            token: listView.token,
            model: "SIE.EMS.EquipRepair.EquipRepairs.ViewModels.BillSuspendViewModel",
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
                var model = SIE.getModel('SIE.EMS.EquipRepair.EquipRepairs.ViewModels.BillSuspendViewModel');
                var entity = new model();

                entity.setId(curEntity.getId());
                detailView.setData(entity);

                var win = SIE.Window.show({
                    title: "维修暂停".t(),
                    width: 480,
                    height: 280,
                    items: ui,
                    buttons: [{
                        xtype: "button", text: "确认".t(), handler: function () {

                            var thisId = ui.viewModel.data.p.data.Id;
                            var isStopMachineRepair = ui.viewModel.data.p.data.IsStopMachineRepair;
                            var SuspendReason = ui.viewModel.data.p.data.SuspendReason;
                            var lv = detailView.listView;
                            console.log(SuspendReason);
                            if (SuspendReason == null || SuspendReason == "") {
                                SIE.Msg.showError("暂停原因不可为空".t());
                                return;
                            }
                            var me = this;
                            lv.execute({
                                data: {
                                    Id: thisId,
                                    SuspendReason: SuspendReason
                                },
                                success: function (res) {
                                    if (res.Result == true) {
                                        me.up('window').close();
                                        lv.reloadData();
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