Ext.define('SIE.Web.ESop.EngDocuments.Scripts.Common', {
    statics: {
        SetFileUseTypeTreeStore: function (folderId) {
            var tree = Ext.getCmp("FileUseTypeTreePanel");
            if (tree) {
                var view = tree.SieView;
                SIE.invokeDataQuery({
                    method: 'GetTreeDatas',
                    params: [folderId],
                    action: 'queryer',
                    type: 'SIE.Web.FMS.FileManageDataQueryer',
                    token: view.getToken(),
                    success: function (res) {

                        var dataStore = Ext.create('Ext.data.TreeStore', {
                            root: { expanded: true },
                            data: res.Result,
                        });
                        tree.setStore(dataStore);
                    }
                });
            }
        },
        SetEngDocTreeStore: function (folderId) {
            var tree = Ext.getCmp("EngDocTreePanel");
            if (tree) {
                var view = tree.SieView;
                SIE.invokeDataQuery({
                    method: 'GetTargetTreeDatas',
                    params: [folderId],
                    action: 'queryer',
                    type: 'SIE.Web.FMS.FileManageDataQueryer',
                    token: view.getToken(),
                    success: function (res) {

                        var dataStore = Ext.create('Ext.data.TreeStore', {
                            root: { expanded: true },
                            data: res.Result,
                        });
                        tree.setStore(dataStore);
                    }
                });
            }
        },
        GetFileUseTypePath: function (folderId) {
            var tree = Ext.getCmp("FileUseTypeTreePanel");
            if (tree) {
                var view = tree.SieView;
                var resData;
                var fileId;
                var filePath = "";
                
                SIE.invokeDataQuery({
                    method: 'GetFileManageDatasByFolder',
                    params: [folderId],
                    action: 'queryer',
                    type: 'SIE.Web.FMS.FileManageDataQueryer',
                    async: false,
                    token: view.getToken(),
                    success: function (res) {
                        resData = res.Result;
                        for (var i = 0; i < resData.navData.length; i++) {
                            fileId = resData.navData[i].FolderId;
                            filePath += resData.navData[i].Name;
                            if (i != resData.navData.length - 1) {
                                filePath += '/';
                            }
                        }
                    }
                });
                var fileData = {
                    FolderId: fileId,
                    FilePath: filePath,
                }
                return fileData;
            }
        },

        SetGridStore: function (folderId) {
            var gridControl = Ext.getCmp("EngDocGrid");
            if (gridControl) {
                var view = gridControl.SieView;
                SIE.invokeDataQuery({
                    method: 'GetReleaseFileManageDatasByFolder',
                    params: [folderId],
                    action: 'queryer',
                    type: 'SIE.Web.FMS.FileManageDataQueryer',
                    token: view.getToken(),
                    success: function (res) {
                        var resData = res.Result;
                        gridControl.initStore(resData.gridData);
                    }
                });
            }
        },

        GetUseTypeFolderId: function (token, useType) {
            var configFolderId = null;
            SIE.invokeDataQuery({
                method: 'GetConfigUseTypeFolderId',
                params: [useType],
                action: 'queryer',
                type: 'SIE.Web.ESop.EngDocuments.DataQueryers.EngDocDataQueryer',
                token: token,
                async: false,
                success: function (res) {
                    configFolderId = res.Result;
                }
            });
            return configFolderId;
        },

        GetRootFolders: function (token, folderId) {
            var rootNameList = "";
            SIE.invokeDataQuery({
                method: 'GetRootFolders',
                params: [folderId],
                action: 'queryer',
                type: 'SIE.Web.FMS.FileManageDataQueryer',
                token: token,
                async: false,
                success: function (res) {
                    var list = res.Result;
                    for (var i = 0; i < list.length; i++) {
                        rootNameList += list[i] + '/';
                    }
                }
            });
            return rootNameList;
        },

        GetDocumentType: function (token, exten, name) {
            var docExtData;
            SIE.invokeDataQuery({
                method: 'GetDocumentType',
                params: [exten, name],
                action: 'queryer',
                type: 'SIE.Web.ESop.EngDocuments.DataQueryers.EngDocDataQueryer',
                token: token,
                async: false,
                success: function (res) {
                    docExtData = res.Result;
                }
            });
            return docExtData;
        }
    }
    
})
