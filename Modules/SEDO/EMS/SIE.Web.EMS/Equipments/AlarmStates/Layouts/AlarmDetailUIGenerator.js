Ext.define('SIE.Web.EMS.Equipments.AlarmStates.Layouts.AlarmDetailUIGenerator', {
    extend: 'SIE.autoUI.AggtUIGeneratorDefault',
    _mainView: null,
    _childView: null,
    _grandsonView: null,
    _conditionView: null,
    _echartPanel: null,
    /**
     * 生成控件重写框架的方法
     * @param aggtMeta 聚合块元数据
     * @param entity 实体
     * @returns 聚合控件
     */
    generateControl: function (aggtMeta, entity) {
        this._mainView = this._generateMainView(aggtMeta);

        var control = this._layout();

        if (this._mainView.hasListeners['isready']) {
            this._mainView.fireEvent('isReady', true);
        }

        return new SIE.autoUI.ControlResult(this._mainView, control);
    },

    /**生成主视图 */
    _generateMainView: function (aggtMeta) {
        var mk = aggtMeta.mainBlock;
        this._initMetaConfig(mk);
        var mainView = this._vf.createListView(mk);
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
        this._initMetaConfig(childMeta.mainBlock);
        var childView = this._vf.createListView(childMeta.mainBlock);
        childView._childProperty = childMeta.childProperty;
        childView._associatedProperty = childMeta.associatedProperty;
        childView._setParent(mainView);
        this._childView = childView;

    },

    /** 
     * 主在上，从孙在下 
    */
    _layout: function () {
        var me = this;
        //考虑权限增加留空处理  
        var childItems = this._childView ? {
            region: 'center',
            xtype: 'panel',
            width: '35%',
            defaults: {
                layout: 'fit'
            },            
            items: me._childView.getControl()
        } : [];

        //考虑权限增加留空处理  
        var grandSonItems = {
            region: 'east',
            width: '65%',
            xtype: 'panel',            
            items: this.initEchartPanel()
        };
        //  
        var mainItems = {
            xtype: 'container',
            layout: 'border',
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
                height: '50%',                
                items: me._mainView.getControl(),
            }, {
                region: 'south',
                xtype: 'container',
                height: '50%',
                layout: 'border',
                scrollable: false,
                border: 0,
                defaults: {
                    collapsible: false,
                    split: true,
                    layout: 'fit',
                    border: 0
                },
                items: [childItems, grandSonItems]
            }]
        };

        return Ext.widget('container', {
            border: 0,
            layout: 'border',
            scrollable: false,

            defaults: {
                collapsible: false,
                split: true,
                layout: 'fit',
                border: 0
            },
            items: [
                {
                    region: 'west',
                    title: '查询条件'.t(),
                    floatable: false,
                    width: 270,
                    items: this._conditionView.getControl(),
                    collapsible: true,
                }, {
                    region: 'center',
                    collapsible: false,
                    items: mainItems
                }]
        });
    },

    /*
     * 初始化一个Panel作为Echart的容器
     */ 
    initEchartPanel: function () {
        if (this._echartPanel) {
            return;
        }

        var panel = new Ext.Panel({
            id: 'echartPanel',
            html: '<div id="mainEchart" style="height:100%;border:1px solid #ccc;padding:10px;"></div>',
            buttonAlign: 'center',
            autoScroll: true,//允许滚动
            bodyStyle: 'overflow-x:hidden; overflow-y:scroll'
            //开启竖直滚动条，关闭水平滚动条
        });

        this._echartPanel = panel;
        return this._echartPanel;
    },
});