SIE.defineCommand('SIE.Web.EMS.Equipments.RunngingStates.Commands.ManualSyncEquipRunningStateRecordCommand', {
    meta: { text: "手动同步设备运行状态", group: "edit", iconCls: "icon-Play icon-blue" },

    /**
     * @override 是否可执行
     * @param {} view 
     * @returns {} 
     */
    canExecute: function (view) {
        var current = view.getCurrent();
        return true;
    },

    /**
    * @override 执行
    * @returns {}
    */
    execute: function (listview, source) {
        SIE.invokeDataQuery({
            method: 'SyncEquipRunningStateRecord',
            params: [],
            action: 'queryer',
            type: 'SIE.Web.EMS.Equipments.RunngingStates.DataQuery.EquipRunningStateRecordDataQueryer',
            token: this.view.token,
            success: function (res) {
                if (res.Success) {
                    SIE.Msg.showMessage(Ext.String.format("已成功同步{0}条数据，重新查询可以查看数据!".L10N(),res.Result));
                } else {
                    SIE.Msg.showMessage("同步失败！".t());
                }
            }
        })

    },

    /**
    * override 重写保存后提示信息
    * @param {type} view
    * @param {type} res
    */
    onSavedMsg: function (view, res) {
        SIE.Msg.showInstantMessage('同步成功，可以重新查询！'.t());
    }

});