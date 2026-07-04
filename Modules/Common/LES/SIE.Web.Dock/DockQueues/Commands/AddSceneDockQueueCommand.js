SIE.defineCommand('SIE.Web.Dock.DockQueues.Commands.AddSceneDockQueueCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "现场取号", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    //onItemCreated: function (entity) {
    //    var me = this;
    //    if (entity) {
    //        var model = entity.data;
    //        this.view.execute({
    //            data: model,
    //            isSubmmit: false,
    //            success: function (res) {
    //                var data = res.Result;
    //                entity.setNo(data.No);
    //                entity.setTakeNoWay(data.TakeNoWay);
    //            }
    //        }, me.view);
    //    }
    //},
    showView: function (entity) {
        let me = this;
        if (entity) {
            var model = entity.data;
            this.view.execute({
                data: model,
                isSubmmit: false,
                success: function (res) {
                    var data = res.Result;
                    entity.setNo(data.No);
                    entity.setTakeNoWay(data.TakeNoWay);
                    me.addSceneDockQueueView(entity);
                }
            }, me.view); 
        }
    },
    addSceneDockQueueView: function (entity) {
        let me = this;
        SIE.AutoUI.getMeta({
            model: 'SIE.Dock.DockQueues.DockQueue',
            ignoreCommands: true,
            isDetail: true,
            ignoreQuery: false,
            callback: function (res) {
                let mainBlock;
                if (res.mainBlock)
                    mainBlock = res.mainBlock;
                else
                    mainBlock = res;
                let detailView = SIE.AutoUI.createDetailView(mainBlock);
                detailView._setDefaultValue(entity);
                detailView.setData(entity);
                let ui = detailView.getControl();
                let viewTitle = "月台排队-现场取号".t();
                if (entity.data.TakeNoWay == 1) {
                    viewTitle = "月台排队-预约取号".t();
                }
                let win = SIE.Window.show({
                    title: viewTitle.t(),
                    width: "60%",
                    height: "45%",
                    items: ui,
                    callback: function (btn) {
                        if (btn == "确定".t()) {
                            let indata = detailView.getCurrent().data;
                            SIE.invokeDataQuery({
                                async: false,
                                type: "SIE.Web.Dock.DockQueues.DataQueryer.DockQueueDataQueryer",
                                method: 'SaveDockQueueData',
                                token: me.view.token,
                                params: [indata],
                                success: function (res) {
                                    win.close();
                                    SIE.Msg.showInstantMessage('添加成功！'.t());
                                    me.view.reloadData();
                                },
                                error: function (res) {
                                    SIE.Msg.showError(res.Message);
                                }
                            });

                            return false;
                        }
                    }
                });
            },
        });
    }
});