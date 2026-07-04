SIE.defineCommand('SIE.Web.EMS.Checks.Plans.Commands.GetProjectRealTimeDataCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "获取设备实时数据", group: "business" },

    canExecute: function (view) {
        var accountParent = view._parent.getCurrent();
        if (accountParent == null) {
            return false;
        }

        return accountParent.getExeState() == 0 || accountParent.getExeState() == 4;
    },
    execute: function (view, source) {
        var code = view._parent.getCurrent().data.EquipAccountCode;//设备编码
        var items = view.getControl().store.data.items;//物联参数列表
        var data = {};
        data.EquipmentCode = code;
        data.ProjectDetailIds = [];

        for (var i = 0; i < items.length; i++) {
            data.ProjectDetailIds.push(items[i].data.Id);
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
                                if (item.data.Id == rtn.ProjectDetailId) {
                                    item.setActualValue(rtn.Value);
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