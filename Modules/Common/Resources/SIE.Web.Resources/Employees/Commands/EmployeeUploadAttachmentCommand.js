//UploadAttachmentCommand
SIE.defineCommand('SIE.Web.Resources.Employees.Commands.EmployeeUploadAttachmentCommand', {
    extend: 'SIE.Web.Common.Attachments.Commands.UploadAttachmentCommand',
    meta: { text: "批量上传签名", group: "edit", iconCls: "icon-Upload icon-green" },
    canExecute: function (view) {
        return true;
    },
    validateFile: function (fileSize, fileName, file, entity) {
        var result = this.callParent(arguments);
        if (!result) {
            return false;
        } else {
            //var appHisId = entity.AppHisId;
            //var zipName = appHisId.toString() + ".zip";
            var fileNameArr = fileName.split('.')
            if (fileNameArr[fileNameArr.length-1] != "zip") {
                Ext.MessageBox.alert("提示", "上传的文件必须是.zip的压缩文件".t());
                return false;
            }
            return true;
        }
    },
    buttonChange: function (field,newValue) {
        var me = this;
        var files = field.fileInputEl.dom.files;
        var filesLen = files.length;
        var preUpFiles = [];
        var preUpFileLen = 0;
        console.log("view",this.view.getCurrent());
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
            preUpFile.File = file;
            preUpFiles.push(preUpFile);
        }
        if (!batValidateSucess) {
            return;
        }
        preUpFileLen = preUpFiles.length;

        const readFileAsync = function (file) {
            return new Promise(function (resolve) {
                var reader = new FileReader();
                reader.onload = function (evt) { resolve(evt.target.result) };
                reader.readAsDataURL(file);
            })
        };

        const batchReadFiles = function (files) {

            const readFileAsync = file => new Promise(resolve => {
                const reader = new FileReader()
                reader.onload = evt => resolve(evt.target.result)
                reader.readAsDataURL(file.File)
            })

            var postDatas = [];
            for (let i = 0; i < files.length; i++) {
                readFileAsync(files[i]).then(function (f) {
                    //减少网络请求，一次请求多笔数据
                    //兼容现有程序接口结构，因为使用地方多
                    var p = preUpFiles[i];

                    delete p.File;
                    var search = 'base64,';
                    p.Content = f.substr(f.indexOf(search) + search.length, f.length);
                    var postData = { Attachment: p};
                    postDatas.push(postData);

                    if (postDatas.length == files.length)
                        postFiles(postDatas);
                });
            }
        };

        const postFiles = function (postDatas) {
            if (me.view) {
                SIE.Msg.wait("提示框".t(), "正在上传,请稍等.....".t());
                me.view.execute({
                    data: postDatas,
                    success: function (res) {
                        Ext.MessageBox.alert("提示".t(), "上传成功".t());
                        me.view.reloadData();
                        SIE.Msg.close();
                        me.afterSave(me.view);
                    }
                });
            }
        };
        if (filesLen > 1 && preUpFileLen > 1) {
            //SIE.Msg.askQuestion(Ext.String.format('选择了{0}笔附件，验证通过了{1}笔，是否继续上传？'.t(), filesLen, preUpFileLen), function () {
            //    batchReadFiles(preUpFiles);
            //});
            Ext.MessageBox.alert("提示".t(), "一次只能上传一个文件".t());
        } else {
            batchReadFiles(preUpFiles);
        }
        
    }
});