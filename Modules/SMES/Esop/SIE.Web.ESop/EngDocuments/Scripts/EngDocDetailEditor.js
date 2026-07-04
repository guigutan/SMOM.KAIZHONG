Ext.define('SIE.Web.ESop.EngDocuments.Scripts.EngDocDetailEditor', {
    extend: 'Ext.util.Observable',
    constructor: function () {
        this.callParent(arguments);
    },
    onClick: function (field, trigger, e) {
        var me = this;
        var token = field.up().up().up().SIEView.token;
        var current = field.up().up().up().SIEView.getCurrent();
        var useType = current.getUseType();
        if (useType.length === 0) {
            Ext.Msg.show({
                title: '提示'.t(),
                message: '请先维护文件使用类型'.t(),
                buttons: Ext.Msg.OK,
                icon: Ext.Msg.INFO
            });
            return;
        }
        var configFolderId = SIE.Web.ESop.EngDocuments.Scripts.Common.GetUseTypeFolderId(token, useType);
        if (configFolderId === null) {
            Ext.Msg.show({
                title: '提示'.t(),
                message: '未维护使用类型对应的文件夹'.t(),
                buttons: Ext.Msg.OK,
                icon: Ext.Msg.INFO
            });
            return;
        }
        // 创建树形结构的面板
        var treePanel = Ext.create(
            {
                xtype: 'treepanel',
                rootVisible: false,
                id: 'EngDocTreePanel',
                SieView: field.up().up().up().SIEView,
                reserveScrollbar: true,
                rootVisible: false,
                multiSelect: true,
                singleExpand: true,
                region: 'center',
                flex: 1,
                listeners: {
                    afterrender: function (comp) {
                        SIE.Web.ESop.EngDocuments.Scripts.Common.SetEngDocTreeStore(configFolderId); 
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
                                token: token,
                                success: function (res) {
                                    item.appendChild(res.Result);
                                }
                            });
                        }
                    },
                    itemdblclick: function (v, record, item, index, e, opts) {
                        SIE.Web.ESop.EngDocuments.Scripts.Common.SetGridStore(record.data.folderId);
                    }
                }
            }
        );

        var fileGrid = {
            region: 'center',
            id: "EngDocGrid",
            xtype: 'grid',
            SieView: field.up().up().up().SIEView,
            frame: true,
            header: false,
            columnLines: false,
            flex:4,
            style: 'border-width:0;',
            iconCls: 'my-panel-no-border icon-grid',
            selModel: {
                type: 'checkboxmodel',
                mode: 'SINGLE' // 单选模式
            },
            plugins: {
                cellediting: {
                    dbClicksToEdit: 1
                }
            },
            columns: [
                {
                    text: "文件名".t(),
                    dataIndex: 'FileName',
                    flex: 1,
                    renderer: function (v, p, record) {
                        if (record.data.IsFile) {
                            var newcss = "";
                            if (record.data.IsNew) { newcss = "color:orange;" }
                            return "<div style='width:100%; overflow:hidden;'><div class='iconfont icon-FileOutline' style='float:left;'></div><div style='float:left; margin-left:5px;" + newcss + "'>" + record.data.FileName + "<div><div>";
                        }
                        else
                            return "<div style='width:100%;overflow:hidden;'><div class='iconfont icon-Folder icon-blue' style='float:left;'></div><div style='float:left; margin-left:5px;'>" + record.data.FileName + "<div><div>";
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

                },
            ],
            listeners: {
                celldblclick: function (g, row, col, record, tr, rowindex) {
                    var s = record.data;
                    if (!s.IsFile && s.FId > 0) {
                        SIE.Web.ESop.EngDocuments.Scripts.Common.SetGridStore(s.FId);
                    }
                },
                selectionchange: function (control, selected, eOpts) {
                },
                afterRender: function (comp) {
                    SIE.Web.ESop.EngDocuments.Scripts.Common.SetGridStore(configFolderId);
                }
            },
            initStore: function (queryData) {
                var cont = Ext.getCmp("EngDocGrid");
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
        var panel = Ext.create({
            xtype: 'panel',
            layout: {
                type: 'hbox',
                align: 'stretch'
            },
            items: [treePanel, fileGrid]
        });
        // 创建弹框
        var window = Ext.create('Ext.window.Window', {
            title: '选择文件'.t(),
            width: 1400,
            height: 500,
            layout: 'fit',
            modal: true,
            isAutoClose: false,
            items: panel,
            buttons: [{
                text: '确定'.t(),
                handler: function () {
                    // 选择的文件
                    var selectedNode = Ext.getCmp("EngDocGrid").getSelectionModel().getSelection()[0];
                    var view = Ext.getCmp("EngDocGrid").SIEView;
                    if (selectedNode) {
                        var fileData = selectedNode.data;
                        var fileName = fileData.FileName;
                        var fileExten = "." + fileData.ServerFileName.split('.')[1];
                        var filePath = "";
                        if (!fileData.IsFile) {
                            Ext.Msg.show({
                                title: '提示'.t(),
                                message: '请选择文件'.t(),
                                buttons: Ext.Msg.OK,
                                icon: Ext.Msg.INFO
                            });
                        }
                        else {
                            var pathDisplay = SIE.Web.ESop.EngDocuments.Scripts.Common.GetRootFolders(token, fileData.FId);
                            var docExtData = SIE.Web.ESop.EngDocuments.Scripts.Common.GetDocumentType(token, fileExten, fileName);
                            var docType = docExtData.DocumentType;
                            var md5 = docExtData.MD5;
                            current.setSavePath(fileData.FilePath);
                            current.setFileSize(fileData.Size);
                            current.setSavePathDisplay(pathDisplay + fileName);
                            current.setFId(fileData.FId);
                            current.setServerFileName(fileData.ServerFileName);    
                            current.setDocumentType(docType);    
                            current.setMD5(md5);
                        }
                    }
                    window.close();
                }
            }, {
                text: '取消'.t(),
                handler: function () {
                    window.close();
                }
            }]
        });

        // 显示弹框
        window.show();
    }

})
