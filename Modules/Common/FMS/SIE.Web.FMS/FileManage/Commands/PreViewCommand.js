SIE.defineCommand('SIE.Web.FMS.FileManage.Commands.PreViewCommand', {
    extend: 'SIE.cmd.Command',
    meta: { text: "预览", group: "edit", iconCls: "icon-PageSearch icon-blue" },
    canExecute: function (view) {
        var gridControl = Ext.getCmp("fileManage-id");
        if (!gridControl) return false;
        var items = gridControl.getSelectionModel().getSelection();
        if (items.length !== 1) return false;
        if (!items[0].data.IsFile) return false;
        return true;
    },
    execute: function (view) {
        SIE.Web.FMS.FileManages.CommonFunctions.DownloadFile();
    },
    PreViewShowWin: function (fileName, FilePath) {
        var win = SIE.Window.show({
            title: "预览".t(),
            ayout:"fit",
            minWidth: 1000,
            height: 600,
            closeAction: "hide",
            plain: true,     
            resizable: true,
            items:[{
                title: '弹出的窗口'.t(),
                header: false,
                html: "<iframe src='https://view.officeapps.live.com/op/view.aspx?src='" + FilePath+" ' width='100%' height='100%' frameborder='1'></iframe >",
               // html: '<iframe style="border-top-width: 0px; border-left-width: 0px; border-bottom-width: 0px; width: 728px; height: 455px; border-right-width: 0px" src=http://www.baidu.com frameborder="0" width="100%" scrolling="no" height="100%"></iframe>',
                border: false
            }],
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
                                    if (res.Result == "")
                                        SIE.Msg.showInstantMessage("发布成功！".t());
                                    else
                                        SIE.Msg.showError(res.Result);
                                    //刷新数据
                                    win.close();
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
});