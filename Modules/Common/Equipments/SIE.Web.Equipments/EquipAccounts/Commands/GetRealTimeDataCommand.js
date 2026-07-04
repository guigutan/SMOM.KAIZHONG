SIE.defineCommand('SIE.Web.Equipments.EquipAccounts.Commands.GetRealTimeDataCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "获取设备实时数据", group: "business", iconCls: "icon-NetworkNormal icon-green" },

    canExecute: function (view) {
        var accountParent = view._parent.getCurrent();
        if (accountParent == null) {
            return false;
        }

        return true;
    },
    execute: function (view, source) {
        var code = view._parent.getCurrent().data.Code;//设备编码
        var items = view.getControl().store.data.items;//物联参数列表
        var data = {};
        data.EquipmentCode = code;
        data.Paras = [];

        for (var i = 0; i < items.length; i++) {
            data.Paras.push(items[i].data.MDCVariableName);
        }

        SIE.Msg.askQuestion(Ext.String.format('确定获取设备[{0}]实时数据?'.t(), data.EquipmentCode), function () {
            view.execute({
                data: data,
                withIds: true,
                selectIds: view.getSelectionIds(),
                success: function (res) { //回调
                    var result = res.Result;
                    if (result.IsSuccess == true) {
                        SIE.each(result.Data, function (rtn) {
                            //item.QualityStamp

                            SIE.each(items, function (item) {
                                if (item.data.MDCVariableName == rtn.Tag) {
                                    item.setRealValue(rtn.Value);
                                    item.setAutoGetDate(new Date());
                                    item.markSaved();
                                }
                            });
                        });
                    }
                    else
                        SIE.Msg.showError(result.Error.Message);

                    //view.reloadData();
                }
            });
        });
    }
});