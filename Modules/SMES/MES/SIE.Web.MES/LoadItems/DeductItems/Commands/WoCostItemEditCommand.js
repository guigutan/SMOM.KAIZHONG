SIE.defineCommand("SIE.Web.MES.LoadItems.DeductItems.Commands.WoCostItemEditCommand", {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    canExecute: function (listView) {
        var selectModels = listView.getSelection();
        if (selectModels.length == 0) return false;
        var res = true;
        SIE.each(selectModels, function (model) {
            if (model.data.State != 10) //新增待提交10才可修改
                res = false;
        });
        return res;
    },
    onEditting: function (entity) {
        if (entity) {
            this.mon(entity, 'propertyChanged', this.onEntityPropertyChanged, this);
        }
    },
    onEntityPropertyChanged: function (e) {
        var me = this;
        if (e.property == 'WorkOrderId' && e.value != null) {
            e.entity.setItemId(null);
            e.entity.setCostItemName(null);
            SIE.invokeDataQuery({
                method: "GetFactoryInfoByWoId",
                params: [e.value],
                async: false,
                action: 'queryer',
                type: "SIE.Web.MES.LoadItems.DeductItems.DataQuerys.WoCostItemDataQuery",
                token: this.view.token,
                success: function (res) {
                    var factoryInfo = res.Result;
                    e.entity.setFactoryId_Display(factoryInfo.FactoryId === 0 ? null : factoryInfo.FactoryName);
                    e.entity.setFactoryId(factoryInfo.FactoryId === 0 ? null : factoryInfo.FactoryId);
                    e.entity.setWipResourceId_Display(factoryInfo.FactoryId === 0 ? null : factoryInfo.WipResourceName);
                    e.entity.setWipResourceId(factoryInfo.WipResourceId === 0 ? null : factoryInfo.WipResourceId);
                }
            });
        }
        if (e.property == 'ItemExtPropName' || e.property == 'ItemExtProp') {
            e.entity.setCostItemLabelId(null);
            e.entity.setLot(null);
            e.entity.setWarehouse(null);
            e.entity.setStorage(null);
        }
    }
});
