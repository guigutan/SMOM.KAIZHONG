Ext.define('SIE.Web.ESop.EngDocuments.Scripts.FileUseTypeSelDocEditor', {
    extend: 'Ext.util.Observable',
    constructor: function () {
        this.callParent(arguments);
    },
    onClick: function (field, trigger, e) {
        var me = this;
        var token = field.up().up().up().SIEView.token;
        var current = field.up().up().up().SIEView.getCurrent();
        // 创建树形结构的面板
        var treePanel = Ext.create(
            {
                xtype: 'treepanel',
                rootVisible: false,
                id: 'FileUseTypeTreePanel',
                SieView: field.up().up().up().SIEView,
                reserveScrollbar: true,
                rootVisible: false,
                multiSelect: true,
                singleExpand: true,
                region: 'center',
                listeners: {
                    afterrender: function (comp) {
                        SIE.Web.ESop.EngDocuments.Scripts.Common.SetFileUseTypeTreeStore();
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
                        
                    }
                }
            }
        );

        // 创建弹框
        var window = Ext.create('Ext.window.Window', {
            title: '文件夹'.t(),
            width: 500,
            height: 300,
            layout: 'fit',
            modal: true,
            isAutoClose: false,
            items: [treePanel],
            buttons: [{
                text: '确定'.t(),
                handler: function () {
                    var selectedNode = treePanel.getSelectionModel().getSelection()[0];
                    if (selectedNode) {
                        var fileData = SIE.Web.ESop.EngDocuments.Scripts.Common.GetFileUseTypePath(selectedNode.data.folderId);
                        if (fileData === null || fileData.FilePath.length === 0 || fileData.FolderId === null) {
                            Ext.Msg.show({
                                title: '提示'.t(),
                                message: '请选择文件夹'.t(),
                                buttons: Ext.Msg.OK,
                                icon: Ext.Msg.INFO
                            });
                        }
                        current.setFile(fileData.FilePath);
                        current.setFolderId(fileData.FolderId);
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
    },
    
});
