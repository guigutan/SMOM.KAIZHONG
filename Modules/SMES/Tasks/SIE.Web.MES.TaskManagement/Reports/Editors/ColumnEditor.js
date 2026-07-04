/*
* 实现列配置了编辑器的显示
* */
Ext.define('SIE.Web.MES.TaskManagement.ReportDispatchTaskDisplay', {
    extend: 'Ext.grid.column.Column',
    alias: 'widget.ReportDispatchTaskDisplay',
    renderer: function (value, record) {
        var data = record.record.data;
        var total = data.DispatchQty;
        var firval = data.OkQty;
        var secval = data.NgQty;
        var thival = data.DispatchQty - data.ReportQty;
        if (thival % 1 != 0)
            thival = thival.toFixed(3);
        var firWidth = (firval / total) * 100 > 100 ? 100 : (firval / total) * 100;
        var secWidth = (secval / total) * 100 > 100 ? 100 : (secval / total) * 100;
        var thiWidth = (thival / total) * 100 > 100 ? 100 : (thival / total) * 100;

        var str = "<div style='width:100%; height:20px;'>"
            + (firval > 0 ? "<div style = 'width:" + firWidth + "%;float:left; background-color:#74F8A7; text-align:center;'>" + firval + "</div>" : "")
            + (secWidth > 0 ? "<div style = 'width:" + secWidth + "%;float:left; background-color:#FF0000; text-align:center;'>" + secval + "</div>" : "")
            + (thiWidth > 0 ? "<div style = 'width:" + thiWidth + "%;float:left; background-color:#F2F2F2; text-align:center;'>" + thival + "</div>" : "")
            + "</div> ";
        return str;
    }
});
/*
* 实现表格列显示图标
* */
Ext.define('SIE.Web.MES.TaskManagement.ReportDispatchPriorityDisplay', {
    extend: 'Ext.grid.column.Column',
    alias: 'widget.ReportDispatchPriorityDisplay',
    renderer: function (value, record) {
        var color = 'icon-green';
        var priority = record.record.data.Priority;
        if (priority == 1) color = 'color-warning';
        return "<div style='width:100%; text-align:center;'><div class='iconfont icon-AddEntity " + color + "'></div></div>";
    }
});
/*
* 优先级颜色
* */
Ext.define('SIE.Web.MES.TaskManagement.ReportDispatchPriorityComboBox', {
    extend: 'SIE.grid.column.ComboBox',
    alias: 'widget.ReportDispatchPriorityComboBox',
    renderer: function (value, meta) {
        if (value == 1) {
            meta.tdCls = "icon-red";
            return "紧急".t();
        }
        else if (value == 0)
            return "普通".t();
    }
});

Ext.define('SIE.Web.Barcodes.WipBatchs.Editors.ReportDefectNamesDisplay', {
    extend: 'Ext.form.FieldContainer',
    alias: 'widget.ReportDefectNamesDisplay',
    //requires: [
    //    'SIE.Web.MES.TaskManagement.Reports.DefectSelectCommand'
    //],
    layout: {
        type: 'hbox',
    },
    items: [{
        fieldLabel: '主单共模模具数'.t(),
        hideLabel: true,
        id: 'MainProportionField',
        style: 'display:none',
        xtype: 'displayfield',
        //value: '2',
        bind: '{p.DispatchTaskProportion}'
    }, {
        fieldLabel: '缺陷录入'.t(),
        labelAlign: 'right',
        //hideLabel: true,
        xtype: 'textfield',
        id: 'mainDefectValue',
        labelWidth: 125,
        style: 'width:218px;',
        readOnly: true,
        name: 'quexianValue',
        bind: '{p.DefectNames}',
        valueIds: []
    }, {
        xtype: 'button',
        style: 'width:35px;border:0;background:none;',
        iconCls: "iconfont icon-TextQuality icon-blue",
        name: 'quexianBtn',
        bind: {
            disabled: '{p.NgQty<=0}'
        },
        listeners: {
            click: {
                fn: function () {
                    var me = this;
                    var view = this.ownerCt.ownerCt.SIEView;
                    if (view._current.data.NgQty <= 0) {
                        SIE.Msg.showInstantMessage('请先录入不合格数'.t(), '提示'.t(), 2, function () {

                        });
                        return;
                    }
                    view.currentBtnId = this.id;
                    var cmd = Ext.create(
                        'SIE.Web.MES.TaskManagement.Reports.DefectSelectCommand', {});
                    cmd._setOwnerView(view);
                    cmd.command = Ext.getClassName(cmd);
                    cmd.tryExecute(cmd);
                }
            }
        }
    }]
});

