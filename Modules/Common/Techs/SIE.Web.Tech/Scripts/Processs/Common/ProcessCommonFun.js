Ext.define('SIE.Web.Tech.ProcessCommonFun', {
    statics: {
        /**
         * 工序属性变更事件
         * @param {any} e
         */
        ProcessPropertyChanged: function (e) {
            
            var entity = e.entity;
            if (e.property.length > 0) {
                if (entity != null) {
                    if (e.property == 'Type') {
                        var childView = e.entity.belongsView.getChildren().first(function (p) { return p.model == 'SIE.Tech.Processs.ProcessParameter'; });
                        if (!childView) return;
                        childView.getData().data.removeAll();
                        if (e.value < 25 && e.oldvalue >= 25 || e.value >= 25 && e.oldvalue < 25 || e.oldvalue == 'null')//单体工序切换成批次工序或反向
                            SIE.Web.Tech.ProcessCommonFun.initStepChild(entity);
                        if ((e.value == 30 || e.oldvalue == 30) && e.value != e.oldvalue && e.value != 'null') {
                            SIE.Web.Tech.ProcessCommonFun.initParamChild(entity);
                        }
                        SIE.Web.Tech.ProcessCommonFun.initParameterData(entity);
                    }
                    //if (e.property == 'IsOutsourcing')
                    //{
                    //    if (e.value) {
                    //        entity.setEnableMoveInControl(true);
                    //    }
                    //}
                }
            }
        },
        /**
         * 工序类型变更处理
         * @param entity 工序实体     
         */
        initParameterData: function (entity) {
            var _type = entity.data.Type;
            var view = entity.belongsView;
            if (view.getChildren().length <= 0) return;
            var pv = view.getChildren().first(function (m) { return m.model === 'SIE.Tech.Processs.ProcessParameter' });
            if (pv) {
                var childProperty = pv._childProperty;
                if (_type === 0 || _type === 5 || _type ==22) {//检验、终检 老化
                    view.getCurrent()[childProperty]().getData().removeAll();
                    SIE.Web.Tech.ProcessCommonFun.parameterData('通过'.t(), 1, view);
                    //SIE.Web.Tech.ProcessCommonFun.parameterData('失败'.t(), 2, view);
                }
                else if (_type === 10 || _type === 20 || _type === 35 || _type === 40) {
                    //维修、包装
                    view.getCurrent()[childProperty]().getData().removeAll();
                    SIE.Web.Tech.ProcessCommonFun.parameterData('通过'.t(), 1, view);
                }
                else if (_type === 13 || _type === 15 || _type === 25) {
                    view.getCurrent()[childProperty]().getData().removeAll();
                    //SIE.Web.Tech.ProcessCommonFun.parameterData('任意'.t(), 3, view);
                }
                else if (_type === 30) {//批次检验
                    view.getCurrent()[childProperty]().getData().removeAll();
                    SIE.Web.Tech.ProcessCommonFun.parameterData('通过'.t(), 1, view);
                    //SIE.Web.Tech.ProcessCommonFun.parameterData('失败'.t(), 2, view);
                    //SIE.Web.Tech.ProcessCommonFun.parameterData('', 4, view);
                }
            }
        },
        /**
             * 工序类型变更处理
             * @param txt 描述 
             * @param type 工序类型 
             */
        parameterData: function (txt, type, view) {
            var parent = view.getCurrent();
            if (view.getChildren().length <= 0) return;
            var childrenView = view.getChildren().first(function (m) { return m.model === 'SIE.Tech.Processs.ProcessParameter' });
            if (childrenView) {
                var childrenView = view.getChildren()[0];
                var store = childrenView.getData();
                var valtrue = childrenView.addNew();
                valtrue.data.Type = type;
                valtrue.data.Description = txt;
                valtrue.data.ProcessId = parent.getId();
                store.add(valtrue);
                parent[childrenView._childProperty]().getData().add(valtrue)
                valtrue.mon(valtrue, 'propertyChanged', this.onParameterTypeChanged, view);
            }
        },

        /**
         * 工序参数类型变更处理
         * @param {any} e
         */
        onParameterTypeChanged: function (e) {
            if (e.property.length > 0) {
                if (e.property == "Type") {
                    if (e.entity.data.Type == 4) {
                        e.entity.setDescription('');
                    }
                    else {
                        var t = e.entity.data.Type;
                        if (t == 1) {
                            e.entity.setDescription('通过'.t());
                        }
                        else if (t == 2) {
                            e.entity.setDescription('失败'.t());
                        }
                        else if (t == 3) {
                            e.entity.setDescription('任意'.t());
                        }

                        e.entity.setScript('');
                    }
                }
            }
        },


        /**
         * 工序类型变更步骤显示列控制
         * @param {any} entity 工序实体
         */
        initStepChild: function (entity, view) {
            var pType = entity.data.Type;
            var viewGroup;
            if (view) viewGroup = view.viewGroup;
            else viewGroup = entity.belongsView.viewGroup;
            if (entity.belongsView.getChildren().length <= 0) return;
            var stepView = entity.belongsView.getChildren().first(function (m) { return m.model === 'SIE.Tech.Processs.ProcessCollectStep' });
            if (stepView) {
                var stepControl = stepView.getControl();
                if (pType < 25) {
                    Ext.each(stepControl.columns, function (item) {
                        var dIndex = item.dataIndex;
                        if (dIndex == "PlugType" || dIndex == "IsGenerateBatch") {
                            item.hide();
                        }
                        else {
                            item.show();
                        }
                    });
                    SIE.Web.Tech.ProcessCommonFun.setAllData('Single', viewGroup)
                }
                else {
                    Ext.each(stepControl.columns, function (item) {
                        if (item.dataIndex == "IsUnbound") {
                            item.hide();
                        }
                        else {
                            item.show();
                        }
                    });
                    SIE.Web.Tech.ProcessCommonFun.setAllData('Batch', viewGroup)
                }
                stepView.getControl().store.clearData();
                stepControl.setStore(stepView.getControl().store);
            }
        },
        /**
         * 缓存所有选择项，并设置条码类型下拉框的可见值
         * @param {any} type 批次/单体
         */
        setAllData: function (type, viewGroup) {
            SIE.Web.Tech.ProcessCommonFun.setEditorItems('BarcodeType', viewGroup, function (items) {
                if (type == 'Batch') {
                    return items.where(function (p) { return p.data.text === '载具号'.t() || p.data.text === '批次条码'.t(); });
                }
                else {
                    return items.where(function (p) { return p.data.text !== '载具号'.t() && p.data.text !== '批次条码'.t() && p.data.text !== '拼板码'.t(); });
                }
            });
        },
        /**
        * 工序类型（批次检验与其他切换）参数结果下拉选项变更
        * @param {any} entity 工序实体
        */
        initParamChild: function (entity) {
            var pType = entity.data.Type;
            var viewGroup = entity.belongsView.viewGroup;
            SIE.Web.Tech.ProcessCommonFun.setEditorItems('Type', viewGroup, function (items) {
                if (pType === 30) {
                    return items;
                }
                else {
                    return items.where(function (p) { return p.data.text !== '自定义'.t(); });
                }
            });
        },
        /**
         * 工序参数点击事件        
         */
        setStepResultEditorItems: function (g, row, col, record, tr, rowindex) {
            var me = this;
            var pType = me._parent._current.data.Type;
            var viewGroup = me._parent.viewGroup;
            SIE.Web.Tech.ProcessCommonFun.setEditorItems('Type', viewGroup, function (items) {
                if (pType === 30) {
                    return items;
                }
                else {
                    return items.where(function (p) { return p.data.text !== '自定义'.t(); });
                }
            });
        },

        /**
        * 采集步骤行点击事件        
        */
        setBarcodeTypeEditorItems: function (g, row, col, record, tr, rowindex) {
            var me = this;
            var pType = me._parent._current.data.Type;
            var viewGroup = me._parent.viewGroup;
            SIE.Web.Tech.ProcessCommonFun.setEditorItems('BarcodeType', viewGroup, function (items) {
                if (pType >= 25) {
                    return items.where(function (p) { return p.data.text === '载具号'.t() || p.data.text === '批次条码'.t(); });
                }
                else {
                    return items.where(function (p) { return p.data.text !== '载具号'.t() && p.data.text !== '批次条码'.t() && p.data.text !== '拼板码'.t(); });
                }
            });
        },
        /**
    * 设置编辑器项
    * @param {String} typeName 编辑器绑定属性名称
    * @param {Function} callback 回调，过滤数据
    */
        setEditorItems: function (typeName, viewGroup, callback) {
            var resultEditor
            if (viewGroup == "ListView")
                resultEditor = Ext.ComponentQuery.query('[xtype=StepBarcodeTypeEditor]').first(function (p) { return p.name === typeName; });
            else if (viewGroup == "DetailsView")
                resultEditor = Ext.ComponentQuery.query('[xtype=StepBarcodeTypeEditorRouting]').first(function (p) { return p.name === typeName; });
            if (resultEditor) {
                if (!resultEditor.AllData) {
                    resultEditor.AllData = new Ext.data.Store();
                    var items = resultEditor.getStore().data.items;
                    resultEditor.AllData.data.add(items);
                }
                var newStore = new Ext.data.Store();
                var results = callback(resultEditor.AllData.data.items);
                newStore.data.add(results);
                resultEditor.setStore(newStore);
            }
        }
    }
});