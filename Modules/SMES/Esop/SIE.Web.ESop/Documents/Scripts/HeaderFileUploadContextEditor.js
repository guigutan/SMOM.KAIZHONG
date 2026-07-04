Ext.define('SIE.Web.ESop.Documents.Scripts.HeaderFileUploadContextEditor', {
    extend: 'Ext.form.FieldContainer',
    alias: 'widget.HeaderFileUploadContextEditor',
    layout: 'vbox',
    width: 400,
    _imageDom: null,
    items: [
        {
            xtype: 'filefield',
            columnWidth: 200,
            id: 'headerFileUploadContextEditor',
            name: 'fileUpload',
            fieldLabel: '上传'.t(),
            labelAlign: 'right',
            reference: 'basicFile',
            autoWidth: 'false',
            msgTarget: 'side',
            allowBlank: false,
            buttonText: '浏览'.t(),
            regex: /^.*\.(xls|xlsx|csv|crb)$/i,//正则表达式，用来检验文件格式
            regexText: '请选择Excel对应格式(*.xls|*.xlsx|*.csv|*.crb)文件！'.t(),
            listeners: {
                change: function (field, newValue, oldValue) {

                    if (!field.fileInputEl.dom) {
                        Ext.MessageBox.alert("提示".t(), "请选择正确的文件格式文件上传(xlsx,xls等)！请勿重复连续点击上传文件！请退出重新修改！".t());
                        return;
                    }

                    var file = field.fileInputEl.dom.files.item(0);
                    if (!file)
                        return;
                    var value = file;
                    var fileExtesion = file.name.substring(file.name.lastIndexOf(".") + 1).toLowerCase();
                    field.up().up().SIEView.getData().dirty = true;
                    var fileReader = new FileReader('file://' + newValue);
                    fileReader.readAsDataURL(file);
                    fileReader.onload = function (e) {

                        var content = e.target.result;
                        field.up().up().SIEView.getCurrent().setFileName(value.name);
                        field.up().up().SIEView.getCurrent().setFileExtension(fileExtesion);
                        field.up().up().SIEView.getCurrent().setFileSize(value.size);

                        SIE.invokeDataQuery({
                            type: "SIE.Web.ESop.Documents.DataQuerys.DocumentsDataQuery",
                            method: "UploadFile",
                            params: [content, value.name, field.up().up().SIEView.getCurrent().getData()],
                            async: false,
                            token: field.up().up().SIEView.token,
                            callback: function (result) {
                                if (result.Success) {
                                    debugger
                                    if (result.Result != "") {
                                        var cur = field.up().up().SIEView.getCurrent();
                                        cur.setFileName(result.Result.Item1.FileName);
                                        cur.setFilePath(result.Result.Item1.FilePath);
                                        cur.setMd5(result.Result.Item1.Md5);
                                        cur.setFileSize(result.Result.Item1.FileSize);
                                        cur.setIsProcessed(result.Result.Item1.IsProcessed);
                                        var docs = field.up().up().SIEView.getChildren()[0];
                                        docs.getData().removeAll();
                                        SIE.each(result.Result.Item2, function (item) {
                                            item.UpdateDate = new Date();
                                            item.CreateDate = new Date();
                                            
                                            docs.getData().add(item);
                                        });
                                        

                                        return false;
                                    }
                                    else {
                                        SIE.Msg.showMessage(result.Message);
                                    }
                                }
                            },
                        });
                    }
                }
            }
        }
    ],
});