SIE.defineCommand('SIE.Web.Andon.Andons.Commands.AndonManageReassignmentCommand', {
    meta: { text: "转派", group: "edit", iconCls: "icon-Redo icon-blue" },
    canExecute: function (view) {
        var entity = view.getCurrent();
        if (entity == null)
            return false;
        if (entity.getState() != 20 && entity.getState() != 10) {
            return false;
        }
        //列表界面防止多选
        if (view.viewGroup == 'ListView' && view.getSelection().length > 1) {
            return false;
        }
        return true;
    },
    execute: function (view, source) {
        var me = this;
        var editEntity = me.view.getCurrent();

        SIE.AutoUI.getMeta({
            async: false,
            ignoreCommands: true,
            isDetail: true,
            ignoreQuery: true,
            model: me.view.model,
            viewGroup: "ReassignmentViewGroup",
            callback: function (meta) {
                meta.token = me.view.token;
                me.viewMeta = meta;

                SIE.invokeDataQuery({
                    type: 'SIE.Web.Andon.Andons.DataQuery.AndonManageDataQuery',
                    method: 'GetAndonManageInfo',
                    params: [editEntity.data.Id],
                    async: false,
                    action: 'queryer',
                    token: view.token,
                    success: function (res) {
                        if (res.Success) {
                            info = res.Result.data.items[0];
                            //回弹数据
                            editEntity.setAndonId_Display(info.getAndonId_Display());
                            editEntity.setAndonId(info.getAndonId());
                            editEntity.setHandlerId_Display(info.getHandlerId_Display());
                            editEntity.setHandlerId(info.getHandlerId());

                            var cfg = {
                                associateCmd: me,
                                viewMeta: me.viewMeta,
                                entity: editEntity,
                                editMode: me.view.editMode,
                                title: "转派".t(),
                                confirm: function (btn) {
                                    var retFlag = false;
                                    var data = {
                                        AndonManageId: editEntity.data.Id,
                                        OperateType: 2,
                                        ReassignEmployeeId: editEntity.data.HandlerId,
                                        ReassignAndonId: editEntity.data.AndonId,
                                    }
                                    view.execute({
                                        data:data,
                                        async: false,
                                        success: function (resExecute) {
                                            retFlag = true;
                                            if (view.viewGroup == 'LookUpViewGroup') {
                                                editEntity.setAndonId_Display(editEntity.getAndonId_Display());
                                                editEntity.setAndonId(editEntity.getAndonId());
                                                editEntity.setHandlerId_Display(editEntity.getHandlerId_Display());
                                                editEntity.setHandlerId(editEntity.getHandlerId());
                                                editEntity.markSaved();
                                                editEntity._OperateLogList.reload();
                                                CRT.Event.fire(view.model + '_refresh');
                                            }
                                            else {
                                                view.reloadData();
                                        }
                                            Ext.MessageBox.alert("提示".t(), "转派成功".t());
                                        },
                                        error: function (resExecute) {
                                            SIE.Msg.showError(resExecute.Message);
                                        }
                                    });
                                    return retFlag;
                                }
                            };
                            SIE.App.showDialog(cfg);
                        }
                    }
                });
            }
        });
    }
});