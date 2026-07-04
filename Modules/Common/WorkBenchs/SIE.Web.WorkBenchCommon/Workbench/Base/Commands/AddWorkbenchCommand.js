SIE.defineCommand('SIE.Web.WorkBenchCommon.Workbench.Base.Commands.AddWorkbenchCommand', {
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },

    canExecute: function (view) {
        return true;
    },
    execute: function (view, source) {
        var me = this;
        var opt = { model: 'SIE.WorkBenchCommon.Workbench.Base.WorkbenchViewModel', viewGroup: 'AddDetailsView', title: "添加工作台" };

        SIE.AutoUI.getMeta({
            model: opt.model,
            viewGroup: opt.viewGroup,
            ignoreCommands: true,
            isDetail: true,
            ignoreQuery: true,
            callback: function (res) {
                var mainBlock;
                if (res.mainBlock) mainBlock = res.mainBlock;
                else mainBlock = res;
                var ui = SIE.AutoUI.createDetailView(mainBlock);
                ui.token = view.getToken();
                //2.0 打开弹窗
                var store = new ui._model();
                ui.setData(store);
                var win = SIE.Window.show({
                    title: opt.title.t(),
                    items: ui.getControl(),
                    callback: function (btn) {
                        if (btn == "确定".t()) {
                            var entity = store.data;

                            if (!entity.LayoutCode)
                                return false;

                            var param = { code: entity.Code, name: entity.Name, desc: entity.Description, layoutCode: entity.LayoutCode, token: view.token };

                           

                            CRT.Workbench.showPageDialog({
                                tabId: 'workBenchDesign',
                                title: "添加工作台".L10N(),
                                url: '/WorkBench/AddWorkBench',
                                params: param,
                                method: 'POST',
                                view: null
                            });
                          
                        }
                    }
                });
            }
        });

    },
});