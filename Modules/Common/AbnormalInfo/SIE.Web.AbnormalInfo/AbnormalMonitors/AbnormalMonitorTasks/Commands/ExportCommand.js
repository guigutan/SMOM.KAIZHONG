SIE.defineCommand('SIE.Web.AbnormalInfo.AbnormalMonitors.Commands.ExportCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "导出", group: "edit", iconCls: "icon-ExportData icon-blue" },
    ///**
    // * @override 是否可执行
    // * @param {any} view
    // */
    canExecute: function (view) {
        return view.getSelection().length == 1;
    }, 

    /**
     * @override 执行
     * @param {any} view
     */
    execute: function (view, source) {
        var me = this;
        var data = this.getEditEntity().getData();
        var indata = {};
        indata.Data = Ext.encode(data);
        //调用后台计算
        view.execute({
            data: indata,
            command: "SIE.Web.AbnormalInfo.AbnormalMonitors.Commands.ExportCommand",
            success: function (res) {
                if (res.Success) {
                    var result = res.Result;
                    if (result) {
                        var fileName = result.FileName;
                        var dataURI = result.FileContent;

                        //获取blob文件数据
                        var blob = base64ToBlob(dataURI);
                        if (window.navigator.msSaveOrOpenBlob) {
                            navigator.msSaveBlob(blob, fileName);
                        } else {
                            var link = document.createElement('a');
                            var body = document.querySelector('body');
                            link.href = window.URL.createObjectURL(blob);
                            link.download = fileName;
                            //兼容火狐
                            link.style.display = 'none';
                            body.appendChild(link);
                            link.click();
                            body.removeChild(link);
                            window.URL.revokeObjectURL(link.href);
                        }
                    }
                }
            },
            error: function (res) {
                Ext.Msg.alert('提示'.t(), res.Message);
            }
        });
    },

});