SIE.defineCommand('SIE.Web.Andon.Andons.Commands.AndonManageImageCommand', {
    meta: { text: "查看图片", group: "edit", iconCls: "icon-Image icon-blue" },
    canExecute: function (view) {
        var entity = view.getCurrent();
        if (entity == null)
            return false;
        if (entity.getPhotoFile().length == 0) {
            return false;
        }
        //列表界面防止多选
        if (view.viewGroup == 'ListView' && view.getSelection().length > 1) {
            return false;
        }
        return true;
    },
    execute: function (view, source) {
        var me = this;
        var curr = view.getCurrent();
        var postdata = {
            FileName: curr.data.PhotoFile.split('/').last(),
            FilePath: curr.data.PhotoFile,
        };
        view.execute({
            data: postdata,
            success: function (res) {
                var file = res.Result;
                SIE.Window.show({
                    title: me.meta.text + '-' + file.FileName,
                    width: '45%',
                    height: '70%',
                    closable: true,
                    scrollable: true,
                    layout: 'auto',
                    items: [
                        {
                            xtype: 'image',
                            scrollable: true,
                            src: file.FileContent
                        },
                    ],
                    buttons: []
                });
            },
        });
    }
});