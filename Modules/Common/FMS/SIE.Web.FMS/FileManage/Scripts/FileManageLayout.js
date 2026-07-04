Ext.define('SIE.Web.FMS.FileManageLayout', {
    extend: 'SIE.autoUI.layouts.Common',
    xtype: 'FileManageLayout',
    view: null,
    control: null,
    gridId: 'fileManage-id',
    _layoutChildren: function (regions) {
        var me = this;
        var main = regions.main;
        var mainControl = main.getControl();
        me.control = mainControl;
        me.view = main.getView();
        me.view.mon(me.control, 'closewin', function (evtArgs) {
            me._viewClose(evtArgs.tab, evtArgs.control);
        });
        var toolbar = mainControl.getDockedItems()[0];
        var treeCtl = me.initTreeControl(me.view);
        var mainCtl = me.initGirdControl(me.view);
        me.initMenu(toolbar, me.view);
        me.view.CurFolderId = null;
        me.view.GridControl = mainCtl;
        me.view.TreeControl = treeCtl;
        me.view.owner = me;
        me.initCss();
        return Ext.widget('container', {
            height: '100%',
            layout: {
                type: 'border',
                align: 'stretch'
            },
            defaults: {
                frame: true,//底色变化
                bodyPadding: 0
            },
            items: [{
                height: '100%',
                flex: 4,
                region: 'center',
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },
                baseCls: 'my-panel-no-border',
                defaults: {
                    frame: true,//底色变化
                    bodyPadding: 0
                },
                items: [{
                    baseCls: 'my-panel-no-border',
                    items: toolbar  //命令栏
                }, {
                    xtype: "container",
                    baseCls: 'my-panel-no-border',
                    height: 35,
                    style: ';background-color:white;',
                    html: "<div style='width:100%; line-height:35px; height:100%;padding-left:10px;'><div style='float:left;'><a href='javascript:SIE.Web.FMS.FileManages.CommonFunctions.SetGridStore();'  style='text-decoration:none;'>" + '首页'.t() + "</a> / </div>" +
                        "<div style='float:left;' id='topNav'><a href='javascript:alert(1)' style='text-decoration:none;'>工艺文件</a> / <span>CNC加工中心</span> /</div> </div>"
                }, mainCtl
                ]
            },
                treeCtl
            ],
            listeners: {
                afterRender: function (comp) {
                    window.onresize = function () {
                        var height = window.innerHeight;
                        if (window.innerHeight < 100)
                            height = 100;
                        Ext.getCmp("fileManage-id").height = height - 74;
                    }
                }
            },
        });
    },
    initTreeControl: function (view) {
        return {
            xtype: 'treepanel',
            region: 'east',
            style: 'border-width:0;',
            flex: 1,
            rootVisible: false,
            id: 'fileManageTree',
            SieView: view,
            title: '文件夹'.t(),
            collapsible: true,
            split: true,
            useArrows: true,
            width: 200,
            listeners: {
                afterrender: function (comp) {
                    SIE.Web.FMS.FileManages.CommonFunctions.SetTreeStore();
                    //取消双击展开收起
                    this.view.toggleOnDblClick = false;
                },
                beforeitemexpand: function (node, index, item, eOpts) {
                    node.data.iconCls = 'iconfont icon-OpenFile icon-blue';
                },
                beforeitemcollapse: function (node, index, item, eOpts) {
                    node.data.iconCls = 'iconfont icon-Folder icon-blue';
                },
                afteritemexpand: function (item, index) {
                    var folderId = item.data.folderId;
                    if (item.childNodes.length == 0) {
                        SIE.invokeDataQuery({
                            method: 'GetTreeDatas',
                            params: [folderId],
                            action: 'queryer',
                            type: 'SIE.Web.FMS.FileManageDataQueryer',
                            token: view.getToken(),
                            success: function (res) {
                                item.appendChild(res.Result);
                            }
                        });
                    }
                },
                itemdblclick: function (v, record, item, index, e, opts) {
                    SIE.Web.FMS.FileManages.CommonFunctions.SetGridStore(record.data.folderId);
                }
            }
        }
    },
    //初始化表格控件
    initGirdControl: function (view) {
        var me = this;
        var x = window.innerHeight - 74;
        return {
            region: 'center',
            id: me.gridId,
            xtype: 'grid',
            width: '100%',
            SieView: view,
            height: x,
            frame: true,
            header: false,
            columnLines: false,
            style: 'border-width:0;',
            iconCls: 'my-panel-no-border icon-grid',
            //selType: 'cellmodel',
            selType: 'checkboxmodel',
            plugins: {
                cellediting: {
                    dbClicksToEdit: 1
                }
            },
            columns: [
                //{
                //    xtype: 'checkcolumn',
                //    header: '',
                //    dataIndex: 'checkFile',
                //    headerCheckbox: true,
                //    width: 30,
                //    menuDisabled: true,
                //    sortable: false,
                //    listerers: {
                //        onchange: function () {

                //        }
                //    }
                //    //renderer: function (v, p, record) {
                //    //    if (record.data.IsFile) {
                //    //        return (new Ext.grid.column.CheckColumn).renderer(v);
                //    //    }
                //    //    else
                //    //        return '';
                //    //}
                //},
                {
                    text: "文件名".t(),
                    dataIndex: 'FileName',
                    flex: 1,
                    //editor: {
                    //    allowBlank: false
                    //},
                    renderer: function (v, p, record) {
                        if (record.data.IsFile) {
                            var newcss = "";
                            if (record.data.IsNew) { newcss = "color:orange;" }
                            return "<div style='width:100%; overflow:hidden;display:flex;'><div class='iconfont icon-FileOutline' style='float:left;'></div><div style='float:left; margin-left:5px;" + newcss + "'>" + record.data.FileName + "<div><div>";
                        }
                        else
                            return "<div style='width:100%;overflow:hidden;display:flex;'><div class='iconfont icon-Folder icon-blue' style='float:left;'></div><div style='float:left; margin-left:5px;'>" + record.data.FileName + "<div><div>";
                    }
                }, {
                    text: "文件编码".t(),
                    dataIndex: 'Code',
                    align: 'center',
                    width: 100,
                }, {
                    text: "版本".t(),
                    dataIndex: 'Version',
                    align: 'center',
                    width: 80,
                }, {
                    text: "状态".t(),
                    dataIndex: 'FileState_Display',
                    align: 'center',
                    width: 100,
                }, {
                    text: "大小".t(),
                    width: 80,
                    align: 'center',
                    dataIndex: 'Size'
                }, {
                    text: "上传人".t(),
                    width: 80,
                    align: 'center',
                    dataIndex: 'CreatebyName'
                }, {
                    text: "上传时间".t(),
                    width: 150,
                    align: 'center',
                    dataIndex: 'CreateDate',

                }, {
                    text: "操作".t(),
                    xtype: 'actioncolumn',
                    width: 60,
                    menuDisabled: true,
                    sortable: false,
                    align: 'center',
                    items: [{
                        iconCls: 'iconfont icon-DeleteEntity icon-red',
                        tooltip: '删除'.t(),
                        handler: function (grid, rowIndex, colIndex) {
                            var rec = grid.getStore().getAt(rowIndex);
                            if (rec.data.IsFile) {
                                SIE.Web.FMS.FileManages.CommonFunctions.DeleteFile(rec.data.FId);
                            }
                            else {
                                if (rec.data.HasChild) {
                                    return;
                                }
                                else
                                    SIE.Web.FMS.FileManages.CommonFunctions.DeleteFolder(rec.data.FId);
                            }
                        },
                        getClass: function (v, meta, rec) {
                            if (rec.data.HasChild) {
                                return "iconfont icon-DeleteEntity icon-gray";
                            }
                            else
                                return "iconfont icon-DeleteEntity icon-red";
                        },
                    }, {
                        tooltip: '修改名称'.t(),
                        handler: function (grid, rowIndex, colIndex) {
                            var rec = grid.getStore().getAt(rowIndex);
                            if (!rec.data.IsFile) {
                                SIE.Web.FMS.FileManages.CommonFunctions.EditFolderName(rec.data.FId, rec.data.FileName);
                            }
                        },
                        getClass: function (v, meta, rec) {
                            if (rec.data.IsFile) {
                                return "hideItem";
                            }
                            else
                                return "iconfont icon-Edit icon-blue iconMargin";
                        },
                    }],
                    listeners: {
                        render: function (comp) {
                            //加了这个会里面的按钮tooltip才生效
                            Ext.QuickTips.init();
                            Ext.QuickTips.register({
                                target: comp.id,
                                text: "操作".t()
                            });
                        }
                    }
                }],
            listeners: {
                celldblclick: function (g, row, col, record, tr, rowindex) {
                    var s = record.data;
                    if (!s.IsFile && s.FId > 0) {
                        SIE.Web.FMS.FileManages.CommonFunctions.SetGridStore(s.FId);
                    }
                },
                selectionchange: function (control, selected, eOpts) {
                    SIE.Web.FMS.FileManages.CommonFunctions._funcCmdState(control);
                },
                afterRender: function (comp) {
                    SIE.Web.FMS.FileManages.CommonFunctions.SetGridStore();
                    SIE.Web.FMS.FileManages.CommonFunctions.SetAdminPermission(view);
                }
            },

            //查询调用，动态更改store
            initStore: function (queryData) {
                var cont = Ext.getCmp(me.gridId);
                var empty = cont.getStore().isEmptyStore
                if (empty) {
                    var taksStore = Ext.create('Ext.data.Store', {
                        data: queryData,
                    });
                    cont.setStore(taksStore);
                }
                else {
                    cont.getStore().setData(queryData);
                }
            },
        }
    },
    initMenu: function (toolbar, view) {
        var file = new Ext.menu.Menu({
            shadow: "drop",
            allowOtherMenus: true,
            items: [
                new Ext.menu.Item({
                    text: "新建".t(),
                    iconCls: "iconfont icon-AddEntity icon-green",
                    height: 30,
                    id: 'FileMenuItem_AddNewFolder',
                    handler: function () {
                        SIE.Web.FMS.FileManages.CommonFunctions.AddNewFolder();
                    }
                }),
                new Ext.menu.Item({
                    text: "上传".t(),
                    iconCls: "iconfont icon-ExportData icon-green",
                    height: 30,
                    id: 'FileMenuItem_Upload',
                    handler: function () {
                        SIE.Web.FMS.FileManages.CommonFunctions.UploadFiles();
                    }
                }),
                new Ext.menu.Item({
                    text: "下载".t(),
                    iconCls: "iconfont icon-FileTree icon-blue",
                    height: 30,
                    id: 'FileMenuItem_Download',
                    handler: function () {
                        SIE.Web.FMS.FileManages.CommonFunctions.DownloadFile();
                    }
                }),
                new Ext.menu.Item({
                    text: "批量上传".t(),
                    iconCls: "iconfont icon-Upgrade icon-blue",
                    height: 30,
                    id: 'FileMenuItem_MultiUploadFiles',
                    handler: function () {
                        SIE.Web.FMS.FileManages.CommonFunctions.MultiUploadFiles();
                    }
                }),
                new Ext.menu.Separator(),
                new Ext.menu.Item({
                    text: "预览".t(),
                    iconCls: "iconfont icon-PageSearch icon-blue",
                    height: 30,
                    id: 'FileMenuItem_Preview',
                    handler: function () { SIE.Web.FMS.FileManages.CommonFunctions.DownloadFile(); }
                }),
                new Ext.menu.Item({
                    text: "修订".t(),
                    iconCls: "iconfont icon-EditEntity icon-blue",
                    height: 30,
                    id: 'FileMenuItem_Modify',
                    handler: function () { SIE.Web.FMS.FileManages.CommonFunctions.EditFiles(); }
                }),
                new Ext.menu.Item({
                    text: "发布".t(),
                    iconCls: "iconfont icon-ArrowWithCircleRight icon-blue",
                    height: 30,
                    id: 'FileMenuItem_Publish',
                    handler: function () { SIE.Web.FMS.FileManages.CommonFunctions.PublishFiles(); }
                }),
                new Ext.menu.Item({
                    text: "作废".t(),
                    iconCls: "iconfont icon-PageDelete icon-red",
                    height: 30,
                    id: 'FileMenuItem_Scrap',
                    handler: function () {
                        SIE.Web.FMS.FileManages.CommonFunctions.ScarpFiles();
                    }
                }),
                new Ext.menu.Separator(),
                {
                    text: "权限管理".t(),
                    iconCls: "iconfont icon-CalendarText icon-blue",
                    height: 30,
                    menu: new Ext.menu.Menu({
                        ignoreParentClicks: true,
                        items: [{
                            text: "文件授权".t(),
                            iconCls: "iconfont icon-CalendarText icon-blue",
                            id: 'FileMenuItem_Authorization',
                            handler: function () { SIE.Web.FMS.FileManages.CommonFunctions.FileAuthorization(false); }
                        }, {
                            text: "查看权限".t(),
                            iconCls: "iconfont icon-CalendarText icon-blue",
                            handler: function () { SIE.Web.FMS.FileManages.CommonFunctions.FileAuthorization(true); }
                        }]
                    })
                },
                {
                    text: "版本管理".t(),
                    iconCls: "iconfont icon-ContentCopy icon-blue",
                    height: 30,
                    menu: new Ext.menu.Menu({
                        ignoreParentClicks: true,
                        items: [{
                            text: "历史版本".t(),
                            iconCls: "iconfont icon-ContentCopy icon-blue",
                            handler: function () { SIE.Web.FMS.FileManages.CommonFunctions.LookHistoryVersion(); }
                        }, {
                            text: "操作记录".t(),
                            iconCls: "iconfont icon-ContentCopy icon-blue",
                            handler: function () { SIE.Web.FMS.FileManages.CommonFunctions.LookFileLog(); }
                        }]
                    })
                },
                new Ext.menu.Item({
                    text: "设置".t(),
                    iconCls: "iconfont icon-ConfigItem icon-blue",
                    height: 30,
                    id: 'FileMenuItem_Setting',
                    handler: function () {
                        SIE.Web.FMS.FileManages.CommonFunctions.ShowSetting();
                    }
                }),
            ]
        });
        toolbar.insert(0, {
            text: "菜单".t(),
            iconCls: "iconfont icon-Menu icon-blue",
            menu: file
        });
    },
    initCss: function () {
        Ext.util.CSS.createStyleSheet('#fileManage-id .x-grid-td{overflow: hidden;border-width: 0;vertical-align: top;height: 30px;line-height: 30px;font-size: 14px;}');
        Ext.util.CSS.createStyleSheet('#fileManage-id .x-grid-checkcolumn-cell-inner{padding: 12px 4px 4px 4px}');
        Ext.util.CSS.createStyleSheet('#fileManage-id .x-grid-dirty-cell{background:none;}');
        Ext.util.CSS.createStyleSheet('#fileManage-id .x-grid-cell-inner-action-col{padding: 12px 4px 4px 4px}');
        Ext.util.CSS.createStyleSheet('#fileManage-id .hideItem{display:none;}');
        Ext.util.CSS.createStyleSheet('#fileManage-id .iconMargin{margin:0 0 0 10px}');
    }
});









