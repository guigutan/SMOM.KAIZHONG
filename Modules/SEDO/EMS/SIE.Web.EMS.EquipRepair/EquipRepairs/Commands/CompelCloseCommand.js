SIE.defineCommand('SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.CompelCloseCommand', {
    extend: 'SIE.cmd.Save',
    meta: { text: "强制关单", group: "edit", iconCls: "icon-NetworkError icon-red" },
    canExecute: function (listView) {

        var current = listView.getCurrent();
        if (current === null) { return false; }
        if (current.getRepairState() == 5 || current.getRepairState() == 8) {
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
            model: "SIE.EMS.EquipRepair.EquipRepairs.ViewModels.CompelCloseViewModel",
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
                var model = SIE.getModel('SIE.EMS.EquipRepair.EquipRepairs.ViewModels.CompelCloseViewModel');
                var entity = new model();

                entity.setId(curEntity.getId());
                detailView.setData(entity);

                var win = SIE.Window.show({
                    title: "强制关单".t(),
                    width: 480,
                    height: 280,
                    items: ui,
                    buttons: [{
                        xtype: "button", text: "确认".t(), handler: function () {

                            var thisId = ui.viewModel.data.p.data.Id;
                            var isStopMachineRepair = ui.viewModel.data.p.data.IsStopMachineRepair;
                            var CloseReason = ui.viewModel.data.p.data.CloseReason;
                            var lv = detailView.listView;

                            if (CloseReason == null || CloseReason == "") {
                                SIE.Msg.showMessage("关单原因不可为空".t());
                                return;
                            }
                            var me = this;
                            lv.execute({
                                data: {
                                    Id: thisId,
                                    CloseReason: CloseReason
                                },
                                success: function (res) {
                                    if (res.Result == true) {
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