SIE.defineCommand('SIE.Web.MES.Routings.RoutingBoms.Commands.RoutingBomDetailImportCommand', {
    extend: 'SIE.Web.Common.Import.Commands.ImportCommandBase',
    meta: { text: "导入", group: "business", iconCls: "icon-Upload icon-blue" },
    canExecute: function (view) {
        //if (view.getParent() == null || view.getParent().getCurrent() == null) return false;
        return true;
    },
    // /**
    //* @override 初始化下载模板类型
    //* BehaviorName=‘Download’：下载默认类型模板，BehaviorName=“DownloadCustom”下载自定义列模板
    //* @returns  grid
    //*/
    _downloadTemplateType: function () {
        this.BehaviorName = "DownloadCustom";
    },

    /*
     * 创建导入面板--表单
     * @param {*} myview
     * @returns ，form
     * 子类可以根据具体情况覆写，创建不同的面板
     */
    creatImportFormPanel: function (myview) {
        var me = this;
        //上传按钮
        var uploadBtn = Ext.create('Ext.button.Button', {
            text: '导入'.t(),
            itemId: 'btnOK',
            iconCls: 'iconfont icon-Upload',
            formBind: true,     //only enabled once the form is valid
            disabled: true,
            handler: function () {
                var field = Ext.getCmp('filefield');
                var newValue = field.getValue();
                //获取文件对象
                var file = field.fileInputEl.dom.files.item(0);
                var fileReader = new FileReader('file://' + newValue);
                fileReader.readAsDataURL(file);
                fileReader.onload = function (e) {
                    //SIE.Msg.wait('数据正在导入中，请稍候...');
                    me._progressBar.show();
                    me._progressBar.wait({
                        interval: 100,
                        duration: 36000000,
                        text: '数据正在导入中，请稍候...'.t(),
                        increment: 10,
                        scope: this,
                        fn: function () {

                        }
                    });
                    var parent = myview.getParent() != null && myview.getParent().getCurrent() != null ? myview.getParent().getCurrent().data : null;

                    var bomImportInfo = {
                        RoutingBomId: 0, FileName: file.name, FileSize: file.size
                    };
                    myview.execute({
                        data: {
                            BehaviorName: 'ImportData',
                            Type: myview.model,
                            SelectedParent: parent != null ? Ext.encode(parent) : null,
                            SelectedParentId: parent != null ? parent.Id : 0,
                            ExtendData: Ext.encode(bomImportInfo),
                            Data: e.target.result,
                            ViewGroup: myview.viewGroup
                        },
                        success: function (res) {
                            //导入模板成功后处理数据
                            me._importExcelCallback(res, myview);
                            //SIE.Msg.hide();
                            me._progressBar.hide();
                            SIE.Msg.showMessage(res.Result.ImportMsg);
                        }
                    });
                }
                // fileReader.onload--end
            }
        });
        //下载模板按钮
        var downTempleBtn = Ext.create('Ext.button.Button', {
            text: '下载模板'.t(),
            itemId: 'templatebutton',
            iconCls: 'iconfont icon-Download',
            handler: function () {
                myview.execute({
                    data: {
                        BehaviorName: me.BehaviorName,
                        Type: myview.model
                    },
                    success: function (res) {
                        me.downloadTemplateSuccess(res);
                    }
                });
            }
        });

        var form = new Ext.form.FormPanel({
            bodyStyle: 'padding:5px 5px 0',
            frame: true,
            border: true,
            buttons: [uploadBtn, downTempleBtn],
            items: [{
                xtype: 'filefield',
                id: 'filefield',
                name: 'fileUpload',
                fieldLabel: '请选择导入文件'.t(),
                reference: 'basicFile',
                autoWidth: 'true',
                msgTarget: 'side',
                allowBlank: false,
                anchor: '100%',
                buttonText: '浏览'.t(),
                regex: /^.*\.(xls|xlsx)$/i,//正则表达式，用来检验文件格式
                regexText: '请选择Excel对应格式(*.xls|*.xlsx)文件！'.t(),
            }, {
                xtype: 'textfield',
                id: 'msgtextfield',
                fieldLabel: '导入处理的消息'.t(),
                readOnly: true,
                anchor: '100%'
            }]
        });
        return form;
    },

    ///**
    //* 下载模板-成功
    //*/
    //downloadTemplateSuccess: function (res) {
    //    var fileName = res.Result.FileName;
    //    var dataURI = res.Result.FileContent;

    //    //获取blob文件数据
    //    var blob = base64ToBlob(dataURI);
    //    if (window.navigator.msSaveOrOpenBlob) {
    //        navigator.msSaveBlob(blob, fileName);
    //    } else {
    //        var link = document.createElement('a');
    //        var body = document.querySelector('body');
    //        link.href = window.URL.createObjectURL(blob);
    //        link.download = fileName;
    //        //兼容火狐
    //        link.style.display = 'none';
    //        body.appendChild(link);
    //        link.click();
    //        body.removeChild(link);
    //        window.URL.revokeObjectURL(link.href);
    //    }
    //},

    /**
     * 下载模板-成功
     * @param {any} res
     */
    downloadTemplateSuccess: function (res) {
        var filePath = res.Result.FilePath;
        var url = window.location.origin + "/" + filePath;
        window.open(url);
    }
});