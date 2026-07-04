SIE.defineCommand('SIE.Web.WorkBenchCommon.Workbench.Base.Commands.DesignWorkbenchCommand', {
    meta: { text: "设计", group: "edit", iconCls: "icon-Design icon-blue" },

    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length !== 1) {
            return false;
        }
        var p = view.getCurrent();
        if (p == null) { return false; }
        if (p.isNew()) { return false; }
        return true;
    },
    execute: function (view, source) {
        var me = this;
        var curr_data = view.getCurrent().data;
        var opt = { model: 'SIE.WorkBenchCommon.Workbench.Base.WorkbenchViewModel', viewGroup: 'DesignDetailsView', title: "设计工作台" };

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
                store.setCode(curr_data.Code);
                store.setName(curr_data.Name);
                store.setDescription(curr_data.Description);
                store.setLayoutCode(curr_data.LayoutCode);
                
                ui.setData(store);
                var win = SIE.Window.show({
                    title: opt.title.t(),
                    items: ui.getControl(),
                    callback: function (btn) {
                        if (btn == "确定".t()) {
                            var entity = store.data;

                            if (!entity.LayoutCode)
                                return false;

                            var param = { code: entity.Code, layoutCode: entity.LayoutCode, token: view.token };

                           

                            CRT.Workbench.showPageDialog({
                                tabId: 'workBenchDesign',
                                title: "设计工作台".L10N(),
                                url: '/WorkBench/DesignWorkBench',
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