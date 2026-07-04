Ext.define('SIE.Web.EMS.AssetTransfers.UploadButtonBehavior', {
    /**
     * view生命周期函数--view聚合后
     * @param {*} view 生成的view
     */
    onViewReady: function (view) {
        var cmdDelName = "SIE.Web.Common.Attachments.Commands.DeleteAttachmentCommand";
        var cmdDownloadName = "SIE.Web.Common.Attachments.Commands.DownloadCommand";
       
        var cmdDelNameBtn = view.getCmdControl(cmdDelName);
        if (cmdDelNameBtn) {
            cmdDelNameBtn.setHidden(true);
            view._commands.removeAtKey(cmdDelNameBtn);
        }
        var cmdDownloadBtn = view.getCmdControl(cmdDownloadName);
        if (cmdDownloadBtn) {
            cmdDownloadBtn.setHidden(true);
            view._commands.removeAtKey(cmdDownloadBtn);
        }
    }
});