Ext.define('SIE.Web.Dock.DockMapCommons', {
    statics: {

        getDockDatas: function (token, parkId, whId, state, beginDate, endDate) {
            SIE.invokeDataQuery({
                async: false,
                type: "SIE.Web.Dock.DockMap.DataQueryer.DockMapDataQueryer",
                method: 'LoadData',
                token: token,
                params: [parkId, whId, state, beginDate, endDate],
                success: function (res) {
                    var datass = res.Result;
                    var saveData = datass.events.rows;
                    SIE.Web.Dock.DockMapCommons.saveAppoint(token, saveData);
                },
                error: function (res) {
                    SIE.Msg.showError(res.Message);
                }
            });
        },

        /**
        * 修改预约数据弹窗
        * @param {any} dockId 月台Id
        * @param {any} appointId 预约信息Id 新增传0
        * @param {any} view 当前视图包含token的
        * @param callBack 回调方法
        */
        showAppiontView: function (dockId, appointId, token, callBack) {
            var me = this;
            SIE.invokeDataQuery({
                async: false,
                type: "SIE.Web.Dock.DockMap.DataQueryer.DockMapDataQueryer",
                method: 'GetDockAppoint',
                token: token,
                params: [dockId, appointId],
                callback: function (r) {
                    if (r.Result) {
                        me.showEditAppiontView(r.Result.data.items[0], token, callBack);
                    }
                }
            });
        },
        /**
       * 修改预约数据弹窗
       * @param {any} editEntity
       * @param {any} view
       */
        showEditAppiontView: function (editEntity, token, callBack) {
            var me = this;

            var viewGroup = "DetailsView";

            if (!editEntity.data.IsAllowEdit && editEntity.data.CreateBy > 0) {
                viewGroup = "ReadOnlyView";
            }
            SIE.AutoUI.getMeta({
                model: 'SIE.Dock.DockAppoints.DockAppoint',
                ignoreCommands: true,
                isDetail: true,
                ignoreQuery: false,
                viewGroup: viewGroup,
                module: 'SIE.Dock.DockMaintains.DockMaintain,SIE.Dock',
                callback: function (res) {
                    var mainBlock;
                    if (res.mainBlock)
                        mainBlock = res.mainBlock;
                    else
                        mainBlock = res;
                    var detailView = SIE.AutoUI.createDetailView(mainBlock);
                    if (editEntity.data.CreateBy > 0) {
                        editEntity.setAppointDock(editEntity.data.DockMaintainCode + ":  " + Ext.util.Format.date(editEntity.data.AppointStartDate, 'H:i') + "~" + Ext.util.Format.date(editEntity.data.AppointEndDate, 'H:i'));
                    }
                    else {
                        editEntity.setAppointDock(editEntity.data.DockMaintainId_Display);
                    }
                    detailView._setDefaultValue(editEntity);
                    detailView.setData(editEntity);
                    var ui = detailView.getControl();

                    var win = SIE.Window.show({
                        title: "月台预约".t(),
                        resizable: false,
                        width: '60%',
                        height: '40%',
                        items: ui,
                        editEntity: editEntity,
                        callback: function (btn) {
                            if (btn == "确定".t()) {
                                //弹窗的确认后回调                          
                                var indata = detailView.getCurrent().data;
                                if (!SIE.Web.Dock.DockAppoints.DockAppointAction.validateDockAppointData(indata))
                                    return false;
                                if (editEntity.data.CreateBy == 0 || editEntity.data.CreateBy == null) {
                                    SIE.invokeDataQuery({
                                        async: false,
                                        type: "SIE.Web.Dock.DockMap.DataQueryer.DockMapDataQueryer",
                                        method: 'NewCheckAppointDatas',
                                        token: token,
                                        params: [editEntity.data],
                                        callback: function (r) {
                                            if (r.Result) {
                                                if (callBack)
                                                    callBack(editEntity);
                                            }
                                        }
                                    });
                                }
                            }
                            if (btn == "取消".t()) {
                                win.close();
                            }
                        }
                    });
                }
            });
        },

        /**
         * 保存数据
         * @param {any} token 票据
         * @param {any} entitys 提交的数据
         * @param {any} callBack 点亮格子方法
         */
        saveAppoint: function (token, entitys) {
            SIE.invokeDataQuery({
                async: false,
                type: "SIE.Web.Dock.DockMap.DataQueryer.DockMapDataQueryer",
                method: 'SaveAppointDatas',
                token: token,
                params: [entitys],
                callback: function (r) {
                    if (r.Result) {
                        var data = r.Result.data;
                        if (data.ErrorMsg && data.ErrorMsg != "") {
                            SIE.Msg.showError(data.ErrorMsg);                             
                        }
                    }
                }
            });
        },
    }
});