SIE.defineCommand('SIE.Web.EMS.IdleArchives.Commands.AddIdleArchivesDetailCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },
    onItemCreated: function (entity) {
        var me = this;

        var entity = me.view.getCurrent();
        var parent = me.view.getParent().getData().data;
        if (entity != null && parent != null) {
            entity.setFactoryId(parent.FactoryId);
            entity.setDepartmentId(parent.DepartmentId);
            entity.setUseDepartmentId(parent.UseDepartmentId);
            entity.setTypeCategory(parent.TypeCategory);
            entity.setIsAsset(parent.IsAsset);
            entity.setIdleArchiveType(parent.IdleArchiveType);
        }
        me.view.mon(entity, 'propertyChanged', me.onEntityPropertyChanged, me.view);
    },
    /**
     * 属性变更事件
     * @param {any} e
     */
    onEntityPropertyChanged: function (e) {
        var me = this;
        var entity = me.getCurrent();
        var parent = me.getParent().getData().data;
        if ((e.property === 'FixtureEncodeId_Display' || e.property === "FixtureQualityState") && e.value !== null) {


        }
    }
});