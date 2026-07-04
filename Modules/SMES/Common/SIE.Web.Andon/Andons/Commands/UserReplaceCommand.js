SIE.defineCommand('SIE.Web.Andon.Andons.Commands.UserReplaceCommand', {
    meta: { text: "责任人替换", group: "edit", iconCls: "icon-Handshake icon-blue" },
    canExecute: function (view) {
        return true;
    },

    execute: function (view) {
        var me = this;
        SIE.AutoUI.getMeta({
            model: 'SIE.Andon.Andons.ViewModels.UserReplaceViewModel',
            isDetail: true,
            ignoreQuery: true,
            async: false,
            module: 'SIE.Andon.Andons.AndonGroup,SIE.Andon',
            token: this._token,
            callback: function (res) {
                var mainBlock;
                if (res.mainBlock)
                    mainBlock = res.mainBlock;
                else
                    mainBlock = res;
                var detailView = SIE.AutoUI.createDetailView(mainBlock);
                var entity = new detailView._model();
                detailView._setDefaultValue(entity);
                detailView.setData(entity);
                var ui = detailView.getControl();

                var win = SIE.Window.show({
                    title: '责任人替换'.t(),
                    width: 600,
                    height: 200,
                    items: ui,
                    buttons: ['确认', '取消'],
                    callback: function (btn) {
                        if (btn == '确认') {
                            me.view.execute({
                                data: entity.data,
                                success: function (rst) {
                                    me.view.reloadData();
                                }
                            });
                        }
                    }
                });
            }
        });
    }
});
