Ext.define('SIE.Web.Static.StaticConstTController', {
    extend: 'Ext.app.ViewController', //需要继承这个
    alias: 'controller.StaticConstTController', //别名
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
                if (store.isChanged == true) {
                    var grid = view.getControl();
                    store.sort("SampleQty","ASC");
                    grid.reconfigure(store, store.changeColumns);
                }
                else {
                    var grid = view.getControl();
                    //默认值的单元格不可编辑
                    grid.on("beforecelldblclick", me.beforecelldblclick, this);
                    grid.on("cellclick", me.cellkeydown, view);

                    //行分组的Key，即以其相关的数据集作为一行。
                    var rowGroupKeyArray = store.data.items.select(function (c) { return c.data.SampleQty }).distinct();
                    //组建列，以第一行的数据集
                    var columnData = store.data.items.filter(function (e) { return e.data.SampleQty == rowGroupKeyArray[0]; });
                    columnData.sort(function (a, b) { return a.data.Alpha - b.data.Alpha; });//按某字段排序

                    //新model的fields
                    var fields = [];
                    fields.push({ name: "SampleQty", type: 'number', defaultValue: null, convert: null });
                    for (var i = 0; i < columnData.length; i++) {
                        var index = i + 1;
                        fields.push({ name: 'Alpha_' + index, type: 'number', defaultValue: null, convert: null });
                    }
                    //定义model
                    var model = Ext.create("Ext.data.Model", {
                        fields: fields
                    });

                    //原store数据转化为新store
                    var records = [];
                    for (var i = 0; i < rowGroupKeyArray.length; i++) {
                        var record = { "SampleQty": rowGroupKeyArray[i] };
                        var recordData = store.data.items.filter(function (e) { return e.data.SampleQty == rowGroupKeyArray[i]; });
                        recordData.sort(function (a, b) { return a.data.Alpha - b.data.Alpha; });
                        for (var j = 0; j < recordData.length; j++) {
                            var index = j + 1;
                            record['Alpha_' + index] = recordData[j].data.Value;
                        }
                        record.isAdd = false;
                        records.push(record);
                    }

                    //组建grid的config
                    var gridColumns = [];
                    var nColumn = {
                        header: 'ν/α', dataIndex: 'SampleQty', sortable: false, width: 100, xtype: "numbercolumn", revertInvalid: true, format: "", isAdd: false,
                        editor: {
                            xtype: 'numberfield', allowBlank: true, allowDecimals: true, allowNegative: true, step: 1, decimalPrecision: 5
                        }
                    };
                    gridColumns.push(nColumn);

                    for (var i = 0; i < columnData.length; i++) {
                        var index = i + 1;
                        gridColumns.push({
                            header: columnData[i].data.Alpha.toString(), dataIndex: 'Alpha_' + index, sortable: false, width: 100, xtype: "numbercolumn", revertInvalid: true, format: "", isAdd: false,
                            editor: {
                                xtype: 'numberfield', allowBlank: true, allowDecimals: true, allowNegative: true, step: 1, decimalPrecision: 5
                            }
                        });
                    }

                    store.setData(records);
                    //store.data.items.sort(function (a, b) { return a.data.SampleQty - b.data.SampleQty; });//按某字段排序
                    //store.data.items.forEach(function (c) { c.markSaved() });//按某字段排序
                    store.commitChanges();
                    store.sort("SampleQty", "ASC");
                    store.isChanged = true;
                    store.changeModel = model;
                    store.changeColumns = gridColumns;
                    grid.reconfigure(store, gridColumns);
                    Ext.defer(function () {
                        grid.scrollTo(0, 0);
                    }, 500);
                   
                }

            }
        }
    },
    beforecelldblclick: function (td, cellIndex, record, tr, rowIndex, e, eOpts) {
        var me = this;
        var columnTest = td.getGridColumns()[cellIndex.cellIndex].text;
        var record = tr.data;
        //if (record.isAdd) {
        //    return true;//新增加的数据可以编辑
        //}

        var defaultColumnArray = ["ν/α", "0.25", "0.2", "0.15", "0.1", "0.05", "0.025", "0.01", "0.005", "0.003"];//默认列，不能删除
        var defaultRowArray = [];//1-100为默认行，不能删除     
        for (var i = 1; i <= 100; i++) {
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
