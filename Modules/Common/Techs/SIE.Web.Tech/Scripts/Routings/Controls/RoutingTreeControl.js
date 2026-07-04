/**
 * 工艺路线树控件
 * @class SIE.Tech.RoutingTreeControl
 * @constructor
 */
Ext.define('SIE.Tech.RoutingTreeControl', {
    extend: 'Ext.tree.Panel',
    xtype: 'techRoutingTree',
    border: 0,
    rootVisible: false,
    region: 'center',
    split: true,
    flex: 1,
    tbar: {
        xtype: 'container',
        layout: 'anchor',
        defaults: { anchor: '0' },
        defaultType: 'toolbar',
        items: [{
            items: [{
                xtype: 'button',
                btnName: '刷新'.L10N(),
                tooltipType: "title",
                tooltip: '刷新工艺路线'.L10N(),
                style: 'width:35px;',
                iconCls: "iconfont icon-Reload icon-blue",
                listeners: {
                    click: {
                        fn: function () {
                            this.up('treepanel').mainView.CurRoutingVersion = null;
                            this.up('treepanel').loadData();
                            this.up('treepanel').mainView.layout.designControl.designCanvas.clearDrawControl();
                            this.up('treepanel').mainView.layout.designControl.designCanvas.drawRouting(null);
                            praseVersionAction.setDisabled(true);
                            addRoutingAndPraseVersionAction.setDisabled(true);
                        }
                    }
                }
            }, {
                xtype: 'button',
                disabled: true,
                btnName: '定位'.L10N(),
                tooltipType: "title",
                tooltip: '定位工艺路线版本'.L10N(),
                style: 'width:35px;',
                iconCls: "iconfont icon-ArrowLeftRight icon-blue",
                listeners: {
                    click: {
                        fn: function () {
                            var me = this.up('treepanel');
                            me.gotoNodeLocation(me.mainView.CurRoutingVersion, function (realNode) {
                                me.mainView.CurRoutingVersion = realNode;
                            });
                        }
                    }
                }
            }, {
                xtype: 'button',
                disabled: true,
                btnName: '新增'.L10N(),
                tooltipType: "title",
                tooltip: '新增工艺路线/版本'.L10N(),
                style: 'width:35px;',
                iconCls: "iconfont icon-AddEntity icon-green",
                listeners: {
                    click: {
                        fn: function () {
                            var me = this;
                            var routingControl = me.up('treepanel');
                            var record = routingControl.getTreeSelected();
                            var depth = record.get('depth');
                            switch (depth) {
                                case 1:
                                    var new_routing = {
                                        name: '',
                                        desc: '',
                                        category: record.get('text')
                                    };

                                    routingControl.popEditRouting(new_routing, false, function (btn, win) {
                                        if (btn === "确定".t()) {
                                            var addRouting = Ext.create('SIE.Web.Tech.Routings.Commands.AddRouting');
                                            addRouting.execute(routingControl.mainView, {
                                                categoryId: record.get('Id'),
                                                name: new_routing.name,
                                                desc: new_routing.desc,
                                                Category: record
                                            }, win);
                                        }
                                    });
                                    break;
                                case 2:
                                    var addRouting = Ext.create('SIE.Web.Tech.Routings.Commands.AddRoutingVersion');
                                    addRouting.execute(routingControl.mainView, record);
                                    break;
                            }
                        }
                    }
                }
            }, {
                xtype: 'button',
                disabled: true,
                btnName: '修改'.L10N(),
                tooltipType: "title",
                tooltip: '修改工艺路线'.L10N(),
                style: 'width:35px;',
                iconCls: "iconfont icon-EditEntity icon-blue",
                listeners: {
                    click: {
                        fn: function () {
                            var me = this;
                            var routingControl = me.up('treepanel');
                            var record = routingControl.getTreeSelected();
                            var depth = record.get('depth');
                            if (depth === 2) {
                                var edit_routing = {
                                    name: record.get('Name'),
                                    desc: record.get('Description'),
                                    category: record.parentNode.get('text')
                                };
                                routingControl.popEditRouting(edit_routing, true, function (btn, win) {
                                    if (btn === "确定".t()) {
                                        var editRouting = Ext.create('SIE.Web.Tech.Routings.Commands.EditRouting');
                                        editRouting.execute(routingControl.mainView, {
                                            id: record.get('Id'),
                                            name: edit_routing.name,
                                            desc: edit_routing.desc,
                                            routingNode: record
                                        }, win);
                                    }
                                });
                            }
                            routingControl.setCommandState(depth === undefined ? 0 : depth);
                        }
                    }
                }
            }, {
                xtype: 'button',
                disabled: true,
                btnName: '删除'.L10N(),
                tooltipType: "title",
                tooltip: '删除工艺路线/版本'.L10N(),
                style: 'width:35px;',
                iconCls: "iconfont icon-DeleteEntity icon-red",
                listeners: {
                    click: {
                        fn: function () {
                            var me = this;
                            var routingControl = me.up('treepanel');
                            var record = routingControl.getTreeSelected();
                            var depth = record.get('depth');
                            if (depth === 2) {
                                var deleteRouting = Ext.create('SIE.Web.Tech.Routings.Commands.DeleteRouting');
                                deleteRouting.execute(routingControl.mainView, record);
                            }
                            else if (depth === 3) {
                                var deleteRoutingVersion = Ext.create('SIE.Web.Tech.Routings.Commands.DeleteRoutingVersion');
                                deleteRoutingVersion.execute(routingControl.mainView, record);
                            }
                            routingControl.setCommandState(0);
                        }
                    }
                }
            },
            {
                xtype: 'button',
                btnName: '导入'.L10N(),
                tooltipType: "title",
                tooltip: '导入工艺路线'.L10N(),
                style: 'width:35px;',
                iconCls: "iconfont icon-Import icon-green",
                listeners: {
                    click: {
                        fn: function () {
                            var me = this;
                            var routingControl = me.up('treepanel');
                            var cmd = Ext.create(
                                'SIE.Web.Tech.Routings.Commands.ImportRouting', {
                            });
                            routingControl.mainView.routingControl = routingControl;
                            cmd._setOwnerView(routingControl.mainView);
                            cmd.command = Ext.getClassName(cmd);
                            cmd.tryExecute(cmd);
                        }
                    }
                }
            }
            ]
        }, {
            items: [{
                emptyText: '工艺路线'.L10N(),
                xtype: 'TextButtonField',
                width: '100%',
                searchProcess: function () {
                    var me = this;
                    var treepanel = me.up('treepanel');
                    treepanel.filterRoutingTreeData(treepanel, me.value);
                },
                onTriggerClick: function (field, trigger, e) {
                    var me = this;
                    me.searchProcess();
                },
                listeners: {
                    specialKey: function (field, e) {
                        if (e.getKey() === Ext.EventObject.ENTER) {
                            var me = this;
                            me.searchProcess();
                        }
                    }
                }
            }]
        }]
    },
    viewConfig: {
        stripeRows: true,  //背景间隔色
        listeners: {
            itemcontextmenu: function (view, record, item, index, e, eOpts) {
                e.stopEvent();
                if (record.data.leaf) {
                    versionContextMenu.context = view.up('treepanel');
                    versionContextMenu.showAt(e.getXY());
                }
                else if (record.data.depth === 2) {
                    routingContextMenu.context = view.up('treepanel');
                    routingContextMenu.showAt(e.getXY());
                }
                else if (record.data.depth === 1) {
                    familyContextMenu.context = view.up('treepanel');
                    familyContextMenu.showAt(e.getXY());
                }
                return false;
            }
        }
    },
    listeners: {
        'deselect': function (control, record, index) {
            var me = this;
            if (record.data.leaf) {
                if ((record.data.isCopy === true || record.data.Id === 0) && record.parentNode) {
                    var routingNode = me.mainView.routingNode;
                    var routingControl = me.mainView.layout.routingControl;
                    var treePanel = routingNode.getOwnerTree();
                    var treeview = treePanel.getView();
                    SIE.Msg.askQuestion('是否切换工艺路线?是则未保存工艺路线将清除'.t(), function () {
                        var parentNode = record.parentNode;
                        parentNode.removeChild(record);
                        var nowRecord = treePanel.getSelection();
                        treeview.fireEvent('itemdblclick', treeview, nowRecord[0], treeview.getNodeByRecord(nowRecord[0]), treeview.indexOfRow(nowRecord[0]));
                    }, function () {
                        var treeview = treePanel.getView();
                        var nowRecord = treePanel.getSelection();
                        treePanel.getSelectionModel().select(record);
                        treeview.focusRow(record);
                        treePanel.getSelectionModel().deselect(nowRecord);
                    });
                    return false;
                }
                else
                    return true;
            }
        },
        'itemdblclick': function (control, record, item, index) {
            var me = this;
            if (record.data.leaf) {
                if (!(record.data.isCopy === true || record.data.Id === 0))
                    me.updateRouting(record);
            }
        },
        'itemclick': function (control, record, item, index) {
            var depth = record.get('depth');
            control.up('panel').setCommandState(depth);
        },
        //******************************自定义事件************************************
        AddFlow: function (control, record) {
            //新建工艺路线版本触发
        },
        EditFlow: function (control, record) {
            //双击/编辑/删除工艺路线版本触发
        },
        CopyFlow: function (control, record) {

        },
        CopyRoutingVersion: function (control, record) {

        },
        PasteRoutingVersion: function (control, record) {

        },
        selectionchange: function (control, selected, eOpts) {
            //叶子节点且工艺路线版本已发布状态，才能设置默认
            if (selected.length === 1 && selected[0].data.leaf === true && selected[0].data.state === 1) {
                setDefaultAction.enable();
            } else {
                setDefaultAction.disable();
            }
        }
    },
    /**
     * 父主视图
     * @property {ListLogicalView} mainView
     */
    mainView: null,

    /**
     * 工艺路线信息集合
     * @property {SIE.Web.Tech.Routings.FamilyCategoryInfo} familyCategoryInfos
     */
    familyCategoryInfos: null,

    /**
     * 过滤后工艺路线信息集合
     * @property {SIE.Web.Tech.Routings.FamilyCategoryInfo} filterFamilyCategoryInfos
     */
    filterFamilyCategoryInfos: null,

    /**
     * 控件初始化
     * @method initComponent
     * @for SIE.Tech.RoutingTreeControl
     */
    initComponent: function () {
        var me = this;
        me.loadData();
        this.callParent();
    },

    /**
     * 设置工艺路线树控件命令状态
     * @method setCommandState
     * @for SIE.Tech.RoutingTreeControl
     * @param {int} depth 树深度，代表不同类型
     */
    setCommandState: function (depth) {
        var me = this;
        var buttons = me.getView().grid.query('button');
        var btnAdd = buttons.first(function (p) { return p.btnName === '新增'.L10N(); });
        var btnEdit = buttons.first(function (p) { return p.btnName === '修改'.L10N(); });
        var btnDelete = buttons.first(function (p) { return p.btnName === '删除'.L10N(); });
        switch (depth) {
            case 0:
                btnAdd.disable();
                btnEdit.disable();
                btnDelete.disable();
                break;
            case 1:
                btnAdd.enable();
                btnEdit.disable();
                btnDelete.disable();
                break;
            case 2:
                btnAdd.enable();
                btnEdit.enable();
                btnDelete.enable();
                break;
            case 3:
                btnAdd.disable();
                btnEdit.disable();
                btnDelete.enable();
                break;
        }
    },

    /**
     * 设置工艺路线设置默认命令状态
     * @method setDefaultCommandState
     * @for SIE.Tech.RoutingTreeControl
     * @param {bool} flag
     */
    setDefaultCommandState: function (flag) {
        if (flag === true)
            setDefaultAction.enable();
        else
            setDefaultAction.disable();
    },

    /**
     * 弹窗新增/修改工艺路线信息
     * @method popEditRouting
     * @for SIE.Tech.RoutingTreeControl
     * @param {Ext.data.TreeModel} record 工艺路线节点信息
     * @param {bool} isEdit 是否编辑工艺路线
     * @param {委托} callback 编辑后回调
     */
    popEditRouting: function (record, isEdit, callback) {
        var form = Ext.create('Ext.form.Panel', {
            defaultType: 'textfield',
            border: false,
            fieldDefaults: {
                labelAlign: 'right',
                labelWidth: 80
            },
            viewModel: {
                //data:{
                //    m: record
                //}
            },
            items: [{
                fieldLabel: '名称'.L10N(),
                name: 'name',
                bind: '{m.name}',
                allowBlank: false,
                xtype: 'textfield'
            }, {
                fieldLabel: '描述'.L10N(),
                name: 'desc',
                bind: '{m.desc}',
                allowBlank: true,
                xtype: 'textfield'
            }, {
                fieldLabel: '产品族'.L10N(),
                name: 'category',
                bind: '{m.category}',
                allowBlank: false,
                xtype: 'displayfield'
            }]
        });

        form.getViewModel().setData({ m: record });

        var title = isEdit ? '修改工艺路线'.L10N() : '新增工艺路线'.L10N();
        var win = SIE.Window.show({
            title: title,
            minWidth: 320,
            items: form,
            callback: function (btn) {
                if (btn == "确定".L10N()) {
                    callback(btn, win);
                    return true;
                }
            }
        });
    },

    updateRouting: function (record) {
        var me = this;
        me.mainView.CurRoutingVersion = record;
        me.mainView.CurRoutingVersion.set('isCopy', false);
        me.routingChanged(record);
        var buttons = me.getView().grid.query('button');
        var btnLocation = buttons.first(function (p) { return p.btnName === '定位'.L10N(); });
        btnLocation.enable();
    },

    /**
     * 工艺路线变更事件
     * @method routingChanged
     * @for SIE.Tech.RoutingTreeControl
     * @param {Ext.data.TreeModel} record 工艺路线节点信息
     */
    routingChanged: function (record) {
        var me = this;
        this.fireEvent('EditFlow', me, record);
    },

    /**
     * 加载工艺路线数据
     * 后台数据格式：产品族分类--工艺路线-工艺路线版本
     * @method loadData
     * @for SIE.Tech.RoutingTreeControl
     * @param {Function} callback 回调函数
     */
    loadData: function (callback) {
        var me = this;
        var token = me.mainView.token;
        SIE.invokeDataQuery({
            type: "SIE.Web.Tech.Routings.TechDataQueryer",
            method: "GetRoutingTreeInfos",
            token: token,
            params: [],
            success: function (res) {
                if (!res.Success)
                    return;
                me.familyCategoryInfos = res.Result;
                me.bindRoutingTree(me.familyCategoryInfos, me);
                if (callback)
                    callback();
            }
        });
    },

    /**
     * 刷新工艺路线
     * @method reloadData
     * @for SIE.Tech.RoutingTreeControl
     */
    reloadData: function () {
        var me = this;
        me.loadData();
        var record = me.mainView.CurRoutingVersion;
        if (record === null || record === undefined)
            return;
    },

    /**
      * 定位到节点
      * @method gotoNodeLocation
      * @for SIE.Tech.RoutingTreeControl
      * @param {Object} versionNode 工艺路线版本
      * @param {Function} callback 回调
      */
    gotoNodeLocation: function (versionNode, callback) {
        var routingControl = this;
        var routingNode = versionNode.parentNode;
        //由于刷新数据的时候会重置树的对象，原来记下来的node不能用来定位，需找到刷新后的
        var categoryNode = routingNode.parentNode;
        var realCategoryNode = routingControl.getRootNode().childNodes.first(function (p) { return p.data.Id === categoryNode.data.Id; });
        realCategoryNode.expand();
        var realRoutingNode = realCategoryNode.childNodes.first(function (p) { return p.data.Id === routingNode.data.Id; });
        realRoutingNode.expand();
        var realNode = realRoutingNode.childNodes.first(function (p) { return p.data.Id === versionNode.data.Id; });

        var treePanel = realRoutingNode.getOwnerTree();
        treePanel.getSelectionModel().select(realNode);
        var treeview = treePanel.getView();
        treeview.focusRow(realNode);
        if (callback)
            callback(realNode);
    },

    /**
     * 绑定工艺路线数据
     * @method bindRoutingTree
     * @for SIE.Tech.RoutingTreeControl
     * @param {SIE.Web.Tech.Routings.FamilyCategoryInfo} familyCategoryInfos 产品族分类信息
     * @param {treepanel} control 树控件
     */
    bindRoutingTree: function (familyCategoryInfos, control) {
        var tree = { root: { children: [] } };
        children = tree.root.children;
        //遍历产品族分类
        familyCategoryInfos.forEach(function (category) {
            var jsonCategory = { Id: category.Id, text: category.Name, leaf: false, children: [] };
            //遍历工艺路线
            category.RoutingList.forEach(function (routing) {
                let jsonRouting = control.createRoutingJson(routing);
                //遍历工艺路线版本
                routing.VersionList.forEach(function (version) {
                    jsonRouting.children.push(control.createRoutingVersionJson(version));
                });
                jsonCategory.children.push(jsonRouting);
            });
            children.push(jsonCategory);
        });

        var treeStore = Ext.create('Ext.data.TreeStore', tree);
        control.setStore(treeStore);
    },

    /**
     * 创建工艺路线版本信息
     * @method createRoutingJson
     * @for SIE.Tech.RoutingTreeControl
     * @param {SIE.Tech.Routings.Routing} routing 工艺路线信息
     * @return {对象} 工艺路线信息
     */
    createRoutingJson: function (routing) {
        return {
            Id: routing.Id,
            CategoryId: routing.CategoryId,
            Name: routing.Name,
            Description: routing.Description,
            DefaultVersionId: routing.DefaultVersionId,
            MaxVersionNum: routing.MaxVersionNum,
            text: routing.Name,
            leaf: false,
            children: []
        };
    },

    /**
     * 创建工艺路线版本信息
     * @method createRoutingVersionJson
     * @for SIE.Tech.RoutingTreeControl
     * @param {SIE.Tech.Routings.RoutingVersion} version 工艺路线版本
     * @return {对象} 工艺路线版本信息
     */
    createRoutingVersionJson: function (version) {
        var name = Ext.String.format('{0}{1}({2})', version.IsDefault === 1 ? '*' : '', version.Name, version.ReferenceTime);
        return {
            Id: version.Id,
            text: name,
            leaf: true,
            nodetype: 'VersionNode',
            routingId: version.RoutingId,
            layoutId: version.LayoutId,
            state: version.State,
            isDefault: version.IsDefault,
            referenceTime: version.ReferenceTime,
            versionName: version.Name
        };
    },

    /**
     * 获取选中节点信息
     * @method getTreeSelected
     * @for SIE.Tech.RoutingTreeControl
     * @return {Ext.data.TreeModel} 选中节点信息
     */
    getTreeSelected: function () {
        var me = this;
        var record = me.getSelection();
        if (!record || record.length <= 0) {
            SIE.Msg.showWarning('请先选中节点'.t());
            //throw '请先选中节点'.L10N();
            return;
        }
        return record[0];
    },

    /**
     * 过滤工艺路线数据
     * @method filterRoutingTreeData
     * @for SIE.Tech.RoutingTreeControl
     * @param {Control} treepanel 树型控件
     * @param {string} filter 过滤条件
     */
    filterRoutingTreeData: function (treepanel, filter) {
        var me = this;
        var token = treepanel.mainView.token;
        SIE.invokeDataQuery({
            type: "SIE.Web.Tech.Routings.TechDataQueryer",
            method: "GetRoutingTreeInfosByKeyword",
            token: token,
            params: [filter],
            success: function (res) {
                if (!res.Success)
                    return;
                me.familyCategoryInfos = res.Result;
                me.bindRoutingTree(me.familyCategoryInfos, treepanel);
                treepanel.expandAll();
                treepanel.setCommandState(0);
            }
        });
    },

    /**
     * 过滤工艺路线数据(旧方式利用缓存，但未考虑添加和删除)
     * @method filterRoutingData
     * @for SIE.Tech.RoutingTreeControl
     * @param {string} filter 过滤条件
     */
    filterRoutingData: function (filter) {
        var me = this;
        if (filter === '') {
            me.filterFamilyCategoryInfos = me.familyCategoryInfos;
            return;
        }
        var infos = me.familyCategoryInfos;
        var result = [];
        //满足条件的产品族分类
        var categoryList = infos.where(function (p) { return p.Name.indexOf(filter) !== -1; });
        if (categoryList.length > 0) {
            me.filterFamilyCategoryInfos = categoryList;
            return;
        }
        else {
            //满足条件的工艺路线
            var dicRouting = infos.selectMany(function (p) { return p.RoutingList; }).where(function (p) { return p.Name.indexOf(filter) !== -1; }).groupBy(function (p) { return p.CategoryId; });
            //满足条件的产品族分类
            var categoryIds = dicRouting.select(function (p) { return p.key; });
            for (var i = 0; i < infos.length; i++) {
                var info = JSON.parse(JSON.stringify(infos[i])); //带过滤产品族分类，深度拷贝
                if (!categoryIds.any(function (p) { return p === info.Id; }))
                    continue;
                info.RoutingList.removeAll();
                var routings = dicRouting.where(function (p) { return p.key === info.Id; });
                if (routings.length > 0) {
                    routings[0].forEach(function (routing) { info.RoutingList.push(routing); });
                }
                if (result.length > 0 && result.any(function (p) { return p.Id === info.Id; })) {
                    //存在产品族分类则添加工艺路线
                    var categoryInfo = result.where(function (p) { return p.Id === info.Id; }).first();
                    routings.forEach(function (routing) { categoryInfo.RoutingList.push(routing); });
                }
                else {
                    result.push(info);
                }
            }
        }
        me.filterFamilyCategoryInfos = result;
    },

    //*******************************右键菜单逻辑********************************
    /**
     * 设置默认工艺路线版本
     * @method setDefault
     * @for SIE.Tech.RoutingTreeControl
     * @param {Object} recode 版本信息
     */
    setDefault: function (recode) {
        var me = this;
        var token = me.mainView.token;
        SIE.invokeDataQuery({
            type: "SIE.Web.Tech.Routings.TechDataQueryer",
            method: "SetDefault",
            token: token,
            params: [recode.data.routingId, recode.data.Id],
            success: function (res) {
                if (!res.Success)
                    return;
                me.loadData(function () {
                    me.gotoNodeLocation(recode);
                });
            }
        });
    },

    /**
     * 粘贴工艺路线版本
     * @param {Object} copyData 工艺路线Id
     */
    praseRoutingVersion: function (copyData) {
        var me = this;
        var token = me.mainView.token;
        var layout = me.mainView.layout;
        var maxVersionNum = 0;
        if (me.mainView.routingNode.data.MaxVersionNum)
            maxVersionNum = me.mainView.routingNode.data.MaxVersionNum;
        var xml = Ext.getCmp(me.mainView.routingDrawControlId).getXml();
        SIE.invokeDataQuery({
            type: 'SIE.Web.Tech.Routings.TechDataQueryer',
            method: 'PraseRoutingVersion',
            params: [copyData, maxVersionNum, xml],
            action: 'queryer',
            token: token,
            success: function (res) {
                var newLayout = res.Result.layout;
                var routingVersion = res.Result.Version;
                //重置属性控件
                layout.propertyControl.resetPropertyControl();

                //重置工序BOM控件
                var bomConrtrol = layout.childControls.first(function (control) {
                    return control.xtype === 'techProcessBom';
                });
                if (bomConrtrol)
                    bomConrtrol.resetControl();


                //加载工艺路线
                me.resetVersion(layout, copyData.RoutingId);
                layout.designControl.designCanvas.drawRouting(newLayout);
                Ext.query('#spTitle')[0].innerHTML = "";

                if (routingVersion) {
                    var routingNode = me.mainView.routingNode;
                    if (!routingNode) {
                        var routingControl = layout.routingControl;
                        routingNode = routingControl.getRootNode().childNodes.selectMany(function (p) { return p.childNodes; }).first(function (p) { return p.data.Id === routingVersion.routingId; });
                    }
                    var node = routingNode.appendChild(routingVersion);
                    var treePanel = routingNode.getOwnerTree();
                    treePanel.getSelectionModel().select(node);
                    var treeview = treePanel.getView();
                    if (routingNode && routingNode.parentNode) {
                        routingNode.expand();
                        routingNode.parentNode.expand();
                    }
                    treeview.focusRow(node);
                    me.mainView.CurRoutingVersion = node;
                }
            }
        });
    },

    /**
     * 重新设定状态
     * @param {any} layout 布局
     * @param {any} routingId 工艺路线Id
     */
    resetVersion: function (layout, routingId) {
        var me = this;
        layout.designControl.resetMainBlock();
        layout.designControl.designCanvas.setLock(false);
        layout.designControl.setMainBlockCommandDisabled('SaveCommand', false);
        layout.designControl.setMainBlockCommandDisabled('PublishCommand', true);
        layout.designControl.setMainBlockCommandDisabled('LeftRightCommand', true);
        layout.designControl.setMainBlockCommandDisabled('UpDownCommand', true);
        layout.designControl.setMainBlockCommandDisabled('HorizontalDistributionCommand', true);
        layout.designControl.setMainBlockCommandDisabled('VerticalDistributionCommand', true);
    },

    /**
    * 复制工艺路线版本
    * @param {any} record 被复制的版本
    */
    showCopyVersionDialog: function (record) {
        var me = this;
        var token = me.mainView.token;
        SIE.AutoUI.getMeta({
            async: false,
            ignoreCommands: false,
            isDetail: true,
            ignoreQuery: true,
            viewGroup: "DetailsView",
            token: this.view.token,
            model: "SIE.Web.Tech.Routings.RoutingVersionCopyViewModel",
            callback: function (res) {
                var mainBlock;
                if (res.mainBlock)
                    mainBlock = res.mainBlock;
                else
                    mainBlock = res;
                var detailView = SIE.AutoUI.createDetailView(mainBlock);
                var ui = detailView.getControl();
                ui.items.items.forEach(function (p) { p.labelWidth = 150; });
                var routingTree = me;
                var win = SIE.Window.show({
                    title: "工艺路线版本复制选项".L10N(),
                    width: 430,
                    height: 220,
                    items: ui,
                    id: "RoutingVersionCopyViewModel001",
                    callback: function (btn) {
                        if (btn === "确定".t()) {
                            var isCopyActivityProperty = ui.items.items[0].value;
                            var isCopyBom = ui.items.items[1].value;
                            var isCopyFixture = ui.items.items[2].value;
                            routingTree.CopyData = {};
                            routingTree.CopyData.VersionId = record.data.Id;
                            routingTree.CopyData.IsCopyActivityProperty = isCopyActivityProperty;
                            routingTree.CopyData.IsCopyBom = isCopyBom;
                            routingTree.CopyData.IsCopyFixture = isCopyFixture;
                            routingTree.mainView.CurRoutingVersion = record;
                            routingTree.routingChanged(record);
                            praseVersionAction.setDisabled(false);
                            addRoutingAndPraseVersionAction.setDisabled(false);
                        }
                    }
                });
            }
        });
    },

    /**
     * 新增工艺路线并粘贴版本
     */
    showAddRoutingDialog: function () {
        var routingControl = this;
        var record = routingControl.getSelection()[0];
        var records = routingControl.getTreeSelected();

        var new_routing = {
            name: '',
            desc: '',
            category: record.get('text')
        };
        routingControl.popEditRouting(new_routing, false, function (btn, win) {
            if (btn === "确定".t()) {
                var iptdata = {
                    CategoryId: record.get('Id'),
                    Name: new_routing.name,
                    Description: new_routing.desc
                };
                var view = routingControl.ownerCt.ownerCt.ownerCt.ownerCt.view;//routingControl.ownerCt.ownerCt.view;
                view.execute({
                    command: 'SIE.Web.Tech.Routings.Commands.AddRouting',
                    data: iptdata,
                    success: function (res) {
                        //在树中添加工艺路线节点
                        var routing = res.Result;
                        var categroyNode = record;
                        var node = categroyNode.appendChild(routing);
                        var treePanel = categroyNode.getOwnerTree();
                        treePanel.getSelectionModel().select(node);
                        var treeview = treePanel.getView();
                        categroyNode.expand();
                        node.expand();
                        treeview.focusRow(node);
                        routingControl.CopyData.RoutingId = res.Result.Id;
                        routingControl.mainView.routingNode = node;
                        routingControl.praseRoutingVersion(routingControl.CopyData);
                    }
                });
            }
        });
    }
});

