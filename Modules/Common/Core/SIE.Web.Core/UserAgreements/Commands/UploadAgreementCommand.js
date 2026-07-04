SIE.defineCommand('SIE.Web.Core.UserAgreements.Commands.UploadAgreementCommand', {
    extend: 'SIE.Web.Common.Attachments.Commands.UploadAttachmentCommand',
    meta: { text: "导入协议", group: "edit", iconCls: "icon-Import icon-blue" },
    agreementType: 0,   //协议类型
    userConfig: {
        multiple: true, //是否多选
        accept: 'application/pdf', //允许什么类型，如各种图片  'image/*'，更多可百度参考mime type
    },
    canExecute: function (view) {
        return true;
    },

    /**
      *获取文件，验证，并保存
      *
      * @param {*} field 输入控件
      * @param {*} newValue 文件内容
      * @returns
      */
    buttonChange: function (field, newValue) {
        var me = this;
        var files = field.fileInputEl.dom.files;
        var filesLen = files.length;
        var preUpFiles = [];
        var preUpFileLen = 0;
        //var pEntity = me.view.getCurrent().data;

        //batch check validate
        var batValidateSucess = true;
        for (let i = 0; i < filesLen; i++) {
            var file = files[i];
            var preUpFile = {};
            preUpFile.FileName = file.name;
            preUpFile.FileSize = file.size;
            var validateResult = me.validateFile(preUpFile.FileSize, preUpFile.FileName, file);
            if (!validateResult) {
                batValidateSucess = false;
                break;
            }
            preUpFile.FileExtesion = preUpFile.FileName.substring(preUpFile.FileName.lastIndexOf(".")).toLowerCase();
            //preUpFile.OwnerId = pEntity.Id;
            preUpFile.File = file;
            preUpFiles.push(preUpFile);
        }
        if (!batValidateSucess) {
            return;
        }
        preUpFileLen = preUpFiles.length;

        const readFileAsync = file => new Promise(resolve => {
            var reader = new FileReader();
            reader.onload = evt => resolve(evt.target.result);
            reader.readAsDataURL(file);
        })

        const batchReadFiles = async files => {
            var filelen = files.length;
            for (let i = 0; i < filelen; i++) {
                var upFile = files[i];
                upFile.Content = await readFileAsync(upFile.File);
            }
            //减少网络请求，一次请求多笔数据
            //兼容现有程序接口结构，因为使用地方多
            var postDatas = [];
            preUpFiles.forEach(p => {
                delete p.File;
                var search = 'base64,';
                p.Content = p.Content.substr(p.Content.indexOf(search) + search.length, p.Content.length);
                var postData = { Attachment: p, AgreementType: me.agreementType };
                postDatas.push(postData);
            });
            postFiles(postDatas);
        }

        const postFiles = postDatas => {
            if (me.view) {
                SIE.Msg.wait("提示框".t(), "正在上传,请稍等.....".t());
                var view = me.view;
                var indata = {};
                indata.Type = "SIE.Core.UserAgreements.UserAgreementAttachment";
                indata.Data = Ext.encode(postDatas[0]);
                var commandInfo = {
                    entityType: view.model,
                    //parentType: view.model,
                    moduleName: "用户协议管理",
                    childModuleName: "",
                    commandName: "导入协议",
                };
                SIE.invokeCommand({
                    token: view.getToken(),
                    cmd: "SIE.Web.Core.UserAgreements.Commands.UploadAgreementCommand",
                    data: indata,
                    logInfo: commandInfo,
                    callback: function (res) {
                        if (res.Success) {
                            me.view.reloadData();
                            SIE.Msg.close();
                            me.afterSave(me.view);
                        }
                        else {
                            SIE.Msg.showError(res.Message);
                        }
                    }
                });
            }
        }
        if (filesLen > 1 && preUpFileLen > 1) {
            SIE.Msg.askQuestion(Ext.String.format('选择了{0}笔附件，验证通过了{1}笔，是否继续上传？'.t(), filesLen, preUpFileLen), function () {
                batchReadFiles(preUpFiles);
            });
        }
        else {
            batchReadFiles(preUpFiles);
        }
    },

    /**
     * 保存后刷新
     * @param {any} view
     */
    afterSave: function (view) {
        this.callParent(arguments);

        var control = view.routingControl;
        control.loadData(); //刷新数据
    }
});