SIE.defineCommand('SIE.Web.Kit.APS.FactoryConfirms.Commands.GenerateOrderReviewCommand', {
    meta: { text: "上传订单评审", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    canExecute: function (view) {
        var flag = true;
        if (view.getSelection().length > 0 && view.getCurrent() !== null) {
            Ext.each(view.getSelection(), function (item) {
                if (item.getLineState() != 10 || item.getEnterpriseId() == null) {
                    flag = false;
                }
            });
        }
        return flag;
    },
    execute: function (view, source) {
        var countState = 0, enterprise = 0;
        var selections = view.getSelection();
        if (selections != null && selections.length > 0) {
            for (var i = 0; i < selections.length; i++) {
                if (selections[i].getLineState() != 10)
                    countState++;
                if (selections[i].getEnterpriseId() == null)
                    enterprise++;
            }
            if (countState == 0 && enterprise == 0) {
                SIE.Msg.askQuestion('是否确认上传订单评审?'.t(),
                    function () {
                        SIE.Msg.wait("正在上传订单评审，请稍等...".t());
                        var ids = view.getSelectionIds();
                        view.execute({
                            data: ids,
                            withIds: true,
                            success: function (res) { //回调
                                if (res.Success) {
                                    view.reloadData();
                                    var tempMsg = res.Result.replace(/\n/g, '<br/>');
                                    if (tempMsg == "") {
                                        tempMsg = "上传成功!";
                                    }
                                    SIE.Msg.showMessage(tempMsg);
                                }
                            }
                        });
                    });
            }
            else {
                if (countState != 0 && enterprise == 0) {
                    SIE.Msg.showMessage("存在非确定状态销售订单!".L10N());
                }
                if (countState == 0 && enterprise != 0) {
                    SIE.Msg.showMessage("存在未分配销售订单!".L10N());
                }
            }
        } else {
            SIE.Msg.showMessage("请选择要生成订单评审!".L10N());
        }
    }
});