/**
 * 设置默认右键命令
 */
var setDefaultAction = Ext.create('Ext.Action', {
    text: '设置默认'.L10N(),
    //iconCls: 'icon-AddEntity icon-green', 框架的图标css怎么出来没弄懂，直接调用只有个框，颜色可以出来
    handler: function (widget, event) {
        var routingTree = widget.up('menu').context;
        var record = routingTree.getSelection()[0];
        if (record && record.data.leaf) {
            routingTree.setDefault(record);
        }
    }
});

/**
 * 复制版本右键命令
 */
var copyVersionAction = Ext.create('Ext.Action', {
    text: '复制版本'.L10N(),
    handler: function (widget, event) {
        var routingTree = widget.up('menu').context;
        var record = routingTree.getSelection()[0];
        if (record && record.data.leaf) {
            routingTree.showCopyVersionDialog(record);
        }
    }
});

/**
 * 删除流程右键命令
 */
var deleteVersionAction = Ext.create('Ext.Action', {
    text: '删除流程'.L10N(),
    handler: function (widget, event) {
        var routingTree = widget.up('menu').context;
        var record = routingTree.getSelection()[0];
        if (record && record.data.leaf) {
            var deleteRoutingVersion = Ext.create('SIE.Web.Tech.Routings.Commands.DeleteRoutingVersion');
            deleteRoutingVersion.execute(routingTree.mainView, record);
        }
    }
});