/*
* 合格数量
* */
Ext.define('SIE.Web.MES.TaskManagement.ReportOkQtyNumber', {
    extend: 'Ext.form.field.Number',
    alias: 'widget.ReportOkQtyNumber',
    minValue: 1,
    allowBlank: false,
    id: 'mainOKQtyField',
    bind: '{p.OkQty}',
    enableKeyEvents: true,
    initComponent: function () {
        var me = this;
        me.xtype = 'decimalfield';
        me.decimalPrecision = 8;
        me.step = 0.00000001;
        me.callParent(arguments);
    },
    listeners: {
        "blur": function (comp, a, b) {
            var me = this;
            var newValue = comp.getValue();
            if (newValue < 0) return;
            var ngQty = Ext.getCmp('mainNgQtyField');
            var lessQty = comp.maxValue - newValue;
            if (lessQty < 0) {
                lessQty = 0;
                newValue = comp.maxValue;
            }
            ngQty.maxValue = lessQty
            if (ngQty.getValue() > lessQty) {
                ngQty.setValue(lessQty);
            }
            newValue = newValue + ngQty.getValue();
            var productId = comp.up().SIEView._parent.getControl().SIEView.getCurrent().getData().ProductId
            var token = comp.up().SIEView._parent.getControl().SIEView.token;
            var roundValue = me.fetchPrecision(productId, token, newValue);

            SIE.Web.MES.TaskManagement.Reports.ReportCommon.setCommonQty(roundValue);
        },
        specialkey: function (field, e) {
            //回车事件
            var me = this;
            var newValue = field.getValue();
            if (newValue < 0) return;
            var ngQty = Ext.getCmp('mainNgQtyField');
            var lessQty = field.maxValue - newValue;
            if (lessQty < 0) {
                lessQty = 0;
                newValue = field.maxValue;
            }
            ngQty.maxValue = lessQty
            if (ngQty.getValue() > lessQty) {
                ngQty.setValue(lessQty);
            }
            //me.callParent();
            newValue = newValue + ngQty.getValue();
            var productId = field.up().SIEView._parent.getControl().SIEView.getCurrent().getData().ProductId
            var token = field.up().SIEView._parent.getControl().SIEView.token;
            var roundValue = me.fetchPrecision(productId, token, newValue);
            SIE.Web.MES.TaskManagement.Reports.ReportCommon.setCommonQty(roundValue);
        }
    },
    fetchPrecision: function (productId, token, value) {
        var me = this;
        var lastvalue = value;
        if (productId && token) {

            SIE.invokeDataQuery({
                type: 'SIE.Web.Items.Common.DataQuery.ItemUnitDataQueryer',
                method: 'GetItemUnitPrecisions',
                params: [productId],
                token: token,
                async: false,
                success: function (jsonRes) {
                     lastvalue = me.setNewValue(jsonRes.Result, me, value);
                }
            });
        }
        return lastvalue;
    },
    setNewValue: function (res, me, value) {
        var stepMap = {
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
        }

        var lastValue = me.getFormatValue(me.rawValue, precision, carry);
        me.setRawValue(lastValue);
        if (minValue)
            me.setMinValue(minValue);
        if (maxValue)
            me.setMaxValue(maxValue);
        me.updateLayout(); // 重新渲染数字输入框
        return lastValue;
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
});

/*
* 不合格数量
* */
Ext.define('SIE.Web.MES.TaskManagement.ReportNgQtyNumber', {
    extend: 'Ext.form.field.Number',
    alias: 'widget.ReportNgQtyNumber',
    minValue: 1,
    allowBlank: false,
    id: 'mainNgQtyField',
    bind: '{p.NgQty}',
    enableKeyEvents: true,
    initComponent: function () {
        var me = this;
        me.xtype = 'decimalfield';
        me.decimalPrecision = 8;
        me.step = 0.00000001;
        me.callParent(arguments);
    },
    fetchPrecision: function (productId, token, value) {
        var me = this;
        var lastvalue = value;
        if (productId && token) {

            SIE.invokeDataQuery({
                type: 'SIE.Web.Items.Common.DataQuery.ItemUnitDataQueryer',
                method: 'GetItemUnitPrecisions',
                params: [productId],
                token: token,
                async: false,
                success: function (jsonRes) {
                    lastvalue = me.setNewValue(jsonRes.Result, me, value);
                }
            });
        }
        return lastvalue;
    },
    setNewValue: function (res, me, value) {
        var stepMap = {
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
        }

        var lastValue = me.getFormatValue(me.rawValue, precision, carry);
        me.setRawValue(lastValue);
        if (minValue)
            me.setMinValue(minValue);
        if (maxValue)
            me.setMaxValue(maxValue);
        me.updateLayout(); // 重新渲染数字输入框
        return lastValue;
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
    listeners: {
        "blur": function (comp, a, b) {
            var me = this;
            var newValue = comp.getValue();
            if (newValue < 0) return;
            var oKQty = Ext.getCmp('mainOKQtyField');
            var lessQty = comp.maxValue - newValue;
            if (lessQty < 0) {
                newValue = comp.maxValue;
            }
            newValue = newValue + oKQty.getValue();
            if (this.value <= 0) {
                var defectControl = Ext.getCmp('mainDefectValue');
                defectControl.setValue('');
                defectControl.valueIds.removeAll();
            }
            var productId = comp.up().SIEView._parent.getControl().SIEView.getCurrent().getData().ProductId
            var token = comp.up().SIEView._parent.getControl().SIEView.token;
            var roundValue = me.fetchPrecision(productId, token, newValue);
            SIE.Web.MES.TaskManagement.Reports.ReportCommon.setCommonQty(roundValue);
        },
        specialkey: function (field, e) {
            //回车事件
            var me = this;
            var newValue = field.getValue();
            if (newValue < 0) return;
            var oKQty = Ext.getCmp('mainOKQtyField');
            var lessQty = field.maxValue - newValue;
            if (lessQty < 0) {
                newValue = field.maxValue;
            }
            newValue = newValue + oKQty.getValue();
            if (this.value <= 0) {
                var defectControl = Ext.getCmp('mainDefectValue');
                defectControl.setValue('');
                defectControl.valueIds.removeAll();
            }
            var productId = field.up().SIEView._parent.getControl().SIEView.getCurrent().getData().ProductId
            var token = field.up().SIEView._parent.getControl().SIEView.token;
            var roundValue = me.fetchPrecision(productId, token, newValue);
            SIE.Web.MES.TaskManagement.Reports.ReportCommon.setCommonQty(roundValue);
        }
    }
});

Ext.define('SIE.Web.MES.TaskManagement.ReportModeColumn', {
    extend: 'Ext.grid.column.Column',
    alias: 'widget.ReportModeColumn',
    renderer: function (value, record) {
        var data = record.record.data;
        var reportMode = data.ReportMode;
        var isSyntypeReport = data.IsSyntypeReport;
        if (reportMode === 0)
            return "自动".t();
        else
            return isSyntypeReport === true ? "手动；按共模比".t() : "手动".t();
    }
});