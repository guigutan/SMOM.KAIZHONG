SIE.defineCommand('SIE.Web.EMS.Equipments.Boms.Commands.DowngradeSparPartCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "降级", group: "business", iconCls: "icon-Download icon-green" },
    canExecute: function (view) {
        var account = view.getCurrent();
        if (account == null || account.data.CreateDate == null) {
            return false;
        }
        var accountList = view.getSelection();
        if (accountList == null || accountList.length > 1) {
            return false;
        }
        return true;
    },
    execute: function (view, source) {
        var me = this;
        var editEntity = this.view.getCurrent();
        //var detailView;
        if (!this.viewMeta) {
            SIE.AutoUI.getMeta({
                async: false,
                ignoreCommands: true,
                isDetail: true,
                ignoreQuery: true,
                model: this.view.model,
                viewGroup: "EquipBomDetailSelectViewGroup",
                callback: function (meta) {
                    meta.token = me.view.token;
                    me.viewMeta = meta;
                    var detailView = SIE.AutoUI.generateAggtControl(meta); 
                }
            });
        }

        var cfg = {
            associateCmd: me,
            viewMeta: me.viewMeta,
            entity: editEntity,
            editMode: this.view.editMode,
            title: "选择".t(),
            confirm: function (isNoSave) {
                if (editEntity.data.EquipBomDetailSelectId == null)
                    SIE.Msg.showMessage("目标父备件不能为空".t());
                else {
                    SIE.Msg.askQuestion(Ext.String.format('确定降级到该备件层级下?'.t(), editEntity.data.SparePartCode), function () {
                        view.execute({
                            data: editEntity.data.EquipBomDetailSelectId,
                            withIds: true,
                            selectIds: view.getSelectionIds(),
                            success: function (res) { //回调
                                view.reloadData();
                            }
                        });
                    });
                    return true;
                }
                return false;    
            }
        };
        //子视图弹框显示
        SIE.App.showDialog(cfg);
    }
});