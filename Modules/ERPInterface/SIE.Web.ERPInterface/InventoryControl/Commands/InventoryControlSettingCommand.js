SIE.defineCommand('SIE.Web.ERPInterface.InventoryControl.Commands.InventoryControlSettingCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "设置", group: "edit", iconCls: "icon-AddEntity icon-green" },
    canExecute: function (listView) {
        return true;
    },
    execute: function (view) {
        var me = this;
        SIE.invokeDataQuery({
            method: 'GetInventoryControlSetting',
            action: 'queryer',
            type: 'SIE.Web.ERPInterface.InventoryControl.DataQueryer.InventoryControlDataQueryer',
            token: view.token,
            success: function (res) {
                if (res.Result) {
                    //console.log("res",res);
                    var data = res.Result;
                    //newModel.setTurnOverRuleId(data.TurnOverRuleId);
                    //newModel.setTurnOverRuleId_Display(data.TurnOverRuleName);
                    //newModel.setAssignRuleId(data.AssignRuleId);
                    //newModel.setAssignRuleId_Display(data.AssignRuleName);
                    //newModel.setCode(data.Code);
                    me.showView(data);
                }
            },
        }); 
    },
    showView: function (settingData) {
        var me = this;
        var mainView = me.view;
        if (!this.viewMeta) {
            SIE.AutoUI.getMeta({
                async: false,
                ignoreCommands: true,
                isDetail: true,
                ignoreQuery: true,
                //viewGroup: "DetailsView",
                token: this.view.token,
                module: mainView.module,
                model: "SIE.ERPInterface.Smom.InventoryControl.InventoryControlSetting",
                callback: function (res) {
                    var mainBlock;
                    if (res.mainBlock)
                        mainBlock = res.mainBlock;
                    else
                        mainBlock = res;
                    var detailView = SIE.AutoUI.createDetailView(mainBlock);
                    var entity = new detailView._model();
                    var oldData = settingData.getData().items[0]
                    entity.setId(oldData.getId());
                    entity.setIsItem(oldData.getIsItem());
                    entity.setIsLot(oldData.getIsLot());
                    entity.setIsOkInv(oldData.getIsOkInv());
                    entity.setIsNgInv(oldData.getIsNgInv());
                    entity.setEbsToWarehouse(oldData.getEbsToWarehouse());
                    entity.setEbsToLot(oldData.getEbsToLot());
                    entity.setIsWareHouse(oldData.getIsWareHouse());
                    detailView.setData(entity);
                    mainView.detailView = detailView;
                    var ui = detailView.getControl();
                    var window = SIE.Window.show({
                        title: "设置".t(),
                        width: '700px',
                        height: '350px',
                        items: ui,
                        buttons: [
                            {
                                xtype: "button", text: "确定".t(), handler: function () {
                                    var indata = detailView.getCurrent().data;
                                    me.view.execute({
                                        data: indata,
                                        success: function (res) {
                                            if (res.Result) {
                                                SIE.Msg.showInstantMessage("保存成功".t());
                                                window.close();
                                            }
                                        }
                                    });
                                }
                            }
                        ]
                    });
                }
            });
        }
    }
});