SIE.defineCommand('SIE.Web.Core.QmsStaticConst.Commands.ConstT.AddRow', {
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
            model: "SIE.Web.Core.QmsStaticConst.ViewModels.TAddRowViewModel",
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
                var win = SIE.Window.show({
                    title: "新增行数据".t(),
                    width: 500,
                    height: 200,
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

        if (data.SampleQty == null || data.SampleQty == "") {
            SIE.Msg.showError("[n值]不能为空!".t());
            return;
        }
      
        if (data.Alpha === null) {
            SIE.Msg.showError("t值不能为空!".t());
            return;
        }
        var array = [];
        var a = data.Alpha.split(';');
        for (var i = 0; i < a.length; i++) {
            if (isNaN(a[i])) {
                SIE.Msg.showError("t值请输入数字类型，多个值使用分号分隔!".t());
                return;
            }
            array.push(parseFloat(a[i]).toFixed(6));
        }


        var column = view.getData().changeColumns.filter(function (c) {
            if (c.dataIndex && c.dataIndex.indexOf("Alpha_") >= 0) {
                return c;
            }
        }).select(function (c) { return c.header; });

        if (array.length != column.length) {
            SIE.Msg.showError("t值和α列数量不一致!".t());
            return;
        }

        var same = view.getData().data.items.filter(function (c) { if (c.getSampleQty() == data.SampleQty) return c; });
        if (same.length > 0) {
            SIE.Msg.showError("已经存在相同的n值!".t());
            return;
        }


       

        var newRow = Ext.create(view.model);
        newRow.data.SampleQty = data.SampleQty;
        for (var i = 0; i < array.length; i++) {
            var t = Ext.Number.from(array[i], -1);
            if (t == -1) {
                SIE.Msg.showError("输入不是数值类型!".t());
                return;
            }
            var index = i+1;
            newRow.data["Alpha_" + index] = t;

        }
        newRow.isAdd = true;
        newRow.data.isAdd = true;
        view.getData().add(newRow);
        win.close();
    },
});