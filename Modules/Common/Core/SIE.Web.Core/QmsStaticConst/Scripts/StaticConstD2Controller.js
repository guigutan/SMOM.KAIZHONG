Ext.define('SIE.Web.Static.StaticConstD2Controller', {
    extend: 'Ext.app.ViewController', //需要继承这个
    alias: 'controller.StaticConstD2Controller', //别名
    isViewController: true,

    drawGrid: function (view) {
        //说明：
        //设置新Columns
        //store设置新model
        //store设置新data
        //Ext.grid.Panel.reconfigure(store,columns)重绘grid
        //如果用Ext.create创建新store，出现列名展示的问题（注意一下）

        //判断父View选中行是否有数据，有数据代表加载了则重绘表格的列，无数据代表未加载数据，重绘表格放在store加载完毕事件中触发
        var me = this;
        var parentEntity = view.getParent().getCurrent();
        if (parentEntity) {
            var key = view.getAssociateStoreKey();
            var store = parentEntity[key];
            if (store && store.isLoaded()) {

                if (store.data.items.length <= 0)
                    return;

                if (store.isChanged == true) {
                    var grid = view.getControl();
                    store.data.items.sort(function (a, b) { return a.data.SampleQty - b.data.SampleQty; });//按某字段排序
                    grid.reconfigure(store, store.changeColumns);
                }
                else {
                    var grid = view.getControl();
                    //默认值的单元格不可编辑
                    grid.on("beforecelldblclick", me.beforecelldblclick, this);
                    grid.on("cellclick", me.cellkeydown, view);

                    //新model的fields
                    var fields = [];
                    fields.push({ name: "SampleQty2", type: 'string', defaultValue: null, convert: null });
                    fields.push({ name: "MsaConstD2Type2", type: 'string', defaultValue: null, convert: null });
                    var dynamicColumnArray = store.data.items.select(function (c) { return c.data.TestQty }).distinct();
                    dynamicColumnArray.sort(function (a, b) { return a - b; });//按某字段排序
                    for (var i = 0; i < dynamicColumnArray.length; i++) {
                        var index = i + 1;
                        fields.push({
                            name: 'TestQty_' + index, type: 'number', defaultValue: null, convert: null, allowDecimals: true, allowNegative: false, decimalPrecision: 5, minValue:0.00001 });
                    }
                    //定义model
                    var model = Ext.create("Ext.data.Model", {
                        fields: fields
                    });

                    //原store数据转化为新store
                    var records = [];
                    var d2 = store.data.items.filter(function (e) { return e.data.MsaConstD2Type == SIE.Core.QmsStaticConst.StaticConstD2Type.d2; });
                    d2.sort(function (a, b) { return a.data.TestQty - b.data.TestQty; });
                    var recordD2 = { "SampleQty": 1, "SampleQty2": "d₂", "MsaConstD2Type": SIE.Core.QmsStaticConst.StaticConstD2Type.d2, "MsaConstD2Type2": "d₂" }; 
                    for (var i = 0; i < d2.length; i++) {
                        var index = i + 1;
                        recordD2['TestQty_' + index] = d2[i].data.Value;
                    }
                    recordD2.isAdd = false;
                    records.push(recordD2);

                    var cd = store.data.items.filter(function (e) { return e.data.MsaConstD2Type == SIE.Core.QmsStaticConst.StaticConstD2Type.cd; });
                    cd.sort(function (a, b) { return a.data.TestQty - b.data.TestQty; });
                    var recordCD = { "SampleQty": 1, "SampleQty2": "cd", "MsaConstD2Type": SIE.Core.QmsStaticConst.StaticConstD2Type.cd, "MsaConstD2Type2": "cd" };
                    for (var i = 0; i < cd.length; i++) {
                        var index = i + 1;
                        recordCD['TestQty_' + index] = cd[i].data.Value;
                    }
                    recordCD.isAdd = false;
                    records.push(recordCD);

                    var rowIndex = store.data.items.filter(function (e) { return e.data.MsaConstD2Type == SIE.Core.QmsStaticConst.StaticConstD2Type.V; }).select(function (c) { return c.data.SampleQty }).distinct();
                    rowIndex.sort(function (a, b) { return a - b; });//按某字段排序
                    for (var i = 0; i < rowIndex.length; i++) {
                        var recordV = { "SampleQty": rowIndex[i], "SampleQty2": rowIndex[i], "MsaConstD2Type": SIE.Core.QmsStaticConst.StaticConstD2Type.V, "MsaConstD2Type2": "V" };
                        var recordVData = store.data.items.filter(function (e) { return e.data.MsaConstD2Type == SIE.Core.QmsStaticConst.StaticConstD2Type.V && e.data.SampleQty == rowIndex[i]; });
                        recordVData.sort(function (a, b) { return a.data.TestQty - b.data.TestQty; });
                        for (var j = 0; j < recordVData.length; j++) {
                            var index = j + 1;
                            recordV['TestQty_' + index] = recordVData[j].data.Value;
                        }
                        recordV.isAdd = false;
                        records.push(recordV);

                        var recordD22 = { "SampleQty": rowIndex[i], "SampleQty2": rowIndex[i], "MsaConstD2Type": SIE.Core.QmsStaticConst.StaticConstD2Type.D2s, "MsaConstD2Type2": "d₂*" };
                        var recordD22Data = store.data.items.filter(function (e) { return e.data.MsaConstD2Type == SIE.Core.QmsStaticConst.StaticConstD2Type.D2s && e.data.SampleQty == rowIndex[i]; });
                        recordD22Data.sort(function (a, b) { return a.data.TestQty - b.data.TestQty; });
                        for (var j = 0; j < recordD22Data.length; j++) {
                            var index = j + 1;
                            recordD22['TestQty_' + index] = recordD22Data[j].data.Value;
                        }
                        recordD22.isAdd = false;
                        records.push(recordD22);

                    }


                    //组建grid的config
                   
                    var gridColumns = [];
                    var nColumn = {
                        header: '样本数', dataIndex: 'SampleQty2', sortable: false, width: 100, revertInvalid: false, format: "", isAdd: false,
                        editor: {
                            xtype: 'numberfield', allowBlank: false, allowDecimals: false, allowNegative: false,  minValue: 1
                        }
                    };
                    gridColumns.push(nColumn);

                    var typeColumn = {
                        header: "值/测量次数",
                        dataIndex: "MsaConstD2Type2",
                        revertInvalid: false,
                        sortable: false,
                        value: null,
                        xtype: "comboboxcolumn",
                    };
                    gridColumns.push(typeColumn);

                    for (var i = 0; i < dynamicColumnArray.length; i++) {
                        var index = i + 1;
                        gridColumns.push({
                            header: dynamicColumnArray[i].toString(), dataIndex: 'TestQty_' + index, sortable: false, width: 100, xtype: "numbercolumn", revertInvalid: true, format: "", isAdd: false,
                            editor: {
                                xtype: 'numberfield', allowBlank: false, allowDecimals: true, allowNegative: false, decimalPrecision: 5, minValue: 0.00001
                            }
                        });
                    }
                    store.setData(records);
                    store.data.items.sort(function (a, b) { return a.data.SampleQty - b.data.SampleQty; });//按某字段排序
                    store.data.items.forEach(function (c) { c.markSaved() });//按某字段排序
                    store.isChanged = true;
                    store.changeModel = model;
                    store.changeColumns = gridColumns;
                    grid.reconfigure(store, gridColumns);
                }
            }
        }
    },
    
    beforecelldblclick: function ( td, cellIndex, record, tr, rowIndex, e, eOpts) {
        var me = this;
        var columnTest = td.getGridColumns()[cellIndex.cellIndex].text;
        var record = tr.data;
        if (record.isAdd) {
            return true;//新增加的数据可以编辑
        }
        var defaultColumnArray = [];//2-20为默认列，不能删除
        for (var i = 2; i <= 20; i++) {
            defaultColumnArray.push(i.toString());
        }
        var defaultRowArray = [];//1-20为默认行，不能删除
        for (var i = 1; i <= 20; i++) {
            defaultRowArray.push(i);
        }

        if (Ext.Number.from(columnTest, -1) == -1) {
            return false;//不是数字类型的列不可编辑
        }
        if (Ext.Array.contains(defaultColumnArray, columnTest) && Ext.Array.contains(defaultRowArray, record.SampleQty)) {

            //默认数据不允许编辑
            return false;
        }

    },
    cellkeydown: function (td, cellIndex, record, tr, rowIndex, e, eOpts) {
        var view = this;
        view.syncCmdState();
    }
});
