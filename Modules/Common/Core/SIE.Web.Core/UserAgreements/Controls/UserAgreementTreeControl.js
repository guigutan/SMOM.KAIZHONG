/**
 * 协议树控件
 * @class SIE.Tech.RoutingTreeControl
 * @constructor
 */
Ext.define('SIE.Core.UserAgreementTreeControl', {
    extend: 'Ext.tree.Panel',
    xtype: 'UserAgreementTree',

    border: 0,
    rootVisible: false,
    region: 'center',
    split: true,
    flex: 1,
    designControl: null, //显示区控件
    curShowRecord:null,//当前显示的协议
    /**
    * 父主视图
    * @property {ListLogicalView} mainView
    */
    mainView: null,
    tbar: {
        xtype: 'container',
        layout: 'anchor',
        defaults: { anchor: '0' },
        defaultType: 'toolbar',
        items: [{
            items: [
                {
                    xtype: 'button',
                    btnName: '刷新',
                    tooltipType: "title",
                    tooltip: '刷新用户协议'.L10N(),
                    style: 'width:35px;',
                    iconCls: "iconfont icon-Reload icon-blue",
                    listeners: {
                        click: {
                            fn: function () {
                                this.up('treepanel').mainView.CurRoutingVersion = null;
                                this.up('treepanel').loadData();
                                praseVersionAction.setDisabled(true);
                                addRoutingAndPraseVersionAction.setDisabled(true);
                                this.up('treepanel').designControl.clear(); //清空内容
                            }
                        }
                    }
                }, {
                    xtype: 'button',
                    disabled: true,
                    btnName: '定位',
                    tooltipType: "title",
                    tooltip: '定位用户协议版本'.L10N(),
                    style: 'width:35px;',
                    iconCls: "iconfont icon-ArrowLeftRight icon-blue",
                    listeners: {
                        click: {
                            fn: function () {
                                var me = this.up('treepanel');
                                me.gotoNodeLocation(me.curShowRecord);
                            }
                        }
                    }
                }, {
                    xtype: 'button',
                    disabled: true,
                    btnName: '新增',
                    tooltipType: "title",
                    tooltip: '新增用户协议版本'.L10N(),
                    style: 'width:35px;',
                    iconCls: "iconfont icon-AddEntity icon-green",
                    listeners: {
                        click: {
                            fn: function () {
                                var me = this;
                                var routingControl = me.up('treepanel');
                                var selects = routingControl.getSelection();
                                if (Ext.isEmpty(selects)) {
                                    return;
                                }
                                var record = selects[0];
                                var depth = record.get('depth');
                                var type = 0; //协议类型
                                switch (depth) {
                                    case 1:
                                        type = record.get('AgreementType');
                                        break;
                                    case 2:
                                        type = record.parentNode.get('AgreementType');
                                        break;
                                }
                                routingControl.uploadFile(type);
                            }
                        }
                    }
                }, {
                    xtype: 'button',
                    disabled: true,
                    btnName: '启用',
                    tooltipType: "title",
                    tooltip: '启用用户协议'.L10N(),
                    style: 'width:35px;',
                    iconCls: "iconfont icon-EditEntity icon-blue",
                    listeners: {
                        click: {
                            fn: function () {
                                var me = this;
                                var routingControl = me.up('treepanel');
                                var selects = routingControl.getSelection();
                                if (Ext.isEmpty(selects)) {
                                    return;
                                }
                                var record = selects[0];
                                routingControl.enableAgreement(record);
                            }
                        }
                    }
                }

            ]
        }]
    },
    viewConfig: {
        stripeRows: true,  //背景间隔色
    },

    listeners: {
        'itemclick': function (control, record, item, index) {
            var depth = record.get('depth');
            control.up('panel').setCommandState(depth, record);
        },
        'itemdblclick': function (control, record, item, index, e, eOpts) {
            var depth = record.get('depth');
            //双击版本才显示文件内容
            if (depth != 2)
                return;
            var treePanel = control.up('panel');
            if (treePanel) {
                treePanel.designControl.showAgreement(record); //显示文件内容
                treePanel.curShowRecord = record;
            }
        }
    },

    /**
     * 设置协议树控件命令状态
     * @method setCommandState
     * @for SIE.Tech.RoutingTreeControl
     * @param {int} depth 树深度，代表不同类型
     */
    setCommandState: function (depth, record) {
        var me = this;
        var buttons = me.getView().grid.query('button');
        var btnAdd = buttons.first(function (p) { return p.btnName === '新增'; });
        var btnPos = buttons.first(function (p) { return p.btnName === '定位'; });
        var btnEnable = buttons.first(function (p) { return p.btnName === '启用'; });
        switch (depth) {
            case 0:
                btnAdd.disable();
                btnEnable.disable();
                break;
            case 1:
                btnAdd.enable();
                btnEnable.disable();
                break;
            case 2:
                btnAdd.enable();
                if (record.data.IsUse)
                    btnEnable.disable();
                else
                    btnEnable.enable();
                break;

        }
        if (me.curShowRecord)
            btnPos.enable();
        else
            btnPos.disable();
    },

    /**
     * 加载用户协议数据
     * @method loadData
     * @for SIE.Tech.RoutingTreeControl
     * @param {Function} callback 回调函数
     */
    loadData: function (callback) {
        var me = this;
        var token = me.mainView.token;

        SIE.invokeDataQuery({
            action: 'queryer',
            type: 'SIE.Web.Core.UserAgreements.DataQueryers.UserAgreementDataQueryer',
            method: 'GetUserAgreements',
            token: token,
            success: function (res) {
                if (res.Result) {
                    var data = res.Result;
                    me.bindRoutingTree(me, data);
                    if (me.curShowRecord) {
                        me.gotoNodeLocation(me.curShowRecord)
                    }
                }
            }
        });
    },
    /**
     * 绑定数的数据
     * 后台数据格式：协议-协议版本
     * @method loadData
     * @for SIE.Tech.RoutingTreeControl
     * @param {Function} callback 回调函数
     */
    bindRoutingTree: function (control, data) {
        var tree = { root: { children: [] } };
        children = tree.root.children;
        var jsonCategoryService = { Id: 1, text: "服务协议", AgreementType: 0, leaf: false, children: [] };
        var jsonCategoryPrivacy = { Id: 2, text: "隐私协议", AgreementType: 1, leaf: false, children: [] };

        //添加到树
        data.each(function (agree) {
            var agreeNode = {
                Id: agree.getId(),
                text: agree.getVersionNoDisplay(),
                AgreementType: agree.getAgreementType(),
                leaf: true,
                IsUse: agree.getIsUse()
            };
            if (agree.getIsUse())
                agreeNode.iconCls = "iconfont icon-Check";
            if (agree.getAgreementType() == 0) {
                jsonCategoryService.children.push(agreeNode);
            }
            else {
                jsonCategoryPrivacy.children.push(agreeNode);
            }
        });

        children.push(jsonCategoryService);
        children.push(jsonCategoryPrivacy);

        var treeStore = Ext.create('Ext.data.TreeStore', tree);
        control.setStore(treeStore);
    },
    /**
      * 定位到节点
      * @method gotoNodeLocation
      * @for SIE.Tech.RoutingTreeControl
      * @param {Object} versionNode 协议版本
      */
    gotoNodeLocation: function (versionNode) {
        var treeControl = this;
        var typeNode = versionNode.parentNode;
        //由于刷新数据的时候会重置树的对象，原来记下来的node不能用来定位，需找到刷新后的
        var realTypeNode = treeControl.getRootNode().childNodes.first(function (p) { return p.data.Id === typeNode.data.Id; });
        realTypeNode.expand();
        var realNode = realTypeNode.childNodes.first(function (p) { return p.data.Id === versionNode.data.Id; });

        var treePanel = realTypeNode.getOwnerTree();
        treePanel.getSelectionModel().select(realNode);
        var treeview = treePanel.getView();
        treeview.focusRow(realNode);

        this.curShowRecord = realNode;
    },

    /**
     * 上传协议
     * @param {any} type
     */
    uploadFile: function (type) {
        var me = this;
        var view = me.mainView;
        var cmd = Ext.create('SIE.Web.Core.UserAgreements.Commands.UploadAgreementCommand', { view: view, agreementType: type });
        cmd.execute(view);
    },

    /**
     * 启用协议
     * @param {any} record
     */
    enableAgreement: function (record) {
        var me = this;
        var view = me.mainView;

        var indata = {};
        indata.Type = view.model;
        indata.Data = Ext.encode(record.data.Id);
        var commandInfo = {
            entityType: view.model,
            //parentType: view.model,
            moduleName: "用户协议管理",
            childModuleName: "",
            commandName: "启用",
        };
        SIE.invokeCommand({
            token: view.getToken(),
            cmd: "SIE.Web.Core.UserAgreements.Commands.EnableAgreementCommand",
            data: indata,
            logInfo: commandInfo,
            callback: function (res) {
                if (res.Success) {
                    var control = view.routingControl;
                    control.loadData(); //刷新数据
                }
                else {
                    SIE.Msg.showError(res.Message);
                }
            }
        });
    }
});
