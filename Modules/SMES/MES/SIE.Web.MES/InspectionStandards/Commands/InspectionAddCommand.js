SIE.defineCommand('SIE.Web.MES.InspectionStandards.Commands.InspectionAddCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },
    onItemCreated: function (entity) {
        if (entity) {
            this.mon(entity, 'propertyChanged', this.onEntityPropertyChanged, this);
        }
    },
    onEntityPropertyChanged: function (e) {
        if (e.property == 'ModelId') {
            var me = this;
            var insepctionId = e.entity.data.Id;
            var modelId = 0;
            var itemId = 0;
            if (e.value) {
                modelId = e.value;
                itemId = null;
            }
            else {
                modelId = null;
                itemId = e.entity.getProductItemId();
                if (!itemId) {
                    e.entity.setOrderNum(0);
                }
            }
            SIE.invokeDataQuery({
                method: "GetMaxOrderNum",
                params: [insepctionId, modelId, itemId],
                async: false,
                action: 'queryer',
                type: "SIE.Web.MES.InspectionStandards.DataQuery.InspectionDataQuery",
                token: this.view.token,
                success: function (res) {
                    var data = res.Result;
                    var view = me.view;
                    var entity = view.getCurrent();
                    var dirtys = {};
                    if (e.value) {
                        dirtys = view.getData().data.filterBy(function (p) { return p.isDirty() && p.getModelId() == e.value && p.getId() != entity.getId(); });
                    }
                    else {
                        dirtys = view.getData().data.filterBy(function (p) { return p.isDirty() && p.getProductItemId() == e.entity.getProductItemId() && p.getId() != entity.getId(); });
                    }
                    if (dirtys.items.length != 0) {
                        var lastOrderNum = dirtys.items.orderByDescending(function (p) { return p.getOrderNum(); }).first().getOrderNum();
                        if (lastOrderNum) {
                            entity.setOrderNum(lastOrderNum + 1);
                        }
                        else {
                            entity.setOrderNum(data + 1);
                        }
                    }
                    else {
                        entity.setOrderNum(data + 1);
                    }
                }
            });
        }
        if (e.property == 'ProductItemId') {
            var me = this;
            var insepctionId = e.entity.data.Id;
            var modelId = 0;
            var itemId = 0;
            if (e.value) {
                itemId = e.value;
                modelId = null;
            }
            else {
                itemId = null;
                modelId = e.entity.getModelId();
                if (!modelId) {
                    e.entity.setOrderNum(0);
                }
            }
            SIE.invokeDataQuery({
                method: "GetMaxOrderNum",
                params: [insepctionId, modelId, itemId],
                async: false,
                action: 'queryer',
                type: "SIE.Web.MES.InspectionStandards.DataQuery.InspectionDataQuery",
                token: this.view.token,
                success: function (res) {
                    var data = res.Result;
                    var view = me.view;
                    var entity = view.getCurrent();
                    var dirtys = {};
                    if (e.value) {
                        dirtys = view.getData().data.filterBy(function (p) { return p.isDirty() && p.getProductItemId() == e.value && p.getId() != entity.getId(); });
                    }
                    else {
                        dirtys = view.getData().data.filterBy(function (p) { return p.isDirty() && p.getModelId() == e.entity.getModelId() && p.getId() != entity.getId(); });
                    }
                    if (dirtys.items.length != 0) {
                        var lastOrderNum = dirtys.items.orderByDescending(function (p) { return p.getOrderNum(); }).first().getOrderNum();
                        if (lastOrderNum) {
                            entity.setOrderNum(lastOrderNum + 1);
                        }
                        else {
                            entity.setOrderNum(data + 1);
                        }
                    }
                    else {
                        entity.setOrderNum(data + 1);
                    }
                }
            });
        }
    },
});
