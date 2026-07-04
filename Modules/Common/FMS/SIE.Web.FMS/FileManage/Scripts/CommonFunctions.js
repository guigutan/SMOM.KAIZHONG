Ext.define('SIE.Web.FMS.FileManages.CommonFunctions', {
    statics: {
        /**
         * 文件管理文件夹内容数据
         * @param {any} view 主视图
         * @param {any} folderId 文件夹Id
         */
        SetGridStore: function (folderId, newFolderIds) {
            var me = this;
            if (folderId == undefined || folderId == null || folderId == 0)
                document.getElementById("topNav").innerHTML = "";
            var gridControl = Ext.getCmp("fileManage-id");
            if (gridControl) {
                var view = gridControl.SieView;
                view.CurFolderId = folderId;
                //设置界面命令权限
                me.SetCommandsPermission(view);
                me._funcCmdState(view);
                //跟新按钮状态
                SIE.invokeDataQuery({
                    method: 'GetFileManageDatasByFolder',
                    params: [folderId],
                    action: 'queryer',
                    type: 'SIE.Web.FMS.FileManageDataQueryer',
                    token: view.getToken(),
                    success: function (res) {
                        var resData = res.Result;
                        if (newFolderIds && newFolderIds.length > 0) {
                            resData.gridData.where(function (p) { return p.IsFile && newFolderIds.contains(p.FId); }).forEach(function (p) {
                                p.IsNew = true;
                            });
                        }
                        gridControl.initStore(resData.gridData);
                        if (folderId > 0) {
                            var navData = resData.navData;
                            var htm = "";
                            navData.forEach(function (p) {
                                if (p.FolderId == folderId)
                                    htm += "<span>" + p.Name + "</span> /";
                                else
                                    htm += "<a href='javascript:SIE.Web.FMS.FileManages.CommonFunctions.SetGridStore(" + p.FolderId + ");' style='text-decoration:none;'>" + p.Name + "</a> / ";
                            });
                            document.getElementById("topNav").innerHTML = htm;
                        }
                    }
                });
            }
        },
         /**grid选中的行*/
        GetGridSelected: function (gridControl) {
            return gridControl.getSelectionModel().getSelection();
        },
        /**
         * 设置树形数据
         * @param {any} view
         */
        SetTreeStore: function (folderId) {
            var tree = Ext.getCmp("fileManageTree");
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
        /**添加新的文件夹 */
        AddNewFolder: function () {
            var gridControl = Ext.getCmp("fileManage-id");
            if (gridControl) {
                var view = gridControl.SieView;
                //当前文件夹Id
                var folderId = view.CurFolderId;
                SIE.Web.FMS.FileManages.CommonFunctions.OperateFolder("新建文件夹".t(), "", function (name, win) {
                    SIE.invokeDataQuery({
                        method: 'AddNewFolder',
                        params: [folderId, name],
                        action: 'queryer',
                        type: 'SIE.Web.FMS.FileManageDataQueryer',
                        token: view.getToken(),
                        success: function (res) {
                            var data = res.Result;
                            if (data.errMsg == "") {
                                //刷新数据
                                SIE.Web.FMS.FileManages.CommonFunctions.SetGridStore(folderId);
                                //给树增加节点
                                SIE.Web.FMS.FileManages.CommonFunctions.AddToTreeNode(data.Id, data.Name, folderId);
                                win.close();
                            }
                            else {
                                SIE.Msg.showError(data.errMsg);
                            }
                        }
                    });
                })
            }
        },

        EditFolderName: function (folderId, oldName) {
            var gridControl = Ext.getCmp("fileManage-id");
            var view = gridControl.SieView;
            //当前文件夹Id
            var curfolderId = view.CurFolderId;
            SIE.Web.FMS.FileManages.CommonFunctions.OperateFolder("新建文件夹".t(), oldName, function (name, win) {
                if (name == oldName) {
                    win.close();
                }
                else {
                    SIE.invokeDataQuery({
                        method: 'EditNewFolder',
                        params: [folderId, name],
                        action: 'queryer',
                        type: 'SIE.Web.FMS.FileManageDataQueryer',
                        token: view.getToken(),
                        success: function (res) {
                            //刷新数据
                            SIE.Web.FMS.FileManages.CommonFunctions.SetGridStore(curfolderId);
                            //给树增加节点
                            SIE.Web.FMS.FileManages.CommonFunctions.EditTreeNode(folderId, name);
                            win.close();
                        }
                    });
                }
            })
        },
        /**
         * 删除文件夹
         * @param {any} folderId
         */
        DeleteFolder: function (folderId) {
            var gridControl = Ext.getCmp("fileManage-id");
            if (gridControl) {
                var view = gridControl.SieView;
                var curfolderId = view.CurFolderId;
                SIE.Msg.askQuestion("确定要删除文件夹?".t(), function () {
                    SIE.invokeDataQuery({
                        method: 'DeleteFolder',
                        params: [folderId],
                        action: 'queryer',
                        type: 'SIE.Web.FMS.FileManageDataQueryer',
                        token: view.getToken(),
                        success: function (res) {
                            //刷新数据
                            SIE.Web.FMS.FileManages.CommonFunctions.SetGridStore(curfolderId);
                            //给树增加节点
                            SIE.Web.FMS.FileManages.CommonFunctions.EditTreeNode(folderId, "", true);
                        }
                    });
                });
            }
        },
        /**
         * 删除文件
         * @param {any} fileId
         */
        DeleteFile: function (fileId) {
            var gridControl = Ext.getCmp("fileManage-id");
            if (gridControl) {
                var view = gridControl.SieView;
                var curfolderId = view.CurFolderId;
                SIE.Msg.askQuestion("确定要删除该文件?".t(), function () {
                    SIE.invokeDataQuery({
                        method: 'DeleteFile',
                        params: [fileId],
                        action: 'queryer',
                        type: 'SIE.Web.FMS.FileManageDataQueryer',
                        token: view.getToken(),
                        callback: function (res) {
                            //刷新数据
                            SIE.Web.FMS.FileManages.CommonFunctions.SetGridStore(curfolderId);
                        }
                    });
                });
            }
        },
        /**
         * 批量删除文件和文件夹        
         */
        DeleteFoldersAndFiles: function () {
            var me = this;
            var gridControl = Ext.getCmp("fileManage-id");
            if (gridControl) {
                var items = me.GetGridSelected(gridControl);
                if (items.any(function (p) { return p.data.HasChild }))
                    SIE.Msg.showError("文件夹下有内容，不允许删除！".t());
                else {
                    //if (items.any(function (p) { return p.data.IsFile == true && p.data.FileState != 0 }))
                    //    SIE.Msg.showError("文件不是草稿状态，不允许删除！".t());
                    var fileIds = items.where(function (p) {
                        return p.data.IsFile == true;
                    }).select(function (p) { return p.data.FId; });
                    var folderIds = items.where(function (p) {
                        return p.data.IsFile == false;
                    }).select(function (p) { return p.data.FId; });
                    var view = gridControl.SieView;
                    var curfolderId = view.CurFolderId;
                    SIE.Msg.askQuestion("确定要删除选中的文件或文件夹?".t(), function () {
                        SIE.invokeDataQuery({
                            method: 'DeleteFoldersAndFiles',
                            params: [folderIds, fileIds],
                            action: 'queryer',
                            type: 'SIE.Web.FMS.FileManageDataQueryer',
                            token: view.getToken(),
                            success: function (res) {
                                var data = res.Result;
                                if (data == "") {
                                    //刷新数据
                                    SIE.Web.FMS.FileManages.CommonFunctions.SetGridStore(curfolderId);
                                    SIE.Web.FMS.FileManages.CommonFunctions.EditTreeNode(curfolderId, "", true, folderIds);
                                }
                                else {
                                    SIE.Msg.showError(data);
                                }
                            }
                        });
                    });
                }
            }
        },
        /**
         * 批量作废文件                  
         */
        ScarpFiles: function () {
            var me = this;
            var gridControl = Ext.getCmp("fileManage-id");
            if (gridControl) {
                var items = me.GetGridSelected(gridControl);
                var fileIds = items.select(function (p) { return p.data.FId; });
                var view = gridControl.SieView;
                var curfolderId = view.CurFolderId;
                SIE.Msg.askQuestion("确定要作废选中的文件？确认后将启动作废文件流程。".t(), function () {
                    SIE.invokeDataQuery({
                        method: 'ScarpFiles',
                        params: [fileIds, window.location.href],
                        action: 'queryer',
                        type: 'SIE.Web.FMS.FileManageDataQueryer',
                        token: view.getToken(),
                        success: function (res) {
                            if (res.Result == "")
                                SIE.Msg.showInstantMessage("操作成功！".t());
                            else
                                SIE.Msg.showError(res.Result);
                            //刷新数据
                            SIE.Web.FMS.FileManages.CommonFunctions.SetGridStore(curfolderId);
                        }
                    });
                });
            }
        },
        /**启动审批流 */
        StartFlow: function () {
            var me = this;
            var gridControl = Ext.getCmp("fileManage-id");
            if (gridControl) {
                var view = gridControl.SieView;
                var items = me.GetGridSelected(gridControl);
                if (items.length > 0) {
                    if (items.any(function (p) { return p.data.FileState != 0 && p.data.FileState != 2 }))
                        SIE.Msg.showError("只能选择草稿或修订状态的文件！".t());
                    SIE.Msg.askQuestion('确定启动审批流程？'.t(), function () {
                        var fileIds = items.where(function (p) {
                            return p.data.IsFile == true;
                        }).select(function (p) { return p.data.FId; });
                        SIE.invokeDataQuery({
                            method: 'StartFlow',
                            params: [fileIds, window.location.href],
                            action: 'queryer',
                            type: 'SIE.Web.FMS.FileManageDataQueryer',
                            token: view.getToken(),
                            callback: function (res) {
                                if (res.Success)
                                    SIE.Msg.showInstantMessage("操作成功！".t());
                                //刷新数据
                                SIE.Web.FMS.FileManages.CommonFunctions.SetGridStore(view.CurFolderId);
                            }
                        });
                    });
                } else {
                    SIE.Msg.showWarning('请选择文件！'.t());
                }
            }
        },
        /**审核文件 */
        AuditFiles: function () {
            var me = this;
            var gridControl = Ext.getCmp("fileManage-id");
            if (gridControl) {
                var items = me.GetGridSelected(gridControl);
                var fileIds = items.select(function (p) { return p.data.FId; });
                var view = gridControl.SieView;
                var curfolderId = view.CurFolderId;
                var win = SIE.Window.show({
                    title: "审核".t(),
                    width: 400,
                    height: 250,
                    resizable: true,
                    items:
                    {
                        layout: {
                            type: 'vbox',
                        },
                        width: 400,
                        border: 0,
                        items: [{
                            style: 'width:350px;text-align:center;',
                            xtype: 'displayfield',
                            hideLabel: true,
                            value: '审核通过选中的文件？'.t(),
                        }, {
                            allowBlank: false,
                            labelWidth: 70,
                            align: 'right',
                            fieldLabel: '审核意见'.t(),
                            maxLength:500,
                            name: 'rejectReason',
                            xtype: 'textarea',
                            fieldStyle: 'width:270px;height:80px;',
                        }],
                    },
                    closable: false,
                    buttons: [
                        {
                            xtype: "button", text: "通过".t(), handler: function () {
                                SIE.Web.FMS.FileManages.CommonFunctions.AuditFileSubmit(fileIds, view, win);
                            }
                        },
                        {
                            xtype: "button", text: "驳回".t(), handler: function () {
                                var txtBox = this.up('window').query('[name=rejectReason]')[0];
                                if (txtBox.activeErrors) {
                                    SIE.Msg.showWarning(txtBox.activeErrors[0].t());
                                    return;
                                }
                                if (txtBox && txtBox.value != "") {
                                    SIE.Web.FMS.FileManages.CommonFunctions.AuditFileSubmit(fileIds, view, win, txtBox.value);
                                }
                            }
                        },
                        {
                            xtype: "button", text: "取消".t(), handler: function () {
                                win.close();
                            }
                        }
                    ],
                });
            }
        },
        AuditFileSubmit: function (fileIds, view, win, reason) {
            var url = window.location.href;
            SIE.invokeDataQuery({
                method: 'AuditFiles',
                params: [fileIds, reason, url],
                action: 'queryer',
                type: 'SIE.Web.FMS.FileManageDataQueryer',
                token: view.getToken(),
                callback: function (res) {
                    if (res.Success)
                        SIE.Msg.showInstantMessage("审核成功！".t());
                    //刷新数据
                    SIE.Web.FMS.FileManages.CommonFunctions.SetGridStore(view.CurFolderId);
                    win.close();
                }
            });
        },
        /**发布文件 */
        PublishFiles: function () {
            var me = this;
            var gridControl = Ext.getCmp("fileManage-id");
            if (gridControl) {
                //var items = gridControl.getStore().data.items.where(function (p) { return p.data.checkFile && p.data.IsFile });
                var items = me.GetGridSelected(gridControl).where(function (p) { return p.data.IsFile });
                if (items.length > 0) {
                    var FileStateEnum = new SIE.Enum.FMS();
                    if (items.any(function (p) { return p.data.FileState !== FileStateEnum.FileState.ToRelease && p.data.FileState !== FileStateEnum.FileState.ScrapToRelease }))
                        SIE.Msg.showError("只能选择待发布状态的文件！".t());
                    else {
                        var fileIds = items.select(function (p) { return p.data.FId; });
                        var fileCodes = "";
                        items.select(function (p) { return p.data.Code; }).forEach(function (p) {
                            fileCodes += p + "；";
                        });
                        if (fileCodes != "")
                            fileCodes = fileCodes.substring(0, fileCodes.length - 1);
                        var fileNames = "";
                        items.select(function (p) { return p.data.FileName; }).forEach(function (p) {
                            fileNames += p + "；";
                        });
                        if (fileNames != "")
                            fileNames = fileNames.substring(0, fileNames.length - 1);
                        var view = gridControl.SieView;
                        Ext.MessageBox.show({
                            msg: '正在执行发布中'.t(),
                            progressText: '...',
                            width: 300,
                            wait: {
                                interval: 200
                            }
                        });
                        SIE.invokeDataQuery({
                            method: 'GetReciveMans',
                            params: [],
                            action: 'queryer',
                            type: 'SIE.Web.FMS.FileManageDataQueryer',
                            token: view.getToken(),
                            success: function (res) {
                                if (res.Result) {
                                    //刷新数据
                                    var receiveStore = Ext.create('Ext.data.Store', {
                                        data: res.Result,
                                    });
                                    SIE.Web.FMS.FileManages.CommonFunctions.PublishFilesShowWin(fileIds, fileCodes, fileNames, receiveStore, view);
                                    Ext.MessageBox.hide();
                                }
                            }
                        });

                    }
                }
                else
                    SIE.Msg.showWarning('请选择文件！'.t());
            }
        },
        PublishFilesShowWin: function (fileIds, fileCodes, fileNames, store, view) {
            var win = SIE.Window.show({
                title: "发布".t(),
                width: 600,
                height: 300,
                resizable: true,
                items:
                {
                    id: 'publishFilePanel',
                    layout: {
                        type: 'vbox',
                    },
                    width: 600,
                    border: 0,

                    items: [{
                        readOnly: true,
                        labelWidth: 70,
                        align: 'right',
                        fieldLabel: '文件编号'.t(),
                        name: 'fileCodes',
                        xtype: 'textarea',
                        width: 570,
                        fieldStyle: 'width:500px;height:80px;',
                        value: fileCodes,
                    }, {
                        readOnly: true,
                        labelWidth: 70,
                        align: 'right',
                        width: 570,
                        fieldLabel: '文件名称'.t(),
                        name: 'fileNames',
                        xtype: 'textarea',
                        fieldStyle: 'width:500px;height:80px;',
                        value: fileNames,
                    }, {
                        allowBlank: false,
                        labelWidth: 70,
                        xtype: 'combobox',
                        name: 'fileCodes',
                        publishes: 'value',
                        name: 'receivers',
                        align: 'right',
                        fieldLabel: '发布对象'.t(),
                        displayField: 'Name',
                        anchor: '-15',
                        store: store,
                        minChars: 0,
                        queryMode: 'local',
                        multiSelect: true,
                        valueField: 'Id',
                        width: 575,

                    }, {
                        width: 580,
                        xtype: 'displayfield',
                        style: 'display:none',
                    }],

                },
                closable: false,
                buttons: [
                    {
                        xtype: "button", text: "发布".t(), handler: function () {
                            var receivers = this.up('window').query('[name=receivers]')[0];
                            if (receivers.value == null) {
                                SIE.Msg.showWarning('请选择发布对象！'.t());
                            }
                            else {
                                SIE.invokeDataQuery({
                                    method: 'PublishFiles',
                                    params: [fileIds, receivers.value],
                                    action: 'queryer',
                                    type: 'SIE.Web.FMS.FileManageDataQueryer',
                                    token: view.getToken(),
                                    success: function (res) {
                                        if (!res.Result) {
                                            //刷新数据
                                            win.close();
                                            SIE.Msg.showInstantMessage("发布成功！".t());
                                        }
                                        else
                                            SIE.Msg.showError(res.Result);
 
                                        SIE.Web.FMS.FileManages.CommonFunctions.SetGridStore(view.CurFolderId);
                                    }
                                });
                            }
                        }
                    },
                    {
                        xtype: "button", text: "取消".t(), handler: function () {
                            win.close();
                        }
                    }
                ],
            });
        },
        /**下载文件 */
        DownloadFile: function () {
            var me = this;
            var gridControl = Ext.getCmp("fileManage-id");
            if (gridControl) {
                var view = gridControl.SieView;
                var items = me.GetGridSelected(gridControl).where(function (p) { return p.data.IsFile });
                if (items.length > 0) {
                    var fileIds = items.select(function (p) { return p.data.FId; });
                    SIE.invokeDataQuery({
                        method: 'DownLoadFiles',
                        params: [fileIds],
                        action: 'queryer',
                        type: 'SIE.Web.FMS.FileManageDataQueryer',
                        token: view.getToken(),
                        success: function (res) {
                            var data = res.Result;
                            if (data == "") {
                                items.forEach(function (p) {
                                    SIE.Web.FMS.FileManages.CommonFunctions.DownLoadSubmit({
                                        Name: 'SIE.Web.Common.Attachments.Commands.FtpDownloadCommand',
                                        Token: view.getToken(),
                                        Data: SIE.data.Utils.seriaizeRequest({
                                            Data: SIE.data.Utils.seriaizeRequest({
                                                FileName: p.data.ServerFileName,
                                                FilePath: p.data.FilePath,
                                            })
                                        })
                                    });
                                });
                            }
                            else {
                                SIE.Msg.showError(data);
                            }
                        }
                    });

                }
                else
                    SIE.Msg.showWarning('请选择文件！'.t());
            }
        },
        /** 下载文件创建panel */
        DownLoadSubmit: function (params) {
            // Create form panel. It contains a basic form that we need for the file download.
            var form = Ext.create('Ext.form.Panel', {
                standardSubmit: true,
                url: "api/Command/Excute",
                method: 'POST',
            });

            // Call the submit to begin the file download.
            form.submit({
                target: '_blank', // Avoids leaving the page. 
                params: params
            });

            Ext.defer(function () {
                form.close();
            }, 100);
        },
        /**
         * 修订文件
         * */
        EditFiles: function () {
            var me = this;
            var gridControl = Ext.getCmp("fileManage-id");
            if (gridControl) {
                var items = me.GetGridSelected(gridControl);
                //if (items[0].data.FileState !== 1 && items[0].data.FileState !== 2) {
                //    SIE.Msg.showWarning('只能修订【发布】和【修订】状态的文件！'.t());
                //    return;
                //}
                var view = gridControl.SieView;
                view.CurCheckFile = items[0].data;
                var btnFile = Ext.create('Ext.form.field.FileButton', { renderTo: Ext.getBody(), hidden: true });
                btnFile.on("change", SIE.Web.FMS.FileManages.CommonFunctions.EditFileBtnChange, view);
                btnFile.fileInputEl.dom.click();
            }
        },
        /**
         * 上传修订的文件
         * @param {any} field 输入控件
         * @param {any} newValue 文件内容
         */
        EditFileBtnChange: function (field, newValue) {
            var me = this;
            var file = field.fileInputEl.dom.files.item(0);
            var fileName = file.name;
            var folderId = me.CurFolderId;
            var validateResult = SIE.Web.FMS.FileManages.CommonFunctions.validateFile(file.size, file.name);
            if (!validateResult)
                return;
            var oldName = "";
            if (me.CurCheckFile.Code == me.CurCheckFile.FileName)
                oldName = me.CurCheckFile.FileName;
            else
                oldName = me.CurCheckFile.Code + "_" + me.CurCheckFile.FileName;
            if (oldName != fileName) {
                SIE.Msg.showWarning('修订的文件，文件名必须与原文件的文件名一致！'.t());
                return;
            }
            var fileExt = fileName.substring(fileName.lastIndexOf(".")).toLowerCase();
            var fileReader = new FileReader('file://' + newValue);
            fileReader.readAsDataURL(file);
            fileReader.onload = function (e) {
                SIE.Msg.wait("提示框".t(), "正在上传,请稍等.....".t());
                var Attachment = {
                    FolderId: folderId,
                    Content: e.target.result,
                    FileSize: file.size,
                    FileExtesion: fileExt,
                    FileName: fileName
                };
                SIE.invokeDataQuery({
                    method: 'UploadEditFile',
                    params: [Attachment, me.CurCheckFile.FId],
                    action: 'queryer',
                    type: 'SIE.Web.FMS.FileManageDataQueryer',
                    token: me.getToken(),
                    success: function (res) {
                        if (res.Success) {
                            SIE.Web.FMS.FileManages.CommonFunctions.SetGridStore(folderId);
                            SIE.Msg.showInstantMessage('上传成功！'.t());
                        }
                    }
                });
            }
        },
        /**文件上传 */
        UploadFiles: function () {
            var gridControl = Ext.getCmp("fileManage-id");
            if (gridControl) {
                var view = gridControl.SieView;
                var btnFile = Ext.create('Ext.form.field.FileButton', { renderTo: Ext.getBody(), hidden: true });
                btnFile.on("change", SIE.Web.FMS.FileManages.CommonFunctions.UploadBtnChange, view);
                btnFile.fileInputEl.dom.click();
            }
        },
        /**文件上传 */
        MultiUploadFiles: function () {
            var gridControl = Ext.getCmp("fileManage-id");
            if (gridControl) {
                var view = gridControl.SieView;
                var btnFile = Ext.create('Ext.form.field.MultiFileButton', { renderTo: Ext.getBody(), hidden: true });
                btnFile.on("change", SIE.Web.FMS.FileManages.CommonFunctions.UploadBtnChange, view);
                btnFile.fileInputEl.dom.click();
            }
        },
        /**
         * 文件上传点击触发事件
         * @param {any} field
         * @param {any} newValue
         */
        UploadBtnChange: function (field, newValue) {
            var me = this;
            var Attachment = [];
            var error = false;
            var files = field.fileInputEl.dom.files;
            var folderId = me.CurFolderId;
            for (var i = 0; i < files.length; i++) {
                var file = files.item(i);
                var fileSize = file.size;
                var fileName = file.name;
                var validateResult = SIE.Web.FMS.FileManages.CommonFunctions.validateFile(fileSize, fileName);
                if (!validateResult) {
                    error = true;
                }
            }
            if (!error) {
                var fileReader = new FileReader('file://' + newValue);
                fileReader.readAsDataURL(files.item(0));
                var fileLength = 0;
                fileReader.onload = function (e) {
                    var file = files.item(fileLength);
                    var fileSize = file.size;

                    var fileName = file.name;
                    var fileExt = fileName.substring(fileName.lastIndexOf(".")).toLowerCase();
                    Attachment.push({
                        FolderId: folderId,
                        Content: e.target.result,
                        FileSize: fileSize,
                        FileExtesion: fileExt,
                        FileName: fileName
                    });
                    fileLength++;
                    if (fileLength < files.length) {
                        fileReader.readAsDataURL(files.item(fileLength));
                    }
                    if (Attachment.length == files.length) {
                        SIE.Msg.wait("提示框".t(), "正在上传,请稍等.....".t());
                        SIE.invokeDataQuery({
                            method: 'UploadFiles',
                            params: [Attachment],
                            action: 'queryer',
                            type: 'SIE.Web.FMS.FileManageDataQueryer',
                            token: me.getToken(),
                            success: function (res) {
                                var data = res.Result;
                                var newF = data.newFolderIds;
                                if (data.errMsg == undefined || data.errMsg == null || data.errMsg == "") {
                                    //刷新数据                                  
                                    SIE.Web.FMS.FileManages.CommonFunctions.SetGridStore(folderId, newF);
                                    SIE.Msg.showInstantMessage('上传成功！'.t(), "提示".t(), 2);
                                }
                                else {
                                    if (files.length > 1 && newF.length > 0) {
                                        SIE.Msg.showError(Ext.String.format('成功上传[{0}]个！失败[{1}]个！'.t() + '<br/>', data.newFolderIds.length, files.length - data.newFolderIds.length) + data.errMsg);
                                        SIE.Web.FMS.FileManages.CommonFunctions.SetGridStore(folderId, newF);
                                    }
                                    else SIE.Msg.showError(data.errMsg);
                                }
                            }
                        });
                    }
                }
            }

        },
        /**
         * 文件验证，        
         *
         * @param {string} fileSize 文件大小，字节
         * @param {string } fileName 文件名称         
         * @returns
         */
        validateFile: function (fileSize, fileName) {
            if (Ext.isEmpty(fileName)) {
                Ext.MessageBox.alert("提示".t(), "上传的文件名不能为空。".t());
                return false;
            }

            var size = fileSize / 1024;
            if (size > 20000) {
                Ext.MessageBox.alert("提示".t(), "附件不能大于20M".t());
                return false;
            }

            return true;
        },
        /**操作文件夹 */
        OperateFolder: function (title, oldName, callBack) {
            var win = SIE.Window.show({
                title: title,
                width: 300,
                height: 160,
                resizable: false,
                autoScroll: false,
                items:
                {
                    layout: {
                        type: 'hbox',
                    },
                    border: 0,
                    items: [{
                        allowBlank: false,
                        labelWidth: 80,
                        fieldLabel: '文件夹名称'.t(),
                        name: 'newFolderName',
                        msgTarget: 'under',
                        emptyText: '',
                        xtype: 'textfield',
                        style: 'margin:20px 0 0 10px;',
                        value: oldName,
                    }],
                },
                closable: false,
                buttons: [
                    {
                        xtype: "button", text: "确定".t(), handler: function () {
                            var txtBox = this.up('window').query('[name=newFolderName]')[0];
                            if (txtBox && txtBox.value != "") {
                                callBack(txtBox.value, win);
                            }
                        }
                    },
                    {
                        xtype: "button", text: "取消".t(), handler: function () {
                            win.close();
                        }
                    }
                ],
            });
        },
        /**
         * 树增加节点
         * @param {any} folderId 要增加的节点Id
         * @param {any} name 要增加的节点名称
         * @param {any} targetFolderId 目标节点
         */
        AddToTreeNode: function (folderId, name, targetFolderId) {
            var tree = Ext.getCmp("fileManageTree");
            if (tree) {
                var item = null;
                var firstNode = tree.items.items[0].node
                var node = { folderId: folderId, text: name, leaf: true, iconCls: "iconfont icon-Folder icon-blue" };
                SIE.Web.FMS.FileManages.CommonFunctions.FindTargetNode(firstNode, targetFolderId, item, node);
            }
        },
        /**
         * 递归找到目标节点        
         */
        FindTargetNode: function (firstNode, targetFolderId, item, node) {
            firstNode.childNodes.some(function (p) {
                if (p.data.folderId == targetFolderId) {
                    item = p;
                    if (item && (item.childNodes.length > 0 || item.data.leaf)) {
                        item.appendChild(node);
                    }
                    return true;
                }
                else {
                    SIE.Web.FMS.FileManages.CommonFunctions.FindTargetNode(p, targetFolderId, item, node);
                }
            });
        },
        /**
         * 修改或删除树节点的名称
         * @param {any} targetFolderId 目标
         * @param {any} name 名称
         * @param {any} node 节点
         * @param {any} isDelete 删除该节点
         * @param {any} deleteNodes 这个参数有值时删除目标节点下的所有包含在deletenodes子节点
         */
        EditTreeNode: function (targetFolderId, name, isDelete, deleteNodes, node) {
            if (node == undefined || node == null) {
                var tree = Ext.getCmp("fileManageTree");
                if (tree) {
                    node = tree.items.items[0].node;
                }
            }
            node.childNodes.some(function (p) {
                if (p.data.folderId == targetFolderId) {
                    if (isDelete) {
                        if (deleteNodes.length > 0) {
                            p.childNodes.forEach(function (f) {
                                if (deleteNodes.any(function (g) { return f.data.folderId == g }))
                                    p.removeChild(f);
                            });
                            if (p.childNodes.length == 0) {
                                p.set('leaf', true);
                                p.set('iconCls', "iconfont icon-Folder icon-blue");
                            }
                        }
                        else {
                            node.removeChild(p);
                            if (node.childNodes.length == 0) {
                                p.set('leaf', true);
                                p.set('iconCls', "iconfont icon-Folder icon-blue");
                            }
                        }
                    }
                    else {
                        p.set('text', name);
                    }
                    return true;
                }
                else {
                    SIE.Web.FMS.FileManages.CommonFunctions.EditTreeNode(targetFolderId, name, isDelete, deleteNodes, p);
                }
            });
        },
        /**设置 */
        ShowSetting: function () {
            var gridControl = Ext.getCmp("fileManage-id");
            if (gridControl) {
                var view = gridControl.SieView;
                SIE.invokeDataQuery({
                    method: 'GetFileSetting',
                    params: [],
                    action: 'queryer',
                    type: 'SIE.Web.FMS.FileManageDataQueryer',
                    token: view.getToken(),
                    success: function (res) {
                        if (res.Result) {
                            SIE.Web.FMS.FileManages.CommonFunctions.SettingView(res.Result.data.items[0]);
                        }
                    }
                });
            }
        },
        /**
         * 设置打开视图
         * @param {any} editEntity
         */
        SettingView: function (editEntity) {
            var me = this;
            var gridControl = Ext.getCmp("fileManage-id");
            if (gridControl) {
                var view = gridControl.SieView;

                SIE.AutoUI.getMeta({
                    async: false,
                    ignoreCommands: false,
                    isDetail: true,
                    ignoreQuery: true,
                    viewGroup: "DetailsView",
                    token: view.token,
                    model: "SIE.FMS.FileSetting",
                    callback: function (res) {
                        var mainBlock;
                        if (res.mainBlock)
                            mainBlock = res.mainBlock;
                        else
                            mainBlock = res;
                        var detailView = SIE.AutoUI.createDetailView(mainBlock);
                        detailView.setData(editEntity);
                        if (editEntity.data.PusherName != "")
                            editEntity.data.PusherId_Display = editEntity.data.PusherName;
                        detailView.mon(editEntity, 'propertyChanged', SIE.Web.FMS.FileManages.CommonFunctions.onEntityPropertyChanged, detailView);
                        var ui = detailView.getControl();
                        var win = SIE.Window.show({
                            title: "设置".t(),
                            width: 430,
                            height: 280,
                            items: ui,

                            buttons: [
                                {
                                    xtype: "button", text: "确定".t(), handler: function () {
                                        var cur = detailView._current.data;
                                        SIE.invokeDataQuery({
                                            method: 'SaveFileSetting',
                                            params: [cur],
                                            action: 'queryer',
                                            type: 'SIE.Web.FMS.FileManageDataQueryer',
                                            token: view.getToken(),
                                            success: function (res) {
                                                if (res.Result) {
                                                    SIE.Msg.showInstantMessage('保存成功！'.t(), "提示".t(), 2, function () { win.close(); });
                                                    me.SetAdminPermission(view);
                                                    win.close();
                                                }
                                            }
                                        });

                                    }
                                },
                                {
                                    xtype: "button", text: "取消".t(), handler: function () { win.close(); }
                                }
                            ],
                        });
                    }
                });
            }
        },
        onEntityPropertyChanged: function (e) {
            if (e.property.length > 0) {
                if (e.property == "PusherId") {
                    if (e.value == null || e.value == 0) {
                        e.entity.setAuditMans("");
                    }
                    else
                        SIE.invokeDataQuery({
                            method: 'GetAuditMans',
                            params: [e.value],
                            action: 'queryer',
                            type: 'SIE.Web.FMS.FileManageDataQueryer',
                            token: this.token,
                            success: function (res) {
                                if (res.Result) {
                                    e.entity.setAuditMans(res.Result);
                                }
                            }
                        });
                }
            }
        },

        /**
         * 历史版本查看
         * */
        LookHistoryVersion: function () {
            var me = this;
            var gridControl = Ext.getCmp("fileManage-id");
            if (gridControl) {
                var items = me.GetGridSelected(gridControl).where(function (p) { return p.data.IsFile });
                if (items.length !== 1) {
                    SIE.Msg.showWarning('请选择一个文件！'.t());
                    return;
                }
                var view = gridControl.SieView;
                SIE.invokeDataQuery({
                    method: 'GetHistoryVersion',
                    params: [items[0].data.FId],
                    action: 'queryer',
                    type: 'SIE.Web.FMS.FileManageDataQueryer',
                    token: view.getToken(),
                    success: function (res) {
                        gridControl.initStore(res.Result.gridData);
                        debugger;
                        document.getElementById("topNav").innerHTML += "<span>" + items[0].data.FileName + "的历史版本".t() + "</span>";
                        view.CurFolderId = null;
                    }
                });
            }
        },
        /**
         * 文件操作日志查看
         * */
        LookFileLog: function () {
            var me = this;
            var gridControl = Ext.getCmp("fileManage-id");
            if (gridControl) {
                var items = me.GetGridSelected(gridControl).where(function (p) { return p.data.IsFile });
                if (items.length !== 1) {
                    SIE.Msg.showWarning('请选择一个文件！'.t());
                    return;
                }
                var view = gridControl.SieView;
                SIE.invokeDataQuery({
                    method: 'GetFileLogs',
                    params: [items[0].data.FId],
                    action: 'queryer',
                    type: 'SIE.Web.FMS.FileManageDataQueryer',
                    token: view.getToken(),
                    success: function (res) {
                        var fileLogStore = Ext.create('Ext.data.Store', {
                            data: res.Result
                        });
                        me._showFileLogWin(items[0].data.FileName, fileLogStore);
                    }
                });
            }
        },
        /**
         * 文件操作日志窗口显示
         * @param {any} fileName 文件名
         * @param {any} fileLogStore 文件操作日志Store
         */
        _showFileLogWin: function (fileName, fileLogStore) {
            var win = SIE.Window.show({
                title: fileName + "历史操作记录".t(),
                width: '50%',
                height: '65%',
                items: {
                    xtype: 'grid',
                    width: '100%',
                    frame: true,
                    header: false,
                    store: fileLogStore,
                    style: 'border-width:0;',
                    iconCls: 'my-panel-no-border icon-grid',
                    columns: [
                        { text: '时间'.t(), dataIndex: 'OperationDate', flex: 1 },
                        { text: '操作'.t(), dataIndex: 'Operation', flex: 1 },
                        { text: '文件版本'.t(), dataIndex: 'Version', flex: 1 },
                        { text: '操作人'.t(), dataIndex: 'OperationBy', flex: 1 }
                    ]
                },
                buttons: []
            });
        },

        /**
         * 设置界面命令权限
         * @param {any} view 视图
         */
        SetCommandsPermission: function (view) {
            var me = this;
            SIE.invokeDataQuery({
                method: 'GetCommandsPermission',
                params: [view.CurFolderId],
                action: 'queryer',
                type: 'SIE.Web.FMS.FileManageDataQueryer',
                token: view.token,
                success: function (res) {
                    if (res.Result) {
                        me._setCommandState(view, "SIE.Web.FMS.FileManage.Commands.UploadCommand", res.Result.Upload);
                        me._setCommandState(view, "SIE.Web.FMS.FileManage.Commands.EditCommand", res.Result.Modify);
                        me._setCommandState(view, "SIE.Web.FMS.FileManage.Commands.ScarpCommand", res.Result.Scrap);
                        me._setCommandState(view, "SIE.Web.FMS.FileManage.Commands.DownLoadCommand", res.Result.Download);
                        me._setCommandState(view, "SIE.Web.FMS.FileManage.Commands.PreViewCommand", res.Result.Preview);
                        me._setCommandState(view, "SIE.Web.FMS.FileManage.Commands.PublishCommand", res.Result.Publish);
                        me._setCommandState(view, "SIE.Web.FMS.FileManage.Commands.DeleteCommand", res.Result.Delete);

                        me._setMenuState("FileMenuItem_Upload", res.Result.Upload);
                        me._setMenuState("FileMenuItem_MultiUploadFiles", res.Result.Upload);
                        me._setMenuState("FileMenuItem_Modify", res.Result.Modify);
                        me._setMenuState("FileMenuItem_Scrap", res.Result.Scrap);
                        me._setMenuState("FileMenuItem_Download", res.Result.Download);
                        me._setMenuState("FileMenuItem_Preview", res.Result.Preview);
                        me._setMenuState("FileMenuItem_Publish", res.Result.Publish);
                    }
                }
            });
        },
        /**
         * 设置命令状态(激活/不激活)
         * @param {any} view 视图
         * @param {any} commandName 命令名称
         * @param {any} canExecute 是否可执行
         */
        _funcCmdState: function () {
            var me = this;
            var gridControl = Ext.getCmp("fileManage-id");
            if (gridControl) {
                var view = gridControl.SieView;
                var cmds = view.getCommands().items;
                Ext.each(cmds, function (cmd) {
                    var btnCl = Ext.create(cmd.meta.command);
                    var btn = Ext.getCmp(cmd.config.meta.id);
                    var canExecute = btnCl.canExecute(view);
                    if (btnCl) {
                        if (canExecute)
                            btn.enable();
                        else
                            btn.disable();
                        //更新菜单按钮状态
                        me._funcMenuCmdState(cmd, canExecute);
                    }
                });
            }
        },
        /**
        * 设置菜单状态(激活/不激活)
        * @param {any} menuId 菜单ID
        * @param {any} canExecute 是否可执行
        */
        _funcMenuCmdState: function (cmd, isCanExcute) {
            var me = this;
            var text = cmd.meta.text.t();
            switch (text) {
                case "上传".t():
                    me._setMenuEnableState("FileMenuItem_Upload", isCanExcute);
                    break;
                case "批量上传".t():
                    me._setMenuEnableState("FileMenuItem_MultiUploadFiles", isCanExcute);
                    break;
                case "修订".t():
                    me._setMenuEnableState("FileMenuItem_Modify", isCanExcute);
                    break;
                case "作废".t():
                    me._setMenuEnableState("FileMenuItem_Scrap", isCanExcute);
                    break;
                case "下载".t():
                    me._setMenuEnableState("FileMenuItem_Download", isCanExcute);
                    break;
                case "预览".t():
                    me._setMenuEnableState("FileMenuItem_Preview", isCanExcute);
                    break;
                case "发布".t():
                    me._setMenuEnableState("FileMenuItem_Publish", isCanExcute);
                    break;

            }
            var gridControl = Ext.getCmp("fileManage-id");
            if (gridControl) {
                var view = gridControl.SieView;
                var cmds = view.getCommands().items;
                Ext.each(cmds, function (cmd) {
                    var btnCl = Ext.create(cmd.meta.command);
                    var btn = Ext.getCmp(cmd.config.meta.id);
                    if (btnCl) {
                        if (btnCl.canExecute(view))
                            btn.enable();
                        else
                            btn.disable();
                    }
                });
            }
        },
        /**
         * 设置有无权限命令状态(隐藏/不隐藏)
         * @param {any} view 视图
         * @param {any} commandName 命令名称
         * @param {any} canExecute 是否可执行
         */
        _setCommandState: function (view, commandName, canExecute) {
            var command = view._commands.items.find(function (p) {
                if (p.config.meta.command == commandName)
                    return true;
            });
            if (command) {
                var btn = Ext.getCmp(command.config.meta.id);
                if (btn) {
                    if (canExecute)
                        btn.show();
                    else
                        btn.hide();
                }
            }
        },
        /**
         * 设置菜单状态(激活/不激活)
         * @param {any} menuId 菜单ID
         * @param {any} canExecute 是否可执行
         */
        _setMenuEnableState: function (menuId, canExecute) {
            var fileMenuItem = Ext.getCmp(menuId);
            if (fileMenuItem) {
                if (canExecute) {
                    fileMenuItem.enable();
                } else {
                    fileMenuItem.disable();
                }
            }
        },
        /**
         * 设置菜单状态隐藏/不隐藏)
         * @param {any} menuId 菜单ID
         * @param {any} canExecute 是否可执行
         */
        _setMenuState: function (menuId, canExecute) {
            var fileMenuItem = Ext.getCmp(menuId);
            if (fileMenuItem) {
                if (canExecute) {
                    fileMenuItem.show();
                } else {
                    fileMenuItem.hide();
                }
            }
        },
        /**
         * 设置管理员和审核权限
         * @param {any} view 视图
         */
        SetAdminPermission: function (view) {
            var me = this;
            SIE.invokeDataQuery({
                method: 'GetAdminPermission',
                params: [],
                action: 'queryer',
                type: 'SIE.Web.FMS.FileManageDataQueryer',
                token: view.token,
                success: function (res) {
                    if (res.Result) {
                        me._setMenuState("FileMenuItem_Authorization", res.Result.IsAdmin);
                        me._setMenuState("FileMenuItem_Setting", res.Result.IsAdmin);
                        me._setCommandState(view, "SIE.Web.FMS.FileManage.Commands.AuditCommand", res.Result.IsAudit);
                    }
                }
            });
        },
        /**
         * 文件授权命令
         * @param {any} isReadOnly 是否只读
         */
        FileAuthorization: function (isReadOnly) {
            var me = this;
            var gridControl = Ext.getCmp("fileManage-id");
            if (gridControl) {
                var view = gridControl.SieView;
                var items = gridControl.getSelectionModel().getSelection();
                if (items.length === 1 || (view.CurFolderId == null && items.length === 0)) {
                    if (items.length === 1) {
                        var folder = items[0];
                        if (folder.data.IsFile) {
                            SIE.Msg.showWarning('当前选中的是文件，请选择一个文件夹授权！'.t());
                            return;
                        }
                        me.GetFileAuthorizationStore(view.token, folder.data.FId, folder.data.FileName, isReadOnly);
                    } else {
                        me.GetFileAuthorizationStore(view.token, null, "首页".t(), isReadOnly);
                    }
                } else {
                    SIE.Msg.showWarning('请只选择一个文件夹，进行文件授权！'.t());
                }
            }
        },
        /**
         * 获取文件授权信息
         * @param {any} token 令牌
         * @param {any} curFolderId 当前文件夹
         * @param {any} folderName 标题
         * @param {any} isReadOnly 是否只读
         */
        GetFileAuthorizationStore: function (token, curFolderId, folderName, isReadOnly) {
            var me = this;
            SIE.invokeDataQuery({
                method: 'GetUserGroupPermissions',
                params: [curFolderId],
                action: 'queryer',
                type: 'SIE.Web.FMS.FileManageDataQueryer',
                token: token,
                success: function (res) {
                    if (res.Result) {
                        var fileUserGroupStore = Ext.create('Ext.data.Store', {
                            data: res.Result
                        });
                        var userGroupPermissionStore = Ext.create('Ext.data.Store', {
                            data: res.Result,
                            listeners: {
                                update: function (store, record, operation, modifiedFieldNames, details, eOpts) {
                                    me._permissionStoreUpdate(store, record, operation, modifiedFieldNames, details, eOpts);
                                }
                            }
                        });
                        me.ShowAuthorizationWin(folderName, fileUserGroupStore, userGroupPermissionStore, curFolderId, token, isReadOnly);
                    }
                }
            });
        },
        /**
         * 权限store变更事件
         * @param {any} store store
         * @param {any} record 行记录
         * @param {any} operation 操作
         * @param {any} modifiedFieldNames 修改字段名
         * @param {any} details 明细
         * @param {any} eOpts 操作
         */
        _permissionStoreUpdate: function (store, record, operation, modifiedFieldNames, details, eOpts) {
            if (modifiedFieldNames.length <= 0)
                return
            record.data.IsModified = true;
            if (modifiedFieldNames[0] == "Upload" && record.data.Upload == true) {
                if (record.data.Modify == false) {
                    record.data.Modify = true;
                    record.modified.Modify = true;
                }
                if (record.data.Download == false) {
                    record.data.Download = true;
                    record.modified.Download = true;
                }
            }
            if ((modifiedFieldNames[0] == "Modify" && record.data.Modify == true) || (modifiedFieldNames[0] == "Upload" && record.data.Upload == true)) {
                if (record.data.Scrap == false) {
                    record.data.Scrap = true;
                    record.modified.Scrap = true;
                }
                if (record.data.Publish == false) {
                    record.data.Publish = true;
                    record.modified.Publish = true;
                }
            }
            if ((modifiedFieldNames[0] == "Scrap" && record.data.Scrap == true) || (modifiedFieldNames[0] == "Publish" && record.data.Publish == true)
                || (modifiedFieldNames[0] == "Modify" && record.data.Modify == true) || (modifiedFieldNames[0] == "Upload" && record.data.Upload == true)) {
                if (record.data.Preview == false) {
                    record.data.Preview = true;
                    record.modified.Preview = true;
                }
            }
            var author_gridControl = Ext.getCmp("fileAuthorization_grid");
            author_gridControl.setStore(store);
        },
        /**
         * 显示文件授权窗口
         * @param {any} folderName 标题
         * @param {any} fileUserGroupStore 用户组Store
         * @param {any} permissionStore 文件权限Store
         * @param {any} curFolderId 当前文件夹ID
         * @param {any} token 令牌
         * @param {any} isReadOnly 是否只读
         */
        ShowAuthorizationWin: function (folderName, fileUserGroupStore, permissionStore, curFolderId, token, isReadOnly) {
            var me = this;
            var win = SIE.Window.show({
                title: folderName + "文件夹授权".t(),
                width: '60%',
                height: '65%',
                layout: {
                    type: 'border',
                    align: 'stretch'
                },
                items: [{
                    xtype: 'panel',
                    region: 'north',
                    width: '100%',
                    items: {
                        xtype: 'combobox',
                        region: 'north',
                        width: '90%',
                        fieldLabel: '选择文件用户组'.t(),
                        store: fileUserGroupStore,
                        queryMode: 'local',
                        displayField: 'FileUserGroupName',
                        valueField: 'FileUserGroupId',
                        id: 'fileAuthorization_combobox',
                        listeners: {
                            change: function (field, newValue, oldValue, eOpts) {
                                if (newValue == null)
                                    permissionStore.clearFilter();
                                else {
                                    permissionStore.clearFilter();
                                    permissionStore.filterBy(function (p) { return p.data.FileUserGroupId == newValue });
                                }
                            }
                        }
                    }
                }, {
                    xtype: 'grid',
                    region: 'center',
                    width: '100%',
                    frame: true,
                    header: false,
                    id: 'fileAuthorization_grid',
                    style: 'border-width:0;',
                    iconCls: 'my-panel-no-border icon-grid',
                    store: permissionStore,
                    columns: [
                        { text: '用户组'.t(), dataIndex: 'FileUserGroupName', flex: 1 },
                        { text: '上传'.t(), xtype: 'checkcolumn', dataIndex: 'Upload', disabled: isReadOnly },
                        { text: '修订'.t(), xtype: 'checkcolumn', dataIndex: 'Modify', disabled: isReadOnly },
                        { text: '作废'.t(), xtype: 'checkcolumn', dataIndex: 'Scrap', disabled: isReadOnly },
                        { text: '下载'.t(), xtype: 'checkcolumn', dataIndex: 'Download', disabled: isReadOnly },
                        { text: '预览'.t(), xtype: 'checkcolumn', dataIndex: 'Preview', disabled: isReadOnly },
                        { text: '发布'.t(), xtype: 'checkcolumn', dataIndex: 'Publish', disabled: isReadOnly },
                        { text: '删除'.t(), xtype: 'checkcolumn', dataIndex: 'Delete', disabled: isReadOnly }
                    ]
                }],
                buttons: [{
                    xtype: "button",
                    text: "确定".t(),
                    handler: function () {
                        if (!isReadOnly)
                            me._permissionSubmit(win, permissionStore.getData().items, curFolderId, token);
                        else
                            win.close();
                    }
                }, {
                    xtype: "button",
                    text: "取消".t(),
                    handler: function () { win.close(); }
                }]
            });
        },
        /**
         * 权限提交保存
         * @param {any} win 权限窗口
         * @param {any} permissions 权限数据
         * @param {any} curFolderId 当前文件夹ID
         * @param {any} token 令牌
         */
        _permissionSubmit: function (win, permissions, curFolderId, token) {
            var datas = [];
            permissions.forEach(function (p) {
                if (p.data.IsModified)
                    datas.push(p.data);
            });
            SIE.invokeDataQuery({
                method: 'PermissionSubmit',
                params: [curFolderId, datas],
                action: 'queryer',
                type: 'SIE.Web.FMS.FileManageDataQueryer',
                token: token,
                success: function (res) {
                    if (res.Success) {
                        win.close();
                        SIE.Msg.showInstantMessage("保存成功！".t());
                    }
                }
            });
        }
    }
});