SIE.defineCommand('SIE.Web.Kit.APS.ProductLocations.Commands.ImportProductLocationCommand', {
    extend: 'SIE.Web.Common.Import.Commands.ImportCommandBase',
    meta: { text: "导入", group: "business", iconCls: "icon-Download icon-blue" },
    /*
* 创建导入面板--Grid
* @returns  grid
*/
    creatImportGridPanel: function () {
        var me = this;
        //动态Jsonstore格式
        var jsonText = '{\"total\":\"0\",\"data\":[{\"index\":\"\"}],\"columnModle\":[{\"text\":\"No\",\"dataIndex\":\"index\"}],\"fieldsNames\":[{\"name\":\"index\"}]}';
        var json = Ext.util.JSON.decode(jsonText);
        //创建strore对象
        var store = new Ext.data.Store({
            proxy: new Ext.data.MemoryProxy(null),
            fields: json.fieldsNames,
            data: json.data,
            totalProperty: json.total,
            pageSize: 10
        });

        //创建动态JsonStore表格
        var importColumns = json.columnModle;
        var bbar = new Ext.PagingToolbar({
            id: 'failedtoolbar',
            xtype: 'pagingtoolbar',
            store: store,//数据
            displayInfo: true,//是否显示数据信息
            displayMsg: '显示{0}-{1}条记录,共{2}条'.t(),//只有displayInfo:true时才有效，用来显示有数据的提示信息。
            emptyMsg: "没有记录",//没有数据显示的信息,
            items: [
                {
                    xtype: 'combobox',
                    itemId: 'pageSizeItem',
                    store: Ext.create('Ext.data.Store', {
                        fields: ['value'],
                        data: [
                            { "value": 10 },
                            { "value": 20 },
                            { "value": 50 },
                            { "value": 100 },
                            { "value": 500 },
                        ]
                    }),
                    listeners: {
                        change: function (clt, newValue, oldValue, eOpts) {
                            var arrtydata = [];
                            var toolbar = Ext.getCmp('failedtoolbar');
                            toolbar.store.setPageSize(newValue);
                            var pageData = toolbar.getPageData();
                            for (var i = pageData.fromRecord - 1; i <= pageData.toRecord - 1; i++) {
                                arrtydata.push(toolbar.store.data.items[i]);
                            }
                            Ext.getCmp('failedGrid').store.setData(arrtydata);
                        }
                    },
                    value: me._pageSize,
                    width: 72,
                    minValue: 0,
                    maxValue: 5000,
                    queryMode: 'local',
                    displayField: 'value',
                    valueField: 'value',
                }
            ],
            listeners: {
                change: {
                    fn: function (clt, newValue, oldValue, eOpts) {
                        var arrtydata = [];
                        for (var i = this.getPageData().fromRecord - 1; i <= this.getPageData().toRecord - 1; i++) {
                            arrtydata.push(this.store.data.items[i]);
                        }
                        Ext.getCmp('failedGrid').store.setData(arrtydata);
                    }
                }
            }
        });

        var grid = Ext.create("Ext.grid.Panel", {
            id: 'failedGrid',
            name: 'failedGrid',
            title: '导入失败数据'.t(),
            xtype: 'grid-filtering', //类型为锁定表格
            columns: importColumns,
            bodyStyle: 'overflow-x:hidden; overflow-y:hidden',
            store: store,
            height: 280,
            layout: "fit",
            width: '100%',
            loadMask: true,
            autoScroll: true,
            plugins: {
                gridexporter: true
            },
            tbar: [{
                xtype: 'button',
                id: 'Importbutton',
                text: '导出Excel'.t(),
                handler:
                    function () {
                        var grid = Ext.getCmp('failedGrid');
                        grid.saveDocumentAs({
                            type: 'xlsx',
                            title: '导入失败的数据',
                            fileName: 'error.xlsx'
                        });
                    },
            }],
            bbar: bbar,
        });
        return grid;
    }
});