/**
 * 新增流程右键命令
 */
var addVersionAction = Ext.create('Ext.Action', {
    text: '新增流程'.L10N(),
    handler: function (widget, event) {
        var routingTree = widget.up('menu').context;
        var record = routingTree.getSelection()[0];
        if (record && record.data.depth === 2) {
            var addRouting = Ext.create('SIE.Web.Tech.Routings.Commands.AddRoutingVersion');
            addRouting.execute(routingTree.mainView, record);
        }
    }
});

/**
 * 修改工艺路线右键命令
 */
var editRoutingAction = Ext.create('Ext.Action', {
    text: '修改工艺路线'.L10N(),
    handler: function (widget, event) {
        var routingTree = widget.up('menu').context;
        var record = routingTree.getSelection()[0];
        if (record && record.data.depth === 2) {
            var depth = record.get('depth');
            if (depth === 2) {
                var edit_routing = {
                    name: record.get('Name'),
                    desc: record.get('Description'),
                    category: record.parentNode.get('text')
                };
                routingTree.popEditRouting(edit_routing, true, function (btn, win) {
                    if (btn === "确定".t()) {
                        var editRouting = Ext.create('SIE.Web.Tech.Routings.Commands.EditRouting');
                        editRouting.execute(routingTree.mainView, {
                            id: record.get('Id'),
                            name: edit_routing.name,
                            desc: edit_routing.desc,
                            routingNode: record
                        }, win);
                    }
                });
            }
            routingTree.setCommandState(depth === undefined ? 0 : depth);
        }
    }
});

