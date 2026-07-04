Ext.define('SIE.Web.AbnormalInfo.AnomalyMonitors.WritingRuleController',
    {
        //----------------树布局&操作-begin----------------------------
        initTopicTree: function (view) {
            var me = this;
            //初始化表关系
            me.inittabRelations(view);
            //构建主题树
            var store = me.InitRightTreeStore(view);
            return Ext.create('Ext.tree.Panel', {
                xtype: 'treepanel',
                region: 'west',
                id:"AbnomalRuleTopicTree",
                /*            style: 'border-width:0;',*/
                rootVisible: true,
                title: "主题/条件".t(),
                collapsible: true,
                lines: true,
                maxWidth: 500,
                width: 300,
                minWidth: 300,
                store: store,
                listeners: {
                    itemmousedown: function (view, re) {
                        if (re.isRoot()) return;
                    },
                    itemdblclick: function (tree, record, item, index, e, eOpts) {
                        // 双击事件处理逻辑
                        var _me = me;
                        if (record.isLeaf()) {
                            _me.recordTabRelation(view,record);
                            _me.addLayerCondition(view.layerView, record);
                        }
                    }
                }
            });
        },
        treeLevel: 1,
        /**
         * Tree-数据结构
         * @param {any} view
         */
        InitRightTreeStore: function (view) {
            var me = this;
            var param = CRT.Context.PageContext.getParams();        
            var filter = { Method: "GetAnomalyMonitorTree", Parameters: [param.MonitorType] };
            return Ext.create('Ext.data.TreeStore', {
                fields: ['text', 'iconCls'],
                autoLoad: true,
                proxy: {
                    type: 'ajax',
                    url: '/api/DataPortal/Query',//请求
                    reader: {
                        type: 'json',
                    },
                    //传参  
                    extraParams: {
                        action: "queryer",
                        type: "SIE.Web.AbnormalInfo.AbnormalMonitors.DataQuerys.AnomalyMonitorQueryer",
                        filter: SIE.data.Utils.seriaizeRequest(filter),
                        token: view.token
                    },
                    extractResponseData: function (response) {
                        var res = response.responseJson;
                        if (res.Success) {
                            return res.Result;
                        }
                        return [];
                    }
                },
                root: {
                    text: param.MonitorName,
                    expanded: true,
                    TabName: param.MonitorTabName
                },
                listeners: {
                    'nodebeforeexpand': function (node, el, eOpts) {
                        me.treeLevel = node.getDepth();
                        //点击父亲节点的菜单会将节点的id通过ajax请求，将到后台  
                        if (!node.isRoot()) {
                            var pa = { Method: "GetAnomalyMonitorTree", Parameters: [node.data.type] };
                            this.proxy.extraParams.filter = SIE.data.Utils.seriaizeRequest(pa);
                        }
                    }
                }
            });
        },
        /**
         * 解析数据存储的表关系
         * @param {any} view
         */
        inittabRelations: function (view) {
            var me = this;
            var tabs = view.getCurrent().getTabRelations();
            if (tabs.length > 0) {
                me.tabRelations = Ext.JSON.decode(tabs);
            }
        },

        // ----------------初始树布局&操作-end----------------------------


        // ----------------层别条件布局&操作-end----------------------------
        /**
         * 添加层别条件
         * @param {any} view
         * @param {any} record
         */
        addLayerCondition: function (view, entity) {
            var me = this;
            var record = entity.data;
            if (!view) return;
            var newEntity = view.createNewItem();
            newEntity.setLayerName(record.text);
            newEntity.setPropDisTabName(entity.parentNode.data.text);
            newEntity.setLayerColumn(record.field);
            newEntity.setFieldProp(record.editType);
            newEntity.setPropType(record.type);
            newEntity.setPropTabName(record.TabName);
            var rowIdx, colIdx;
            rowIdx = newEntity.rowIdx || 0;
            colIdx = 1;
            view.startEdit(newEntity, rowIdx, colIdx);
            //view.mon(newEntity, "propertyChanged", me.onPropertyChanged, view);
        },
        tabRelations:[],
        /**
         * 记录表关系
         * @param {any} view
         * @param {any} record
         */
        recordTabRelation: function (view,record) {
            var me = this;
            var entity = view.getCurrent();
            var flag = me.tabRelations.any(function (item) {
                return item.TabName == record.data.TabName;
            });
            if (!flag) {
                var tab = {};
                tab.TabName = record.data.TabName;
                if (record.parentNode && !record.parentNode.isRoot()) {
                    tab.parentTabName = record.parentNode.parentNode.data.TabName;
                    tab.SuperRefColumnName = record.parentNode.data.field;
                    tab.RefPColumnName = record.parentNode.data.oneToMoreRelationField;
                    tab.TabType = record.parentNode.data.type
                }
                tab.AbnormalDecisionRuleId = entity.getId();
                me.tabRelations.push(tab);
            }
        },
        /**
         * 属性变更处理
         * @param {any} 
         */
        onPropertyChanged: function (e,opt) {
            var me = this;
            var entity = e.entity;
            var mainView = CRT.Context.PageContext.getLogicalView();
            //条件范围视图
            var sourceView = mainView.getChildren()[0];
            //指标条件列表视图
            var indicatorView = mainView.getChildren()[1];
            if (e.property === "IsWhere" && sourceView) {
                mainView.getController().AddOrDeleteEntity(e.value, sourceView, entity);
            } else if (e.property === "IsCacul" && indicatorView) {
                if (e.value) mainView.getController().getInitialzationCodes(mainView);
                mainView.getController().AddOrDeleteEntity(e.value, indicatorView, entity);
            }
            else if (e.property === "FieldProp") {
                entity.setValue1(null);
                entity.setValue2(null);
            }
            mainView.getController().addOrRemoveSelect(mainView.layerView);
        },                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           
        /**
         * 添加/删除实体
         * @param {any} state
         * @param {any} view
         * @param {any} entity
         */
        AddOrDeleteEntity: function (state, view, entity) {
            if (state === true) {
                var newEntity = view.createNewItem();
                if (this.indicatorCode) {
                    newEntity.setCode(this.indicatorCode);
                    this.indicatorCode = null;
                }
                newEntity.setLayerName(entity.getLayerName());
                newEntity.setLayerConditionId(entity.getId());
                var rowIdx, colIdx;
                rowIdx = newEntity.rowIdx || 0;
                colIdx = 1;
                view.startEdit(newEntity, rowIdx, colIdx);
            }
            else {
                var reData = view.getData().getData().items.find(function (item) {
                    return item.getLayerConditionId() == entity.getId();
                });
                if (reData)
                    view.getData().getData().remove(reData);
            }
        },
        indicatorCode:null,
        getInitialzationCodes: function (view) {
            var me = this;
            SIE.invokeDataQuery({
                type: "SIE.Web.AbnormalInfo.AbnormalMonitors.DataQuerys.AnomalyMonitorQueryer",
                method: "GetInitialzationCodes",
                async: false,
                token: view.token,
                callback: function (res) {
                    if (res.Success) {
                        me.indicatorCode = res.Result;
                    } 
                }
            });
        },

        //------------------数据源模块逻辑
        addOrRemoveSelect: function (layerView) {
            var select = "";
            var layerStore = layerView.getData().getData().items;
            var isHaveGroup = layerStore.any(function (item) { return item.IsGroup });
            if (isHaveGroup) {
                select=layerStore.where(x => x.getIsGroup()).map(x => x.getLayerName()).join(';');
            } else {;
                select= select = layerStore.map(x => x.getLayerName()).join(';');
            }
            var mainView = CRT.Context.PageContext.getLogicalView();
            var entity = mainView.getCurrent();
            entity.setDisPlaySelect(select);
        },
        /**
         *指标运算 -entity与ViewModel数据赋值
         * @param {any} curView
         * @param {any} sourceView
         */
        viewDataCopy: function (curView, sourceView) {
            var entity = curView.getCurrent();
            var sourceEntity = sourceView.getCurrent();
            entity.setIndicatorOperation(sourceEntity.getIndicatorOperation());
            entity.setOperator(sourceEntity.getOperator());
            entity.setValue1(sourceEntity.getValue1());
            entity.setValue2(sourceEntity.getValue2());
            entity.setIndicatorName(sourceEntity.getIndicatorName());
            entity.setIndicatorUnit(sourceEntity.getIndicatorUnit());
            

        },

        generalSqlByDataSource: function (view, sqlFiled) {
            var curent = view.getCurrent();
            SIE.invokeDataQuery({
                type: "SIE.Web.AbnormalInfo.AbnormalMonitors.DataQuerys.AnomalyMonitorQueryer",
                method: "GeneralSqlByDataSource",
                params: [curent.getId()],
                token: view.getToken(),
                async: false,
                callback: function callback(res) {
                    if (res.Success) {
                        sqlFiled.setValue(res.Result);
                    }
                }
            });
        }

    });