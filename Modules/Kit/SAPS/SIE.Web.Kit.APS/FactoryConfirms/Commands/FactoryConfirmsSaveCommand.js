
SIE.defineCommand('SIE.Web.Kit.APS.FactoryConfirms.Commands.FactoryConfirmsSaveCommand', {
    meta: { text: "提交", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    canExecute: function (view) {
        var flag = true;
        if (view.getSelection().length > 0 && view.getCurrent() !== null) {
            Ext.each(view.getSelection(), function (item) {
                if (item.getLineState() > 0 || item.getEnterpriseId() == null) {
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
                if (selections[i].getLineState() != 0)
                    countState++;
                if (selections[i].getEnterpriseId() == null)
                    enterprise++;
            }
            if (countState == 0 && enterprise == 0) {
                var me = this;
                SIE.Msg.askQuestion('是否确认提交?'.t(),
                    function () {
                        SIE.Msg.wait("正在提交厂别确认，请稍等...".t());
                        var indata = [];
                        var selections = view.getSelection();
                        Ext.each(selections, function (entity) { indata.push(entity.data); });
                        view.execute({
                            data: indata,
                            withIds: true,
                            selectIds: view.getSelectionIds(),
                            success: function (res) { //回调
                                if (res.Success) {
                                    SIE.Msg.showInstantMessage('提交成功'.t());
                                    view.reloadData();
                                    var year = Ext.getCmp("comboboxYear").value;
                                    me.getstoreData(year, view);
                                    //new SIE.Web.Pcb.APS.FactoryConfirms.FactoryConfirmUIGenerator().getstoreData(year);
                                }
                            }
                        });
                    });
            } else {
                if (countState != 0) {
                    SIE.Msg.showMessage("存在非新建状态销售订单!".L10N());
                }
                if (enterprise != 0) {
                    SIE.Msg.showMessage("存在未分配销售订单!".L10N());
                }
            }
        } else {
            SIE.Msg.showMessage("请选择要提交的销售订单!".L10N());
        }
    },

    /**
* 加载产能数据
*/
    getstoreData: function (data, view) {
        SIE.invokeDataQuery({
            method: 'GetYearQty',
            action: 'queryer',
            params: [data],
            type: 'SIE.Web.Kit.APS.FactoryConfirms.CapacityMapDataQueryer',
            token: view.token,
            success: function (res) {
                var day = res.Result;
                var item = Ext.getCmp("ewe");
                item.setHtml(day);
            }
        });
    }
});



