SIE.defineCommand('SIE.Web.DataTrace.TraceMainDatas.Commands.UploadAttachmentCommand', {
    meta: { text: "文件存档", group: "edit", iconCls: "icon-Upload icon-blue" },
    extend: 'SIE.Web.DataTrace.TraceMainDatas.Commands.ViewMutiDocumenttCommand',
    userConfig: {
        multiple: true, //是否多选
        accept: '', //允许什么类型，如各种图片  'image/*'，更多可百度参考mime type
    },
    /**
     * 子类可以添加保存后的逻辑
     *
     * @param {*} listView 列表视图
     */
    buildFileParams: function () {
        var me = this;
        var preUpFile = {};
        var pEntity = me.view.getCurrent().data;
        preUpFile.FileName = "电子批[" + pEntity.No + "].pdf";
        preUpFile.OwnerId = pEntity.Id;
        return { Attachment: preUpFile, ParentEntity: JSON.stringify(pEntity) };
    },
    operatePdfFile: function (params) {
        var me = this;
        var file = me.buildFileParams();
        params.viewArgs = JSON.stringify(file);
        params.type = "SIE.DataTrace.TraceMainDatas.TraceMainDataAttachment";
        Ext.Ajax.request({
            url: '/SimpleList/Reports/UploadAttachment', // 控制器和方法的 URL
            method: 'POST', // 请求类型
            params: params,/* 请求的参数 */
            success: function (response) {
                // 处理成功响应的逻辑3
                me.view.reloadData();
                // ...
            },
            failure: function (response) {
                SIE.Msg.showInstantMessage('存档失败！'.t());
            }
        });
        Ext.MessageBox.close();
    }
});