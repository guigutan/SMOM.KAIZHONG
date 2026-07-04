Ext.define('SIE.Web.EMS.EquipMaint.Maintains.Confirmations.Editors.FileUploadContextEditor', {
    extend: 'Ext.form.FieldContainer',
    alias: 'widget.maitainConfirmFileUploadContextEditor',
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
            regex: /^.*\.(png|jpg|bmp|gif|webp|avif|apng|jfif|jpeg|tif|pcx|tga|exif|fpx|svg|psd|cdr|pcd|dxf|ufo|eps|ai|raw|wmf)$/i,//正则表达式，用来检验文件格式
            regexText: '请选择正确的图片文件！'.t(),
            reset: function () {
                var me = this,
                    clear = me.clearOnSubmit;
                if (me.rendered) {
                    me.button.reset(clear);
                    me.fileInputEl = me.button.fileInputEl;
                    me.fileInputEl.set({
                        accept: 'image/*'
                    });
                    if (clear) {
                        me.inputEl.dom.value = '';
                    }
                    me.callParent();
                }
            },
            listeners: {
                afterrender: function (cmp) {
                    cmp.fileInputEl.set({
                        accept: 'image/*' // or w/e type
                    });
                },
                change: function (field, newValue, oldValue) {
                    if (!field.fileInputEl.dom) {
                        Ext.MessageBox.alert("提示".t(), "请选择正确的图片文件！请勿重复连续点击上传文件！请退出重新修改！".t());
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