Ext.define('SIE.Web.EMS.SpecialEquipment.RegularInspections.Scripts.RegularInsDtlController', {
    extend: 'Ext.app.ViewController', //需要继承这个
    alias: 'controller.RegularInsDtlController', //别名
    isViewController: true,

    /**
     * 添加样本值的前一列列名
     */
    columnBeforeSamplingValue: "检验结果",

    // 获取已生成样本列的总列数
    getDynamicColumns: function (view) {
        var dynamicColumns = 0;
        view.getControl().columnManager.columns.forEach(function (column) {
            if (column.isDynamic)
                dynamicColumns += 1;
        });
        return dynamicColumns;
    },
    // 数据列列数
    getConfigSamplingQty: function (view) {
        var current = view.getCurrent();
        return current.getData().VoInitialSamplingQty;
    },
    // 获取检验单所有检验项目明细中最大的样本数
    getMaxSamplingQty: function (view) {
        var current = view.getCurrent();
        return current.getData().VoMaxSamplingQty;
    },
    // 检验中的单据获取所有检验项目中已生成样本数的最大值
    getMaxValueListCount: function (view) {
        var current = view.getCurrent();
        return current.getData().VoMaxValueListCount;
    },
    // 获取对应检验项目的测试值列表
    getValueList: function (view, detail) {
        var bill = view.getCurrent();
        var valueList = bill.getData().VoAllQuantitativeValues;
        var filterValues = valueList.where(function (item) { return item.RegularInspectionDetailId === detail.getId(); });
        return filterValues;
    },
    getCheckValueRelations: function (record) {
        var filterValues = [];
        if (record && record.isModel) {
            //lambda 写法支持 依赖平台29982变更集，但是更符合阅读习惯，所以换
            var filterValues = record.relations.where(function (item) { return item.key === 'RegularInspectionValueList'; });
        }
        return filterValues;
    },
    // 获取初始检验值记录的最大Index号，为添加、删除列作定位。
    getInitialMaxIndexOfCheckValues: function (view) {
        var bill = view.getCurrent();
        var valueList = bill.getData().VoAllQuantitativeValues;
        if (Ext.isEmpty(valueList)) return 0;
        var dynamicIndex = valueList.max(function (item) { return item.Index; });
        return dynamicIndex;
    },
    //获取检验值记录对象
    getCheckValueRecord: function (record, dataIndex) {
        var checkRecord = null;
        if (record && record.isModel) {
            var filterValues = this.getCheckValueRelations(record);
            if (filterValues && filterValues.length > 0) {
                var checkStore = filterValues[0].value;
                //规则：用户处理过哪项，则哪项进行变更
                //先从检验值容器中找对应行索引的样本记录
                var checkIndex = checkStore.find('Index', dataIndex, 0, false, false, true); //通过数据查找验证正确-添加删除的列的情况下。//查找方式使用完全匹配，避免查找1时，结果返回10的索引值的情况。
                checkRecord = checkStore.getAt(checkIndex);
                if (Ext.isEmpty(checkRecord)) {
                    //超出样本数的样本值不需创建
                    var curSampleIndex = this.getOrderIndex(record.store, dataIndex); //当前样本列的样本的显示顺序值 
                    checkRecord = new checkStore.model();
                    if (SIE.isEmpty(checkRecord.Index)) {
                        checkRecord.setIndex(dataIndex);
                    }
                    checkRecord.generateId();//添加样本值时生成id
                    checkStore.add(checkRecord);
                }
            }
        }
        return checkRecord;
    },
    //根据样本索引值计算排序的显示顺序值
    getOrderIndex: function (store, checkIndex) {
        var resultIndex = null;
        //查找有最多样本值的项目，然后根据Index排序顺序,查找对应的排序的显示顺序值
        var recordMostValues = store.getData().items.orderByDescending(function (p) { return p.relations.first().value.count(); }).first();
        if (recordMostValues && recordMostValues.relations.length > 0) {
            var checkRelation = recordMostValues.relations.first();
            if (checkRelation) {
                var orderedCheckList = checkRelation.value.getData().items.orderBy(function (p) { return p.getIndex(); });
                for (var i = 0; i < orderedCheckList.length; i++) {
                    if (orderedCheckList[i].getIndex() === checkIndex) {
                        resultIndex = i + 1;
                    }
                }
                //如果找不到，且Index超出已有样本值的最大Index,则是新增列
                if (resultIndex === null && checkIndex > orderedCheckList.max(function (p) { return p.getIndex(); })) {
                    resultIndex = orderedCheckList.length + 1;
                }
            }
        }
        return resultIndex;
    },
    //为每个定量明细构造自己的检验值数据容器
    getQuantativeCheckStore: function (view, record) {
        var pv = view.getParent();
        var checkStore = null;
        var filterValues = this.getCheckValueRelations(record);
        if (filterValues.length > 0) {
            checkStore = filterValues[0].value;
        }
        if (Ext.isEmpty(checkStore)) {
            checkStore = this.createCheckStore();
            var valueList = this.getValueList(pv, record);
            checkStore.loadData(valueList); //有现有检验数据则存放至容器
            checkStore.commitChanges(); //调整状态
            checkStore.queryRecords().forEach(function (value) {
                //ID为空或者0时，数据未保存，设为新增状态
                if (!value.getId())
                    value.phantom = true;
            });
            var relation = {
                key: 'RegularInspectionValueList', value: checkStore
            };
            record.relations.push(relation);
        }
        return checkStore;
    },
    //创建检验值数据容器
    createCheckStore: function () {
        var templateStore = Ext.create('Ext.data.Store', {
            model: 'SIE.EMS.SpecialEquipment.RegularInspections.RegularInspectionValue',
            data: [],
            proxy: {
                type: 'memory',
                reader: {
                    type: 'json'
                }
            }
        });
        return templateStore;
    },

    //定量数据容器数据加载时触发
    onQuantativeStoreLoad: function (records, quantativeView) {
        if (records.length > 0) {
            var ctl = quantativeView.getController();
            for (var i = 0, length = records.length; i < length; i++) {
                if (ctl) {
                    var record = records[i];
                    record.belongsView = quantativeView;
                    quantativeView.mon(record, 'propertyChanged', ctl.onQuantativeValueChanged, quantativeView);//绑定检验值属性变更事件
                }
            }
            Ext.defer(function () {
                ctl.callQuantativeCheckDynamicColumn(quantativeView);
            }, 1); //1ms待页面重绘完成才开始计算添加
        }
    },

    //检验值属性变更事件
    onQuantativeValueChanged: function (e) {
        var me = this;
        var keyword = 'Value';
        var entity = e.entity;
        var property = e.property;
        if (property && typeof (property) === "string" && property.indexOf(keyword) >= 0) {
            var ctl = this.getController();
            var dataIndex = property.replace(keyword, '');
            var checkRecord = ctl.getCheckValueRecord(entity, dataIndex);
            if (checkRecord) {
                checkRecord.setValue(e.value);
            }
            var bill = me.getParent().getCurrent();
            ctl.matchBillResult(bill);
        }
        else if (e.property === 'InspectionResult') {
            var store = me.getData();
            var parent = me.getParent().getCurrent();
            var datas = store.queryRecords();
            if (datas.some(function (rec) { return rec.getInspectionResult() == SIE.EMS.InspectionResult.Fail; })) {
                parent.setInspectionResult(SIE.EMS.InspectionResult.Fail);
            }
            else if (datas.some(function (rec) { return rec.getInspectionResult() == null; })) {
                parent.setInspectionResult(null);
            }
            else {
                parent.setInspectionResult(SIE.EMS.InspectionResult.Pass);
            }
        }
    },

    //计算检验结果
    matchBillResult: function (bill) {
    },

    //添加动态列
    addDynamicColumn: function (view, columnHeaderIndex, rowIndex) {
        var me = this;
        var pv = view.getParent();
        var quantativeStore = view.getData();
        if (quantativeStore.getCount() < 1) { //没记录时
            return;
        }
        var gridPanel = view.getControl();
        var columns = gridPanel.columnManager.columns;
        // 查找样本数所在列的索引，每次添加的样本列插入样本数后面
        var position = 0;
        for (var i = 0; i < columns.length; i++) {
            if (columns[i].config.header == me.columnBeforeSamplingValue.t()) { position = i; break; }
        }
        var text = '数据'.t() + columnHeaderIndex;
        var name = 'Value' + columnHeaderIndex;
        var field = { name: name, type: 'number', defaultValue: null, convert: null };

        var column = {
            text: text, header: text, dataIndex: name, sortable: false, width: 65,
            editor: {
                xtype: 'numberfield', allowBlank: true, allowDecimals: true, allowNegative: true, step: 1, decimalPrecision: 5
            }
        };
        gridPanel.addColumn(field, column, position);

        //dynamicIsReady 友好输入，定焦点，默认第一行,isread冲突了
        if (view.dynamicIsReady) {
            var defalutRecord = quantativeStore.getAt(0);
            view.startEdit(defalutRecord, 0, position + 1);
        }
    },

    //处理定量样本动态测量列
    callQuantativeCheckDynamicColumn: function (view) {
        if (view && view.isView) {
            var me = this; //me socpe is viewController;
            var pv = view.getParent();
            var current = pv.getCurrent();
            var quantativeView = view;
            var writingReportView = quantativeView.getParent();
            var bill = writingReportView.getCurrent();
            var samplingQty = 0;
            var rowIndexRange = [];
            me.removeAllDynamicColumns(quantativeView);//如果已有动态列,先清空定量视图已有动态列
            if (bill.data.InspectionStatus === SIE.EMS.Enums.InspectionStatus.Pending.value) {
                samplingQty = me.getConfigSamplingQty(writingReportView);
                //待检- 初始生成样本列（根据配置项和所有检验项目的样本数（二者取最小）生成样本列）
                rowIndexRange = Array.range(1, samplingQty + 1);   //行索引集合
            }
            else {
                var detail = pv.getData().getData().VoDetailWithMaxValueList;
                var valueList = current.getData().VoAllQuantitativeValues;
                var rowIndexRange = valueList.where(function (item) { return item.ShippingInspBillDetailId === detail.Id; }).select(function (item) { return item.Index; }).orderBy();  //行索引集合
                samplingQty = me.getMaxValueListCount(writingReportView);
                //检验中- 根据检验单所有检验项目明细中最大的样本数生成样本列
            }
            if (samplingQty > 0) {
                for (var i = 0; i < samplingQty; i++) {
                    var columnHeaderIndex = i + 1;
                    var rowIndex = rowIndexRange[i];
                    me.addDynamicColumn(quantativeView, columnHeaderIndex, rowIndex);
                }
                var quantativeStore = quantativeView.getData();
                if (quantativeStore.isStore) {
                    //测试值数据容器-行转列显示
                    for (var i = 0, length = quantativeStore.getCount(); i < length; i++) {
                        var record = quantativeStore.getAt(i);
                        var checkStore = me.getQuantativeCheckStore(quantativeView, record);
                        me.callQuantativeCheckDynamicColumnValue(record, checkStore);
                    }
                    if (Ext.isEmpty(quantativeStore.getNewRecords()))
                        quantativeStore.commitChanges();//定量-数据容器装载完后的状态
                }
                quantativeView.dynamicIndex = me.getInitialMaxIndexOfCheckValues(writingReportView);
                quantativeView.dynamicIsReady = true; //定量视图动态列准备就绪
            }
            view.syncCmdState();
        }
    },

    //处理定量样本动态列值
    callQuantativeCheckDynamicColumnValue: function (detailRecord, checkStore) {
        if (checkStore && checkStore.isStore) {
            var keyword = 'Value';
            checkStore.each(function (record) {
                var checkValue = record.getValue();
                if (checkValue !== null) {
                    detailRecord.set(keyword + record.getIndex(), checkValue);
                }
            });
        }
    },

    //删除所有动态列
    removeAllDynamicColumns: function (quantativeView) {
        var gridPanel = quantativeView.getControl();
        var columns = gridPanel.columnManager.columns;
        if (columns) {
            var ctl = quantativeView.getController();
            // 查找备注所在列的索引
            var delIndex = 0;
            for (var i = 0; i < columns.length; i++) {
                if (columns[i].config.header === ctl.columnBeforeSamplingValue.t()) { delIndex = i; break; }
            }
            if (delIndex > 0) {
                delIndex += 1;  //从样本数的下一列开始删除
                columns.forEach(function (item, index, array) {
                    if (item.isDynamic) {
                        ctl.syncCheckValuesOfRemoveColumn(quantativeView, index);//删除Store的列
                        gridPanel.removeColumn(delIndex); //删除视图配置的列
                    }
                });
            }
        }
        quantativeView.dynamicIndex = null;
        quantativeView.dynamicIsReady = false;
    },
    //删除列时-同步检验值记录状态
    syncCheckValuesOfRemoveColumn: function (view, colIndex) {
        var gridPanel = view.getControl();
        var gridStore = gridPanel.getStore();
        var columns = gridPanel.columnManager.columns;
        var focusColumn = columns[colIndex];
        if (focusColumn.isDynamic) {
            for (var i = 0, length = gridStore.getCount(); i < length; i++) { //处理所有定量的明细记录
                var quantativeDetailRecord = gridStore.getAt(i);
                if (quantativeDetailRecord) {
                    var keyword = 'Value';
                    var dataIndex = focusColumn.dataIndex.replace(keyword, '');
                    var checkRecord = this.getCheckValueRecord(quantativeDetailRecord, dataIndex);//获取明细对应的检验样本值记录
                    if (checkRecord) {
                        var checkStore = checkRecord.store;
                        checkStore.remove(checkRecord);
                    }
                    //调整删除样本数后的样本值记录的Index
                    this.resetCheckValuesAfterRemoveColumn(quantativeDetailRecord, dataIndex);
                }
            }
        }
    },

    //删除动态列
    removeDynamicColumn: function (view) {
        var me = this;  // controller
        var gridPanel = view.getControl();
        var focusColIndex = gridPanel.view.lastFocused.colIdx;
        var bill = view.getParent().getCurrent();
        me.syncCheckValuesOfRemoveColumn(view, focusColIndex); //删除列时-同步检验值记录状态
        me.resetDynamicColumnsHeader(view, focusColIndex); //grid的标题显示重置连续号
        gridPanel.removeColumn(focusColIndex);//焦点索引列-1，因为排除序号列   
        gridPanel.view.lastFocused = null; //删除列后需要将视图的最后聚焦信息清空，避免造成canExecute权限失控
        me.matchBillResult(bill);
    },

    // 删除样本列后重置其他样本列的列头
    resetDynamicColumnsHeader: function (view, colIndex) {
        var gridPanel = view.getControl();
        var gridStore = gridPanel.getStore();
        var columns = gridPanel.columnManager.columns;
        var focusColumn = columns[colIndex];
        var lastText = focusColumn.text; //上一次的样本标题显示
        var lastDataIndex = focusColumn.dataIndex;
        for (var i = colIndex; i >= 0; i--) { //只循环一次从后往前重置标题显示，
            var column = columns[i];
            if (column && column.isDynamic) {
                // 底层grid.removeColumn方法中使用的是这个属性headerCt.grid.config.columns，所以这里对应使用。
                var cfgColIndex = i - 1;//配置列要排除排序列，所以减1
                var header = column.getRootHeaderCt().grid.config.columns[cfgColIndex];
                if (header) {
                    var headerText = column.text;
                    var headerDataIndex = header.dataIndex;
                    header.text = lastText;//设置列标题
                    header.header = lastText;//设置列标题(标题优先显示该内容)
                    this.resetRecordCheckValueProperty(gridStore, header.dataIndex, lastDataIndex);    //设置record的CheckValue属性名
                    header.dataIndex = lastDataIndex;   //  设置绑定属性
                    lastText = headerText;
                    lastDataIndex = headerDataIndex;
                }
            }
        }
    },

    //设置检验值记录的CheckValue属性名
    resetRecordCheckValueProperty: function (gridStore, propertyName, newPropertyName) {
        for (var i = 0, length = gridStore.getCount(); i < length; i++) { //处理所有定量的明细记录
            var quantativeDetailRecord = gridStore.getAt(i);
            if (quantativeDetailRecord) {
                if (quantativeDetailRecord.data.hasOwnProperty(propertyName)) {
                    quantativeDetailRecord.data[newPropertyName] = quantativeDetailRecord.get(propertyName);
                    delete quantativeDetailRecord.data[propertyName];
                }
            }
        }
    },

    //删除列后重置Index大于该列的样本值记录的Index
    resetCheckValuesAfterRemoveColumn: function (record, dataIndex) {
        var filterValues = this.getCheckValueRelations(record);
        if (filterValues && filterValues.length > 0) {
            var checkStore = filterValues[0].value;
            var checks = checkStore.getData().items.filter(function (check) { return check.getIndex() > dataIndex; });
            if (checks && checks.length > 0) {
                checks.forEach(function (check) {
                    if (check.getIndex() != null)
                        check.setIndex(check.getIndex() - 1);  //后面的数据往前移动 
                });
            }
        }
    },

})