SIE.defineCommand('SIE.Web.Core.QmsStaticConst.Commands.D2.AddColumn', {
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
            model: "SIE.Web.Core.QmsStaticConst.ViewModels.D2AddColumnViewModel",
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
                var bigestTestQty = view.getData().changeColumns.max(function (c) { return Ext.Number.from(c.header, -1) });
                entity.setTestQty(bigestTestQty + 1);
                detailView.setData(entity);
                var ui = detailView.getControl();
                var win = SIE.Window.show({
                    title: "新增列数据".t(),
                    width: 500,
                    height: 500,
                    items: ui,
                    buttons: [
                        {
                            xtype: "button", text: "确定".t(), handler: function () {
                                me.callback(view, detailView, win);
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

        var data = detailView.getCurrent().data;
        var store = view.getData();
        if (data.TestQty == null || data.TestQty=="")
        {
            SIE.Msg.showError("[测量次数]不能为空!".t());
            return;
        }
        if (data.D2 == null || data.D2 == "") {
            SIE.Msg.showError("[d₂]不能为空!".t());
            return;
        }
        if (data.Cd == null || data.Cd == "") {
            SIE.Msg.showError("[cd]不能为空!".t());
            return;
        }


        var arrayV = [];
        var vA = data.V.split(';');
        for (var i = 0; i < vA.length; i++) {
            var newvA = Ext.Number.from(vA[i], -1);
            if (newvA <=0) {
                SIE.Msg.showError(Ext.String.format("V值第{0}个数据格式不正确(数字且大于0)!".L10N(),i + 1));
                return;
            }
            arrayV.push(parseFloat(vA[i]).toFixed(2));
        }

        var arrayD2S =[];
        var vD2S = data.D2S.split(';');
        for (var i = 0; i < vD2S.length; i++) {
            var newvD2S = Ext.Number.from(vD2S[i], -1);
            if (newvD2S <=0) {
                SIE.Msg.showError(Ext.String.format("d₂*值第{0}个数据格式不正确(数字且大于0)!".L10N(),i + 1));
                return;
            }

            arrayD2S.push(parseFloat(vD2S[i]).toFixed(5));
        }
        if (arrayV.length != arrayD2S.length) {
            SIE.Msg.showError("V和d₂*的数量不一致!".t());
            return;
        }


        var sampleQtyArray = store.data.items.filter(function (c) { if (c.data.MsaConstD2Type == SIE.Core.QmsStaticConst.StaticConstD2Type.V) { return c; } }).select(function (c) { return Ext.Number.from(c.data.SampleQty, -1); }).distinct();

        if (arrayV.length != sampleQtyArray.length) {
            SIE.Msg.showError("V添加值的数量与存在的样本数不一致!".t());
            return;
        }
        var oldColumn = store.changeColumns.filter(function (c) {
            if (c.dataIndex && c.dataIndex.indexOf("TestQty_") >= 0) {
                return c;
            }
        }).select(function (c) { return c.header; });
        if (Ext.Array.contains(oldColumn, data.TestQty.toString())) {
            SIE.Msg.showError("已经存在相同的列!".t());
            return;
        }
        var index = oldColumn.length + 1;
        store.changeColumns.push({
            header: data.TestQty.toString(), dataIndex: 'TestQty_' + index, sortable: false, width: 100, xtype: "numbercolumn", revertInvalid: true, format: "", isAdd: true,
            editor: {
                xtype: 'numberfield', allowBlank: true, allowDecimals: true, allowNegative: false, step: 1, decimalPrecision: 5, MinValue: 0.00001
            }
        });
        for (var i = 0; i < store.data.items.length; i++) {
            var record = store.data.items[i];
            if (record.data.MsaConstD2Type == SIE.Core.QmsStaticConst.StaticConstD2Type.d2) {
                //record.data["TestQty_" + index] = Ext.Number.from(data.D2, -1);
                record.set("TestQty_" + index, Ext.Number.from(data.D2, -1));

            }
            else if (record.data.MsaConstD2Type == SIE.Core.QmsStaticConst.StaticConstD2Type.cd) {
                //record.data["TestQty_" + index] = Ext.Number.from(data.Cd, -1);
                record.set("TestQty_" + index, Ext.Number.from(data.Cd, -1));
            }
        }
        var vStore = store.data.items.filter(function (c) { return c.data.MsaConstD2Type == SIE.Core.QmsStaticConst.StaticConstD2Type.V });
        for (var i = 0; i < vStore.length; i++) {
           // vStore[i].data["TestQty_" + index] = Ext.Number.from(arrayV[i], -1);
            vStore[i].set("TestQty_" + index, Ext.Number.from(arrayV[i], -1));
        }
        var d2sStore = store.data.items.filter(function (c) { return c.data.MsaConstD2Type == SIE.Core.QmsStaticConst.StaticConstD2Type.D2s });
        for (var i = 0; i < d2sStore.length; i++) {
            //d2sStore[i].data["TestQty_" + index] = Ext.Number.from(arrayD2S[i], -1);
            d2sStore[i].set("TestQty_" + index, Ext.Number.from(arrayD2S[i], -1));
        }


        view.getController().drawGrid(view);
        win.close();
    },
});