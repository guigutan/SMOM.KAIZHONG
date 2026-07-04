/*物料单位编辑器*/
Ext.define('SIE.Web.Items.Common.Editors.ItemUnitEditor', {
    extend: 'Ext.form.field.Number',
    alias: 'widget.ItemUnitEditor',
    precisionValue: 6,
    precisionSpin: 2,

    initComponent: function () {
        var me = this;
        me.xtype = 'decimalfield';
        me.decimalPrecision = 6;
        me.step = 0.000001;
        me.callParent(arguments);
    },

    fetchPrecision: function (typeName) {
        var me = this;
        var cur;
        var token;
        var view;
        if (me.readOnly) {
            return;
        }
        if (me.up('form')) {
            if (me.up().ownerCt.SIEView) {
                cur = me.up().ownerCt.SIEView.getCurrent();
                token = me.up().ownerCt.SIEView.token;
                view = me.up().ownerCt.SIEView;
            }
            else {//QueryView
                cur = me.ownerLayout.owner.SIEView._current;
                token = me.ownerLayout.owner.SIEView.token;
            }
        }
        else {
            cur = me.up().context.record;
            if (me.up().context.view.ownerCt.SIEView) {
                token = me.up().context.view.ownerCt.SIEView.token;
                view = me.up().context.view.ownerCt.SIEView;
            }
            else {
                token = me.up('gridpanel').ownerCt.SIEView.token;
                view = me.up('gridpanel').ownerCt.SIEView;
            }
        }

        var itemUnitId = cur.data[me.config.ItemUnitFileld];
        var itemId = cur.data[me.config.ItemIdField];

        if ((itemUnitId || itemId) && token) {

            if (itemUnitId) {
                SIE.invokeDataQuery({
                    type: 'SIE.Web.Items.Common.DataQuery.ItemUnitDataQueryer',
                    method: 'GetUnitPrecisions',
                    params: [itemUnitId],
                    token: token,
                    async: false,
                    success: function (jsonRes) {
                        if (typeName == "spinDown") {
                            me.setSpinValue(me, jsonRes.Result.unitPrecsion, "cut");
                        } else if (typeName == "spinUp") {           
                            me.setSpinValue(me, jsonRes.Result.unitPrecsion, "add");
                        }
                        else {
                            me.setNewValue(jsonRes.Result, me);
                        }
                    }
                });

            } else {
                SIE.invokeDataQuery({
                    type: 'SIE.Web.Items.Common.DataQuery.ItemUnitDataQueryer',
                    method: 'GetItemUnitPrecisions',
                    params: [itemId],
                    token: token,
                    async: false,
                    success: function (jsonRes) {
                        if (typeName == "spinDown") {
                            me.setSpinValue(me, jsonRes.Result.unitPrecsion, "cut");
                        } else if (typeName == "spinUp") {
                            me.setSpinValue(me, jsonRes.Result.unitPrecsion, "add");
                        } else {
                            me.setNewValue(jsonRes.Result, me);
                        }
                    }
                });
            }
        }
    },


    setNewValue: function (res, me) {
        var stepMap = {
            0: 1,
            1: 0.1,
            2: 0.01,
            3: 0.001,
            4: 0.0001,
            5: 0.00001,
            6: 0.000001,
            7: 0.0000001,
            8: 0.00000001,
            9: 0.000000001,
        };
        var maxValue = me.config.MaxValue;
        var minValue = me.config.MinValue;
        var precision = res.unitPrecsion;
        var carry = res.carry;
        me.decimalPrecision = precision;
        me.step = stepMap[precision];
        me.config.decimalPrecision = precision;
        me.config.step = stepMap[precision];
        me.initialConfig.decimalPrecision = precision;
        me.initialConfig.step = stepMap[precision];
        if (me.config.column != null) {
            me.config.column.config.editor.decimalPrecision = precision;
            me.config.column.config.editor.step = stepMap[precision];
            // 使用变量来动态设置格式
            me.config.column.format = me.dynamicFormatNumber(me.rawValue, precision);
            this.precisionValue = precision;
        }
        me.setRawValue(me.getFormatValue(me.rawValue, precision, carry));
        if (minValue != null && me.rawValue < minValue)
            me.setMinValue(minValue);
        if (maxValue != null && me.rawValue > maxValue)
            me.setMaxValue(maxValue);
        me.updateLayout(); // 重新渲染数字输入框
    },

    dynamicFormatNumber: function (number, precision) {
        if (number === Math.floor(number)) {
            // 如果输入是整数，则将格式设置为不带小数位的格式
            return '0';
        } else {
            return '0.' + '0'.repeat(precision);
        }
    },

    getFormatValue: function (v, precision, carry) {
        var me = this;
        if (typeof v === 'number')
            return v;

        var value = v;
        switch (carry) {
            case 0:
                value = me.roundNumber(v, precision);//四舍五入
                break;
            case 1:
                value = me.floorNumber(v, precision);//舍位
                break;
            case 2:
                value = me.ceilNumber(v, precision);//进位
                break;
        }
        return value;
    },

    roundNumber: function (number, decimalPlaces) {
        var multiplier = Math.pow(10, decimalPlaces);
        return Math.round(number * multiplier) / multiplier;
    },

    floorNumber: function (number, decimalPlaces) {
        var multiplier = Math.pow(10, decimalPlaces);
        return Math.floor(number * multiplier) / multiplier;
    },

    ceilNumber: function (number, decimalPlaces) {
        var multiplier = Math.pow(10, decimalPlaces);
        return Math.ceil(number * multiplier) / multiplier;
    },

    onBlur: function () {
        var me = this;
        me.fetchPrecision("main");
    },
    specialkey: function (field, e) {
        //回车事件
        var me = this;
        me.callParent();
        me.fetchPrecision("main");
    },
    onSpinDown() {
        //减
        var me = this;
       // if (me.value == null || me.value == 0 ) {
            me.fetchPrecision("spinDown");
      //  } 
        //    me.setSpinValue(me, me.decimalPrecision, "cut");
    },
    onSpinUp() {
        //加
        var me = this;
      //  if (me.value == null || me.value == 0) {
            me.fetchPrecision("spinUp"); 
      //  } 
       //     me.setSpinValue(me, me.decimalPrecision, "add");
    },

    setSpinValue: function (file,precision,type) {
        var stepMap = {
            0: 1,
            1: 0.1,
            2: 0.01,
            3: 0.001,
            4: 0.0001,
            5: 0.00001,
            6: 0.000001,
            7: 0.0000001,
            8: 0.00000001,
            9: 0.000000001,
        };
        if (type == "add") {
            file.setValue(Number(file.getValue()) + stepMap[precision]);
            file.step = stepMap[precision];
            file.decimalPrecision = precision;
        } else {
            file.setValue(Number(file.getValue()) - stepMap[precision]);
            file.step = stepMap[precision];
            file.decimalPrecision = precision;
        }
    },
});
