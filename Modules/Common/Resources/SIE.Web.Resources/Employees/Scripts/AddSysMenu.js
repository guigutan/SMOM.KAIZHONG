SIE.definePlugin('SIE.UploadImage', {
    init: function (app) {
        var me = this;
        me.addLogMenu();
    },
    /**
     * 添加上传图片菜单
     */
    addLogMenu: function () {
        var fileContent = "";
        var fileName = "";
        var menuInfo = {
            index: 998,
            text: "上传签名".t(),
            handler: function () {
                console.log("this",this.view);
                var win = SIE.Window.show({
                    title: "签名图片上传".t(),
                    //width: '50%',
                    //height: '100%',
                    closable: true,
                    width: "350px",
                    height: "320px",
                    layout: 'auto',
                    items: [
                        {
                            xtype: 'image',
                            //页面渲染前给图片控件的属性赋值
                            beforeRender: function () {
                                var me = this;
                                this.width = "99%";
                                this.height = "200px";
                                this.cls = "ux-default-img";
                            },
                            //手动给图片增加点击事件
                            listeners: {
                                el: {
                                    click: 'onClick'
                                }
                            },
                            //调用文件按钮的弹窗上传文件
                            onClick: function () {
                                //this.up().items.items[1].el.dom.childNodes[1].click();
                                if (this.up().items.items[1].fileInputEl.el !== null) {
                                    this._imageDom = this.up().items.items[1].fileInputEl.el.dom;
                                    this.up().items.items[1].fileInputEl.el.dom.click();
                                    this.up().items.items[1].fireEvent('change', this.up().items.items[1]);
                                } else {
                                    this._imageDom.click();
                                    this.up().items.items[1].fireEvent('change', this.up().items.items[1]);
                                }
                                //Ext.query('[name=filebutton-1242]')[0].click();
                                //Ext.getCmp('imageComponentFilebutton_Id').el.dom.childNodes[1].click();
                            }
                        },
                        {
                            xtype: 'filebutton',
                            hidden: true,          //隐藏按钮
                            listeners: {
                                change: function (field, newValue, oldValue) {
                                    var me = this;
                                    if (!field.fileInputEl.dom) {
                                        Ext.MessageBox.alert("提示".t(), "请选择正确的图片格式文件上传(jpg,png,gif,bmp,gif等)！请勿重复连续点击上传图片！请退出重新修改！".t());
                                        return;
                                    }
                                    var file = field.fileInputEl.dom.files.item(0);
                                    if (!file)
                                        return;
                                    value = file;
                                    fileName = file.name.substring(file.name.lastIndexOf(".") + 1).toLowerCase();
                                    if (fileName != "jpg" && fileName != "jpeg" && fileName != "png") {
                                        Ext.MessageBox.alert("提示".t(), "请选择图片格式文件上传(jpg,png等)！".t());
                                        field.fileInputEl.dom.value = "";
                                        return false;
                                    }

                                    var fileSize = file.size;
                                    var size = fileSize / 1024;
                                    if (size > 10000) {
                                        Ext.MessageBox.alert("提示".t(), "附件不能大于10M".t());
                                        field.fileInputEl.dom.value = "";
                                        return false;
                                    }
                                    var fileReader = new FileReader('file://' + newValue);
                                    fileReader.readAsDataURL(file);
                                    fileReader.onload = function (e) {
                                        var img = field.up().items.items[0];
                                        //field.up().up().SIEView.getData().set(field.up().name, e.target.result);
                                        fileContent = e.target.result;
                                        img.setSrc(e.target.result);
                                        me.up().items.items[0].el.dom.style.background = "transparent";
                                    }
                                }
                            }
                        }, {
                            width: "99%",
                            border:0,
                            html: "<span style='color: red'>预览结果不符合格则时，请修改图片的长宽比</span>".t(),
                        }
                    ],
                    callback: function (btn) {
                        if (btn == "确定".t()) {
                            if (fileContent == "") {
                                Ext.MessageBox.alert("提示".t(), "请先选择图片".t());
                                return false;
                            }
                            var indata = {};
                            indata.Data = Ext.encode({
                                fileContent: fileContent,
                                fileName: fileName
                            });
                            SIE.invokeCommand({
                                cmd: "SIE.Web.Resources.Employees.Commands.PortalUploadImageCommand",
                                data: indata,
                                callback: function (res) {
                                    if (res.Success) {
                                        SIE.Msg.showMessage('上传成功！'.t());
                                        win.close();
                                    }
                                    if (!res.Success) {
                                        SIE.Msg.showMessage(res.Message);
                                    }
                                }
                            });
                        }

                    }
                });
            }
        }
        portal.addSysMenu(menuInfo);
    }
})