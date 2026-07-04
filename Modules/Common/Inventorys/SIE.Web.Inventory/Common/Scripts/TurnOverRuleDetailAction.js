Ext.define('SIE.Web.Inventory.TurnOverRuleDetailAction', {
    statics: {
        onEntityPropertyChanged: function (e) {
            var me = this;
            if (e.property.length > 0) {
                if (e.property == 'SortField1') {
                    var data = e.entity.data;
                    var entity = e.entity;
                    SIE.invokeDataQuery({
                        async: false,
                        type: "SIE.Web.Inventory.Common.DataQuery.TurnOverDataQuery",
                        method: 'GetTurnOverRuleDetailData',
                        token: me.view.token,
                        params: [data.SortField1],
                        callback: function (res) {
                            if (res.Success) {
                                var data = res.Result;
                                entity.setFieldType1(data.FieldType);
                                entity.setSortType1(data.SortType);
                                entity.setEqualValue1(data.EqualValue);
                                entity.setLowerLimit1(data.LowerLimit);
                                entity.setUpperLimit1(data.UpperLimit);
                                entity.setLowerLimitDay1(data.LowerLimitDay);
                                entity.setUpperLimitDay1(data.UpperLimitDay);
                            }
                            if (!res.Success) {
                                SIE.Msg.showError(res.Message);
                            }
                        }
                    });
                }
                if (e.property == 'SortField2') {
                    var data = e.entity.data;
                    var entity = e.entity;
                    SIE.invokeDataQuery({
                        async: false,
                        type: "SIE.Web.Inventory.Common.DataQuery.TurnOverDataQuery",
                        method: 'GetTurnOverRuleDetailData',
                        token: me.view.token,
                        params: [data.SortField2],
                        callback: function (res) {
                            if (res.Success) {
                                var data = res.Result;
                                entity.setFieldType2(data.FieldType);
                                entity.setSortType2(data.SortType);
                                entity.setEqualValue2(data.EqualValue);
                                entity.setLowerLimit2(data.LowerLimit);
                                entity.setUpperLimit2(data.UpperLimit);
                                entity.setLowerLimitDay2(data.LowerLimitDay);
                                entity.setUpperLimitDay2(data.UpperLimitDay);
                            }
                            if (!res.Success) {
                                SIE.Msg.showError(res.Message);
                            }
                        }
                    });
                }
                if (e.property == 'SortField3') {
                    var data = e.entity.data;
                    var entity = e.entity;
                    SIE.invokeDataQuery({
                        async: false,
                        type: "SIE.Web.Inventory.Common.DataQuery.TurnOverDataQuery",
                        method: 'GetTurnOverRuleDetailData',
                        token: me.view.token,
                        params: [data.SortField3],
                        callback: function (res) {
                            if (res.Success) {
                                var data = res.Result;
                                entity.setFieldType3(data.FieldType);
                                entity.setSortType3(data.SortType);
                                entity.setEqualValue3(data.EqualValue);
                                entity.setLowerLimit3(data.LowerLimit);
                                entity.setUpperLimit3(data.UpperLimit);
                                entity.setLowerLimitDay3(data.LowerLimitDay);
                                entity.setUpperLimitDay3(data.UpperLimitDay);
                            }
                            if (!res.Success) {
                                SIE.Msg.showError(res.Message);
                            }
                        }
                    });
                }
                if (e.property == 'SortField4') {
                    var data = e.entity.data;
                    var entity = e.entity;
                    SIE.invokeDataQuery({
                        async: false,
                        type: "SIE.Web.Inventory.Common.DataQuery.TurnOverDataQuery",
                        method: 'GetTurnOverRuleDetailData',
                        token: me.view.token,
                        params: [data.SortField4],
                        callback: function (res) {
                            if (res.Success) {
                                var data = res.Result;
                                entity.setFieldType4(data.FieldType);
                                entity.setSortType4(data.SortType);
                                entity.setEqualValue4(data.EqualValue);
                                entity.setLowerLimit4(data.LowerLimit);
                                entity.setUpperLimit4(data.UpperLimit);
                                entity.setLowerLimitDay4(data.LowerLimitDay);
                                entity.setUpperLimitDay4(data.UpperLimitDay);
                            }
                            if (!res.Success) {
                                SIE.Msg.showError(res.Message);
                            }
                        }
                    });
                }
                if (e.property == 'SortField5') {
                    var data = e.entity.data;
                    var entity = e.entity;
                    SIE.invokeDataQuery({
                        async: false,
                        type: "SIE.Web.Inventory.Common.DataQuery.TurnOverDataQuery",
                        method: 'GetTurnOverRuleDetailData',
                        token: me.view.token,
                        params: [data.SortField5],
                        callback: function (res) {
                            if (res.Success) {
                                var data = res.Result;
                                entity.setFieldType5(data.FieldType);
                                entity.setSortType5(data.SortType);
                                entity.setEqualValue5(data.EqualValue);
                                entity.setLowerLimit5(data.LowerLimit);
                                entity.setUpperLimit5(data.UpperLimit);
                                entity.setLowerLimitDay5(data.LowerLimitDay);
                                entity.setUpperLimitDay5(data.UpperLimitDay);
                            }
                            if (!res.Success) {
                                SIE.Msg.showError(res.Message);
                            }
                        }
                    });
                }
                if (e.property == 'OrderType') {
                    e.entity.setTransactionId(null);
                }
            }
        },
    }
});