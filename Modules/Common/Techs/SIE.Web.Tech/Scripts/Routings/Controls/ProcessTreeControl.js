/**
 * 工序树控件
 * @class SIE.Tech.ProcessTreeControl
 * @constructor
 */
Ext.define('SIE.Tech.ProcessTreeControl', {
    extend: 'Ext.tree.Panel',
    xtype: 'techProcessTree',
    width: 200,
    border: 0,
    region: 'center',
    flex: 1,
    rootVisible: false,
    tbar:
    {
        xtype: 'container',
        layout: 'anchor',
        defaults: { anchor: '0' },
        defaultType: 'toolbar',
        items: [{
            items: [{
                xtype: 'button',
                btnName: '刷新',
                tooltipType: "title",
                tooltip: '刷新工序'.L10N(),
                style: 'width:35px;',
                iconCls: "iconfont icon-Reload icon-blue",
                listeners: {
                    click: {
                        fn: function () {
                            this.up('treepanel').loadData();
                        }
                    }
                }
            },
            {
                xtype: 'button',
                btnName: '新增',
                tooltipType: "title",
                tooltip: '新增工序'.L10N(),
                disabled: true,
                style: 'width:35px;',
                iconCls: "iconfont icon-AddEntity icon-green",
                listeners: {
                    click: {
                        fn: function () {
                            var me = this;
                            var processControl = me.up('treepanel');
                            var addProcess = Ext.create('SIE.Web.Tech.Routings.Commands.AddProcess');
                            addProcess.execute(processControl.mainView, processControl);
                        }
                    }
                }
            }, {
                xtype: 'button',
                btnName: '修改',
                tooltipType: "title",
                tooltip: '修改工序'.L10N(),
                disabled: true,
                style: 'width:35px;',
                iconCls: "iconfont icon-EditEntity icon-blue",
                listeners: {
                    click: {
                        fn: function () {
                            var me = this;
                            var processControl = me.up('treepanel');
                            var record = processControl.getSelection()[0];
                            var processId = record.get('Id');
                            var view = processControl.mainView;
                            var token = view.token;
                            SIE.invokeDataQuery({
                                method: 'GetProcessById',
                                params: [processId],
                                action: 'queryer',
                                type: 'SIE.Web.Tech.Routings.TechDataQueryer',
                                token: token,
                                success: function (res) {
                                    view._current = res.Result.data.items[0];
                                    view._current.phantom = false;
                                    view.processControl = processControl;
                                    var cmd = Ext.create(
                                        'SIE.Web.Tech.Routings.Commands.EditProcess', {});
                                    cmd._setOwnerView(view);
                                    cmd.command = Ext.getClassName(cmd);
                                    cmd.tryExecute(cmd);
                                }
                            });
                        }
                    }
                }
            }, {
                xtype: 'button',
                btnName: '删除',
                tooltipType: "title",
                tooltip: '删除工序'.L10N(),
                disabled: true,
                style: 'width:35px;',
                iconCls: "iconfont icon-DeleteEntity icon-red",
                listeners: {
                    click: {
                        fn: function () {
                            var me = this;
                            var processControl = me.up('treepanel');
                            var record = processControl.getSelection()[0];
                            var depth = record.get('depth');
                            if (depth === 3) {
                                var deleteRoutingVersion = Ext.create('SIE.Web.Tech.Routings.Commands.DeleteProcess');
                                deleteRoutingVersion.execute(processControl.mainView, record);
                                processControl.setCommandState(1);
                            }
                        }
                    }
                }
            }]
        },
        {
            emptyText: '工序'.L10N(),
            xtype: 'TextButtonField',
            width: '100%',
            searchProcess: function () {
                var me = this;
                var treepanel = me.up('treepanel');
                treepanel.filterByText(me.value);
                treepanel.setScrollY(0);

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
    },
    listeners: {
        'beforeitemmousedown': function (node, record, item, index, e, eOpts) {
            var me = this;
            var nodes = Ext.query('.x-tree-icon-leaf~.x-tree-node-text', null, item);
            if ((!nodes || nodes.length === 0) && !record.isLeaf())
                return;
            nodes[0].dataset.nodetype = record.get('nodetype');
            var type = record.get('Type');
            var processType = 0;
            var transferType = '';
            switch (record.get('TransferType')) {
                case 0:
                    transferType = 0;
                    break;
                case 1:
                    transferType = 1;
                    break;
                case 2:
                    transferType = 2;
                    break;
            }
            switch (type) {
                case 0:
                    processType = 'Pqc';
                    break;
                case 5:
                    processType = 'Fqc';
                    break;
                case 10:
                    processType = 'Fix';
                    break;
                case 13:
                    processType = 'Rework';
                    break;
                case 15:
                    processType = 'Assembly';
                    break;
                case 20:
                    processType = 'Packing';
                    break;
                case 22:
                    processType = 'Ageing';
                    break;
                case 25:
                    processType = 'BatchAssembly';
                    break;
                case 30:
                    processType = 'BatchPqc';
                    break;
                case 35:
                    processType = 'BatchFix';
                    break;
                case 40:
                    processType = 'BatchPacking';
                    break;
            }
            var nodedata = {
                Text: record.get('processName'),
                ProcessId: record.get('Id'),
                State: 'New',
                ProcessState: 'Not', //工序状态
                Type: 'Interaction', //交互状态
                ProcessType: processType,
                ContainerHeight: 110,
                Index: -1,
                ZIndex: 1,
                IsSelected: false,
                IsRepeat: record.get('IsRepeat'),
                CreateSku: record.get('IsCreateSku'),
                IsCalculate: record.get('IsCalculate'),
                IsOptional: record.get('CanChoose'),

                IsGenerateTask: record.get('IsGenerateTask'),
                IsRequirementTask: record.get('IsRequirementTask'),
                IsBuckleMaterial: record.get('IsBuckleMaterial'),
                IsPassRate: record.get('IsPassRate'), 
                IsBinding: record.get('IsBinding'), 
                IsUnBinding: record.get('IsUnBinding'),
                IsStricter: record.get('IsStricter'),
                Overtime: record.get('Overtime'),
                NewProcess: true,
                MaxPassNum: record.get('MaxPassNum'),
                IsNextMoveIn: record.get('IsNextMoveIn'),
                TransferType: transferType,
                EnableMoveInControl: record.get('enableMoveInControl'),
                IsOutsourcing: record.get('IsOutsourcing'),
                StartProcess: null,
                NormalVictory: { Id: record.get('VictoryStandardId'), Display: record.get('VictoryStandard_Display')},
                RepairVictory: { Id: record.get('RepairVictoryId'), Display: record.get('RepairVictory_Display') },
 
            };
            nodedata.Boms = [];
            nodedata.ProcessBoms = [];
            nodedata.Fixtures = [];
            me.processType(nodedata, processType);
            nodes[0].dataset.nodedata = Ext.encode(nodedata);
            me.mainView.layout.designControl.designCanvas.drawNode([nodes[0]]);
        },
        'itemclick': function (control, record, item, index) {
            var depth = record.get('depth');
            control.up('panel').setCommandState(depth);
        }
    },
    /**
    * 父主视图
    * @property {ListLogicalView} mainView
    */
    mainView: null,

    /**
     * 控件初始化
     * @method initComponent
     * @for SIE.Tech.ProcessTreeControl
     */
    initComponent: function () {
        var me = this;
        me.loadData();
        this.callParent();
    },

    /**
   * 设置工序树控件命令状态
   * @method setCommandState
   * @for SIE.Tech.RoutingTreeControl
   * @param {int} depth 树深度，代表不同类型
   */
    setCommandState: function (depth) {
        var me = this;
        var buttons = me.getView().grid.query('button');
        var btnAdd = buttons.first(function (p) { return p.btnName === '新增'; });
        var btnEdit = buttons.first(function (p) { return p.btnName === '修改'; });
        var btnDelete = buttons.first(function (p) { return p.btnName === '删除'; });
        switch (depth) {
            case 1:
                btnAdd.disable();
                btnEdit.disable();
                btnDelete.disable();
                break;
            case 2:
                btnAdd.enable();
                btnEdit.disable();
                btnDelete.disable();
                break;
            case 3:
                btnAdd.disable();
                btnEdit.enable();
                btnDelete.enable();
                break;
        }
    },

    /**
     * 设置工序类型图标和颜色
     * @method processType
     * @for SIE.Tech.ProcessTreeControl
     * @param {Object} nodeData 工序节点信息
     * @param {string} processType 工序类型
     */
    processType: function (nodeData, processType) {

        nodeData.ShowRepeatImg = '/images/drawtools/Repeat.svg';
        nodeData.ShowCheckImg = '/images/drawtools/Check.svg';
        switch (processType) {
            case "Assembly":
                nodeData.ShowImg = '/images/drawtools/Assembly.svg';
                nodeData.ImgBackground = '#0D7CF5';
                break;
            case "Fix":
                nodeData.ShowImg = '/images/drawtools/Fix.svg';
                nodeData.ImgBackground = '#F43B5A';
                break;
            case "Pqc":
                nodeData.ShowImg = '/images/drawtools/Pqc.svg';
                nodeData.ImgBackground = '#23D89E';
                break;
            case "Packing":
                nodeData.ShowImg = '/images/drawtools/Packing.svg';
                nodeData.ImgBackground = '#21D4DB';
                break;
            case "Ageing":
                nodeData.ShowImg = '/images/drawtools/Pqc.svg';
                nodeData.ImgBackground = '#23D89E';
                break;
            case "Fqc":
                nodeData.ShowImg = '/images/drawtools/Fqc.svg';
                nodeData.ImgBackground = '#6D56E2';
                break;
            case "BatchAssembly":
                nodeData.ShowImg = '/images/drawtools/Assembly.svg';
                nodeData.ImgBackground = '#0D7CF5';
                break;
            case "BatchFix":
                nodeData.ShowImg = '/images/drawtools/Fix.svg';
                nodeData.ImgBackground = '#F43B5A';
                break;
            case "BatchPqc":
                nodeData.ShowImg = '/images/drawtools/Pqc.svg';
                nodeData.ImgBackground = '#23D89E';
                break;
            case "BatchPacking":
                nodeData.ShowImg = '/images/drawtools/Packing.svg';
                nodeData.ImgBackground = '#21D4DB';
                break;

            default:
                nodeData.ShowImg = '/images/drawtools/Default.svg';
                nodeData.ImgBackground = '#7C44FF';
                break;
        }
    },

    /**
     * 加载工序数据
     * @method loadDataloadData
     * @for SIE.Tech.ProcessTreeControl
     */
    loadData: function () {
        var me = this;
        var token = me.mainView.token;
        SIE.invokeDataQuery({
            type: "SIE.Web.Tech.Routings.TechDataQueryer",
            method: "GetProcessTreeInfos",
            token: token,
            params: [],
            success: function (res) {
                if (!res.Success)
                    return;
                var familyCategoryInfos = res.Result;
                me.bindProcessTree(familyCategoryInfos, me);
            }
        });
    },

    /**
     * 定位到节点
     * @method gotoNodeLocation
     * @for SIE.Tech.RoutingTreeControl
     * @param {Object} leafNode 叶子节点
     */
    gotoNodeLocation: function (leafNode) {
        var node = leafNode;
        var familyNode = node.parentNode;
        //由于刷新数据的时候会重置树的对象，原来记下来的node不能用来定位，需找到刷新后的
        var realFamilyNode = this.explandByFamilyNode(familyNode);
        var realNode = realFamilyNode.childNodes.first(function (p) { return p.data.Id === node.data.Id; });
        var treePanel = realFamilyNode.getOwnerTree();
        treePanel.getSelectionModel().select(realNode);
        var treeview = treePanel.getView();
        treeview.focusRow(realNode);
    },

    /**
     * 展开产品族
     * @param {Object} familyNode 产品族节点
     * @returns {Object} 产品族节点
     */
    explandByFamilyNode: function (familyNode) {
        var categoryNode = familyNode.parentNode;
        var realCategoryNode = this.getRootNode().childNodes.first(function (p) { return p.data.Id === categoryNode.data.Id; });
        realCategoryNode.expand();
        var realFamilyNode = realCategoryNode.childNodes.first(function (p) { return p.data.Id === familyNode.data.Id; });
        realFamilyNode.expand();
        return realFamilyNode;
    },

    /**
     * 绑定工序数据
     * @method bindProcessTree
     * @for SIE.Tech.ProcessTreeControl
     * @param {SIE.Web.Tech.Routings.FamilyCategoryInfo} familyCategoryInfos 工序节点信息
     * @param {treepanel} control 工序树控件
     */
    bindProcessTree: function (familyCategoryInfos, control) {
        var tree = { root: { children: [] } };
        children = tree.root.children;
        //遍历产品族分类
        familyCategoryInfos.forEach(function (category) {
            var jsonCategory = { Id: category.Id, Code: category.Code, Name: category.Name, text: category.Name + '(' + category.Code + ')', leaf: false, children: [] };
            //遍历产品族
            category.FamilyList.forEach(function (family) {
                var jsonFamily = { Id: family.Id, Code: family.Code, Name: family.Name, text: family.Name + '(' + family.Code + ')', leaf: false, children: [] };
                //遍历工序
                family.ProcessList.forEach(function (process) {
                    jsonFamily.children.push({
                        Id: process.Id,
                        text: Ext.String.format('[{0}.{3}]({1}){2}', process.IsBatch, process.Name, process.ReferenceTime, process.TypeDisplay),
                        processName: process.Name,
                        enableMoveInControl: process.EnableMoveInControl,
                        IsOutsourcing: process.IsOutsourcing,
                        //获取工序属性数据
                        CanChoose: process.CanChoose,
                        IsRepeat: process.IsRepeat,
                        IsCreateSku: process.IsCreateSku,
                        IsCalculate: process.IsCalculate,

                        IsGenerateTask: process.IsGenerateTask,
                        IsRequirementTask: process.IsRequirementTask,
                        IsBuckleMaterial: process.IsBuckleMaterial,
                        VictoryStandard_Display: process.VictoryStandard_Display,
                        VictoryStandardId: process.VictoryStandardId,

                        RepairVictoryId: process.RepairVictoryId,
                        RepairVictory_Display: process.RepairVictory_Display,
                        IsStricter: process.IsStricter,
                        Overtime: process.Overtime,

                        IsPassRate: process.IsPassRate,
                        IsBinding: process.IsBinding,
                        IsUnBinding: process.IsUnBinding,

                        MaxPassNum: process.MaxPassNum,
                        IsNextMoveIn: process.IsNextMoveIn,

                        leaf: true,
                        nodetype: 'RoutingNode',
                        Type: process.Type,
                        TransferType: process.TransferType,
                        parameterList: process.ParameterList
                    });
                });
                jsonCategory.children.push(jsonFamily);
            });
            children.push(jsonCategory);
        });
        var treeStore = Ext.create('Ext.data.TreeStore', tree);
        control.setStore(treeStore);
    }
});