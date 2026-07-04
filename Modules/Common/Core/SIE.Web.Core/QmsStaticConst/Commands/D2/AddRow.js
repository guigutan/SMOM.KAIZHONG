SIE.defineCommand('SIE.Web.Core.QmsStaticConst.Commands.D2.AddRow', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加行", group: "edit", iconCls: "iconfont icon-AddEntity icon-blue" },

    /**
     * @override 执行
     * @param {} view 视图
     * @param {} source 
     * @returns {} 
     */
    execute: function (view, source) {
        var me = this;
        SIE.AutoUI.getMeta({
            model: "SIE.Web.Core.QmsStaticConst.ViewModels.D2AddRowViewModel",
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
                var bigestSampleQty = view.getData().data.items.max(function (c) { return Ext.Number.from(c.data.SampleQty, -1) });
                entity.setN(bigestSampleQty + 1);

                detailView.setData(entity);
                var ui = detailView.getControl();
                var win = SIE.Window.show({
                    title: "新增行数据".t(),
                    width: 500,
                    height: 300,
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
        if (data.N == null || data.N == "") {
            SIE.Msg.showError("[n值]不能为空!".t());
            return;
        }
        var arrayV = [];
        var vA = data.V.split(';');
        for (var i = 0; i < vA.length; i++) {
            var v1 = Ext.Number.from(vA[i], -1);
            if (v1 <= 0) {
                SIE.Msg.showError(Ext.String.format("V第{0}个数据格式不正确(数字且大于0)!".L10N(),i + 1));
                return;
            }


            arrayV.push(parseFloat(vA[i]).toFixed(2));
        }

        var arrayD2S = [];
        var vD2S = data.D2.split(';');
        for (var i = 0; i < vD2S.length; i++) {
            var v2 = Ext.Number.from(vD2S[i], -1);
            if (v2 <= 0) {
                SIE.Msg.showError(Ext.String.format("d₂*第{0}个数据格式不正确(数字且大于0)!".L10N(),i + 1));
                return;
            }

            arrayD2S.push(parseFloat(vD2S[i]).toFixed(5));
        }
        if (arrayV.length != arrayD2S.length) {
            SIE.Msg.showError("V和d₂*的数量不一致!".t());
            return;
        }
        var sampleQtyArray = view.getData().data.items.select(function (c) { return Ext.Number.from(c.data.SampleQty, -1); });
        if (Ext.Array.contains(sampleQtyArray, Ext.Number.from(data.N, -1))) {
            SIE.Msg.showError("已经存在相同的n!".t());
            return;
        }

        var oldColumn = view.getData().changeColumns.filter(function (c) {
            if (c.dataIndex && c.dataIndex.indexOf("TestQty_") >= 0) {
                return c;
            }
        }).select(function (c) { return c.header; });

        if (arrayV.length != oldColumn.length) {
            SIE.Msg.showError("添加值的数量与存在数值的列数不一致!".t());
            return;
        }

        var newRowV = Ext.create(view.model);
        newRowV.data.SampleQty = Ext.Number.from(data.N, -1);
        newRowV.data.SampleQty2 = data.N;
        newRowV.data.MsaConstD2Type = SIE.Core.QmsStaticConst.StaticConstD2Type.V;
        //newRowV.data.MsaConstD2Type = "V";
        for (var i = 0; i < arrayV.length; i++) {
            var newRowVValue = Ext.Number.from(arrayV[i], -1);
            //if (newRowVValue == -1) {
            //    SIE.Msg.showError("V第".t() + (i + 1).toString() + "个数据格式不正确(数字且大于0)!".t());
            //    return;
            //}
            //if (newRowVValue == 0) {
            //    SIE.Msg.showError("V第".t() + (i + 1).toString() + "个数据不能等于0!".t());
            //    return;
            //}
            var index = i + 1;
            newRowV.data["TestQty_" + index] = newRowVValue;
        }
        newRowV.isAdd = true;
        view.getData().add(newRowV);

        var newRowD2 = Ext.create(view.model);
        newRowD2.data.SampleQty = Ext.Number.from(data.N, -1);
        newRowD2.data.SampleQty2 = data.N;
        newRowD2.data.MsaConstD2Type = SIE.Core.QmsStaticConst.StaticConstD2Type.D2s;
        //newRowD2.data.MsaConstD2Type = "d₂*";
        for (var i = 0; i < arrayD2S.length; i++) {
            var newRowD2Value = Ext.Number.from(arrayD2S[i], -1);
            //if (newRowD2Value == -1) {
            //    SIE.Msg.showError("d₂*第".t() + (i + 1).toString() + "个数据格式不正确(数字且大于0)!".t());
            //    return;
            //}
            //if (newRowD2Value == 0) {
            //    SIE.Msg.showError("d₂*第".t() + (i + 1).toString() + "个数据不能等于0!".t());
            //    return;
            //}
            var index = i + 1;
            newRowD2.data["TestQty_" + index] = newRowD2Value;
        }
        newRowD2.isAdd = true;
        newRowD2.data.isAdd = true;
        view.getData().add(newRowD2);
        win.close();
    },
});