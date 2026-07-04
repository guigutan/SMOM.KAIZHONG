SIE.defineCommand('SIE.Web.MES.PackingQC.Commands.ReportUloadCommand', {
    meta: { text: "单条蓝标报工", group: "edit", iconCls: "icon-Upload icon-blue" },
    canExecute: function (view) {
        var model = view.getCurrent();
        if (model == null)
            return false;
        else {
            if (model.data.ReportsType == 1)
                return true;
            else
                return false;
        }
        return true;
    },
    execute: function (view, source) {
        //var msg = '如果数据已上传SAP，请确认SAP数据是否已处理完毕'.L10N();
        var entity = view.getCurrent();
        var me = this;
        view.execute({
            data: entity.data,
            success: function (res) {
                if (res.Result == true) {
                    SIE.Msg.showInstantMessage('上传成功！'.L10N(), '提示', 3);
                    view.reloadData();
                }
                else {
                    SIE.Msg.showError('上传失败:' + res.Result);
                }
            },
            error: function (res) {
                SIE.Msg.showError('上传失败,' + res.Result);
            }
        })
    }
});