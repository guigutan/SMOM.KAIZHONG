SIE.defineCommand('SIE.Web.Items.ProductModels.Commands.ProductModelImportCommand', {
    extend: 'SIE.Web.Common.Import.Commands.ImportCommandBase',
    meta: { text: "导入", group: "business", iconCls: "icon-Upload icon-blue" },
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
                    myview.execute({
                        data: {
                            BehaviorName: 'ImportData',
                            Type: myview.model,
                            SelectedParent: parent != null ? Ext.encode(parent) : null,
                            SelectedParentId: parent != null ? parent.Id : 0,
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
                        var filePath = res.Result.FilePath;
                        var url = window.location.origin + "/" + filePath;
                        window.open(url);
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
});