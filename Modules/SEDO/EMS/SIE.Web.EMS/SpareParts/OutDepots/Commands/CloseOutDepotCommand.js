SIE.defineCommand('SIE.Web.EMS.SpareParts.OutDepots.Commands.CloseOutDepotCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "关单", group: "edit", iconCls: "icon-NetworkError icon-red" },
    canExecute: function (view) {
        if (view.getSelection() == null) {
            return false;
        }
        return view.getSelection().length == 1
            && (view.getSelection()[0].data.OutDepotState == 0 || view.getSelection()[0].data.OutDepotState == 2);
    },
    execute: function (view, source) {

        var me = this;
        var entity = view.getCurrent();

        SIE.AutoUI.getMeta({
            async: false,
            ignoreCommands: true,
            isDetail: true,
            ignoreQuery: true,
            model: view.model,
            viewGroup: "CloseOutDepotViewGroup",
            callback: function (meta) {

                var cfg = {
                    associateCmd: me,
                    viewMeta: meta,
                    entity: entity,
                    editMode: view.editMode,
                    title: "关单-备件出库单".t(),
                    confirm: function () {

                        if (Ext.isEmpty(entity.data.CloseReason)) {
                            SIE.Msg.showError("关单原因不能为空".t());
                            return false;
                        }
                        var indata = { Data: Ext.encode(entity.data)};

                        view.execute({
                            data: indata,
                            async: false,
                            success: function (res) { 
                                view.reloadData();
                                SIE.Msg.showMessage('关单成功'.t());
                            },
                            error: function (res) {
                                SIE.Msg.showError(res.Message);
                            }
                        });
                    }
                };
                //子视图弹框显示
                SIE.App.showDialog(cfg);
            }
        });
    }
});