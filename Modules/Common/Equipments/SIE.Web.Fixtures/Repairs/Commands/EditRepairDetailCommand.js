SIE.defineCommand('SIE.Web.Fixtures.Repairs.Commands.EditRepairDetailCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    canExecute: function (view) {
        var entity = view.getCurrent();
        if (entity != null && entity.isNew()) {
            return true;
        }
        return false;
    },
    onEditting: function (entity) {
        var me = this;
        if (entity) {
            this.mon(entity, 'propertyChanged', me.onEntityPropertyChanged, me);
        }
    },
    /**
     * onEntityPropertyChanged 属性变更事件
     * @param {} e 参数
     * @returns {}
     */
    onEntityPropertyChanged: function (e) {
        var me = this;
        if (e.property.length > 0) {
            var detail = e.entity;
            var data = e.entity.data;
            me.token = this.view.token;
            if (e.property === 'FixtureAccountId') {
                detail.setFixtureStorageLocationId(null);
                detail.setFixtureStorageLocationId_Display("");
                detail.setLocationName("");
                detail.setQty(1);
            }

            if (e.property === 'RepairBeforeState') {
                detail.setFixtureStorageLocationId(null);
                detail.setFixtureStorageLocationId_Display("");
                detail.setLocationName("");
                detail.setQty(1);
            } 
        }
    },
})