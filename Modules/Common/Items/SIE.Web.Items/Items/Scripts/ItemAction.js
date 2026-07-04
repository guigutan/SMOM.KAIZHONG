Ext.define('SIE.Web.Items.Scripts.ItemAction', {
    statics: {
        getFormatValue: function (v, precision, carry) {
            var me = this;
            if (typeof v === 'number')
                return v;

            var value = v;
            switch (carry) {
                case 0:
                    value = roundNumber(v, precision);//四舍五入
                    break;
                case 1:
                    value = floorNumber(v, precision);//舍位
                    break;
                case 2:
                    value = ceilNumber(v, precision);//进位
                    break;
            }
            function roundNumber(number, decimalPlaces) {
                var multiplier = Math.pow(10, decimalPlaces);
                return Math.round(number * multiplier) / multiplier;
            }

            function floorNumber(number, decimalPlaces) {
                var multiplier = Math.pow(10, decimalPlaces);
                return Math.floor(number * multiplier) / multiplier;
            }

            function ceilNumber(number, decimalPlaces) {
                var multiplier = Math.pow(10, decimalPlaces);
                return Math.ceil(number * multiplier) / multiplier;
            }
            return value;
        },
        /*设置辅助数量*/
        setSecondQty: function (v, entity) {
            if ((entity.getNumerator() / entity.getDenominator()) == 1)
                return v;
            var per = entity.getSecondPrecision();
            if (per != 0) {
                per = entity.getSecondPrecision() || 3;
            }
            var peQty = (v * (entity.getNumerator() / entity.getDenominator()));//.toFixed(per);
            var tyQty = SIE.Web.Items.Scripts.ItemAction.getFormatValue(String(peQty), entity.getSecondPrecision(), entity.getSecondTrade());
            return tyQty;
        },
        /*设置原数量*/
        setSourceQty: function (v, entity) {
            if ((entity.getNumerator() / entity.getDenominator()) == 1)
                return v;
            var per = entity.getMainPrecision();
            if (per != 0) {
                per = entity.getMainPrecision() || 3;
            }
            var peQty = (v / (entity.getNumerator() / entity.getDenominator()));//.toFixed(per);
            var tyQty = SIE.Web.Items.Scripts.ItemAction.getFormatValue(String(peQty), entity.getMainPrecision(), entity.getMainTrade());
            return tyQty;
        },
        getDefaultItemUnit: function (token, entity, func) {
            SIE.invokeDataQuery({
                type: "SIE.Web.Items.Items.DataQuery.ItemDataQuery",
                method: "GetDefaultItemUnit",
                params: [entity.getItemId()],
                async: false,
                token: token,
                callback: function (res) {
                    if (res.Success && res.Result) {
                        var data = res.Result;
                        entity.setSecondUnitId_Display(data.UnitName);
                        entity.setSecondUnitId(data.UnitId);
                        entity.setSecondPrecision(data.Precision);
                        entity.setSecondTrade(data.Trade);
                    }
                }
            });
        },
        getSecondUnit: function (token, entity, func) {
            SIE.invokeDataQuery({
                type: "SIE.Web.Items.Items.DataQuery.ItemDataQuery",
                method: "GetSecondUnit",
                params: [entity.getItemId(), entity.getItemUnitId(), entity.getSecondUnitId()],
                async: false,
                token: token,
                callback: function (res) {
                    if (res.Success && res.Result) {
                        var data = res.Result.data.items[0];
                        entity.setDenominator(data.getDenominator());
                        entity.setNumerator(data.getNumerator());
                        var indata = data.data;
                        if (indata.Numerator > 0 && indata.Denominator > 0) {
                            var strConvertFigre = indata.Numerator / indata.Denominator;
                            if (indata.MainUnitPrecision >= 0) {
                                var pe = indata.MainUnitPrecision;
                                if (indata.MainUnitPrecision < indata.SecondUnitPrecision)
                                    pe = indata.SecondUnitPrecision;
                                if (pe == 0)
                                    entity.setConvertFigre(strConvertFigre);
                                else
                                    entity.setConvertFigre(strConvertFigre.toFixed(pe));
                                var perS = indata.SecondUnitPrecision;
                                if (perS != 0) {
                                    perS = indata.SecondUnitPrecision || 3;
                                }
                                var perM = indata.MainUnitPrecision;
                                if (perM != 0) {
                                    perM = indata.MainUnitPrecision || 3;
                                }
                                entity.setSecondPrecision(perS);
                                entity.setMainPrecision(perM);
                                entity.setMainTrade(indata.MainTrade);
                                entity.setSecondTrade(indata.SecondTrade);
                            }
                            else
                                entity.setConvertFigre(strConvertFigre.toFixed(3));
                            if (func) {
                                func(entity);
                            }
                        }
                    }
                    else {
                        entity.setSecondPrecision(entity.data.MainPrecision||3);                                               
                        entity.setSecondTrade(entity.data.MainTrade || 0);
                        entity.setConvertFigre(1);
                        if (func)
                            func(entity);
                    }
                }
            });
        },
        onEntityPropertyChanged: function (e) {
            var me = this;
            var entity = e.entity;
            var data = e.entity.data;
            var value = e.value;
            if (e.property.length > 0 && e.value != e.oldvalue) {
                if (e.property == 'ItemId' || e.property == 'UnitId') {
                    if (data.ItemId != null && data.UnitId != null) {
                        SIE.invokeDataQuery({
                            async: false,
                            type: "SIE.Web.Items.Items.DataQuery.ItemDataQuery",
                            method: 'CheckIsDefaultUnit',
                            token: me.token,
                            params: [data.ItemId, data.UnitId],
                            callback: function (res) {
                                if (res.Success) {
                                    var data = res.Result;
                                    if (data) {
                                        entity.setIsDefault(true);
                                    } else {
                                        entity.setIsDefault(false);
                                    }
                                }
                                if (!res.Success) {
                                    SIE.Msg.showError(res.Message);
                                }
                            }
                        });
                    }
                }
            }
        },

    }
});