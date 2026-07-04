SIE.defineCommand('SIE.Web.EMS.Tpms.Commands.LookImageCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "查看图片", group: "ImageEdit" },
    canExecute: function (view) {
        var curr = view.getCurrent();
        if (curr && curr.getPhoto().length > 0) {
            return true;
        }
        return false;
    },
    execute: function (view, source) {
        var me = this;
        var indata = {};
        var Insp = view.getSelection()[0].getData();
        indata.Data = Ext.encode({ Insp: Insp });
        if (Insp.Photo != undefined || Insp.Photo != null) {
            var win = SIE.Window.show({
                title: '查看图片'.t(),
                width: '50%',
                height: '50%',
                items: [
                Ext.create('Ext.Img', {
                    height: '100%',
                    width: '100%',
                    src: Insp.Photo
                })
                ],
                buttons: []
            });
        }
    }
});