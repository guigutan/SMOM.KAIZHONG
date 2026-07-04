Ext.define('SIE.Web.Kit.APS.FactoryConfirms.FactoryConfirmUIGenerator', {
    extend: 'SIE.autoUI.UIGenerator',
    _mainView: null,
    _childView: null,
    _grandsonView: null,
    _conditionView: null,
    _htmls: null,
    initStore: function () {
        var date = new Date();
        var year = date.getFullYear(); //获取完整的年份(4位)
        var lastYear = date.getFullYear() - 1;
        var lastYear2 = date.getFullYear() - 2;
        var afterYear = date.getFullYear() + 1;
        var optionDataStore = new Ext.data.SimpleStore({
            fields: [
                { name: 'value', mapping: 'value'}
            ],
            data: [{ 'value': lastYear2 }, { 'value': lastYear }, { 'value': year }, { 'value': afterYear }]
        });
        return optionDataStore;
    },

    /**必须实现的方法 */
    generateControl: function (aggtMeta, entity) {
            this._mainView = this._generateMainView(aggtMeta);
            var control = this._layout();
            return new SIE.autoUI.ControlResult(this._mainView, control);
        },

    /**生成主视图 */
    _generateMainView: function (aggtMeta, mainView) {
            var mk = aggtMeta.mainBlock;
            mainView = this._vf.createListView(mk);
            if (aggtMeta.children) {
                this._generateChild(aggtMeta.children[0], mainView);
            }
            if (aggtMeta.surrounders) {
                this._generateConditionView(aggtMeta.surrounders[0], mainView);
            }
            return mainView;
        },

    /**生成查询视图 */
    _generateConditionView: function (surrounderMeta, mainView) {
            var cr = SIE.view.RelationView;
            var conditionView = this._vf.createConditionView(surrounderMeta.mainBlock);
            var reverseRelation = new SIE.view.RelationView(cr.result, mainView);
            var relation = new SIE.view.RelationView(surrounderMeta.surrounderType, conditionView);
            mainView._setRelation(relation);
            conditionView._setRelation(reverseRelation);
            this._conditionView = conditionView;
        },

    /**生成子视图 */
    _generateChild: function (childMeta, mainView) {
            var childView = this._vf.createListView(childMeta.mainBlock);
            childView._childProperty = childMeta.childProperty;
            childView._associatedProperty = childMeta.associatedProperty;
            childView._setParent(mainView);
            this._childView = childView;
        },

    _layoutChildren: function (regions) {
        var me = this;
        regions.main._view._relations[0]._target.mainLayout = me;
        var toolbar = null;
        var dockItems = regions.main._control.getDockedItems();
        dockItems.forEach(function (dockItem) {
            if (dockItem.xtype === 'toolbar')
                toolbar = dockItem;
        });
        var closeRateControl = me.createAbnormalCloseRate(me);
        var lineChartControl = me.createLineChart(me);
        return Ext.widget('container', {
            layout: 'border',
            bodyBorder: false,
            items: [{
                region: 'north',
                items: toolbar,
                border: false,
            }, {
                region: 'center',
                layout: 'border',
                xtype: 'panel',
                border: false,
                items: [closeRateControl, lineChartControl]
            }]
        });
    },
    /**
      * 上下框架
    */
    _layout: function () {
        var me = this;
        var store = me.initStore();
        var mainItems = {
            xtype: 'container',
            layout: 'border',
            background: '#fff',
            scrollable: false,
            border: 0,
            defaults: {
                collapsible: false,
                split: true,
                layout: 'fit',
                border: 0
            },
            items: [{
                region: 'center',
                height: '49%',
                defaults: {
                    layout: 'fit'
                },
                items: me._mainView.getControl(),
            }, {
                region: 'south',
                height: '51%',
                layout: 'border',
                width: "98%",
                autoScroll: true,
                scrollable: true,
                background: '#fff',
                labelWidth: 40,
                defaults: {
                    collapsible: false,
                    layout: 'fit',
                    border: 0
                },
                items: [{
                    region: 'north',
                    height: 60,
                    tbar: {
                        items: ['全年每月负载：', {
                           xtype: 'combobox',
                            typeAhead: true,
                           id:'comboboxYear',
                           width: 80,
                           indent: true,
                           store: store,
                           displayField: 'value',
                           mode: 'local',   //数据本地
                           forceSelection: true, //单选
                           selectOnFocus: true,
                           listeners: {
                               select: function (combo, record, opts) {
                                   var data = combo.getValue();
                                  
                                   me.getstoreData(data);
                                 
                               },
                               afterRender: function (combo) {
                                   me.tableHeadData();
                                   combo.setValue(new Date().getFullYear());//同时下拉框会将与name为firstValue值对应的 text显示
                                   var data = new Date().getFullYear();
                                   me.getstoreData(data);
                                }
                            },
                        }, '年']
                    }

                },
                    
                    {
                        id: "tableHead",
                        height: 40,
                        background: "#fff",
                        region: 'north',
                        html: me._htmls,
                        border: 0,

                    },
                {
                    id: "ewe",
                    background: "#fff",
                    region: 'center',
                    html:  me._htmls,
                    border: 0,
                    scrollable: true,

                },

                ]

            }]
        };

        return Ext.widget('container', {
            border: 0,
            layout: 'border',
            scrollable: true,

            defaults: {
                collapsible: false,
                split: true,
                layout: 'fit',
                border: 0
            },
            items: [
                {
                    xtype: 'panel',
                    region: 'west',
                    width: 285,
                    title: "查询条件",
                    items: this._conditionView.getControl(),
                    layout: 'fit',
                    collapsible: true,
                }, {
                    region: 'center',
                    items: mainItems
                }]
        });

    },

    /**
    * 加载产能数据
    */
    getstoreData: function (data) {
        var me = this;
        SIE.invokeDataQuery({
            method: 'GetYearQty',
            action: 'queryer',
            params: [data],
            type: 'SIE.Web.Kit.APS.FactoryConfirms.CapacityMapDataQueryer',
            token: me._mainView.token,
            success: function (res) {
                var day = res.Result;
                var item = Ext.getCmp("ewe");
                item.setHtml(day);
            }
        });
    },

      tableHeadData: function (data) {
        var me = this;
        SIE.invokeDataQuery({
            method: 'TableHead',
            action: 'queryer',
            type: 'SIE.Web.Kit.APS.FactoryConfirms.CapacityMapDataQueryer',
            token: me._mainView.token,
            success: function (res) {
                var day = res.Result;
                var item = Ext.getCmp("tableHead");
                item.setHtml(day);
            }
        });
    }

  });
    
