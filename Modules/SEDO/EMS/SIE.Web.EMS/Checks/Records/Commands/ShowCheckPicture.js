SIE.defineCommand('SIE.Web.EMS.Checks.Records.Commands.ShowCheckPicture', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "查看图片", group: "edit", iconCls: "icon-PrintData icon-blue" },
    canExecute: function (view) {
        var entity = view.getCurrent();
        return entity != null && entity.data.ExistPhoto === true; 
    },
    execute: function (view, source) {
        var editEntity = this.getEditEntity();
        this.onEditting(editEntity);
        this.edit(editEntity);
        this.onEdited(editEntity);
        this.showView(editEntity);
    },

    /*
     *弹出子窗体 
     */
    showView: function (editEntity) {
        var me = this;
        var mainView = me.view;
        me.getPhoto(mainView.token, editEntity.getId(), function (photo) {
            editEntity.setPhoto(photo);
            SIE.AutoUI.getMeta({
                async: false,
                ignoreCommands: false,
                isDetail: true,
                ignoreQuery: true,
                viewGroup: "DetailsView",
                token: mainView.token,
                module: mainView.module,
                model: "SIE.Web.EMS.Checks.Records.ViewModels.CheckRecordProjectViewModel",
                callback: function (res) {
                    var mainBlock;
                    if (res.mainBlock)
                        mainBlock = res.mainBlock;
                    else
                        mainBlock = res;
                    var detailView = SIE.AutoUI.createDetailView(mainBlock);

                    detailView._setDefaultValue(editEntity);
                    detailView.setData(editEntity);
                    detailView.mainView = mainView;
                    var ui = detailView.getControl();
                    var win = SIE.Window.show({
                        title: "点检图片".t(),
                        width: 400, height: 400,
                        buttons: [
                            { xtype: "button", text: "确定".t(), hidden: true },
                            { xtype: "button", text: "取消".t(), hidden: true }
                        ],
                        items: ui
                    });
                },
            });
        }); 
    }, 
    getPhoto: function (token, projectId, callback) {
        SIE.invokeDataQuery({
            method: 'GetCheckProjectPhoto',
            params: [projectId],
            action: 'queryer',
            type: 'SIE.Web.EMS.Checks.Records.DataQueryers.CheckRecordDataQueryer',
            token: token,
            success: function (res) {
                callback(res.Result);
            }
        });
    }
});