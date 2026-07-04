SIE.defineCommand('SIE.Web.EMS.Tpms.Commands.EditRecordDetailCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    /**
     * @override 是否执行
     * @param {} view 视图
     * @returns {} 
     */
    canExecute: function (view) {
        var entity = view.getCurrent();
        if (entity)
            return true;
        return false;
    },

    /**
     * @override 编辑中
     * @param {} entity 实体
     * @returns {} 
     */
    onEditting: function (entity) {
        if (entity) {
            this.mon(entity, 'propertyChanged', this.onEntityPropertyChanged, this);
        }
    },

    /**
     * @onEntityPropertyChanged 属性变更事件
     * @param {} e 参数
     * @returns {} 
     */
    onEntityPropertyChanged: function (e) {
        var me = this;
    },
});