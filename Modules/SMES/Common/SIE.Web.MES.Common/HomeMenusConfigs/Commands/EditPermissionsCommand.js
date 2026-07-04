SIE.defineCommand('SIE.Web.MES.Common.HomeMenusConfigs.Commands.EditPermissionsCommand', {
    meta: { text: "编辑菜单", group: "edit", iconCls: "iconfont icon-Phone icon-blue" },
    _menus: [],
    _curPositionItem: null,//当前操作的元素
    _menuPanel: null,

    /**
     * 命令可用性验证
     * @param {any} view
     */
    canExecute: function (view) {
        var p = view.getCurrent();
        if (p == null) { return false; }
        if (p.isNew()) { return false; }
        if (view.getSelectionIds().length > 1)
            return false;
        return true;
    },

    /**
     *  执行命令
     * @param {any} listView
     * @param {any} source
     */
    execute: function (listView, source) {
        var me = this;
        me._curPositionItem = listView.getCurrent();
        me._initData(source);
    },

    /**
     * 查询当前角色的权限树型
     * @param {any} source
     */
    _initData: function (source) {
        var me = this;
        var roleId = me._curPositionItem.get("RoleId");
        var userId = me._curPositionItem.get("UserId");
        SIE.invokeDataQuery({
            type: "SIE.Web.MES.Common.HomeMenusConfigs.HomeMenusDataQueryer",
            method: "GetAllMenus",
            params: [roleId, userId, me._curPositionItem.data.Id],
            async: false,
            token: me.view.token,
            callback: function (res) {
                if (res.Success == true) {
                me._menus = res.Result;
                var panel = me._initModuleMenu(source);
                    me._popupWin(panel, source);
                }

            },
        });
    },

    /**
     * 构造权限面板
     * @param {any} source
     */
    _initModuleMenu: function (source) {
        var me = this;
        //创建菜单数据源
        var menusStore = Ext.create('Ext.data.TreeStore', {
            model: 'SIE.rbac.AppMenuModel',
            data: me._menus,
            proxy: {
                type: 'memory',
                reader: {
                    type: 'json'
                }
            },
            root: {
                Id: 0.0,
                leaf: false,
                text: "root",
                expanded: true
            },
            parentIdProperty: "TreePId",
            listeners: {
                load: function (tStore, records, successful, operation, node, eOpts) {
                    node.expand(true);
                }
            }
        });

        me._menuPanel = Ext.create('Ext.ux.grid.TriStateTree', {
            checkPropagation: 'both',
            tbar: [
                {
                    id: 'menu_searchTxt1',
                    name: 'menu_searchTxt1',
                    emptyText: '菜单名'.L10N(),
                    xtype: 'textfield',
                    width: 200,
                    listeners: {
                        specialKey: function (field, e) {
                            if (e.getKey() == Ext.EventObject.ENTER) {
                                me._searchMenu();
                            }
                        }
                    }
                },
                {
                    id: 'menu_searchBtn',
                    name: 'menu_searchBtn',
                    text: '查找'.L10N(),
                    xtype: 'button',
                    handler: function () {
                        me._searchMenu();
                    }
                }
            ],
            store: menusStore,
            rootVisible: false,
            canUpdateLeafNode: true,
        });

        //控件布局
        var con = Ext.widget('container', {
            layout: 'border',
            items: [{
                region: 'center',
                border: 0,
                split: true,
                layout: 'fit',
                items: me._menuPanel
            }]
        });

        return con;
    },

    /**
     * //弹窗
     * @param {any} ui 
     * @param {any} source
     */
    _popupWin: function (ui, source) {
        var me = this;
        var win = SIE.Window.show({
            title: '菜单编辑'.L10N(),
            items: ui,
            width: 350,
            height: 500,
            modal: true,
            callback: function (btn) {
                if (btn.t() === '确定'.L10N()) {
                    Ext.MessageBox.show({
                        msg: '保存数据中, 请稍等...'.L10N(),
                        progressText: '保存中...'.L10N(),
                        width: 300,
                        closable: false,
                        wait: {
                            interval: 200
                        }
                    });
                    win.hide();
                    me._save(win);
                    return false;
                }
            }
        });

    },

    /**
     * 保存权限
     * @param {any} win
     */
    _save: function (win) {
        var me = this;
        var checkedCmds = [];
        var nocheckedCmds = [];
        var roleId = me._curPositionItem.get("RoleId");
        var treePanel = me._menuPanel;
        treePanel.getRootNode().cascade(function (n) {
            var rec = treePanel.getView().getRecord(n);
            var checked = rec.get('checked');
            var moduleKey = rec.get('ModuleKey');
            var scopeKey = rec.get('ScopeKey');
            var treePid = rec.get('TreePId');
            if (checked && treePid != 0) {
                checkedCmds.push({
                    "ModuleKey": moduleKey,
                    "ScopeKey": scopeKey,
                    "OperationKey": ""
                });
            } else {
                nocheckedCmds.push({
                    "ModuleKey": moduleKey,
                    "ScopeKey": scopeKey,
                    "OperationKey": ""
                });
            }
        });
        var indata = {
            RecordId: me._curPositionItem.data.Id,
            CheckedCmds: checkedCmds,
            NoCheckedCmds: nocheckedCmds
        };

        me.view.execute({
            data: indata,
            success: function (res) {
                Ext.MessageBox.close();
                me._showToast('保存成功！'.L10N(), '完成'.L10N());
                win.close();
            },
            error: function (res) {
                win.show();
            }
        }, me._ownerView);
    },

    /**
     * 提示框
     * @param {any} s
     * @param {any} title
     */
    _showToast: function (s, title) {
        Ext.toast({
            html: s,
            closable: false,
            align: 't',
            slideInDuration: 400
        });
    },

    /**
     * 树型Store过滤
     * */
    _searchMenu: function () {
        var me = this;
        var seachTxt = me._menuPanel.queryById("menu_searchTxt1");
        me._menuPanel.filterByText(seachTxt.value);

    }
});

//树型实体
Ext.define('SIE.rbac.AppMenuModel', {
    extend: 'Ext.data.TreeModel',
    alias: 'model.AppMenuModel',
    idProperty: 'Id',
    fields: [
        { name: 'Id', type: 'float' },
        { name: 'TreePId', type: 'float' },
        { name: 'text', type: 'string' },
        { name: 'checked', type: 'boolean', defaultValue: false },
        { name: 'ScopeKey', type: 'string' },
        { name: 'OperationKey', type: 'string' },
        { name: 'ModuleKey', type: 'string' },
    ]
});