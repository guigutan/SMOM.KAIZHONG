Ext.define('SIE.Web.Andon.Andons.Scripts.AndonManageUploadAttachmentButtonEditor', {
    extend: 'Ext.form.FieldContainer',
    alias: 'widget.andonmanageattachmentbtn',
    layout: 'vbox',
    _imageDom: null,
    items: [
        {
            xtype: 'filefield',
            columnWidth: .5,
            id: 'filefield',
            name: 'fileUpload',
            fieldLabel: '请选择文件'.t(),
            labelAlign: 'right',
            reference: 'basicFile',
            autoWidth: 'true',
            msgTarget: 'side',
            allowBlank: false,
            buttonText: '浏览'.t(),
            listeners: {
                change: function (field, newValue, oldValue) {
                    if (!field.fileInputEl.dom) {
                        Ext.MessageBox.alert("提示".t(), "请选择正确的文件格式文件上传！请勿重复连续点击上传文件！请退出重新修改！".t());
                        return;
                    }
                    var file = field.fileInputEl.dom.files.item(0);
                    if (!file)
                        return;
                    var value = file;
                    var fileName = file.name.substring(file.name.lastIndexOf(".") + 1).toLowerCase();

                    var fileReader = new FileReader('file://' + newValue);
                    fileReader.readAsDataURL(file);
                    fileReader.onload = function (e) {
                        field.up().up().SIEView.FileContent = e.target.result;
                        field.up().up().SIEView.FileName = value.name;
                        field.up().up().SIEView.FileExtesion = fileName;
                    }
                }
            }
        }
    ],
});

