SIE.defineCommand('SIE.Web.MES.WorkOrders.SaveWorkOrderCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    /**
   * 是否可执行 
   * @method canExecute 
   * @param {ListLogicalView} view 列表逻辑视图
   * @return {Boolean} 能执行返回true，否则返回false
   */
    canExecute: function (view) {
        var wo = view.getCurrent();
        if (!wo || !wo.data || !wo.isDirty())
            return false;
        return (wo.isDirty() && wo.loaded && wo.loaded) ? true : false;
    },

    execute: function (view) {
        var woData = view._current.data;
        var me = this;
        if (woData.CreateBy > 0) {
            SIE.invokeDataQuery({
                method: 'IsReGenerateTask',
                params: [woData],
                action: 'queryer',
                type: 'SIE.Web.MES.WorkOrders.WorkOrderDataQueryer',
                token: view.token,
                success: function (res) {
                    if (res.Result) {
                        SIE.Msg._showMsg({
                            title: '操作确认'.t(),
                            msg: '当前修改将会影响任务单，是否重新生成任务单？'.L10N(),
                            buttons: Ext.Msg.YESNO,
                            defaultFocus: Ext.Msg.NO,
                            icon: Ext.Msg.QUESTION,
                            iconCls: 'iconfont icon-Notice',
                            fn: function (btnId) {
                                if (btnId === 'yes') {
                                    view._current.data.IsReGenerateTask = true;
                                    me.saveWorkOrder(view);
                                }
                                else {
                                    me.saveWorkOrder(view);
                                }
                            }
                        });
                    }
                    else {
                        view._current.data.IsReGenerateTask = false;
                        me.saveWorkOrder(view);
                    }
                }
            });
        }
        else {
            me.saveWorkOrder(view);
        }
    },

    saveWorkOrder: function (view) {
        var me = this;
        var children = view.getChildren();
        var withChildren = children.length > 0;
        if (!this.onValidation(view)) { SIE.MessageBox.showError("信息填写不完整！".L10N()); return; }
        var planQty = view.getData().getPlanQty();
        var orderQty = view.getData().getOrderQty();
        //if (planQty > orderQty) {
        //    SIE.Msg.showInstantMessage('计划数量不能大于订单数量！'.t());
        //    return false;
        //}
        var provalueChild = children.first(function (p) { return p.model == "SIE.Web.Items.ViewModels.PropertyValueViewModel"; });
        var proValue = [];
        if (provalueChild && provalueChild.getData() && provalueChild.getData().data.items && provalueChild.getData().data.items.length > 0) {
            proValue = provalueChild.getData().data.items.select(function (p) { return p.data; });
        }
        if (provalueChild && me.isRepeat(provalueChild.getData().data.items)) {
            SIE.Msg.showWarning("该工单属性的属性值不能重复！".t());
            return false;
        }

        if (!me.getProcessBomProValue(children, "SIE.MES.WorkOrders.WorkOrderProcessBom"))
            return false;
        if (!me.getBomProValue(children, "SIE.MES.WorkOrders.WorkOrderBom"))
            return false;
        var bomProValue = [];
        var processBomProValue = [];
        SIE.each(me._ownerView.bomPropertyList, function (item) {
            if (!item.data)
                Ext.Array.push(bomProValue, item);
            else
                Ext.Array.push(bomProValue, item.data);
        })
        SIE.each(me._ownerView.processBomPropertyList, function (item) {
            Ext.Array.push(processBomProValue, item.data);
        })
        var processbomChild = view._children.first(function (p) { return p.model == 'SIE.MES.WorkOrders.WorkOrderProcessBom'; });
        processbomChild.getData().data.items.forEach(function (p) { p.dirty = true; });
        view._children.first(function (p) { return p.model == 'SIE.MES.WorkOrders.WorkOrderPackageRuleDetail'; }).getData().data.items.forEach(function (p) { p.dirty = true; });
        var currentRoutingProcess = view._current._RoutingProcessList;
        if (currentRoutingProcess && currentRoutingProcess.getData().items.length == 0) {
            currentRoutingProcess.getData().add(view._children.first(function (p) { return p.model == 'SIE.MES.WorkOrders.WorkOrderRoutingProcess'; }).getData().data.items);
        }
        me.setBatchValue(view);
        if (view._current.data.CreateBy != null) {

            var result = me.setTempValue(view);
        }
        var woId = view.getData().data.Id;
        me.view.getData().dirty = true;//设置保存
        if (!me.validateProValue(proValue, processBomProValue, bomProValue)) {
            SIE.Msg.showWarning("属性值不能为空".t());
            return false;
        }
        Ext.MessageBox.show({
            msg: '正在保存数据'.t(),
            progressText: '...',
            width: 300,
            wait: {
                interval: 200
            }
        });

        var params = CRT.Context.PageContext.getParams();
        view.execute({
            withChildren: withChildren,
            success: function (res) {
                if (params.action === 2) {//修改的时候走这个
                    view._children[5].refreshData();
                    SIE.Msg.showToast('保存成功'.t(), '完成'.t());
                    window.setTimeout(function () {

                        me.initEditTemplate(view);
                        view._current.markSaved();
                        CRT.Workbench.closeCurrentTab();
                        CRT.Event.fire(view.model + "_refresh", view.getCurrent().data.Id);
                    }, 1000);
                } else {
                    me.onSaved(view, res);
                }
            }
        });
    },
    initEditTemplate: function (view) {
        var me = this;
        var batModel = "SIE.Core.Items.LabelPrintTemplate";
        var store = SIE.data.Utils.createStore({
            model: batModel,
            storeConfig: {
                proxy: Ext.clone(SIE.getModel(batModel).proxyConfig)
            },
            remoteSort: true
        });
        var tempView = view._children.first(function (p) { return p.model === batModel; }); 
        /*me.activeTabItem(batModel, true);*/
        var bM = SIE.getModel(batModel);
        var newEntity = new bM();
        store.add(newEntity);
        view._current[tempView.getAssociateKey()] = store;
    },
    //保存后方法
    onSaved: function (view, res) {
        //view.woListView.reloadData();
        var me = this;
        view._children[2].reloadData();
        var current = view.getCurrent();
        current.markSaved();
        me.onSavedMsg(view, res);
        view.syncCmdState(view, false);
    },
    /**
      * 获取属性值
      * @param children 工单子列表
      * @param dataModel 型号
     */
    getProcessBomProValue: function (children, dataModel) {
        var me = this;
        var m = "SIE.Web.Items.ViewModels.PropertyValueViewModel";
        var bomProcessChild = children.first(function (p) { return p.model == dataModel; });
        if (!bomProcessChild) return;
        var bomProValueChild = bomProcessChild._children.first(function (p) { return p.model == m; });
        //直接判断列表数据是否重复
        if (bomProValueChild && me.isRepeat(bomProValueChild.getData().data.items)) {
            SIE.Msg.showWarning("该工序BOM属性的属性值不能重复！".t());
            return false;
        }
        var propertyList = me._ownerView.processBomPropertyList;
        if (bomProValueChild && bomProValueChild.getData().data.items.length > 0) {
            //清空对应物料属性，重新添加对应物料的属性
            var itemid = bomProValueChild.getData().data.items[0].data.ItemId;
            SIE.each(propertyList, function (item) {
                if (item.data.ItemId == itemid) {
                    Ext.Array.remove(propertyList, item);
                }
            })
            SIE.each(bomProValueChild.getData().data.items, function (item) {
                Ext.Array.push(propertyList, item);
            })
            me._ownerView.processBomPropertyList = propertyList;
        }
        else {
            //清空对应物料属性
            if (me._ownerView._children[2]._current != null) {
                var itemid = me._ownerView._children[2]._current.data.ItemId;
                SIE.each(propertyList, function (item) {
                    if (item.data.ItemId == itemid) {
                        Ext.Array.remove(propertyList, item);
                    }
                })
            }
        }
        return true;
    },
    getBomProValue: function (children, dataModel) {
        var me = this;
        var m = "SIE.Web.Items.ViewModels.PropertyValueViewModel";
        var bomProcessChild = children.first(function (p) { return p.model == dataModel; });
        if (!bomProcessChild) return;
        var bomProValueChild = bomProcessChild._children.first(function (p) { return p.model == m; });
        //直接判断列表数据是否重复
        if (bomProValueChild && me.isRepeat(bomProValueChild.getData().data.items)) {
            SIE.Msg.showWarning("该工序BOM属性的属性值不能重复！".t());
            return false;
        }
        var propertyList = me._ownerView.bomPropertyList;
        if (bomProValueChild && bomProValueChild.getData().data.items.length > 0) {
            //清空对应物料属性，重新添加对应物料的属性
            var itemid = bomProValueChild.getData().data.items[0].data.ItemId;
            SIE.each(propertyList, function (item) {
                if ((item.data && item.data.ItemId == itemid) || item.ItemId == itemid) {
                    Ext.Array.remove(propertyList, item);
                }
            })
            SIE.each(bomProValueChild.getData().data.items, function (item) {
                Ext.Array.push(propertyList, item);
            })
            me._ownerView.bomPropertyList = propertyList;
        }
        else {
            //清空对应物料属性
            if (me._ownerView._children[1]._current != null) {
                var itemid = me._ownerView._children[1]._current.data.ItemId;
                SIE.each(propertyList, function (item) {
                    if ((item.data && item.data.ItemId == itemid) || item.ItemId == itemid) {
                        Ext.Array.remove(propertyList, item);
                    }
                })
            }
        }
        return true;
    },
    /**
     * 验证属性值是否空
     * @param {any} proValue 属性值
     * @param {any} processBomProValue 工序BOM属性值
     * @param {any} bomProValue BOM列表属性值
     */
    validateProValue: function (proValue, processBomProValue, bomProValue) {
        var validatevalue = true;
        proValue.forEach(function (model) {
            if (model.Values == undefined || model.DefinitionId == undefined) {
                validatevalue = false;
            }
            else { model.DefinitionValueId = 1; }

        });
        processBomProValue.forEach(function (model) {
            if (model.WoBomValue && model.Values == undefined) {
                model.Values = model.WoBomValue.split(';');
            }
            if (model.Values == undefined || model.DefinitionId == undefined) {
                validatevalue = false;
            }
        });
        bomProValue.forEach(function (model) {
            if (model.WoBomValue && model.Values == undefined) {
                model.Values = model.WoBomValue.split(';');
            }
            if (model.Values == undefined || model.DefinitionId == undefined) {
                validatevalue = false;
            }
            else { model.DefinitionValueId = 1; }
        });
        return validatevalue;
    },
    //判断添加的属性值是否重复
    isRepeat: function (ary) {
        var me = this;
        var nary = [];
        SIE.each(ary, function (item) {
            Ext.Array.push(nary, item.data);
        })
        nary = nary.sort(me.compare('DefinitionId'));
        for (var i = 0; i < ary.length - 1; i++) {
            if (nary[i].DefinitionId == nary[i + 1].DefinitionId) {
                return true;
            }
        }
        return false;
    },
    compare: function (property) {
        return function (a, b) {
            var value1 = a[property];
            var value2 = b[property];
            return value1 - value2;
        }
    },

    /**
     * 设置工单模板属性的值
     * @param {any} view 工单视图
     */
    setTempValue: function (view) {
        var temp = view._children.first(function (p) { return p.model == 'SIE.Core.Items.LabelPrintTemplate'; });
        if (temp) {
            var asKey = temp.getAssociateKey();
            if (view._current[asKey] && view._current[asKey].data.items.length > 0) {
                view._current[asKey].data.items[0].data = temp.getData().data;
                return true;
            } else {
                view._current[asKey].data.items.push(temp.getData().data);
                return true;
            }
            return false;
        }
    },
    /**
         * 设置工单模板属性的值
         * @param {any} view 工单视图
         */
    setBatchValue: function (view) {
        var batch = view._children.first(function (p) { return p.model == 'SIE.Core.WorkOrders.WoWipBatch'; });
        if (batch) {
            var asKey = batch.getAssociateKey();
            if (view._current[asKey] && view._current[asKey].data.items.length > 0)
                view._current[asKey].data.items[0].data = batch.getData().data;
        }
    }
});