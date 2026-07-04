SIE.defineCommand('SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.CancelCommand', {
    extend: 'SIE.cmd.Save',
    meta: { text: "取消", group: "edit", iconCls: "icon-Cancel icon-red" },
    selectedItems: [],
    canExecute: function (listview) {

        this.selectedItems = listview.getSelection();
        if (this.selectedItems.length === 0)
            return false;

        var curId = CRT.Context.GlobalContext.getContext('userInfo').EmployeeId.toString();
        for (i = 0; i < this.selectedItems.length; i++) {
            var item = this.selectedItems[i];
            if (item.data.RepairState == 0)
                return true;
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

        SIE.AutoUI.getMeta({
            async: false,
            ignoreCommands: false,
            isDetail: true,
            ignoreQuery: true,
            viewGroup: "DetailsView",
            token: listView.token,
            model: "SIE.EMS.EquipRepair.EquipRepairs.ViewModels.BillCancelViewModel",
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
                var model = SIE.getModel('SIE.EMS.EquipRepair.EquipRepairs.ViewModels.BillCancelViewModel');
                var entity = new model();

                entity.setId(curEntity.getId());
                detailView.setData(entity);

                var win = SIE.Window.show({
                    title: "单据取消".t(),
                    width: 480,
                    height: 280,
                    items: ui,
                    buttons: [{
                        xtype: "button", text: "确认".t(), handler: function () {

                            var thisId = ui.viewModel.data.p.data.Id;
                            var isStopMachineRepair = ui.viewModel.data.p.data.IsStopMachineRepair;
                            var CancelReason = ui.viewModel.data.p.data.CancelReason;

                            var lv = detailView.listView;
                            if (CancelReason == null || CancelReason == "") {
                                SIE.Msg.showMessage("取消原因不可为空".t());
                                return;
                            }

                            var me = this;
                            lv.execute({
                                data: {
                                    Id: thisId,
                                    CancelReason: CancelReason
                                },
                                success: function (res) {
                                    if (res.Result) {
                                        lv.reloadData();
                                        me.up('window').close();
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