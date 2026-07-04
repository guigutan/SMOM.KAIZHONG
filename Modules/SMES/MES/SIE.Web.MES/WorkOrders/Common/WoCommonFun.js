Ext.define('SIE.Web.MES.WoCommonFun', {
    statics: {
        /**
         * 工单属性变更方法
         * @param {any} e
         */
        WorkOrderPropertyChanged: function (e) {

            console.log(e.property);
            var entity = e.entity;
            if (e.property.length > 0) {
                if (entity != null) {
                    if (e.property == 'FactoryId' && entity.data.FactoryId > 0)
                        SIE.Web.MES.WoCommonFun.callFactoryChanged(e);
                    if (e.property == 'ProductId' && entity.data.ProductId > 0)
                        SIE.Web.MES.WoCommonFun.callProductChanged(e);
                    if (e.property == 'PlanQty' && entity.data.ProductId > 0)
                        SIE.Web.MES.WoCommonFun.callPlanQtyChanged(e);
                    if (entity.data.WorkShopId > 0 && e.property == 'WorkShopId')
                        SIE.Web.MES.WoCommonFun.callWorkShopChanged(e);
                    if ((e.property == 'ResourceId' && entity.data.ResourceId > 0 ||
                        e.property == 'PlanBeginDate' && entity.data.PlanBeginDate != null ||
                        e.property == 'Type' ||
                        e.property == 'ProcessTechId') && entity.data.ProductId > 0)
                        SIE.Web.MES.WoCommonFun.callBindingRoutingVersion(e);
                    if (entity.data.VersionId > 0 && (e.property == "ItemExtProp"))
                        SIE.Web.MES.WoCommonFun.callRoutingVersionChanged(e);

                    if (entity.data.VersionId > 0 && (e.property == 'VersionId'))
                        SIE.Web.MES.WoCommonFun.callRoutingVersionChanged(e);
                    if (entity.data.ErpWorkOrderId > 0 && e.property == 'ErpWorkOrderId')
                        SIE.Web.MES.WoCommonFun.callErpWorkOrderChanged(e);
                    if (e.property == 'Type')
                        SIE.Web.MES.WoCommonFun.callWorkOrderTypeChanged(e);
                    if (e.property == 'CustomerId' && entity.data.CustomerId > 0)
                        SIE.Web.MES.WoCommonFun.callCustomerChanged(e);
                }
            }
        },

        /**
         * 工单状态变更弹出窗口
         * @param {any} listView 视图
         * @param {any} title 窗口标题
         */
        showStateChangeView: function (listView, title) {
            SIE.AutoUI.getMeta({
                async: false,
                ignoreCommands: false,
                isDetail: true,
                ignoreQuery: true,
                viewGroup: "DetailsView",
                token: listView.token,
                model: "SIE.Web.MES.WorkOrders.ViewModels.WorkOrderChangeStatus",
                callback: function (res) {
                    var mainBlock;
                    if (res.mainBlock)
                        mainBlock = res.mainBlock;
                    else
                        mainBlock = res;
                    var detailView = SIE.AutoUI.createDetailView(mainBlock);
                    var ui = detailView.getControl();
                    detailView.listView = listView;
                    var model = SIE.Web.MES.WoCommonFun.getEditEntity(listView);
                    model.setIsPause(model.getIsPause().t());
                    detailView.setData(model);

                    var win = SIE.Window.show({
                        title: title + "".t(),
                        width: 480,
                        height: 280,
                        items: ui,
                        buttons: [{
                            xtype: "button", text: "确定".t(), handler: function () {
                                var reason = ui.viewModel.data.p.data.Reason;
                                var woId = detailView.listView.getCurrent().data.Id;
                                var lv = detailView.listView;
                                var me = this;
                                lv.execute({
                                    data: { WorkOrderId: woId, Reason: reason },
                                    success: function (res) {
                                        if (res.Result == true) {
                                            me.up('window').close();
                                            lv.reloadData();
                                        }
                                    }
                                });
                            }
                        },
                        {
                            xtype: "button", text: "取消".t(), handler: function () {
                                this.up('window').close()
                            }
                        }]

                    });
                }
            });
        },
        /**
         * 获取工单状态视图实体
         * @param {any} listView 视图
         */
        getEditEntity: function (listView) {
            var curEntity = listView.getCurrent();
            var curData = curEntity.getData();
            var model = SIE.getModel('SIE.Web.MES.WorkOrders.ViewModels.WorkOrderChangeStatus');
            var newModel = new model();
            newModel.setWorkOrderId(curData.Id);
            newModel.setWorkOrderNo(curData.No);
            if (curData.IsPause == 0)
                newModel.setIsPause("否".t());
            else
                newModel.setIsPause("是".t());
            newModel.setState(curData.DisplayState);
            newModel.ownerView = listView;
            newModel.token = listView.token;
            return newModel;
        },

        //**********************************工单产品属性变更****************************************
        /**
         * 工单产品属性变更
         * @param {any} e
         */
        callProductChanged: function (e) {
            if (e.value == e.oldvalue) return;
            var me = this;
            me.initProductChanged(e.entity);
        },
        /**
         * 工单产品属性变更
         * @param {any} entity
         */
        initProductChanged: function (entity) {
            var me = this;
            entity.belongsView.syncCmdState(entity.belongsView, true);
            var logicalView = CRT.Context.PageContext.getLogicalView();
            var pv = logicalView.getChildren().first(function (m) {
                return m.model === 'SIE.Web.Items.ViewModels.PropertyValueViewModel'
            });
            if (pv) { pv.setData(null) }

            //清空工单工序BOM
            var processBomView = entity.belongsView._children.first(function (p) {
                return p.model == 'SIE.MES.WorkOrders.WorkOrderProcessBom';
            });

            if (processBomView) {
                if (processBomView.getData()) {
                    processBomView.getData().data.clear();
                }

                if (processBomView.getData().data.autoSource) {
                    processBomView.getData().data.autoSource.clear();
                }

                var store = processBomView.getControl().getStore();
                processBomView.getControl().setStore(store);
            }
            me.initRoutingVersion(entity)
            //工单获取BomList、包装信息、打印模板、产品批次信息、工艺路线版本
            //SIE.invokeDataQuery({
            //    method: 'ProductChangedGetData',
            //    params: [entity.data],
            //    action: 'queryer',
            //    type: 'SIE.Web.MES.WorkOrders.WorkOrderDataQueryer',
            //    token: entity.belongsView.token,
            //    success: function (res) {
            //        var resultData = res.Result;
            //        if (!resultData) return;
            //        me._setBomData(entity, resultData);
            //        me._setPackData(entity, resultData);
            //        me._setTemplateData(entity, resultData);
            //        me._setBatchData(entity, resultData);
            //        me._setRoutingVersionData(entity, resultData);
            //        entity.belongsView.getData().setPanelQty(resultData.PanelQty);
            //        entity.belongsView.getData().setIsPanelWorkOrder(resultData.IsPanelWorkOrder);
            //    }
            //});
        },
        /**
         * 工单产品属性变更后,设置bom信息
         * @param {any} entity
         * @param {any} resultData
         */
        _setBomData: function (entity, resultData) {
            var bomView = entity.belongsView._children.first(function (p) { return p.model == 'SIE.MES.WorkOrders.WorkOrderBom'; });
            if (!bomView) return;
            var bomStore = bomView.getControl().getStore();
            if (resultData.BomInfos) {
                resultData.BomInfos.forEach(function (p) {
                    p.ItemId_Display = p.ExtValues.ItemId_Display;
                });
                bomStore.setData(resultData.BomInfos);
            }
            //设置bom替代料数据
            var alternativeView = bomView.findChild('SIE.MES.WorkOrders.WorkOrderBomAlternative');
            if (alternativeView) {
                for (var i = 0; i < resultData.BomInfos.length; i++) {
                    bomStore.data.items[i].data.AlternativeList.forEach(function (p) {
                        p.ItemId_Display = p.ExtValues.ItemId_Display;
                    })
                    bomStore.data.items[i].belongsView = bomView;
                    bomStore.data.items[i][alternativeView._childProperty]().setData(bomStore.data.items[i].data.AlternativeList);
                }
            }

            //设置bom属性值数据
            var propertyView = bomView.findChild('SIE.Web.Items.ViewModels.PropertyValueViewModel');
            if (propertyView) {
                var key = propertyView.getAssociateKey();
                var propertyModel = "SIE.Web.Items.ViewModels.PropertyValueViewModel";
                for (var i = 0; i < resultData.BomInfos.length; i++) {
                    var bomPropertyData = [];
                    resultData.WorkOrderBomPropertys.forEach(function (p) {
                        if (p.ParentId == bomStore.data.items[i].getId())
                            bomPropertyData.push(p);
                    });
                    if (!bomStore.data.items[i][key]) {
                        var propertyStore = SIE.data.Utils.createStore({
                            model: propertyModel,
                            remoteSort: true,
                            storeConfig: {
                                proxy: Ext.clone(SIE.getModel(propertyModel).proxyConfig),
                                remoteSort: true
                            }
                        });
                        bomStore.data.items[i][key] = propertyStore;
                    }
                    bomStore.data.items[i][key].setData(bomPropertyData);
                }
            }
            if (resultData.WorkOrderBomPropertys && resultData.WorkOrderBomPropertys.length > 0) {
                resultData.WorkOrderBomPropertys.forEach(function (p) {
                    p.Values = p.Value.split(',');
                    p.DefinitionValueId = p.Value;
                    p.DefinitionId_Display = p.DefinitionName;
                    p.Type = undefined;
                });
                entity.belongsView.bomPropertyList = resultData.WorkOrderBomPropertys;
            }
        },
        /**
         * 工单产品属性变更后,设置包装信息
         * @param {any} entity
         * @param {any} resultData
         */
        _setPackData: function (entity, resultData) {
            var packRule = entity.belongsView._children.first(function (p) { return p.model == 'SIE.MES.WorkOrders.WorkOrderPackageRuleDetail'; });
            if (!packRule) return;
            var packRuleProcess = packRule.findChild("SIE.MES.WorkOrders.WorkOrderProcessPackingUnit");
            var packStore = packRule.getControl().getStore();
            if (resultData.PackageRuleInfos) {
                resultData.PackageRuleInfos.forEach(function (p) {
                    p.NumberRuleId_Display = p.ExtValues.NumberRuleId_Display;
                    p.PrintTemplateId_Display = p.TemplateName;
                });
                packStore.setData(resultData.PackageRuleInfos);
                var tabIndex = 0;
                var user = CRT.Context.GlobalContext.getContext('userInfo');
                var date = new Date();
                packStore.getData().items.forEach(function (p) {
                    packRule.setCurrent(p);
                    //packRuleProcess.getControl().getStore().setData(resultData.PackageRuleInfos[tabIndex].WorkOrderProcessPackingUnitList);
                    resultData.PackageRuleInfos[tabIndex].WorkOrderProcessPackingUnitList.forEach(item => {
                        var newEntity = packRuleProcess.createNewItem();
                        newEntity.setProcessId(item.ProcessId);
                        newEntity.setProcessId_Display(item.ExtValues.ProcessId_Display);
                        newEntity.setCreateBy(user.EmployeeId);
                        newEntity.setCreateByName(user.Name);
                        newEntity.setCreateDate(date);
                        newEntity.setUpdateBy(user.EmployeeId);
                        newEntity.setUpdateByName(user.Name);
                        newEntity.setUpdateDate(date);
                        packRuleProcess.getData().add(newEntity);
                    });
                    tabIndex += 1;
                });
                packRule.setCurrent(null);
            }
        },
        /**
         * 工单产品属性变更后,设置打印模板
         * @param {any} entity
         * @param {any} resultData
         */
        _setTemplateData: function (entity, resultData) {
            var tempView = entity.belongsView._children.first(function (p) { return p.model == 'SIE.Core.Items.LabelPrintTemplate'; });
            if (!tempView) return;
            var viewData = tempView.getData();
            var params = CRT.Context.PageContext.getParams();
            if (params.action != 1) {//0新增、1复制新增、2修改、3查看
                if (resultData.Template) {
                    var tempData = resultData.Template;
                    viewData.setNumberRuleId(tempData.NumberRuleId);
                    viewData.setNumberRuleId_Display(tempData.ExtValues.NumberRuleId_Display);
                    viewData.setLabelTemplateId(tempData.LabelTemplateId);
                    viewData.setLabelTemplateId_Display(tempData.ExtValues.LabelTemplateId_Display);
                    viewData.setPackingTemplateId(tempData.PackingTemplateId);
                    viewData.setPackingTemplateId_Display(tempData.ExtValues.PackingTemplateId_Display);
                    if (tempView._parent._current[tempView.getAssociateKey()].data.items.length > 0) {
                        tempView._parent._current[tempView.getAssociateKey()].data.items[0].data = tempView.getData().data;
                    }
                }
                else {
                    viewData.setNumberRuleId(null);
                    viewData.setNumberRuleId_Display("");
                    viewData.setLabelTemplateId(null);
                    viewData.setLabelTemplateId_Display("");
                    viewData.setPackingTemplateId(null);
                    viewData.setPackingTemplateId_Display("");
                }
            }
        },
        /**
         * 工单产品属性变更后,设置产品批次信息
         * @param {any} entity
         * @param {any} resultData
         */
        _setBatchData: function (entity, resultData) {
            var batchChild = entity.belongsView._children.first(function (p) { return p.model == 'SIE.Core.WorkOrders.WoWipBatch'; });
            if (!batchChild) return;
            if (entity.data.CreateBy == null) {
                if (batchChild._control && batchChild._control.items && batchChild._control.items.items.length > 0) {
                    //var inputId = batchChild._control.items.items[0].inputId;
                    //document.getElementById(inputId).value = 1;
                    batchChild._control.items.items[0].value = 1;
                }
                if (batchChild.getData()) {
                    batchChild.getData().setQty(1);
                }
            }
            if (batchChild._control && batchChild._control.items && batchChild._control.items.items.length > 0)
                batchChild._control.items.items[0].setDisabled(resultData.IsSingle);
        },
        /**
         * 工单产品属性变更后,定义工艺路线版本
         * @param {any} entity
         * @param {any} resultData
         */
        _setRoutingVersionData: function (entity, resultData) {
            if (resultData.RoutingVersion && resultData.RoutingVersion != null) {
                var routing = resultData.RoutingVersion;
                if (routing.Id != entity.data.VersionId) {
                    entity.setVersionId(routing.Id);
                    entity.setVersionId_Display(routing.Name);
                    entity.setVersionName(routing.Name);
                }
                if (routing.RoutingId > 0)
                    entity.setRoutingId(routing.RoutingId);
                else
                    entity.setRoutingId(null);
            }
            else {
                entity.setVersion(null);
                entity.setVersionId(null);
                entity.setRoutingId(null);
            }
        },

        //****************************客户、计划数、工单类型、ERP工单变更*******************************
        /**
         * 工单客户属性变更
         * @param {any} e
         */
        callCustomerChanged: function (e) {
            if (e.value == e.oldvalue) return;
            var me = this;
            var entity = e.entity;
            me.getPrintTemplate(entity);
        },
        /**
         * 获取物料+客户（可扩展）打印模板
         * @param {any} woEntity
         */
        getPrintTemplate: function (woEntity) {
            var tempView = woEntity.belongsView._children.first(function (p) { return p.model == 'SIE.Core.Items.LabelPrintTemplate'; });
            if (!tempView) return;
            SIE.invokeDataQuery({
                method: 'GetTemplateByItemIdOrCustomerId',
                params: [woEntity.data.ProductId, woEntity.data.CustomerId],
                action: 'queryer',
                type: 'SIE.Web.MES.WorkOrders.WorkOrderDataQueryer',
                token: woEntity.belongsView.token,
                success: function (res) {
                    if (res.Result && res.Result.data && res.Result.data.items.length > 0 && tempView.getData().data.NumberRuleId) {
                        var tempData = res.Result.data.items[0];
                        tempView.getData().setNumberRuleId(tempData.data.NumberRuleId);
                        tempView.getData().setNumberRuleId_Display(tempData.data.NumberRuleId_Display);
                        tempView.getData().setLabelTemplateId(tempData.data.LabelTemplateId);
                        tempView.getData().setLabelTemplateId_Display(tempData.data.LabelTemplateId_Display);
                        tempView.getData().setPackingTemplateId(tempData.data.PackingTemplateId);
                        tempView.getData().setPackingTemplateId_Display(tempData.data.PackingTemplateId_Display);
                        tempView._parent._current[tempView.getAssociateKey()].data.items[0] = tempView.getData();
                    }
                    else {
                        tempView.getData().setNumberRuleId(null);
                        tempView.getData().setNumberRuleId_Display("");
                        tempView.getData().setLabelTemplateId(null);
                        tempView.getData().setLabelTemplateId_Display("");
                        tempView.getData().setPackingTemplateId(null);
                        tempView.getData().setPackingTemplateId_Display("");
                    }
                }
            });
        },

        /**
         * 工单计划数量变更
         * @param {any} e
         */
        callPlanQtyChanged: function (e) {
            if (e.value == e.oldvalue) return;
            var woEntity = e.entity;
            var planQty = woEntity.data.PlanQty;
            var bomView = woEntity.belongsView._children.first(function (p) { return p.model == 'SIE.MES.WorkOrders.WorkOrderBom'; });
            if (!bomView) return;
            if (bomView.getData().data && bomView.getData().data.items.length > 0) {
                var bomStore = bomView.getControl().getStore();
                var bomData = bomView.getData().data.items;
                bomData.forEach(function (p) { p.data.RequireQty = p.data.SingleQty * planQty; p.dirty = true; });
                bomStore.setData(bomData);
            }
        },

        /**
         * 工单类型变更事件
         * @param {any} e
         */
        callWorkOrderTypeChanged: function (e) {
            if (e.value == e.oldvalue) return;
            if (e.entity.isNew() == false) return;
            if (e.entity.getType() === 2)
                e.entity.setUseOldSn(true);
            else
                e.entity.setUseOldSn(false);
        },

        /**
         * 工单ERP工单变更事件
         * @param {any} e
         */
        callErpWorkOrderChanged: function (e) {
            var woEntity = e.entity;
            SIE.invokeDataQuery({
                method: 'ErpWorkOrderChanged',
                params: [e.entity.data],
                action: 'queryer',
                type: 'SIE.Web.MES.WorkOrders.WorkOrderDataQueryer',
                token: e.entity.belongsView.token,
                success: function (res) {
                    if (res.Result && res.Result.data && res.Result.data.items.length > 0) {
                        var returnwo = res.Result.data.items[0].data;
                        woEntity.setErpWorkOrderNo(returnwo.ErpWorkOrderNo);
                        woEntity.setErpOrderNo(returnwo.ErpOrderNo);
                        woEntity.setSaleOrderNo(returnwo.SaleOrderNo);
                        woEntity.setCustomerName(returnwo.CustomerName);
                        woEntity.setOrderQty(returnwo.OrderQty);
                    }
                    else {
                        woEntity.setErpWorkOrderNo("");
                        woEntity.setErpOrderNo("");
                        woEntity.setSaleOrderNo("");
                        woEntity.setCustomerName("");
                        woEntity.setOrderQty("");
                    }
                }
            });
        },

        //******************资源、计划开始时间、工单类型、制程工艺，引起工艺路线变更************************
        /**
         * 工单绑定工艺路线版本
         * @param {any} e
         */
        callBindingRoutingVersion: function (e) {
            if (e.value == e.oldvalue) return;
            var me = this;
            me.initRoutingVersion(e.entity);
        },

        /**
         * 定义工艺路线版本
         * @param {any} entity
         */
        initRoutingVersion: function (entity) {
            SIE.invokeDataQuery({
                method: 'BindingRoutingVersion',
                params: [entity.data],
                action: 'queryer',
                type: 'SIE.Web.MES.WorkOrders.WorkOrderDataQueryer',
                token: entity.belongsView.token,
                success: function (res) {
                    if (res.Result && res.Result.data && res.Result.data.items.length > 0) {
                        var ret = res.Result.data.items[0].data;
                        if (ret.Id != entity.data.VersionId) {
                            entity.setVersionId(ret.Id);
                            entity.setVersionId_Display(ret.Name);
                            entity.setVersionName(ret.Name);
                        }
                        if (ret.RoutingId > 0)
                            entity.setRoutingId(ret.RoutingId);
                        else
                            entity.setRoutingId(null);
                    }
                    else {
                        // 清空工艺路线
                        entity.setVersion(null);
                        entity.setVersionId(null);
                        entity.setRoutingId(null);
                        // 清空工单bom
                        var bomView = entity.belongsView._children.first(function (p) { return p.model == 'SIE.MES.WorkOrders.WorkOrderBom'; });
                        if (bomView) {
                            bomView.getData().removeAll();
                        }
                        
                        // 清空工序bom
                        var processBomView = entity.belongsView._children.first(function (p) { return p.model == 'SIE.MES.WorkOrders.WorkOrderProcessBom'; });
                        if (processBomView) {
                            processBomView.getData().removeAll();
                        }
                        
                        // 清空包装规则
                        var packRuleView = entity.belongsView._children.first(function (p) { return p.model == 'SIE.MES.WorkOrders.WorkOrderPackageRuleDetail'; });
                        if (packRuleView) {
                            packRuleView.getData().removeAll();
                        }
                        
                        // 清空打印设置
                        var tempView = entity.belongsView._children.first(function (p) { return p.model == 'SIE.Core.Items.LabelPrintTemplate'; });
                        if (tempView && tempView.getData()) {
                            var viewData = tempView.getData();
                            viewData.setNumberRuleId(null);
                            viewData.setNumberRuleId_Display("");
                            viewData.setLabelTemplateId(null);
                            viewData.setLabelTemplateId_Display("");
                            viewData.setPackingTemplateId(null);
                            viewData.setPackingTemplateId_Display("");
                        }
                    }
                }
            });
        },
        /**
         * 工单工厂属性变更
         * @param {any} e
         */
        callFactoryChanged: function (e) {
            if (e.value == e.oldvalue) return;
            var entity = e.entity;
            entity.belongsView.getData().setWorkShopId(0);
            entity.belongsView.getData().setWorkShopId_Display(null);
            var me = this;
            me.callWorkShopChanged(e);
        },

        /**
         * 工单车间属性变更
         * @param {any} e
         */
        callWorkShopChanged: function (e) {
            if (e.value == e.oldvalue) return;
            var entity = e.entity;
            entity.belongsView.getData().setResourceId(0);
            entity.belongsView.getData().setResourceId_Display(null);
            var me = this;
            me.callBindingRoutingVersion(e);
        },

        /**
         * 工单工艺路线版本变更事件
         * @param {any} e
         */
        callRoutingVersionChanged: function (e) {
            if (e.value == e.oldvalue) return;
            var me = this;
            var woEntity = e.entity;
            Ext.MessageBox.show({
                msg: '工艺路线切换, 请稍等...'.L10N(),
                progressText: '数据加载中...'.L10N(),
                width: 300,
                closable: true,
                modal: true,
                wait: {
                    interval: 200
                }
            });

            SIE.invokeDataQuery({
                method: 'RoutingVersionChanged',
                params: [woEntity.data],
                action: 'queryer',
                type: 'SIE.Web.MES.WorkOrders.WorkOrderDataQueryer',
                token: woEntity.belongsView.token,
                success: function (res) {
                    if (res.Result) {
                        //第一步工序清单
                        //第二步工序BOM(取与产品bom交集)
                        //第三步生成工单工序单位关系 
                        woEntity.setLayout(null);
                        //第一步
                        var ProcessRouting = woEntity.belongsView._children.first(function (p) { return p.model == 'SIE.MES.WorkOrders.WorkOrderRoutingProcess'; });
                        if (!ProcessRouting) return;
                        if (ProcessRouting.getData().data.autoSource)
                            ProcessRouting.getData().data.autoSource.clear();
                        ProcessRouting.getData().data.clear();
                        var store = ProcessRouting.getControl().getStore();
                        ProcessRouting.getControl().setStore(store);
                        if (res.Result && res.Result.data) {
                            var resData = res.Result.data.items;
                            ProcessRouting.getControl().getStore().data.add(resData);
                            woEntity.data.RoutingProcessList = res.Result.data.items.select(function (p) { return p.data; });
                        }
                        //第二步 需先设置BOM的数据
                        me.initPackingUnit(woEntity.data, woEntity);
                        var bomView = woEntity.belongsView._children.first(function (p) { return p.model == 'SIE.MES.WorkOrders.WorkOrderBom'; });
                        if (bomView.getControl().getStore().data && bomView.getControl().getStore().data.items.length > 0)
                            woEntity.data.BomList = bomView.getControl().getStore().data.items.select(function (p) { return p.data; });
                        else
                            woEntity.data.BomList = [];
                        //第三步
                        me.initProcessBomList(woEntity.data, woEntity);
                        woEntity.loaded = true;
                        woEntity.belongsView.syncCmdState(woEntity.belongsView, true)
                    }
                },
                error: function (res) {
                    Ext.MessageBox.close();
                }
            });
        },

        /**
         * 初始化工序BOM列表
         * @param {any} wodata
         * @param {any} entity
         */
        initProcessBomList: function (wodata, entity) {
            var bomView = entity.belongsView._children.first(function (p) { return p.model == 'SIE.MES.WorkOrders.WorkOrderProcessBom'; });
            if (!bomView) return;
            if (bomView.getData() == null) return;
            bomView.getData().data.clear();
            if (bomView.getData().data.autoSource)
                bomView.getData().data.autoSource.clear();
            var store = bomView.getControl().getStore();
            bomView.getControl().setStore(store);
            SIE.invokeDataQuery({
                method: 'RoutingVersionChangedProcessBom',
                params: [wodata],
                action: 'queryer',
                type: 'SIE.Web.MES.WorkOrders.WorkOrderDataQueryer',
                token: entity.belongsView.token,
                success: function (res) {

                    // 由于出现多次请求造成数据重复的情况，需要在这里再次清空一下数据
                    bomView.getData().data.clear();
                    if (bomView.getData().data.autoSource)
                        bomView.getData().data.autoSource.clear();
                    var store = bomView.getControl().getStore();
                    bomView.getControl().setStore(store);
                    //---------------------------------------------------------

                    if (res.Result) {
                        if (res.Result.data && res.Result.data.items.length > 0) {
                            var resData = res.Result.data.items;
                            bomView.getControl().getStore().data.add(resData);
                        }
                    }
                    Ext.MessageBox.close();
                },
                error: function (res) {
                    Ext.MessageBox.close();
                }
            });
        },

        /**
         * 初始化工序单位关系
         * @param {any} wodata
         * @param {any} entity
         */
        initPackingUnit: function (wodata, entity) {
            var me = this;
            //工单获取BomList、包装信息、打印模板、产品批次信息、工艺路线版本
            SIE.invokeDataQuery({
                method: 'ProductChangedGetData',
                params: [entity.data],
                async: false,
                action: 'queryer',
                type: 'SIE.Web.MES.WorkOrders.WorkOrderDataQueryer',
                token: entity.belongsView.token,
                success: function (res) {
                    var resultData = res.Result;
                    if (!resultData) return;
                    me._setBomData(entity, resultData);
                    me._setPackData(entity, resultData);
                    me._setTemplateData(entity, resultData);
                    me._setBatchData(entity, resultData);
                    entity.belongsView.getData().setPanelQty(resultData.PanelQty);
                    entity.belongsView.getData().setIsPanelWorkOrder(resultData.IsPanelWorkOrder);
                }
            });
        },

        //**********************************工序BOM属性变更事件****************************************
        /**
         * 工序BOM属性变更事件
         * @param {any} e
         */
        ProcessBomPropertyChanged: function (e) {
            var me = this;
            if (e.property.length > 0) {
                if (e.property == "LinkData") {
                    var linkData = e.entity.data.LinkData;
                    e.entity.setUnitId(linkData.UnitId);
                    e.entity.setUnitId_Display(linkData.UnitId_Display);
                }
                else if (e.property == "RoutingProcessId" && e.value > 0) {
                    //#var routingProcess = e.entity.belongsView._parent._children.first(function (p) { return p.model == 'SIE.MES.WorkOrders.WorkOrderRoutingProcess'; });
                    var routingProcess = me._parent._children.first(function (p) {
                        return p.model == 'SIE.MES.WorkOrders.WorkOrderRoutingProcess';
                    });
                    if (!routingProcess) return;
                    var routData = routingProcess.getData().data.items.where(function (p) { return p.data.ProcessType == 15 || p.data.ProcessType == 25 || p.data.ProcessType == 13; }).select(function (p) { return p.data; });
                    var processId = routData.first(function (p) { return p.Id == e.value; }).ProcessId;
                    e.entity.setProcessId(processId);
                }
            }
        }
    }
});