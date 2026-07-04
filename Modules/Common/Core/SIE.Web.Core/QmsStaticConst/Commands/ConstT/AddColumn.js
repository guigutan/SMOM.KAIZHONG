SIE.defineCommand('SIE.Web.Core.QmsStaticConst.Commands.ConstT.AddColumn', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加列", group: "edit", iconCls: "iconfont icon-AddEntity icon-blue" },

    /**
     * @override 执行
     * @param {} view 视图
     * @param {} source 
     * @returns {} 
     */
    execute: function (view, source) {
        var me = this;
        SIE.AutoUI.getMeta({
            model: "SIE.Web.Core.QmsStaticConst.ViewModels.TAddColumnViewModel",
            ingoreCommands: true,
            isDetail: true,
            ignoreQuery: true,
            callback: function (res) {
                var mainBlock;
                if (res.mainBlock)
                    mainBlock = res.mainBlock;
                else
                    mainBlock = res;
                var detailView = SIE.AutoUI.createDetailView(mainBlock);
                var entity = new detailView._model();
                detailView.setData(entity);
                var ui = detailView.getControl();
                //ui.config.items.forEach(function (c) {
                //    c.labelWidth = 150;
                //    c.labelAlign = "left";
                //});
               // ui.config.items[0].labelWidth = 120;
                var win = SIE.Window.show({
                    title: "新增列数据".t(),
                    width: 500,
                    height: 220,
                    items: ui,
                    buttons: [
                        {
                            xtype: "button", text: "确定".t(), handler: function () {
                                me.callback(view,detailView, win);
                            }
                        },
                        {
                            xtype: "button", text: "取消".t(), handler: function () {
                                win.close();
                            }
                        }
                    ]
                });
            }
        });
    },
    callback: function (view, detailView, win) {
        var isvalidate = detailView.validateData();
        if (isvalidate == false) {
            SIE.Msg.showError("数据验证不通过!".t());
            return;
        }
        var store = view.getData();
        var data = detailView.getCurrent().data;
        if (data.Alpha == null || data.Alpha == "") {
            SIE.Msg.showError("[α]不能为空!".t());
            return;
        }
        var array = [];
        var a = data.SampleQty.split(';');
        for (var i = 0; i < a.length; i++) {
            if (isNaN(a[i])) {
                SIE.Msg.showError("t值请输入数字类型，多个值使用分号分隔!".t());
                return;
            }
            array.push(parseFloat(a[i]).toFixed(6));
        }

        var sampleQtyArray = store.data.items.select(function (c) { return Ext.Number.from(c.data.SampleQty, -1); }).distinct();
        if (Ext.Array.contains(sampleQtyArray, -1)) {
            SIE.Msg.showError("转换数值失败，请检查!".t());
            return
        }

        if (array.length != sampleQtyArray.length) {
            SIE.Msg.showError("添加t值的数量与存在的n数量不一致!".t());
            return
        }

        var oldColumn = store.changeColumns.filter(function (c) {
            if (c.dataIndex && c.dataIndex.indexOf("Alpha_") >= 0) {
                return c;
            }
        }).select(function (c) { return c.header; });

        if (Ext.Array.contains(oldColumn, data.Alpha.toString())) {
            SIE.Msg.showError("已经存在相同的列!".t());
            return;
        }

     
        var index = oldColumn.length + 1;
        store.changeColumns.push({
            header: data.Alpha.toString(), dataIndex: 'Alpha_' + index, sortable: false, width: 100, xtype: "numbercolumn", revertInvalid: true, format: "", isAdd: true, isDirty: true,
            editor: {
                xtype: 'numberfield', allowBlank: true, allowDecimals: true, allowNegative: false, step: 1, decimalPrecision: 5, MinValue: 0.00001
            }
        });

        for (var i = 0; i < store.data.items.length; i++) {
            var record = store.data.items[i];
            //record.data['Alpha_' + index] = this.getBit(array[i]);
            record.set('Alpha_' + index,this.getBit(array[i]));
        }
        view.getController().drawGrid(view);
        win.close();
    },
    //四舍五入
    getBit: function (value) {
        return SIE.Static.ControlCharts.Helper.round(value, 5);
    },
});