SIE.defineCommand('SIE.Web.EMS.InventoryPlans.Commands.SeePhotoCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: '查看图档', group: 'edit', iconCls: 'icon-Image icon-blue' },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length !== 1) {
            return false;
        }
        var p = view.getCurrent();
        if (p == null) return false;
        if (p.data.CloseRemark != "") return false;
        return p.data.PhotoFilePath.length > 0;
    },
    execute: function (view, source) {
        var me = this;
        var curr = view.getCurrent();
        var postdata = {
            FileName: curr.data.PhotoFilePath.split('/').last(),
            FilePath: curr.data.PhotoFilePath,
        };
        view.execute({
            data: postdata,
            success: function (res) {
                var file = res.Result;
                SIE.Window.show({
                    title: me.meta.text + '-' + file.FileName,
                    width: '90%',
                    height: '100%',
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
    },
});