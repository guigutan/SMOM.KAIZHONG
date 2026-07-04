Ext.define('SIE.Web.EMS.Equipments.Accounts.Scripts.EquipAccountPictureEditor', {
    extend: 'Ext.form.FieldContainer',
    id: 'equipAccountPictureFieldContainer',
    alias: 'widget.equipAccountPictureEditor',
    layout: 'vbox',
    _imageDom: null,
    items: [
        {
            xtype: 'image',
            id: 'equipAccountPicture',
            beforeRender: function () {
                if (this.up().up().SIEView._parent.getCurrent() !== null)
                    this.src = this.up().up().SIEView._parent.getCurrent().getPhoto();
                else
                    this.src = "";
                this.width = 300;
                this.height = 300;
            },
            listeners: {
                el: {
                    click: 'onClick'
                }
            },
            onClick: function () {
                if (this.up().items.items[1].fileInputEl.el !== null) {
                    this._imageDom = this.up().items.items[1].fileInputEl.el.dom;
                    this.up().items.items[1].fileInputEl.el.dom.click();
                    this.up().items.items[1].fireEvent('change', this.up().items.items[1]);
                } else {
                    this._imageDom.click();
                    this.up().items.items[1].fireEvent('change', this.up().items.items[1]);
                }
            }
        },
        {
            xtype: 'filebutton',
            hidden: true,
            listeners: {
                change: function (field, newValue, oldValue) {
                    var me = this;
                    if (!field.fileInputEl.dom) {
                        Ext.MessageBox.alert("提示", "请选择正确的图片格式文件上传(jpg,png,gif,bmp,gif等)！请勿重复连续点击上传图片！请退出重新修改！".t());
                        return;
                    }
                    var file = field.fileInputEl.dom.files.item(0);
                    if (!file)
                        return;
                    value = file;
                    var fileName = file.name.substring(file.name.lastIndexOf(".") + 1).toLowerCase();
                    if (fileName != "jpg" && fileName != "jpeg" && fileName != "png" && fileName != "bmp" && fileName != "gif") {
                        Ext.MessageBox.alert("提示", "请选择图片格式文件上传(jpg,png,gif,bmp,gif等)！".t());
                        field.fileInputEl.dom.value = "";
                        return false;
                    }

                    var fileSize = file.size;
                    var size = fileSize / 1024;
                    if (size > 1000) {
                        Ext.MessageBox.alert("提示", "附件不能大于1M".t());
                        field.fileInputEl.dom.value = "";
                        return false;
                    }

                    var fileReader = new FileReader('file://' + newValue);
                    fileReader.readAsDataURL(file);
                    fileReader.onload = function (e) {
                        var img = field.up().items.items[0];
                        field.up().up().SIEView.getData().set(field.up().name, e.target.result);
                        img.setSrc(e.target.result);
                    }
                }
            }
        }
    ],
});