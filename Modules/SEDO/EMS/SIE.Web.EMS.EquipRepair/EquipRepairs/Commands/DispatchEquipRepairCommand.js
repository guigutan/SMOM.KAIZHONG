SIE.defineCommand('SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.DispatchEquipRepairCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "派工", group: "edit", iconCls: "icon-People icon-blue" },
    selectedItems: [],
    canExecute: function (listview) {
        this.selectedItems = listview.getSelection();
        if (this.selectedItems.length === 0)
            return false;

        for (i = 0; i < this.selectedItems.length; i++) {
            var item = this.selectedItems[i];
            if (item.data.RepairState != 0)
                return false;
        }
        return true;
    },
    showWindow: function (meta, editEntity, view) {
        var detailView = SIE.AutoUI.generateAggtControl(meta);
        detailView._view.setData(editEntity);

        var win = SIE.Window.show({
            title: "派工".t(),
            width: '60%',
            height: '80%',
            items: detailView.getControl(),
            callback: function (btn) {
                if (btn === "确定".t()) {

                    var retFlag = false;
                    var indata = {};
                    var data = editEntity.data;
                    indata.Data = Ext.encode(data);

                    if (data.RepairWay == null) {
                        SIE.Msg.showError('派工类型不能为空！'.t());
                        return false;
                    }

                    view.execute({
                        data: indata,
                        async: false,//设置成同步，即可消除弹窗无法关闭或者马上关闭的问题
                        success: function (res) { //回调
                            retFlag = true;
                            view.reloadData();
                            Ext.MessageBox.alert("提示".t(), "派工成功".t());
                        },
                        error: function (res) {
                            SIE.Msg.showError(res.Message);
                        }
                    });

                    return retFlag;
                }
            }
        });
    },
    execute: function (view, source) {
        var me = this;
        var editEntity = me.view.getCurrent();

        SIE.AutoUI.getMeta({
            async: false,
            ignoreCommands: true,
            isDetail: true,
            ignoreQuery: true,
            model: me.view.model,
            viewGroup: "DispatchRepairViewGroup",
            callback: function (meta) {

                //派工内型（内修）
                editEntity.data.RepairMasterId = null;
                editEntity.data.RepairMasterId_Display = "";
                editEntity.data.RepairWay = 0;

                if (!Ext.isEmpty(editEntity.data.EquipAccountId)) {
                    SIE.invokeDataQuery({
                        type: 'SIE.Web.EMS.EquipRepair.EquipRepairs.DataQuerys.EquipRepairDataQuery',
                        method: 'GetEquipWarrantyState',
                        params: [editEntity.data.EquipAccountId],
                        async: false,
                        action: 'queryer',
                        token: view.token,
                        success: function (res) {
                            if (res.Success) {

                                //若是设备维修，查询出保修期限和保修状态
                                editEntity.data.EquipWarrantyState = res.Result;
                                //子视图弹框显示
                                me.showWindow(meta, editEntity, view);
                            }
                        }
                    });
                }
                else {
                    //子视图弹框显示
                    me.showWindow(meta, editEntity, view);
                }
            }
        });
    }
});