/**
 * 删除工艺路线右键命令
 */
var deleteRoutingAction = Ext.create('Ext.Action', {
    text: '删除工艺路线'.L10N(),
    handler: function (widget, event) {
        var routingTree = widget.up('menu').context;
        var record = routingTree.getSelection()[0];
        if (record && record.data.depth === 2) {
            var deleteRouting = Ext.create('SIE.Web.Tech.Routings.Commands.DeleteRouting');
            deleteRouting.execute(routingTree.mainView, record);
        }
    }
});

/**
 * 粘贴版本右键命令
 */
var praseVersionAction = Ext.create('Ext.Action', {
    text: '粘贴版本'.L10N(),
    disabled: true,
    handler: function (widget, event) {
        var routingTree = widget.up('menu').context;
        if (routingTree.CopyData && routingTree.CopyData.VersionId) {
            var record = routingTree.getSelection()[0];
            var routingId = record.data.Id;
            if (record && record.data.depth === 2) {
                routingTree.CopyData.RoutingId = routingId;
                routingTree.mainView.routingNode = record;
                routingTree.praseRoutingVersion(routingTree.CopyData);
            }
        }
    }
});

/**
 * 新增工艺路线右键命令
 */
var addRoutingAction = Ext.create('Ext.Action', {
    text: '新增工艺路线'.L10N(),
    handler: function (widget, event) {
        var routingTree = widget.up('menu').context;
        var record = routingTree.getSelection()[0];
        if (record && record.data.depth === 1) {
            var new_routing = {
                name: '',
                desc: '',
                category: record.get('text')
            };

            routingTree.popEditRouting(new_routing, false, function (btn, win) {
                if (btn === "确定".t()) {
                    var addRouting = Ext.create('SIE.Web.Tech.Routings.Commands.AddRouting');
                    addRouting.execute(routingTree.mainView, {
                        categoryId: record.get('Id'),
                        name: new_routing.name,
                        desc: new_routing.desc,
                        Category: record
                    }, win);
                }
            });
        }
    }
});

/**
 * 新增工艺路线并粘贴版本右键命令
 */
var addRoutingAndPraseVersionAction = Ext.create('Ext.Action', {
    text: '粘贴版本'.L10N(),
    disabled: true,
    handler: function (widget, event) {
        var routingTree = widget.up('menu').context;
        if (routingTree.CopyData) {
            var record = routingTree.getSelection()[0];
            if (record && record.data.depth === 1) {
                routingTree.showAddRoutingDialog();
            }
        }
    }
});

/**
 * 工艺路线版本节点右键菜单
 */
var versionContextMenu = Ext.create('Ext.menu.Menu', {
    items: [
        setDefaultAction, copyVersionAction, deleteVersionAction
    ]
});

/**
 * 工艺路线流程节点右键菜单
 */
var routingContextMenu = Ext.create('Ext.menu.Menu', {
    items: [
        addVersionAction, editRoutingAction, deleteRoutingAction, praseVersionAction
    ]
});

/**
 * 产品族节点右键菜单
 */
var familyContextMenu = Ext.create('Ext.menu.Menu', {
    items: [
        addRoutingAction, addRoutingAndPraseVersionAction
    ]
});