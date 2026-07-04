SIE.defineCommand('SIE.Web.EMS.Tpms.Commands.ShowScorePicture', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "查看图片", group: "edit", iconCls: "icon-PrintData icon-blue" },
    canExecute: function (view) {
        var entity = view.getCurrent();
        if (entity != null && entity.data) {
            return true;
        }
        return false;
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
        if (!this.viewMeta) {
            SIE.AutoUI.getMeta({
                async: false,
                ignoreCommands: false,
                isDetail: true,
                ignoreQuery: true,
                viewGroup: "DetailsView",
                token: this.view.token,
                module: mainView.module,
                model: "SIE.EMS.Tpms.TpmRecordDetail",
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
                        title: "评分图片".t(),
              
                        buttons: [
                            { xtype: "button", text: "确定".t(), hidden: true },
                            { xtype: "button", text: "取消".t(), hidden: true }
                        ],
                        items: ui
                    });
                },
            });
        }
    }
});