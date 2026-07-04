SIE.defineCommand('SIE.Web.Inventory.Strategy.Commands.TurnOverRuleDetailSortRuleViewModelEditCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    onEditting: function (entity) {
        if (entity) {
            this.mon(entity, 'propertyChanged', this.onEntityPropertyChanged, this);
        }
    },
    onEntityPropertyChanged: function (e) {
        
        var me = this;
        if (e.property.length > 0) {
            if (e.property == 'SortField') {
                var data = e.entity.data;
                var entity = e.entity;
                SIE.invokeDataQuery({
                    async: false,
                    type: "SIE.Web.Inventory.Common.DataQuery.TurnOverDataQuery",
                    method: 'GetTurnOverRuleDetailData',
                    token: me.view.token,
                    params: [data.SortField],
                    callback: function (res) {
                        
                        if (res.Success) {
                            var data = res.Result;
                            entity.setFieldType(data.FieldType);
                            entity.setSortType(data.SortType);
                            entity.setEqualValue(data.EqualValue);
                            entity.setLowerLimit(data.LowerLimit);
                            entity.setUpperLimit(data.UpperLimit);
                            entity.setLowerLimitDay(data.LowerLimitDay);
                            entity.setUpperLimitDay(data.UpperLimitDay);
                        }
                        if (!res.Success) {
                            SIE.Msg.showError(res.Message);
                        }
                    }
                });
            }
        }
    },
    canVisible: function (view, source) {
        return false;
    }
});