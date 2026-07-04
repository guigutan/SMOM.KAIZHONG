SIE.defineCommand('SIE.Web.Kit.APS.FactoryConfirms.Commands.GenerateEngineeringPlanCommand', {
    meta: { text: "生成工程计划", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    canExecute: function (view) {
        return true;
    },
    execute: function (view, source) {
        var countState = 0, enterprise = 0;
        var selections = view.getSelection();
        if (selections != null && selections.length > 0) {
            for (var i = 0; i < selections.length; i++) {
                if (selections[i].getLineState() != 0)
                    countState++;
                if (selections[i].getEnterpriseId() == null)
                    enterprise++;
            }
            if (countState == 0 && enterprise == 0) {
                SIE.Msg.askQuestion('是否确认生成工程计划?'.t(),
                    function () {
                        SIE.Msg.wait("正在生成工程计划，请稍等...".t());
                        var factoryConfirm = [];
                        var sales = view.getSelection();
                        Ext.each(sales, function (entity) { factoryConfirm.push(entity.data) });
                        view.execute({
                            data: factoryConfirm,
                            withIds: true,
                            selectIds: view.getSelectionIds(),
                            success: function (res) { //回调
                                if (res.Success) {
                                    view.reloadData();
                                    var tempMsg = res.Result.replace(/\n/g, '<br/>');
                                    if (tempMsg == "") {
                                        tempMsg = "生成成功!";
                                    }
                                    SIE.Msg.showMessage(tempMsg);
                                    // SIE.Msg.showInstantMessage('提交成功'.t());
                                }
                            }
                        });
                    });
            }
            else {
                if (countState != 0 && enterprise == 0) {
                    SIE.Msg.showMessage("存在确定状态销售订单!".L10N());
                }
                if (countState == 0 && enterprise != 0) {
                    SIE.Msg.showMessage("存在未分配销售订单!".L10N());
                }
                if (countState != 0 && enterprise != 0) {
                    SIE.Msg.showMessage("存在未分配和确定状态销售订单!".L10N());
                }
            }
        } else {
            SIE.Msg.showMessage("请选择要生成工程计划的销售订单!".L10N());
        }   
    }
});

   