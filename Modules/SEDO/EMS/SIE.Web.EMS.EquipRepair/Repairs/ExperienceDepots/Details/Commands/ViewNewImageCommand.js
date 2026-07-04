SIE.defineCommand('SIE.Web.EMS.EquipRepair.Repairs.ExperienceDepots.Details.ViewNewImageCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: '查看图片', group: 'edit', iconCls: 'icon-Image icon-blue' },
    canExecute: function (view) {
        var curr = view.getCurrent();
        return curr && SIE.isImageExt(curr.data.FileExtesion.replace('.', '')) || false;
    },
    execute: function (view, source) {
        var me = this;
        var curr = view.getCurrent();
        var postdata = {
            FileName: curr.data.FileName,
            FilePath: curr.data.FilePath,
        };
        view.execute({
            data: postdata,
            success: function (res) {
                var file = res.Result;

                me.showImage(file);
            },
        });
    },

    showImage: function (data) {
        var me = this;
        // Create a panel for the image
        var imagePanel = Ext.create({
            xtype: 'panel',
            width: "100%",
            height: "100%",
            layout: 'fit',
            items: {
                xtype: 'container',
                items: {
                    xtype: 'box',
                    itemId: 'imageBox',
                    autoEl: {
                        tag: 'div',
                        style: 'width: 100%; height: 100%; text-align: center; overflow: auto;border:0px',
                        html: '<img id="viewImage" src="' + data.FileContent + '" style="padding:5px" />'
                    }
                }
            }
        });

        // Create a toolbar with zoom controls
        var zoomToolbar = Ext.create('Ext.toolbar.Toolbar', {
            dock: 'top',
            items: [
                {
                    xtype: 'button',
                    iconCls: 'iconfont icon-MagnifyAdd icon-blue',
                    cls: 'x-btn-default-toolbar-small enlarge-button',
                    text: '放大'.t(),
                    handler: function () {
                        var img = Ext.get('viewImage').dom;
                        img.style.width = (img.offsetWidth * 1.2) + 'px';
                        img.style.height = (img.offsetHeight * 1.2) + 'px';
                    }
                },
                {
                    xtype: 'button',
                    iconCls: 'iconfont icon-MagnifyMinus icon-blue',
                    cls: 'x-btn-default-toolbar-small enlarge-button',
                    text: '缩小'.t(),
                    handler: function () {
                        var img = Ext.get('viewImage').dom;
                        img.style.width = (img.offsetWidth / 1.2) + 'px';
                        img.style.height = (img.offsetHeight / 1.2) + 'px';
                    }
                }
            ]
        });

        SIE.Window.show({
            title: me.meta.text + '-' + data.FileName,
            width: '90%',
            height: '90%',
            closable: true,
            scrollable: true,
            layout: 'fit',
            items: [
                imagePanel // Add the image panel
            ],
            dockedItems: [
                zoomToolbar // Add the zoom toolbar as a docked item
            ],
            buttons: [
                {
                    xtype: "button",
                    text: "确定".t(),
                    handler: function () {
                        this.up('window').close();
                    }
                }
            ]
        });
    }

});