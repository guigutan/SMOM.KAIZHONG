SIE.defineCommand('SIE.Web.Dock.DockAppoints.Commands.EditDockAppointCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length != 1) {
            return false;
        }

        var dockAppoint = view.getCurrent();
        if (dockAppoint == null) {
            return false;
        }

        var dt = new Date();
        if (dockAppoint.getIsCancelAppoint() == true || dockAppoint.getAppointStartDate() <= dt) {
            return false;
        }

        return true;
    },
    onEditting: function (editEntity) {
        editEntity.setAppointDock(editEntity.data.DockMaintainCode + ":  " + Ext.util.Format.date(editEntity.data.AppointStartDate, 'H:i') + "~" + Ext.util.Format.date(editEntity.data.AppointEndDate, 'H:i'));
        editEntity.setUseHoursDisplay(editEntity.getUseHours());
    },
    showView: function (editEntity) {
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
                detailView._setDefaultValue(editEntity);
                detailView.setData(editEntity);
                var ui = detailView.getControl();
                me.view.detailView = detailView;
                //打开前先commit()一下,不然后面点修改命令按取消会当前记录会消失。
                var win = SIE.Window.show({
                    title: me.getEditViewTitle(editEntity),
                    disableWinAutoSize: true,
                    width: '60%',
                    height: '40%',
                    items: ui,
                    editEntity: editEntity,
                    callback: function (btn) {
                        if (btn == "确定".t()) {
                            //弹窗的确认后回调                          
                            var indata = detailView.getCurrent().data;
                            if (!SIE.Web.Dock.DockAppoints.DockAppointAction.validateDockAppointData(indata, me.view))
                                return false;

                            SIE.invokeDataQuery({
                                async: false,
                                type: "SIE.Web.Dock.DockAppoints.DataQueryer.DockAppointDataQueryer",
                                method: 'UpdateDockAppointData',
                                token: me.view.token,
                                params: [indata],
                                callback: function (r) {
                                    if (!r.Success) {
                                        SIE.Msg.showError(r.Message);
                                    }
                                    else {
                                        win.close();
                                        SIE.Msg.showInstantMessage('修改成功！'.t());
                                        me.view.reloadData();
                                    }
                                }
                            });

                            return false;
                        }
                        if (btn == "取消".t()) {
                            win.close();
                        }
                    }
                });
            }
        });
    },
});