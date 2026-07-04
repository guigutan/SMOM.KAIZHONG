SIE.defineCommand("SIE.Web.MES.ProjectDesigns.Commands.ProjectDesignAddCommand", {
    extend: "SIE.cmd.Add",
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },
    getEditEntity: function () {
        var newEntity = Ext.create(this.view.model);
        if (this.view.isListView) {
            newEntity = this.createNewItem();
        }
        newEntity.setState('0');
        newEntity.setExamineStatus('0');
        newEntity.setBaseInfo('0');
        newEntity.setRoutingInfo('0');
        newEntity.setBomInfo('0');
        newEntity.setAttachInfo('0');
        this.onItemCreated(newEntity);
        return newEntity;
    },
})