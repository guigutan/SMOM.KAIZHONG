Ext.define('SIE.Web.EMS.Editors.EquipmentAcceptancesFileUploadEditor', {
    extend: 'Ext.form.FieldContainer',
    alias: 'widget.emsFileUploadEditor',
    _imageDom: null,
    defaults: {
        layout: '100%'
    },
    layout: {
        type: 'absolute' // 设置为绝对定位
    },
    fieldDefaults: {
        width: '486px'
    },
    items: [{
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
        //regex: /^.*\.(xls|xlsx|csv|crb)$/i,//正则表达式，用来检验文件格式
        //regexText: '请选择Excel对应格式(*.xls|*.xlsx|*.csv|*.crb)文件！',
        listeners: {
            change: function (field, newValue, oldValue) {
                if (!field.fileInputEl.dom) {
                    Ext.MessageBox.alert("提示".t(), "请选择正确的文件格式文件上传(xlsx,xls,csv,crb等)！请勿重复连续点击上传文件！请退出重新修改！".t());
                    return;
                }
                var file = field.fileInputEl.dom.files.item(0);
                if (!file)
                    return;
                var value = file;
                var fileExtesion = file.name.substring(file.name.lastIndexOf(".") + 1).toLowerCase();

                var fileReader = new FileReader('file://' + newValue);
                fileReader.readAsDataURL(file);
                fileReader.onload = function (e) {
                    field.up().up().SIEView.FileContent = e.target.result;
                    field.up().up().SIEView.getCurrent().setContent(e.target.result);
                    field.up().up().SIEView.getCurrent().setFileName(value.name);
                    field.up().up().SIEView.getCurrent().setFileExtesion(fileExtesion);
                    field.up().up().SIEView.getCurrent().setFileSize(file.size);
                }
            }
        },
        // 设置绝对定位的位置
        x: -16,
        y: 0
    }
    ],
});