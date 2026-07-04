SIE.defineCommand('SIE.Web.ESop.Documents.Commands.DownloadCommand', {
    meta: { text: "下载", group: "edit", iconCls: "icon-Download icon-blue" },
    execute: function (listView, source) {
        var selections = listView.getSelection();
        if (!selections || selections.length <= 0) {
            SIE.Msg.showWarning('请选择一行。'.t());
            return;
        }
        if (selections[0].data.FilePath =="") {
            SIE.Msg.showWarning('请选择存在文件路径行。'.t());
            return;
        }

        listView.execute({
            data: {},
            success: function (res) {
                if (Ext.isEmpty(res.Result)) {
                    SIE.Msg.showWarning('请设置服务端文件下载路径。'.t());
                    return;
                }

                // 选择行中获取文件路径   
                var filePath = selections[0].data.FilePath;

                // 服务端返回基地址
                var rootUrl = res.Result;
                var url = rootUrl +'/'+ filePath;
                window.open(url);
            },
            error: function (res) {
                Ext.Msg.alert('提示'.t(), res.Result);
                console.log(res.Result)
            }
        });
    }
});