SIE.defineCommand('SIE.Web.Dock.DockAppoints.Commands.AddDockAppointCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    monPropertyChanged: function (me, entity) {
        me.mon(entity, 'propertyChanged', SIE.Web.Dock.DockAppoints.DockAppointAction.onEntityPropertyChanged, me);
    },
    showView: function (entity) {
        var me = this;
        if (entity) {
            var model = entity.data;
            this.view.execute({
                data: model,
                isSubmmit: false,
                success: function (res) {
                    var data = res.Result;
                    entity.setNo(data.No);
                    entity.setAppointDate(data.AppointDate);
                    me.monPropertyChanged(me, entity);
                    me.addDockAppointView(entity);
                }
            }, me.view);
        }
    },
    addDockAppointView: function (entity) {
        var me = this;
        SIE.AutoUI.getMeta({
            model: 'SIE.Dock.DockAppoints.DockAppoint',
            ignoreCommands: true,
            isDetail: true,
            ignoreQuery: false,
            callback: function (res) {
                var mainBlock;
                if (res.mainBlock)
                    mainBlock = res.mainBlock;
                else
                    mainBlock = res;
                var detailView = SIE.AutoUI.createDetailView(mainBlock);
                detailView._setDefaultValue(entity);
                detailView.setData(entity);
                var ui = detailView.getControl();
                var win = SIE.Window.show({
                    title: me.getEditViewTitle(entity),
                    width: "60%",
                    height: "50%",
                    items: ui,
                    callback: function (btn) {
                        if (btn == "确定".t()) {
                            var indata = detailView.getCurrent().data;
                            if (!SIE.Web.Dock.DockAppoints.DockAppointAction.validateDockAppointData(indata, me.view))
                                return false;

                            SIE.invokeDataQuery({
                                async: false,
                                type: "SIE.Web.Dock.DockAppoints.DataQueryer.DockAppointDataQueryer",
                                method: 'SaveDockAppointData',
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