SIE.defineCommand('SIE.Web.Resources.Employees.Commands.SignShowImageCommand', {
    extend: 'SIE.Web.Common.Attachments.Commands.ViewImageCommand',
    meta: { text: '查看图片', group: 'edit', iconCls: 'icon-Image icon-blue' },
    canExecute: function (view) {
        var curr = view.getCurrent();
        if (curr) {
            return true;
        }
        return false;
    },
    execute: function (view, source) {
        var me = this;
        var curr = view.getCurrent();
        var postdata = {
            Id: curr.data.Id,
        };
        view.execute({
            data: postdata,
            success: function (res) {
                var file = JSON.parse(res.Result);
                var win = SIE.Window.show({
                    title: me.meta.text + "-" + curr.data.PhotoName,
                    width: '50%',
                    height: '80%',
                    closable: true,
                    scrollable: true,
                    layout: "auto",
                    items: [
                        {
                            xtype: 'image',
                            scrollable: true,
                            src: file.fileContent,
                            width: "100%",
                            height: "100%",
                        },
                    ],
                    buttons: []
                });
            },
        });
        //SIE.invokeDataQuery({
        //    type: "SIE.Web.Resources.ProcessTechs.DataQuery.ProcessTechTypeQueryer",
        //    method: "GetProcessTechType",
        //    params: [processTechTypeId],
        //    async: false,
        //    token: me.view.token,
        //    callback: function (res) {
        //        if (res.Success && res.Result != null) {
        //            var algorithmMarking = res.Result.getData().items[0].getAlgorithmMarking();
        //            entity.setAlgorithmMarking(algorithmMarking);
        //        }
        //    },
        //});
    },
});